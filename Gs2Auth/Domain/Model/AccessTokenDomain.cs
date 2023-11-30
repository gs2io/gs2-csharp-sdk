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
using Gs2.Gs2Auth.Request;
using Gs2.Gs2Auth.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Auth.Domain.Model
{

    public partial class AccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2AuthRestClient _client;

        private readonly String _parentKey;
        public string Token { get; set; }
        public string UserId { get; set; }
        public long? Expire { get; set; }
        public string Status { get; set; }

        public AccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2AuthRestClient(
                gs2.RestSession
            );
            this._parentKey = "auth:AccessToken";
        }

        public static string CreateCacheParentKey(
            string childType
        )
        {
            return string.Join(
                ":",
                "auth",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class AccessTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginFuture(
            LoginRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> self)
            {
                var future = this._client.LoginFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this._gs2.Cache.Put(
                    this._parentKey,
                    AccessTokenDomain.CreateCacheKey(),
                    new AccessToken()
                            .WithToken(result?.Token)
                            .WithUserId(result?.UserId)
                            .WithExpire(result?.Expire),
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * 15
                );
                this.Token = domain.Token = result?.Token;
                this.UserId = domain.UserId = result?.UserId;
                this.Expire = domain.Expire = result?.Expire;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
            #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
            #endif
            LoginRequest request
        ) {
            LoginResult result = null;
                result = await this._client.LoginAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this._gs2.Cache.Put(
                this._parentKey,
                AccessTokenDomain.CreateCacheKey(),
                new AccessToken()
                        .WithToken(result?.Token)
                        .WithUserId(result?.UserId)
                        .WithExpire(result?.Expire),
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * 15
            );
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to LoginFuture.")]
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> Login(
            LoginRequest request
        ) {
            return LoginFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureFuture(
            LoginBySignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> self)
            {
                var future = this._client.LoginBySignatureFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this._gs2.Cache.Put(
                    this._parentKey,
                    AccessTokenDomain.CreateCacheKey(),
                    new AccessToken()
                            .WithToken(result?.Token)
                            .WithUserId(result?.UserId)
                            .WithExpire(result?.Expire),
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * 15
                );
                this.Token = domain.Token = result?.Token;
                this.UserId = domain.UserId = result?.UserId;
                this.Expire = domain.Expire = result?.Expire;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
            #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
            #endif
            LoginBySignatureRequest request
        ) {
            LoginBySignatureResult result = null;
                result = await this._client.LoginBySignatureAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            this._gs2.Cache.Put(
                this._parentKey,
                AccessTokenDomain.CreateCacheKey(),
                new AccessToken()
                        .WithToken(result?.Token)
                        .WithUserId(result?.UserId)
                        .WithExpire(result?.Expire),
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * 15
            );
            this.Token = domain.Token = result?.Token;
            this.UserId = domain.UserId = result?.UserId;
            this.Expire = domain.Expire = result?.Expire;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to LoginBySignatureFuture.")]
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignature(
            LoginBySignatureRequest request
        ) {
            return LoginBySignatureFuture(request);
        }
        #endif

    }

    public partial class AccessTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Model.AccessToken> self)
            {
                var parentKey = string.Join(
                    ":",
                    "auth",
                    "AccessToken"
                );
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Auth.Model.AccessToken>(
                    parentKey,
                    Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Model.AccessToken>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Auth.Model.AccessToken> ModelAsync()
            #else
        public async Task<Gs2.Gs2Auth.Model.AccessToken> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "auth",
                "AccessToken"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Auth.Model.AccessToken>(
                _parentKey,
                Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Auth.Model.AccessToken>(
                    parentKey,
                    Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                    )
                );
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Auth.Model.AccessToken> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Auth.Model.AccessToken> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Auth.Model.AccessToken>(
                _parentKey,
                Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

    }
}
