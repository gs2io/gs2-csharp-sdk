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
        public string UserId => _accessToken.UserId;
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
                this.NamespaceName,
                this.UserId,
                this.InventoryName,
                this.ItemName,
                this.ItemSetName,
                "ReferenceOf"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<string[]> GetFuture(
            GetReferenceOfRequest request
        ) {

            IEnumerator Impl(IFuture<string[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var future = this._client.GetReferenceOfFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                            request.ReferenceOf.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "referenceOf")
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                GetReferenceOfResult result = null;
                try {
                    result = await this._client.GetReferenceOfAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        request.ReferenceOf.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "referenceOf")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
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
                            this.NamespaceName,
                            this.InventoryName,
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
                }
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<string[]>(Impl);
        }
        #else
        private async Task<string[]> GetAsync(
            GetReferenceOfRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var future = this._client.GetReferenceOfFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        request.ReferenceOf.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "referenceOf")
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
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            GetReferenceOfResult result = null;
            try {
                result = await this._client.GetReferenceOfAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                    request.ReferenceOf.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "referenceOf")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.ItemSet != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
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
                        this.NamespaceName,
                        this.InventoryName,
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
            }
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> VerifyFuture(
            VerifyReferenceOfRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
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
                #else
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                VerifyReferenceOfResult result = null;
                    result = await this._client.VerifyReferenceOfAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
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
                            this.NamespaceName,
                            this.InventoryName,
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
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
                        request.ReferenceOf
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> VerifyAsync(
            VerifyReferenceOfRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
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
            #else
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            VerifyReferenceOfResult result = null;
                result = await this._client.VerifyReferenceOfAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.ItemSet != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
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
                        this.NamespaceName,
                        this.InventoryName,
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
            }
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
                    request.ReferenceOf
                );
            }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> VerifyAsync(
            VerifyReferenceOfRequest request
        ) {
            var future = VerifyFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to VerifyFuture.")]
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> Verify(
            VerifyReferenceOfRequest request
        ) {
            return VerifyFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> DeleteFuture(
            DeleteReferenceOfRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                var future = this._client.DeleteReferenceOfFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                            request.ReferenceOf.ToString()
                        );
                        _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "referenceOf")
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithInventoryName(this.InventoryName)
                    .WithItemName(this.ItemName)
                    .WithItemSetName(this.ItemSetName)
                    .WithReferenceOf(this.ReferenceOf);
                DeleteReferenceOfResult result = null;
                try {
                    result = await this._client.DeleteReferenceOfAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        request.ReferenceOf.ToString()
                        );
                    _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "referenceOf")
                    {
                        throw;
                    }
                }
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.ItemSet != null) {
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.InventoryName,
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
                            this.NamespaceName,
                            this.InventoryName,
                            "ItemModel"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                            resultModel.ItemModel.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
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
                        cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                    }
                }
                var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
                        request.ReferenceOf
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> DeleteAsync(
            DeleteReferenceOfRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            var future = this._client.DeleteReferenceOfFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        request.ReferenceOf.ToString()
                    );
                    _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "referenceOf")
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
                .WithAccessToken(this._accessToken?.Token)
                .WithInventoryName(this.InventoryName)
                .WithItemName(this.ItemName)
                .WithItemSetName(this.ItemSetName)
                .WithReferenceOf(this.ReferenceOf);
            DeleteReferenceOfResult result = null;
            try {
                result = await this._client.DeleteReferenceOfAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                    request.ReferenceOf.ToString()
                    );
                _cache.Put<Gs2.Gs2Inventory.Model.ReferenceOf>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "referenceOf")
                {
                    throw;
                }
            }
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.ItemSet != null) {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.InventoryName,
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
                        this.NamespaceName,
                        this.InventoryName,
                        "ItemModel"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemModelDomain.CreateCacheKey(
                        resultModel.ItemModel.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Inventory.Model.ItemModel>(parentKey, key);
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
                    cache.Delete<Gs2.Gs2Inventory.Model.Inventory>(parentKey, key);
                }
            }
            var domain = new Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[result?.Item.Length ?? 0];
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
                    request.ReferenceOf
                );
            }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> DeleteAsync(
            DeleteReferenceOfRequest request
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
        public IFuture<Gs2.Gs2Inventory.Domain.Model.ReferenceOfAccessTokenDomain[]> Delete(
            DeleteReferenceOfRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

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
        public IFuture<string> ModelFuture()
        {
            IEnumerator Impl(IFuture<string> self)
            {
                var (value, find) = _cache.Get<string>(
                    _parentKey,
                    Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                        this.ReferenceOf?.ToString()
                    )
                );
                self.OnComplete(value);
                return null;
            }
            return new Gs2InlineFuture<string>(Impl);
        }
        #else
        public async Task<string> ModelAsync()
        {
            var (value, find) = _cache.Get<string>(
                _parentKey,
                Gs2.Gs2Inventory.Domain.Model.ReferenceOfDomain.CreateCacheKey(
                    this.ReferenceOf?.ToString()
                )
            );
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<string> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<string> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<string> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<string> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
