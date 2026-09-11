using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Authentication;
using System.Threading;
using Gs2.Core.Domain;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Core.Result;
using Gs2.Core.Util;
using Gs2.Gs2Distributor.Model;
using Gs2.Gs2JobQueue.Model;
#if UNITY_2017_1_OR_NEWER
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;
using UnityEngine.Events;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#endif

namespace Gs2.Core.Net
{
    public partial class Gs2WebSocketSession : IGs2Session, IRequestTrackingSession
    {
        public static string EndpointHost = "wss://gateway-ws.{region}.gen2.gs2io.com";
        public static int OpenTimeoutSec = 10;
        public static int CloseTimeoutSec = 3;

        public delegate void NotificationHandler(NotificationMessage message);
        public event NotificationHandler OnNotificationMessage;
        internal event NotificationHandler OnSdkNotificationMessage;
        public delegate void DisconnectHandler();
        public event DisconnectHandler OnDisconnect;
        public delegate void ErrorHandler(Gs2.Core.Exception.Gs2Exception error);
        public event ErrorHandler OnError;

        private WebSocketSession _session;
        private long _sessionGeneration;
        private Gs2SessionTaskId _loginRequestTaskId = Gs2SessionTaskId.InvalidId;
        private Gs2Exception _openError;
        private long _disconnectPendingGeneration;
        private long _disconnectNotifiedGeneration;

        // ReSharper disable once MemberCanBePrivate.Global
        public volatile State State;
        private readonly object _stateLock = new object();
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _closeSemaphore = new SemaphoreSlim(1, 1);

        private readonly ConcurrentDictionary<Gs2SessionTaskId, WebSocketSessionRequest> _inflightRequest = new ConcurrentDictionary<Gs2SessionTaskId, WebSocketSessionRequest>();
        private readonly ConcurrentDictionary<Gs2SessionTaskId, WebSocketResult> _result = new ConcurrentDictionary<Gs2SessionTaskId, WebSocketResult>();
        internal int InflightRequestCount => _inflightRequest.Count;
        internal int ResultCount => _result.Count;
        private readonly Func<string, bool, WebSocketSession> _webSocketSessionFactory;
        private readonly Action<Action> _runOnMainThread;

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        private readonly bool _checkCertificateRevocation;

        private string _steadyEndpoint;

        /// <summary>
        /// Steady（専用フリート）の基点（https://&lt;host&gt;）。null なら共有クラウド（従来の gateway-ws）。
        /// Open の前に設定する。設定すると接続先が wss://&lt;host&gt;/（基点が http:// なら ws://）になる。
        /// 末尾の / と空白は落として正規化する。
        /// </summary>
        public string SteadyEndpoint
        {
            get => this._steadyEndpoint;
            set => this._steadyEndpoint = Gs2RestSession.NormalizeSteadyEndpoint(value);
        }

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true, string steadyEndpoint = null) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation, steadyEndpoint)
        {
        }

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true, string steadyEndpoint = null)
            : this(basicGs2Credential, region, checkCertificateRevocation, CreateWebSocketSession, null, steadyEndpoint)
        {
        }

        internal Gs2WebSocketSession(
            IGs2Credential basicGs2Credential,
            string region,
            bool checkCertificateRevocation,
            Func<string, bool, WebSocketSession> webSocketSessionFactory,
            Action<Action> runOnMainThread = null,
            string steadyEndpoint = null
        )
        {
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);
            SteadyEndpoint = steadyEndpoint;

            this._checkCertificateRevocation = checkCertificateRevocation;
            this._webSocketSessionFactory = webSocketSessionFactory;
            this._runOnMainThread = runOnMainThread ?? TaskUtilities.RunOnMainThreadIfSupported;
            this.State = State.Idle;
        }

        /// <summary>
        /// 接続先。優先順は <see cref="SteadyEndpoint"/> ＞ 共有クラウドの <see cref="EndpointHost"/>
        /// （Go の webSocketUrl（core/websocket.go）と同じ）。Steady の基点は wss://&lt;host&gt;/ にする
        /// （http:// の基点（ローカルの試験・開発）は ws://）。URL として読めない基点は共有クラウドに落とす。
        /// ★handshake には上限を置いていない: WebSocketSession（websocket-sharp / WebGL）に handshake 専用の
        /// タイムアウトを渡す口が無い。REST 側の近似（<see cref="Gs2RestSession.SteadyConnectTimeoutSec"/>）だけが効く。
        /// </summary>
        public string EndpointUrl()
        {
            return SteadyWebSocketUrl(this._steadyEndpoint)
                   ?? EndpointHost.Replace("{region}", Region.DisplayName());
        }

        /// <summary>Steady の基点（https://&lt;host&gt;）から接続先 wss://&lt;host&gt;/ を作る。空 / 読めないなら null。</summary>
        internal static string SteadyWebSocketUrl(string steadyEndpoint)
        {
            var steady = Gs2RestSession.NormalizeSteadyEndpoint(steadyEndpoint);
            if (steady == null)
            {
                return null;
            }
            if (!Uri.TryCreate(steady, UriKind.Absolute, out var uri) || string.IsNullOrEmpty(uri.Host))
            {
                return null;
            }
            var scheme = uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == "ws" ? "ws" : "wss";
            return scheme + "://" + uri.Authority + "/";
        }

        private static WebSocketSession CreateWebSocketSession(string url, bool checkCertificateRevocation)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return new WebSocketSession(url);
#else
            return new WebSocketSession(url, checkCertificateRevocation);
#endif
        }

        private bool IsActiveSessionEvent(WebSocketSession session, long sessionGeneration)
        {
            return ReferenceEquals(this._session, session) &&
                   this._sessionGeneration == sessionGeneration &&
                   this._disconnectPendingGeneration != sessionGeneration &&
                   this.State is State.Opening or State.LoggingIn or State.Available;
        }

        private bool TryBeginDisconnect(WebSocketSession session, long sessionGeneration)
        {
            lock (this._stateLock)
            {
                if (!IsActiveSessionEvent(session, sessionGeneration))
                {
                    return false;
                }
                this._disconnectPendingGeneration = sessionGeneration;
                this._inflightRequest.Clear();
                return true;
            }
        }

        private void HandleProtocolError(
            WebSocketSession session,
            long sessionGeneration,
            System.Exception exception
        )
        {
            var error = exception as Gs2Exception ??
                        new Gs2.Core.Exception.UnknownException(exception.Message, exception);
            if (!TryBeginDisconnect(session, sessionGeneration))
            {
                return;
            }
            TaskUtilities.RunOnMainThreadIfSupported(() =>
            {
                try
                {
                    OnError?.Invoke(error);
                    NotifyDisconnect(sessionGeneration);
                }
                finally
                {
                    RecordOpenError(session, sessionGeneration, error);
                    CloseSessionAsync(session, sessionGeneration).Forget();
                }
            });
        }

        private void RecordOpenError(
            WebSocketSession session,
            long sessionGeneration,
            Gs2Exception error
        )
        {
            lock (this._stateLock)
            {
                if (ReferenceEquals(this._session, session) &&
                    this._sessionGeneration == sessionGeneration &&
                    this.State is State.Opening or State.LoggingIn)
                {
                    Volatile.Write(ref this._openError, error);
                }
            }
        }

        private void NotifyDisconnect(long sessionGeneration)
        {
            lock (this._stateLock)
            {
                if (this._disconnectNotifiedGeneration == sessionGeneration)
                {
                    return;
                }
                this._disconnectNotifiedGeneration = sessionGeneration;
            }
            OnDisconnect?.Invoke();
        }

        private bool IsReOpenWaitingForTransition()
        {
            lock (this._stateLock)
            {
                return this.State is State.Opening or State.LoggingIn or State.CancellingTasks or State.Closing ||
                       this.State == State.Available &&
                       this._disconnectPendingGeneration == this._sessionGeneration;
            }
        }

        // Open
        
        
#if GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> OpenAsync()
#else
        public async Task<OpenResult> OpenAsync()
#endif
        {
            await TaskUtilities.WaitAsync(this._semaphore);

            var openingStarted = false;
            long sessionGeneration = 0;
            try {
                lock (this._stateLock)
                {
                    if (this.State == State.Available)
                    {
                        return new OpenResult();
                    }

                    if (this.State != State.Idle && this.State != State.Closed)
                    {
                        throw new InvalidOperationException("invalid state");
                    }

                    this._result.Clear();
                    this._inflightRequest.Clear();
                    this._loginRequestTaskId = Gs2SessionTaskId.InvalidId;
                    Volatile.Write(ref this._openError, null);
                    this._session = null;
                    sessionGeneration = ++this._sessionGeneration;
                    this.State = State.Opening;
                    openingStarted = true;
                }

                {
                    var url = EndpointUrl();

                    var session = this._webSocketSessionFactory.Invoke(url, this._checkCertificateRevocation);
                    session.OnOpen += () =>
                    {
                        var loginRequired = true;
                        lock (this._stateLock)
                        {
                            if (!ReferenceEquals(this._session, session) ||
                                this._sessionGeneration != sessionGeneration ||
                                this._disconnectPendingGeneration == sessionGeneration ||
                                this.State != State.Opening)
                            {
                                return;
                            }

                            if (Credential is ProjectTokenGs2Credential)
                            {
                                this.State = State.Available;
                                loginRequired = false;
                            }
                            else
                            {
                                this.State = State.LoggingIn;
                            }
                        }
                        if (!loginRequired)
                        {
                            return;
                        }
                        try
                        {
                            new WebSocketOpenTask(this, new LoginRequest {ClientId = Credential.ClientId, ClientSecret = Credential.ClientSecret,}).NonBlockingInvoke();
                        }
                        catch (System.Exception exception)
                        {
                            try
                            {
                                HandleProtocolError(session, sessionGeneration, exception);
                            }
                            catch (System.Exception)
                            {
                                // The open task observes the protocol error through OnError.
                            }
                        }
                    };

                    session.OnMessage += (message) => TaskUtilities.RunOnMainThreadIfSupported(() =>
                    {
                        lock (this._stateLock)
                        {
                            if (!IsActiveSessionEvent(session, sessionGeneration))
                            {
                                return;
                            }
                        }

                        WebSocketResult gs2WebSocketResponse;
                        try
                        {
                            gs2WebSocketResponse = new WebSocketResult(message);
                        }
                        catch (System.Exception exception)
                        {
                            HandleProtocolError(session, sessionGeneration, exception);
                            return;
                        }
                        if (gs2WebSocketResponse.Gs2SessionTaskId == Gs2SessionTaskId.InvalidId)
                        {
                            // API 応答以外のメッセージ
                            NotificationMessage notification;
                            try
                            {
                                notification = NotificationMessage.FromJson(gs2WebSocketResponse.Body);
                                if (notification?.subject == null || notification.payload == null)
                                {
                                    throw new InvalidOperationException("Notification did not contain required dispatch fields.");
                                }
                            }
                            catch (System.Exception exception)
                            {
                                HandleProtocolError(session, sessionGeneration, exception);
                                return;
                            }
                            lock (this._stateLock)
                            {
                                if (!IsActiveSessionEvent(session, sessionGeneration))
                                {
                                    return;
                                }
                            }
                            try
                            {
                                OnSdkNotificationMessage?.Invoke(notification);
                            }
                            catch (NotificationPayloadException exception)
                            {
                                HandleProtocolError(session, sessionGeneration, exception);
                                return;
                            }
                            lock (this._stateLock)
                            {
                                if (!IsActiveSessionEvent(session, sessionGeneration))
                                {
                                    return;
                                }
                            }
                            OnNotificationMessage?.Invoke(notification);
                        }
                        else
                        {
                            var isLoginResponse = false;
                            lock (this._stateLock)
                            {
                                isLoginResponse = this.State == State.LoggingIn &&
                                                  gs2WebSocketResponse.Gs2SessionTaskId == this._loginRequestTaskId;
                            }
                            if (isLoginResponse)
                            {
                                var loginError = gs2WebSocketResponse.Error;
                                if (loginError == null)
                                {
                                    try
                                    {
                                        var loginResult = LoginResult.FromJson(gs2WebSocketResponse.Body);
                                        var projectToken = loginResult?.AccessToken;
                                        if (projectToken == null)
                                        {
                                            throw new InvalidOperationException("Login response did not contain an access token.");
                                        }
                                        lock (this._stateLock)
                                        {
                                            if (ReferenceEquals(this._session, session) &&
                                                this._sessionGeneration == sessionGeneration &&
                                                this._disconnectPendingGeneration != sessionGeneration &&
                                                this.State == State.LoggingIn)
                                            {
                                                this.Credential.ProjectToken = projectToken;
                                                this.State = State.Available;
                                            }
                                        }
                                    }
                                    catch (System.Exception exception)
                                    {
                                        loginError = exception as Gs2Exception ??
                                                     new Gs2.Core.Exception.UnknownException(exception.Message, exception);
                                    }
                                }
                                ForgetRequest(gs2WebSocketResponse.Gs2SessionTaskId);
                                if (loginError != null)
                                {
                                    if (!TryBeginDisconnect(session, sessionGeneration))
                                    {
                                        return;
                                    }
                                    try
                                    {
                                        OnError?.Invoke(loginError);
                                    }
                                    finally
                                    {
                                        RecordOpenError(session, sessionGeneration, loginError);
                                        CloseSessionAsync(session, sessionGeneration).Forget();
                                    }
                                }
                                return;
                            }
                            lock (this._stateLock)
                            {
                                if (!IsActiveSessionEvent(session, sessionGeneration))
                                {
                                    return;
                                }
                            }
                            OnMessage(gs2WebSocketResponse);
                        }
                    });

                    session.OnClose += () =>
                    {
                        if (!TryBeginDisconnect(session, sessionGeneration))
                        {
                            return;
                        }
                        this._runOnMainThread(() =>
                        {
                            try
                            {
                                NotifyDisconnect(sessionGeneration);
                            }
                            finally
                            {
                                CloseSessionAsync(session, sessionGeneration).Forget();
                            }
                        });
                    };

                    session.OnError += (errorEventArgs) =>
                    {
                        if (!TryBeginDisconnect(session, sessionGeneration))
                        {
                            return;
                        }
                        var error = new Gs2.Core.Exception.UnknownException(
                            new Gs2.Core.Model.RequestError[]{
                                new Gs2.Core.Model.RequestError {
                                    Component = "WebSocket",
                                    Message = errorEventArgs.Message
                                }
                            },
                            errorEventArgs.Exception
                        );
                        this._runOnMainThread(() =>
                        {
                            try
                            {
                                OnError?.Invoke(error);
                                NotifyDisconnect(sessionGeneration);
                            }
                            finally
                            {
                                RecordOpenError(session, sessionGeneration, error);
                                CloseSessionAsync(session, sessionGeneration).Forget();
                            }
                        });
                    };

                    var adopted = false;
                    lock (this._stateLock)
                    {
                        if (this.State == State.Opening &&
                            this._sessionGeneration == sessionGeneration &&
                            this._session == null)
                        {
                            this._session = session;
                            adopted = true;
                            session.Connect();
                        }
                    }

                    if (!adopted)
                    {
                        session.Dispose();
                        throw new SessionNotOpenException(Array.Empty<RequestError>());
                    }
                }

                {
                    var timer = Stopwatch.StartNew();
                    while (this.State != State.Available)
                    {
                        var caught = Volatile.Read(ref this._openError);
                        if (caught != null) {
                            await CloseAsync();
                            throw caught;
                        }
                        if (this.State is State.Closing or State.CancellingTasks) {
                            await CloseAsync();
                            throw new SessionNotOpenException(Array.Empty<RequestError>());
                        }
                        if (this.State == State.Closed) {
                            throw new SessionNotOpenException(Array.Empty<RequestError>());
                        }
                        if (timer.Elapsed.TotalSeconds >= OpenTimeoutSec) {
                            await CloseAsync();
                            throw new RequestTimeoutException(Array.Empty<RequestError>());
                        }

                        await TaskUtilities.Yield();
                    }
                }
                openingStarted = false;
                return new OpenResult();
            }
            catch
            {
                if (openingStarted)
                {
                    try
                    {
                        await CloseAsync();
                    }
                    catch (System.Exception)
                    {
                        // Preserve the startup exception if transport cleanup also fails.
                    }
                }
                throw;
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

            await OpenAsync();

            return new OpenResult();
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
            WebSocketSession session;
            long sessionGeneration;
            lock (this._stateLock)
            {
                session = this._session;
                sessionGeneration = this._sessionGeneration;
            }
            await CloseSessionAsync(session, sessionGeneration);
        }

#if GS2_ENABLE_UNITASK
        private async UniTask CloseSessionAsync(WebSocketSession expectedSession, long expectedGeneration)
#else
        private async Task CloseSessionAsync(WebSocketSession expectedSession, long expectedGeneration)
#endif
        {
            await TaskUtilities.WaitAsync(this._closeSemaphore);
            var shouldTransitionToClosed = false;
            try
            {
                WebSocketSession session;
                long sessionGeneration;
                lock (this._stateLock)
                {
                    if (!ReferenceEquals(this._session, expectedSession) ||
                        this._sessionGeneration != expectedGeneration)
                    {
                        return;
                    }

                    if (this.State == State.Idle || this.State == State.Closed)
                    {
                        if (this.State == State.Idle)
                        {
                            this.State = State.Closed;
                        }
                        return;
                    }

                    shouldTransitionToClosed = true;
                    this.State = State.CancellingTasks;
                    session = this._session;
                    sessionGeneration = this._sessionGeneration;
                }

                {
                    var timer = Stopwatch.StartNew();
                    while (!this._inflightRequest.IsEmpty) {
                        if (timer.Elapsed.TotalSeconds >= CloseTimeoutSec) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        await TaskUtilities.Yield();
                    }
                }

                this.State = State.Closing;

                if (session == null)
                {
                    return;
                }

                try
                {
                    session.Close();
                    TaskUtilities.RunOnMainThreadIfSupported(
                        () => NotifyDisconnect(sessionGeneration)
                    );

                    {
                        var timer = Stopwatch.StartNew();
                        while (session.GetState() != WebSocketSession.StateEnum.Closed)
                        {
                            if (timer.Elapsed.TotalSeconds >= CloseTimeoutSec) {
                                this._inflightRequest.Clear();
                                break;
                            }

                            await TaskUtilities.Yield();
                        }
                    }
                }
                finally
                {
                    session.Dispose();
                }
            }
            finally
            {
                if (shouldTransitionToClosed)
                {
                    lock (this._stateLock)
                    {
                        this.State = State.Closed;
                    }
                }
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
        
        private void SendImpl(IGs2SessionRequest request, bool isLoginRequest = false) {
            if (request is not WebSocketSessionRequest)
            {
                throw new ArgumentException("The request is not a WebSocket session request.", nameof(request));
            }
            if (request is WebSocketSessionRequest sessionRequest) {
                WebSocketSession session;
                long sessionGeneration;
                lock (this._stateLock)
                {
                    if (isLoginRequest)
                    {
                        if (this.State != State.LoggingIn ||
                            this._loginRequestTaskId != Gs2SessionTaskId.InvalidId)
                        {
                            return;
                        }
                    }
                    else if (this.State != State.Available)
                    {
                        throw new SessionNotOpenException("Session no longer open.");
                    }
                    if (this._disconnectPendingGeneration == this._sessionGeneration)
                    {
                        if (isLoginRequest)
                        {
                            return;
                        }
                        throw new SessionNotOpenException("Session no longer open.");
                    }

                    this._inflightRequest[sessionRequest.TaskId] = sessionRequest;
                    if (isLoginRequest)
                    {
                        this._loginRequestTaskId = sessionRequest.TaskId;
                    }
                    session = this._session;
                    sessionGeneration = this._sessionGeneration;
                }

                try {
                    session.Send(sessionRequest.Body);
                }
                catch (System.Exception exception) {
                    lock (this._stateLock)
                    {
                        this._inflightRequest.TryRemove(sessionRequest.TaskId, out _);
                    }
                    try
                    {
                        HandleProtocolError(session, sessionGeneration, exception);
                    }
                    catch (System.Exception)
                    {
                        // Preserve the transport send exception if a public callback also fails.
                    }
                    throw;
                }
            }
        }

        public IEnumerator Send(IGs2SessionRequest request) {
            SendImpl(request);
            yield return null;
        }
        
#pragma warning disable CS1998 // The cross-platform async API delegates to a synchronous WebSocket send.
#if GS2_ENABLE_UNITASK
        public async UniTask SendAsync(IGs2SessionRequest request)
#else
        public async Task SendAsync(IGs2SessionRequest request)
#endif
        {
            SendImpl(request);
        }
#pragma warning restore CS1998
        
        public void SendNonBlocking(IGs2SessionRequest request)
        {
            SendImpl(request);
        }

        internal void SendLoginNonBlocking(IGs2SessionRequest request)
        {
            SendImpl(request, true);
        }

        public bool Ping()
        {
            WebSocketSession session;
            lock (this._stateLock)
            {
                if (this._disconnectPendingGeneration == this._sessionGeneration)
                {
                    return false;
                }
                session = this._session;
            }
            return session?.Ping() ?? false;
        }

        public bool IsCanceled()
        {
            return this.State == State.CancellingTasks;
        }

        public bool IsCompleted(IGs2SessionRequest request)
        {
            return this._result.ContainsKey(request.TaskId);
        }

        public bool IsDisconnected()
        {
            lock (this._stateLock)
            {
                return this.State == State.Idle ||
                       this.State == State.Closing ||
                       this.State == State.Closed ||
                       this._disconnectPendingGeneration == this._sessionGeneration;
            }
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            lock (this._stateLock)
            {
                this._result.TryRemove(request.TaskId, out var result);
                return result;
            }
        }

        void IRequestTrackingSession.Forget(IGs2SessionRequest request)
        {
            ForgetRequest(request.TaskId);
        }

        RequestTrackingState IRequestTrackingSession.GetRequestTrackingState(IGs2SessionRequest request)
        {
            lock (this._stateLock)
            {
                if (this._result.ContainsKey(request.TaskId))
                {
                    return RequestTrackingState.Completed;
                }
                if (this._inflightRequest.TryGetValue(request.TaskId, out var pendingRequest) &&
                    ReferenceEquals(pendingRequest, request))
                {
                    return RequestTrackingState.Pending;
                }
                return RequestTrackingState.Abandoned;
            }
        }

        private void ForgetRequest(Gs2SessionTaskId taskId)
        {
            lock (this._stateLock)
            {
                this._inflightRequest.TryRemove(taskId, out _);
                this._result.TryRemove(taskId, out _);
            }
        }

        private void OnMessage(WebSocketResult result)
        {
            lock (this._stateLock)
            {
                if (this._inflightRequest.TryRemove(result.Gs2SessionTaskId, out _))
                {
                    try
                    {
                        this._result[result.Gs2SessionTaskId] = result;
                    }
                    catch (Gs2Exception e)
                    {
                        this._result[result.Gs2SessionTaskId] = new WebSocketResult("{}")
                        {
                            Error = e,
                        };
                    }
                }
            }
        }
    }
}
