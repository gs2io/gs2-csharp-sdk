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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class AutoJobQueueDomain : TransactionDomain
    {
        private static Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly string _namespaceName;
        private readonly string _jobName;

        public AutoJobQueueDomain(
            Gs2 gs2,
            string userId,
            string namespaceName,
            string jobName
        ): base(
            gs2,
            userId,
            null
        ) {
            this._namespaceName = namespaceName;
            this._jobName = jobName;
        }

        private TransactionDomain HandleResult(
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
                    null,
                    job,
                    result
                );
            }
            
            var nextTransactions = new List<TransactionDomain>();
            JsonData resultJson = null!;
            if (job.ScriptId.EndsWith("push_by_user_id")) {
                resultJson = JsonMapper.ToObject(result.Result);
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    UserId,
                    PushByUserIdResult.FromJson(resultJson)
                ));
            }
            
            resultJson = resultJson ?? TryParseObjectResult(result.Result);
            if (resultJson != null && resultJson.ContainsKey("autoRunStampSheet")) {
                nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                    Gs2,
                    UserId,
                    resultJson.ContainsKey("autoRunStampSheet") && bool.Parse(resultJson["autoRunStampSheet"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionId") ? resultJson["transactionId"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheet") ? resultJson["stampSheet"]?.ToString() : null,
                    resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null,
                    resultJson.ContainsKey("atomicCommit") && bool.Parse(resultJson["atomicCommit"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionResult") && resultJson["transactionResult"] != null ? TransactionResult.FromJson(JsonMapper.ToObject(resultJson["transactionResult"].ToString())) : null,
                    resultJson.ContainsKey("metadata") && resultJson["metadata"] != null ? ResultMetadata.FromJson(JsonMapper.ToObject(resultJson["metadata"].ToJson())) : null
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

        private static JsonData TryParseObjectResult(
            string result
        ) {
            var trimmed = result?.TrimStart();
            if (string.IsNullOrEmpty(trimmed) || trimmed[0] != '{') {
                return null!;
            }
            return JsonMapper.ToObject(result);
        }

#if UNITY_2017_1_OR_NEWER
        public override IFuture<TransactionDomain> WaitFuture(
            bool all = false
        ) => WaitAsync(all).ToGs2Future();
#endif
        
#if GS2_ENABLE_UNITASK
        public override async UniTask<TransactionDomain> WaitAsync(
#else
        public override async Task<TransactionDomain> WaitAsync(
#endif
            bool all = false
        ) {
            var begin = DateTime.Now;
            RETRY:
            if (DateTime.Now - begin > TimeSpan.FromSeconds(10)) {
                throw new TimeoutException("Failed to retrieve the results of the Job Queue execution: either there is some kind of failure in GS2, or the GS2-Gateway used for notification has not been configured for GS2-JobQueue used to execute the Job Queue, or the GS2-Gateway has a user ID setting to receive notifications. API may not have been invoked.");
            }
            var domain = new Gs2JobQueue.Domain.Gs2JobQueue(
                Gs2
            ).Namespace(
                this._namespaceName
            ).User(
                UserId
            ).Job(
                this._jobName
            ).JobResult();
            var result = await domain.ModelAsync();
            if (result == null) {
                domain.Invalidate();
                await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                await Gs2.JobQueue.DispatchByUserIdAsync(UserId);
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
    }
}
