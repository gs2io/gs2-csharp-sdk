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
using Gs2.Core.Exception;
using Gs2.Core.Net;
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
    public partial class AutoJobQueueDomain : TransactionDomain
    {
        private readonly string _jobId;

        public AutoJobQueueDomain(
            Gs2 gs2,
            string userId,
            string jobId
        ): base(
            gs2,
            userId,
            null
        ) {
            this._jobId = jobId;
        }

        private TransactionDomain HandleResult(
            Job job,
            JobResult result,
            JobResultBody resultBody
        ) {
            Gs2.UpdateCacheFromJobResult(
                job,
                resultBody
            );

            var nextTransactions = new List<TransactionDomain>();
            if (result.ScriptId.EndsWith("push_by_user_id")) {
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    UserId,
                    PushByUserIdResult.FromJson(JsonMapper.ToObject(result.Result))
                ));
            }
            
            var resultJson = JsonMapper.ToObject(result.Result);
            if (resultJson.ContainsKey("autoRunStampSheet")) {
                nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                    Gs2,
                    UserId,
                    resultJson.ContainsKey("autoRunStampSheet") && bool.Parse(resultJson["autoRunStampSheet"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionId") ? resultJson["transactionId"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheet") ? resultJson["stampSheet"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null
                ));
            }

            if (nextTransactions.Count > 0) {
                return new TransactionDomain(
                    Gs2,
                    UserId,
                    nextTransactions
                );
            }

            return null;
        }

#if UNITY_2017_1_OR_NEWER
        public override IFuture<TransactionDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionDomain> self) {
                RETRY:
            
                var future = new Gs2JobQueue.Domain.Gs2JobQueue(
                    Gs2
                ).Namespace(
                    Job.GetNamespaceNameFromGrn(this._jobId)
                ).User(
                    UserId
                ).Job(
                    Job.GetJobNameFromGrn(this._jobId)
                ).JobResult().ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                if (result == null) {
                    yield return new WaitForSeconds(0.01f);
                    
                    var future2 = Gs2.DispatchByUserIdFuture(UserId);
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
                    result,
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
            return new Gs2InlineFuture<TransactionDomain>(Impl);
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK

    #if UNITY_2017_1_OR_NEWER
        public override async UniTask<TransactionDomain> WaitAsync(
    #else
        public override async Task<TransactionDomain> WaitAsync(
    #endif
            bool all = false
        ) {
            RETRY:
            
            var result = await new Gs2JobQueue.Domain.Gs2JobQueue(
                Gs2
            ).Namespace(
                Job.GetNamespaceNameFromGrn(this._jobId)
            ).User(
                UserId
            ).Job(
                Job.GetJobNameFromGrn(this._jobId)
            ).JobResult().ModelAsync();
            if (result == null) {
#if UNITY_2017_1_OR_NEWER
                await UniTask.Delay(TimeSpan.FromMilliseconds(10));
#else
                await Task.Delay(TimeSpan.FromMilliseconds(10));
#endif
                await Gs2.DispatchByUserIdAsync(UserId);
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
                result,
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
#endif
    }
}
