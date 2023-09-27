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
using Gs2.Gs2Dictionary.Domain.Iterator;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Result;
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

namespace Gs2.Gs2Dictionary.Domain.Model
{

    public partial class EntryModelMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2DictionaryRestClient _client;
        private readonly string _namespaceName;
        private readonly string _entryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string EntryName => _entryName;

        public EntryModelMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string entryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2DictionaryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._entryName = entryName;
            this._parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "EntryModelMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string entryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "dictionary",
                namespaceName ?? "null",
                entryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string entryName
        )
        {
            return string.Join(
                ":",
                entryName ?? "null"
            );
        }

    }

    public partial class EntryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> GetFuture(
            GetEntryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEntryName(this.EntryName);
                var future = this._client.GetEntryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                            request.EntryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "entryModelMaster")
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
                    .WithEntryName(this.EntryName);
                GetEntryModelMasterResult result = null;
                try {
                    result = await this._client.GetEntryModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        request.EntryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "entryModelMaster")
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
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EntryModelMaster"
                        );
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> GetAsync(
            GetEntryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var future = this._client.GetEntryModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        request.EntryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "entryModelMaster")
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
                .WithEntryName(this.EntryName);
            GetEntryModelMasterResult result = null;
            try {
                result = await this._client.GetEntryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                    request.EntryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "entryModelMaster")
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
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EntryModelMaster"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateFuture(
            UpdateEntryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEntryName(this.EntryName);
                var future = this._client.UpdateEntryModelMasterFuture(
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
                    .WithEntryName(this.EntryName);
                UpdateEntryModelMasterResult result = null;
                    result = await this._client.UpdateEntryModelMasterAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EntryModelMaster"
                        );
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateAsync(
            UpdateEntryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var future = this._client.UpdateEntryModelMasterFuture(
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
                .WithEntryName(this.EntryName);
            UpdateEntryModelMasterResult result = null;
                result = await this._client.UpdateEntryModelMasterAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EntryModelMaster"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> UpdateAsync(
            UpdateEntryModelMasterRequest request
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
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> Update(
            UpdateEntryModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteFuture(
            DeleteEntryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithEntryName(this.EntryName);
                var future = this._client.DeleteEntryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                            request.EntryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "entryModelMaster")
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
                    .WithEntryName(this.EntryName);
                DeleteEntryModelMasterResult result = null;
                try {
                    result = await this._client.DeleteEntryModelMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        request.EntryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "entryModelMaster")
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
                        var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "EntryModelMaster"
                        );
                        var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Dictionary.Model.EntryModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteAsync(
            DeleteEntryModelMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithEntryName(this.EntryName);
            var future = this._client.DeleteEntryModelMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        request.EntryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "entryModelMaster")
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
                .WithEntryName(this.EntryName);
            DeleteEntryModelMasterResult result = null;
            try {
                result = await this._client.DeleteEntryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                    request.EntryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "entryModelMaster")
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
                    var parentKey = Gs2.Gs2Dictionary.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "EntryModelMaster"
                    );
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Dictionary.Model.EntryModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> DeleteAsync(
            DeleteEntryModelMasterRequest request
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
        public IFuture<Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain> Delete(
            DeleteEntryModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class EntryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        this.EntryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetEntryModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                                    this.EntryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "entryModelMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                            this.EntryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                        this.EntryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetEntryModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                                    this.EntryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "entryModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                            this.EntryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Dictionary.Model.EntryModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Dictionary.Model.EntryModelMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                    this.EntryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Dictionary.Model.EntryModelMaster>(
                _parentKey,
                Gs2.Gs2Dictionary.Domain.Model.EntryModelMasterDomain.CreateCacheKey(
                    this.EntryName.ToString()
                ),
                callbackId
            );
        }

    }
}
