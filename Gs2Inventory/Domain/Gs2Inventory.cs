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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Inventory.Domain.Iterator;
using Gs2.Gs2Inventory.Domain.Model;
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Gs2Inventory.Model;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain
{

    public class Gs2Inventory {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2InventoryRestClient _client;

        private readonly String _parentKey;

        public Gs2Inventory(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2InventoryRestClient(
                session
            );
            this._parentKey = "inventory";
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
            #else
        public IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespace(
            #endif
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> CreateNamespaceAsync(
        #endif
            CreateNamespaceRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain> self)
            {
        #endif
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateNamespaceFuture(
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
            var result = await this._client.CreateNamespaceAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = string.Join(
                        ":",
                        "inventory",
                        "Namespace"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.NamespaceDomain.CreateCacheKey(
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
            var domain = new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
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
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.NamespaceDomain>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Namespace> Namespaces(
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Inventory.Model.Namespace> NamespacesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Inventory.Model.Namespace> Namespaces(
            #endif
        #else
        public DescribeNamespacesIterator Namespaces(
        #endif
        )
        {
            return new DescribeNamespacesIterator(
                this._cache,
                this._client
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

        public Gs2.Gs2Inventory.Domain.Model.NamespaceDomain Namespace(
            string namespaceName
        ) {
            return new Gs2.Gs2Inventory.Domain.Model.NamespaceDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                namespaceName
            );
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult> AddCapacityByUserIdComplete = new UnityEvent<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult>();
    #else
        public static Action<string, AddCapacityByUserIdRequest, AddCapacityByUserIdResult> AddCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult> SetCapacityByUserIdComplete = new UnityEvent<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult>();
    #else
        public static Action<string, SetCapacityByUserIdRequest, SetCapacityByUserIdResult> SetCapacityByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult> AcquireItemSetByUserIdComplete = new UnityEvent<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult>();
    #else
        public static Action<string, AcquireItemSetByUserIdRequest, AcquireItemSetByUserIdResult> AcquireItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult> AddReferenceOfByUserIdComplete = new UnityEvent<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult>();
    #else
        public static Action<string, AddReferenceOfByUserIdRequest, AddReferenceOfByUserIdResult> AddReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdComplete = new UnityEvent<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult>();
    #else
        public static Action<string, DeleteReferenceOfByUserIdRequest, DeleteReferenceOfByUserIdResult> DeleteReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdComplete = new UnityEvent<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult>();
    #else
        public static Action<string, AcquireSimpleItemsByUserIdRequest, AcquireSimpleItemsByUserIdResult> AcquireSimpleItemsByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampSheet(
                CacheDatabase cache,
                string transactionId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "AddCapacityByUserId": {
                        var requestModel = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
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

                        AddCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "SetCapacityByUserId": {
                        var requestModel = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = SetCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.Item != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
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

                        SetCapacityByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireItemSetByUserId": {
                        var requestModel = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    item.ItemName.ToString(),
                                    item.Name.ToString()
                                );
                                if (item.Count == 0) {
                                    cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key
                                    );
                                }
                                else
                                {
                                    cache.Put(
                                        parentKey,
                                        key,
                                        item,
                                        item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                            }
                            var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                            {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    group.Key,
                                    null
                                );
                                var items = group.ToArray();
                                cache.Put(
                                    parentKey,
                                    key,
                                    items,
                                    items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                                return items;
                            }).ToArray();
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
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
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Inventory.Revision)
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Inventory,
                                    resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                            }
                        }

                        AcquireItemSetByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AddReferenceOfByUserId": {
                        var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
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
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
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

                        AddReferenceOfByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "DeleteReferenceOfByUserId": {
                        var requestModel = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = DeleteReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                resultModel.ItemSet.ItemName.ToString(),
                                resultModel.ItemSet.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(parentKey, key);
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
                                "ItemModel"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                                resultModel.ItemModel.Name.ToString()
                            );
                            cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                        }
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                        }

                        DeleteReferenceOfByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "AcquireSimpleItemsByUserId": {
                        var requestModel = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = AcquireSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "SimpleItem"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                    item.ItemName.ToString()
                                );
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        AcquireSimpleItemsByUserIdComplete?.Invoke(
                            transactionId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete = new UnityEvent<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult>();
    #else
        public static Action<string, ConsumeItemSetByUserIdRequest, ConsumeItemSetByUserIdResult> ConsumeItemSetByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdComplete = new UnityEvent<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult>();
    #else
        public static Action<string, VerifyReferenceOfByUserIdRequest, VerifyReferenceOfByUserIdResult> VerifyReferenceOfByUserIdComplete;
    #endif

    #if UNITY_2017_1_OR_NEWER
        public static UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete = new UnityEvent<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult>();
    #else
        public static Action<string, ConsumeSimpleItemsByUserIdRequest, ConsumeSimpleItemsByUserIdResult> ConsumeSimpleItemsByUserIdComplete;
    #endif

        public static void UpdateCacheFromStampTask(
                CacheDatabase cache,
                string taskId,
                string method,
                string request,
                string result
        ) {
                switch (method) {
                    case "ConsumeItemSetByUserId": {
                        var requestModel = ConsumeItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "ItemSet"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    item.ItemName.ToString(),
                                    item.Name.ToString()
                                );
                                if (item.Count == 0) {
                                    cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                        parentKey,
                                        key
                                    );
                                }
                                else
                                {
                                    cache.Put(
                                        parentKey,
                                        key,
                                        item,
                                        item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                    );
                                }
                            }
                            var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                            {
                                var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                    group.Key,
                                    null
                                );
                                var items = group.ToArray();
                                cache.Put(
                                    parentKey,
                                    key,
                                    items,
                                    items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                                return items;
                            }).ToArray();
                        }
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
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
                        if (resultModel.Inventory != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                "Inventory"
                            );
                            var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                                resultModel.Inventory.InventoryName.ToString()
                            );
                            var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                                parentKey,
                                key
                            );
                            if (item == null || item.Revision < resultModel.Inventory.Revision)
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    resultModel.Inventory,
                                    resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                                );
                            }
                        }

                        ConsumeItemSetByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "VerifyReferenceOfByUserId": {
                        var requestModel = VerifyReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = VerifyReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        
                        if (resultModel.ItemSet != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
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
                        if (resultModel.ItemModel != null) {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.InventoryName,
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

                        VerifyReferenceOfByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                    case "ConsumeSimpleItemsByUserId": {
                        var requestModel = ConsumeSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(request));
                        var resultModel = ConsumeSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result));
                        {
                            var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                                requestModel.NamespaceName,
                                requestModel.UserId,
                                requestModel.InventoryName,
                                "SimpleItem"
                            );
                            foreach (var item in resultModel.Items) {
                                var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                    item.ItemName.ToString()
                                );
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }

                        ConsumeSimpleItemsByUserIdComplete?.Invoke(
                            taskId,
                            requestModel,
                            resultModel
                        );
                        break;
                    }
                }
        }

        public static void UpdateCacheFromJobResult(
                CacheDatabase cache,
                string method,
                Gs2.Gs2JobQueue.Model.Job job,
                Gs2.Gs2JobQueue.Model.JobResultBody result
        ) {
            switch (method) {
                case "add_capacity_by_user_id": {
                    var requestModel = AddCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
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

                    AddCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "set_capacity_by_user_id": {
                    var requestModel = SetCapacityByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = SetCapacityByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
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

                    SetCapacityByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_item_set_by_user_id": {
                    var requestModel = AcquireItemSetByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireItemSetByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                item.ItemName.ToString(),
                                item.Name.ToString()
                            );
                            if (item.Count == 0) {
                                cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(
                                    parentKey,
                                    key
                                );
                            }
                            else
                            {
                                cache.Put(
                                    parentKey,
                                    key,
                                    item,
                                    item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                                );
                            }
                        }
                        var _ = resultModel.Items.GroupBy(v => v.ItemName).Select(group =>
                        {
                            var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                                group.Key,
                                null
                            );
                            var items = group.ToArray();
                            cache.Put(
                                parentKey,
                                key,
                                items,
                                items == null || items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                            return items;
                        }).ToArray();
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
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
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        var (item, find) = cache.Get<Gs2.Gs2Inventory.Model.Inventory>(
                            parentKey,
                            key
                        );
                        if (item == null || item.Revision < resultModel.Inventory.Revision)
                        {
                            cache.Put(
                                parentKey,
                                key,
                                resultModel.Inventory,
                                resultModel.Items == null || resultModel.Items.Length == 0 ? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes : resultModel.Items.Min(v => v.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes)
                            );
                        }
                    }

                    AcquireItemSetByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "add_reference_of_by_user_id": {
                    var requestModel = AddReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AddReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
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
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
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

                    AddReferenceOfByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "delete_reference_of_by_user_id": {
                    var requestModel = DeleteReferenceOfByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = DeleteReferenceOfByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            resultModel.ItemSet.ItemName.ToString(),
                            resultModel.ItemSet.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemSet>(parentKey, key);
                    }
                    if (resultModel.ItemModel != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryModelDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
                    }
                    if (resultModel.Inventory != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.UserDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            "Inventory"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheKey(
                            resultModel.Inventory.InventoryName.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }

                    DeleteReferenceOfByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
                case "acquire_simple_items_by_user_id": {
                    var requestModel = AcquireSimpleItemsByUserIdRequest.FromJson(JsonMapper.ToObject(job.Args));
                    var resultModel = AcquireSimpleItemsByUserIdResult.FromJson(JsonMapper.ToObject(result.Result));
                    {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                            requestModel.NamespaceName,
                            requestModel.UserId,
                            requestModel.InventoryName,
                            "SimpleItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                                item.ItemName.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }

                    AcquireSimpleItemsByUserIdComplete?.Invoke(
                        job.JobId,
                        requestModel,
                        resultModel
                    );
                    break;
                }
            }
        }

        public void HandleNotification(
                CacheDatabase cache,
                string action,
                string payload
        ) {
    #if UNITY_2017_1_OR_NEWER
    #endif
        }
    }
}
