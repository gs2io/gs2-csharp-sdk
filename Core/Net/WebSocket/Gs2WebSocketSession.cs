using System;
using System.Collections;
using System.Collections.Generic;
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
using Gs2.Util.WebSocketSharp;
#if UNITY_WEBGL && !UNITY_EDITOR
using Gs2.HybridWebSocket;
#endif
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

        public delegate void NotificationHandler(NotificationMessage message);
        public event NotificationHandler OnNotificationMessage;
        public delegate void DisconnectHandler();
        public event DisconnectHandler OnDisconnect;

#if UNITY_WEBGL && !UNITY_EDITOR
        private HybridWebSocket.WebSocket _session;
#else
        private WebSocket _session;
#endif
        // ReSharper disable once MemberCanBePrivate.Global
        public State State;
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);

        private readonly Dictionary<Gs2SessionTaskId, WebSocketSessionRequest> _inflightRequest = new Dictionary<Gs2SessionTaskId, WebSocketSessionRequest>();
        private readonly Dictionary<Gs2SessionTaskId, WebSocketResult> _result = new Dictionary<Gs2SessionTaskId, WebSocketResult>();

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

        private OpenResult OpenImpl()
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
            this.State = State.Opening;
            
            var url = EndpointHost.Replace("{region}", Region.DisplayName());

#if UNITY_WEBGL && !UNITY_EDITOR
            this._session = WebSocketFactory.CreateInstance(url);
#else
            this._session = new WebSocket(url) {SslConfiguration = {EnabledSslProtocols = SslProtocols.Tls12, CheckCertificateRevocation = this._checkCertificateRevocation}};
#endif
            
#if UNITY_WEBGL && !UNITY_EDITOR
            this._session.OnOpen += () =>
#else
            this._session.OnOpen += (_, _) =>
#endif
            {
                this.State = State.LoggingIn;
                new WebSocketOpenTask(this, new LoginRequest {ClientId = Credential.ClientId, ClientSecret = Credential.ClientSecret,}).NonBlockingInvoke();
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            this._session.OnMessage += (message) =>
			{
                var gs2WebSocketResponse = new WebSocketResult(message);
                if (gs2WebSocketResponse.Gs2SessionTaskId == Gs2SessionTaskId.InvalidId)
                {
                    // API 応答以外のメッセージ
                    OnNotificationMessage?.Invoke(NotificationMessage.FromJson(gs2WebSocketResponse.Body));
                }
                else
                {
                    if (State == State.LoggingIn)
                    {
                        if (gs2WebSocketResponse.Error == null)
                        {
                            var projectToken = LoginResult.FromJson(gs2WebSocketResponse.Body).AccessToken;
                            if (projectToken != null)
                            {
                                Credential.ProjectToken = projectToken;
                                this.State = State.Available;
                            }
                        }
                        else
                        {
#pragma warning disable 4014
                            CloseAsync();
#pragma warning restore 4014
                        }
                    }

                    OnMessage(gs2WebSocketResponse);
                }
            };
#else
            this._session.OnMessage += (_, messageEventArgs) =>
            {
                if (!messageEventArgs.IsText) {
                    return;
                }
                
                var gs2WebSocketResponse = new WebSocketResult(messageEventArgs.Data);
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
#pragma warning disable 4014
                            CloseAsync();
#pragma warning restore 4014
                        }
                    }
                    OnMessage(gs2WebSocketResponse);
                }
            };
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            this._session.OnClose += (_) =>
#else
            this._session.OnClose += (_, _) =>
#endif
            {
                OnDisconnect?.Invoke();
#pragma warning disable 4014
                CloseAsync();
#pragma warning restore 4014
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            this._session.OnError += (errorEventArgs) =>
#else
            this._session.OnError += (_, _) =>
#endif
            {
#pragma warning disable 4014
                CloseAsync();
#pragma warning restore 4014
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            this._session.Connect();
#else
            try
            {
                this._session.ConnectAsync();
            }
            catch (PlatformNotSupportedException)
            {
                this._session.Connect();
            }
#endif
            return new OpenResult();
        }

        // Open
        
        
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
                while (!this._semaphore.Wait(0)) {
                    yield return null;
                }
                try {
                    OpenImpl();
                    {
                        var begin = DateTime.Now;
                        while (this.State == State.LoggingIn) {
                            if ((DateTime.Now - begin).Seconds > 10) {
                                result.OnError(
                                    new RequestTimeoutException(Array.Empty<RequestError>())
                                );
                                yield break;
                            }

    #if UNITY_2017_1_OR_NEWER
                            yield return new WaitForSeconds(0.05f);
    #endif
                        }
                    }
                    {
                        var begin = DateTime.Now;
    #if UNITY_WEBGL && !UNITY_EDITOR
                        while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Open)
    #else
                        while (this._session.ReadyState != WebSocketState.Open)
    #endif
                        {
                            if ((DateTime.Now - begin).Seconds > 10) {
                                result.OnError(
                                    new RequestTimeoutException(Array.Empty<RequestError>())
                                );
                                yield break;
                            }

    #if UNITY_2017_1_OR_NEWER
                            yield return new WaitForSeconds(0.05f);
    #endif
                        }
                    }
                    result.OnComplete(new OpenResult());
                }
                finally {
                    this._semaphore.Release();
                }
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
            // ReSharper disable once MethodHasAsyncOverload
            while (!this._semaphore.Wait(0)) {
    #if UNITY_2017_1_OR_NEWER
                await UniTask.Yield();
    #else
                await Task.Yield();
    #endif
            }
            try {
                var result = OpenImpl();
                {
                    var begin = DateTime.Now;
                    while (this.State != State.Available)
                    {
                        if ((DateTime.Now - begin).Seconds > 10)
                        {
                            throw new RequestTimeoutException(Array.Empty<RequestError>());
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }
                }
                {
                    var begin = DateTime.Now;
    #if UNITY_WEBGL && !UNITY_EDITOR
                    while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Open)
    #else
                    while (this._session.ReadyState != WebSocketState.Open)
    #endif
                    {
                        if ((DateTime.Now - begin).Seconds > 10)
                        {
                            throw new RequestTimeoutException(Array.Empty<RequestError>());
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }
                }

                return result;
            }
            finally {
                this._semaphore.Release();
            }
        }
#endif
        
        // ReOpen
        
        
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
                if (this.State == State.Opening || this.State == State.LoggingIn) {
                    var begin = DateTime.Now;
                    while (this.State != State.Available) {
                        if ((DateTime.Now - begin).Seconds > 10) {
                            throw new RequestTimeoutException(Array.Empty<RequestError>());
                        }

#if UNITY_2017_1_OR_NEWER
                        yield return new WaitForSeconds(0.05f);
#endif
                    }
                }

                var future = OpenFuture();
                yield return future;
                
                result.OnComplete(new OpenResult());
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
            if (this.State == State.Opening || this.State == State.LoggingIn) {
                var begin = DateTime.Now;
                while (this.State != State.Available) {
                    if ((DateTime.Now - begin).Seconds > 10) {
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                }
            }

            await OpenAsync();

            return new OpenResult();
        }
#endif
        
        // Close

        
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
                if (this.State == State.Idle) {
                    this.State = State.Closed;
                }
                else {
                    this.State = State.CancellingTasks;

                    {
                        var begin = DateTime.Now;
                        while (this._inflightRequest.Count > 0) {
                            if ((DateTime.Now - begin).Seconds > 3) {
                                this._inflightRequest.Clear();
                                break;
                            }

#if UNITY_2017_1_OR_NEWER
                            yield return new WaitForSeconds(0.01f);
#endif
                        }
                    }

                    this.State = State.Closing;

                    this._session.Close();

                    {
                        var begin = DateTime.Now;
#if UNITY_WEBGL && !UNITY_EDITOR
                    while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Closed)
#else
                        while (this._session.ReadyState != WebSocketState.Closed)
#endif
                        {
                            if ((DateTime.Now - begin).Seconds > 3) {
                                this._inflightRequest.Clear();
                                break;
                            }

#if UNITY_2017_1_OR_NEWER
                            yield return new WaitForSeconds(0.05f);
#endif
                        }
                    }

                    this.State = State.Closed;
                }
                
                result.OnComplete(null);

#if !UNITY_2017_1_OR_NEWER
                yield break;
#endif
            }

            return new Gs2InlineFuture(Impl);
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask CloseAsync()
#else
        public async Task CloseAsync()
#endif
        {
            if (this.State == State.Idle) {
                this.State = State.Closed;
            }
            else {
                this.State = State.CancellingTasks;

                {
                    var begin = DateTime.Now;
                    while (this._inflightRequest.Count > 0) {
                        if ((DateTime.Now - begin).Seconds > 3) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }
                }

                this.State = State.Closing;

                // ReSharper disable once MethodHasAsyncOverload
                this._session.Close();

                {
                    var begin = DateTime.Now;
#if UNITY_WEBGL && !UNITY_EDITOR
                    while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Closed)
#else
                    while (this._session.ReadyState != WebSocketState.Closed)
#endif
                    {
                        if ((DateTime.Now - begin).Seconds > 3) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }
                }

                this.State = State.Closed;
            }
        }
        
        // Send
        
        public IEnumerator Send(IGs2SessionRequest request) {
            if (request is WebSocketSessionRequest sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try {
                    this._session.Send(sessionRequest.Body);
                }
                catch (SystemException) {
                    this._inflightRequest.Remove(sessionRequest.TaskId);
                }
            }
            yield return null;
        }
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask SendAsync(IGs2SessionRequest request)
    #else
        public async Task SendAsync(IGs2SessionRequest request)
    #endif
        {
            if (request is WebSocketSessionRequest sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try {
                    this._session.Send(sessionRequest.Body);
                }
                catch (SystemException) {
                    this._inflightRequest.Remove(sessionRequest.TaskId);
                }
            }
        }
#endif
        
        public void SendNonBlocking(IGs2SessionRequest request)
        {
            if (request is WebSocketSessionRequest sessionRequest)
            {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try
                {
                    this._session.Send(sessionRequest.Body);
                }
                catch (SystemException)
                {
                    this._inflightRequest.Remove(sessionRequest.TaskId);
                }
            }
        }

        public bool Ping()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#else
            return this._session.Ping();
#endif
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
            return this.State == State.Idle || this.State == State.Closing || this.State == State.Closed;
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            var result = _result[request.TaskId];
            this._result.Remove(request.TaskId);
            return result;
        }

        private void OnMessage(WebSocketResult result)
        {
            if (this._inflightRequest.ContainsKey(result.Gs2SessionTaskId))
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
                this._inflightRequest.Remove(result.Gs2SessionTaskId);
            }
        }
    }
}