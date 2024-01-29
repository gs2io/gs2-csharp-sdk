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
using Cysharp.Threading.Tasks.Linq;
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
        private readonly Gs2.Core.Domain.Gs2 _gs2;
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
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2InventoryRestClient(
                gs2.RestSession
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
                this._gs2.Cache,
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
        public DescribeItemSetsByUserIdIterator ItemSetsAsync(
        #endif
        )
        {
            return new DescribeItemSetsByUserIdIterator(
                this._gs2.Cache,
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
            Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.InventoryName,
                    "ItemSet"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeItemSetsWithInitialCallAsync(
            Action<Gs2.Gs2Inventory.Model.ItemSet[]> callback
        )
        {
            var items = await ItemSetsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeItemSets(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeItemSets(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Inventory.Model.ItemSet>(
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
                this._gs2,
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
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "inventory")
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
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Inventory.Model.Inventory> GetAsync(
            #else
        private async Task<Gs2.Gs2Inventory.Model.Inventory> GetAsync(
            #endif
            GetInventoryByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "inventory")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> AddCapacityAsync(
            #endif
            AddCapacityByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            AddCapacityByUserIdResult result = null;
                result = await this._client.AddCapacityByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        _gs2.Cache.Put(
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

                var requestModel = request;
                var resultModel = result;
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
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Item.Revision)
                        {
                            _gs2.Cache.Put(
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
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> SetCapacityAsync(
            #endif
            SetCapacityByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            SetCapacityByUserIdResult result = null;
                result = await this._client.SetCapacityByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
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
                    var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Item.Revision)
                    {
                        _gs2.Cache.Put(
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
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "inventory")
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
                        _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> DeleteAsync(
            #endif
            DeleteInventoryByUserIdRequest request
        ) {
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
                this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "inventory")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
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
                    _gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
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

                var requestModel = request;
                var resultModel = result;
                if (resultModel != null) {
                    
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacityAsync(
            #endif
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            VerifyInventoryCurrentMaxCapacityByUserIdResult result = null;
                result = await this._client.VerifyInventoryCurrentMaxCapacityByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to VerifyCurrentMaxCapacityFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryDomain> VerifyCurrentMaxCapacity(
            VerifyInventoryCurrentMaxCapacityByUserIdRequest request
        ) {
            return VerifyCurrentMaxCapacityFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeFuture(
            AcquireItemSetWithGradeByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.AcquireItemSetWithGradeByUserIdFuture(
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
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            resultModel.Item.ItemName.ToString(),
                            resultModel.Item.Name.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                    if (resultModel.Status != null) {
                        var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "Status"
                        );
                        var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                            resultModel.Status.GradeName.ToString(),
                            resultModel.Status.PropertyId.ToString()
                        );
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Status,
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
                        _gs2.Cache.Put(
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
                        var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            _gs2.Cache.Put(
                                parentKey,
                                key,
                                resultModel.Item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.InventoryName,
                    result?.Item?.ItemName,
                    result?.Item?.Name
                );
                domain.OverflowCount = result?.OverflowCount;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeAsync(
            #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGradeAsync(
            #endif
            AcquireItemSetWithGradeByUserIdRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithInventoryName(this.InventoryName);
            AcquireItemSetWithGradeByUserIdResult result = null;
                result = await this._client.AcquireItemSetWithGradeByUserIdAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
                        "ItemSet"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        resultModel.Item.ItemName.ToString(),
                        resultModel.Item.Name.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                if (resultModel.Status != null) {
                    var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "Status"
                    );
                    var key = Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                        resultModel.Status.GradeName.ToString(),
                        resultModel.Status.PropertyId.ToString()
                    );
                    _gs2.Cache.Put(
                        parentKey,
                        key,
                        resultModel.Status,
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
                    _gs2.Cache.Put(
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
                    var (item, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        parentKey,
                        key
                    );
                    if (item == null || item.Revision < resultModel.Inventory.Revision)
                    {
                        _gs2.Cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Inventory.Domain.Model.ItemSetDomain(
                    this._gs2,
                    request.NamespaceName,
                    result?.Item?.UserId,
                    result?.Item?.InventoryName,
                    result?.Item?.ItemName,
                    result?.Item?.Name
                );
            domain.OverflowCount = result?.OverflowCount;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to AcquireItemSetWithGradeFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemSetDomain> AcquireItemSetWithGrade(
            AcquireItemSetWithGradeByUserIdRequest request
        ) {
            return AcquireItemSetWithGradeFuture(request);
        }
        #endif

    }

    public partial class InventoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.Inventory> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.Inventory> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
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
                            this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "inventory")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
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
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
            #else
        public async Task<Gs2.Gs2Inventory.Model.Inventory> ModelAsync()
            #endif
        {
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Inventory.Model.Inventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName?.ToString()
                )).LockAsync())
            {
        # endif
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
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
                        this._gs2.Cache.Put<Gs2.Gs2Inventory.Model.Inventory>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (e.errors.Length == 0 || e.errors[0].component != "inventory")
                        {
                            throw;
                        }
                    }
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
                }
                return value;
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        # endif
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
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


        public void Invalidate()
        {
            this._gs2.Cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                )
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
                    ModelAsync().Forget();
            #else
                    ModelAsync();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Inventory.Model.Inventory>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                    this.InventoryName.ToString()
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Inventory.Model.Inventory> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
