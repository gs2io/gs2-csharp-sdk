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
using Gs2.Gs2Account.Domain.Iterator;
using Gs2.Gs2Account.Model.Cache;
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
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

namespace Gs2.Gs2Account.Domain.Model
{

    public partial class TakeOverAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AccountRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public int? Type { get; } = null!;
        public string UserIdentifier { get; } = null!;
        public string AuthorizationUrl { get; set; } = null!;
        public string Payload { get; set; } = null!;

        public TakeOverAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            int? type
        ) {
            this._gs2 = gs2;
            this._client = new Gs2AccountRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.Type = type;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateFuture(
            CreateTakeOverRequest request
        ) => CreateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateAsync(
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateAsync(
        #endif
            CreateTakeOverRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithType(this.Type);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.CreateTakeOverAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateOpenIdConnectFuture(
            CreateTakeOverOpenIdConnectRequest request
        ) => CreateOpenIdConnectAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateOpenIdConnectAsync(
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> CreateOpenIdConnectAsync(
        #endif
            CreateTakeOverOpenIdConnectRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithType(this.Type);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.CreateTakeOverOpenIdConnectAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Account.Model.TakeOver> GetFuture(
            GetTakeOverRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Account.Model.TakeOver> GetAsync(
        #else
        private async Task<Gs2.Gs2Account.Model.TakeOver> GetAsync(
        #endif
            GetTakeOverRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithType(this.Type);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetTakeOverAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> UpdateFuture(
            UpdateTakeOverRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> UpdateAsync(
        #endif
            UpdateTakeOverRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithType(this.Type);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.UpdateTakeOverAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ start */
        public IFuture<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> DeleteFuture(
            DeleteTakeOverRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Account.Domain.Model.TakeOverAccessTokenDomain> DeleteAsync(
        #endif
            DeleteTakeOverRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithType(this.Type);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteTakeOverAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff +++ end */
        public IFuture<Gs2.Gs2Account.Model.TakeOver> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Account.Model.TakeOver> ModelAsync()
        #else
        public async Task<Gs2.Gs2Account.Model.TakeOver> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Account.Model.TakeOver>(
                        (null as Gs2.Gs2Account.Model.TakeOver).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Account.Model.TakeOver).CacheKey(
                            this.Type
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Account.Model.TakeOver).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.Type,
 diff --- end */
                    this.Type ?? default, /* diff +++ */
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Account.Model.TakeOver).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
/* diff --- start
                    this.Type,
 diff --- end */
                    this.Type ?? default, /* diff +++ */
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetTakeOverRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Account.Model.TakeOver> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Account.Model.TakeOver> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Account.Model.TakeOver> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Account.Model.TakeOver> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Account.Model.TakeOver).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
/* diff --- start
                this.Type,
 diff --- end */
                this.Type ?? default, /* diff +++ */
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Account.Model.TakeOver> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Account.Model.TakeOver).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Account.Model.TakeOver).CacheKey(
/* diff --- start
                    this.Type
 diff --- end */
                    this.Type ?? default /* diff +++ */
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Account.Model.TakeOver>(
                (null as Gs2.Gs2Account.Model.TakeOver).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Account.Model.TakeOver).CacheKey(
/* diff --- start
                    this.Type
 diff --- end */
                    this.Type ?? default /* diff +++ */
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.TakeOver> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Account.Model.TakeOver> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.TakeOver> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Account.Model.TakeOver> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
