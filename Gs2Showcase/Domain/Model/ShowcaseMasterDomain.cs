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

    public partial class ShowcaseMasterDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private readonly string _showcaseName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string ShowcaseName => _showcaseName;

        public ShowcaseMasterDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string showcaseName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._showcaseName = showcaseName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "ShowcaseMaster"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string showcaseName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                showcaseName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string showcaseName
        )
        {
            return string.Join(
                ":",
                showcaseName ?? "null"
            );
        }

    }

    public partial class ShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster> GetFuture(
            GetShowcaseMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.GetShowcaseMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                            request.ShowcaseName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "showcaseMaster")
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
                    .WithShowcaseName(this.ShowcaseName);
                GetShowcaseMasterResult result = null;
                try {
                    result = await this._client.GetShowcaseMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        request.ShowcaseName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "showcaseMaster")
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
                            "ShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Showcase.Model.ShowcaseMaster> GetAsync(
            GetShowcaseMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.GetShowcaseMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        request.ShowcaseName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "showcaseMaster")
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
                .WithShowcaseName(this.ShowcaseName);
            GetShowcaseMasterResult result = null;
            try {
                result = await this._client.GetShowcaseMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                    request.ShowcaseName.ToString()
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "showcaseMaster")
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
                        "ShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> UpdateFuture(
            UpdateShowcaseMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.UpdateShowcaseMasterFuture(
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
                    .WithShowcaseName(this.ShowcaseName);
                UpdateShowcaseMasterResult result = null;
                    result = await this._client.UpdateShowcaseMasterAsync(
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
                            "ShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> UpdateAsync(
            UpdateShowcaseMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.UpdateShowcaseMasterFuture(
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
                .WithShowcaseName(this.ShowcaseName);
            UpdateShowcaseMasterResult result = null;
                result = await this._client.UpdateShowcaseMasterAsync(
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
                        "ShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
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
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> UpdateAsync(
            UpdateShowcaseMasterRequest request
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> Update(
            UpdateShowcaseMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> DeleteFuture(
            DeleteShowcaseMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.DeleteShowcaseMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                            request.ShowcaseName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "showcaseMaster")
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
                    .WithShowcaseName(this.ShowcaseName);
                DeleteShowcaseMasterResult result = null;
                try {
                    result = await this._client.DeleteShowcaseMasterAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        request.ShowcaseName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "showcaseMaster")
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
                            "ShowcaseMaster"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Showcase.Model.ShowcaseMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> DeleteAsync(
            DeleteShowcaseMasterRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.DeleteShowcaseMasterFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        request.ShowcaseName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "showcaseMaster")
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
                .WithShowcaseName(this.ShowcaseName);
            DeleteShowcaseMasterResult result = null;
            try {
                result = await this._client.DeleteShowcaseMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                    request.ShowcaseName.ToString()
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "showcaseMaster")
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
                        "ShowcaseMaster"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Showcase.Model.ShowcaseMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> DeleteAsync(
            DeleteShowcaseMasterRequest request
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
        public IFuture<Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain> Delete(
            DeleteShowcaseMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class ShowcaseMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetShowcaseMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                                    this.ShowcaseName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "showcaseMaster")
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
                    (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                            this.ShowcaseName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Model.ShowcaseMaster> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                        this.ShowcaseName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetShowcaseMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                                    this.ShowcaseName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "showcaseMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                            this.ShowcaseName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.ShowcaseMaster> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.ShowcaseMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.ShowcaseMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.ShowcaseMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Showcase.Model.ShowcaseMaster> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Showcase.Model.ShowcaseMaster>(
                _parentKey,
                Gs2.Gs2Showcase.Domain.Model.ShowcaseMasterDomain.CreateCacheKey(
                    this.ShowcaseName.ToString()
                ),
                callbackId
            );
        }

    }
}
