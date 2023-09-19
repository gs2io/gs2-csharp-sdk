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
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
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

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class BoxItemsAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _prizeTableName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string PrizeTableName => _prizeTableName;

        public BoxItemsAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string prizeTableName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2LotteryRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._prizeTableName = prizeTableName;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "BoxItems"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Lottery.Model.BoxItems> GetFuture(
            GetBoxRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = this._client.GetBoxFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            request.PrizeTableName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "boxItems")
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
                    .WithPrizeTableName(this.PrizeTableName);
                GetBoxResult result = null;
                try {
                    result = await this._client.GetBoxAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        request.PrizeTableName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "boxItems")
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
                        var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            "BoxItems"
                        );
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            resultModel.Item.PrizeTableName.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.BoxItems>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Lottery.Model.BoxItems> GetAsync(
            GetBoxRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPrizeTableName(this.PrizeTableName);
            var future = this._client.GetBoxFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        request.PrizeTableName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "boxItems")
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
                .WithPrizeTableName(this.PrizeTableName);
            GetBoxResult result = null;
            try {
                result = await this._client.GetBoxAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                    request.PrizeTableName.ToString()
                    );
                _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "boxItems")
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
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        resultModel.Item.PrizeTableName.ToString()
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
        public IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxFuture(
            ResetBoxRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithPrizeTableName(this.PrizeTableName);
                var future = this._client.ResetBoxFuture(
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
                    .WithPrizeTableName(this.PrizeTableName);
                ResetBoxResult result = null;
                    result = await this._client.ResetBoxAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    {
                        var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName?.ToString(),
                            this.UserId.ToString(),
                            "BoxItems"
                        );
                        var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            requestModel.PrizeTableName
                        );
                        this._cache.Delete<Gs2.Gs2Lottery.Model.BoxItems>(
                            parentKey,
                            key
                        );
                        this._cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                            parentKey
                        );
                    }
                }
                var domain = this;
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxAsync(
            ResetBoxRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithPrizeTableName(this.PrizeTableName);
            var future = this._client.ResetBoxFuture(
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
                .WithPrizeTableName(this.PrizeTableName);
            ResetBoxResult result = null;
                result = await this._client.ResetBoxAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                {
                    var parentKey = Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName?.ToString(),
                        this.UserId.ToString(),
                        "BoxItems"
                    );
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        requestModel.PrizeTableName
                    );
                    this._cache.Delete<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey,
                        key
                    );
                    this._cache.ClearListCache<Gs2.Gs2Lottery.Model.BoxItems>(
                        parentKey
                    );
                }
            }
                var domain = this;
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBoxAsync(
            ResetBoxRequest request
        ) {
            var future = ResetBoxFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to ResetBoxFuture.")]
        public IFuture<Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain> ResetBox(
            ResetBoxRequest request
        ) {
            return ResetBoxFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string prizeTableName,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                userId ?? "null",
                prizeTableName ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string prizeTableName
        )
        {
            return string.Join(
                ":",
                prizeTableName ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.BoxItems> self)
            {
                var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        this.PrizeTableName?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetBoxRequest()
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                                    this.PrizeTableName?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                                _parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "boxItems")
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
                    (value, _) = _cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            this.PrizeTableName?.ToString()
                        )
                    );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.BoxItems>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Lottery.Model.BoxItems> ModelAsync()
        {
            var (value, find) = _cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                    _parentKey,
                    Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                        this.PrizeTableName?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetBoxRequest()
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                                    this.PrizeTableName?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "boxItems")
                    {
                        throw;
                    }
                }
                (value, _) = _cache.Get<Gs2.Gs2Lottery.Model.BoxItems>(
                        _parentKey,
                        Gs2.Gs2Lottery.Domain.Model.BoxItemsDomain.CreateCacheKey(
                            this.PrizeTableName?.ToString()
                        )
                    );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.BoxItems> ModelAsync()
        {
            var future = ModelFuture();
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Lottery.Model.BoxItems> Model()
        {
            return await ModelAsync();
        }
        #endif

    }
}
