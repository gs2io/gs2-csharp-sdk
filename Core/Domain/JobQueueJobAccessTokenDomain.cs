/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

using System;
using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
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

        private readonly Gs2 _gs2;
        private readonly AccessToken _accessToken;
        public bool AutoRunJobQueue;
        public string JobId;

        public JobQueueJobAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            bool autoRunJobQueue,
            string jobId
        ) {
            this._gs2 = gs2;
            this._accessToken = accessToken;
            this.AutoRunJobQueue = autoRunJobQueue;
            this.JobId = jobId;
        }
        
#if UNITY_2017_1_OR_NEWER
        public IFuture<JobQueueJobAccessTokenDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<JobQueueJobAccessTokenDomain> self) {
                var namespaceName = Job.GetNamespaceNameFromGrn(this.JobId);
                if (this.AutoRunJobQueue) {
                    RETRY:
                    var future = new Gs2JobQueue.Domain.Gs2JobQueue(
                        this._gs2
                    ).Namespace(
                        namespaceName
                    ).AccessToken(
                        this._accessToken
                    ).Job(
                        Job.GetJobNameFromGrn(this.JobId)
                    ).JobResult().ModelFuture();

                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;

                    if (result == null) {
                        yield return new WaitForSeconds(0.01f);
                        
                        var dispatchFuture = this._gs2.JobQueue.DispatchFuture(this._accessToken);
                        yield return dispatchFuture;
                        if (dispatchFuture.Error != null) {
                            self.OnError(dispatchFuture.Error);
                            yield break;
                        }
                        goto RETRY;
                    }

                    var resultJson = JsonMapper.ToObject(result.Result);
                    if (resultJson.ContainsKey("transactionId") && !string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                        var future2 = new TransactionAccessTokenDomain(
                            this._gs2,
                            this._accessToken,
                            resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                            !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                            !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                            !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                        ).WaitFuture(all);
                        yield return future2;
                        if (future2.Error != null) {
                            self.OnError(future2.Error);
                            yield break;
                        }
                        
                        var dispatchFuture = this._gs2.DispatchFuture(this._accessToken);
                        yield return dispatchFuture;
                        if (dispatchFuture.Error != null) {
                            self.OnError(dispatchFuture.Error);
                            yield break;
                        }
                    }
                    
                    if (result.ScriptId.EndsWith("push_by_user_id"))
                    {
                        var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                        if (all)
                        {
                            foreach (var job in result2.Items) {
                                var future3 = new JobQueueJobAccessTokenDomain(
                                    this._gs2,
                                    this._accessToken,
                                    result2.AutoRun ?? false,
                                    job.JobId
                                ).WaitFuture(all);
                                yield return future3;
                                if (future3.Error != null) {
                                    self.OnError(future3.Error);
                                    yield break;
                                }
                                
                                var dispatchFuture = this._gs2.DispatchFuture(this._accessToken);
                                yield return dispatchFuture;
                                if (dispatchFuture.Error != null) {
                                    self.OnError(dispatchFuture.Error);
                                    yield break;
                                }
                            }
                        }
                    }
                } else {
                    var dispatchFuture = this._gs2.DispatchFuture(this._accessToken);
                    yield return dispatchFuture;
                    if (dispatchFuture.Error != null) {
                        self.OnError(dispatchFuture.Error);
                        yield break;
                    }
                }
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
            bool all = false
        ) {
            var namespaceName = Job.GetNamespaceNameFromGrn(this.JobId);
            if (this.AutoRunJobQueue) {
                RETRY:
                var result = await new Gs2JobQueue.Domain.Gs2JobQueue(
                    this._gs2
                ).Namespace(
                    namespaceName
                ).AccessToken(
                    this._accessToken
                ).Job(
                    Job.GetJobNameFromGrn(this.JobId)
                ).JobResult().ModelAsync();
                if (result == null) {
#if UNITY_2017_1_OR_NEWER
                    await UniTask.Delay(TimeSpan.FromMilliseconds(10));
#else
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
#endif
                    await this._gs2.DispatchAsync(this._accessToken);
                    goto RETRY;
                }

                var resultJson = JsonMapper.ToObject(result.Result);
                if (resultJson.ContainsKey("transactionId") && !string.IsNullOrEmpty(resultJson["transactionId"]?.ToString())) {
                    await new TransactionAccessTokenDomain(
                        this._gs2,
                        this._accessToken,
                        resultJson.ContainsKey("autoRunStampSheet") && (resultJson["autoRunStampSheet"] ?? false).ToString().ToLower() == "true",
                        !resultJson.ContainsKey("transactionId") ? null : resultJson["transactionId"]?.ToString(),
                        !resultJson.ContainsKey("stampSheet") ? null : resultJson["stampSheet"]?.ToString(),
                        !resultJson.ContainsKey("stampSheetEncryptionKeyId") ? null : resultJson["stampSheetEncryptionKeyId"]?.ToString()
                    ).WaitAsync(all);
                    await this._gs2.DispatchAsync(this._accessToken);
                }
                
                if (result.ScriptId.EndsWith("push_by_user_id"))
                {
                    var result2 = PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    if (all)
                    {
                        foreach (var job in result2.Items) {
                            await new JobQueueJobAccessTokenDomain(
                                this._gs2,
                                this._accessToken,
                                result2.AutoRun ?? false,
                                job.JobId
                            ).WaitAsync(all);
                            await this._gs2.DispatchAsync(this._accessToken);
                        }
                    }
                }
            }
            else {
                await this._gs2.DispatchAsync(this._accessToken);
            }
            return this;
        }
#endif
    }
}
