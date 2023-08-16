using System;
using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class JobQueueJobAccessTokenDomain
    {

        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly AccessToken _accessToken;
        public bool AutoRunJobQueue;
        public string JobId;

        public JobQueueJobAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            AccessToken accessToken,
            bool autoRunJobQueue,
            string jobId
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._accessToken = accessToken;
            this.AutoRunJobQueue = autoRunJobQueue;
            this.JobId = jobId;
        }
        
#if UNITY_2017_1_OR_NEWER
        public IFuture<JobQueueJobAccessTokenDomain> Wait() {
            IEnumerator Impl(IFuture<JobQueueJobAccessTokenDomain> self) {
    #if GS2_ENABLE_UNITASK
                yield return WaitAsync().ToCoroutine(
                    v => { self.OnComplete(this); },
                    e => { self.OnError(e as Gs2Exception); }
                );
    #else
                if (this.AutoRunJobQueue) {
                    while (true) {
                        var future = new Gs2JobQueue.Domain.Gs2JobQueue(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session
                        ).Namespace(
                            this._stampSheetConfiguration.NamespaceName
                        ).AccessToken(
                            this._accessToken
                        ).Job(
                            this.JobId
                        ).JobResult().Model();

                        yield return future;
                        if (future.Error != null) {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;

                        if (result == null) {
                            yield return new WaitForSeconds(0.1f);
                            yield return Gs2JobQueue.Domain.Gs2JobQueue.Dispatch(
                                this._cache,
                                this._session,
                                this._accessToken
                            );
                            continue;
                        }

                        var resultJson = JsonMapper.ToObject(result.Result);
                        if (!string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                            var future2 = new TransactionAccessTokenDomain(
                                this._cache,
                                this._jobQueueDomain,
                                this._stampSheetConfiguration,
                                this._session,
                                this._accessToken,
                                resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                                !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                                !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                                !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                            ).Wait();
                            yield return future2;
                            if (future2.Error != null) {
                                self.OnError(future2.Error);
                                yield break;
                            }
                        }
                        
                        if (result.ScriptId.EndsWith("push_by_user_id"))
                        {
                            var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                            if (result2?.AutoRun != null && result2.AutoRun.Value)
                            {
                                foreach (var job in result2.Items) {
                                    var future3 = new JobQueueJobAccessTokenDomain(
                                        this._cache,
                                        this._jobQueueDomain,
                                        this._stampSheetConfiguration,
                                        this._session,
                                        this._accessToken,
                                        true,
                                        job.JobId
                                    ).Wait();
                                    yield return future3;
                                    if (future3.Error != null) {
                                        self.OnError(future3.Error);
                                        yield break;
                                    }
                                }
                            }
                        }
                    }
                    self.OnComplete(this);
                } else {
                    var namespaceName = Job.GetNamespaceNameFromGrn(this.JobId);
                    if (!string.IsNullOrEmpty(namespaceName)) {
                        while (true) {
                            var future = new Gs2JobQueue.Domain.Gs2JobQueue(
                                this._cache,
                                this._jobQueueDomain,
                                this._stampSheetConfiguration,
                                this._session
                            ).Namespace(
                                namespaceName
                            ).AccessToken(
                                this._accessToken
                            ).Run(
                                new RunRequest()
                            );
                            yield return future;
                            if (future.Error != null) {
                                self.OnError(future.Error);
                                yield break;
                            }
                            var result = future.Result;
                            
                            if (result.IsLastJob ?? true) {
                                break;
                            }
                        }
                        var future2 = new JobQueueJobAccessTokenDomain(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session,
                            this._accessToken,
                            true,
                            this.JobId
                        ).Wait();
                        yield return future2;
                        if (future2.Error != null) {
                            self.OnError(future2.Error);
                            yield break;
                        }
                    }
                }
    #endif
            }
            return new Gs2InlineFuture<JobQueueJobAccessTokenDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public async UniTask<JobQueueJobAccessTokenDomain> WaitAsync(
    #else
        public async Task<JobQueueJobAccessTokenDomain> WaitAsync(
    #endif
        ) {
            if (this.AutoRunJobQueue) {
                while (true) {
                    var result = await new Gs2JobQueue.Domain.Gs2JobQueue(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session
                    ).Namespace(
                        this._stampSheetConfiguration.NamespaceName
                    ).AccessToken(
                        this._accessToken
                    ).Job(
                        this.JobId
                    ).JobResult().Model();
                    if (result == null) {
#if UNITY_2017_1_OR_NEWER
                        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
#else
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
#endif
                        continue;
                    }

                    var resultJson = JsonMapper.ToObject(result.Result);
                    if (resultJson.ContainsKey("transactionId") && !string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                        await new TransactionAccessTokenDomain(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session,
                            this._accessToken,
                            resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                            !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                            !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                            !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                        ).WaitAsync();
                    }
                    
                    if (result.ScriptId.EndsWith("push_by_user_id"))
                    {
                        var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                        if (result2?.AutoRun != null && result2.AutoRun.Value)
                        {
                            foreach (var job in result2.Items) {
                                await new JobQueueJobAccessTokenDomain(
                                    this._cache,
                                    this._jobQueueDomain,
                                    this._stampSheetConfiguration,
                                    this._session,
                                    this._accessToken,
                                    true,
                                    job.JobId
                                ).WaitAsync();
                            }
                        }
                    }
                    
                    break;
                }
            }
            else {
                var namespaceName = Job.GetNamespaceNameFromGrn(this.JobId);
                if (!string.IsNullOrEmpty(namespaceName)) {
                    while (true) {
                        var result = await new Gs2JobQueue.Domain.Gs2JobQueue(
                            this._cache,
                            this._jobQueueDomain,
                            this._stampSheetConfiguration,
                            this._session
                        ).Namespace(
                            namespaceName
                        ).AccessToken(
                            this._accessToken
                        ).RunAsync(
                            new RunRequest()
                        );
                        if (result.IsLastJob ?? true) {
                            break;
                        }
                    }
                    await new JobQueueJobAccessTokenDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        this._accessToken,
                        true,
                        this.JobId
                    ).WaitAsync();
                }
            }
            return this;
        }
#endif
    }
}
