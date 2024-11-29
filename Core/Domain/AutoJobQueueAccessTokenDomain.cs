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
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Exception;
using Gs2.Core.Model;
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
    public partial class AutoJobQueueAccessTokenDomain : TransactionAccessTokenDomain
    {
        private static Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly string _namespaceName;
        private readonly string _jobName;

        public AutoJobQueueAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            string namespaceName,
            string jobName
        ): base(
            gs2,
            accessToken,
            null
        ) {
            this._namespaceName = namespaceName;
            this._jobName = jobName;
        }

        private TransactionAccessTokenDomain HandleResult(
            Gs2JobQueue.Model.Job job,
            Gs2JobQueue.Model.JobResultBody result
        ) {
            if (result.StatusCode / 100 != 2) {
                throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 0);
            }

            var skipCallback = false;
            lock (_handled) {
                if (_handled.ContainsKey(this._jobName)) {
                    _handled = _handled
                        .Where(pair => pair.Value >= UnixTime.ToUnixTime(DateTime.Now))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                    skipCallback = true;
                }
                else {
                    _handled.Add(this._jobName, UnixTime.ToUnixTime(DateTime.Now.Add(TimeSpan.FromMinutes(3))));
                }
            }
            
            if (!skipCallback) {
                Gs2.UpdateCacheFromJobResult(
                    job,
                    new JobResultBody {
                        TryNumber = result.TryNumber,
                        StatusCode = result.StatusCode,
                        Result = result.Result,
                    }
                );
            }
            
            var nextTransactions = new List<TransactionAccessTokenDomain>();
            if (job.ScriptId.EndsWith("push_by_user_id")) {
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    AccessToken,
                    PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result))
                ));
            }
            
            var resultJson = JsonMapper.ToObject(result.Result);
            if (resultJson.ContainsKey("autoRunStampSheet")) {
                nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                    Gs2,
                    AccessToken,
                    resultJson.ContainsKey("autoRunStampSheet") && bool.Parse(resultJson["autoRunStampSheet"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionId") ? resultJson["transactionId"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheet") ? resultJson["stampSheet"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null,
                    resultJson.ContainsKey("atomicCommit") && bool.Parse(resultJson["atomicCommit"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionResult") && resultJson["transactionResult"] != null ? TransactionResult.FromJson(JsonMapper.ToObject(resultJson["transactionResult"].ToString())) : null
                ));
            }

            if (nextTransactions.Count > 0) {
                return new TransactionAccessTokenDomain(
                    Gs2,
                    AccessToken,
                    nextTransactions
                );
            }

            return null;
        }

#if UNITY_2017_1_OR_NEWER
        public override IFuture<TransactionAccessTokenDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionAccessTokenDomain> self) {
                var begin = DateTime.Now;
                RETRY:
                if (DateTime.Now - begin > TimeSpan.FromSeconds(10)) {
                    self.OnError(new UnknownException("Failed to retrieve the results of the Job Queue execution: either there is some kind of failure in GS2, or the GS2-Gateway used for notification has not been configured for GS2-JobQueue used to execute the Job Queue, or the GS2-Gateway has a user ID setting to receive notifications. API may not have been invoked."));
                    yield break;
                }
                var domain = Gs2.JobQueue.Namespace(
                    this._namespaceName
                ).AccessToken(
                    AccessToken
                ).Job(
                    this._jobName
                ).JobResult();
                var future = domain.ModelFuture();
                yield return future;
                if (future.Error != null) {
                    domain.Invalidate();
                    if (!future.Error.RecommendAutoRetry) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    yield return new WaitForSeconds(0.01f);
                    goto RETRY;
                }
                var result = future.Result;
                if (result == null) {
                    yield return new WaitForSeconds(0.01f);
                    
                    var future2 = Gs2.JobQueue.DispatchFuture(AccessToken);
                    yield return future2;
                    if (future2.Error != null) {
                        self.OnError(future2.Error);
                        yield break;
                    }
                    
                    goto RETRY;
                }

                var transaction = HandleResult(
                    new Job {
                        JobId = result.JobId,
                        Name = Job.GetJobNameFromGrn(result.JobId),
                        UserId = Job.GetUserIdFromGrn(result.JobId),
                        ScriptId = result.ScriptId,
                        Args = result.Args,
                    },
                    new JobResultBody {
                        TryNumber = result.TryNumber,
                        StatusCode = result.StatusCode,
                        Result = result.Result,
                    }
                );
                if (all && transaction != null) {
                    var future3 = transaction.WaitFuture(true);
                    yield return future3;
                    if (future3.Error != null) {
                        self.OnError(future3.Error);
                        yield break;
                    }
                    self.OnComplete(null);
                    yield return null;
                }
                self.OnComplete(transaction);
                yield return null;
            }
            return new Gs2InlineFuture<TransactionAccessTokenDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public override async UniTask<TransactionAccessTokenDomain> WaitAsync(
    #else
        public override async Task<TransactionAccessTokenDomain> WaitAsync(
    #endif
            bool all = false
        ) {
            var begin = DateTime.Now;
            RETRY:
            if (DateTime.Now - begin > TimeSpan.FromSeconds(10)) {
                throw new TimeoutException("Failed to retrieve the results of the Job Queue execution: either there is some kind of failure in GS2, or the GS2-Gateway used for notification has not been configured for GS2-JobQueue used to execute the Job Queue, or the GS2-Gateway has a user ID setting to receive notifications. API may not have been invoked.");
            }
            var domain = Gs2.JobQueue.Namespace(
                this._namespaceName
            ).AccessToken(
                AccessToken
            ).Job(
                this._jobName
            ).JobResult();
            try {
                var result = await domain.ModelAsync();
                if (result == null) {
#if UNITY_2017_1_OR_NEWER
                    await UniTask.Delay(TimeSpan.FromMilliseconds(10));
#else
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
#endif
                    await Gs2.JobQueue.DispatchAsync(AccessToken);
                    goto RETRY;
                }

                var transaction = HandleResult(
                    new Job {
                        JobId = result.JobId,
                        Name = Job.GetJobNameFromGrn(result.JobId),
                        UserId = Job.GetUserIdFromGrn(result.JobId),
                        ScriptId = result.ScriptId,
                        Args = result.Args,
                    },
                    new JobResultBody {
                        TryNumber = result.TryNumber,
                        StatusCode = result.StatusCode,
                        Result = result.Result,
                    }
                );
                if (all && transaction != null) {
                    return await transaction.WaitAsync(true);
                }
                return transaction;
            }
            catch (Gs2Exception e) {
                domain.Invalidate();
                if (!e.RecommendAutoRetry) {
                    throw;
                }
#if UNITY_2017_1_OR_NEWER
                await UniTask.Delay(TimeSpan.FromMilliseconds(10));
#else
                await Task.Delay(TimeSpan.FromMilliseconds(10));
#endif
                goto RETRY;
            }
        }
#endif
    }
}
