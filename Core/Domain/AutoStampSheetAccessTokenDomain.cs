using System.Collections;
using System.Threading.Tasks;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Distributor.Domain.Model;
using Gs2.Gs2Distributor.Model;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public class AutoStampSheetAccessTokenDomain
    {
        private readonly Gs2 _gs2;
        private readonly AccessToken _accessToken;
        private readonly string _transactionId;

        public AutoStampSheetAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            string transactionId
        )
        {
            this._gs2 = gs2;
            this._accessToken = accessToken;
            this._transactionId = transactionId;
        }
        
#if UNITY_2017_1_OR_NEWER
        public Gs2Future RunFuture()
        {
            IEnumerator Impl(Gs2Future self)
            {
                if (this._gs2.TransactionConfiguration?.NamespaceName == null) {
                    yield break;
                }
                this._gs2.Cache.Delete<StampSheetResult>(
                    UserDomain.CreateCacheParentKey(
                        this._gs2.TransactionConfiguration.NamespaceName,
                        this._accessToken.UserId,
                        "StampSheetResult"
                    ),
                    StampSheetResultDomain.CreateCacheKey(
                        this._transactionId?.ToString()
                    )
                );
                var future = this._gs2.Distributor.Namespace(
                    this._gs2.TransactionConfiguration.NamespaceName
                ).AccessToken(
                    this._accessToken
                ).StampSheetResult(
                    this._transactionId
                ).ModelFuture();
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                if (result.TaskRequests != null) {
                    for (var i = 0; i < result.TaskRequests.Length; i++) {
                        var stampTask = result.TaskRequests[i];
                        if (i < result.TaskResults.Length) {
                            this._gs2.TransactionConfiguration.StampTaskEventHandler.Invoke(
                                this._gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                stampTask.Action,
                                stampTask.Request,
                                result.TaskResults[i]
                            );
                        }
                    }
                }

                this._gs2.TransactionConfiguration.StampSheetEventHandler.Invoke(
                    this._gs2.Cache,
                    this._transactionId,
                    result.SheetRequest.Action,
                    result.SheetRequest.Request,
                    result.SheetResult
                );
                var requestJson = JsonMapper.ToObject(result.SheetRequest.Request.ToString());

                if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId" && result.SheetResult != null) {
                    var jobResult = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                    var autoRun = jobResult?.AutoRun;
                    if (autoRun != null && !autoRun.Value)
                    {
                        this._gs2.PushJobQueue(
                            requestJson["namespaceName"].ToString()
                        );
                    }
                }
            }
            return new Gs2InlineFuture(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask RunAsync()
    #else
        public async Task RunAsync()
    #endif
        {
            if (this._gs2.TransactionConfiguration?.NamespaceName == null) {
                return;
            }
            this._gs2.Cache.Delete<StampSheetResult>(
                UserDomain.CreateCacheParentKey(
                    this._gs2.TransactionConfiguration.NamespaceName,
                    this._accessToken.UserId,
                    "StampSheetResult"
                ),
                StampSheetResultDomain.CreateCacheKey(
                    this._transactionId?.ToString()
                )
            );
            var result = await this._gs2.Distributor.Namespace(
                this._gs2.TransactionConfiguration.NamespaceName
            ).AccessToken(
                this._accessToken
            ).StampSheetResult(
                this._transactionId
            ).ModelAsync();
            if (result != null) {
                if (result.TaskRequests != null) {
                    for (var i = 0; i < result.TaskRequests.Length; i++) {
                        var stampTask = result.TaskRequests[i];
                        if (i < result.TaskResults.Length) {
                            this._gs2.TransactionConfiguration.StampTaskEventHandler.Invoke(
                                this._gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                stampTask.Action,
                                stampTask.Request,
                                result.TaskResults[i]
                            );
                        }
                    }
                }

                this._gs2.TransactionConfiguration.StampSheetEventHandler.Invoke(
                    this._gs2.Cache,
                    this._transactionId,
                    result.SheetRequest.Action,
                    result.SheetRequest.Request,
                    result.SheetResult
                );
                var requestJson = JsonMapper.ToObject(result.SheetRequest.Request.ToString());

                if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId" && result.SheetResult != null) {
                    var jobResult = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult));
                    var autoRun = jobResult?.AutoRun;
                    if (autoRun != null && !autoRun.Value) {
                        this._gs2.PushJobQueue(
                            requestJson["namespaceName"].ToString()
                        );
                    }
                }
            }
        }
#endif
    }
}