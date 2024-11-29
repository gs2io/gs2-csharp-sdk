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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public partial class ManualStampSheetDomain : TransactionDomain
    {
        private readonly string _transactionId;
        private readonly string _stampSheet;
        private readonly string _stampSheetEncryptionKeyId;

        public ManualStampSheetDomain(
            Gs2 gs2,
            string userId,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKeyId
        ): base(
            gs2,
            userId,
            null
        ) {
            this._transactionId = transactionId;
            this._stampSheet = stampSheet;
            this._stampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
        }

        private TransactionDomain HandleResult(
            string action,
            JsonData resultJson
        ) {
            var nextTransactions = new List<TransactionDomain>();
            if (action == "Gs2JobQueue:PushByUserId")
            {
                nextTransactions.Add(JobQueueJobDomainFactory.ToTransaction(
                    Gs2,
                    UserId,
                    PushByUserIdResult.FromJson(resultJson)
                ));
            }
            
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
                        var future = client.RunVerifyTaskWithoutNamespaceFuture(
                            new RunVerifyTaskWithoutNamespaceRequest()
                                    .WithContextStack(contextStack)
                                    .WithVerifyTask(verifyTasks[i].ToString())
                                    .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null) {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        if (result.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            verifyTaskPayloadJson["action"].ToString(),
                            verifyTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    else
                    {
                        var future = client.RunVerifyTaskFuture(
                            new RunVerifyTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                                .WithVerifyTask(verifyTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is NotFoundException) {
                                if (Gs2.TransactionConfiguration != null) {
                                    Gs2.TransactionConfiguration.NamespaceName = null;
                                    var future2 = WaitFuture(all);
                                    yield return future2;
                                    if (future2.Error != null) {
                                        self.OnError(future2.Error);
                                        yield break;
                                    }
                                    self.OnComplete(future2.Result);
                                    yield break;
                                }
                            }
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        if (result.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
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
                        var future = client.RunStampTaskWithoutNamespaceFuture(
                            new RunStampTaskWithoutNamespaceRequest()
                                    .WithContextStack(contextStack)
                                    .WithStampTask(stampTasks[i].ToString())
                                    .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null) {
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        if (result.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    else
                    {
                        var future = client.RunStampTaskFuture(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                                .WithStampTask(stampTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        yield return future;
                        if (future.Error != null) {
                            if (future.Error is NotFoundException) {
                                if (Gs2.TransactionConfiguration != null) {
                                    Gs2.TransactionConfiguration.NamespaceName = null;
                                    var future2 = WaitFuture(all);
                                    yield return future2;
                                    if (future2.Error != null) {
                                        self.OnError(future2.Error);
                                        yield break;
                                    }
                                    self.OnComplete(future2.Result);
                                    yield break;
                                }
                            }
                            self.OnError(future.Error);
                            yield break;
                        }
                        var result = future.Result;
                        contextStack = result.ContextStack;
                        if (result.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                        }
                        Gs2.TransactionConfiguration?.ConsumeActionEventHandler?.Invoke(
                            Gs2.Cache,
                            stampSheetPayloadJson["transactionId"].ToString() + "[" + i + "]",
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
                    var future = client.RunStampSheetWithoutNamespaceFuture(
                        new RunStampSheetWithoutNamespaceRequest()
                            .WithContextStack(contextStack)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.AcquireActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }
                else
                {
                    var future = client.RunStampSheetFuture(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    yield return future;
                    if (future.Error != null) {
                        if (future.Error is NotFoundException) {
                            if (Gs2.TransactionConfiguration != null) {
                                Gs2.TransactionConfiguration.NamespaceName = null;
                                var future2 = WaitFuture(all);
                                yield return future2;
                                if (future2.Error != null) {
                                    self.OnError(future2.Error);
                                    yield break;
                                }
                                self.OnComplete(future2.Result);
                                yield break;
                            }
                        }
                        self.OnError(future.Error);
                        yield break;
                    }
                    var result = future.Result;
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.AcquireActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }

                var transaction = HandleResult(action, resultJson);
                if (all && transaction != null) {
                    var future = transaction.WaitFuture(true);
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    self.OnComplete(null);
                    yield break;
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
                }
                else
                {
                    try {
                        var result = await client.RunVerifyTaskAsync(
                            new RunVerifyTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                                .WithVerifyTask(verifyTasks[i].ToString())
                                .WithKeyId(_stampSheetEncryptionKeyId)
                        );
                        contextStack = result.ContextStack;
                        if (result.StatusCode / 100 != 2) {
                            throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                        }
                    }
                    catch (NotFoundException) {
                        if (Gs2.TransactionConfiguration != null) {
                            Gs2.TransactionConfiguration.NamespaceName = null;
                            return await WaitAsync(all);
                        }
                    }
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
                        stampTaskPayloadJson["action"].ToString(),
                        stampTaskPayloadJson["args"].ToString(),
                        result.Result
                    );
                }
                else
                {
                    try {
                        var result = await client.RunStampTaskAsync(
                            new RunStampTaskRequest()
                                .WithContextStack(contextStack)
                                .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
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
                            stampTaskPayloadJson["action"].ToString(),
                            stampTaskPayloadJson["args"].ToString(),
                            result.Result
                        );
                    }
                    catch (NotFoundException) {
                        if (Gs2.TransactionConfiguration != null) {
                            Gs2.TransactionConfiguration.NamespaceName = null;
                            return await WaitAsync(all);
                        }
                    }
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
                    stampSheetPayloadJson["action"].ToString(),
                    stampSheetPayloadJson["args"].ToString(),
                    result.Result
                );
                action = stampSheetPayloadJson["action"].ToString();
                resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
            }
            else
            {
                try {
                    var result = await client.RunStampSheetAsync(
                        new RunStampSheetRequest()
                            .WithContextStack(contextStack)
                            .WithNamespaceName(Gs2.TransactionConfiguration?.NamespaceName)
                            .WithStampSheet(_stampSheet)
                            .WithKeyId(_stampSheetEncryptionKeyId)
                    );
                    if (result.StatusCode / 100 != 2) {
                        throw Gs2Exception.ExtractError(result.Result, result.StatusCode ?? 999);
                    }
                    Gs2.TransactionConfiguration?.AcquireActionEventHandler?.Invoke(
                        Gs2.Cache,
                        stampSheetPayloadJson["transactionId"].ToString(),
                        stampSheetPayloadJson["action"].ToString(),
                        stampSheetPayloadJson["args"].ToString(),
                        result.Result
                    );
                    action = stampSheetPayloadJson["action"].ToString();
                    resultJson = JsonMapper.ToObject(result.Result.Length != 0 ? result.Result : "{}");
                }
                catch (NotFoundException) {
                    if (Gs2.TransactionConfiguration != null) {
                        Gs2.TransactionConfiguration.NamespaceName = null;
                        return await WaitAsync(all);
                    }
                }
            }

            var transaction = HandleResult(action, resultJson);
            if (all && transaction != null) {
                return await transaction.WaitAsync(true);
            }
            return transaction;
        }
#endif
    }
}
