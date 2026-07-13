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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model.Cache;
using Gs2.Gs2Auth.Request;
using Gs2.Gs2Auth.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Auth.Domain.Model
{

    public partial class AccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AuthRestClient _client;
        public string Token { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public long? Expire { get; set; } = null!;
        public string Status { get; set; } = null!;

        public AccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2AuthRestClient(
                gs2.RestSession
            );
        }

    }

    public partial class AccessTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginFuture(
            LoginRequest request
        ) => LoginAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
        #endif
            LoginRequest request
        ) {
/* diff --- start
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserId(this.UserId);
 diff --- end */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.LoginAsync(request)
            );
            var domain = this;
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureFuture(
            LoginBySignatureRequest request
        ) => LoginBySignatureAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
        #endif
            LoginBySignatureRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.LoginBySignatureAsync(request)
            );
            var domain = this;
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> FederationFuture(
            FederationRequest request
        ) => FederationAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> FederationAsync(
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> FederationAsync(
        #endif
            FederationRequest request
        ) {
/* diff --- start
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserId(this.UserId);
 diff --- end */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.FederationAsync(request)
            );
            var domain = this;
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> IssueTimeOffsetTokenFuture(
            IssueTimeOffsetTokenByUserIdRequest request
        ) => IssueTimeOffsetTokenAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> IssueTimeOffsetTokenAsync(
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> IssueTimeOffsetTokenAsync(
        #endif
            IssueTimeOffsetTokenByUserIdRequest request
        ) {
/* diff --- start
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserId(this.UserId);
 diff --- end */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.IssueTimeOffsetTokenByUserIdAsync(request)
            );
            var domain = this;
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }

    }

    public partial class AccessTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Model.AccessToken> ModelAsync()
        #else
        public async Task<Gs2.Gs2Auth.Model.AccessToken> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Auth.Model.AccessToken>(
                        (null as Gs2.Gs2Auth.Model.AccessToken).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Auth.Model.AccessToken).CacheKey(
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Auth.Model.AccessToken).GetCache(
                    this._gs2.Cache,
                    null
                );
                if (find) {
                    return value;
                }
                return null;
            }
 diff --- end */
/* diff +++ start */
            return new AccessToken()
                .WithToken(Token)
                .WithUserId(UserId)
                .WithExpire(Expire);
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Auth.Model.AccessToken> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Auth.Model.AccessToken> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Auth.Model.AccessToken).DeleteCache(
                this._gs2.Cache,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Auth.Model.AccessToken> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Auth.Model.AccessToken).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Auth.Model.AccessToken).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Auth.Model.AccessToken>(
                (null as Gs2.Gs2Auth.Model.AccessToken).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Auth.Model.AccessToken).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Auth.Model.AccessToken> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Auth.Model.AccessToken> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Auth.Model.AccessToken> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Auth.Model.AccessToken> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
