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

    public partial class InventoryModelMasterDomain {
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

        public InventoryModelMasterDomain(
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
                "InventoryModelMaster"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Inventory.Model.InventoryModelMaster> GetAsync(
            #else
        private IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Inventory.Model.InventoryModelMaster> GetAsync(
        #endif
            GetInventoryModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetInventoryModelMasterFuture(
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
            var result = await this._client.GetInventoryModelMasterAsync(
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
                        "InventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> UpdateAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> Update(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> UpdateAsync(
        #endif
            UpdateInventoryModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.UpdateInventoryModelMasterFuture(
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
            var result = await this._client.UpdateInventoryModelMasterAsync(
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
                        "InventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
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
            Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> DeleteAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> Delete(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> DeleteAsync(
        #endif
            DeleteInventoryModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteInventoryModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else {
                    self.OnError(future.Error);
                    yield break;
                }
            }
            var result = future.Result;
            #else
            DeleteInventoryModelMasterResult result = null;
            try {
                result = await this._client.DeleteInventoryModelMasterAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "inventoryModelMaster")
                {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                else
                {
                    throw e;
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
                        "InventoryModelMaster"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.InventoryModelMaster>(parentKey, key);
                }
            }
            Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain domain = this;

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMasterAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMaster(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> CreateItemModelMasterAsync(
        #endif
            CreateItemModelMasterRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateItemModelMasterFuture(
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
            var result = await this._client.CreateItemModelMasterAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.InventoryName,
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
            }
            var domain = new Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.InventoryName,
                result?.Item?.Name
            );

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
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemModelMaster> ItemModelMasters(
        )
        {
            return new DescribeItemModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.ItemModelMaster> ItemModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemModelMaster> ItemModelMasters(
            #endif
        #else
        public DescribeItemModelMastersIterator ItemModelMasters(
        #endif
        )
        {
            return new DescribeItemModelMastersIterator(
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

        public Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain ItemModelMaster(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ItemModelMasterDomain(
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

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() {
            #else
        public IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Model.InventoryModelMaster> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster> self)
            {
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                       _parentKey,
                       Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                    this.InventoryName?.ToString()
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetInventoryModelMasterRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "inventoryModelMaster")
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
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "inventoryModelMaster")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryModelMasterDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.InventoryModelMaster>(Impl);
        #endif
        }

    }
}
