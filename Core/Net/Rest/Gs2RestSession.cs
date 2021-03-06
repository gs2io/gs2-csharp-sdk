using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Core.Result;
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

        public State State;
        
        private Dictionary<Gs2SessionTaskId, RestSessionRequest> _inflightRequest = new Dictionary<Gs2SessionTaskId, RestSessionRequest>();
        private Dictionary<Gs2SessionTaskId, RestResult> _result = new Dictionary<Gs2SessionTaskId, RestResult>();

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        public bool _checkCertificateRevocation { get; } = true;
        
        public Gs2RestSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation)
        {
            
        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true)
        {
            Credential = basicGs2Credential;
            if (Enum.TryParse(region, out Region result))
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

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            if (State == State.Available)
            {
                callback.Invoke(new AsyncResult<OpenResult>(new OpenResult(), null));
                yield break;
            }

            if (State != State.Idle && State != State.Closed)
            {
                throw new InvalidOperationException("invalid state");
            }

            _result.Clear();
            _inflightRequest.Clear();
            State = State.Opening;
            var task = new RestOpenTask(
                this,
#if UNITY_2017_1_OR_NEWER
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_checkCertificateRevocation)),
#else
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
#endif
                new LoginRequest
                {
                    ClientId = Credential.ClientId,
                    ClientSecret = Credential.ClientSecret,
                }
            );
            yield return task;

            if (task.Error != null)
            {
                callback.Invoke(new AsyncResult<OpenResult>(null, task.Error));
                yield break;
            }
            
            Credential.ProjectToken = task.Result.AccessToken;
            State = State.Available;
            
            callback.Invoke(new AsyncResult<OpenResult>(new OpenResult(), null));
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
            if (State == State.Available)
            {
                return new OpenResult();
            }

            State = State.Opening;
            var result = await new RestOpenTask(
                this,
#if UNITY_2017_1_OR_NEWER
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_checkCertificateRevocation)),
#else
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
#endif
                new LoginRequest
                {
                    ClientId = Credential.ClientId,
                    ClientSecret = Credential.ClientSecret,
                }
            ).Invoke();
            Credential.ProjectToken = result.AccessToken;
            State = State.Available;
            return new OpenResult();
        }

        public IEnumerator Send(IGs2SessionRequest request)
        {
            if (request is RestSessionRequestFuture sessionRequest)
            {
                _inflightRequest[sessionRequest.TaskId] = sessionRequest;

                yield return sessionRequest;

                if (sessionRequest.Error != null)
                {
                    _inflightRequest.Remove(sessionRequest.TaskId);
                }

                _result[sessionRequest.TaskId] = sessionRequest.Result;
            }
            yield return null;
        }

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask SendAsync(IGs2SessionRequest request)
#else
        public async Task SendAsync(IGs2SessionRequest request)
#endif
        {
            if (request is RestSessionRequest sessionRequest)
            {
                _inflightRequest[sessionRequest.TaskId] = sessionRequest;

                try
                {
                    _result[sessionRequest.TaskId] = await sessionRequest.Invoke();
                }
                catch (Gs2Exception e)
                {
                    _result[sessionRequest.TaskId] = new RestResult(0, "{}")
                    {
                        Error = e,
                    };
                    
                    _inflightRequest.Remove(sessionRequest.TaskId);
                }
            }
            await Task.Yield();
        }

        public bool Ping()
        {
            return true;
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback)
#else
        public IEnumerator Close(Action callback)
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
                        
#if UNITY_2017_1_OR_NEWER
                        yield return new WaitForSeconds(0.01f);
#endif
                    }
                }

                State = State.Closing;
                
                State = State.Closed;
            }
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

                State = State.Closed;
            }

            await Task.Yield();
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
            _inflightRequest.Remove(request.TaskId);
            _result.Remove(request.TaskId);
            return result;
        }
    }
}