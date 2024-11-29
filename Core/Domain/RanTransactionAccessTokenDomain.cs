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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class RanTransactionAccessTokenDomain : TransactionAccessTokenDomain
    {
        private readonly string _transactionId;

        public RanTransactionAccessTokenDomain(
            Gs2 gs2,
            string transactionId,
            TransactionResult result
        ): base(
            gs2,
            null,
            null
        ) {
            this._transactionId = transactionId;
            HandleResult(result);
        }

        private TransactionAccessTokenDomain HandleResult(
            TransactionResult result
        ) {
            if (result.ConsumeResults != null) {
                for (var i = 0; i < result.ConsumeResults.Length; i++) {
                    var consumeActionResult = result.ConsumeResults[i];
                    if (consumeActionResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(consumeActionResult.ConsumeResult, consumeActionResult.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration.ConsumeActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId + "[" + i + "]",
                        consumeActionResult.Action,
                        consumeActionResult.ConsumeRequest,
                        consumeActionResult.ConsumeResult
                    );
                }
            }

            if (result.AcquireResults != null) {
                for (var i = 0; i < result.AcquireResults.Length; i++) {
                    var acquireResult = result.AcquireResults[i];
                    if (acquireResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(acquireResult.AcquireResult, acquireResult.StatusCode ?? 999);
                    }

                    Gs2.TransactionConfiguration.AcquireActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId,
                        acquireResult.Action,
                        acquireResult.AcquireRequest,
                        acquireResult.AcquireResult
                    );

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
                }
            }
            return null;
        }


#if UNITY_2017_1_OR_NEWER
        public override IFuture<TransactionAccessTokenDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionAccessTokenDomain> self) {
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
            return null;
        }
#endif
    }
}
