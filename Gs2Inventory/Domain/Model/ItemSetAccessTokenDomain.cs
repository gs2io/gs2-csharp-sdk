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
        public string UserId => _accessToken.UserId;
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
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                "ItemSet"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> GetFuture(
            GetItemSetRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.GetItemSetFuture(
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
                        _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                GetItemSetResult result = null;
                try {
                    result = await this._client.GetItemSetAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName.ToString(),
                        request.ItemSetName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
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
                                cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key
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
                        var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                parentKey,
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
        #else
        private async Task<Gs2.Gs2Inventory.Model.ItemSet[]> GetAsync(
            GetItemSetRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var future = this._client.GetItemSetFuture(
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
                    _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            GetItemSetResult result = null;
            try {
                result = await this._client.GetItemSetAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    request.ItemName.ToString(),
                    request.ItemSetName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
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
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key
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
                    var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            parentKey,
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureFuture(
            GetItemWithSignatureRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                var future = this._client.GetItemWithSignatureFuture(
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
                        _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                GetItemWithSignatureResult result = null;
                try {
                    result = await this._client.GetItemWithSignatureAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName.ToString(),
                        request.ItemSetName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
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
                                cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key
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
                                parentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                parentKey,
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
                        null
                    );
                } else {
                    self.OnComplete(this);
                }
                this.Body = result?.Body;
                this.Signature = result?.Signature;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
            GetItemWithSignatureRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            var future = this._client.GetItemWithSignatureFuture(
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
                    _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            GetItemWithSignatureResult result = null;
            try {
                result = await this._client.GetItemWithSignatureAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                    request.ItemName.ToString(),
                    request.ItemSetName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
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
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key
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
                    var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            parentKey,
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
                        null
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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignatureAsync(
            GetItemWithSignatureRequest request
        ) {
            var future = GetItemWithSignatureFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to GetItemWithSignatureFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> GetItemWithSignature(
            GetItemWithSignatureRequest request
        ) {
            return GetItemWithSignatureFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeFuture(
            ConsumeItemSetRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                ConsumeItemSetResult result = null;
                    result = await this._client.ConsumeItemSetAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
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
                                cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key
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
                                parentKey,
                                key,
                                resultModel.Items,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                        var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                parentKey,
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
                        null
                    );
                } else {
                    self.OnComplete(this);
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
            ConsumeItemSetRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            ConsumeItemSetResult result = null;
                result = await this._client.ConsumeItemSetAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
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
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key
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
                    var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                    {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            group.Key,
                            null
                        );
                        var items = group.ToArray();
                        cache.Put(
                            parentKey,
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
                        null
                    );
                } else {
                    return this;
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> ConsumeAsync(
            ConsumeItemSetRequest request
        ) {
            var future = ConsumeFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ConsumeFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetAccessTokenDomain> Consume(
            ConsumeItemSetRequest request
        ) {
            return ConsumeFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOfFuture(
            AddReferenceOfRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName);
                AddReferenceOfResult result = null;
                    result = await this._client.AddReferenceOfAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
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
                            this._cache.Put(
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOfAsync(
            AddReferenceOfRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName);
            AddReferenceOfResult result = null;
                result = await this._client.AddReferenceOfAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
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
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOfAsync(
            AddReferenceOfRequest request
        ) {
            var future = AddReferenceOfFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AddReferenceOfFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> AddReferenceOf(
            AddReferenceOfRequest request
        ) {
            return AddReferenceOfFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<string> ReferenceOves(
        )
        {
            return new DescribeReferenceOfIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken,
                this.ItemName,
                this.ItemSetName
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
                this.NamespaceName,
                this.InventoryName,
                this.AccessToken,
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

        public Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain ReferenceOf(
            string referenceOf
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this._accessToken,
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

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemSet[]> self)
            {
                Gs2.Gs2Inventory.Model.ItemSet[] value;
                bool find = false;
                if (!string.IsNullOrEmpty(this.ItemSetName)) {
                    var (v, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            this.ItemSetName?.ToString()
                        )
                    );
                    if (v == null) {
                        value = null;
                    }
                    else {
                        value = new[] {v};
                        find = true;
                    }
                }
                else 
                {
                    (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            null
                        )
                    );
                }
                if (!find) {
                    var future = this.GetFuture(
                        new GetItemSetRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    this.ItemName?.ToString(),
                                    this.ItemSetName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

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
                        var (v, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                            _parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                this.ItemName?.ToString(),
                                this.ItemSetName?.ToString()
                            )
                        );
                        if (v == null) {
                            value = null;
                        }
                        else {
                            value = new[] {v};
                        }
                    }
                    else 
                    {
                        (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                            _parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                this.ItemName?.ToString(),
                                null
                            )
                        );
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
                var (v, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        this.ItemName?.ToString(),
                        this.ItemSetName?.ToString()
                    )
                );
                if (v == null) {
                    value = null;
                }
                else {
                    value = new[] {v};
                    find = true;
                }
            }
            else 
            {
                (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        this.ItemName?.ToString(),
                        null
                    )
                );
            }
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetItemSetRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    this.ItemName?.ToString(),
                                    this.ItemSetName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "itemSet")
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(this.ItemSetName)) {
                    var (v, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            this.ItemSetName?.ToString()
                        )
                    );
                    if (v == null) {
                        value = null;
                    }
                    else {
                        value = new[] {v};
                    }
                }
                else 
                {
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.ItemSet[]>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            this.ItemName?.ToString(),
                            null
                        )
                    );
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

    }
}
