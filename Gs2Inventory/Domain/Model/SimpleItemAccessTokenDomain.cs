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

    public partial class SimpleItemAccessTokenDomain {
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
        public string Body { get; set; }
        public string Signature { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;

        public SimpleItemAccessTokenDomain(
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
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                "SimpleItem"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.SimpleItem> GetAsync(
            #else
        private IFuture<Gs2.Gs2Inventory.Model.SimpleItem> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Inventory.Model.SimpleItem> GetAsync(
        #endif
            GetSimpleItemRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.SimpleItem> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetSimpleItemFuture(
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
            var result = await this._client.GetSimpleItemAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "SimpleItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemModelDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.SimpleItem>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain> GetWithSignatureAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain> GetWithSignature(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain> GetWithSignatureAsync(
        #endif
            GetSimpleItemWithSignatureRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetSimpleItemWithSignatureFuture(
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
            var result = await this._client.GetSimpleItemWithSignatureAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "SimpleItem"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        resultModel.Item.ItemName.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.SimpleItemModel != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryModelDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "SimpleItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemModelDomain.CreateCacheKey(
                        resultModel.SimpleItemModel.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.SimpleItemModel,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain domain = this;
            domain.Body = result?.Body;
            domain.Signature = result?.Signature;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.SimpleItemAccessTokenDomain>(Impl);
        #endif
        }

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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.SimpleItem> Model() {
            #else
        public IFuture<Gs2.Gs2Inventory.Model.SimpleItem> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Model.SimpleItem> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.SimpleItem> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Inventory.Model.SimpleItem>(
                       _parentKey,
                       Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.SimpleItem>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                    this.ItemName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetSimpleItemRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                    this.ItemName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.SimpleItem>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "simpleItem")
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
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.SimpleItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "simpleItem")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.SimpleItem>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        this.ItemName?.ToString()
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.SimpleItem>(Impl);
        #endif
        }

    }
}
