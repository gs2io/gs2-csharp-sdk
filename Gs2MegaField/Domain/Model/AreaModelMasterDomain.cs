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
using Gs2.Gs2MegaField.Domain.Iterator;
using Gs2.Gs2MegaField.Request;
using Gs2.Gs2MegaField.Result;
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

namespace Gs2.Gs2MegaField.Domain.Model
{

    public partial class AreaModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MegaFieldRestClient _client;
        private readonly string _namespaceName;
        private readonly string _areaModelName;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string AreaModelName => _areaModelName;

        public AreaModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string areaModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2MegaFieldRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._areaModelName = areaModelName;
            this._parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "AreaModelMaster"
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2MegaField.Model.LayerModelMaster> LayerModelMasters(
        )
        {
            return new DescribeLayerModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AreaModelName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2MegaField.Model.LayerModelMaster> LayerModelMastersAsync(
            #else
        public Gs2Iterator<Gs2.Gs2MegaField.Model.LayerModelMaster> LayerModelMasters(
            #endif
        #else
        public DescribeLayerModelMastersIterator LayerModelMasters(
        #endif
        )
        {
            return new DescribeLayerModelMastersIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                this.AreaModelName
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

        public ulong SubscribeLayerModelMasters(Action callback)
        {
            return this._cache.ListSubscribe<Gs2.Gs2MegaField.Model.LayerModelMaster>(
                Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName,
                    "LayerModelMaster"
                ),
                callback
            );
        }

        public void UnsubscribeLayerModelMasters(ulong callbackId)
        {
            this._cache.ListUnsubscribe<Gs2.Gs2MegaField.Model.LayerModelMaster>(
                Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.AreaModelName,
                    "LayerModelMaster"
                ),
                callbackId
            );
        }

        public Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain LayerModelMaster(
            string layerModelName
        ) {
            return new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.AreaModelName,
                layerModelName
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string areaModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "megaField",
                namespaceName ?? "null",
                areaModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string areaModelName
        )
        {
            return string.Join(
                ":",
                areaModelName ?? "null"
            );
        }

    }

    public partial class AreaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> GetFuture(
            GetAreaModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = this._client.GetAreaModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                            request.AreaModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "areaModelMaster")
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
                    .WithAreaModelName(this.AreaModelName);
                GetAreaModelMasterResult result = null;
                try {
                    result = await this._client.GetAreaModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        request.AreaModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "areaModelMaster")
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
                        var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "AreaModelMaster"
                        );
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> GetAsync(
            GetAreaModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var future = this._client.GetAreaModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        request.AreaModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "areaModelMaster")
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
                .WithAreaModelName(this.AreaModelName);
            GetAreaModelMasterResult result = null;
            try {
                result = await this._client.GetAreaModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                    request.AreaModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "areaModelMaster")
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
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "AreaModelMaster"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateFuture(
            UpdateAreaModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = this._client.UpdateAreaModelMasterFuture(
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
                    .WithAreaModelName(this.AreaModelName);
                UpdateAreaModelMasterResult result = null;
                    result = await this._client.UpdateAreaModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "AreaModelMaster"
                        );
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateAsync(
            UpdateAreaModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var future = this._client.UpdateAreaModelMasterFuture(
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
                .WithAreaModelName(this.AreaModelName);
            UpdateAreaModelMasterResult result = null;
                result = await this._client.UpdateAreaModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "AreaModelMaster"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> UpdateAsync(
            UpdateAreaModelMasterRequest request
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
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> Update(
            UpdateAreaModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteFuture(
            DeleteAreaModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = this._client.DeleteAreaModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                            request.AreaModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "areaModelMaster")
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
                    .WithAreaModelName(this.AreaModelName);
                DeleteAreaModelMasterResult result = null;
                try {
                    result = await this._client.DeleteAreaModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        request.AreaModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "areaModelMaster")
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
                        var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "AreaModelMaster"
                        );
                        var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2MegaField.Model.AreaModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteAsync(
            DeleteAreaModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var future = this._client.DeleteAreaModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        request.AreaModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "areaModelMaster")
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
                .WithAreaModelName(this.AreaModelName);
            DeleteAreaModelMasterResult result = null;
            try {
                result = await this._client.DeleteAreaModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                    request.AreaModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "areaModelMaster")
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
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "AreaModelMaster"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2MegaField.Model.AreaModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> DeleteAsync(
            DeleteAreaModelMasterRequest request
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
        public IFuture<Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain> Delete(
            DeleteAreaModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterFuture(
            CreateLayerModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAreaModelName(this.AreaModelName);
                var future = this._client.CreateLayerModelMasterFuture(
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
                    .WithAreaModelName(this.AreaModelName);
                CreateLayerModelMasterResult result = null;
                    result = await this._client.CreateLayerModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.AreaModelName,
                            "LayerModelMaster"
                        );
                        var key = Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.AreaModelName,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterAsync(
            CreateLayerModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAreaModelName(this.AreaModelName);
            var future = this._client.CreateLayerModelMasterFuture(
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
                .WithAreaModelName(this.AreaModelName);
            CreateLayerModelMasterResult result = null;
                result = await this._client.CreateLayerModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.AreaModelName,
                        "LayerModelMaster"
                    );
                    var key = Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.AreaModelName,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMasterAsync(
            CreateLayerModelMasterRequest request
        ) {
            var future = CreateLayerModelMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to CreateLayerModelMasterFuture.")]
        public IFuture<Gs2.Gs2MegaField.Domain.Model.LayerModelMasterDomain> CreateLayerModelMaster(
            CreateLayerModelMasterRequest request
        ) {
            return CreateLayerModelMasterFuture(request);
        }
        #endif

    }

    public partial class AreaModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        this.AreaModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetAreaModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                                    this.AreaModelName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "areaModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                            this.AreaModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2MegaField.Model.AreaModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                    _parentKey,
                    Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                        this.AreaModelName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetAreaModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                                    this.AreaModelName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "areaModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                        _parentKey,
                        Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                            this.AreaModelName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2MegaField.Model.AreaModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2MegaField.Model.AreaModelMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                    this.AreaModelName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2MegaField.Model.AreaModelMaster>(
                _parentKey,
                Gs2.Gs2MegaField.Domain.Model.AreaModelMasterDomain.CreateCacheKey(
                    this.AreaModelName.ToString()
                ),
                callbackId
            );
        }

    }
}
