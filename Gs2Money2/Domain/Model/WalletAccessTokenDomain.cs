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
 * deny overwrite
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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Money2.Domain.Model
{

    public partial class WalletAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Money2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public int? Slot { get; } = null!;
        public Gs2.Gs2Money2.Model.DepositTransaction[] WithdrawTransactions { get; set; } = null!;

        public WalletAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            int? slot
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Money2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.Slot = slot;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Money2.Model.Wallet> GetFuture(
            GetWalletRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Money2.Model.Wallet> GetAsync(
        #else
        private async Task<Gs2.Gs2Money2.Model.Wallet> GetAsync(
        #endif
            GetWalletRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithSlot(this.Slot);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetWalletAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Domain.Model.WalletAccessTokenDomain> WithdrawFuture(
            WithdrawRequest request
        ) => WithdrawAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Money2.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
        #else
        public async Task<Gs2.Gs2Money2.Domain.Model.WalletAccessTokenDomain> WithdrawAsync(
        #endif
            WithdrawRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithSlot(this.Slot);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.WithdrawAsync(request)
            );
            this.WithdrawTransactions = result.WithdrawTransactions; /* diff +++ */
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Money2.Model.Wallet> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Money2.Model.Wallet> ModelAsync()
        #else
        public async Task<Gs2.Gs2Money2.Model.Wallet> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Money2.Model.Wallet>(
                        (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Money2.Model.Wallet).CacheKey(
                            this.Slot
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Money2.Model.Wallet).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.Slot,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Money2.Model.Wallet).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.Slot,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetWalletRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Money2.Model.Wallet).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.Slot ?? default,
                this.AccessToken?.TimeOffset
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Money2.Model.Wallet).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.Slot ?? default,
                this.AccessToken?.TimeOffset,
                () => this.GetAsync(
                    new GetWalletRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Money2.Model.Wallet> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Money2.Model.Wallet> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Money2.Model.Wallet> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Money2.Model.Wallet> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Money2.Model.Wallet> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Money2.Model.Wallet> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Money2.Model.Wallet).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                this.Slot,
 diff --- end */
                this.Slot ?? default, /* diff +++ */
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Money2.Model.Wallet> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Money2.Model.Wallet).CacheKey(
/* diff --- start
                    this.Slot
 diff --- end */
                    this.Slot ?? default /* diff +++ */
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
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Money2.Model.Wallet>(
                (null as Gs2.Gs2Money2.Model.Wallet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Money2.Model.Wallet).CacheKey(
/* diff --- start
                    this.Slot
 diff --- end */
                    this.Slot ?? default /* diff +++ */
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Money2.Model.Wallet> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Money2.Model.Wallet> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money2.Model.Wallet> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Money2.Model.Wallet> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
