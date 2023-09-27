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

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.Model
{

    public partial class SimpleInventoryDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string InventoryName => _inventoryName;

        public SimpleInventoryDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InventoryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._inventoryName = inventoryName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "SimpleInventory"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItems(
        )
        {
            return new DescribeSimpleItemsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItemsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.SimpleItem> SimpleItems(
            #endif
        #else
        public DescribeSimpleItemsByUserIdIterator SimpleItems(
        #endif
        )
        {
            return new DescribeSimpleItemsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeSimpleItems(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2Inventory.Model.SimpleItem>(
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "SimpleItem"
                ),
                callback
            );
        }

        public void UnsubscribeSimpleItems(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.SimpleItem>(
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "SimpleItem"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain SimpleItem(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                itemName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string inventoryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
                userId ?? "null",
                inventoryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string inventoryName
        )
        {
            return string.Join(
                ":",
                inventoryName ?? "null"
            );
        }

    }

    public partial class SimpleInventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> AcquireSimpleItemsFuture(
            AcquireSimpleItemsByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.AcquireSimpleItemsByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                AcquireSimpleItemsByUserIdResult result = null;
                    result = await this._client.AcquireSimpleItemsByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "SimpleItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                item.ItemName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> AcquireSimpleItemsAsync(
            AcquireSimpleItemsByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.AcquireSimpleItemsByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            AcquireSimpleItemsByUserIdResult result = null;
                result = await this._client.AcquireSimpleItemsByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            item.ItemName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> AcquireSimpleItemsAsync(
            AcquireSimpleItemsByUserIdRequest request
        ) {
            var future = AcquireSimpleItemsFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AcquireSimpleItemsFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> AcquireSimpleItems(
            AcquireSimpleItemsByUserIdRequest request
        ) {
            return AcquireSimpleItemsFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> ConsumeSimpleItemsFuture(
            ConsumeSimpleItemsByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.ConsumeSimpleItemsByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                ConsumeSimpleItemsByUserIdResult result = null;
                    result = await this._client.ConsumeSimpleItemsByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "SimpleItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                item.ItemName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> ConsumeSimpleItemsAsync(
            ConsumeSimpleItemsByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.ConsumeSimpleItemsByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                self.OnError(future.Error);
                yield break;
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            ConsumeSimpleItemsByUserIdResult result = null;
                result = await this._client.ConsumeSimpleItemsByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            item.ItemName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        result.Items[i]?.UserId,
                        request.InventoryName,
                        result.Items[i]?.ItemName
                    );
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        result.Items[i].ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> ConsumeSimpleItemsAsync(
            ConsumeSimpleItemsByUserIdRequest request
        ) {
            var future = ConsumeSimpleItemsFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ConsumeSimpleItemsFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain[]> ConsumeSimpleItems(
            ConsumeSimpleItemsByUserIdRequest request
        ) {
            return ConsumeSimpleItemsFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain> DeleteSimpleItemsFuture(
            DeleteSimpleItemsByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.DeleteSimpleItemsByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                DeleteSimpleItemsByUserIdResult result = null;
                try {
                    result = await this._client.DeleteSimpleItemsByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain> DeleteSimpleItemsAsync(
            DeleteSimpleItemsByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.DeleteSimpleItemsByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            DeleteSimpleItemsByUserIdResult result = null;
            try {
                result = await this._client.DeleteSimpleItemsByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain> DeleteSimpleItemsAsync(
            DeleteSimpleItemsByUserIdRequest request
        ) {
            var future = DeleteSimpleItemsFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteSimpleItemsFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain> DeleteSimpleItems(
            DeleteSimpleItemsByUserIdRequest request
        ) {
            return DeleteSimpleItemsFuture(request);
        }
        #endif

    }

    public partial class SimpleInventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.SimpleInventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.SimpleInventory>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.SimpleInventory> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.SimpleInventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleInventory> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.SimpleInventory> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.SimpleInventory> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Inventory.Model.SimpleInventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callbackId
            );
        }

    }
}
