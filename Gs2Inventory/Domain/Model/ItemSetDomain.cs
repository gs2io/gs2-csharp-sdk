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

    public partial class ItemSetDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;

        private readonly String _parentKey;
        private readonly String _nullParentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public long? OverflowCount { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;
        public string ItemSetName => _itemSetName;

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
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._inventoryName = inventoryName;
            this._itemName = itemName;
            this._itemSetName = itemSetName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                "ItemSet"
            );
            this._nullParentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                "ItemSet:Null"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> ReferenceOves(
        )
        {
            return new DescribeReferenceOfByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                this.ItemName,
                this.ItemSetName
            );
        }

        public IUniTaskAsyncEnumerable<string> ReferenceOvesAsync(
            #else
        public Gs2Iterator<string> ReferenceOves(
            #endif
        #else
        public DescribeReferenceOfByUserIdIterator ReferenceOvesAsync(
        #endif
        )
        {
            return new DescribeReferenceOfByUserIdIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId,
                this.ItemName,
                this.ItemSetName
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
                userId ?? "null",
                inventoryName ?? "null",
                itemName ?? "null",
                itemSetName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string itemName,
            string itemSetName
        )
        {
            return string.Join(
                ":",
                itemName ?? "null",
                itemSetName ?? "null"
            );
        }

    }

    public partial class ItemSetDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> GetFuture(
            GetItemSetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.GetItemSetByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            request.ItemName.ToString(),
                            request.ItemSetName
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "itemSet")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key,
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        if (resultModel.Items.Length == 0) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                ItemName,
                                null
                            );
                            cache.Put(
                                _nullParentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                _nullParentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }
                }
                self.OnComplete(result?.Items);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            #else
        private async Task<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            #endif
            GetItemSetByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            GetItemSetByUserIdResult result = null;
            try {
                result = await this._client.GetItemSetByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    request.ItemName.ToString(),
                    request.ItemSetName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "itemSet")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0) {
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        else
                        {
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                    if (resultModel.Items.Length == 0) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            ItemName,
                            null
                        );
                        cache.Put(
                            _nullParentKey,
                            key,
                            resultModel.Items,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            _nullParentKey,
                            key,
                            items,
                            items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                        return items;
                    }).ToArray();
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Inventory != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Inventory.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Inventory.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Inventory,
                            resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                    }
                }
            }
            return result?.Items;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureFuture(
            GetItemWithSignatureByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.GetItemWithSignatureByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            request.ItemName.ToString(),
                            request.ItemSetName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "itemSet")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key,
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        if (resultModel.Items.Length == 0) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                ItemName,
                                null
                            );
                            cache.Put(
                                _nullParentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                _nullParentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }
                }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        null
                    );
                } else {
                    self.OnComplete(this);
                }
                this.Body = result?.Body;
                this.Signature = result?.Signature;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignatureAsync(
            #endif
            GetItemWithSignatureByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            GetItemWithSignatureByUserIdResult result = null;
            try {
                result = await this._client.GetItemWithSignatureByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    request.ItemName.ToString(),
                    request.ItemSetName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "itemSet")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0) {
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        else
                        {
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                    if (resultModel.Items.Length == 0) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            ItemName,
                            null
                        );
                        cache.Put(
                            _nullParentKey,
                            key,
                            resultModel.Items,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                            _nullParentKey,
                            key,
                            items,
                            items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                        return items;
                    }).ToArray();
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Inventory != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Inventory.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Inventory.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Inventory,
                            resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                    }
                }
            }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        result?.Items[0]?.Name
                    );
                } else {
                    return this;
                }
            this.Body = result?.Body;
            this.Signature = result?.Signature;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to GetItemWithSignatureFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> GetItemWithSignature(
            GetItemWithSignatureByUserIdRequest request
        ) {
            return GetItemWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireFuture(
            AcquireItemSetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.AcquireItemSetByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key,
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        if (resultModel.Items.Length == 0) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                ItemName,
                                null
                            );
                            cache.Put(
                                _nullParentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                                _nullParentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }
                }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        null
                    );
                } else {
                    self.OnComplete(this);
                }
                this.OverflowCount = result?.OverflowCount;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireAsync(
            #endif
            AcquireItemSetByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            AcquireItemSetByUserIdResult result = null;
                result = await this._client.AcquireItemSetByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0) {
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        else
                        {
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                    if (resultModel.Items.Length == 0) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            ItemName,
                            null
                        );
                        cache.Put(
                            _nullParentKey,
                            key,
                            resultModel.Items,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            _nullParentKey,
                            key,
                            items,
                            items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                        return items;
                    }).ToArray();
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Inventory != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Inventory.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Inventory.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Inventory,
                            resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                    }
                }
            }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        null
                    );
                } else {
                    return this;
                }
            this.OverflowCount = result?.OverflowCount;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AcquireFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> Acquire(
            AcquireItemSetByUserIdRequest request
        ) {
            return AcquireFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeFuture(
            ConsumeItemSetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.ConsumeItemSetByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key,
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        if (resultModel.Items.Length == 0) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                ItemName,
                                null
                            );
                            cache.Put(
                                _nullParentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                _nullParentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }
                }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        result?.Items[0]?.Name
                    );
                } else {
                    self.OnComplete(this);
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> ConsumeAsync(
            #endif
            ConsumeItemSetByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            ConsumeItemSetByUserIdResult result = null;
                result = await this._client.ConsumeItemSetByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0) {
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        else
                        {
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                    if (resultModel.Items.Length == 0) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            ItemName,
                            null
                        );
                        cache.Put(
                            _nullParentKey,
                            key,
                            resultModel.Items,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    var _ = resultModel.Items.Where(v => v.Count > 0).GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            _nullParentKey,
                            key,
                            items,
                            items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                        return items;
                    }).ToArray();
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Inventory != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Inventory.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Inventory.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Inventory,
                            resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                        );
                    }
                }
            }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        null
                    );
                } else {
                    return this;
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to ConsumeFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> Consume(
            ConsumeItemSetByUserIdRequest request
        ) {
            return ConsumeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteFuture(
            DeleteItemSetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.DeleteItemSetByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            request.ItemName.ToString(),
                            request.ItemSetName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "itemSet")
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    else {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                this._itemName.ToString(),
                                null
                            ),
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }
                }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        result?.Items[0]?.Name
                    );
                } else {
                    self.OnComplete(this);
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> DeleteAsync(
            #endif
            DeleteItemSetByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            DeleteItemSetByUserIdResult result = null;
            try {
                result = await this._client.DeleteItemSetByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    request.ItemName.ToString(),
                    request.ItemSetName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "itemSet")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                        parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this._itemName.ToString(),
                            null
                        ),
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                }
                if (resultModel.Inventory != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Inventory.InventoryName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                }
            }
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain domain = null;
                if (result?.Items.Length > 0) {
                    domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                        this._gs2,
                        request.NamespaceName,
                        result?.Items[0]?.UserId,
                        result?.Items[0]?.InventoryName,
                        result?.Items[0]?.ItemName,
                        result?.Items[0]?.Name
                    );
                } else {
                    return this;
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> Delete(
            DeleteItemSetByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyFuture(
            VerifyItemSetByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.VerifyItemSetByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> VerifyAsync(
            #endif
            VerifyItemSetByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            VerifyItemSetByUserIdResult result = null;
                result = await this._client.VerifyItemSetByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to VerifyFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> Verify(
            VerifyItemSetByUserIdRequest request
        ) {
            return VerifyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> AddReferenceOfFuture(
            AddReferenceOfByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.AddReferenceOfByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            resultModel.ItemSet.ItemName.ToString(),
                            resultModel.ItemSet.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemSet,
                            resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.ItemModel,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[result?.Item.Length ?? 0];
                for (int i=0; i<result?.Item.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                        this._gs2,
                        request.NamespaceName,
                        request.UserId,
                        request.InventoryName,
                        request.ItemName,
                        request.ItemSetName,
                        request.ReferenceOf
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> AddReferenceOfAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> AddReferenceOfAsync(
            #endif
            AddReferenceOfByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            AddReferenceOfByUserIdResult result = null;
                result = await this._client.AddReferenceOfByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        this.ItemName,
                        this.ItemSetName,
                        "ReferenceOf"
                    );
                    foreach (var item in resultModel.Item) {
                        this._gs2.Cache.Put(
                            parentKey,
                            item,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                if (resultModel.ItemSet != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        resultModel.ItemSet.ItemName.ToString(),
                        resultModel.ItemSet.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemSet,
                        resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.ItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[result?.Item.Length ?? 0];
            for (int i=0; i<result?.Item.Length; i++)
            {
                domain[i] = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                    this._gs2,
                    request.NamespaceName,
                    request.UserId,
                    request.InventoryName,
                    request.ItemName,
                    request.ItemSetName,
                    request.ReferenceOf
                );
            }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AddReferenceOfFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> AddReferenceOf(
            AddReferenceOfByUserIdRequest request
        ) {
            return AddReferenceOfFuture(request);
        }
        #endif

    }

    public partial class ItemSetDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                Gs2.Gs2Inventory.Model.ItemSet[] value;
                bool find = false;
                if (!string.IsNullOrEmpty(this.ItemSetName)) {
                    var v = this._gs2.Cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            this.ItemSetName?.ToString()
                        )
                    ).Item1;
                    value = null;
                    if (v != null) {
                        value = new[] {v};
                        find = true;
                    }
                }
                else {
                    var isListCached = this._gs2.Cache.IsListCached<Gs2.Gs2Inventory.Model.ItemSet>(this._parentKey);
                    if (isListCached) {
                        var items = this._gs2.Cache.ListForce<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey
                        );
                        if (items == null) {
                            value = null;
                        }
                        else {
                            value = items.Where(v => v.ItemName == ItemName).OrderByDescending(v => v.Count).ToArray();
                            find = true;
                        }
                    }
                    else {
                        value = null;
                    }
                }
                if (!find) {
                    var future = this.GetFuture(
                        new GetItemSetByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (string.IsNullOrEmpty(this.ItemSetName)) {
                                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                                    _parentKey,
                                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                        this.ItemName?.ToString(),
                                        null
                                    ),
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                            else {
                                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                    _parentKey,
                                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                        this.ItemName?.ToString(),
                                        this.ItemSetName?.ToString()
                                    ),
                                    null,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }

                            if (e.errors[0].component != "itemSet")
                            {
                                self.OnError(future.Error);
                                yield break;
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.ItemSetName)) {
                        var v = this._gs2.Cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                this.ItemName?.ToString(),
                                this.ItemSetName?.ToString()
                            )
                        ).Item1;
                        value = null;
                        if (v != null) {
                            value = new[] {v};
                            find = true;
                        }
                    }
                    else 
                    {
                        var items = this._gs2.Cache.ListForce<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey
                        );
                        if (items == null) {
                            value = null;
                        }
                        else {
                            value = items.Where(v => v.ItemName == ItemName).OrderByDescending(v => v.Count).ToArray();
                            find = true;
                        }
                    }
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
        {
            Gs2.Gs2Inventory.Model.ItemSet[] value;
            bool find = false;
            if (!string.IsNullOrEmpty(this.ItemSetName)) {
                var v = this._gs2.Cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        this.ItemName?.ToString(),
                        this.ItemSetName?.ToString()
                    )
                ).Item1;
                value = null;
                if (v != null) {
                    value = new[] {v};
                    find = true;
                }
            }
            else 
            {
                var isListCached = this._gs2.Cache.IsListCached<Gs2.Gs2Inventory.Model.ItemSet>(this._parentKey);
                if (isListCached) {
                    var items = this._gs2.Cache.ListForce<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey
                    );
                    if (items == null) {
                        value = null;
                    }
                    else {
                        value = items.Where(v => v.ItemName == ItemName).OrderByDescending(v => v.Count).ToArray();
                        find = true;
                    }
                }
                else {
                    value = null;
                }
            }
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetItemSetByUserIdRequest()
                    );
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        this.ItemName?.ToString(),
                        this.ItemSetName
                    );
                    this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "itemSet") {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(this.ItemSetName)) {
                    var v = this._gs2.Cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            this.ItemSetName?.ToString()
                        )
                    ).Item1;
                    value = null;
                    if (v != null) {
                        value = new[] {v};
                        find = true;
                    }
                }
                else 
                {
                    var items = this._gs2.Cache.ListForce<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey
                    );
                    if (items == null) {
                        value = null;
                    }
                    else {
                        value = items.Where(v => v.ItemName == ItemName).OrderByDescending(v => v.Count).ToArray();
                        find = true;
                    }
                }
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

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


        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.ItemSet> callback)
        {
            if (string.IsNullOrEmpty(this.ItemSetName)) {
                throw new InvalidOperationException();
            }
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    this.ItemName.ToString(),
                    this.ItemSetName
                ),
                callback
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback)
        {
            if (!string.IsNullOrEmpty(this.ItemSetName)) {
                throw new InvalidOperationException();
            }
            return this._gs2.Cache.Subscribe(
                _nullParentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    this.ItemName.ToString(),
                    null
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    this.ItemName.ToString(),
                    this.ItemSetName
                ),
                callbackId
            );
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.ItemSet[]>(
                _nullParentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    this.ItemName.ToString(),
                    this.ItemSetName
                ),
                callbackId
            );
        }

    }
}
