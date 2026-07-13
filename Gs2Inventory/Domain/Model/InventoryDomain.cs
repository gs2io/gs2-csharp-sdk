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

    public partial class InventoryDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string InventoryName { get; } = null!;
        public long? OverflowCount { get; set; } = null!;
        public string NextPageToken { get; set; } = null!;

        public InventoryDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.InventoryName = inventoryName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemSet> ItemSets(
            string timeOffsetToken = null
        )
        {
            return new DescribeItemSetsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.ItemSet> ItemSetsAsync(
        #else
        public DescribeItemSetsByUserIdIterator ItemSetsAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeItemSetsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                timeOffsetToken
            );
        }

        public ulong SubscribeItemSets(
            Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
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
                            callback.Invoke(await ItemSetsAsync(
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
        public async UniTask<ulong> SubscribeItemSetsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeItemSetsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback
        )
        {
            var items = await ItemSetsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeItemSets(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeItemSets(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateItemSets(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Inventory.Model.ItemSet>(
                (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                )
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.ItemSetDomain ItemSet(
            string itemName,
            string itemSetName = null
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                itemName,
                itemSetName
            );
        }

    }

    public partial class InventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.Inventory> GetFuture(
            GetInventoryByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.Inventory> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.Inventory> GetAsync(
        #endif
            GetInventoryByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetInventoryByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityFuture(
            AddCapacityByUserIdRequest request
        ) => AddCapacityAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
        #endif
            AddCapacityByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AddCapacityByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityFuture(
            SetCapacityByUserIdRequest request
        ) => SetCapacityAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
        #endif
            SetCapacityByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetCapacityByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteFuture(
            DeleteInventoryByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
        #endif
            DeleteInventoryByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName)
                    .WithUserId(this.UserId);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteInventoryByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityFuture(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) => VerifyCurrentMaxCapacityAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
        #endif
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyInventoryCurrentMaxCapacityByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeFuture(
            AcquireItemSetWithGradeByUserIdRequest request
        ) => AcquireItemSetWithGradeAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeAsync(
        #endif
            AcquireItemSetWithGradeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AcquireItemSetWithGradeByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.InventoryName,
                result?.Item?.ItemName,
                result?.Item?.Name
            );
            domain.OverflowCount = result?.OverflowCount;

            return domain;
        }

    }

    public partial class InventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.Inventory> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.Inventory>(
                        (null as Gs2.Gs2Inventory.Model.Inventory).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.Inventory).CacheKey(
                            this.InventoryName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.Inventory).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.Inventory).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null,
                    () => this.GetAsync(
                        new GetInventoryByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Inventory.Model.Inventory> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.Inventory> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.Inventory> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.Inventory).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.Inventory>(
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.Inventory).CacheKey(
                    this.InventoryName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.Inventory> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
