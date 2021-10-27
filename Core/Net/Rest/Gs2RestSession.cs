using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Model.Internal;
using Gs2.Core.Result;
#if UNITY_2017_1_OR_NEWER
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

        public Gs2RestSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1) : this(basicGs2Credential, region.DisplayName())
        {
            
        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, string region)
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
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
        
        public async Task<OpenResult> OpenAsync()
        {
            if (State == State.Available)
            {
                return new OpenResult();
            }

            State = State.Opening;
            var result = await new RestOpenTask(
                this,
#if UNITY_2017_1_OR_NEWER
                new RestSessionRequestFactory(() => new UnityRestSessionRequest()),
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
                _result[sessionRequest.TaskId] = sessionRequest.Result;
            }
            yield return null;
        }

        public async Task SendAsync(IGs2SessionRequest request)
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
                }
            }
            await Task.Yield();
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
                
                while (_inflightRequest.Count > 0)
                {
#if UNITY_2017_1_OR_NEWER
                    yield return new WaitForSeconds(0.01f);
#endif
                }

                State = State.Closing;
                
                State = State.Closed;
            }
            yield return null;
        }
        public async Task CloseAsync()
        {
            if (State == State.Idle)
            {
                State = State.Closed;
            }
            else
            {
                State = State.CancellingTasks;
                
                while (_inflightRequest.Count > 0)
                {
                    await Task.Delay(10);
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

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            var result = _result[request.TaskId];
            _inflightRequest.Remove(request.TaskId);
            _result.Remove(request.TaskId);
            return result;
        }
    }
}