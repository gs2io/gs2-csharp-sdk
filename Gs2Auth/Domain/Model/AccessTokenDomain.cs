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
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2AuthRestClient _client;

        private readonly String _parentKey;
        public string Token { get; set; }
        public string UserId { get; set; }
        public long? Expire { get; set; }

        public AccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2AuthRestClient(
                session
            );
            this._parentKey = "auth:Gs2.Gs2Auth.Model.AccessToken";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
            #else
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> Login(
            #endif
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginAsync(
        #endif
            LoginRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            #else
            var result = await this._client.LoginAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Auth.Domain.Model.AccessTokenDomain domain = this;
            this._cache.Put(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
            #else
        public IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignature(
            #endif
        #else
        public async Task<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> LoginBySignatureAsync(
        #endif
            LoginBySignatureRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
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
            #else
            var result = await this._client.LoginBySignatureAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            Gs2.Gs2Auth.Domain.Model.AccessTokenDomain domain = this;
            this._cache.Put(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Domain.Model.AccessTokenDomain>(Impl);
        #endif
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Auth.Model.AccessToken> Model() {
            #else
        public IFuture<Gs2.Gs2Auth.Model.AccessToken> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Auth.Model.AccessToken> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Auth.Model.AccessToken> self)
            {
        #endif
            Gs2.Gs2Auth.Model.AccessToken value = _cache.Get<Gs2.Gs2Auth.Model.AccessToken>(
                _parentKey,
                Gs2.Gs2Auth.Domain.Model.AccessTokenDomain.CreateCacheKey(
                )
            );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Auth.Model.AccessToken>(Impl);
        #endif
        }

    }
}
