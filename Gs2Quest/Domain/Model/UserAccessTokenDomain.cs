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

#pragma warning disable 1998
#pragma warning disable CS0169, CS0168

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Quest.Domain.Iterator;
using Gs2.Gs2Quest.Model.Cache;
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Quest.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2QuestRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2QuestRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> StartFuture(
            StartRequest request,
            bool speculativeExecute = true
        ) => StartAsync(request, speculativeExecute).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> StartAsync(
        #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> StartAsync(
        #endif
            StartRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2Quest.Domain.Transaction.SpeculativeExecutor.StartByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    StartByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.StartAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Quest.Model.CompletedQuestList> CompletedQuestLists(
        )
        {
            return new DescribeCompletedQuestListsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Quest.Model.CompletedQuestList> CompletedQuestListsAsync(
        #else
        public DescribeCompletedQuestListsIterator CompletedQuestListsAsync(
        #endif
        )
        {
            return new DescribeCompletedQuestListsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public ulong SubscribeCompletedQuestLists(
            Action<Gs2.Gs2Quest.Model.CompletedQuestList[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Quest.Model.CompletedQuestList>(
                (null as Gs2.Gs2Quest.Model.CompletedQuestList).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await CompletedQuestListsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCompletedQuestListsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeCompletedQuestListsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Quest.Model.CompletedQuestList[]> callback
        )
        {
            var items = await CompletedQuestListsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeCompletedQuestLists(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeCompletedQuestLists(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Quest.Model.CompletedQuestList>(
                (null as Gs2.Gs2Quest.Model.CompletedQuestList).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateCompletedQuestLists(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Quest.Model.CompletedQuestList>(
                (null as Gs2.Gs2Quest.Model.CompletedQuestList).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Quest.Domain.Model.CompletedQuestListAccessTokenDomain CompletedQuestList(
            string questGroupName
        ) {
            return new Gs2.Gs2Quest.Domain.Model.CompletedQuestListAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                questGroupName
            );
        }

        public Gs2.Gs2Quest.Domain.Model.ProgressAccessTokenDomain Progress(
        ) {
            return new Gs2.Gs2Quest.Domain.Model.ProgressAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken
            );
        }

    }
}
