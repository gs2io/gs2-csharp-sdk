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
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Model.Cache;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
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

namespace Gs2.Gs2Inventory.Domain.Model
{

    public partial class NamespaceDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
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
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
        }

        public Gs2.Gs2Inventory.Domain.Model.CurrentItemModelMasterDomain CurrentItemModelMaster(
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.CurrentItemModelMasterDomain(
                this._gs2,
                this.NamespaceName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModel> InventoryModels(
        )
        {
            return new DescribeInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.InventoryModel> InventoryModelsAsync(
        #else
        public DescribeInventoryModelsIterator InventoryModelsAsync(
        #endif
        )
        {
            return new DescribeInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeInventoryModels(
            Action<Gs2.Gs2Inventory.Model.InventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.InventoryModel>(
                (null as Gs2.Gs2Inventory.Model.InventoryModel).CacheParentKey(
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
                            callback.Invoke(await InventoryModelsAsync(
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
        public async UniTask<ulong> SubscribeInventoryModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeInventoryModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.InventoryModel[]> callback
        )
        {
            var items = await InventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.InventoryModel>(
                (null as Gs2.Gs2Inventory.Model.InventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateInventoryModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.InventoryModel>(
                (null as Gs2.Gs2Inventory.Model.InventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain InventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.UserDomain User(
            string userId = null
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.UserDomain(
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
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster> SimpleInventoryModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeSimpleInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster> SimpleInventoryModelMastersAsync(
        #else
        public DescribeSimpleInventoryModelMastersIterator SimpleInventoryModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeSimpleInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeSimpleInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster).CacheParentKey(
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
                            callback.Invoke(await SimpleInventoryModelMastersAsync(
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
        public async UniTask<ulong> SubscribeSimpleInventoryModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeSimpleInventoryModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await SimpleInventoryModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeSimpleInventoryModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeSimpleInventoryModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSimpleInventoryModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain SimpleInventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleInventoryModel> SimpleInventoryModels(
        )
        {
            return new DescribeSimpleInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleInventoryModel> SimpleInventoryModelsAsync(
        #else
        public DescribeSimpleInventoryModelsIterator SimpleInventoryModelsAsync(
        #endif
        )
        {
            return new DescribeSimpleInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeSimpleInventoryModels(
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModel).CacheParentKey(
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
                            callback.Invoke(await SimpleInventoryModelsAsync(
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
        public async UniTask<ulong> SubscribeSimpleInventoryModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeSimpleInventoryModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.SimpleInventoryModel[]> callback
        )
        {
            var items = await SimpleInventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSimpleInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeSimpleInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSimpleInventoryModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.SimpleInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.SimpleInventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain SimpleInventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModel> BigInventoryModels(
        )
        {
            return new DescribeBigInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigInventoryModel> BigInventoryModelsAsync(
        #else
        public DescribeBigInventoryModelsIterator BigInventoryModelsAsync(
        #endif
        )
        {
            return new DescribeBigInventoryModelsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }

        public ulong SubscribeBigInventoryModels(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModel[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.BigInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModel).CacheParentKey(
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
                            callback.Invoke(await BigInventoryModelsAsync(
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
        public async UniTask<ulong> SubscribeBigInventoryModelsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeBigInventoryModelsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.BigInventoryModel[]> callback
        )
        {
            var items = await BigInventoryModelsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeBigInventoryModels(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeBigInventoryModels(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.BigInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateBigInventoryModels(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.BigInventoryModel>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModel).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain BigInventoryModel(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> BigInventoryModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeBigInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> BigInventoryModelMastersAsync(
        #else
        public DescribeBigInventoryModelMastersIterator BigInventoryModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeBigInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeBigInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
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
                            callback.Invoke(await BigInventoryModelMastersAsync(
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
        public async UniTask<ulong> SubscribeBigInventoryModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeBigInventoryModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await BigInventoryModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeBigInventoryModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeBigInventoryModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateBigInventoryModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain BigInventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.InventoryModelMaster> InventoryModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.InventoryModelMaster> InventoryModelMastersAsync(
        #else
        public DescribeInventoryModelMastersIterator InventoryModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeInventoryModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                namePrefix
            );
        }

        public ulong SubscribeInventoryModelMasters(
            Action<Gs2.Gs2Inventory.Model.InventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
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
                            callback.Invoke(await InventoryModelMastersAsync(
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
        public async UniTask<ulong> SubscribeInventoryModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeInventoryModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.InventoryModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await InventoryModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeInventoryModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeInventoryModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateInventoryModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain InventoryModelMaster(
            string inventoryName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                inventoryName
            );
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusFuture(
            GetNamespaceStatusRequest request
        ) => GetStatusAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> GetStatusAsync(
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
        private IFuture<Gs2.Gs2Inventory.Model.Namespace> GetFuture(
            GetNamespaceRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.Namespace> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.Namespace> GetAsync(
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateFuture(
            UpdateNamespaceRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> UpdateAsync(
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteFuture(
            DeleteNamespaceRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> DeleteAsync(
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterFuture(
            CreateSimpleInventoryModelMasterRequest request
        ) => CreateSimpleInventoryModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain> CreateSimpleInventoryModelMasterAsync(
        #endif
            CreateSimpleInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateSimpleInventoryModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterFuture(
            CreateBigInventoryModelMasterRequest request
        ) => CreateBigInventoryModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> CreateBigInventoryModelMasterAsync(
        #endif
            CreateBigInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateBigInventoryModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterFuture(
            CreateInventoryModelMasterRequest request
        ) => CreateInventoryModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> CreateInventoryModelMasterAsync(
        #endif
            CreateInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateInventoryModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class NamespaceDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.Namespace> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.Namespace> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.Namespace> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.Namespace>(
                        (null as Gs2.Gs2Inventory.Model.Namespace).CacheParentKey(
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.Namespace).CacheKey(
                            this.NamespaceName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.Namespace).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.Namespace).FetchAsync(
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
        public UniTask<Gs2.Gs2Inventory.Model.Namespace> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.Namespace> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.Namespace> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.Namespace).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.Namespace>(
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheParentKey(
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.Namespace).CacheKey(
                    this.NamespaceName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.Namespace> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Namespace> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
