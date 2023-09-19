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

    public partial class BigItemAccessTokenDomain {
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

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;

        public BigItemAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName,
            string itemName
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
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                "BigItem"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.BigItem> GetFuture(
            GetBigItemRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigItem> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                var future = this._client.GetBigItemFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            request.ItemName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "bigItem")
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
                    .WithItemName(this.ItemName);
                GetBigItemResult result = null;
                try {
                    result = await this._client.GetBigItemAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        request.ItemName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "bigItem")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "BigItem"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            resultModel.Item.ItemName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "BigItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelDomain.CreateCacheKey(
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigItem>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inventory.Model.BigItem> GetAsync(
            GetBigItemRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var future = this._client.GetBigItemFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        request.ItemName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "bigItem")
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
                .WithItemName(this.ItemName);
            GetBigItemResult result = null;
            try {
                result = await this._client.GetBigItemAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                    request.ItemName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "bigItem")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "BigItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        resultModel.Item.ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.ItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "BigItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelDomain.CreateCacheKey(
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain> ConsumeFuture(
            ConsumeBigItemRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                var future = this._client.ConsumeBigItemFuture(
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
                    .WithItemName(this.ItemName);
                ConsumeBigItemResult result = null;
                    result = await this._client.ConsumeBigItemAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "BigItem"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            resultModel.Item.ItemName.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain> ConsumeAsync(
            ConsumeBigItemRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var future = this._client.ConsumeBigItemFuture(
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
                .WithItemName(this.ItemName);
            ConsumeBigItemResult result = null;
                result = await this._client.ConsumeBigItemAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "BigItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        resultModel.Item.ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain> ConsumeAsync(
            ConsumeBigItemRequest request
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemAccessTokenDomain> Consume(
            ConsumeBigItemRequest request
        ) {
            return ConsumeFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
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
                childType
            );
        }

        public static string CreateCacheKey(
            string itemName
        )
        {
            return string.Join(
                ":",
                itemName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.BigItem> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigItem> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigItem>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        this.ItemName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBigItemRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                                    this.ItemName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "bigItem")
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
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigItem>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigItem>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.BigItem> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigItem>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                        this.ItemName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBigItemRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                                    this.ItemName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "bigItem")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigItem>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigItemDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.BigItem> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.BigItem> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.BigItem> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.BigItem> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
