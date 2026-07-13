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

    public partial class BigInventoryModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string InventoryName { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public BigInventoryModelMasterDomain(
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
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigItemModelMaster> BigItemModelMasters(
            string namePrefix = null
        )
        {
            return new DescribeBigItemModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                namePrefix
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigItemModelMaster> BigItemModelMastersAsync(
        #else
        public DescribeBigItemModelMastersIterator BigItemModelMastersAsync(
        #endif
            string namePrefix = null
        )
        {
            return new DescribeBigItemModelMastersIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                namePrefix
            );
        }

        public ulong SubscribeBigItemModelMasters(
            Action<Gs2.Gs2Inventory.Model.BigItemModelMaster[]> callback,
            string namePrefix = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigItemModelMaster).CacheParentKey(
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
                            callback.Invoke(await BigItemModelMastersAsync(
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
        public async UniTask<ulong> SubscribeBigItemModelMastersWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeBigItemModelMastersWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.BigItemModelMaster[]> callback,
            string namePrefix = null
        )
        {
            var items = await BigItemModelMastersAsync(
                namePrefix
            ).ToArrayAsync();
            var callbackId = SubscribeBigItemModelMasters(
                callback,
                namePrefix
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeBigItemModelMasters(
            ulong callbackId,
            string namePrefix = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigItemModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.InventoryName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateBigItemModelMasters(
            string namePrefix = null
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigItemModelMaster).CacheParentKey(
                    this.NamespaceName,
                    this.InventoryName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain BigItemModelMaster(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.InventoryName,
                itemName
            );
        }

    }

    public partial class BigInventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> GetFuture(
            GetBigInventoryModelMasterRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> GetAsync(
        #endif
            GetBigInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetBigInventoryModelMasterAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateFuture(
            UpdateBigInventoryModelMasterRequest request
        ) => UpdateAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateAsync(
        #endif
            UpdateBigInventoryModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.UpdateBigInventoryModelMasterAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteFuture(
            DeleteBigInventoryModelMasterRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteAsync(
        #endif
            DeleteBigInventoryModelMasterRequest request
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
                    () => this._client.DeleteBigInventoryModelMasterAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterFuture(
            CreateBigItemModelMasterRequest request
        ) => CreateBigItemModelMasterAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterAsync(
        #endif
            CreateBigItemModelMasterRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.CreateBigItemModelMasterAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain(
                this._gs2,
                this.NamespaceName,
                this.InventoryName,
                result?.Item?.Name
            );

            return domain;
        }

    }

    public partial class BigInventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheKey(
                            this.InventoryName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.InventoryName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.InventoryName,
                    null,
                    () => this.GetAsync(
                        new GetBigInventoryModelMasterRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.InventoryName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.BigInventoryModelMaster).CacheKey(
                    this.InventoryName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
