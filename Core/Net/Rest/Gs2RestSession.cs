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

        public Gs2RestSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation) {

        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true)
        {
#if UNITY_2017_1_OR_NEWER && (!UNITY_WEBGL || UNITY_EDITOR)
            ValidateCertificateRevocationConfiguration(checkCertificateRevocation, false);
#endif
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);

            this._checkCertificateRevocation = checkCertificateRevocation;
            this.State = State.Idle;
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

                RestResult result;
                try
                {
                    var invokeTask = InvokeRequestAsync(sessionRequest);
                    using var timeoutCancellation = new CancellationTokenSource();
                    var timeoutTask = timeoutSec <= 0
                        ? Task.CompletedTask
                        : Task.Delay(TimeSpan.FromSeconds(timeoutSec), timeoutCancellation.Token);
                    if (await Task.WhenAny(invokeTask, timeoutTask) != invokeTask)
                    {
                        TryAbort(sessionRequest);
                        invokeTask.Forget();
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }
                    timeoutCancellation.Cancel();
                    result = await invokeTask;
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

        protected virtual Task<RestResult> InvokeRequestAsync(RestSessionRequest request)
        {
            return request.Invoke();
        }
    }
}
