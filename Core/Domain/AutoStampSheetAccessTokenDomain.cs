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
using System.Diagnostics;
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
    public partial class AutoStampSheetAccessTokenDomain : TransactionAccessTokenDomain
    {
        private static readonly ExpiringHandledResultSet HandledResults = new ExpiringHandledResultSet(TimeSpan.FromMinutes(3));
        private readonly string _transactionId;
        private readonly string _namespaceName;
        public string TransactionId => _transactionId;

        public AutoStampSheetAccessTokenDomain(
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

        internal AutoStampSheetAccessTokenDomain(
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
            Gs2Distributor.Model.StampSheetResult result
        ) {
            var skipCallback = !HandledResults.TryHandle(this._transactionId);
            
            if (result.VerifyTaskRequests != null) {
                for (var i = 0; i < result.VerifyTaskRequests.Length; i++) {
                    var verifyTask = result.VerifyTaskRequests[i];
                    if (i < result.VerifyTaskResults.Length) {
                        if (result.VerifyTaskResultCodes[i] / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.VerifyTaskResults[i], result.VerifyTaskResultCodes[i]);
                        }
                        if (!skipCallback) {
                            Gs2.TransactionConfiguration.VerifyActionEventHandler.Invoke(
                                Gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                this.AccessToken?.TimeOffset,
                                verifyTask.Action,
                                verifyTask.Request,
                                result.VerifyTaskResults[i]
                            );
                        }
                    }
                }
            }

            if (result.TaskRequests != null) {
                for (var i = 0; i < result.TaskRequests.Length; i++) {
                    var stampTask = result.TaskRequests[i];
                    if (i < result.TaskResults.Length) {
                        if (result.TaskResultCodes[i] / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.TaskResults[i], result.TaskResultCodes[i]);
                        }
                        if (!skipCallback) {
                            Gs2.TransactionConfiguration.ConsumeActionEventHandler.Invoke(
                                Gs2.Cache,
                                this._transactionId + "[" + i + "]",
                                this.AccessToken?.TimeOffset,
                                stampTask.Action,
                                stampTask.Request,
                                result.TaskResults[i]
                            );
                        }
                    }
                }
            }

            if (result.SheetResult != null) {
                if (result.SheetResultCode / 100 != 2) {
                    throw Gs2Exception.ExtractError(result.SheetResult, result.SheetResultCode ?? 999);
                }
                
                if (!skipCallback) {
                    Gs2.TransactionConfiguration.AcquireActionEventHandler.Invoke(
                        Gs2.Cache,
                        this._transactionId,
                        this.AccessToken?.TimeOffset,
                        result.SheetRequest.Action,
                        result.SheetRequest.Request,
                        result.SheetResult
                    );
                }

                var nextTransactions = new List<TransactionAccessTokenDomain>();
                if (result.SheetRequest.Action == "Gs2JobQueue:PushByUserId")
                {
                    nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                        Gs2,
                        AccessToken,
                        PushByUserIdResult.FromJson(JsonMapper.ToObject(result.SheetResult))
                    ));
                }

                var resultJson = JsonMapper.ToObject(result.SheetResult);
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
            var timer = Stopwatch.StartNew();
            RETRY:
            if (timer.Elapsed > TimeSpan.FromSeconds(10)) {
                throw new TimeoutException("Failed to retrieve transaction results, either because there is some failure in GS2, or the GS2-Gateway used to notify the GS2-Distributor used to execute the transaction is not yet configured, or the GS2-Gateway has a user ID to receive notifications The configuration API may not have been invoked.");
            }
            var domain = new Gs2Distributor.Domain.Gs2Distributor(
                Gs2
            ).Namespace(
                this._namespaceName ?? Gs2.TransactionConfiguration.NamespaceName ?? "default"
            ).AccessToken(
                AccessToken
            ).StampSheetResult(
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
                await TaskUtilities.DelayAsync(Gs2Constant.RetryWait);
                goto RETRY;
            }
        }
    }
}
