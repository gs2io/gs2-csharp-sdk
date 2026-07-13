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
    public partial class AutoTransactionAccessTokenDomain : TransactionAccessTokenDomain
    {
        private static Dictionary<string, long> _handled = new Dictionary<string, long>();
        private readonly string _transactionId;
        private readonly string _namespaceName;
        public string TransactionId => _transactionId;

        public AutoTransactionAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            string transactionId
        ): this(
            gs2,
            accessToken,
            transactionId,
            null
        ) {
        }

        internal AutoTransactionAccessTokenDomain(
            Gs2 gs2,
            AccessToken accessToken,
            string transactionId,
            string namespaceName
        ): base(
            gs2,
            accessToken,
            null
        ) {
            this._transactionId = transactionId;
            this._namespaceName = namespaceName;
        }

        private TransactionAccessTokenDomain HandleResult(
            Gs2Distributor.Model.TransactionResult result
        ) {
            var skipCallback = false;
            lock (_handled) {
                if (_handled.ContainsKey(this._transactionId)) {
                    _handled = _handled
                        .Where(pair => pair.Value >= UnixTime.ToUnixTime(DateTime.Now))
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
                    skipCallback = true;
                }
                else {
                    _handled.Add(this._transactionId, UnixTime.ToUnixTime(DateTime.Now.Add(TimeSpan.FromMinutes(3))));
                }
            }
            
            if (result.VerifyResults != null) {
                for (var i = 0; i < result.VerifyResults.Length; i++) {
                    var consumeActionResult = result.VerifyResults[i];
                    if (consumeActionResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(consumeActionResult.VerifyResult, consumeActionResult.StatusCode ?? 999);
                    }
                }
            }

            if (result.ConsumeResults != null) {
                for (var i = 0; i < result.ConsumeResults.Length; i++) {
                    var consumeActionResult = result.ConsumeResults[i];
                    if (consumeActionResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(consumeActionResult.ConsumeResult, consumeActionResult.StatusCode ?? 999);
                    }
                }
            }
            if (result.AcquireResults != null) {
                for (var i = 0; i < result.AcquireResults.Length; i++) {
                    var acquireResult = result.AcquireResults[i];
                    if (acquireResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(acquireResult.AcquireResult, acquireResult.StatusCode ?? 999);
                    }
                }
            }

            if (result.VerifyResults != null) {
                for (var i = 0; i < result.VerifyResults.Length; i++) {
                    var consumeActionResult = result.VerifyResults[i];
                    if (!skipCallback) {
                        Gs2.TransactionConfiguration.VerifyActionEventHandler.Invoke(
                            Gs2.Cache,
                            this._transactionId + "[" + i + "]",
                            AccessToken?.TimeOffset,
                            consumeActionResult.Action,
                            consumeActionResult.VerifyRequest,
                            consumeActionResult.VerifyResult
                        );
                    }
                }
            }

            if (result.ConsumeResults != null) {
                for (var i = 0; i < result.ConsumeResults.Length; i++) {
                    var consumeActionResult = result.ConsumeResults[i];
                    if (!skipCallback) {
                        Gs2.TransactionConfiguration.ConsumeActionEventHandler.Invoke(
                            Gs2.Cache,
                            this._transactionId + "[" + i + "]",
                            AccessToken?.TimeOffset,
                            consumeActionResult.Action,
                            consumeActionResult.ConsumeRequest,
                            consumeActionResult.ConsumeResult
                        );
                    }
                }
            }

            if (result.AcquireResults != null) {
                for (var i = 0; i < result.AcquireResults.Length; i++) {
                    var acquireResult = result.AcquireResults[i];
                    if (!skipCallback) {
                        Gs2.TransactionConfiguration.AcquireActionEventHandler.Invoke(
                            Gs2.Cache,
                            this._transactionId,
                            AccessToken?.TimeOffset,
                            acquireResult.Action,
                            acquireResult.AcquireRequest,
                            acquireResult.AcquireResult
                        );
                    }

                    var nextTransactions = new List<TransactionAccessTokenDomain>();
                    if (acquireResult.Action == "Gs2JobQueue:PushByUserId") {
                        nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                            Gs2,
                            AccessToken,
                            PushByUserIdResult.FromJson(JsonMapper.ToObject(acquireResult.AcquireResult))
                        ));
                    }

                    var resultJson = JsonMapper.ToObject(acquireResult.AcquireResult);
                    if (resultJson.ContainsKey("autoRunStampSheet")) {
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
                }
            }
            return null;
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
            if (DateTime.Now - begin > TimeSpan.FromSeconds(10)) {
                throw new TimeoutException("Failed to retrieve transaction results, either because there is some failure in GS2, or the GS2-Gateway used to notify the GS2-Distributor used to execute the transaction is not yet configured, or the GS2-Gateway has a user ID to receive notifications The configuration API may not have been invoked.");
            }
            var domain = new Gs2Distributor.Domain.Gs2Distributor(
                Gs2
            ).Namespace(
                this._namespaceName ?? Gs2.TransactionConfiguration.NamespaceName ?? "default"
            ).AccessToken(
                AccessToken
            ).TransactionResult(
                this._transactionId
            );
            try {
                var result = await domain.ModelAsync();
                if (result == null) {
                    domain.Invalidate();
                    await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                    await Gs2.Distributor.DispatchAsync(AccessToken);
                    goto RETRY;
                }

                var transaction = HandleResult(result);
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
                await TaskUtilities.Yield();
                goto RETRY;
            }
        }
    }
}
