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
using Gs2.Gs2MegaField.Domain.Iterator;
using Gs2.Gs2MegaField.Model.Cache;
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
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

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MegaFieldRestClient _client;
        public string NamespaceName { get; } = null!;
        public string Status { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MegaFieldRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2MegaField.Domain.Model.CurrentFieldMasterDomain CurrentFieldMaster(
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.CurrentFieldMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2MegaField.Model.AreaModel> AreaModels(
        )
        {
            return new DescribeAreaModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2MegaField.Model.AreaModel> AreaModelsAsync(
        #else
        public DescribeAreaModelsIterator AreaModelsAsync(
        #endif
        )
        {
            return new DescribeAreaModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeAreaModels(
            Action<Gs2.Gs2MegaField.Model.AreaModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2MegaField.Model.AreaModel>(
                (null as Gs2.Gs2MegaField.Model.AreaModel).CacheParentKey(
                    this.NamespaceName,
                    null
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
                            callback.Invoke(await AreaModelsAsync(
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
        public async UniTask<ulong> SubscribeAreaModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeAreaModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2MegaField.Model.AreaModel[]> callback
        )
        {
            var items = await AreaModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeAreaModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeAreaModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2MegaField.Model.AreaModel>(
                (null as Gs2.Gs2MegaField.Model.AreaModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateAreaModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2MegaField.Model.AreaModel>(
                (null as Gs2.Gs2MegaField.Model.AreaModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.AreaModelDomain AreaModel(
            string areaModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.AreaModelDomain(
                this._gs2,
                this.NamespaceName,
                areaModelName
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.UserDomain(
                this._gs2,
                this.NamespaceName,
                userId
            );
        }

        public UserAccessTokenDomain AccessToken(
            AccessToken accessToken
        ) {
            return new UserAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2MegaField.Model.AreaModelMaster> AreaModelMasters(
        )
        {
            return new DescribeAreaModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2MegaField.Model.AreaModelMaster> AreaModelMastersAsync(
        #else
        public DescribeAreaModelMastersIterator AreaModelMastersAsync(
        #endif
        )
        {
            return new DescribeAreaModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeAreaModelMasters(
            Action<Gs2.Gs2MegaField.Model.AreaModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
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
                            callback.Invoke(await AreaModelMastersAsync(
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
        public async UniTask<ulong> SubscribeAreaModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeAreaModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2MegaField.Model.AreaModelMaster[]> callback
        )
        {
            var items = await AreaModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeAreaModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeAreaModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateAreaModelMasters(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                (null as Gs2.Gs2MegaField.Model.AreaModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain AreaModelMaster(
            string areaModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                areaModelName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> GetStatusAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> GetStatusAsync(
        #endif
            GetNamespaceStatusRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetNamespaceStatusAsync(request)
            );
            var domain = this;
            this.Status = domain.Status = result?.Status;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2MegaField.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2MegaField.Model.Namespace> GetAsync(
        #else
        private async Task<Gs2.Gs2MegaField.Model.Namespace> GetAsync(
        #endif
            GetNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetNamespaceAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> UpdateAsync(
        #endif
            UpdateNamespaceRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateNamespaceAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.NamespaceDomain> DeleteAsync(
        #endif
            DeleteNamespaceRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteNamespaceAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> CreateAreaModelMasterFuture(
            CreateAreaModelMasterRequest request
        ) => CreateAreaModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> CreateAreaModelMasterAsync(
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> CreateAreaModelMasterAsync(
        #endif
            CreateAreaModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateAreaModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2MegaField.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2MegaField.Model.Namespace>(
                        (null as Gs2.Gs2MegaField.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2MegaField.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2MegaField.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2MegaField.Model.Namespace).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null,
                    () => this.GetAsync(
                        new GetNamespaceRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2MegaField.Model.Namespace> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.Namespace> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2MegaField.Model.Namespace> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2MegaField.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2MegaField.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2MegaField.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2MegaField.Model.Namespace).CacheKey(
                    this.NamespaceName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2MegaField.Model.Namespace>(
                (null as Gs2.Gs2MegaField.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2MegaField.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2MegaField.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2MegaField.Model.Namespace> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2MegaField.Model.Namespace> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
