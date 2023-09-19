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

    public partial class InventoryModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _inventoryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string InventoryName => _inventoryName;

        public InventoryModelDomain(
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
                "InventoryModel"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemModel> ItemModels(
        )
        {
            return new DescribeItemModelsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.InventoryName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.ItemModel> ItemModelsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.ItemModel> ItemModels(
            #endif
        #else
        public DescribeItemModelsIterator ItemModels(
        #endif
        )
        {
            return new DescribeItemModelsIterator(
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

        public Gs2.Gs2Inventory.Domain.Model.ItemModelDomain ItemModel(
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.ItemModelDomain(
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

    public partial class InventoryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Inventory.Model.InventoryModel> GetFuture(
            GetInventoryModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.InventoryModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInventoryName(this.InventoryName);
                var future = this._client.GetInventoryModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                            request.InventoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "inventoryModel")
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
                GetInventoryModelResult result = null;
                try {
                    result = await this._client.GetInventoryModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "inventoryModel")
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
                            "InventoryModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.InventoryModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Inventory.Model.InventoryModel> GetAsync(
            GetInventoryModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInventoryName(this.InventoryName);
            var future = this._client.GetInventoryModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                        request.InventoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "inventoryModel")
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
            GetInventoryModelResult result = null;
            try {
                result = await this._client.GetInventoryModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                    request.InventoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "inventoryModel")
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
                        "InventoryModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
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

    }

    public partial class InventoryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Model.InventoryModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Model.InventoryModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModel>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetInventoryModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "inventoryModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModel>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Model.InventoryModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Model.InventoryModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModel>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                        this.InventoryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetInventoryModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                                    this.InventoryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Inventory.Model.InventoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "inventoryModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Inventory.Model.InventoryModel>(
                        _parentKey,
                        Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheKey(
                            this.InventoryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Model.InventoryModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Inventory.Model.InventoryModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Inventory.Model.InventoryModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Inventory.Model.InventoryModel> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
