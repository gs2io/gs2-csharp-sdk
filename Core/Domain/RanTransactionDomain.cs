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
using Gs2.Gs2Distributor.Model.Cache;
using Gs2.Gs2Exchange.Result;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public class IssueTransactionApiResult : IResult
    {
        public Model.TransactionResult TransactionResult { set; get; }
        
        public IssueTransactionApiResult WithTransactionResult(Model.TransactionResult transactionResult) {
            this.TransactionResult = transactionResult;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
        [Preserve]
#endif
        public static IssueTransactionApiResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssueTransactionApiResult()
                .WithTransactionResult(!data.Keys.Contains("transactionResult") || data["transactionResult"] == null ? null : Model.TransactionResult.FromJson(data["transactionResult"]));
        }
    }
    
    public partial class RanTransactionDomain : TransactionDomain
    {
        private readonly string _transactionId;
        private readonly List<TransactionDomain> _nextTransactions = new();

        public RanTransactionDomain(
            Gs2 gs2,
            string transactionId,
            string userId,
            TransactionResult result,
            ResultMetadata metadata
        ): base(
            gs2,
            userId,
            null
        ) {
            this._transactionId = transactionId;
            HandleResult(result);
            HandleResult(metadata);
        }

        private void HandleResult(
            TransactionResult result
        ) {
            result.PutCache(
                Gs2.Cache,
                Gs2.TransactionConfiguration.NamespaceName,
                UserId,
                result.TransactionId,
                null
            );

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

            if (result.HasError ?? false) {
                if (result.AcquireResults != null) {
                    for (var i = 0; i < result.AcquireResults.Length; i++) {
                        var acquireResult = result.AcquireResults[i];
                        var res = IssueTransactionApiResult.FromJson(JsonMapper.ToObject(acquireResult.AcquireResult));
                        if (res.TransactionResult != null && (res.TransactionResult.HasError ?? false)) {
                            HandleResult(res.TransactionResult);
                        }
                    }
                }
                throw new UnknownException("Ran transaction failed.");
            }

            if (result.VerifyResults != null) {
                for (var i = 0; i < result.VerifyResults.Length; i++) {
                    var consumeActionResult = result.VerifyResults[i];
                    Gs2.TransactionConfiguration.VerifyActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId + "[" + i + "]",
                        null,
                        consumeActionResult.Action,
                        consumeActionResult.VerifyRequest,
                        consumeActionResult.VerifyResult
                    );
                }
            }

            if (result.ConsumeResults != null) {
                for (var i = 0; i < result.ConsumeResults.Length; i++) {
                    var consumeActionResult = result.ConsumeResults[i];
                    Gs2.TransactionConfiguration.ConsumeActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId + "[" + i + "]",
                        null,
                        consumeActionResult.Action,
                        consumeActionResult.ConsumeRequest,
                        consumeActionResult.ConsumeResult
                    );
                }
            }

            if (result.AcquireResults != null) {
                for (var i = 0; i < result.AcquireResults.Length; i++) {
                    var acquireResult = result.AcquireResults[i];
                    Gs2.TransactionConfiguration.AcquireActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId,
                        null,
                        acquireResult.Action,
                        acquireResult.AcquireRequest,
                        acquireResult.AcquireResult
                    );

                    if (acquireResult.Action == "Gs2JobQueue:PushByUserId") {
                        this._nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                            Gs2,
                            UserId,
                            PushByUserIdResult.FromJson(JsonMapper.ToObject(acquireResult.AcquireResult))
                        ));
                    }

                    var resultJson = JsonMapper.ToObject(acquireResult.AcquireResult);
                    if (resultJson.ContainsKey("autoRunStampSheet")) {
                        this._nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                            Gs2,
                            UserId,
                            resultJson.ContainsKey("autoRunStampSheet") && bool.Parse(resultJson["autoRunStampSheet"]?.ToString() ?? "false"),
                            resultJson.ContainsKey("transactionId") ? resultJson["transactionId"]?.ToString() : null,
                            resultJson.ContainsKey("stampSheet") ? resultJson["stampSheet"]?.ToString() : null,
                            resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null,
                            resultJson.ContainsKey("atomicCommit") && bool.Parse(resultJson["atomicCommit"]?.ToString() ?? "false"),
                            resultJson.ContainsKey("transactionResult") && resultJson["transactionResult"] != null ? TransactionResult.FromJson(JsonMapper.ToObject(resultJson["transactionResult"].ToJson())) : null,
                            resultJson.ContainsKey("metadata") && resultJson["metadata"] != null ? ResultMetadata.FromJson(JsonMapper.ToObject(resultJson["metadata"].ToJson())) : null
                        ));
                    }
                }
            }
        }

        private void HandleResult(
            ResultMetadata metadata
        ) {
            if (metadata?.ScriptTransactionResults != null) {
                foreach (var result in metadata.ScriptTransactionResults) {
                    if (result.TransactionResult?.VerifyResults != null) {
                        for (var i = 0; i < result.TransactionResult.VerifyResults.Length; i++) {
                            var consumeActionResult = result.TransactionResult.VerifyResults[i];
                            if (consumeActionResult.StatusCode / 100 != 2) {
                                throw Gs2Exception.ExtractError(consumeActionResult.VerifyResult, consumeActionResult.StatusCode ?? 999);
                            }
                            Gs2.TransactionConfiguration.VerifyActionEventHandler.Invoke(
                                Gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                null,
                                consumeActionResult.Action,
                                consumeActionResult.VerifyRequest,
                                consumeActionResult.VerifyResult
                            );
                        }
                    }

                    if (result.TransactionResult?.ConsumeResults != null) {
                        for (var i = 0; i < result.TransactionResult.ConsumeResults.Length; i++) {
                            var consumeActionResult = result.TransactionResult.ConsumeResults[i];
                            if (consumeActionResult.StatusCode / 100 != 2) {
                                throw Gs2Exception.ExtractError(consumeActionResult.ConsumeResult, consumeActionResult.StatusCode ?? 999);
                            }
                            Gs2.TransactionConfiguration.ConsumeActionEventHandler.Invoke(
                                Gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                null,
                                consumeActionResult.Action,
                                consumeActionResult.ConsumeRequest,
                                consumeActionResult.ConsumeResult
                            );
                        }
                    }

                    if (result.TransactionResult?.AcquireResults != null) {
                        for (var i = 0; i < result.TransactionResult.AcquireResults.Length; i++) {
                            var acquireResult = result.TransactionResult.AcquireResults[i];
                            if (acquireResult.StatusCode / 100 != 2) {
                                throw Gs2Exception.ExtractError(acquireResult.AcquireResult, acquireResult.StatusCode ?? 999);
                            }

                            Gs2.TransactionConfiguration.AcquireActionEventHandler.Invoke(
                                Gs2.Cache,
                                this._transactionId,
                                null,
                                acquireResult.Action,
                                acquireResult.AcquireRequest,
                                acquireResult.AcquireResult
                            );

                            if (acquireResult.Action == "Gs2JobQueue:PushByUserId") {
                                this._nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                                    Gs2,
                                    UserId,
                                    PushByUserIdResult.FromJson(JsonMapper.ToObject(acquireResult.AcquireResult))
                                ));
                            }

                            var resultJson = JsonMapper.ToObject(acquireResult.AcquireResult);
                            if (resultJson.ContainsKey("autoRunStampSheet")) {
                                this._nextTransactions.Add(TransactionDomainFactory.ToTransaction(
                                    Gs2,
                                    UserId,
                                    resultJson.ContainsKey("autoRunStampSheet") && bool.Parse(resultJson["autoRunStampSheet"]?.ToString() ?? "false"),
                                    resultJson.ContainsKey("transactionId") ? resultJson["transactionId"]?.ToString() : null,
                                    resultJson.ContainsKey("stampSheet") ? resultJson["stampSheet"]?.ToString() : null,
                                    resultJson.ContainsKey("stampSheetEncryptionKeyId") ? resultJson["stampSheetEncryptionKeyId"]?.ToString() : null,
                                    resultJson.ContainsKey("atomicCommit") && bool.Parse(resultJson["atomicCommit"]?.ToString() ?? "false"),
                                    resultJson.ContainsKey("transactionResult") && resultJson["transactionResult"] != null ? TransactionResult.FromJson(JsonMapper.ToObject(resultJson["transactionResult"].ToJson())) : null,
                                    resultJson.ContainsKey("metadata") && resultJson["metadata"] != null ? ResultMetadata.FromJson(JsonMapper.ToObject(resultJson["metadata"].ToJson())) : null
                                ));
                            }
                        }
                    }
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public override IFuture<TransactionDomain> WaitFuture(
            bool all = false
        ) {
            IEnumerator Impl(IFuture<TransactionDomain> self) {
                var nextTransactions = new List<TransactionDomain>();
                foreach (var transaction in _nextTransactions) {
                    var future = transaction.WaitFuture(all);
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    if (future.Result != null) {
                        nextTransactions.Add(future.Result);
                    }
                }
                if (nextTransactions.Count == 0) {
                    self.OnComplete(null);
                    yield break;
                }
                self.OnComplete(new TransactionDomain(
                    Gs2,
                    UserId,
                    nextTransactions
                ));
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
            var nextTransactions = new List<TransactionDomain>();
            foreach (var transaction in _nextTransactions) {
                var nextTransaction = await transaction.WaitAsync(all);
                if (nextTransaction != null) {
                    nextTransactions.Add(nextTransaction);
                }
            }
            if (nextTransactions.Count == 0) {
                return null;
            }
            return new TransactionDomain(
                Gs2,
                UserId,
                nextTransactions
            );
        }
#endif
    }
}
