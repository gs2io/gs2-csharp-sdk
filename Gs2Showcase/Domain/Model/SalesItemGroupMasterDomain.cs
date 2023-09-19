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
using Gs2.Gs2Showcase.Domain.Iterator;
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Result;
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

namespace Gs2.Gs2Showcase.Domain.Model
{

    public partial class SalesItemGroupMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private readonly string _salesItemGroupName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string SalesItemGroupName => _salesItemGroupName;

        public SalesItemGroupMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string salesItemGroupName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._salesItemGroupName = salesItemGroupName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "SalesItemGroupMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string salesItemGroupName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                salesItemGroupName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string salesItemGroupName
        )
        {
            return string.Join(
                ":",
                salesItemGroupName ?? "null"
            );
        }

    }

    public partial class SalesItemGroupMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> GetFuture(
            GetSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.GetSalesItemGroupMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            request.SalesItemGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                GetSalesItemGroupMasterResult result = null;
                try {
                    result = await this._client.GetSalesItemGroupMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        request.SalesItemGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "salesItemGroupMaster")
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> GetAsync(
            GetSalesItemGroupMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            var future = this._client.GetSalesItemGroupMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        request.SalesItemGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                .WithSalesItemGroupName(this.SalesItemGroupName);
            GetSalesItemGroupMasterResult result = null;
            try {
                result = await this._client.GetSalesItemGroupMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    request.SalesItemGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "salesItemGroupMaster")
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
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateFuture(
            UpdateSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.UpdateSalesItemGroupMasterFuture(
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
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                UpdateSalesItemGroupMasterResult result = null;
                    result = await this._client.UpdateSalesItemGroupMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateAsync(
            UpdateSalesItemGroupMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            var future = this._client.UpdateSalesItemGroupMasterFuture(
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
                .WithSalesItemGroupName(this.SalesItemGroupName);
            UpdateSalesItemGroupMasterResult result = null;
                result = await this._client.UpdateSalesItemGroupMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> UpdateAsync(
            UpdateSalesItemGroupMasterRequest request
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> Update(
            UpdateSalesItemGroupMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteFuture(
            DeleteSalesItemGroupMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                var future = this._client.DeleteSalesItemGroupMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            request.SalesItemGroupName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                    .WithSalesItemGroupName(this.SalesItemGroupName);
                DeleteSalesItemGroupMasterResult result = null;
                try {
                    result = await this._client.DeleteSalesItemGroupMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        request.SalesItemGroupName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "salesItemGroupMaster")
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
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "SalesItemGroupMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteAsync(
            DeleteSalesItemGroupMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithSalesItemGroupName(this.SalesItemGroupName);
            var future = this._client.DeleteSalesItemGroupMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        request.SalesItemGroupName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "salesItemGroupMaster")
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
                .WithSalesItemGroupName(this.SalesItemGroupName);
            DeleteSalesItemGroupMasterResult result = null;
            try {
                result = await this._client.DeleteSalesItemGroupMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                    request.SalesItemGroupName.ToString()
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "salesItemGroupMaster")
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
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "SalesItemGroupMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> DeleteAsync(
            DeleteSalesItemGroupMasterRequest request
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain> Delete(
            DeleteSalesItemGroupMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class SalesItemGroupMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        this.SalesItemGroupName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetSalesItemGroupMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                                    this.SalesItemGroupName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "salesItemGroupMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            this.SalesItemGroupName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                        this.SalesItemGroupName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetSalesItemGroupMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                                    this.SalesItemGroupName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "salesItemGroupMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.SalesItemGroupMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.SalesItemGroupMasterDomain.CreateCacheKey(
                            this.SalesItemGroupName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.SalesItemGroupMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
