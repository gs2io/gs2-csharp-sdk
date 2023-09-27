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
using Gs2.Gs2Idle.Domain.Iterator;
using Gs2.Gs2Idle.Request;
using Gs2.Gs2Idle.Result;
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

namespace Gs2.Gs2Idle.Domain.Model
{

    public partial class CategoryModelDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2IdleRestClient _client;
        private readonly string _namespaceName;
        private readonly string _categoryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string CategoryName => _categoryName;

        public CategoryModelDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string categoryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2IdleRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._categoryName = categoryName;
            this._parentKey = Gs2.Gs2Idle.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CategoryModel"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string categoryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "idle",
                namespaceName ?? "null",
                categoryName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string categoryName
        )
        {
            return string.Join(
                ":",
                categoryName ?? "null"
            );
        }

    }

    public partial class CategoryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Idle.Model.CategoryModel> GetFuture(
            GetCategoryModelRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Idle.Model.CategoryModel> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.GetCategoryModelFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                            request.CategoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "categoryModel")
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
                    .WithCategoryName(this.CategoryName);
                GetCategoryModelResult result = null;
                try {
                    result = await this._client.GetCategoryModelAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                        request.CategoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "categoryModel")
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
                        var parentKey = Gs2.Gs2Idle.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CategoryModel"
                        );
                        var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Idle.Model.CategoryModel>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Idle.Model.CategoryModel> GetAsync(
            GetCategoryModelRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName);
            var future = this._client.GetCategoryModelFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                        request.CategoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "categoryModel")
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
                .WithCategoryName(this.CategoryName);
            GetCategoryModelResult result = null;
            try {
                result = await this._client.GetCategoryModelAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                    request.CategoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "categoryModel")
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
                    var parentKey = Gs2.Gs2Idle.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CategoryModel"
                    );
                    var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
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

    }

    public partial class CategoryModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Idle.Model.CategoryModel> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Idle.Model.CategoryModel> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Idle.Model.CategoryModel>(
                    _parentKey,
                    Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                        this.CategoryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCategoryModelRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                                    this.CategoryName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "categoryModel")
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
                    (value, _) = _cache.Get<Gs2.Gs2Idle.Model.CategoryModel>(
                        _parentKey,
                        Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                            this.CategoryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Idle.Model.CategoryModel>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Idle.Model.CategoryModel> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Idle.Model.CategoryModel>(
                    _parentKey,
                    Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                        this.CategoryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCategoryModelRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                                    this.CategoryName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Idle.Model.CategoryModel>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "categoryModel")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Idle.Model.CategoryModel>(
                        _parentKey,
                        Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                            this.CategoryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Idle.Model.CategoryModel> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Idle.Model.CategoryModel> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Idle.Model.CategoryModel> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Idle.Model.CategoryModel> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Idle.Model.CategoryModel> callback)
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                    this.CategoryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._cache.Unsubscribe<Gs2.Gs2Idle.Model.CategoryModel>(
                _parentKey,
                Gs2.Gs2Idle.Domain.Model.CategoryModelDomain.CreateCacheKey(
                    this.CategoryName.ToString()
                ),
                callbackId
            );
        }

    }
}
