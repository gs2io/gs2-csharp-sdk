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
 *
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

namespace Gs2.Gs2Inventory.Domain.Model
{

    public partial class ItemSetDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        public string NamespaceName { get; }
        public string UserId { get; }
        public string InventoryName { get; }
        public string ItemName { get; }
        public string ItemSetName { get; }
        public string Body { get; set; }
        public string Signature { get; set; }
        public long? OverflowCount { get; set; }

        public ItemSetDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName
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
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<string> ReferenceOves(
            string timeOffsetToken = null
        )
        {
            return new DescribeReferenceOfByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                this.ItemName,
                this.ItemSetName,
                timeOffsetToken
            );
        }
        #endif

            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<string> ReferenceOvesAsync(
            #else
        public DescribeReferenceOfByUserIdIterator ReferenceOvesAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeReferenceOfByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                this.ItemName,
                this.ItemSetName,
                timeOffsetToken
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain ReferenceOf(
            string referenceOf
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                referenceOf
            );
        }

    }

    public partial class ItemSetDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> GetFuture(
            GetItemSetByUserIdRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
        #else
        private async Task<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
        #endif
            GetItemSetByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetItemSetByUserIdAsync(request)
            );
            return result?.Items;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureFuture(
            GetItemWithSignatureByUserIdRequest request
        ) => GetItemWithSignatureAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureAsync(
        #endif
            GetItemWithSignatureByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetItemWithSignatureByUserIdAsync(request)
            );
            var domain = this;
            this.Body = result?.Body;
            this.Signature = result?.Signature;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireFuture(
            AcquireItemSetByUserIdRequest request
        ) => AcquireAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireAsync(
        #endif
            AcquireItemSetByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AcquireItemSetByUserIdAsync(request)
            );
            var domain = this;
            this.OverflowCount = result?.OverflowCount;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeFuture(
            ConsumeItemSetByUserIdRequest request
        ) => ConsumeAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeAsync(
        #endif
            ConsumeItemSetByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.ConsumeItemSetByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteFuture(
            DeleteItemSetByUserIdRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteAsync(
        #endif
            DeleteItemSetByUserIdRequest request
        ) {
            try {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteItemSetByUserIdAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyFuture(
            VerifyItemSetByUserIdRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyAsync(
        #endif
            VerifyItemSetByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyItemSetByUserIdAsync(request)
            );
            var domain = this;
            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> AddReferenceOfFuture(
            AddReferenceOfByUserIdRequest request
        ) => AddReferenceOfAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> AddReferenceOfAsync(
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain> AddReferenceOfAsync(
        #endif
            AddReferenceOfByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.AddReferenceOfByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                request.ReferenceOf
            );

            return domain;
        }

    }

    public partial class ItemSetDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
        #else
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
        #endif
        {
            if (this.ItemSetName == null) {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet[]).GetCache(
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
                return await (null as Gs2.Gs2Inventory.Model.ItemSet[]).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    null,
                    () => this.GetAsync(
                        new GetItemSetByUserIdRequest()
                    )
                );
            }
            else {
                var (value, find) = (null as Gs2.Gs2Inventory.Model.ItemSet).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    null
                );
                if (find) {
                    return value != null ? new []{ value } : Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>();
                }
                return await (null as Gs2.Gs2Inventory.Model.ItemSet).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    null,
                    () => this.GetAsync(
                        new GetItemSetByUserIdRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            if (this.ItemSetName == null) {
                (null as Gs2.Gs2Inventory.Model.ItemSet).DeleteCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    this.ItemSetName,
                    null
                );
            }
            else {
                (null as Gs2.Gs2Inventory.Model.ItemSet[]).DeleteCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    this.ItemName,
                    null
                );
            }
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
        {
            if (this.ItemSetName == null) {
                return this._gs2.Cache.Subscribe(
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        null
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheKey(
                        this.ItemName
                    ),
                    callback,
                    () =>
                    {
                        ModelAsync().Forget();
                    }
                );
            }
            else {
                return this._gs2.Cache.Subscribe(
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        null
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheKey(
                        this.ItemName,
                        this.ItemSetName
                    ),
                    callback,
                    () =>
                    {
                        ModelAsync().Forget();
                    }
                );
            }
        }

        public void Unsubscribe(ulong callbackId)
        {
            if (this.ItemSetName == null) {
                this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        null
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheKey(
                        this.ItemName
                    ),
                    callbackId
                );
            }
            else {
                this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                    (null as Gs2.Gs2Inventory.Model.ItemSet[]).CacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        null
                    ),
                    (null as Gs2.Gs2Inventory.Model.ItemSet).CacheKey(
                        this.ItemName,
                        this.ItemSetName
                    ),
                    callbackId
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
