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
using Gs2.Gs2Formation.Domain.Iterator;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Domain.Model
{

    public partial class PropertyFormModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private readonly string _propertyFormModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string PropertyFormModelName => _propertyFormModelName;

        public PropertyFormModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string propertyFormModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._propertyFormModelName = propertyFormModelName;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "PropertyFormModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string propertyFormModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                propertyFormModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string propertyFormModelName
        )
        {
            return string.Join(
                ":",
                propertyFormModelName ?? "null"
            );
        }

    }

    public partial class PropertyFormModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster> GetFuture(
            GetPropertyFormModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                var future = this._client.GetPropertyFormModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            request.PropertyFormModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "propertyFormModelMaster")
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
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                GetPropertyFormModelMasterResult result = null;
                try {
                    result = await this._client.GetPropertyFormModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "propertyFormModelMaster")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Formation.Model.PropertyFormModelMaster> GetAsync(
            GetPropertyFormModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithPropertyFormModelName(this.PropertyFormModelName);
            var future = this._client.GetPropertyFormModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "propertyFormModelMaster")
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
                .WithPropertyFormModelName(this.PropertyFormModelName);
            GetPropertyFormModelMasterResult result = null;
            try {
                result = await this._client.GetPropertyFormModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                    request.PropertyFormModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "propertyFormModelMaster")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> UpdateFuture(
            UpdatePropertyFormModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                var future = this._client.UpdatePropertyFormModelMasterFuture(
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
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                UpdatePropertyFormModelMasterResult result = null;
                    result = await this._client.UpdatePropertyFormModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> UpdateAsync(
            UpdatePropertyFormModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithPropertyFormModelName(this.PropertyFormModelName);
            var future = this._client.UpdatePropertyFormModelMasterFuture(
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
                .WithPropertyFormModelName(this.PropertyFormModelName);
            UpdatePropertyFormModelMasterResult result = null;
                result = await this._client.UpdatePropertyFormModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> UpdateAsync(
            UpdatePropertyFormModelMasterRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> Update(
            UpdatePropertyFormModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> DeleteFuture(
            DeletePropertyFormModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                var future = this._client.DeletePropertyFormModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            request.PropertyFormModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "propertyFormModelMaster")
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
                    .WithPropertyFormModelName(this.PropertyFormModelName);
                DeletePropertyFormModelMasterResult result = null;
                try {
                    result = await this._client.DeletePropertyFormModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "propertyFormModelMaster")
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
                        var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "PropertyFormModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> DeleteAsync(
            DeletePropertyFormModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithPropertyFormModelName(this.PropertyFormModelName);
            var future = this._client.DeletePropertyFormModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        request.PropertyFormModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "propertyFormModelMaster")
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
                .WithPropertyFormModelName(this.PropertyFormModelName);
            DeletePropertyFormModelMasterResult result = null;
            try {
                result = await this._client.DeletePropertyFormModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                    request.PropertyFormModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "propertyFormModelMaster")
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
                    var parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "PropertyFormModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> DeleteAsync(
            DeletePropertyFormModelMasterRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain> Delete(
            DeletePropertyFormModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class PropertyFormModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        this.PropertyFormModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetPropertyFormModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                                    this.PropertyFormModelName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "propertyFormModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            this.PropertyFormModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Model.PropertyFormModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                        this.PropertyFormModelName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetPropertyFormModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                                    this.PropertyFormModelName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "propertyFormModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Formation.Model.PropertyFormModelMaster>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.PropertyFormModelMasterDomain.CreateCacheKey(
                            this.PropertyFormModelName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.PropertyFormModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.PropertyFormModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.PropertyFormModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.PropertyFormModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
