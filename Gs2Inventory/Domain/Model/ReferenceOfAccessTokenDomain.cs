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

    public partial class ReferenceOfAccessTokenDomain {
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
        private readonly string _referenceOf;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken?.UserId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;
        public string ItemSetName => _itemSetName;
        public string ReferenceOf => _referenceOf;

        public ReferenceOfAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
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
            this._referenceOf = referenceOf;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._accessToken?.UserId?.ToString(),
                this._inventoryName != null ? this._inventoryName.ToString() : null,
                this._itemName != null ? this._itemName.ToString() : null,
                this._itemSetName != null ? this._itemSetName.ToString() : null,
                "ReferenceOf"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<string[]> GetAsync(
            #else
        private IFuture<string[]> Get(
            #endif
        #else
        private async Task<string[]> GetAsync(
        #endif
            GetReferenceOfRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<string[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetReferenceOfFuture(
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
                cache.Put(
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
            var result = await this._client.GetReferenceOfAsync(
                request
            );
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
                cache.Put(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<string[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> VerifyAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> Verify(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> VerifyAsync(
        #endif
            VerifyReferenceOfRequest request
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
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.VerifyReferenceOfFuture(
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
                cache.Put(
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
            var result = await this._client.VerifyReferenceOfAsync(
                request
            );
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
                cache.Put(
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
                    result?.Item[i]
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
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> DeleteAsync(
        #endif
            DeleteReferenceOfRequest request
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
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteReferenceOfFuture(
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
                cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(parentKey, key);
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
                cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
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
                cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
            }
            #else
            DeleteReferenceOfResult result = null;
            try {
                result = await this._client.DeleteReferenceOfAsync(
                    request
                );
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
                    cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(parentKey, key);
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
                    cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
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
                    cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                }
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
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
                    result?.Item[i]
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

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf,
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
                referenceOf ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string referenceOf
        )
        {
            return string.Join(
                ":",
                referenceOf ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<string> Model() {
            #else
        public IFuture<string> Model() {
            #endif
        #else
        public async Task<string> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<string> self)
            {
                self.OnComplete(this.ReferenceOf?.ToString());
                yield return null;
            }
            return new Gs2InlineFuture<string>(Impl);
        #else
            return this.ReferenceOf;
        #endif
        }

    }
}
