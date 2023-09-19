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

    public partial class BigInventoryModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _inventoryName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string InventoryName => _inventoryName;

        public BigInventoryModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
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
            this._inventoryName = inventoryName;
            this._parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "BigInventoryModelMaster"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigItemModelMaster> BigItemModelMasters(
        )
        {
            return new DescribeBigItemModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.BigItemModelMaster> BigItemModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.BigItemModelMaster> BigItemModelMasters(
            #endif
        #else
        public DescribeBigItemModelMastersIterator BigItemModelMasters(
        #endif
        )
        {
            return new DescribeBigItemModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName
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

        public Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain BigItemModelMaster(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.InventoryName,
                itemName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string inventoryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "inventory",
                namespaceName ?? "null",
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

    public partial class BigInventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> GetFuture(
            GetBigInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.GetBigInventoryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            request.InventoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "bigInventoryModelMaster")
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
                    .WithInventoryName(this.InventoryName);
                GetBigInventoryModelMasterResult result = null;
                try {
                    result = await this._client.GetBigInventoryModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "bigInventoryModelMaster")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BigInventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> GetAsync(
            GetBigInventoryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var future = this._client.GetBigInventoryModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "bigInventoryModelMaster")
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
                .WithInventoryName(this.InventoryName);
            GetBigInventoryModelMasterResult result = null;
            try {
                result = await this._client.GetBigInventoryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                    request.InventoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "bigInventoryModelMaster")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BigInventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateFuture(
            UpdateBigInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.UpdateBigInventoryModelMasterFuture(
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
                    .WithInventoryName(this.InventoryName);
                UpdateBigInventoryModelMasterResult result = null;
                    result = await this._client.UpdateBigInventoryModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BigInventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateAsync(
            UpdateBigInventoryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var future = this._client.UpdateBigInventoryModelMasterFuture(
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
                .WithInventoryName(this.InventoryName);
            UpdateBigInventoryModelMasterResult result = null;
                result = await this._client.UpdateBigInventoryModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BigInventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> UpdateAsync(
            UpdateBigInventoryModelMasterRequest request
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> Update(
            UpdateBigInventoryModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteFuture(
            DeleteBigInventoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.DeleteBigInventoryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            request.InventoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "bigInventoryModelMaster")
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
                    .WithInventoryName(this.InventoryName);
                DeleteBigInventoryModelMasterResult result = null;
                try {
                    result = await this._client.DeleteBigInventoryModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "bigInventoryModelMaster")
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
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "BigInventoryModelMaster"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteAsync(
            DeleteBigInventoryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var future = this._client.DeleteBigInventoryModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "bigInventoryModelMaster")
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
                .WithInventoryName(this.InventoryName);
            DeleteBigInventoryModelMasterResult result = null;
            try {
                result = await this._client.DeleteBigInventoryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                    request.InventoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "bigInventoryModelMaster")
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
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "BigInventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> DeleteAsync(
            DeleteBigInventoryModelMasterRequest request
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain> Delete(
            DeleteBigInventoryModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterFuture(
            CreateBigItemModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.CreateBigItemModelMasterFuture(
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
                    .WithInventoryName(this.InventoryName);
                CreateBigItemModelMasterResult result = null;
                    result = await this._client.CreateBigItemModelMasterAsync(
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.InventoryName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterAsync(
            CreateBigItemModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var future = this._client.CreateBigItemModelMasterFuture(
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
                .WithInventoryName(this.InventoryName);
            CreateBigItemModelMasterResult result = null;
                result = await this._client.CreateBigItemModelMasterAsync(
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
                var domain = new Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.InventoryName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMasterAsync(
            CreateBigItemModelMasterRequest request
        ) {
            var future = CreateBigItemModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateBigItemModelMasterFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.BigItemModelMasterDomain> CreateBigItemModelMaster(
            CreateBigItemModelMasterRequest request
        ) {
            return CreateBigItemModelMasterFuture(request);
        }
        #endif

    }

    public partial class BigInventoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBigInventoryModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "bigInventoryModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBigInventoryModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "bigInventoryModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.BigInventoryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.BigInventoryModelMasterDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.BigInventoryModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
