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
using Gs2.Gs2Money2.Domain.Iterator;
using Gs2.Gs2Money2.Model.Cache;
using Gs2.Gs2Money2.Request;
using Gs2.Gs2Money2.Result;
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

namespace Gs2.Gs2Money2.Domain.Model
{

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Money2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public Gs2.Gs2Money2.Model.DepositTransaction[] WithdrawTransactions { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Money2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain> VerifyReceiptFuture(
            VerifyReceiptRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.VerifyReceiptFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.TransactionId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain> VerifyReceiptAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain> VerifyReceiptAsync(
            #endif
            VerifyReceiptRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.VerifyReceiptAsync(request)
            );
            var domain = new Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.TransactionId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> AllocateSubscriptionStatusFuture(
            AllocateSubscriptionStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.AllocateSubscriptionStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.ContentName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> AllocateSubscriptionStatusAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> AllocateSubscriptionStatusAsync(
            #endif
            AllocateSubscriptionStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.AllocateSubscriptionStatusAsync(request)
            );
            var domain = new Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.ContentName
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> TakeoverSubscriptionStatusFuture(
            TakeoverSubscriptionStatusRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.TakeoverSubscriptionStatusFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.ContentName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> TakeoverSubscriptionStatusAsync(
            #else
        public async Task<Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain> TakeoverSubscriptionStatusAsync(
            #endif
            TakeoverSubscriptionStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.TakeoverSubscriptionStatusAsync(request)
            );
            var domain = new Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.ContentName
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.Wallet> Wallets(
        )
        {
            return new DescribeWalletsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.Wallet> WalletsAsync(
            #else
        public DescribeWalletsIterator WalletsAsync(
            #endif
        )
        {
            return new DescribeWalletsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeWallets(
            Action<Gs2.Gs2Money2.Model.Wallet[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.Wallet>(
                (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
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
                            callback.Invoke(await WalletsAsync(
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
        public async UniTask<ulong> SubscribeWalletsWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.Wallet[]> callback
        )
        {
            var items = await WalletsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeWallets(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeWallets(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.Wallet>(
                (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateWallets(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Money2.Model.Wallet>(
                (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Money2.Domain.Model.WalletAccessTokenDomain Wallet(
            int? slot
        ) {
            return new Gs2.Gs2Money2.Domain.Model.WalletAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                slot
            );
        }

        public Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain Event(
            string transactionId
        ) {
            return new Gs2.Gs2Money2.Domain.Model.EventAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                transactionId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Money2.Model.SubscriptionStatus> SubscriptionStatuses(
        )
        {
            return new DescribeSubscriptionStatusesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Money2.Model.SubscriptionStatus> SubscriptionStatusesAsync(
            #else
        public DescribeSubscriptionStatusesIterator SubscriptionStatusesAsync(
            #endif
        )
        {
            return new DescribeSubscriptionStatusesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscriptionStatuses(
            Action<Gs2.Gs2Money2.Model.SubscriptionStatus[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Money2.Model.SubscriptionStatus>(
                (null as Gs2.Gs2Money2.Model.SubscriptionStatus).CacheParentKey(
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
                            callback.Invoke(await SubscriptionStatusesAsync(
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
        public async UniTask<ulong> SubscribeSubscriptionStatusesWithInitialCallAsync(
            Action<Gs2.Gs2Money2.Model.SubscriptionStatus[]> callback
        )
        {
            var items = await SubscriptionStatusesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSubscriptionStatuses(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscriptionStatuses(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Money2.Model.SubscriptionStatus>(
                (null as Gs2.Gs2Money2.Model.SubscriptionStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateSubscriptionStatuses(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Money2.Model.SubscriptionStatus>(
                (null as Gs2.Gs2Money2.Model.SubscriptionStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain SubscriptionStatus(
            string contentName
        ) {
            return new Gs2.Gs2Money2.Domain.Model.SubscriptionStatusAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                contentName
            );
        }

    }
}
