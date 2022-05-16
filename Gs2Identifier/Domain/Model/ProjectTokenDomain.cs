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
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class ProjectTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdentifierRestClient _client;

        private readonly String _parentKey;
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int? ExpiresIn { get; set; }

        public ProjectTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdentifierRestClient(
                session
            );
            this._parentKey = "identifier:Gs2.Gs2Identifier.Model.ProjectToken";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> Login(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
        #endif
            LoginRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> self)
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
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            #else
            var result = await this._client.LoginAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            #endif
            Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain domain = this;
            this.AccessToken = domain.AccessToken = result?.AccessToken;
            this.TokenType = domain.TokenType = result?.TokenType;
            this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
            #else
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUser(
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
        #endif
            LoginByUserRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.LoginByUserFuture(
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
            var cache = _cache;
              
            {
                var parentKey = string.Join(
                    ":",
                    "identifier",
                    "ProjectToken"
                );
                var key = Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.LoginByUserAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = string.Join(
                    ":",
                    "identifier",
                    "ProjectToken"
                );
                var key = Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string childType
        )
        {
            return string.Join(
                ":",
                "identifier",
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
        public async UniTask<Gs2.Gs2Identifier.Model.ProjectToken> Model() {
            #else
        public IFuture<Gs2.Gs2Identifier.Model.ProjectToken> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Identifier.Model.ProjectToken> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.ProjectToken> self)
            {
        #endif
            var parentKey = string.Join(
                ":",
                "identifier",
                "ProjectToken"
            );
            Gs2.Gs2Identifier.Model.ProjectToken value = _cache.Get<Gs2.Gs2Identifier.Model.ProjectToken>(
                parentKey,
                Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.ProjectToken>(Impl);
        #endif
        }

    }
}
