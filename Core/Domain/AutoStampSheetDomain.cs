using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Net;
using Gs2.Gs2Distributor;
using Gs2.Gs2Distributor.Domain.Model;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public class AutoStampSheetDomain
    {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly Gs2RestSession _session;
        private readonly UserAccessTokenDomain _userAccessTokenDomain;
        private readonly string _transactionId;
        private readonly Action<CacheDatabase, string, string, string, string> _stampTaskEvent;
        private readonly Action<CacheDatabase, string, string, string, string> _stampSheetEvent;

        public AutoStampSheetDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            UserAccessTokenDomain userAccessTokenDomain,
            Gs2RestSession session,
            string transactionId,
            Action<CacheDatabase, string, string, string, string> stampTaskEvent,
            Action<CacheDatabase, string, string, string, string> stampSheetEvent
        )
        {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._session = session;
            this._userAccessTokenDomain = userAccessTokenDomain;
            this._transactionId = transactionId;
            this._stampTaskEvent = stampTaskEvent;
            this._stampSheetEvent = stampSheetEvent;
        }
        
#if UNITY_2017_1_OR_NEWER
    #if GS2_ENABLE_UNITASK
        public async UniTask RunAsync()
    #else
        public Gs2Future Run()
    #endif
#else
        public async Task RunAsync()
#endif
        {
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(Gs2Future self)
            {
#endif
                _cache.Delete<Gs2Distributor.Model.StampSheetResult>(
                    Gs2Distributor.Domain.Model.UserDomain.CreateCacheParentKey(
                        _userAccessTokenDomain.NamespaceName,
                        _userAccessTokenDomain.UserId,
                        "StampSheetResult"
                    ),
                    Gs2Distributor.Domain.Model.StampSheetResultDomain.CreateCacheKey(
                        _transactionId?.ToString()
                    )
                );
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = _userAccessTokenDomain.StampSheetResult(
                    _transactionId
                ).Model();
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
#else
                var result = await _userAccessTokenDomain.StampSheetResult(
                    _transactionId
                ).Model();
#endif
                if (result.TaskRequests != null) {
                    for (var i = 0; i < result.TaskRequests.Length; i++) {
                        var stampTask = result.TaskRequests[i];
                        if (i < result.TaskResults.Length) {
                            _stampTaskEvent.Invoke(
                                _cache,
                                _transactionId + "[" + i + "]",
                                stampTask.Action,
                                stampTask.Request,
                                result.TaskResults[i]
                            );
                        }
                    }
                }

            string action = null;
                JsonData requestJson = null;
                _stampSheetEvent.Invoke(
                    _cache,
                    _transactionId,
                    result.SheetRequest.Action,
                    result.SheetRequest.Request,
                    result.SheetResult
                );
                requestJson = JsonMapper.ToObject(result.SheetRequest.Request.ToString());

                if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId" && result.SheetResult != null) {
                    var jobResult = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                    var autoRun = jobResult?.AutoRun;
                    if (autoRun != null && !autoRun.Value)
                    {
                        Gs2.PushJobQueue(
                            _jobQueueDomain,
                            requestJson["namespaceName"].ToString()
                        );
                    }
                }
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture(Impl);
#endif
        }
    }
}