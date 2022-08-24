using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Core.Result;
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
        internal HybridWebSocket.WebSocket _session;
#else
        internal WebSocket _session;
#endif
        public State State;

        internal Dictionary<Gs2SessionTaskId, WebSocketSessionRequest> _inflightRequest = new Dictionary<Gs2SessionTaskId, WebSocketSessionRequest>();
        private Dictionary<Gs2SessionTaskId, WebSocketResult> _result = new Dictionary<Gs2SessionTaskId, WebSocketResult>();

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        public bool _checkCertificateRevocation { get; } = false;

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation)
        {
            
        }

        public Gs2WebSocketSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true)
        {
            Credential = basicGs2Credential;
            if (Enum.TryParse<Region>(region, out Region result))
            {
                Region = result;
            }
            else
            {
                Region = Region.ApNortheast1;
            }

            _checkCertificateRevocation = checkCertificateRevocation;
            State = State.Idle;
        }

        private OpenResult OpenImpl()
        {
            if (State == State.Available)
            {
                return new OpenResult();
            }

            if (State != State.Idle && State != State.Closed)
            {
                throw new InvalidOperationException("invalid state");
            }

            _result.Clear();
            _inflightRequest.Clear();
            State = State.Opening;
            
            var url = EndpointHost.Replace("{region}", Region.DisplayName());

#if UNITY_WEBGL && !UNITY_EDITOR
            _session = WebSocketFactory.CreateInstance(url);
#else
            _session = new WebSocket(url) {SslConfiguration = {EnabledSslProtocols = SslProtocols.Tls12, CheckCertificateRevocation = _checkCertificateRevocation}};
#endif
            
#if UNITY_WEBGL && !UNITY_EDITOR
            _session.OnOpen += () =>
#else
            _session.OnOpen += (sender, eventArgs) =>
#endif
            {
                State = State.LoggingIn;
                new WebSocketOpenTask(this, new LoginRequest {ClientId = Credential.ClientId, ClientSecret = Credential.ClientSecret,}).NonBlockingInvoke();
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            _session.OnMessage += (message) =>
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
                            Credential.ProjectToken = LoginResult.FromJson(gs2WebSocketResponse.Body).AccessToken;
                            State = State.Available;
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
            _session.OnMessage += (sender, messageEventArgs) =>
            {
                if (messageEventArgs.IsText)
                {
                    var gs2WebSocketResponse = new WebSocketResult(messageEventArgs.Data);
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
                                Credential.ProjectToken = LoginResult.FromJson(gs2WebSocketResponse.Body).AccessToken;
                                State = State.Available;
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
                }
            };
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            _session.OnClose += (closeEventArgs) =>
#else
            _session.OnClose += (sender, closeEventArgs) =>
#endif
            {
                OnDisconnect?.Invoke();
#pragma warning disable 4014
                CloseAsync();
#pragma warning restore 4014
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            _session.OnError += (errorEventArgs) =>
#else
            _session.OnError += (sender, errorEventArgs) =>
#endif
            {
#pragma warning disable 4014
                CloseAsync();
#pragma warning restore 4014
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            _session.Connect();
#else
            try
            {
                _session.ConnectAsync();
            }
            catch (PlatformNotSupportedException)
            {
                _session.Connect();
            }
#endif
            return new OpenResult();
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            var result = OpenImpl();
            {
                var begin = DateTime.Now;
                while (State == State.LoggingIn)
                {
                    if ((DateTime.Now - begin).Seconds > 10)
                    {
                        callback.Invoke(new AsyncResult<OpenResult>(null,
                            new RequestTimeoutException(new RequestError[0])));
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
                while (_session.ReadyState != WebSocketState.Open)
#endif
                {
                    if ((DateTime.Now - begin).Seconds > 10)
                    {
                        callback.Invoke(new AsyncResult<OpenResult>(null,
                            new RequestTimeoutException(new RequestError[0])));
                        yield break;
                    }

#if UNITY_2017_1_OR_NEWER
                    yield return new WaitForSeconds(0.05f);
#endif
                }
            }

            callback.Invoke(new AsyncResult<OpenResult>(result, null));
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReOpen(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator ReOpen(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            if (State == State.Opening || State == State.LoggingIn)
            {
                var begin = DateTime.Now;
                while (State != State.Available)
                {
                    if ((DateTime.Now - begin).Seconds > 10)
                    {
                        callback.Invoke(new AsyncResult<OpenResult>(null,
                            new RequestTimeoutException(new RequestError[0])));
                        yield break;
                    }

#if UNITY_2017_1_OR_NEWER
                    yield return new WaitForSeconds(0.05f);
#endif
                }
            }

            yield return Open(callback);
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> OpenAsync()
#else
        public async Task<OpenResult> OpenAsync()
#endif
        {
            var result = OpenImpl();
            {
                var begin = DateTime.Now;
                while (State == State.LoggingIn)
                {
                    if ((DateTime.Now - begin).Seconds > 10)
                    {
                        throw new RequestTimeoutException(new RequestError[0]);
                    }

                    await Task.Delay(50);
                }
            }
            {
                var begin = DateTime.Now;
#if UNITY_WEBGL && !UNITY_EDITOR
                while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Open)
#else
                while (_session.ReadyState != WebSocketState.Open)
#endif
                {
                    if ((DateTime.Now - begin).Seconds > 10)
                    {
                        throw new RequestTimeoutException(new RequestError[0]);
                    }

                    await Task.Delay(50);
                }
            }

            return result;
        }

        public IEnumerator Send(IGs2SessionRequest request)
        {
            if (request is WebSocketSessionRequest sessionRequest)
            {
                _inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try
                {
                    _session.Send(sessionRequest?.Body);
                }
                catch (SystemException e)
                {
                    _inflightRequest.Remove(sessionRequest.TaskId);
                }
            }

            yield return null;
        }

        public void SendNonBlocking(IGs2SessionRequest request)
        {
            if (request is WebSocketSessionRequest sessionRequest)
            {
                _inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try
                {
                    _session.Send(sessionRequest?.Body);
                }
                catch (SystemException e)
                {
                    _inflightRequest.Remove(sessionRequest.TaskId);
                }
            }
        }

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask SendAsync(IGs2SessionRequest request)
#else
        public async Task SendAsync(IGs2SessionRequest request)
#endif
        {
            if (request is WebSocketSessionRequest sessionRequest)
            {
                _inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try
                {
                    _session.Send(sessionRequest?.Body);
                }
                catch (SystemException e)
                {
                    _inflightRequest.Remove(sessionRequest.TaskId);
                }
            }

            await Task.Yield();
        }

        public bool Ping()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#else
            return _session.Ping();
#endif
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback) {
#else
        public IEnumerator Close(Action callback) {
#endif

            if (State == State.Idle)
            {
                State = State.Closed;
            }
            else
            {
                State = State.CancellingTasks;
                
				{
                	var begin = DateTime.Now;
                	while (_inflightRequest.Count > 0)
                	{
                    	if ((DateTime.Now - begin).Seconds > 3)
                    	{
                        	_inflightRequest.Clear();
                        	break;
                    	}

#if UNITY_2017_1_OR_NEWER
                    	yield return new WaitForSeconds(0.01f);
#endif
                	}
				}

                State = State.Closing;
                
                _session.Close();

                {
                    var begin = DateTime.Now;
#if UNITY_WEBGL && !UNITY_EDITOR
                    while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Closed)
#else
                    while (_session.ReadyState != WebSocketState.Closed)
#endif
                    {
                        if ((DateTime.Now - begin).Seconds > 3)
                        {
                        	_inflightRequest.Clear();
                        	break;
                        }

#if UNITY_2017_1_OR_NEWER
                        yield return new WaitForSeconds(0.05f);
#endif
                    }
                }

                State = State.Closed;
            }
            callback.Invoke();

            yield return null;
        }

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask CloseAsync()
#else
        public async Task CloseAsync()
#endif
        {
            if (State == State.Idle)
            {
                State = State.Closed;
            }
            else
            {
                State = State.CancellingTasks;

                {
                    var begin = DateTime.Now;
                    while (_inflightRequest.Count > 0)
                    {
                        if ((DateTime.Now - begin).Seconds > 3)
                        {
                        	_inflightRequest.Clear();
                        	break;
                        }

                        await Task.Delay(10);
                    }
                }

                State = State.Closing;
                
                _session.Close();

                {
                    var begin = DateTime.Now;
#if UNITY_WEBGL && !UNITY_EDITOR
                    while (_session.GetState() != Gs2.HybridWebSocket.WebSocketState.Closed)
#else
                    while (_session.ReadyState != WebSocketState.Closed)
#endif
                    {
                        if ((DateTime.Now - begin).Seconds > 3)
                        {
                            throw new RequestTimeoutException(new RequestError[0]);
                        }

                        await Task.Delay(50);
                    }
                }

                State = State.Closed;
            }
        }

        public bool IsCanceled()
        {
            return State == State.CancellingTasks;
        }

        public bool IsCompleted(IGs2SessionRequest request)
        {
            return _result.ContainsKey(request.TaskId);
        }

        public bool IsDisconnected()
        {
            return State == State.Idle || State == State.Closing || State == State.Closed;
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            var result = _result[request.TaskId];
            _result.Remove(request.TaskId);
            return result;
        }
        
        public void OnMessage(WebSocketResult result)
        {
            if (_inflightRequest.ContainsKey(result.Gs2SessionTaskId))
            {
                try
                {
                    _result[result.Gs2SessionTaskId] = result;
                }
                catch (Gs2Exception e)
                {
                    _result[result.Gs2SessionTaskId] = new WebSocketResult("{}")
                    {
                        Error = e,
                    };
                }
                _inflightRequest.Remove(result.Gs2SessionTaskId);
            }
        }
    }
}