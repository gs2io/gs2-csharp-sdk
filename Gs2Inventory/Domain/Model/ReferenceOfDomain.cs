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

    public partial class ReferenceOfDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string InventoryName { get; } = null!;
        public string ItemName { get; } = null!;
        public string ItemSetName { get; } = null!;
        public string ReferenceOf { get; } = null!;

        public ReferenceOfDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.InventoryName = inventoryName;
            this.ItemName = itemName;
            this.ItemSetName = itemSetName;
            this.ReferenceOf = referenceOf;
        }

    }

    public partial class ReferenceOfDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<string> GetFuture(
            GetReferenceOfByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<string> GetAsync(
        #else
        private async Task<string> GetAsync(
        #endif
            GetReferenceOfByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId) /* diff +++ */
                .WithInventoryName(this.InventoryName)
/* diff --- start
                .WithUserId(this.UserId)
 diff --- end */
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetReferenceOfByUserIdAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> VerifyFuture(
            VerifyReferenceOfByUserIdRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> VerifyAsync(
        #endif
            VerifyReferenceOfByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId) /* diff +++ */
                .WithInventoryName(this.InventoryName)
/* diff --- start
                .WithUserId(this.UserId)
 diff --- end */
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyReferenceOfByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
/* diff --- start
                result?.Item?.Name
 diff --- end */
                result?.Item /* diff +++ */
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> DeleteFuture(
            DeleteReferenceOfByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> DeleteAsync(
        #endif
            DeleteReferenceOfByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId) /* diff +++ */
                    .WithInventoryName(this.InventoryName)
/* diff --- start
                    .WithUserId(this.UserId)
 diff --- end */
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteReferenceOfByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
/* diff --- start
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                result?.Item?.Name
            );
            return domain;
 diff --- end */
            return this; /* diff +++ */
        }

    }

    public partial class ReferenceOfDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<string> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<string> ModelAsync()
        #else
        public async Task<string> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<string>(
                        (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            this.ItemName,
                            this.ItemSetName,
                            null
                        ),
                        (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheKey(
                            this.ReferenceOf
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ReferenceOf).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    this.ReferenceOf,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Inventory.Model.ReferenceOf).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    this.ReferenceOf,
                    null,
                    () => this.GetAsync(
                        new GetReferenceOfByUserIdRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Inventory.Model.ReferenceOf).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Inventory.Model.ReferenceOf).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf,
                null,
                () => this.GetAsync(
                    new GetReferenceOfByUserIdRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<string> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<string> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<string> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<string> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<string> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<string> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Inventory.Model.ReferenceOf).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                this.ReferenceOf,
                null
            );
        }

        public ulong Subscribe(Action<string> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheKey(
                    this.ReferenceOf
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
            this._gs2.Cache.Unsubscribe<string>(
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    null
                ),
                (null as Gs2.Gs2Inventory.Model.ReferenceOf).CacheKey(
                    this.ReferenceOf
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<string> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<string> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<string> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<string> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
