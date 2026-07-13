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

    public partial class SimpleItemDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string InventoryName { get; } = null!;
        public string ItemName { get; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;

        public SimpleItemDomain(
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

    public partial class SimpleItemDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.SimpleItem> GetFuture(
            GetSimpleItemByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.SimpleItem> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.SimpleItem> GetAsync(
        #endif
            GetSimpleItemByUserIdRequest request
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
                () => this._client.GetSimpleItemByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> GetWithSignatureFuture(
            GetSimpleItemWithSignatureByUserIdRequest request
        ) => GetWithSignatureAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> GetWithSignatureAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> GetWithSignatureAsync(
        #endif
            GetSimpleItemWithSignatureByUserIdRequest request
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
                () => this._client.GetSimpleItemWithSignatureByUserIdAsync(request)
            );
            var domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> VerifyFuture(
            VerifySimpleItemByUserIdRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain> VerifyAsync(
        #endif
            VerifySimpleItemByUserIdRequest request
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
                () => this._client.VerifySimpleItemByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

    }

    public partial class SimpleItemDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.SimpleItem> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleItem> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.SimpleItem> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.SimpleItem>(
                        (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheKey(
                            this.ItemName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.SimpleItem).GetCache(
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
                return await (null as Gs2.Gs2Inventory.Model.SimpleItem).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    null,
                    () => this.GetAsync(
                        new GetSimpleItemByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Inventory.Model.SimpleItem> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.SimpleItem> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Inventory.Model.SimpleItem> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.SimpleItem).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.SimpleItem> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheKey(
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.SimpleItem>(
                (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.SimpleItem).CacheKey(
                    this.ItemName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.SimpleItem> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.SimpleItem> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.SimpleItem> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
