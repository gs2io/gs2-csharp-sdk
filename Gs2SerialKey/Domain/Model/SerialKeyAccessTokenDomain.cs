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
using Gs2.Gs2SerialKey.Domain.Iterator;
using Gs2.Gs2SerialKey.Model.Cache;
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Result;
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

namespace Gs2.Gs2SerialKey.Domain.Model
{

    public partial class SerialKeyAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2SerialKeyRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string SerialKeyCode { get; } = null!;
        public string Url { get; set; } = null!;

        public SerialKeyAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string serialKeyCode
        ) {
            this._gs2 = gs2;
            this._client = new Gs2SerialKeyRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.SerialKeyCode = serialKeyCode;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Domain.Model.SerialKeyAccessTokenDomain> UseFuture(
            UseRequest request
        ) => UseAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Domain.Model.SerialKeyAccessTokenDomain> UseAsync(
        #else
        public async Task<Gs2.Gs2SerialKey.Domain.Model.SerialKeyAccessTokenDomain> UseAsync(
        #endif
            UseRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
/* diff --- start
                .WithAccessToken(this.AccessToken?.Token);
 diff --- end */
/* diff +++ start */
                .WithAccessToken(this.AccessToken?.Token)
                .WithCode(this.SerialKeyCode);
/* diff +++ end */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.UseAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2SerialKey.Model.SerialKey> ModelAsync()
        #else
        public async Task<Gs2.Gs2SerialKey.Model.SerialKey> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2SerialKey.Model.SerialKey>(
                        (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheKey(
                            this.SerialKeyCode
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2SerialKey.Model.SerialKey).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.SerialKeyCode,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return null;
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2SerialKey.Model.SerialKey).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.SerialKeyCode,
                this.AccessToken?.TimeOffset
            );
            if (find) {
                return value;
/* diff +++ end */
            }
            return null; /* diff +++ */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2SerialKey.Model.SerialKey> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2SerialKey.Model.SerialKey> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2SerialKey.Model.SerialKey> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2SerialKey.Model.SerialKey).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.SerialKeyCode,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2SerialKey.Model.SerialKey> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheKey(
                    this.SerialKeyCode
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2SerialKey.Model.SerialKey>(
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2SerialKey.Model.SerialKey).CacheKey(
                    this.SerialKeyCode
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SerialKey.Model.SerialKey> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2SerialKey.Model.SerialKey> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SerialKey.Model.SerialKey> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2SerialKey.Model.SerialKey> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
