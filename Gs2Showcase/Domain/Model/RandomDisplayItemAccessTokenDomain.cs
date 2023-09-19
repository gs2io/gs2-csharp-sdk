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

    public partial class RandomDisplayItemAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2ShowcaseRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _showcaseName;
        private readonly string _displayItemName;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string ShowcaseName => _showcaseName;
        public string DisplayItemName => _displayItemName;

        public RandomDisplayItemAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string showcaseName,
            string displayItemName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2ShowcaseRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._showcaseName = showcaseName;
            this._displayItemName = displayItemName;
            this._parentKey = Gs2.Gs2Showcase.Domain.Model.RandomShowcaseDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                this.ShowcaseName,
                "RandomDisplayItem"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem> GetFuture(
            GetRandomDisplayItemRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName)
                    .WithDisplayItemName(this.DisplayItemName);
                var future = this._client.GetRandomDisplayItemFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                            request.DisplayItemName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "randomDisplayItem")
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName)
                    .WithDisplayItemName(this.DisplayItemName);
                GetRandomDisplayItemResult result = null;
                try {
                    result = await this._client.GetRandomDisplayItemAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        request.DisplayItemName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "randomDisplayItem")
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
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Showcase.Model.RandomDisplayItem> GetAsync(
            GetRandomDisplayItemRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName)
                .WithDisplayItemName(this.DisplayItemName);
            var future = this._client.GetRandomDisplayItemFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        request.DisplayItemName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "randomDisplayItem")
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
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName)
                .WithDisplayItemName(this.DisplayItemName);
            GetRandomDisplayItemResult result = null;
            try {
                result = await this._client.GetRandomDisplayItemAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                    request.DisplayItemName.ToString()
                    );
                _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "randomDisplayItem")
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
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> RandomShowcaseBuyFuture(
            RandomShowcaseBuyRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName)
                    .WithDisplayItemName(this.DisplayItemName);
                var future = this._client.RandomShowcaseBuyFuture(
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
                    .WithAccessToken(this._accessToken?.Token)
                    .WithShowcaseName(this.ShowcaseName)
                    .WithDisplayItemName(this.DisplayItemName);
                RandomShowcaseBuyResult result = null;
                    result = await this._client.RandomShowcaseBuyAsync(
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
                var stampSheet = new Gs2.Core.Domain.TransactionAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId

                );
                if (result?.StampSheet != null)
                {
                    var future2 = stampSheet.Wait();
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }

            self.OnComplete(stampSheet);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> RandomShowcaseBuyAsync(
            RandomShowcaseBuyRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName)
                .WithDisplayItemName(this.DisplayItemName);
            var future = this._client.RandomShowcaseBuyFuture(
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
                .WithAccessToken(this._accessToken?.Token)
                .WithShowcaseName(this.ShowcaseName)
                .WithDisplayItemName(this.DisplayItemName);
            RandomShowcaseBuyResult result = null;
                result = await this._client.RandomShowcaseBuyAsync(
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
            var stampSheet = new Gs2.Core.Domain.TransactionAccessTokenDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId

            );
            if (result?.StampSheet != null)
            {
                await stampSheet.WaitAsync();
            }

            return stampSheet;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> RandomShowcaseBuyAsync(
            RandomShowcaseBuyRequest request
        ) {
            var future = RandomShowcaseBuyFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to RandomShowcaseBuyFuture.")]
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> RandomShowcaseBuy(
            RandomShowcaseBuyRequest request
        ) {
            return RandomShowcaseBuyFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName,
            string childType
        )
        {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
                showcaseName ?? "null",
                displayItemName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string displayItemName
        )
        {
            return string.Join(
                ":",
                displayItemName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        this.DisplayItemName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRandomDisplayItemRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                    this.DisplayItemName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "randomDisplayItem")
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
                    (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                            this.DisplayItemName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Showcase.Model.RandomDisplayItem> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                    _parentKey,
                    Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                        this.DisplayItemName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetRandomDisplayItemRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                                    this.DisplayItemName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "randomDisplayItem")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Showcase.Model.RandomDisplayItem>(
                        _parentKey,
                        Gs2.Gs2Showcase.Domain.Model.RandomDisplayItemDomain.CreateCacheKey(
                            this.DisplayItemName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Showcase.Model.RandomDisplayItem> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Showcase.Model.RandomDisplayItem> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Showcase.Model.RandomDisplayItem> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Showcase.Model.RandomDisplayItem> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
