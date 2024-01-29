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
using Cysharp.Threading.Tasks.Linq;
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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;

        private readonly String _parentKey;
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int? ExpiresIn { get; set; }
        public string OwnerId { get; set; }

        public ProjectTokenDomain(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
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
                this.AccessToken = domain.AccessToken = result?.AccessToken;
                this.TokenType = domain.TokenType = result?.TokenType;
                this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
                this.OwnerId = domain.OwnerId = result?.OwnerId;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
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
            this.AccessToken = domain.AccessToken = result?.AccessToken;
            this.TokenType = domain.TokenType = result?.TokenType;
            this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
            this.OwnerId = domain.OwnerId = result?.OwnerId;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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
                if (resultModel != null) {
                    
                    {
                        var parentKey = string.Join(
                            ":",
                            "identifier",
                            "ProjectToken"
                        );
                        var key = Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                        );
                        _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
            #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
            #endif
            LoginByUserRequest request
        ) {
            LoginByUserResult result = null;
                result = await this._client.LoginByUserAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "identifier",
                        "ProjectToken"
                    );
                    var key = Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                    );
                    _gs2.Cache.Put(
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
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.ProjectToken>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Identifier.Model.ProjectToken>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
            #else
        public async Task<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
            #endif
        {
            var parentKey = string.Join(
                ":",
                "identifier",
                "ProjectToken"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Identifier.Model.ProjectToken>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Identifier.Model.ProjectToken>(
                    parentKey,
                    Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Identifier.Model.ProjectToken>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.ProjectToken>(
                _parentKey,
                Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain.CreateCacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
