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

    public partial class InventoryDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;

        private readonly String _parentKey;
        public long? OverflowCount { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string InventoryName => _inventoryName;

        public InventoryDomain(
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
                "Inventory"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemSet> ItemSets(
        )
        {
            return new DescribeItemSetsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.ItemSet> ItemSetsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemSet> ItemSets(
            #endif
        #else
        public DescribeItemSetsByUserIdIterator ItemSets(
        #endif
        )
        {
            return new DescribeItemSetsByUserIdIterator(
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

        public ulong SubscribeItemSets(
            Action callback,
            string itemSetName
        )
        {
            return this._cache.ListSubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "ItemSet"
                ),
                callback
            );
        }

        public void UnsubscribeItemSets(
            ulong callbackId,
            string itemSetName
        )
        {
            this._cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "ItemSet"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Inventory.Domain.Model.ItemSetDomain ItemSet(
            string itemName,
            string itemSetName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                itemName,
                itemSetName
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

    public partial class InventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.Inventory> GetFuture(
            GetInventoryByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.Inventory> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.GetInventoryByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            request.InventoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "inventory")
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
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                GetInventoryByUserIdResult result = null;
                try {
                    result = await this._client.GetInventoryByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "inventory")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.Inventory>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inventory.Model.Inventory> GetAsync(
            GetInventoryByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.GetInventoryByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "inventory")
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
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            GetInventoryByUserIdResult result = null;
            try {
                result = await this._client.GetInventoryByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    request.InventoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "inventory")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Item.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityFuture(
            AddCapacityByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.AddCapacityByUserIdFuture(
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
                AddCapacityByUserIdResult result = null;
                    result = await this._client.AddCapacityByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
            AddCapacityByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.AddCapacityByUserIdFuture(
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
            AddCapacityByUserIdResult result = null;
                result = await this._client.AddCapacityByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Item.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
            AddCapacityByUserIdRequest request
        ) {
            var future = AddCapacityFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to AddCapacityFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacity(
            AddCapacityByUserIdRequest request
        ) {
            return AddCapacityFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityFuture(
            SetCapacityByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.SetCapacityByUserIdFuture(
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
                SetCapacityByUserIdResult result = null;
                    result = await this._client.SetCapacityByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
            SetCapacityByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.SetCapacityByUserIdFuture(
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
            SetCapacityByUserIdResult result = null;
                result = await this._client.SetCapacityByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Item.InventoryName.ToString()
                    );
                    var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
            SetCapacityByUserIdRequest request
        ) {
            var future = SetCapacityFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to SetCapacityFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacity(
            SetCapacityByUserIdRequest request
        ) {
            return SetCapacityFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteFuture(
            DeleteInventoryByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.DeleteInventoryByUserIdFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            request.InventoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "inventory")
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
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                DeleteInventoryByUserIdResult result = null;
                try {
                    result = await this._client.DeleteInventoryByUserIdAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "inventory")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Item.InventoryName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
            DeleteInventoryByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.DeleteInventoryByUserIdFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "inventory")
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
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            DeleteInventoryByUserIdResult result = null;
            try {
                result = await this._client.DeleteInventoryByUserIdAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    request.InventoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "inventory")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Inventory"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        resultModel.Item.InventoryName.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
            DeleteInventoryByUserIdRequest request
        ) {
            var future = DeleteFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> Delete(
            DeleteInventoryByUserIdRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityFuture(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.VerifyInventoryCurrentMaxCapacityByUserIdFuture(
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
                VerifyInventoryCurrentMaxCapacityByUserIdResult result = null;
                    result = await this._client.VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            var future = this._client.VerifyInventoryCurrentMaxCapacityByUserIdFuture(
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
            VerifyInventoryCurrentMaxCapacityByUserIdResult result = null;
                result = await this._client.VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                    request
                );
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
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            var future = VerifyCurrentMaxCapacityFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to VerifyCurrentMaxCapacityFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacity(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            return VerifyCurrentMaxCapacityFuture(request);
        }
        #endif

    }

    public partial class InventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.Inventory> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.Inventory> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetInventoryByUserIdRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "inventory")
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
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.Inventory>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetInventoryByUserIdRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "inventory")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.Inventory> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.Inventory> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.Inventory> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Inventory.Model.Inventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callbackId
            );
        }

    }
}
