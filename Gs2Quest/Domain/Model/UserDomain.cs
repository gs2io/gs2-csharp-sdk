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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Quest.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2QuestRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2QuestRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Quest.Model.CompletedQuestList> CompletedQuestLists(
            string timeOffsetToken = null
        )
        {
            return new DescribeCompletedQuestListsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Quest.Model.CompletedQuestList> CompletedQuestListsAsync(
            #else
        public DescribeCompletedQuestListsByUserIdIterator CompletedQuestListsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeCompletedQuestListsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeCompletedQuestLists(
            Action<Gs2.Gs2Quest.Model.CompletedQuestList[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Quest.Model.CompletedQuestList>(
                (null as Gs2.Gs2Quest.Model.CompletedQuestList).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeCompletedQuestListsWithInitialCallAsync(
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
        #endif

        public void UnsubscribeCompletedQuestLists(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Quest.Model.CompletedQuestList>(
                (null as Gs2.Gs2Quest.Model.CompletedQuestList).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Quest.Domain.Model.CompletedQuestListDomain CompletedQuestList(
            string questGroupName
        ) {
            return new Gs2.Gs2Quest.Domain.Model.CompletedQuestListDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                questGroupName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Quest.Model.Progress> Progresses(
            string timeOffsetToken = null
        )
        {
            return new DescribeProgressesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Quest.Model.Progress> ProgressesAsync(
            #else
        public DescribeProgressesByUserIdIterator ProgressesAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeProgressesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeProgresses(
            Action<Gs2.Gs2Quest.Model.Progress[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Quest.Model.Progress>(
                (null as Gs2.Gs2Quest.Model.Progress).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeProgressesWithInitialCallAsync(
            Action<Gs2.Gs2Quest.Model.Progress[]> callback
        )
        {
            var items = await ProgressesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeProgresses(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeProgresses(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Quest.Model.Progress>(
                (null as Gs2.Gs2Quest.Model.Progress).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Quest.Domain.Model.ProgressDomain Progress(
        ) {
            return new Gs2.Gs2Quest.Domain.Model.ProgressDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain> CreateProgressFuture(
            CreateProgressByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.CreateProgressByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Quest.Domain.Model.ProgressDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Quest.Domain.Model.ProgressDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Quest.Domain.Model.ProgressDomain> CreateProgressAsync(
            #else
        public async Task<Gs2.Gs2Quest.Domain.Model.ProgressDomain> CreateProgressAsync(
            #endif
            CreateProgressByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.CreateProgressByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Quest.Domain.Model.ProgressDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> StartFuture(
            StartByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.StartByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.UserId,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionDomain> StartAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionDomain> StartAsync(
            #endif
            StartByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.StartByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

    }

    public partial class UserDomain {

    }
}
