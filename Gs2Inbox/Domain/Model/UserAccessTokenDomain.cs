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
using Gs2.Gs2Inbox.Domain.Iterator;
using Gs2.Gs2Inbox.Model.Cache;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Result;
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

namespace Gs2.Gs2Inbox.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InboxRestClient _client;
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
            this._client = new Gs2InboxRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageFuture(
            ReceiveGlobalMessageRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.ReceiveGlobalMessageFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                _gs2.Cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                    (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        null
                    )
                );
                _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                    (null as Gs2.Gs2Inbox.Model.Received).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        null
                    ),
                    (null as Gs2.Gs2Inbox.Model.Received).CacheKey(
                    )
                );
                var domain = result?.Item?.Select(v => new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageAsync(
            #else
        public async Task<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain[]> ReceiveGlobalMessageAsync(
            #endif
            ReceiveGlobalMessageRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.ReceiveGlobalMessageAsync(request)
            );
            _gs2.Cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                )
            );
            _gs2.Cache.Delete<Gs2.Gs2Inbox.Model.Received>(
                (null as Gs2.Gs2Inbox.Model.Received).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Inbox.Model.Received).CacheKey(
                )
            );
            var domain = result?.Item?.Select(v => new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain>();
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> BatchReadMessagesFuture(
            BatchReadMessagesRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor.BatchReadMessagesByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        BatchReadMessagesByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.BatchReadMessagesFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
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
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> BatchReadMessagesAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> BatchReadMessagesAsync(
            #endif
            BatchReadMessagesRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor.BatchReadMessagesByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    BatchReadMessagesByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.BatchReadMessagesAsync(request)
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
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inbox.Model.Message> Messages(
            bool? isRead = null
        )
        {
            return new DescribeMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                isRead
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inbox.Model.Message> MessagesAsync(
            #else
        public DescribeMessagesIterator MessagesAsync(
            #endif
            bool? isRead = null
        )
        {
            return new DescribeMessagesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                isRead
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeMessages(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await MessagesAsync(
                                isRead
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeMessagesWithInitialCallAsync(
            Action<Gs2.Gs2Inbox.Model.Message[]> callback,
            bool? isRead = null
        )
        {
            var items = await MessagesAsync(
                isRead
            ).ToArrayAsync();
            var callbackId = SubscribeMessages(
                callback,
                isRead
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeMessages(
            ulong callbackId,
            bool? isRead = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateMessages(
            bool? isRead = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inbox.Model.Message>(
                (null as Gs2.Gs2Inbox.Model.Message).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain Message(
            string messageName
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.MessageAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                messageName
            );
        }

        public Gs2.Gs2Inbox.Domain.Model.ReceivedAccessTokenDomain Received(
        ) {
            return new Gs2.Gs2Inbox.Domain.Model.ReceivedAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken
            );
        }

    }
}
