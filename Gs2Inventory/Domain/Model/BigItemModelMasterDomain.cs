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

    public partial class BigItemModelMasterDomain {
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

        public BigItemModelMasterDomain(
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
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.InventoryName,
                "BigItemModelMaster"
            );
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

    }

    public partial class BigItemModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster> GetFuture(
            GetBigItemModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                var future = this._client.GetBigItemModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            request.ItemName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "bigItemModelMaster")
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
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                GetBigItemModelMasterResult result = null;
                try {
                    result = await this._client.GetBigItemModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        request.ItemName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "bigItemModelMaster")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "BigItemModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inventory.Model.BigItemModelMaster> GetAsync(
            GetBigItemModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var future = this._client.GetBigItemModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        request.ItemName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "bigItemModelMaster")
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
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            GetBigItemModelMasterResult result = null;
            try {
                result = await this._client.GetBigItemModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                    request.ItemName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "bigItemModelMaster")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "BigItemModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> UpdateFuture(
            UpdateBigItemModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                var future = this._client.UpdateBigItemModelMasterFuture(
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
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                UpdateBigItemModelMasterResult result = null;
                    result = await this._client.UpdateBigItemModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "BigItemModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> UpdateAsync(
            UpdateBigItemModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var future = this._client.UpdateBigItemModelMasterFuture(
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
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            UpdateBigItemModelMasterResult result = null;
                result = await this._client.UpdateBigItemModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "BigItemModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
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
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> UpdateAsync(
            UpdateBigItemModelMasterRequest request
        ) {
            var future = UpdateFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> Update(
            UpdateBigItemModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> DeleteFuture(
            DeleteBigItemModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                var future = this._client.DeleteBigItemModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            request.ItemName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "bigItemModelMaster")
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
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName);
                DeleteBigItemModelMasterResult result = null;
                try {
                    result = await this._client.DeleteBigItemModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        request.ItemName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "bigItemModelMaster")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.InventoryName,
                            "BigItemModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.BigItemModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> DeleteAsync(
            DeleteBigItemModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            var future = this._client.DeleteBigItemModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        request.ItemName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "bigItemModelMaster")
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
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName);
            DeleteBigItemModelMasterResult result = null;
            try {
                result = await this._client.DeleteBigItemModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                    request.ItemName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "bigItemModelMaster")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
                        "BigItemModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.BigItemModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> DeleteAsync(
            DeleteBigItemModelMasterRequest request
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> Delete(
            DeleteBigItemModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class BigItemModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        this.ItemName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBigItemModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                                    this.ItemName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "bigItemModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.BigItemModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                        this.ItemName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBigItemModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                                    this.ItemName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "bigItemModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                            this.ItemName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.BigItemModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.BigItemModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.BigItemModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.BigItemModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Inventory.Model.BigItemModelMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                    this.ItemName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Inventory.Model.BigItemModelMaster>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain.CreateCacheKey(
                    this.ItemName.ToString()
                ),
                callbackId
            );
        }

    }
}