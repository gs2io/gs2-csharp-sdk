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
using Gs2.Gs2Log.Domain.Iterator;
using Gs2.Gs2Log.Request;
using Gs2.Gs2Log.Result;
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

namespace Gs2.Gs2Log.Domain.Model
{

    public partial class InsightDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LogRestClient _client;
        private readonly string _namespaceName;
        private readonly string _insightName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string InsightName => _insightName;

        public InsightDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string insightName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LogRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._insightName = insightName;
            this._parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "Insight"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string insightName,
            string childType
        )
        {
            return string.Join(
                ":",
                "log",
                namespaceName ?? "null",
                insightName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string insightName
        )
        {
            return string.Join(
                ":",
                insightName ?? "null"
            );
        }

    }

    public partial class InsightDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Log.Model.Insight> GetFuture(
            GetInsightRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Insight> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInsightName(this.InsightName);
                var future = this._client.GetInsightFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            request.InsightName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Log.Model.Insight>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "insight")
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
                    .WithInsightName(this.InsightName);
                GetInsightResult result = null;
                try {
                    result = await this._client.GetInsightAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        request.InsightName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "insight")
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
                        var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Insight"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Insight>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Log.Model.Insight> GetAsync(
            GetInsightRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInsightName(this.InsightName);
            var future = this._client.GetInsightFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        request.InsightName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "insight")
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
                .WithInsightName(this.InsightName);
            GetInsightResult result = null;
            try {
                result = await this._client.GetInsightAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                    request.InsightName.ToString()
                    );
                _cache.Put<Gs2.Gs2Log.Model.Insight>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "insight")
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
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> DeleteFuture(
            DeleteInsightRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithInsightName(this.InsightName);
                var future = this._client.DeleteInsightFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            request.InsightName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Log.Model.Insight>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "insight")
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
                    .WithInsightName(this.InsightName);
                DeleteInsightResult result = null;
                try {
                    result = await this._client.DeleteInsightAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        request.InsightName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "insight")
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
                        var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "Insight"
                        );
                        var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Log.Model.Insight>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Domain.Model.InsightDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Domain.Model.InsightDomain> DeleteAsync(
            DeleteInsightRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithInsightName(this.InsightName);
            var future = this._client.DeleteInsightFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        request.InsightName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "insight")
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
                .WithInsightName(this.InsightName);
            DeleteInsightResult result = null;
            try {
                result = await this._client.DeleteInsightAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                    request.InsightName.ToString()
                    );
                _cache.Put<Gs2.Gs2Log.Model.Insight>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "insight")
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
                    var parentKey = Gs2.Gs2Log.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "Insight"
                    );
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Log.Model.Insight>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Domain.Model.InsightDomain> DeleteAsync(
            DeleteInsightRequest request
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
        public IFuture<Gs2.Gs2Log.Domain.Model.InsightDomain> Delete(
            DeleteInsightRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class InsightDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Log.Model.Insight> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Log.Model.Insight> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                    _parentKey,
                    Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        this.InsightName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetInsightRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                                    this.InsightName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Log.Model.Insight>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "insight")
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
                    (value, _) = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            this.InsightName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Log.Model.Insight>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Log.Model.Insight> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                    _parentKey,
                    Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                        this.InsightName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetInsightRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                                    this.InsightName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "insight")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Log.Model.Insight>(
                        _parentKey,
                        Gs2.Gs2Log.Domain.Model.InsightDomain.CreateCacheKey(
                            this.InsightName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Log.Model.Insight> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Log.Model.Insight> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Log.Model.Insight> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Log.Model.Insight> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
