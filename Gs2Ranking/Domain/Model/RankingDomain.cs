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

    public partial class RankingDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _categoryName;

        private readonly String _parentKey;
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;
        public string CategoryName => _categoryName;

        public RankingDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId,
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
            this._userId = userId;
            this._categoryName = categoryName;
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "Singleton",
                "Ranking"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Ranking.Model.Ranking> GetAsync(
            #else
        private IFuture<Gs2.Gs2Ranking.Model.Ranking> Get(
            #endif
        #else
        private async Task<Gs2.Gs2Ranking.Model.Ranking> GetAsync(
        #endif
            GetRankingByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Ranking> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithScorerUserId(this.UserId)
                .WithUserId(this._userId)
                .WithCategoryName(this._categoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.GetRankingByUserIdFuture(
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
            var result = await this._client.GetRankingByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Singleton",
                    "Ranking"
                );
                var key = Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    requestModel.CategoryName.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(result?.Item);
        #else
            return result?.Item;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Ranking>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScoreAsync(
            #else
        public IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScore(
            #endif
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScoreAsync(
        #endif
            PutScoreByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId)
                .WithCategoryName(this._categoryName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PutScoreByUserIdFuture(
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
            var result = await this._client.PutScoreByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
          
            {
                var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                    _namespaceName.ToString(),
                    "Singleton",
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
            Gs2.Gs2Ranking.Domain.Model.ScoreDomain domain = new Gs2.Gs2Ranking.Domain.Model.ScoreDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.CategoryName,
                result?.Item?.ScorerUserId,
                result?.Item?.UniqueId
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.ScoreDomain>(Impl);
        #endif
        }

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
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Model.Ranking> Model() {
            #else
        public IFuture<Gs2.Gs2Ranking.Model.Ranking> Model() {
            #endif
        #else
        public async Task<Gs2.Gs2Ranking.Model.Ranking> Model() {
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Model.Ranking> self)
            {
        #endif
            Gs2.Gs2Ranking.Model.Ranking value = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    this.CategoryName?.ToString()
                )
            );
            if (value == null) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    var future = this.Get(
        #else
                try {
                    await this.GetAsync(
        #endif
                        new GetRankingByUserIdRequest()
                    );
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return future;
                    if (future.Error != null)
                    {
                        if (future.Error is Gs2.Core.Exception.NotFoundException e)
                        {
                            if (e.errors[0].component == "ranking")
                            {
                                _cache.Delete<Gs2.Gs2Ranking.Model.Ranking>(
                                    _parentKey,
                                    Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                                        this.CategoryName?.ToString()
                                    )
                                );
                            }
                            else
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
                    if (e.errors[0].component == "ranking")
                    {
                    _cache.Delete<Gs2.Gs2Ranking.Model.Ranking>(
                            _parentKey,
                            Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                                this.CategoryName?.ToString()
                            )
                        );
                    }
                    else
                    {
                        throw e;
                    }
                }
        #endif
                value = _cache.Get<Gs2.Gs2Ranking.Model.Ranking>(
                _parentKey,
                Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                    this.CategoryName?.ToString()
                )
            );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(value);
            yield return null;
        #else
            return value;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Model.Ranking>(Impl);
        #endif
        }

    }
}
