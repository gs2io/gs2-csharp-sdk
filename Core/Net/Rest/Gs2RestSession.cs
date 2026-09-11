using System;
using System.Collections;
#if !UNITY_2017_1_OR_NEWER
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Core.Result;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;
using UnityEngine.Events;
#endif

namespace Gs2.Core.Net
{
    public partial class Gs2RestSession : IGs2Session 
    {
        public static string EndpointHost = "https://{service}.{region}.gen2.gs2io.com";
        public static int OpenTimeoutSec = 10;
        public static int CloseTimeoutSec = 3;

        // ---------------------------------------------------------------- Steady（専用フリート）
        //
        // フリートは 1 つの名前（SteadyEndpoint、例 https://bs-dev.ap-northeast-1.dev.gen2.gs2io.com）で受け、
        // REST は <steady>/<service>/...、WebSocket は wss://<host>/ を使う。名前はフリートのノードへ直接
        // 解決される（間に ALB は無い）ので、フリートが手放した公開 IP に当たると SYN が落ちる。
        // そのため Steady のときだけ接続段階に上限を置き、接続段階の失敗（1 バイトも送っていない）だけは
        // 同じ要求をもう 1 回だけ送る。送信後の失敗は届いたかもしれないので再送しない（非冪等要求の二重実行を作らない）。

        /// <summary>
        /// Steady の基点への接続段階（DNS / TCP dial / TLS handshake）の上限秒。
        /// ★近似である: .NET 4.7.1 の HttpClientHandler と UnityWebRequest には接続専用のタイムアウトが無く、
        /// 接続段階だけを切ることができない。そこで Steady 宛の冪等な GET / DELETE だけ、要求タイムアウト全体を
        /// この値に縮める。非冪等な POST / PUT には掛けない（GS2 の長い API を殺すし、接続段階と読み取りを
        /// 区別できないので二重実行を作る）。
        /// </summary>
        public static int SteadyConnectTimeoutSec = 5;

        private const string ServicePlaceholder = "{service}";
        private const string RegionPlaceholder = "{region}";

        // ReSharper disable once MemberCanBePrivate.Global
        public volatile State State;
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _closeSemaphore = new SemaphoreSlim(1, 1);
        private readonly object _stateLock = new object();
        private long _sessionGeneration;
        private object _sessionOpenToken;
        private bool _sessionOpenRequestClaimed;
        internal int InflightRequestCount => GetInflightRequestCount();

#if UNITY_2017_1_OR_NEWER
        private Dictionary<Gs2SessionTaskId, RestSessionRequest> _inflightRequest = new Dictionary<Gs2SessionTaskId, RestSessionRequest>();
        protected Dictionary<Gs2SessionTaskId, RestResult> _result = new Dictionary<Gs2SessionTaskId, RestResult>();
#else
        private ConcurrentDictionary<Gs2SessionTaskId, RestSessionRequest> _inflightRequest = new ConcurrentDictionary<Gs2SessionTaskId, RestSessionRequest>();
        protected ConcurrentDictionary<Gs2SessionTaskId, RestResult> _result = new ConcurrentDictionary<Gs2SessionTaskId, RestResult>();
#endif

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        public string OwnerId { get; private set; }

        private readonly bool _checkCertificateRevocation;
        public bool EnableRequestCompression { get; set; } = true;
        public bool EnableResponseDecompression { get; set; } = true;

        private string _steadyEndpoint;

        /// <summary>
        /// Steady（専用フリート）の基点（https://&lt;host&gt;）。null なら共有クラウド（URL は従来と byte 単位で同じ）。
        /// Open の前に設定する。設定すると全サービスの接続先が &lt;steady&gt;/&lt;service&gt; になり、
        /// 接続段階に上限と 1 回の再送が付く。末尾の / と空白は落として正規化する。
        /// </summary>
        public string SteadyEndpoint
        {
            get => this._steadyEndpoint;
            set => this._steadyEndpoint = NormalizeSteadyEndpoint(value);
        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true, string steadyEndpoint = null) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation, steadyEndpoint) {

        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true, string steadyEndpoint = null)
        {
#if UNITY_2017_1_OR_NEWER && (!UNITY_WEBGL || UNITY_EDITOR)
            ValidateCertificateRevocationConfiguration(checkCertificateRevocation, false);
#endif
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);
            SteadyEndpoint = steadyEndpoint;

            this._checkCertificateRevocation = checkCertificateRevocation;
            this.State = State.Idle;
        }

        /// <summary>末尾の / と空白を落とす。空（null / 空白だけ）なら null。</summary>
        public static string NormalizeSteadyEndpoint(string value)
        {
            if (value == null)
            {
                return null;
            }
            var normalized = value.Trim().TrimEnd('/');
            return normalized.Length == 0 ? null : normalized;
        }

        /// <summary>共有クラウドの接続先（従来の <see cref="EndpointHost"/> の置換）。</summary>
        internal static string SharedCloudEndpoint(string service, string regionDisplayName)
        {
            return EndpointHost
                .Replace(ServicePlaceholder, service)
                .Replace(RegionPlaceholder, regionDisplayName);
        }

        /// <summary>
        /// service の接続先（https://... まで。パスは呼び手が足す）。
        /// 優先順: サービスごとの override（endpointHost）＞ <see cref="SteadyEndpoint"/> ＞
        /// 共有クラウドの template（静的 <see cref="EndpointHost"/>）。Go の
        /// <c>Gs2RestSession.EndpointHost(service, endpointHost)</c>（core/steady.go）と同じ順。
        /// <see cref="SteadyEndpoint"/> が null なら従来の文字列と byte 単位で同じ。
        /// </summary>
        public string EndpointFor(string service, string endpointHost = null)
        {
            if (endpointHost == null && this._steadyEndpoint != null)
            {
                return this._steadyEndpoint + "/" + service;
            }
            return (endpointHost ?? EndpointHost)
                .Replace(ServicePlaceholder, service)
                .Replace(RegionPlaceholder, Region.DisplayName());
        }

        /// <summary>
        /// 生成クライアントが静的 <see cref="EndpointHost"/> の template から組んだ URL を Steady の基点宛へ書き換える。
        /// ★生成クライアント（Gs2&lt;Service&gt;RestClient）はセッションを知らずに静的 template だけから URL を組むので
        /// （引数で接続先を渡す口が無い）、送る直前にここで直す。template の {service} の位置で service 名を切り出し、
        /// &lt;steady&gt;/&lt;service&gt; + 残りのパス にする。
        /// 対象でない URL（Steady 未設定 / template に {service} が無い独自プロキシ / 形が違う）はそのまま返す
        /// ―― つまり service を特定できる形の override だけが Steady に書き換えられる。
        /// </summary>
        internal string ToSteadyUrl(string url)
        {
            var steady = this._steadyEndpoint;
            if (steady == null || url == null)
            {
                return url;
            }
            if (IsSteadyUrl(url))
            {
                // ★既に基点宛。基点の名前が共有クラウドの template と同じ形（bs.ap-northeast-1.gen2.gs2io.com など）
                // でも二重に書き換えない（<steady>/bs/<service>/... になってしまう）。
                return url;
            }

            var template = EndpointHost.Replace(RegionPlaceholder, Region.DisplayName());
            var placeholder = template.IndexOf(ServicePlaceholder, StringComparison.Ordinal);
            if (placeholder < 0)
            {
                return url;
            }
            var prefix = template.Substring(0, placeholder);
            var suffix = template.Substring(placeholder + ServicePlaceholder.Length);
            if (!url.StartsWith(prefix, StringComparison.Ordinal))
            {
                return url;
            }

            int serviceEnd;
            if (suffix.Length == 0)
            {
                // template が {service} で終わる形（https://host/{service}）。service 名は次の / まで。
                serviceEnd = url.IndexOf('/', prefix.Length);
                if (serviceEnd < 0)
                {
                    serviceEnd = url.Length;
                }
            }
            else
            {
                serviceEnd = url.IndexOf(suffix, prefix.Length, StringComparison.Ordinal);
                if (serviceEnd < 0)
                {
                    return url;
                }
            }

            var service = url.Substring(prefix.Length, serviceEnd - prefix.Length);
            if (service.Length == 0 || service.IndexOf('/') >= 0)
            {
                return url;
            }
            return steady + "/" + service + url.Substring(serviceEnd + suffix.Length);
        }

        /// <summary>要求 URL が Steady の基点宛か（接続段階の失敗で再送してよいのはこれだけ）。</summary>
        internal bool IsSteadyUrl(string url)
        {
            var steady = this._steadyEndpoint;
            if (steady == null || url == null)
            {
                return false;
            }
            return url == steady || url.StartsWith(steady + "/", StringComparison.Ordinal);
        }

        internal static void ValidateCertificateRevocationConfiguration(
            bool checkCertificateRevocation,
            bool canSafelyDisableCertificateRevocation
        )
        {
            if (!checkCertificateRevocation && !canSafelyDisableCertificateRevocation)
            {
                throw new PlatformNotSupportedException(
                    "Disabling certificate revocation checks is not supported on this platform without disabling certificate validation."
                );
            }
        }

#if UNITY_2017_1_OR_NEWER
        public virtual RestSessionRequestFactory CreateRestSessionRequestFactory(UnityEngine.Networking.CertificateHandler certificateHandler = null)
        {
            return new RestSessionRequestFactory(
                () => certificateHandler != null
                    ? new UnityRestSessionRequest(certificateHandler)
                    : new UnityRestSessionRequest(_checkCertificateRevocation),
                EnableRequestCompression,
                EnableResponseDecompression
            );
        }
#else
        public virtual RestSessionRequestFactory CreateRestSessionRequestFactory()
        {
            return new RestSessionRequestFactory(
                () => new DotNetRestSessionRequest(_checkCertificateRevocation),
                EnableRequestCompression,
                EnableResponseDecompression
            );
        }
#endif

        // Open

#if GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> OpenAsync()
#else
        public async Task<OpenResult> OpenAsync()
#endif
        {
            await TaskUtilities.WaitAsync(this._semaphore);
            long generation;
            try {
                lock (_stateLock)
                {
                    if (this.State == State.Available) {
                        return new OpenResult();
                    }

                    if (this.State != State.Idle && this.State != State.Closed) {
                        throw new InvalidOperationException("invalid state: " + this.State);
                    }

                    this._result.Clear();
                    this._inflightRequest.Clear();
                    this._sessionOpenToken = new object();
                    this._sessionOpenRequestClaimed = false;
                    this.State = State.Opening;
                    generation = ++_sessionGeneration;
                }

                try
                {
                    if (Credential is ProjectTokenGs2Credential)
                    {
                        lock (_stateLock)
                        {
                            ThrowIfOpenWasInvalidated(generation);
                            OwnerId = Credential.ClientId;
                        }
                    }
                    else
                    {
                        var result = await new RestOpenTask(
                            this,
                            CreateRestSessionRequestFactory(),
                            new LoginRequest {
                                ClientId = Credential.ClientId,
                                ClientSecret = Credential.ClientSecret,
                            },
                            this._sessionOpenToken
                        ).Invoke();

                        var projectToken = result?.AccessToken;
                        if (projectToken == null)
                        {
                            var exception = new InvalidOperationException(
                                "Login response did not contain an access token."
                            );
                            throw new Gs2.Core.Exception.UnknownException(exception.Message, exception);
                        }

                        lock (_stateLock)
                        {
                            ThrowIfOpenWasInvalidated(generation);
                            Credential.ProjectToken = projectToken;
                            OwnerId = result.OwnerId;
                        }
                    }

                    lock (_stateLock)
                    {
                        ThrowIfOpenWasInvalidated(generation);
                        this._sessionOpenToken = null;
                        this.State = State.Available;
                    }
                }
                catch
                {
                    lock (_stateLock)
                    {
                        if (_sessionGeneration == generation)
                        {
                            this.State = State.Closed;
                        }
                    }
                    throw;
                }

                return new OpenResult();
            }
            finally {
                this._semaphore.Release();
            }
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback) => OpenAsync().ToCoroutine(callback);
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback) => OpenAsync().ToCoroutine(callback);
#endif
        
        public Gs2Future<OpenResult> OpenFuture() => OpenAsync().ToGs2Future();

        // ReOpen
        
        
#if GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> ReOpenAsync()
#else
        public async Task<OpenResult> ReOpenAsync()
#endif
        {
            if (IsReOpenWaitingForTransition()) {
                var timer = Stopwatch.StartNew();
                while (IsReOpenWaitingForTransition()) {
                    if (timer.Elapsed.TotalSeconds >= OpenTimeoutSec) {
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }

                    await TaskUtilities.Yield();
                }
            }

            return await OpenAsync();
        }

        private bool IsReOpenWaitingForTransition()
        {
            lock (_stateLock)
            {
                return this.State is State.Opening or State.LoggingIn or State.CancellingTasks or State.Closing;
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReOpen(UnityAction<AsyncResult<OpenResult>> callback) => ReOpenAsync().ToCoroutine(callback);
#else
        public IEnumerator ReOpen(Action<AsyncResult<OpenResult>> callback) => ReOpenAsync().ToCoroutine(callback);
#endif
        
        // ReSharper disable once MemberCanBePrivate.Global
        public Gs2Future<OpenResult> ReOpenFuture() => ReOpenAsync().ToGs2Future();
        
        // Close
        
#if GS2_ENABLE_UNITASK
        public async UniTask CloseAsync()
#else
        public async Task CloseAsync()
#endif
        {
            long expectedGeneration;
            lock (_stateLock)
            {
                expectedGeneration = _sessionGeneration;
            }
            await TaskUtilities.WaitAsync(this._closeSemaphore);
            try
            {
                lock (_stateLock)
                {
                    if (_sessionGeneration != expectedGeneration)
                    {
                        return;
                    }
                    if (this.State == State.Closed)
                    {
                        return;
                    }

                    ++_sessionGeneration;
                    if (this.State == State.Idle)
                    {
                        this.State = State.Closed;
                        return;
                    }

                    this.State = State.CancellingTasks;
                }

                {
                    var timer = Stopwatch.StartNew();
                    while (GetInflightRequestCount() > 0) {
                        if (timer.Elapsed.TotalSeconds >= CloseTimeoutSec) {
                            RestSessionRequest[] requestsToAbort;
                            lock (_stateLock)
                            {
                                requestsToAbort = this._inflightRequest.Values.ToArray();
                                this._inflightRequest.Clear();
                            }
                            foreach (var request in requestsToAbort)
                            {
                                TryAbort(request);
                            }
                            break;
                        }

                        await TaskUtilities.Yield();
                    }
                }

                lock (_stateLock)
                {
                    this.State = State.Closing;
                    this._inflightRequest.Clear();
                    this._result.Clear();
                    this.State = State.Closed;
                }
            }
            finally
            {
                this._closeSemaphore.Release();
            }
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback) => CloseAsync().ToCoroutine(callback);
#else
        public IEnumerator Close(Action callback) => CloseAsync().ToCoroutine(callback);
#endif
        
        public Gs2Future CloseFuture() => CloseAsync().ToGs2Future();
        
        // Send
        
#if GS2_ENABLE_UNITASK
        public virtual async UniTask SendAsync(IGs2SessionRequest request)
#else
        public virtual async Task SendAsync(IGs2SessionRequest request)
#endif
        {
            if (request is not RestSessionRequest sessionRequest)
            {
                throw new ArgumentException("The request is not a REST session request.", nameof(request));
            }
            else
            {
                sessionRequest.EnableRequestCompression = this.EnableRequestCompression;
                sessionRequest.EnableResponseDecompression = this.EnableResponseDecompression;
#if !UNITY_2017_1_OR_NEWER
                if (sessionRequest is DotNetRestSessionRequest dotNetRequest)
                {
                    dotNetRequest.ConfigureCertificateRevocation(this._checkCertificateRevocation);
                }
#endif
                // ★生成クライアントは静的 EndpointHost から URL を組むので、ここで Steady の基点宛へ直す（ToSteadyUrl）。
                sessionRequest.Url = ToSteadyUrl(sessionRequest.Url);

                long generation;
                int timeoutSec;
                lock (_stateLock)
                {
                    if (this.State == State.Opening)
                    {
                        if (this._sessionOpenToken == null ||
                            this._sessionOpenRequestClaimed ||
                            !ReferenceEquals(sessionRequest.SessionOpenToken, this._sessionOpenToken))
                        {
                            throw new SessionNotOpenException("Session no longer open.");
                        }
                        this._sessionOpenRequestClaimed = true;
                    }
                    else if (this.State != State.Available)
                    {
                        throw new SessionNotOpenException("Session no longer open.");
                    }

                    generation = _sessionGeneration;
                    timeoutSec = this.State == State.Opening
                        ? OpenTimeoutSec
                        : 10;
                    this._inflightRequest[sessionRequest.TaskId] = sessionRequest;
                }

                // Steady の基点宛の要求だけ、接続段階の失敗に備える（Steady 未設定なら従来どおり 1 回送るだけ）。
                //   - 接続段階の失敗（ConnectFailed。1 バイトも送っていない）→ 同じ要求をもう 1 回だけ送る。
                //   - タイムアウト → 冪等な GET / DELETE だけもう 1 回（POST / PUT は届いたかもしれないので再送しない）。
                //   - 最初の試行の上限は、GET / DELETE だけ SteadyConnectTimeoutSec に縮める。
                // ★この時間の縮めは近似: .NET 4.7.1 の HttpClientHandler も UnityWebRequest も接続専用の
                //   タイムアウトを持たないので、POST / PUT では接続段階の固まりと読み取りの遅さを区別できない。
                //   だから POST / PUT は縮めないし、タイムアウトでは再送もしない。再送は 1 回だけ（3 回目は無い）。
                var viaSteady = IsSteadyUrl(sessionRequest.Url);
                var idempotent = sessionRequest.Method == HttpMethod.Get || sessionRequest.Method == HttpMethod.Delete;
                var firstTimeoutSec = viaSteady && idempotent && SteadyConnectTimeoutSec > 0
                    ? Math.Min(timeoutSec, SteadyConnectTimeoutSec)
                    : timeoutSec;

                RestResult result;
                try
                {
                    result = await InvokeOnceAsync(sessionRequest, firstTimeoutSec);
                    var timedOut = result == null;
                    if (viaSteady && (timedOut ||
                                      result.TransportFailure == TransportFailure.ConnectFailed ||
                                      result.TransportFailure == TransportFailure.Timeout))
                    {
                        var connectFailed = !timedOut && result.TransportFailure == TransportFailure.ConnectFailed;
                        if ((connectFailed || idempotent) && IsRetryableSessionState(generation))
                        {
                            result = await InvokeOnceAsync(sessionRequest, timeoutSec);
                            timedOut = result == null;
                        }
                    }
                    if (timedOut)
                    {
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }
                }
                catch
                {
                    lock (_stateLock)
                    {
                        RemoveInflightRequest(sessionRequest);
                    }
                    throw;
                }

                lock (_stateLock)
                {
                    if (generation != _sessionGeneration ||
                        this.State == State.CancellingTasks ||
                        this.State == State.Closing ||
                        this.State == State.Closed ||
                        this.State == State.Idle)
                    {
                        RemoveInflightRequest(sessionRequest);
                        throw new SessionNotOpenException("Session no longer open.");
                    }

                    this._result[sessionRequest.TaskId] = result;
                }
            }
        }

        public IEnumerator Send(IGs2SessionRequest request) => SendAsync(request).ToCoroutine((Action)null);

        public bool Ping()
        {
            return true;
        }

        public bool IsCanceled()
        {
            return this.State == State.CancellingTasks;
        }

        public bool IsCompleted(IGs2SessionRequest request)
        {
            lock (_stateLock)
            {
                return this._result.ContainsKey(request.TaskId);
            }
        }

        public bool IsDisconnected()
        {
            return this.State == State.Idle || this.State == State.Closing || this.State == State.Closed;
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            lock (_stateLock)
            {
                _result.TryGetValue(request.TaskId, out var result);
#if UNITY_2017_1_OR_NEWER
                this._inflightRequest.Remove(request.TaskId);
                this._result.Remove(request.TaskId);
#else
                this._inflightRequest.TryRemove(request.TaskId, out _);
                this._result.TryRemove(request.TaskId, out _);
#endif
                return result;
            }
        }

        private void ThrowIfOpenWasInvalidated(long generation)
        {
            if (_sessionGeneration != generation || this.State != State.Opening)
            {
                throw new SessionNotOpenException("Session no longer open.");
            }
        }

        private int GetInflightRequestCount()
        {
            lock (_stateLock)
            {
                return _inflightRequest.Count;
            }
        }

        private void RemoveInflightRequest(RestSessionRequest request)
        {
            if (!_inflightRequest.TryGetValue(request.TaskId, out var currentRequest) ||
                !ReferenceEquals(currentRequest, request))
            {
                return;
            }

#if UNITY_2017_1_OR_NEWER
            _inflightRequest.Remove(request.TaskId);
#else
            _inflightRequest.TryRemove(request.TaskId, out _);
#endif
        }

        private static void TryAbort(RestSessionRequest request)
        {
            try
            {
                request.Abort();
            }
            catch (System.Exception)
            {
                // Preserve the timeout/close result even if transport cleanup fails.
            }
        }

        /// <summary>要求を送ってよい状態か（Steady の再送の前に確かめる。世代が変わっていたら送らない）。</summary>
        private bool IsRetryableSessionState(long generation)
        {
            lock (_stateLock)
            {
                return generation == _sessionGeneration &&
                       (this.State == State.Available || this.State == State.Opening);
            }
        }

        /// <summary>
        /// 要求を 1 回だけ送る。timeoutSec 内に終わらなければ中断して null を返す（呼び手が RequestTimeoutException にする）。
        /// </summary>
        private async Task<RestResult> InvokeOnceAsync(RestSessionRequest request, int timeoutSec)
        {
            var invokeTask = InvokeRequestAsync(request);
            using var timeoutCancellation = new CancellationTokenSource();
            var timeoutTask = timeoutSec <= 0
                ? Task.CompletedTask
                : Task.Delay(TimeSpan.FromSeconds(timeoutSec), timeoutCancellation.Token);
            if (await Task.WhenAny(invokeTask, timeoutTask) != invokeTask)
            {
                TryAbort(request);
                invokeTask.Forget();
                return null;
            }
            timeoutCancellation.Cancel();
            return await invokeTask;
        }

        protected virtual Task<RestResult> InvokeRequestAsync(RestSessionRequest request)
        {
            return request.Invoke();
        }
    }
}
