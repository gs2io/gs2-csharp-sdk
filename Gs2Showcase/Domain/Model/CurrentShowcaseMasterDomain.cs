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

    public partial class CurrentShowcaseMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;

        public CurrentShowcaseMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CurrentShowcaseMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
        )
        {
            return "Singleton";
        }

    }

    public partial class CurrentShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterFuture(
            ExportMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.ExportMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                ExportMasterResult result = null;
                    result = await this._client.ExportMasterAsync(
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
                            "CurrentShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterAsync(
            ExportMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.ExportMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            ExportMasterResult result = null;
                result = await this._client.ExportMasterAsync(
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
                        "CurrentShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMasterAsync(
            ExportMasterRequest request
        ) {
            var future = ExportMasterFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ExportMasterFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> ExportMaster(
            ExportMasterRequest request
        ) {
            return ExportMasterFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> GetFuture(
            GetCurrentShowcaseMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.GetCurrentShowcaseMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "currentShowcaseMaster")
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
                    .WithNamespaceName(this.NamespaceName);
                GetCurrentShowcaseMasterResult result = null;
                try {
                    result = await this._client.GetCurrentShowcaseMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "currentShowcaseMaster")
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
                            "CurrentShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> GetAsync(
            GetCurrentShowcaseMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.GetCurrentShowcaseMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "currentShowcaseMaster")
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
                .WithNamespaceName(this.NamespaceName);
            GetCurrentShowcaseMasterResult result = null;
            try {
                result = await this._client.GetCurrentShowcaseMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "currentShowcaseMaster")
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
                        "CurrentShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFuture(
            UpdateCurrentShowcaseMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentShowcaseMasterFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateCurrentShowcaseMasterResult result = null;
                    result = await this._client.UpdateCurrentShowcaseMasterAsync(
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
                            "CurrentShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateAsync(
            UpdateCurrentShowcaseMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateCurrentShowcaseMasterFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentShowcaseMasterResult result = null;
                result = await this._client.UpdateCurrentShowcaseMasterAsync(
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
                        "CurrentShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateAsync(
            UpdateCurrentShowcaseMasterRequest request
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> Update(
            UpdateCurrentShowcaseMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubFuture(
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName);
                var future = this._client.UpdateCurrentShowcaseMasterFromGitHubFuture(
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
                    .WithNamespaceName(this.NamespaceName);
                UpdateCurrentShowcaseMasterFromGitHubResult result = null;
                    result = await this._client.UpdateCurrentShowcaseMasterFromGitHubAsync(
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
                            "CurrentShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubAsync(
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName);
            var future = this._client.UpdateCurrentShowcaseMasterFromGitHubFuture(
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
                .WithNamespaceName(this.NamespaceName);
            UpdateCurrentShowcaseMasterFromGitHubResult result = null;
                result = await this._client.UpdateCurrentShowcaseMasterFromGitHubAsync(
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
                        "CurrentShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHubAsync(
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) {
            var future = UpdateFromGitHubFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to UpdateFromGitHubFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain> UpdateFromGitHub(
            UpdateCurrentShowcaseMasterFromGitHubRequest request
        ) {
            return UpdateFromGitHubFuture(request);
        }
        #endif

    }

    public partial class CurrentShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCurrentShowcaseMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                                );
                            _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "currentShowcaseMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCurrentShowcaseMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                                );
                    _cache.Put<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "currentShowcaseMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.CurrentShowcaseMasterDomain.CreateCacheKey(
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.CurrentShowcaseMaster> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
