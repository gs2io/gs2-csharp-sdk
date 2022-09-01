using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Net;
using Gs2.Gs2Distributor;
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
        private readonly string _namespaceName;
        private readonly string _transactionId;
        private readonly string _accessToken;
        private readonly Action<CacheDatabase, string, string, string> _stampTaskEvent;
        private readonly Action<CacheDatabase, string, string, string> _stampSheetEvent;

        public AutoStampSheetDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            Gs2RestSession session,
            string namespaceName,
            string transactionId,
            string accessToken,
            Action<CacheDatabase, string, string, string> stampTaskEvent,
            Action<CacheDatabase, string, string, string> stampSheetEvent
        )
        {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._session = session;
            this._namespaceName = namespaceName;
            this._transactionId = transactionId;
            this._accessToken = accessToken;
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
                var client = new Gs2DistributorRestClient(
                    _session
                );
#if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = client.GetStampSheetResultFuture(
                    new GetStampSheetResultRequest()
                        .WithNamespaceName(_namespaceName)
                        .WithTransactionId(_transactionId)
                        .WithAccessToken(_accessToken)
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
#else
                var result = await client.GetStampSheetResultAsync(
                    new GetStampSheetResultRequest()
                        .WithNamespaceName(_namespaceName)
                        .WithTransactionId(_transactionId)
                        .WithAccessToken(_accessToken)
                );
#endif
                for (var i = 0; i < result.Item.TaskRequests.Length; i++)
                {
                    var stampTask = result.Item.TaskRequests[i];
                    _stampTaskEvent.Invoke(
                        _cache,
                        stampTask.Action,
                        stampTask.Request,
                        result.Item.TaskResults[i]
                    );
                }

                string action = null;
                JsonData requestJson = null;
                _stampSheetEvent.Invoke(
                    _cache,
                    result.Item.SheetRequest.Action,
                    result.Item.SheetRequest.Request,
                    result.Item.SheetResult
                );
                requestJson = JsonMapper.ToObject(result.Item.SheetRequest.Request.ToString());

                if (result.Item.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                {
                    var autoRun = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Item.SheetResult)).AutoRun;
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