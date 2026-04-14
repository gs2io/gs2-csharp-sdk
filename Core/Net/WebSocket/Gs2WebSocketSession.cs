using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public partial class Gs2WebSocketSession : IGs2Session
    {
        public static string EndpointHost = "wss://gateway-ws.{region}.gen2.gs2io.com";
        public static int OpenTimeoutSec = 10;
        public static int CloseTimeoutSec = 3;

        public delegate void NotificationHandler(NotificationMessage message);
        public event NotificationHandler OnNotificationMessage;
        public delegate void DisconnectHandler();
        public event DisconnectHandler OnDisconnect;
        public delegate void ErrorHandler(Gs2.Core.Exception.Gs2Exception error);
        public event ErrorHandler OnError;

        private WebSocketSession _session;

        // ReSharper disable once MemberCanBePrivate.Global
        public volatile State State;
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);

        private readonly ConcurrentDictionary<Gs2SessionTaskId, WebSocketSessionRequest> _inflightRequest = new ConcurrentDictionary<Gs2SessionTaskId, WebSocketSessionRequest>();
        private readonly ConcurrentDictionary<Gs2SessionTaskId, WebSocketResult> _result = new ConcurrentDictionary<Gs2SessionTaskId, WebSocketResult>();

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        private readonly bool _checkCertificateRevocation;

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation)
        {
        }

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true)
        {
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);

            this._checkCertificateRevocation = checkCertificateRevocation;
            this.State = State.Idle;
        }

        // Open
        
        
        private IEnumerable OpenImpl(Gs2Future<OpenResult> result)
        {
            while (!this._semaphore.Wait(0)) {
                yield return null;
            }

            Gs2Exception caughtError = null;
            void HandleError(Gs2Exception error)
            {
                Volatile.Write(ref caughtError, error);
            }
            OnError += HandleError;
            try {
                if (this.State != State.Available)
                {
                    if (this.State != State.Idle && this.State != State.Closed)
                    {
                        throw new InvalidOperationException("invalid state");
                    }

                    this._result.Clear();
                    this._inflightRequest.Clear();
                    this.State = State.Opening;
                    
                    var url = EndpointHost.Replace("{region}", Region.DisplayName());

                    this._session = new WebSocketSession(url);
                    
                    this._session.OnOpen += () =>
                    {
                        this.State = State.LoggingIn;
                        new WebSocketOpenTask(this, new LoginRequest {ClientId = Credential.ClientId, ClientSecret = Credential.ClientSecret,}).NonBlockingInvoke();
                    };

                    this._session.OnMessage += (message) => TaskUtilities.RunOnMainThreadIfSupported(() =>
                    {
                        var gs2WebSocketResponse = new WebSocketResult(message);
                        if (gs2WebSocketResponse.Gs2SessionTaskId == Gs2SessionTaskId.InvalidId)
                        {
                            // API 応答以外のメッセージ
                            OnNotificationMessage?.Invoke(NotificationMessage.FromJson(gs2WebSocketResponse.Body));
                        }
                        else
                        {
                            if (this.State == State.LoggingIn)
                            {
                                if (gs2WebSocketResponse.Error == null)
                                {
                                    var projectToken = LoginResult.FromJson(gs2WebSocketResponse.Body).AccessToken;
                                    if (projectToken != null)
                                    {
                                        this.Credential.ProjectToken = projectToken;
                                        this.State = State.Available;
                                    }
                                }
                                else
                                {
                                    OnError?.Invoke(gs2WebSocketResponse.Error);
                                    CloseAsync().Forget();
                                }
                            }
                            OnMessage(gs2WebSocketResponse);
                        }
                    });

                    this._session.OnClose += () => TaskUtilities.RunOnMainThreadIfSupported(() =>
                    {
                        OnDisconnect?.Invoke();
                        CloseAsync().Forget();
                    });

                    this._session.OnError += (errorEventArgs) => TaskUtilities.RunOnMainThreadIfSupported(() =>
                    {
                        var error = new Gs2.Core.Exception.UnknownException(new Gs2.Core.Model.RequestError[]{
                            new Gs2.Core.Model.RequestError {
                                Component = "WebSocket",
                                Message = errorEventArgs.Message
                            }
                        });
                        OnError?.Invoke(error);
                        OnDisconnect?.Invoke();
                        CloseAsync().Forget();
                    });

                    this._session.Connect();
                }

                {
                    var begin = DateTime.Now;
                    while (this.State != State.Available)
                    {
                        var caught = Volatile.Read(ref caughtError);
                        if (caught != null) {
                            this._session?.Close();
                            this.State = State.Closed;
                            result.OnError(caught);
                            yield break;
                        }
                        if (this.State is State.Closing or State.Closed or State.CancellingTasks) {
                            result.OnError(
                                new SessionNotOpenException(Array.Empty<RequestError>())
                            );
                            yield break;
                        }
                        if ((DateTime.Now - begin).Seconds > OpenTimeoutSec) {
                            this._session?.Close();
                            this.State = State.Closed;
                            result.OnError(
                                new RequestTimeoutException(Array.Empty<RequestError>())
                            );
                            yield break;
                        }

                        yield return null;
                    }
                }
                result.OnComplete(new OpenResult());
            }
            finally {
                OnError -= HandleError;
                this._semaphore.Release();
            }
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            var future = OpenFuture();
            yield return future;
            callback.Invoke(
                new AsyncResult<OpenResult>(
                    future.Result,
                    future.Error
                )
            );
        }
        
        public Gs2Future<OpenResult> OpenFuture()
        {
            IEnumerator Impl(Gs2Future<OpenResult> result) {
                foreach (var _ in OpenImpl(result)) yield return null;
            }

            return new Gs2InlineFuture<OpenResult>(Impl);
        }
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask<OpenResult> OpenAsync()
    #else
        public async Task<OpenResult> OpenAsync()
    #endif
        {
            Gs2Future<OpenResult> future = new Gs2InlineFuture<OpenResult>(_ => null);

            foreach (var _ in OpenImpl(future)) {
    #if UNITY_2017_1_OR_NEWER
                await UniTask.Yield();
    #else
                await Task.Yield();
    #endif
            }

            if (future.Error != null) throw future.Error;

            return future.Result;
        }
#endif
        
        // ReOpen
        
        
        private IEnumerable ReOpenImpl(Gs2Future<OpenResult> result) {
            if (this.State == State.Opening || this.State == State.LoggingIn) {
                var begin = DateTime.Now;
                while (this.State != State.Available && this.State != State.Closed) {
                    if ((DateTime.Now - begin).Seconds > OpenTimeoutSec) {
                        result.OnError(new RequestTimeoutException(Array.Empty<RequestError>()));
                        yield break;
                    }

                    yield return null;
                }
            }

            foreach (var _ in OpenImpl(result)) yield return null;
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReOpen(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator ReOpen(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            var future = ReOpenFuture();
            yield return future;
            callback.Invoke(
                new AsyncResult<OpenResult>(
                    future.Result,
                    future.Error
                )
            );
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public Gs2Future<OpenResult> ReOpenFuture()
        {
            IEnumerator Impl(Gs2Future<OpenResult> result) {
                foreach (var _ in ReOpenImpl(result)) yield return null;
            }

            return new Gs2InlineFuture<OpenResult>(Impl);
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask<OpenResult> ReOpenAsync()
    #else
        public async Task<OpenResult> ReOpenAsync()
    #endif
        {
            Gs2Future<OpenResult> future = new Gs2InlineFuture<OpenResult>(_ => null);

            foreach (var _ in ReOpenImpl(future)) {
    #if UNITY_2017_1_OR_NEWER
                await UniTask.Yield();
    #else
                await Task.Yield();
    #endif
            }

            if (future.Error != null) throw future.Error;

            return future.Result;
        }
#endif
        
        // Close

        private IEnumerable CloseImpl()
        {
            if (this.State == State.Idle) {
                this.State = State.Closed;
            }
            else {
                this.State = State.CancellingTasks;

                {
                    var begin = DateTime.Now;
                    while (!this._inflightRequest.IsEmpty) {
                        if ((DateTime.Now - begin).Seconds > CloseTimeoutSec) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        yield return null;
                    }
                }

                this.State = State.Closing;

                this._session.Close();

                {
                    var begin = DateTime.Now;
                    while (this._session.GetState() != WebSocketSession.StateEnum.Closed)
                    {
                        if ((DateTime.Now - begin).Seconds > CloseTimeoutSec) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        yield return null;
                    }
                }

                this.State = State.Closed;
            }
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback) {
#else
        public IEnumerator Close(Action callback) {
#endif
            var future = CloseFuture();
            yield return future;
            callback.Invoke();
        }
        
        public Gs2Future CloseFuture()
        {
            IEnumerator Impl(Gs2Future result) {
                foreach (var _ in CloseImpl()) yield return null;

                result.OnComplete(null);
            }

            return new Gs2InlineFuture(Impl);
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask CloseAsync()
#else
        public async Task CloseAsync()
#endif
        {
            foreach (var _ in CloseImpl())
            {
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                await UniTask.Yield();
#else
                await Task.Yield();
#endif
            }
        }
        
        // Send
        
        private void SendImpl(IGs2SessionRequest request) {
            if (request is WebSocketSessionRequest sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try {
                    this._session.Send(sessionRequest.Body);
                }
                catch (SystemException) {
                    this._inflightRequest.TryRemove(sessionRequest.TaskId, out _);
                }
            }
        }

        public IEnumerator Send(IGs2SessionRequest request) {
            SendImpl(request);
            yield return null;
        }
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask SendAsync(IGs2SessionRequest request)
    #else
        public async Task SendAsync(IGs2SessionRequest request)
    #endif
        {
            SendImpl(request);
        }
#endif
        
        public void SendNonBlocking(IGs2SessionRequest request)
        {
            SendImpl(request);
        }

        public bool Ping() => this._session.Ping();

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
            return this.State == State.Idle || this.State == State.Closing || this.State == State.Closed;
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            this._result.TryRemove(request.TaskId, out var result);
            return result;
        }

        private void OnMessage(WebSocketResult result)
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
