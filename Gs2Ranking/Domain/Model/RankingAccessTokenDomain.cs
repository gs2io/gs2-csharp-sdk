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

    public partial class RankingAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _categoryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string CategoryName => _categoryName;

        public RankingAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string categoryName
        ) {
            this._cache = cache;
            this._jobQueueDomain = jobQueueDomain;
            this._stampSheetConfiguration = stampSheetConfiguration;
            this._session = session;
            this._client = new Gs2RankingRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._categoryName = categoryName;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Ranking"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking.Model.Ranking> GetFuture(
            GetRankingRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Ranking> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.GetRankingFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
                        var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                            request.CategoryName.ToString()
                        );
                        _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                            _parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );

                        if (future.Error.Errors[0].Component != "ranking")
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
                    .WithCategoryName(this.CategoryName);
                GetRankingResult result = null;
                try {
                    result = await this._client.GetRankingAsync(
                        request
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        request.CategoryName.ToString()
                        );
                    _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.Errors[0].Component != "ranking")
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
                        var parentKey = string.Join(
                            ":",
                            this.NamespaceName,
                            this.UserId,
                            this.CategoryName,
                            "Ranking"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                            resultModel.Item.UserId.ToString()
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
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Ranking>(Impl);
        }
        #else
        private async Task<Gs2.Gs2Ranking.Model.Ranking> GetAsync(
            GetRankingRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithCategoryName(this.CategoryName);
            var future = this._client.GetRankingFuture(
                request
            );
            yield return future;
            if (future.Error != null)
            {
                if (future.Error is Gs2.Core.Exception.NotFoundException) {
                    var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        request.CategoryName.ToString()
                    );
                    _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                        _parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (future.Error.Errors[0].Component != "ranking")
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
                .WithCategoryName(this.CategoryName);
            GetRankingResult result = null;
            try {
                result = await this._client.GetRankingAsync(
                    request
                );
            } catch (Gs2.Core.Exception.NotFoundException e) {
                var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    request.CategoryName.ToString()
                    );
                _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                    _parentKey,
                    key,
                    null,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );

                if (e.Errors[0].Component != "ranking")
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
                    var parentKey = string.Join(
                        ":",
                        this.NamespaceName,
                        this.UserId,
                        this.CategoryName,
                        "Ranking"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        resultModel.Item.UserId.ToString()
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
        public IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain> PutScoreFuture(
            PutScoreRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain> self)
            {
                #if UNITY_2017_1_OR_NEWER
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token)
                    .WithCategoryName(this.CategoryName);
                var future = this._client.PutScoreFuture(
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
                    .WithCategoryName(this.CategoryName);
                PutScoreResult result = null;
                    result = await this._client.PutScoreAsync(
                        request
                    );
                #endif

                var requestModel = request;
                var resultModel = result;
                var cache = _cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = string.Join(
                            ":",
                            this.NamespaceName,
                            this.UserId,
                            this.CategoryName,
                            resultModel.Item.ScorerUserId,
                            "Score"
                        );
                        var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            resultModel.Item.CategoryName.ToString(),
                            resultModel.Item.ScorerUserId.ToString(),
                            resultModel.Item.UniqueId.ToString()
                        );
                        cache.Put(
                            parentKey,
                            key,
                            resultModel.Item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                        );
                    }
                }
                var domain = new Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.CategoryName,
                    result?.Item?.ScorerUserId,
                    result?.Item?.UniqueId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain> PutScoreAsync(
            PutScoreRequest request
        ) {
            #if UNITY_2017_1_OR_NEWER
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithCategoryName(this.CategoryName);
            var future = this._client.PutScoreFuture(
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
                .WithCategoryName(this.CategoryName);
            PutScoreResult result = null;
                result = await this._client.PutScoreAsync(
                    request
                );
            #endif

            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = string.Join(
                        ":",
                        this.NamespaceName,
                        this.UserId,
                        this.CategoryName,
                        resultModel.Item.ScorerUserId,
                        "Score"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
                        resultModel.Item.ScorerUserId.ToString(),
                        resultModel.Item.UniqueId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
                var domain = new Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.CategoryName,
                    result?.Item?.ScorerUserId,
                    result?.Item?.UniqueId
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain> PutScoreAsync(
            PutScoreRequest request
        ) {
            var future = PutScoreFuture(request);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }
            #endif
        [Obsolete("The name has been changed to PutScoreFuture.")]
        public IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreAccessTokenDomain> PutScore(
            PutScoreRequest request
        ) {
            return PutScoreFuture(request);
        }
        #endif

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string categoryName,
            string childType
        )
        {
            return string.Join(
                ":",
                "ranking",
                namespaceName ?? "null",
                userId ?? "null",
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
        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Model.Ranking> ModelFuture(
            string scorerUserId
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Ranking> self)
            {
                var parentKey = string.Join(
                    ":",
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    "Ranking"
                );
                var (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        scorerUserId?.ToString()
                    )
                );
                if (!find) {
                    var future = this.GetFuture(
                        new GetRankingRequest()
                            .WithScorerUserId(scorerUserId)
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                                    scorerUserId?.ToString()
                                );
                            _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "ranking")
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
                    (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        scorerUserId?.ToString()
                    )
                );
                }
                self.OnComplete(value);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Ranking>(Impl);
        }
        #else
        public async Task<Gs2.Gs2Ranking.Model.Ranking> ModelAsync(
            string scorerUserId
        ) {
            var parentKey = string.Join(
                ":",
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                "Ranking"
            );
            var (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        scorerUserId?.ToString()
                    )
                );
            if (!find) {
                try {
                    await this.GetAsync(
                        new GetRankingRequest()
                            .WithScorerUserId(scorerUserId)
                    );
                } catch (Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                                    scorerUserId?.ToString()
                                );
                    _cache.Put<Gs2.Gs2Ranking.Model.Ranking>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );

                    if (e.errors[0].component != "ranking")
                    {
                        throw;
                    }
                }
                (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                        scorerUserId?.ToString()
                    )
                );
            }
            return value;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Model.Ranking> ModelAsync(
            string scorerUserId
        )
        {
            var future = ModelFuture(scorerUserId);
            await future;
            if (future.Error != null) {
                throw future.Error;
            }
            return future.Result;
        }

        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking.Model.Ranking> Model(
            string scorerUserId
        )
        {
            return await ModelAsync(scorerUserId);
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking.Model.Ranking> Model(
            string scorerUserId
        )
        {
            return ModelFuture(scorerUserId);
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking.Model.Ranking> Model(
            string scorerUserId
        )
        {
            return await ModelAsync(scorerUserId);
        }
        #endif

        public ulong Subscribe(
            Action<Gs2.Gs2Ranking.Model.Ranking> callback,
            string userId
        )
        {
            return this._cache.Subscribe(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    userId
                ),
                callback
            );
        }

        public void Unsubscribe(
            ulong callbackId,
            string userId
        )
        {
            this._cache.Unsubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    userId
                ),
                callbackId
            );
        }

    }
}
