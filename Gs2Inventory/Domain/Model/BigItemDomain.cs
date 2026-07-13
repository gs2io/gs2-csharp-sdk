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

    public partial class BigItemDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string InventoryName { get; } = null!;
        public string ItemName { get; } = null!;

        public BigItemDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.InventoryName = inventoryName;
            this.ItemName = itemName;
        }

    }

    public partial class BigItemDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.BigItem> GetFuture(
            GetBigItemByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.BigItem> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.BigItem> GetAsync(
        #endif
            GetBigItemByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId)
                .WithItemName(this.ItemName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetBigItemByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> AcquireFuture(
            AcquireBigItemByUserIdRequest request
        ) => AcquireAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> AcquireAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> AcquireAsync(
        #endif
            AcquireBigItemByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId)
                .WithItemName(this.ItemName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AcquireBigItemByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> ConsumeFuture(
            ConsumeBigItemByUserIdRequest request
        ) => ConsumeAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> ConsumeAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> ConsumeAsync(
        #endif
            ConsumeBigItemByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId)
                .WithItemName(this.ItemName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.ConsumeBigItemByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> SetFuture(
            SetBigItemByUserIdRequest request
        ) => SetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> SetAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> SetAsync(
        #endif
            SetBigItemByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithUserId(this.UserId)
                .WithItemName(this.ItemName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SetBigItemByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> DeleteFuture(
            DeleteBigItemByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> DeleteAsync(
        #endif
            DeleteBigItemByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName)
                    .WithUserId(this.UserId)
                    .WithItemName(this.ItemName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteBigItemByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> VerifyFuture(
            VerifyBigItemByUserIdRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemDomain> VerifyAsync(
        #endif
            VerifyBigItemByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyBigItemByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class BigItemDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.BigItem> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.BigItem> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.BigItem> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.BigItem>(
                        (null as Gs2.Gs2Inventory.Model.BigItem).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.BigItem).CacheKey(
                            this.ItemName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.BigItem).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.BigItem).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    null,
                    () => this.GetAsync(
                        new GetBigItemByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Inventory.Model.BigItem> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.BigItem> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.BigItem> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.BigItem).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.BigItem> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.BigItem).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.BigItem).CacheKey(
                    this.ItemName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.BigItem>(
                (null as Gs2.Gs2Inventory.Model.BigItem).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.BigItem).CacheKey(
                    this.ItemName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.BigItem> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.BigItem> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.BigItem> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
