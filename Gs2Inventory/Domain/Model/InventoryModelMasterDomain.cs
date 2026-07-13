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

    public partial class InventoryModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string InventoryName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public InventoryModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string inventoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.InventoryName = inventoryName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemModelMaster> ItemModelMasters(
        )
        {
            return new DescribeItemModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.ItemModelMaster> ItemModelMastersAsync(
        #else
        public DescribeItemModelMastersIterator ItemModelMastersAsync(
        #endif
        )
        {
            return new DescribeItemModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName
            );
        }

        public ulong SubscribeItemModelMasters(
            Action<Gs2.Gs2Inventory.Model.ItemModelMaster[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.ItemModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.InventoryName,
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
                            callback.Invoke(await ItemModelMastersAsync(
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
        public async UniTask<ulong> SubscribeItemModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeItemModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.ItemModelMaster[]> callback
        )
        {
            var items = await ItemModelMastersAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeItemModelMasters(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeItemModelMasters(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.ItemModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.InventoryName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateItemModelMasters(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.ItemModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.InventoryName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain ItemModelMaster(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.InventoryName,
                itemName
            );
        }

    }

    public partial class InventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> GetFuture(
            GetInventoryModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.InventoryModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.InventoryModelMaster> GetAsync(
        #endif
            GetInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetInventoryModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> UpdateFuture(
            UpdateInventoryModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> UpdateAsync(
        #endif
            UpdateInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateInventoryModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> DeleteFuture(
            DeleteInventoryModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> DeleteAsync(
        #endif
            DeleteInventoryModelMasterRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    null,
                    null,
                    () => this._client.DeleteInventoryModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMasterFuture(
            CreateItemModelMasterRequest request
        ) => CreateItemModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMasterAsync(
        #endif
            CreateItemModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateItemModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.InventoryName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class InventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.InventoryModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.InventoryModelMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                        (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheKey(
                            this.InventoryName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.InventoryName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.InventoryName,
                    null,
                    () => this.GetAsync(
                        new GetInventoryModelMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.InventoryName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.InventoryModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheKey(
                    this.InventoryName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.InventoryModelMaster).CacheKey(
                    this.InventoryName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.InventoryModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.InventoryModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.InventoryModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
