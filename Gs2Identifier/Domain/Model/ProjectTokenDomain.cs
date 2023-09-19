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
            this._parentKey = "identifier:ProjectToken";
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

    }

    public partial class ProjectTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginFuture(
            LoginRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                LoginResult result = null;
                    result = await this._client.LoginAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                this.AccessToken = domain.AccessToken = result?.AccessToken;
                this.TokenType = domain.TokenType = result?.TokenType;
                this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
            LoginRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            LoginResult result = null;
                result = await this._client.LoginAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            this.AccessToken = domain.AccessToken = result?.AccessToken;
            this.TokenType = domain.TokenType = result?.TokenType;
            this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
            LoginRequest request
        ) {
            var future = LoginFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to LoginFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> Login(
            LoginRequest request
        ) {
            return LoginFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserFuture(
            LoginByUserRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
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
                #else
                LoginByUserResult result = null;
                    result = await this._client.LoginByUserAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
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
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
            LoginByUserRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
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
            #else
            LoginByUserResult result = null;
                result = await this._client.LoginByUserAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
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
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
            LoginByUserRequest request
        ) {
            var future = LoginByUserFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to LoginByUserFuture.")]
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUser(
            LoginByUserRequest request
        ) {
            return LoginByUserFuture(request);
        }
        #endif

    }

    public partial class ProjectTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.ProjectToken> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Identifier.Model.ProjectToken> self)
            {
                var parentKey = string.Join(
                    ":",
                    "identifier",
                    "ProjectToken"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.ProjectToken>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.ProjectToken>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
        {
            var parentKey = string.Join(
                ":",
                "identifier",
                "ProjectToken"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Identifier.Model.ProjectToken>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Identifier.Model.ProjectToken> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.ProjectToken> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Identifier.Model.ProjectToken> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
