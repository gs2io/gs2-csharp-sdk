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

    public partial class ReferenceOfDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;
        private readonly string _referenceOf;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string InventoryName => _inventoryName;
        public string ItemName => _itemName;
        public string ItemSetName => _itemSetName;
        public string ReferenceOf => _referenceOf;

        public ReferenceOfDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._inventoryName = inventoryName;
            this._itemName = itemName;
            this._itemSetName = itemSetName;
            this._referenceOf = referenceOf;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                this._userId != null ? this._userId.ToString() : null,
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
            GetReferenceOfByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<string[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetReferenceOfByUserIdFuture(
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
            var result = await this._client.GetReferenceOfByUserIdAsync(
                request
            );
            #endif
            string parentKey = "inventory:string";
            foreach (var item in result?.Item) {
                    
                if (item != null) {
                    _cache.Put(
                        parentKey,
                        item,
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                    
            if (result.ItemSet != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName != null ? request.ItemName.ToString() : null,
                        result.ItemSet?.Name?.ToString()
                    ),
                    result.ItemSet,
                    result.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.ItemModel != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        request.ItemName != null ? request.ItemName.ToString() : null
                    ),
                    result.ItemModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.Inventory != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName != null ? request.InventoryName.ToString() : null
                    ),
                    result.Inventory,
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
            return new Gs2InlineFuture<string[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> VerifyAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> Verify(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> VerifyAsync(
        #endif
            VerifyReferenceOfByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.VerifyReferenceOfByUserIdFuture(
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
            var result = await this._client.VerifyReferenceOfByUserIdAsync(
                request
            );
            #endif
            string parentKey = "inventory:string";
            foreach (var item in result?.Item) {
                    
                if (item != null) {
                    _cache.Put(
                        parentKey,
                        item,
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                    
            if (result.ItemSet != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName != null ? request.ItemName.ToString() : null,
                        result.ItemSet?.Name?.ToString()
                    ),
                    result.ItemSet,
                    result.ItemSet.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.ItemModel != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        request.ItemName != null ? request.ItemName.ToString() : null
                    ),
                    result.ItemModel,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
                    
            if (result.Inventory != null) {
                _cache.Put(
                    parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName != null ? request.InventoryName.ToString() : null
                    ),
                    result.Inventory,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[] domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[result?.Item.Length ?? 0];
            for (int i=0; i<result?.Item.Length; i++)
            {
                domain[i] = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> DeleteAsync(
        #endif
            DeleteReferenceOfByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithInventoryName(this._inventoryName)
                .WithItemName(this._itemName)
                .WithItemSetName(this._itemSetName)
                .WithReferenceOf(this._referenceOf);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteReferenceOfByUserIdFuture(
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
            DeleteReferenceOfByUserIdResult result = null;
            try {
                result = await this._client.DeleteReferenceOfByUserIdAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException) {}
            #endif
            string parentKey = "inventory:string";
            Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[] domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[result?.Item.Length ?? 0];
            for (int i=0; i<result?.Item.Length; i++)
            {
                domain[i] = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain[]>(Impl);
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
                var value = _cache.Get<string>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        this.ReferenceOf?.ToString()
                    )
                );
                self.OnComplete(value);
                yield return null;
            }
            return new Gs2InlineFuture<string>(Impl);
        #else
            return _cache.Get<string>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                    this.ReferenceOf?.ToString()
                )
            );
        #endif
        }

    }
}