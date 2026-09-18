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
using Gs2.Gs2Enhance.Domain.Iterator;
using Gs2.Gs2Enhance.Model.Cache;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Enhance.Result;
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

namespace Gs2.Gs2Enhance.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnhanceRestClient _client;
        public string NamespaceName { get; } = null!;
        public string Status { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string UploadToken { get; set; } = null!;
        public string UploadUrl { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public NamespaceDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnhanceRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2Enhance.Domain.Model.CurrentRateMasterDomain CurrentRateMaster(
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.CurrentRateMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enhance.Model.UnleashRateModel> UnleashRateModels(
        )
        {
            return new DescribeUnleashRateModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enhance.Model.UnleashRateModel> UnleashRateModelsAsync(
        #else
        public DescribeUnleashRateModelsIterator UnleashRateModelsAsync(
        #endif
        )
        {
            return new DescribeUnleashRateModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeUnleashRateModels(
            Action<Gs2.Gs2Enhance.Model.UnleashRateModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModel).CacheParentKey(
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
                            callback.Invoke(await UnleashRateModelsAsync(
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
        public async UniTask<ulong> SubscribeUnleashRateModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeUnleashRateModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Enhance.Model.UnleashRateModel[]> callback
        )
        {
            var items = await UnleashRateModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeUnleashRateModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeUnleashRateModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateUnleashRateModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Enhance.Model.UnleashRateModel>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain UnleashRateModel(
            string rateName
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.UnleashRateModelDomain(
                this._gs2,
                this.NamespaceName,
                rateName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> UnleashRateModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeUnleashRateModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enhance.Model.UnleashRateModelMaster> UnleashRateModelMastersAsync(
        #else
        public DescribeUnleashRateModelMastersIterator UnleashRateModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeUnleashRateModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeUnleashRateModelMasters(
            Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await UnleashRateModelMastersAsync(
                                namePrefix
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
        public async UniTask<ulong> SubscribeUnleashRateModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeUnleashRateModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Enhance.Model.UnleashRateModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await UnleashRateModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeUnleashRateModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeUnleashRateModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateUnleashRateModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Enhance.Model.UnleashRateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.UnleashRateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain UnleashRateModelMaster(
            string rateName
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                rateName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enhance.Model.RateModel> RateModels(
        )
        {
            return new DescribeRateModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enhance.Model.RateModel> RateModelsAsync(
        #else
        public DescribeRateModelsIterator RateModelsAsync(
        #endif
        )
        {
            return new DescribeRateModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeRateModels(
            Action<Gs2.Gs2Enhance.Model.RateModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enhance.Model.RateModel>(
                (null as Gs2.Gs2Enhance.Model.RateModel).CacheParentKey(
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
                            callback.Invoke(await RateModelsAsync(
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
        public async UniTask<ulong> SubscribeRateModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeRateModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Enhance.Model.RateModel[]> callback
        )
        {
            var items = await RateModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeRateModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeRateModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enhance.Model.RateModel>(
                (null as Gs2.Gs2Enhance.Model.RateModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRateModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Enhance.Model.RateModel>(
                (null as Gs2.Gs2Enhance.Model.RateModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.RateModelDomain RateModel(
            string rateName
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.RateModelDomain(
                this._gs2,
                this.NamespaceName,
                rateName
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.UserDomain User(
            string userId
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.UserDomain(
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
        public Gs2Iterator<Gs2.Gs2Enhance.Model.RateModelMaster> RateModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeRateModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enhance.Model.RateModelMaster> RateModelMastersAsync(
        #else
        public DescribeRateModelMastersIterator RateModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeRateModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeRateModelMasters(
            Action<Gs2.Gs2Enhance.Model.RateModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enhance.Model.RateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.RateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => namePrefix == null || item.Name.StartsWith(namePrefix))
                    .ToArray()),
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
                            callback.Invoke(await RateModelMastersAsync(
                                namePrefix
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
        public async UniTask<ulong> SubscribeRateModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeRateModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Enhance.Model.RateModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await RateModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeRateModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeRateModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enhance.Model.RateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.RateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRateModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Enhance.Model.RateModelMaster>(
                (null as Gs2.Gs2Enhance.Model.RateModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain RateModelMaster(
            string rateName
        ) {
            return new Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                rateName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> GetStatusAsync(
        #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> GetStatusAsync(
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
        private IFuture<Gs2.Gs2Enhance.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Enhance.Model.Namespace> GetAsync(
        #else
        private async Task<Gs2.Gs2Enhance.Model.Namespace> GetAsync(
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
        public IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> UpdateAsync(
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
        public IFuture<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.NamespaceDomain> DeleteAsync(
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
        public IFuture<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> CreateUnleashRateModelMasterFuture(
            CreateUnleashRateModelMasterRequest request
        ) => CreateUnleashRateModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> CreateUnleashRateModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain> CreateUnleashRateModelMasterAsync(
        #endif
            CreateUnleashRateModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateUnleashRateModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Enhance.Domain.Model.UnleashRateModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain> CreateRateModelMasterFuture(
            CreateRateModelMasterRequest request
        ) => CreateRateModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain> CreateRateModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain> CreateRateModelMasterAsync(
        #endif
            CreateRateModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateRateModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Enhance.Domain.Model.RateModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Enhance.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Enhance.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2Enhance.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Enhance.Model.Namespace>(
                        (null as Gs2.Gs2Enhance.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Enhance.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Enhance.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Enhance.Model.Namespace).FetchAsync(
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
        public UniTask<Gs2.Gs2Enhance.Model.Namespace> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Enhance.Model.Namespace> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Enhance.Model.Namespace> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Enhance.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Enhance.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Enhance.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Enhance.Model.Namespace).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Enhance.Model.Namespace>(
                (null as Gs2.Gs2Enhance.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Enhance.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Enhance.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.Namespace> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Enhance.Model.Namespace> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
