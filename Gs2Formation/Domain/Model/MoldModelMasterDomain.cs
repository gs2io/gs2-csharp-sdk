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

    public partial class MoldModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2FormationRestClient _client;
        private readonly string _namespaceName;
        private readonly string _moldModelName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string MoldModelName => _moldModelName;

        public MoldModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string moldModelName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2FormationRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._moldModelName = moldModelName;
            this._parentKey = Gs2.Gs2Formation.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "MoldModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string moldModelName,
            string childType
        )
        {
            return string.Join(
                ":",
                "formation",
                namespaceName ?? "null",
                moldModelName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string moldModelName
        )
        {
            return string.Join(
                ":",
                moldModelName ?? "null"
            );
        }

    }

    public partial class MoldModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Formation.Model.MoldModelMaster> GetFuture(
            GetMoldModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.MoldModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMoldModelName(this.MoldModelName);
                var future = this._client.GetMoldModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            request.MoldModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "moldModelMaster")
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
                    .WithMoldModelName(this.MoldModelName);
                GetMoldModelMasterResult result = null;
                try {
                    result = await this._client.GetMoldModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        request.MoldModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "moldModelMaster")
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
                            "MoldModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.MoldModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Formation.Model.MoldModelMaster> GetAsync(
            GetMoldModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMoldModelName(this.MoldModelName);
            var future = this._client.GetMoldModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        request.MoldModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "moldModelMaster")
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
                .WithMoldModelName(this.MoldModelName);
            GetMoldModelMasterResult result = null;
            try {
                result = await this._client.GetMoldModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                    request.MoldModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "moldModelMaster")
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
                        "MoldModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> UpdateFuture(
            UpdateMoldModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMoldModelName(this.MoldModelName);
                var future = this._client.UpdateMoldModelMasterFuture(
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
                    .WithMoldModelName(this.MoldModelName);
                UpdateMoldModelMasterResult result = null;
                    result = await this._client.UpdateMoldModelMasterAsync(
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
                            "MoldModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> UpdateAsync(
            UpdateMoldModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMoldModelName(this.MoldModelName);
            var future = this._client.UpdateMoldModelMasterFuture(
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
                .WithMoldModelName(this.MoldModelName);
            UpdateMoldModelMasterResult result = null;
                result = await this._client.UpdateMoldModelMasterAsync(
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
                        "MoldModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> UpdateAsync(
            UpdateMoldModelMasterRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> Update(
            UpdateMoldModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> DeleteFuture(
            DeleteMoldModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithMoldModelName(this.MoldModelName);
                var future = this._client.DeleteMoldModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            request.MoldModelName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "moldModelMaster")
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
                    .WithMoldModelName(this.MoldModelName);
                DeleteMoldModelMasterResult result = null;
                try {
                    result = await this._client.DeleteMoldModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        request.MoldModelName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "moldModelMaster")
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
                            "MoldModelMaster"
                        );
                        var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Formation.Model.MoldModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> DeleteAsync(
            DeleteMoldModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithMoldModelName(this.MoldModelName);
            var future = this._client.DeleteMoldModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        request.MoldModelName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "moldModelMaster")
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
                .WithMoldModelName(this.MoldModelName);
            DeleteMoldModelMasterResult result = null;
            try {
                result = await this._client.DeleteMoldModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                    request.MoldModelName.ToString()
                    );
                _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "moldModelMaster")
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
                        "MoldModelMaster"
                    );
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Formation.Model.MoldModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> DeleteAsync(
            DeleteMoldModelMasterRequest request
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
        public IFuture<Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain> Delete(
            DeleteMoldModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class MoldModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Formation.Model.MoldModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Formation.Model.MoldModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.MoldModelMaster>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        this.MoldModelName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetMoldModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                                    this.MoldModelName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "moldModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            this.MoldModelName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Formation.Model.MoldModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Formation.Model.MoldModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Formation.Model.MoldModelMaster>(
                    _parentKey,
                    Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                        this.MoldModelName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetMoldModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                                    this.MoldModelName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "moldModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Formation.Model.MoldModelMaster>(
                        _parentKey,
                        Gs2.Gs2Formation.Domain.Model.MoldModelMasterDomain.CreateCacheKey(
                            this.MoldModelName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Formation.Model.MoldModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Formation.Model.MoldModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Formation.Model.MoldModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Formation.Model.MoldModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
