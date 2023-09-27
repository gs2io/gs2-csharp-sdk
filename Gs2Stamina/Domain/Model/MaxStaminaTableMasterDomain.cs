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
using Gs2.Gs2Stamina.Domain.Iterator;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
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

namespace Gs2.Gs2Stamina.Domain.Model
{

    public partial class MaxStaminaTableMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2StaminaRestClient _client;
        private readonly string _namespaceName;
        private readonly string _maxStaminaTableName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string MaxStaminaTableName => _maxStaminaTableName;

        public MaxStaminaTableMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string maxStaminaTableName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2StaminaRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._maxStaminaTableName = maxStaminaTableName;
            this._parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "MaxStaminaTableMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string maxStaminaTableName,
            string childType
        )
        {
            return string.Join(
                ":",
                "stamina",
                namespaceName ?? "null",
                maxStaminaTableName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string maxStaminaTableName
        )
        {
            return string.Join(
                ":",
                maxStaminaTableName ?? "null"
            );
        }

    }

    public partial class MaxStaminaTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> GetFuture(
            GetMaxStaminaTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                var future = this._client.GetMaxStaminaTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                            request.MaxStaminaTableName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "maxStaminaTableMaster")
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
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                GetMaxStaminaTableMasterResult result = null;
                try {
                    result = await this._client.GetMaxStaminaTableMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        request.MaxStaminaTableName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "maxStaminaTableMaster")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MaxStaminaTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> GetAsync(
            GetMaxStaminaTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            var future = this._client.GetMaxStaminaTableMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        request.MaxStaminaTableName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "maxStaminaTableMaster")
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
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            GetMaxStaminaTableMasterResult result = null;
            try {
                result = await this._client.GetMaxStaminaTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                    request.MaxStaminaTableName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "maxStaminaTableMaster")
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
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MaxStaminaTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> UpdateFuture(
            UpdateMaxStaminaTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                var future = this._client.UpdateMaxStaminaTableMasterFuture(
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
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                UpdateMaxStaminaTableMasterResult result = null;
                    result = await this._client.UpdateMaxStaminaTableMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MaxStaminaTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> UpdateAsync(
            UpdateMaxStaminaTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            var future = this._client.UpdateMaxStaminaTableMasterFuture(
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
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            UpdateMaxStaminaTableMasterResult result = null;
                result = await this._client.UpdateMaxStaminaTableMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MaxStaminaTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> UpdateAsync(
            UpdateMaxStaminaTableMasterRequest request
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> Update(
            UpdateMaxStaminaTableMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> DeleteFuture(
            DeleteMaxStaminaTableMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                var future = this._client.DeleteMaxStaminaTableMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                            request.MaxStaminaTableName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "maxStaminaTableMaster")
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
                    .WithMaxStaminaTableName(this.MaxStaminaTableName);
                DeleteMaxStaminaTableMasterResult result = null;
                try {
                    result = await this._client.DeleteMaxStaminaTableMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        request.MaxStaminaTableName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "maxStaminaTableMaster")
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
                        var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "MaxStaminaTableMaster"
                        );
                        var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> DeleteAsync(
            DeleteMaxStaminaTableMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            var future = this._client.DeleteMaxStaminaTableMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        request.MaxStaminaTableName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "maxStaminaTableMaster")
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
                .WithMaxStaminaTableName(this.MaxStaminaTableName);
            DeleteMaxStaminaTableMasterResult result = null;
            try {
                result = await this._client.DeleteMaxStaminaTableMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                    request.MaxStaminaTableName.ToString()
                    );
                _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "maxStaminaTableMaster")
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
                    var parentKey = Gs2.Gs2Stamina.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "MaxStaminaTableMaster"
                    );
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> DeleteAsync(
            DeleteMaxStaminaTableMasterRequest request
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
        public IFuture<Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain> Delete(
            DeleteMaxStaminaTableMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class MaxStaminaTableMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        this.MaxStaminaTableName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMaxStaminaTableMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                                    this.MaxStaminaTableName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "maxStaminaTableMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                            this.MaxStaminaTableName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                    _parentKey,
                    Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                        this.MaxStaminaTableName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMaxStaminaTableMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                                    this.MaxStaminaTableName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "maxStaminaTableMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                        _parentKey,
                        Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                            this.MaxStaminaTableName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                    this.MaxStaminaTableName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Stamina.Model.MaxStaminaTableMaster>(
                _parentKey,
                Gs2.Gs2Stamina.Domain.Model.MaxStaminaTableMasterDomain.CreateCacheKey(
                    this.MaxStaminaTableName.ToString()
                ),
                callbackId
            );
        }

    }
}
