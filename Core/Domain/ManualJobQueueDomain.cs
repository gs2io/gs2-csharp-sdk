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
    public partial class ManualJobQueueDomain : TransactionDomain
    {
        private static Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly string _namespaceName;
        private readonly string _jobName;

        public ManualJobQueueDomain(
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
                    job,
                    result
                );
            }
            
            var nextTransactions = new List<TransactionDomain>();
            if (job.ScriptId.EndsWith("push_by_user_id")) {
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
                    resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null,
                    resultJson.ContainsKey("atomicCommit") && bool.Parse(resultJson["atomicCommit"]?.ToString() ?? "false"),
                    resultJson.ContainsKey("transactionResult") && resultJson["transactionResult"] != null ? TransactionResult.FromJson(JsonMapper.ToObject(resultJson["transactionResult"].ToString())) : null
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
                var future = Gs2.JobQueue.Namespace(
                    this._namespaceName
                ).User(
                    UserId
                ).RunFuture(
                    new RunByUserIdRequest()
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                if (result.IsLastJob ?? true) {
                    self.OnComplete(null);
                    yield break;
                }
                var future2 = result.ModelFuture();
                yield return future2;
                if (future2.Error != null) {
                    self.OnError(future2.Error);
                    yield break;
                }
                var job = future2.Result;
                if (job == null) {
                    self.OnComplete(null);
                    yield break;
                }
                if (job.Name != this._jobName) {
                    HandleResult(job, result.Result);
                    goto RETRY;
                }

                var transaction = HandleResult(job, result.Result);
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
                this._namespaceName
            ).User(
                UserId
            ).RunAsync(
                new RunByUserIdRequest()
            );
            if (result.IsLastJob ?? true) {
                return null;
            }
            var job = await result.ModelAsync();
            if (job == null) {
                return null;
            }
            if (job.Name != this._jobName) {
                HandleResult(job, result.Result);
                goto RETRY;
            }

            var transaction = HandleResult(job, result.Result);
            if (all && transaction != null) {
                return await transaction.WaitAsync(true);
            }
            return transaction;
        }
#endif
    }
}
