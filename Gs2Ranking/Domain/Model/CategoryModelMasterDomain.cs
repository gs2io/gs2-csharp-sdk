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
#pragma warning disable CS0169, CS0168

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Ranking.Domain.Iterator;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
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

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class CategoryModelMasterDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _categoryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string CategoryName => _categoryName;

        public CategoryModelMasterDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string categoryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RankingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._categoryName = categoryName;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "CategoryModelMaster"
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
                "ranking",
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

    public partial class CategoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster> GetFuture(
            GetCategoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.GetCategoryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                            request.CategoryName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "categoryModelMaster")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CategoryModelMaster"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Ranking.Model.CategoryModelMaster> GetAsync(
            #else
        private async Task<Gs2.Gs2Ranking.Model.CategoryModelMaster> GetAsync(
            #endif
            GetCategoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName);
            GetCategoryModelMasterResult result = null;
            try {
                result = await this._client.GetCategoryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                    request.CategoryName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "categoryModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CategoryModelMaster"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
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
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> UpdateFuture(
            UpdateCategoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.UpdateCategoryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CategoryModelMaster"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> UpdateAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> UpdateAsync(
            #endif
            UpdateCategoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName);
            UpdateCategoryModelMasterResult result = null;
                result = await this._client.UpdateCategoryModelMasterAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CategoryModelMaster"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
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
        [Obsolete("The name has been changed to UpdateFuture.")]
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> Update(
            UpdateCategoryModelMasterRequest request
        ) {
            return UpdateFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> DeleteFuture(
            DeleteCategoryModelMasterRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.DeleteCategoryModelMasterFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                            request.CategoryName.ToString()
                        );
                        this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors.Length == 0 || future.Error.Errors[0].Component != "categoryModelMaster")
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

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            "CategoryModelMaster"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                            resultModel.Item.Name.ToString()
                        );
                        cache.Delete<Gs2.Gs2Ranking.Model.CategoryModelMaster>(parentKey, key);
                    }
                }
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> DeleteAsync(
            #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> DeleteAsync(
            #endif
            DeleteCategoryModelMasterRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName);
            DeleteCategoryModelMasterResult result = null;
            try {
                result = await this._client.DeleteCategoryModelMasterAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                    request.CategoryName.ToString()
                    );
                this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors.Length == 0 || e.Errors[0].Component != "categoryModelMaster")
                {
                    throw;
                }
            }

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        "CategoryModelMaster"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                        resultModel.Item.Name.ToString()
                    );
                    cache.Delete<Gs2.Gs2Ranking.Model.CategoryModelMaster>(parentKey, key);
                }
            }
                var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to DeleteFuture.")]
        public IFuture<Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain> Delete(
            DeleteCategoryModelMasterRequest request
        ) {
            return DeleteFuture(request);
        }
        #endif

    }

    public partial class CategoryModelMasterDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster> self)
            {
                var (value, find) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                        this.CategoryName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetCategoryModelMasterRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                                    this.CategoryName?.ToString()
                                );
                            this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors.Length == 0 || e.errors[0].component != "categoryModelMaster")
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
                    (value, _) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                            this.CategoryName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster>(Impl);
        }
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking.Model.CategoryModelMaster> ModelAsync()
            #else
        public async Task<Gs2.Gs2Ranking.Model.CategoryModelMaster> ModelAsync()
            #endif
        {
            var (value, find) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                    _parentKey,
                    Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                        this.CategoryName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetCategoryModelMasterRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                                    this.CategoryName?.ToString()
                                );
                    this._gs2.Cache.Put<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors.Length == 0 || e.errors[0].component != "categoryModelMaster")
                    {
                        throw;
                    }
                }
                (value, _) = _gs2.Cache.Get<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                        _parentKey,
                        Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                            this.CategoryName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking.Model.CategoryModelMaster> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking.Model.CategoryModelMaster> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking.Model.CategoryModelMaster> Model()
        {
            return await ModelAsync();
        }
        #endif


        public ulong Subscribe(Action<Gs2.Gs2Ranking.Model.CategoryModelMaster> callback)
        {
            return this._gs2.Cache.Subscribe(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                    this.CategoryName.ToString()
                ),
                callback
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking.Model.CategoryModelMaster>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.CategoryModelMasterDomain.CreateCacheKey(
                    this.CategoryName.ToString()
                ),
                callbackId
            );
        }

    }
}
