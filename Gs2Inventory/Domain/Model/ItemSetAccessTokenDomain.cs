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

    public partial class ItemSetAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;

        private readonly String _parentKey;
        public string Body { get; set; }
        public string Signature { get; set; }
        public long? OverflowCount { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;
        public string ItemSetName => _itemSetName;

        public ItemSetAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName,
            string itemName,
            string itemSetName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InventoryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._inventoryName = inventoryName;
            this._itemName = itemName;
            this._itemSetName = itemSetName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                this._inventoryName != null ? this._inventoryName.ToString() : null,
                "ItemSet"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            #else
        private IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
        #endif
            GetItemSetRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetItemSetFuture(
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
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                    foreach (var item in resultModel.Items)
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            item.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0)
                        {
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                _parentKey,
                                key
                            );
                        }
                        else
                        {
                            cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                item,
                                item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName.ToString(),
                    requestModel.ItemSetName.ToString()
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.GetItemSetAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                foreach (var item in resultModel.Items)
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        item.ItemName?.ToString(),
                        item.Name?.ToString()
                    );
                    if (item.Count == 0)
                    {
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key
                        );
                    }
                    else
                    {
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName.ToString(),
                    requestModel.ItemSetName
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Items);
        #else
            return result?.Items;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignature(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
        #endif
            GetItemWithSignatureRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetItemWithSignatureFuture(
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
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                foreach (var item in resultModel.Items)
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        item.ItemName.ToString(),
                        item.Name.ToString()
                    );
                    if (item.Count == 0)
                    {
                        _cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key
                        );
                    }
                    else
                    {
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) +
                            1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName.ToString(),
                    requestModel.ItemSetName.ToString()
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.GetItemWithSignatureAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                foreach (var item in resultModel.Items)
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        item.ItemName?.ToString(),
                        item.Name?.ToString()
                    );
                    if (item.Count == 0)
                    {
                        _cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key
                        );
                    }
                    else
                    {
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName.ToString(),
                    requestModel.ItemSetName
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain domain = null;
            if (result?.Items.Length > 0) {
                domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                this._accessToken,
                result?.Items[0]?.InventoryName,
                result?.Items[0]?.ItemName,
                "null"
                );
            } else {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(this);
            yield return null;
        #else
                return this;
        #endif
            }
            this.Body = domain.Body = result?.Body;
            this.Signature = domain.Signature = result?.Signature;
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> Consume(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
        #endif
            ConsumeItemSetRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.ConsumeItemSetFuture(
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
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                foreach (var item in resultModel.Items)
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        item.ItemName.ToString(),
                        item.Name.ToString()
                    );
                    if (item.Count == 0)
                    {
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key
                        );
                    }
                    else
                    {
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName.ToString(),
                    requestModel.ItemSetName
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.ConsumeItemSetAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    requestModel.NamespaceName.ToString(),
                    this._accessToken?.UserId.ToString(),
                    requestModel.InventoryName.ToString(),
                    "ItemSet"
                );
                foreach (var item in resultModel.Items)
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        item.ItemName.ToString(),
                        item.Name.ToString()
                    );
                    if (item.Count == 0)
                    {
                        _cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            key
                        );
                    }
                    else
                    {
                        cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var key2 = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    requestModel.ItemName?.ToString(),
                    requestModel.ItemSetName?.ToString()
                );
                long? expiresAt = null;
                foreach (var item in resultModel.Items)
                {
                    if (item.ExpiresAt > 0)
                    {
                        if (expiresAt.HasValue)
                        {
                            expiresAt = Math.Min(expiresAt.Value, item.ExpiresAt.Value);
                        }
                        else
                        {
                            expiresAt = item.ExpiresAt.Value;
                        }
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key2,
                    resultModel.Items,
                    expiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain domain = null;
            if (result?.Items.Length > 0) {
                domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                this._accessToken,
                result?.Items[0]?.InventoryName,
                result?.Items[0]?.ItemName,
                "null"
                );
            } else {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(this);
            yield return null;
        #else
                return this;
        #endif
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOfAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOf(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOfAsync(
        #endif
            AddReferenceOfRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.AddReferenceOfFuture(
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
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.ItemSet.UserId.ToString(),
                    resultModel.ItemSet.InventoryName.ToString(),
                        "ItemSet"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    resultModel.ItemSet.ItemName.ToString(),
                    resultModel.ItemSet.Name.ToString()
                );
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                    parentKey,
                    key,
                    resultModel.ItemSet,
                    resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #else
            var result = await this._client.AddReferenceOfAsync(
                request
            );
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
              
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.ItemSet.UserId.ToString(),
                    resultModel.ItemSet.InventoryName.ToString(),
                    resultModel.ItemSet.ItemName.ToString(),
                    resultModel.ItemSet.Name.ToString(),
                    "ReferenceOf"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                    requestModel.ReferenceOf
                );
                cache.Put(
                    parentKey,
                    key,
                    requestModel.ReferenceOf,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.ItemSet.UserId.ToString(),
                    resultModel.ItemSet.InventoryName.ToString(),
                        "ItemSet"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    resultModel.ItemSet.ItemName.ToString(),
                    resultModel.ItemSet.Name.ToString()
                );
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                    parentKey,
                    key,
                    resultModel.ItemSet,
                    resultModel.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    _inventoryName.ToString(),
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
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Inventory.UserId.ToString(),
                        "Inventory"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    resultModel.Inventory.InventoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            #endif
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.ItemSet.UserId.ToString(),
                    resultModel.ItemSet.InventoryName.ToString(),
                    "ItemSet"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    resultModel.ItemSet.ItemName.ToString(),
                    "null"
                );
                var items = cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key
                );
                for (var i=0; i<items.Length; i++)
                {
                    if (items[i].Name == resultModel.ItemSet.Name)
                    {
                        items[i] = resultModel.ItemSet;
                    }
                }
                cache.Put<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    parentKey,
                    key,
                    items,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[] domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
            for (int i=0; i<result?.Item.Length; i++)
            {
                domain[i] = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    this._accessToken,
                    request.InventoryName,
                    request.ItemName,
                    request.ItemSetName,
                    request.ReferenceOf
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> ReferenceOves(
        )
        {
            return new DescribeReferenceOfIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._inventoryName,
                this._accessToken,
                this._itemName,
                this._itemSetName
            );
        }

        public IUniTaskAsyncEnumerable<string> ReferenceOvesAsync(
            #else
        public Gs2Iterator<string> ReferenceOves(
            #endif
        #else
        public DescribeReferenceOfIterator ReferenceOves(
        #endif
        )
        {
            return new DescribeReferenceOfIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._inventoryName,
                this._accessToken,
                this._itemName,
                this._itemSetName
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

        public Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain ReferenceOf(
            string referenceOf
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._accessToken,
                this._inventoryName,
                this._itemName,
                this._itemSetName,
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.ItemSet[]> Model() {
            #else
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Model.ItemSet[]> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
        #endif
            Gs2.Gs2Inventory.Model.ItemSet[] value = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    this.ItemName?.ToString(),
                    this.ItemSetName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetItemSetRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "itemSet")
                            {
                                _cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                    _parentKey,
                                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                        this.ItemName?.ToString(),
                                        this.ItemSetName?.ToString()
                                    )
                                );
                            }
                            else
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    if (e.errors[0].component == "itemSet")
                    {
                        _cache.Delete<Gs2.Gs2Inventory.Model.ItemSet[]>(
                            _parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                this.ItemName?.ToString(),
                                this.ItemSetName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        this.ItemName?.ToString(),
                        this.ItemSetName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemSet[]>(Impl);
        #endif
        }

    }
}
