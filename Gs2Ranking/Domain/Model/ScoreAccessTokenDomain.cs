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
using Gs2.Gs2Ranking.Domain.Iterator;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
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

    public partial class ScoreAccessTokenDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;
        private readonly string _categoryName;
        private readonly string _scorerUserId;
        private readonly string _uniqueId;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;
        public string CategoryName => _categoryName;
        public string ScorerUserId => _scorerUserId;
        public string UniqueId => _uniqueId;

        public ScoreAccessTokenDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            AccessToken accessToken,
            string categoryName,
            string scorerUserId,
            string uniqueId
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
            this._scorerUserId = scorerUserId;
            this._uniqueId = uniqueId;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Score"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Ranking.Model.Score> GetAsync(
            #else
        private IFuture<Gs2.Gs2Ranking.Model.Score> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Ranking.Model.Score> GetAsync(
        #endif
            GetScoreRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Score> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token)
                .WithCategoryName(this.CategoryName)
                .WithScorerUserId(this.ScorerUserId)
                .WithUniqueId(this.UniqueId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetScoreFuture(
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
            var result = await this._client.GetScoreAsync(
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
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Score>(Impl);
        #endif
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string categoryName,
            string scorerUserId,
            string uniqueId,
            string childType
        )
        {
            return string.Join(
                ":",
                "ranking",
                namespaceName ?? "null",
                userId ?? "null",
                categoryName ?? "null",
                scorerUserId ?? "null",
                uniqueId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string categoryName,
            string scorerUserId,
            string uniqueId
        )
        {
            return string.Join(
                ":",
                categoryName ?? "null",
                scorerUserId ?? "null",
                uniqueId ?? "null"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Model.Score> Model() {
            #else
        public IFuture<Gs2.Gs2Ranking.Model.Score> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Ranking.Model.Score> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Score> self)
            {
        #endif
            var parentKey = string.Join(
                ":",
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.ScorerUserId,
                "Score"
            );
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            using (await this._cache.GetLockObject<Gs2.Gs2Ranking.Model.Score>(
                       _parentKey,
                       Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.ScorerUserId?.ToString(),
                            this.UniqueId?.ToString() ?? "0"
                        )).LockAsync())
            {
        # endif
            var (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Score>(
                parentKey,
                Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                    this.CategoryName?.ToString(),
                    this.ScorerUserId?.ToString(),
                    this.UniqueId?.ToString() ?? "0"
                )
            );
            if (!find) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetScoreRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                                    this.CategoryName?.ToString(),
                                    this.ScorerUserId?.ToString(),
                                    this.UniqueId?.ToString() ?? "0"
                                );
                            _cache.Put<Gs2.Gs2Ranking.Model.Score>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                            );

                            if (e.errors[0].component != "score")
                            {
                                self.OnError(future.Error);
                            }
                        }
                        else
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
        #else
                } catch(Gs2.Core.Exception.NotFoundException e) {
                    var key = Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                            this.CategoryName?.ToString(),
                            this.ScorerUserId?.ToString(),
                            this.UniqueId?.ToString() ?? "0"
                        );
                    _cache.Put<Gs2.Gs2Ranking.Model.Score>(
                        parentKey,
                        key,
                        null,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                    if (e.errors[0].component != "score")
                    {
                        throw e;
                    }
                }
        #endif
                (value, find) = _cache.Get<Gs2.Gs2Ranking.Model.Score>(
                    parentKey,
                    Gs2.Gs2Ranking.Domain.Model.ScoreDomain.CreateCacheKey(
                        this.CategoryName?.ToString(),
                        this.ScorerUserId?.ToString(),
                        this.UniqueId?.ToString() ?? "0"
                    )
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if (UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK) || !UNITY_2017_1_OR_NEWER
            }
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Score>(Impl);
        #endif
        }

    }
}
