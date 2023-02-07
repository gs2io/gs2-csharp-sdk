using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gs2.Core.Domain;
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

        // Open
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback)
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback)
#endif
        {
            var future = OpenFuture();
            yield return future;
            callback.Invoke(new AsyncResult<OpenResult>(
                future.Result,
                future.Error
            ));
        }
        
        public Gs2Future<OpenResult> OpenFuture()
        {
            IEnumerator Impl(Gs2Future<OpenResult> result) {
                if (this.State == State.Available) {
                    result.OnComplete(new OpenResult());
                    yield break;
                }

                if (this.State != State.Idle && this.State != State.Closed) {
                    throw new InvalidOperationException("invalid state");
                }

                this._result.Clear();
                this._inflightRequest.Clear();
                this.State = State.Opening;
                var task = new RestOpenTask(
                    this,
#if UNITY_2017_1_OR_NEWER
                    new RestSessionRequestFactory(() => new UnityRestSessionRequest(_checkCertificateRevocation)),
#else
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
#endif
                    new LoginRequest {
                        ClientId = Credential.ClientId,
                        ClientSecret = Credential.ClientSecret,
                    }
                );
                yield return task;

                if (task.Error != null) {
                    result.OnError(task.Error);
                    yield break;
                }

                Credential.ProjectToken = task.Result.AccessToken;
                this.State = State.Available;

                result.OnComplete(new OpenResult());
            }

            return new Gs2InlineFuture<OpenResult>(Impl);
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> OpenAsync()
#else
        public async Task<OpenResult> OpenAsync()
#endif
        {
            if (this.State == State.Available) {
                return new OpenResult();
            }

            if (this.State != State.Idle && this.State != State.Closed) {
                throw new InvalidOperationException("invalid state");
            }

            this._result.Clear();
            this._inflightRequest.Clear();
            this.State = State.Opening;
            var task = new RestOpenTask(
                this,
#if UNITY_2017_1_OR_NEWER
                new RestSessionRequestFactory(() => new UnityRestSessionRequest(_checkCertificateRevocation)),
#else
                new RestSessionRequestFactory(() => new DotNetRestSessionRequest()),
#endif
                new LoginRequest {
                    ClientId = Credential.ClientId,
                    ClientSecret = Credential.ClientSecret,
                }
            );
            var result = await task.Invoke();

            Credential.ProjectToken = result.AccessToken;
            this.State = State.Available;

            return new OpenResult();
        }
        
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
        
        public Gs2Future<OpenResult> ReOpenFuture()
        {
            IEnumerator Impl(Gs2Future<OpenResult> result) {
                if (this.State == State.Opening || this.State == State.LoggingIn) {
                    var begin = DateTime.Now;
                    while (this.State != State.Available) {
                        if ((DateTime.Now - begin).Seconds > 10) {
                            result.OnError(
                                new RequestTimeoutException(new RequestError[0])
                            );
                            yield break;
                        }

#if UNITY_2017_1_OR_NEWER
                        yield return new WaitForSeconds(0.05f);
#endif
                    }
                }

                var future = OpenFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                future.OnComplete(future.Result);
            }

            return new Gs2InlineFuture<OpenResult>(Impl);
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<OpenResult> ReOpenAsync()
#else
        public async Task<OpenResult> ReOpenAsync()
#endif
        {
            if (this.State == State.Opening || this.State == State.LoggingIn) {
                var begin = DateTime.Now;
                while (this.State != State.Available) {
                    if ((DateTime.Now - begin).Seconds > 10) {
                        throw new RequestTimeoutException(new RequestError[0]);
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                }
            }

            return await OpenAsync();
        }
        
        // Close
        
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback)
#else
        public IEnumerator Close(Action callback)
#endif
        {
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

                    this.State = State.Closed;
                }
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

                this.State = State.Closed;
            }
        }
        
        // Send
        
        public IEnumerator Send(IGs2SessionRequest request) {
            if (request is RestSessionRequestFuture sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                yield return sessionRequest;

                if (sessionRequest.Error != null) {
                    this._inflightRequest.Remove(sessionRequest.TaskId);
                }

                this._result[sessionRequest.TaskId] = sessionRequest.Result;
            }
            yield return null;
        }
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask SendAsync(IGs2SessionRequest request)
#else
        public async Task SendAsync(IGs2SessionRequest request)
#endif
        {
            if (request is RestSessionRequestFuture sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                await sessionRequest.Invoke();

                if (sessionRequest.Error != null) {
                    this._inflightRequest.Remove(sessionRequest.TaskId);
                }

                this._result[sessionRequest.TaskId] = sessionRequest.Result;
            }
        }
        
        public bool Ping()
        {
            return true;
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