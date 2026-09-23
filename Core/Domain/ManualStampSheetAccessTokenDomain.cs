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
using Gs2.Gs2Distributor;
using Gs2.Gs2Distributor.Request;
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
    public partial class ManualStampSheetAccessTokenDomain : TransactionAccessTokenDomain
    {
        private readonly string _transactionId;
        private readonly string _stampSheet;
        private readonly string _stampSheetEncryptionKeyId;
        // The stamp sheet runs once per instance: the SDK waits on it inside the
        // action that issued it, and a caller that waits again (Gs2Bind does)
        // shares that run instead of sending every task a second time.
#if GS2_ENABLE_UNITASK
        private UniTask<TransactionAccessTokenDomain>? _run;
        private int _runGeneration;
#else
        private Task<TransactionAccessTokenDomain> _run;
        private readonly object _runLock = new object();
#endif
        public string TransactionId => _transactionId;

        public ManualStampSheetAccessTokenDomain(
            Gs2 gs2,
            AccessToken AccessToken,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKeyId
        ): base(
            gs2,
            AccessToken,
            null
        ) {
            this._transactionId = transactionId;
            this._stampSheet = stampSheet;
            this._stampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
        }

        private TransactionAccessTokenDomain HandleResult(
            string action,
            JsonData resultJson
        ) {
            var nextTransactions = new List<TransactionAccessTokenDomain>();
            if (action == "Gs2JobQueue:PushByUserId")
            {
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    AccessToken,
                    PushByUserIdResult.FromJson(resultJson)
                ));
            }
            
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
#if GS2_ENABLE_UNITASK
            if (_run == null) {
                _run = RunAsync().Preserve();
                _runGeneration++;
            }
            var run = _run.Value;
            var generation = _runGeneration;
            TransactionAccessTokenDomain transaction;
            try {
                transaction = await run;
            } catch {
                // A failed run is forgotten so that waiting again retries it,
                // unless a retry has already replaced it.
                if (_runGeneration == generation) _run = null;
                throw;
            }
#else
            Task<TransactionAccessTokenDomain> run;
            lock (_runLock) {
                run = _run ??= RunAsync();
            }
            TransactionAccessTokenDomain transaction;
            try {
                transaction = await run;
            } catch {
                // A failed run is forgotten so that waiting again retries it.
                lock (_runLock) {
                    if (_run == run) _run = null;
                }
                throw;
            }
#endif
            if (all && transaction != null) {
                return await transaction.WaitAsync(true);
            }
            return transaction;
        }

#if GS2_ENABLE_UNITASK
        private async UniTask<TransactionAccessTokenDomain> RunAsync(
#else
        private async Task<TransactionAccessTokenDomain> RunAsync(
#endif
        ) {
            var client = new Gs2DistributorRestClient(
                Gs2.RestSession
            );
            var stampSheetJson = JsonMapper.ToObject(_stampSheet);
            var stampSheetPayload = stampSheetJson["body"].ToString();
            var stampSheetPayloadJson = JsonMapper.ToObject(stampSheetPayload);
            var verifyTasks = stampSheetPayloadJson["verifyTasks"];
            var stampTasks = stampSheetPayloadJson["tasks"];
            string contextStack = null;
            for (var i = 0; i < verifyTasks.Count; i++)
            {
                var verifyTask = verifyTasks[i].ToString();
                var verifyTaskJson = JsonMapper.ToObject(verifyTasks[i].ToString());
                var verifyTaskPayload = verifyTaskJson["body"].ToString();
                var verifyTaskPayloadJson = JsonMapper.ToObject(verifyTaskPayload);
                if (string.IsNullOrEmpty(Gs2.TransactionConfiguration?.NamespaceName))
                {
                    var result = await client.RunVerifyTaskWithoutNamespaceAsync(
                        new RunVerifyTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithVerifyTask(verifyTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    contextStack = result.ContextStack;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.VerifyActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                        this.AccessToken?.TimeOffset,
                        verifyTaskPayloadJson["action"].ToString(),
                        verifyTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
                else
                {
                    global::Gs2.Gs2Distributor.Result.RunVerifyTaskResult result;
                    try {
                        result = await client.RunVerifyTaskAsync(
                            new RunVerifyTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                                .WithVerifyTask(verifyTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                    }
                    catch (NotFoundException) {
                        if (Gs2.TransactionConfiguration == null) {
                            throw;
                        }
                        Gs2.TransactionConfiguration.NamespaceName = null;
                        var fallbackResult = await client.RunVerifyTaskWithoutNamespaceAsync(
                            new RunVerifyTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithVerifyTask(verifyTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        contextStack = fallbackResult.ContextStack;
                        if (fallbackResult.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(fallbackResult.Result, fallbackResult.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration.VerifyActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            this.AccessToken?.TimeOffset,
                            verifyTaskPayloadJson["action"].ToString(),
                            verifyTaskPayloadJson["args"].ToString(),
                            fallbackResult.Result
                        );
                        continue;
                    }
                    contextStack = result.ContextStack;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.VerifyActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                        this.AccessToken?.TimeOffset,
                        verifyTaskPayloadJson["action"].ToString(),
                        verifyTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
            }
            for (var i = 0; i < stampTasks.Count; i++)
            {
                var stampTask = stampTasks[i].ToString();
                var stampTaskJson = JsonMapper.ToObject(stampTasks[i].ToString());
                var stampTaskPayload = stampTaskJson["body"].ToString();
                var stampTaskPayloadJson = JsonMapper.ToObject(stampTaskPayload);
                if (string.IsNullOrEmpty(Gs2.TransactionConfiguration?.NamespaceName))
                {
                    var result = await client.RunStampTaskWithoutNamespaceAsync(
                        new RunStampTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    contextStack = result.ContextStack;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                        this.AccessToken?.TimeOffset,
                        stampTaskPayloadJson["action"].ToString(),
                        stampTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
                else
                {
                    global::Gs2.Gs2Distributor.Result.RunStampTaskResult result;
                    try {
                        result = await client.RunStampTaskAsync(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                    }
                    catch (NotFoundException) {
                        if (Gs2.TransactionConfiguration == null) {
                            throw;
                        }
                        Gs2.TransactionConfiguration.NamespaceName = null;
                        var fallbackResult = await client.RunStampTaskWithoutNamespaceAsync(
                            new RunStampTaskWithoutNamespaceRequest()
                                .WithContextStack(contextStack)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        contextStack = fallbackResult.ContextStack;
                        if (fallbackResult.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(fallbackResult.Result, fallbackResult.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration.ConsumeActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            this.AccessToken?.TimeOffset,
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            fallbackResult.Result
                        );
                        continue;
                    }
                    contextStack = result.ContextStack;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                        this.AccessToken?.TimeOffset,
                        stampTaskPayloadJson["action"].ToString(),
                        stampTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
            }

            string action = null;
            JsonData resultJson = null;
            if (string.IsNullOrEmpty(Gs2.TransactionConfiguration?.NamespaceName))
            {
                var result = await client.RunStampSheetWithoutNamespaceAsync(
                    new RunStampSheetWithoutNamespaceRequest()
                        .WithContextStack(contextStack)
                        .WithStampSheet(_stampSheet)
                        .WithKeyId(_stampSheetEncryptionKeyId)
                );
                if (result.StatusCode / 100 != 2) {
                    throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                }
                Gs2.TransactionConfiguration?.AcquireActionEventHandler?.Invoke(
                    Gs2.Cache,
                    stampSheetPayloadJson["transactionId"].ToString(),
                    this.AccessToken?.TimeOffset,
                    stampSheetPayloadJson["action"].ToString(),
                    stampSheetPayloadJson["args"].ToString(),
                    result.Result
                );
                action = stampSheetPayloadJson["action"].ToString();
                resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
            }
            else
            {
                global::Gs2.Gs2Distributor.Result.RunStampSheetResult result = null;
                try {
                    result = await client.RunStampSheetAsync(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                }
                catch (NotFoundException) {
                    if (Gs2.TransactionConfiguration == null) {
                        throw;
                    }
                    Gs2.TransactionConfiguration.NamespaceName = null;
                    var fallbackResult = await client.RunStampSheetWithoutNamespaceAsync(
                        new RunStampSheetWithoutNamespaceRequest()
                            .WithContextStack(contextStack)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    if (fallbackResult.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(fallbackResult.Result, fallbackResult.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration.AcquireActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        this.AccessToken?.TimeOffset,
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        fallbackResult.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    resultJson = JsonMapper.ToObject(fallbackResult.Result.Length != 0 ? fallbackResult.Result : "{}");
                }
                if (result != null) {
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.AcquireActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        this.AccessToken?.TimeOffset,
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }
            }

            return HandleResult(action, resultJson);
        }
    }
}
