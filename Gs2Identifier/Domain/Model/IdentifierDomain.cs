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

    public partial class IdentifierDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2IdentifierRestClient _client;
        public string UserName { get; } = null!;
        public string ClientId { get; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string Status { get; set; } = null!;

        public IdentifierDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string userName,
            string clientId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2IdentifierRestClient(
                gs2.RestSession
            );
            this.UserName = userName;
            this.ClientId = clientId;
        }

    }

    public partial class IdentifierDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Identifier.Model.Identifier> GetFuture(
            GetIdentifierRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Identifier.Model.Identifier> GetAsync(
        #else
        private async Task<Gs2.Gs2Identifier.Model.Identifier> GetAsync(
        #endif
            GetIdentifierRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName)
                .WithClientId(this.ClientId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetIdentifierAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteFuture(
            DeleteIdentifierRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain> DeleteAsync(
        #endif
            DeleteIdentifierRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithUserName(this.UserName)
                    .WithClientId(this.ClientId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteIdentifierAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> AttachGuardFuture(
            AttachGuardRequest request
        ) => AttachGuardAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> AttachGuardAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> AttachGuardAsync(
        #endif
            AttachGuardRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName)
                .WithClientId(this.ClientId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.AttachGuardAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
/* diff --- start
                this._gs2
 diff --- end */
/* diff +++ start */
                this._gs2,
                this.UserName,
                this.ClientId
/* diff +++ end */
            )).ToArray() ?? Array.Empty<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain>();
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> DetachGuardFuture(
            DetachGuardRequest request
        ) => DetachGuardAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> DetachGuardAsync(
        #else
        public async Task<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain[]> DetachGuardAsync(
        #endif
            DetachGuardRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithUserName(this.UserName)
                .WithClientId(this.ClientId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.DetachGuardAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Identifier.Domain.Model.IdentifierDomain(
/* diff --- start
                this._gs2
 diff --- end */
/* diff +++ start */
                this._gs2,
                this.UserName,
                this.ClientId
/* diff +++ end */
            )).ToArray() ?? Array.Empty<Gs2.Gs2Identifier.Domain.Model.IdentifierDomain>();
            return domain;
        }

    }

    public partial class IdentifierDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Identifier.Model.Identifier> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Identifier.Model.Identifier> ModelAsync()
        #else
        public async Task<Gs2.Gs2Identifier.Model.Identifier> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Identifier.Model.Identifier>(
                        (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                            this.UserName,
                            null
                        ),
                        (null as Gs2.Gs2Identifier.Model.Identifier).CacheKey(
                            this.ClientId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Identifier.Model.Identifier).GetCache(
                    this._gs2.Cache,
                    this.UserName,
                    this.ClientId,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Identifier.Model.Identifier).FetchAsync(
                    this._gs2.Cache,
                    this.UserName,
                    this.ClientId,
                    null,
                    () => this.GetAsync(
                        new GetIdentifierRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Identifier.Model.Identifier).GetCache(
                this._gs2.Cache,
                this.UserName,
                this.ClientId,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Identifier.Model.Identifier).FetchAsync(
                this._gs2.Cache,
                this.UserName,
                this.ClientId,
                null,
                () => this.GetAsync(
                    new GetIdentifierRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Identifier.Model.Identifier> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Identifier.Model.Identifier> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Identifier.Model.Identifier> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Identifier.Model.Identifier> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Identifier.Model.Identifier).DeleteCache(
                this._gs2.Cache,
                this.UserName,
                this.ClientId,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Identifier.Model.Identifier> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                    this.UserName,
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheKey(
                    this.ClientId
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Identifier.Model.Identifier>(
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheParentKey(
                    this.UserName,
                    null
                ),
                (null as Gs2.Gs2Identifier.Model.Identifier).CacheKey(
                    this.ClientId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.Identifier> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Identifier.Model.Identifier> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.Identifier> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Identifier.Model.Identifier> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
