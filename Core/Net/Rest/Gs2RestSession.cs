using System;
using System.Collections;
#if !UNITY_2017_1_OR_NEWER
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
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
        public State State;
        private readonly SemaphoreSlim _semaphore  = new SemaphoreSlim(1, 1);

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
            Credential = basicGs2Credential;
            Region = RegionExt.ValueOf(region);

            this._checkCertificateRevocation = checkCertificateRevocation;
            this.State = State.Idle;
        }

#if UNITY_2017_1_OR_NEWER
        public RestSessionRequestFactory CreateRestSessionRequestFactory(UnityEngine.Networking.CertificateHandler certificateHandler = null)
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
        public RestSessionRequestFactory CreateRestSessionRequestFactory()
        {
            return new RestSessionRequestFactory(
                () => new DotNetRestSessionRequest(),
                EnableRequestCompression,
                EnableResponseDecompression
            );
        }
#endif

        // Open

#if GS2_ENABLE_UNITASK
        private async UniTask<OpenResult> OpenImplAsync()
#else
        private async Task<OpenResult> OpenImplAsync()
#endif
        {
            await TaskUtilities.WaitAsync(this._semaphore);
            try {
                if (this.State == State.Available) {
                    return new OpenResult();
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
                    try
                    {
                        var result = await new RestOpenTask(
                            this,
                            CreateRestSessionRequestFactory(),
                            new LoginRequest {
                                ClientId = Credential.ClientId,
                                ClientSecret = Credential.ClientSecret,
                            }
                        ).Invoke();

                        Credential.ProjectToken = result.AccessToken;
                        OwnerId = result.OwnerId;
                    }
                    catch
                    {
                        this.State = State.Closed;
                        throw;
                    }
                }
                this.State = State.Available;

                return new OpenResult();
            }
            finally {
                this._semaphore.Release();
            }
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Open(UnityAction<AsyncResult<OpenResult>> callback) => OpenImplAsync().ToCoroutine(callback);
#else
        public IEnumerator Open(Action<AsyncResult<OpenResult>> callback) => OpenImplAsync().ToCoroutine(callback);
#endif
        
        public Gs2Future<OpenResult> OpenFuture() => OpenImplAsync().ToGs2Future();
        
#if GS2_ENABLE_UNITASK
        public UniTask<OpenResult> OpenAsync() => OpenImplAsync().AsUniTask<OpenResult>();
#else
        public Task<OpenResult> OpenAsync() => OpenImplAsync().AsTask();
#endif
        
        // ReOpen
        
        
#if GS2_ENABLE_UNITASK
        private async UniTask<OpenResult> ReOpenImplAsync()
#else
        private async Task<OpenResult> ReOpenImplAsync()
#endif
        {
            if (this.State == State.Opening || this.State == State.LoggingIn) {
                var begin = DateTime.Now;
                while (this.State != State.Available) {
                    if ((DateTime.Now - begin).Seconds > OpenTimeoutSec) {
                        throw new RequestTimeoutException(Array.Empty<RequestError>());
                    }

                    await TaskUtilities.Yield();
                }
            }

            return await OpenAsync();
        }

#if UNITY_2017_1_OR_NEWER
        public IEnumerator ReOpen(UnityAction<AsyncResult<OpenResult>> callback) => ReOpenImplAsync().ToCoroutine(callback);
#else
        public IEnumerator ReOpen(Action<AsyncResult<OpenResult>> callback) => ReOpenImplAsync().ToCoroutine(callback);
#endif
        
        // ReSharper disable once MemberCanBePrivate.Global
        public Gs2Future<OpenResult> ReOpenFuture() => ReOpenImplAsync().ToGs2Future();
        
        // ReSharper disable once MemberCanBePrivate.Global
#if GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public UniTask<OpenResult> ReOpenAsync() => ReOpenImplAsync().AsUniTask<OpenResult>();
    #else
        public Task<OpenResult> ReOpenAsync() => ReOpenImplAsync().AsTask();
    #endif
#elif !UNITY_2017_1_OR_NEWER
        public Task<OpenResult> ReOpenAsync() => ReOpenImplAsync();
#endif
        
        // Close
        
#if GS2_ENABLE_UNITASK
        private async UniTask CloseImplAsync()
#else
        private async Task CloseImplAsync()
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
                        if ((DateTime.Now - begin).Seconds > CloseTimeoutSec) {
                            this._inflightRequest.Clear();
                            break;
                        }

                        await TaskUtilities.Yield();
                    }
                }

                this.State = State.Closing;

                this.State = State.Closed;
            }
        }
        
#if UNITY_2017_1_OR_NEWER
        public IEnumerator Close(UnityAction callback) => CloseImplAsync().ToCoroutine(callback);
#else
        public IEnumerator Close(Action callback) => CloseImplAsync().ToCoroutine(callback);
#endif
        
        public Gs2Future CloseFuture() => CloseImplAsync().ToGs2Future();
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public UniTask CloseAsync() => CloseImplAsync().AsUniTask();
#else
        public Task CloseAsync() => CloseImplAsync().AsTask();
#endif
        
        // Send
        
#if GS2_ENABLE_UNITASK
        protected virtual async UniTask SendImplAsync(IGs2SessionRequest request)
#else
        protected virtual async Task SendImplAsync(IGs2SessionRequest request)
#endif
        {
            if (request is RestSessionRequest sessionRequest) {
                this._inflightRequest[sessionRequest.TaskId] = sessionRequest;

                this._result[sessionRequest.TaskId] = await sessionRequest.Invoke();
            }
        }

        public IEnumerator Send(IGs2SessionRequest request) => SendImplAsync(request).ToCoroutine((Action)null);
        
#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public UniTask SendAsync(IGs2SessionRequest request) => SendImplAsync(request).AsUniTask();
#else
        public Task SendAsync(IGs2SessionRequest request) => SendImplAsync(request).AsTask();
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
#if UNITY_2017_1_OR_NEWER
            this._inflightRequest.Remove(request.TaskId);
            this._result.Remove(request.TaskId);
#else
            this._inflightRequest.Remove(request.TaskId, out var inflightRequestvalue);
            this._result.Remove(request.TaskId, out var resultValue);
#endif
            return result;
        }
    }
}