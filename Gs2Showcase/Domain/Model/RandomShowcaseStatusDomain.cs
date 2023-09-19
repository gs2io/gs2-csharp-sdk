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
 *
 * deny overwrite
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

    public partial class RandomShowcaseStatusDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _showcaseName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string ShowcaseName => _showcaseName;

        public RandomShowcaseStatusDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._showcaseName = showcaseName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "RandomShowcaseStatus"
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string showcaseName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
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

    public partial class RandomShowcaseStatusDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> IncrementPurchaseCountFuture(
            IncrementPurchaseCountByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.IncrementPurchaseCountByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                IncrementPurchaseCountByUserIdResult result = null;
                    result = await this._client.IncrementPurchaseCountByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.ShowcaseName,
                            "RandomDisplayItem"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.ShowcaseName,
                    request.DisplayItemName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> IncrementPurchaseCountAsync(
            IncrementPurchaseCountByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.IncrementPurchaseCountByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            IncrementPurchaseCountByUserIdResult result = null;
                result = await this._client.IncrementPurchaseCountByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        "RandomDisplayItem"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.ShowcaseName,
                    request.DisplayItemName
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> IncrementPurchaseCountAsync(
            IncrementPurchaseCountByUserIdRequest request
        ) {
            var future = IncrementPurchaseCountFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to IncrementPurchaseCountFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> IncrementPurchaseCount(
            IncrementPurchaseCountByUserIdRequest request
        ) {
            return IncrementPurchaseCountFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> DecrementPurchaseCountFuture(
            DecrementPurchaseCountByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.DecrementPurchaseCountByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                DecrementPurchaseCountByUserIdResult result = null;
                    result = await this._client.DecrementPurchaseCountByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.ShowcaseName,
                            "RandomDisplayItem"
                        );
                        var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.ShowcaseName,
                    request.DisplayItemName
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> DecrementPurchaseCountAsync(
            DecrementPurchaseCountByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.DecrementPurchaseCountByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            DecrementPurchaseCountByUserIdResult result = null;
                result = await this._client.DecrementPurchaseCountByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        "RandomDisplayItem"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
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
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    request.UserId,
                    result?.Item?.ShowcaseName,
                    request.DisplayItemName
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> DecrementPurchaseCountAsync(
            DecrementPurchaseCountByUserIdRequest request
        ) {
            var future = DecrementPurchaseCountFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to DecrementPurchaseCountFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain> DecrementPurchaseCount(
            DecrementPurchaseCountByUserIdRequest request
        ) {
            return DecrementPurchaseCountFuture(request);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]> ForceReDrawFuture(
            ForceReDrawByUserIdRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                var future = this._client.ForceReDrawByUserIdFuture(
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
                    .WithUserId(this.UserId)
                    .WithShowcaseName(this.ShowcaseName);
                ForceReDrawByUserIdResult result = null;
                    result = await this._client.ForceReDrawByUserIdAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    {
                        var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.ShowcaseName,
                            "RandomDisplayItem"
                        );
                        foreach (var item in resultModel.Items) {
                            var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                item.Name.ToString()
                            );
                            cache.Put(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );
                        }
                    }
                }
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        request.UserId,
                        result.Items[i]?.ShowcaseName,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        "RandomDisplayItem"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]> ForceReDrawAsync(
            ForceReDrawByUserIdRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            var future = this._client.ForceReDrawByUserIdFuture(
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
                .WithUserId(this.UserId)
                .WithShowcaseName(this.ShowcaseName);
            ForceReDrawByUserIdResult result = null;
                result = await this._client.ForceReDrawByUserIdAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                {
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        "RandomDisplayItem"
                    );
                    foreach (var item in resultModel.Items) {
                        var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                            item.Name.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
            }
                var domain = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[result?.Items.Length ?? 0];
                for (int i=0; i<result?.Items.Length; i++)
                {
                    domain[i] = new Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain(
                        this._cache,
                        this._jobQueueDomain,
                        this._stampSheetConfiguration,
                        this._session,
                        request.NamespaceName,
                        request.UserId,
                        result.Items[i]?.ShowcaseName,
                        result.Items[i]?.Name
                    );
                    var parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        this.ShowcaseName,
                        "RandomDisplayItem"
                    );
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        result.Items[i].Name.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        result.Items[i],
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]> ForceReDrawAsync(
            ForceReDrawByUserIdRequest request
        ) {
            var future = ForceReDrawFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ForceReDrawFuture.")]
        public IFuture<Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain[]> ForceReDraw(
            ForceReDrawByUserIdRequest request
        ) {
            return ForceReDrawFuture(request);
        }
        #endif

    }

    public partial class RandomShowcaseStatusDomain {

    }
}
