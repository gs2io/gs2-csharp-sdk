using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        // ReSharper disable once MemberCanBePrivate.Global
        public State State;
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);
        
        private Dictionary<Gs2SessionTaskId, RestSessionRequest> _inflightRequest = new Dictionary<Gs2SessionTaskId, RestSessionRequest>();
        protected Dictionary<Gs2SessionTaskId, RestResult> _result = new Dictionary<Gs2SessionTaskId, RestResult>();

        public IGs2Credential Credential { get; }
        public Region Region { get; }
        public string OwnerId { get; private set; }

        private readonly bool _checkCertificateRevocation;
        
        public Gs2RestSession(IGs2Credential basicGs2Credential, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : this(basicGs2Credential, region.DisplayName(), checkCertificateRevocation) {
            
        }

        public Gs2RestSession(IGs2Credential basicGs2Credential, string region, bool checkCertificateRevocation = true)
        {
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);

            this._checkCertificateRevocation = checkCertificateRevocation;
            this.State = State.Idle;
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
                while (!this._semaphore.Wait(0)) {
                    yield return null;
                }
                try {
                    if (this.State == State.Available) {
                        result.OnComplete(new OpenResult());
                        yield break;
                    }

                    if (this.State != State.Idle && this.State != State.Closed) {
                        throw new InvalidOperationException("invalid state: " + this.State);
                    }

                    this._result.Clear();
                    this._inflightRequest.Clear();
                    this.State = State.Opening;
                    if (Credential is ProjectTokenGs2Credential) {
                        OwnerId = Credential.ClientId;
                    } else {
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
                            this.State = State.Closed;
                            result.OnError(task.Error);
                            yield break;
                        }

                        Credential.ProjectToken = task.Result.AccessToken;
                        OwnerId = task.Result.OwnerId;
                    }
                    this.State = State.Available;

                    result.OnComplete(new OpenResult());
#if !UNITY_2017_1_OR_NEWER
                    yield break;
#endif
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
                try {
                    var result = await task.Invoke();

                    Credential.ProjectToken = result.AccessToken;
                    OwnerId = result.OwnerId;
                    this.State = State.Available;

                    return new OpenResult();
                }
                catch (System.Exception) {
                    this.State = State.Closed;
                    throw;
                }
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

                var future = OpenFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                future.OnComplete(future.Result);
                
#if !UNITY_2017_1_OR_NEWER
                yield break;
#endif
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

            return await OpenAsync();
        }
#endif
        
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

                this.State = State.Closed;
            }
        }
        
        // Send
        
        public virtual IEnumerator Send(IGs2SessionRequest request) {
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
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public virtual async UniTask SendAsync(IGs2SessionRequest request)
    #else
        public virtual async Task SendAsync(IGs2SessionRequest request)
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
#endif

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
            return this._result.ContainsKey(request.TaskId);
        }

        public bool IsDisconnected()
        {
            return this.State == State.Idle || this.State == State.Closing || this.State == State.Closed;
        }

        public IGs2SessionResult MarkRead(IGs2SessionRequest request)
        {
            var result = _result[request.TaskId];
            this._inflightRequest.Remove(request.TaskId);
            this._result.Remove(request.TaskId);
            return result;
        }
    }
}