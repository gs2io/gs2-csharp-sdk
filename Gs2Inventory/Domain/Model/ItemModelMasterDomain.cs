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

    public partial class ItemModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _inventoryName;
        private readonly string _itemName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;

        public ItemModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
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
            this._inventoryName = inventoryName;
            this._itemName = itemName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._inventoryName != null ? this._inventoryName.ToString() : null,
                "ItemModelMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.ItemModelMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Inventory.Model.ItemModelMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Inventory.Model.ItemModelMaster> GetAsync(
        #endif
            GetItemModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemModelMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetItemModelMasterFuture(
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
            var result = await this._client.GetItemModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.InventoryName.ToString(),
                    "ItemModelMaster"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemModelMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> UpdateAsync(
        #endif
            UpdateItemModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateItemModelMasterFuture(
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
            var result = await this._client.UpdateItemModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.InventoryName.ToString(),
                    "ItemModelMaster"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> DeleteAsync(
        #endif
            DeleteItemModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteItemModelMasterFuture(
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
            DeleteItemModelMasterResult result = null;
            try {
                result = await this._client.DeleteItemModelMasterAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    resultModel.Item.InventoryName.ToString(),
                    "ItemModelMaster"
                );
                var key = Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Delete<Gs2.Gs2Inventory.Model.ItemModelMaster>(parentKey, key);
            }
            Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string inventoryName,
            string itemName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
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
        public async UniTask<Gs2.Gs2Inventory.Model.ItemModelMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Inventory.Model.ItemModelMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Model.ItemModelMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.ItemModelMaster> self)
            {
        #endif
            Gs2.Gs2Inventory.Model.ItemModelMaster value = _cache.Get<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                    this.ItemName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetItemModelMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "itemModelMaster")
                            {
                                _cache.Delete<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                                    _parentKey,
                                    Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                                        this.ItemName?.ToString()
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
                    if (e.errors[0].component == "itemModelMaster")
                    {
                    _cache.Delete<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                            _parentKey,
                            Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
                                this.ItemName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Inventory.Model.ItemModelMaster>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.ItemModelMaster>(Impl);
        #endif
        }

    }
}
