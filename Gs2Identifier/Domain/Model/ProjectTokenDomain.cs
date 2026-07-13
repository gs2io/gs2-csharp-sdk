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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Identifier.Domain.Iterator;
using Gs2.Gs2Identifier.Model.Cache;
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Domain.Model
{

    public partial class ProjectTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        public string AccessToken { get; set; } = null!;
        public string TokenType { get; set; } = null!;
        public int? ExpiresIn { get; set; } = null!;
        public string OwnerId { get; set; } = null!;

        public ProjectTokenDomain(
            Gs2.Core.Domain.Gs2 gs2
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
        }

    }

    public partial class ProjectTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginFuture(
            LoginRequest request
        ) => LoginAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginAsync(
        #endif
            LoginRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.LoginAsync(request)
            );
            var domain = this;
            this.AccessToken = domain.AccessToken = result?.AccessToken;
            this.TokenType = domain.TokenType = result?.TokenType;
            this.ExpiresIn = domain.ExpiresIn = result?.ExpiresIn;
            this.OwnerId = domain.OwnerId = result?.OwnerId;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserFuture(
            LoginByUserRequest request
        ) => LoginByUserAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.ProjectTokenDomain> LoginByUserAsync(
        #endif
            LoginByUserRequest request
        ) {
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.LoginByUserAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class ProjectTokenDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.ProjectToken> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
        #else
        public async Task<Gs2.Gs2Identifier.Model.ProjectToken> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Identifier.Model.ProjectToken>(
                        (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheKey(
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Identifier.Model.ProjectToken).GetCache(
                    this._gs2.Cache,
                    null
                );
                if (find) {
                    return value;
                }
                return null;
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Identifier.Model.ProjectToken> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Identifier.Model.ProjectToken> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Identifier.Model.ProjectToken> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Identifier.Model.ProjectToken).DeleteCache(
                this._gs2.Cache,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.ProjectToken>(
                (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.ProjectToken).CacheKey(
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.ProjectToken> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
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

    }
}
