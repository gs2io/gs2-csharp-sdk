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
    public partial class ManualJobQueueAccessTokenDomain : TransactionAccessTokenDomain
    {
        private static readonly TimeSpan RunRetryTimeout = TimeSpan.FromSeconds(15);
        private static Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly string _namespaceName;
        private readonly string _jobName;
        public string JobName => _jobName;

        public ManualJobQueueAccessTokenDomain(
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
                    this.AccessToken?.TimeOffset,
                    job,
                    new JobResultBody {
                        TryNumber = result.TryNumber,
                        StatusCode = result.StatusCode,
                        Result = result.Result,
                    }
                );
            }
            
            var nextTransactions = new List<TransactionAccessTokenDomain>();
            JsonData resultJson = null!;
            if (job.ScriptId.EndsWith("push_by_user_id")) {
                resultJson = JsonMapper.ToObject(result.Result);
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    AccessToken,
                    PushByUserIdResult.FromJson(resultJson)
                ));
            }
            
            resultJson = resultJson ?? TryParseObjectResult(result.Result);
            if (resultJson != null && resultJson.ContainsKey("autoRunStampSheet")) {
                nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                    Gs2,
                    AccessToken,
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
                return new TransactionAccessTokenDomain(
                    Gs2,
                    AccessToken,
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
        public override IFuture<TransactionAccessTokenDomain> WaitFuture(
            bool all = false
        ) => WaitAsync(all).ToGs2Future();
#endif
        
#if GS2_ENABLE_UNITASK
        public override async UniTask<TransactionAccessTokenDomain> WaitAsync(
#else
        public override async Task<TransactionAccessTokenDomain> WaitAsync(
#endif
            bool all = false
        ) {
            var begin = DateTime.Now;
            RETRY:
            global::Gs2.Gs2JobQueue.Domain.Model.JobAccessTokenDomain result;
            try {
                result = await new Gs2JobQueue.Domain.Gs2JobQueue(
                    Gs2
                ).Namespace(
                    this._namespaceName
                ).AccessToken(
                    AccessToken
                ).RunAsync(
                    new RunRequest()
                );
            }
            catch (Gs2Exception e) {
                if (!e.RecommendAutoRetry || DateTime.Now - begin > RunRetryTimeout) {
                    throw;
                }
                await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                goto RETRY;
            }
            var job = result.Item;
            if (job == null) {
                return null;
            }
            if (job.Name != this._jobName) {
                HandleResult(job, result.Result);
                if (result.IsLastJob ?? true) {
                    return null;
                }
                goto RETRY;
            }

            var transaction = HandleResult(job, result.Result);
            if (all && transaction != null) {
                return await transaction.WaitAsync(true);
            }
            return transaction;
        }
    }
}
