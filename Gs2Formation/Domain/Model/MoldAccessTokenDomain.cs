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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class MoldAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2FormationRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string MoldModelName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public MoldAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string moldModelName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2FormationRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.MoldModelName = moldModelName;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.Mold> GetFuture(
            GetMoldRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Formation.Model.Mold> GetAsync(
        #else
        private async Task<Gs2.Gs2Formation.Model.Mold> GetAsync(
        #endif
            GetMoldRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithMoldModelName(this.MoldModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetMoldAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> SubCapacityFuture(
            SubMoldCapacityRequest request
        ) => SubCapacityAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> SubCapacityAsync(
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> SubCapacityAsync(
        #endif
            SubMoldCapacityRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithMoldModelName(this.MoldModelName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.SubMoldCapacityAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteFuture(
            DeleteMoldRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldAccessTokenDomain> DeleteAsync(
        #endif
            DeleteMoldRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithMoldModelName(this.MoldModelName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.DeleteMoldAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Formation.Model.Form> Forms(
        )
        {
            return new DescribeFormsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.MoldModelName,
                this.AccessToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Formation.Model.Form> FormsAsync(
        #else
        public DescribeFormsIterator FormsAsync(
        #endif
        )
        {
            return new DescribeFormsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.MoldModelName,
                this.AccessToken
            );
        }

        public ulong SubscribeForms(
            Action<Gs2.Gs2Formation.Model.Form[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Formation.Model.Form>(
                (null as Gs2.Gs2Formation.Model.Form).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.AccessToken?.TimeOffset
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await FormsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeFormsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeFormsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Formation.Model.Form[]> callback
        )
        {
            var items = await FormsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeForms(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeForms(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Formation.Model.Form>(
                (null as Gs2.Gs2Formation.Model.Form).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateForms(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Formation.Model.Form>(
                (null as Gs2.Gs2Formation.Model.Form).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain Form(
            int? index
        ) {
            return new Gs2.Gs2Formation.Domain.Model.FormAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.MoldModelName,
                index
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.Mold> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.Mold> ModelAsync()
        #else
        public async Task<Gs2.Gs2Formation.Model.Mold> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Formation.Model.Mold>(
                        (null as Gs2.Gs2Formation.Model.Mold).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Formation.Model.Mold).CacheKey(
                            this.MoldModelName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Formation.Model.Mold).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Formation.Model.Mold).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.MoldModelName,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetMoldRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Formation.Model.Mold> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.Mold> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Formation.Model.Mold> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Formation.Model.Mold).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.MoldModelName,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Formation.Model.Mold> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Formation.Model.Mold).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Formation.Model.Mold).CacheKey(
                    this.MoldModelName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Formation.Model.Mold>(
                (null as Gs2.Gs2Formation.Model.Mold).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Formation.Model.Mold).CacheKey(
                    this.MoldModelName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Formation.Model.Mold> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Mold> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Formation.Model.Mold> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
