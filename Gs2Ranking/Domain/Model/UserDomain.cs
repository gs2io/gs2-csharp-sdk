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

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public bool? Processing { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _userId;

        public UserDomain(
            CacheDatabase cache,
            JobQueueDomain jobQueueDomain,
            StampSheetConfiguration stampSheetConfiguration,
            Gs2RestSession session,
            string namespaceName,
            string userId
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
            this._parentKey = Gs2.Gs2Ranking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
            #else
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> Subscribe(
            #endif
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
        #endif
            SubscribeByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.SubscribeByUserIdFuture(
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
            var result = await this._client.SubscribeByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
                        this.UserId,
                        "SubscribeUser"
                    );
                    var key = Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain.CreateCacheKey(
                        resultModel.Item.CategoryName.ToString(),
                        resultModel.Item.TargetUserId.ToString()
                    );
                    cache.Put(
                        parentKey,
                        key,
                        resultModel.Item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }
            }
            var domain = new Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.CategoryName,
                result?.Item?.TargetUserId
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain>(Impl);
        #endif
        }

        public Gs2.Gs2Ranking.Domain.Model.SubscribeDomain Subscribe(
            string categoryName
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                categoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryName(
            string categoryName
        )
        {
            return new DescribeSubscribesByCategoryNameAndUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryNameAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryName(
            #endif
        #else
        public DescribeSubscribesByCategoryNameAndUserIdIterator SubscribesByCategoryName(
        #endif
            string categoryName
        )
        {
            return new DescribeSubscribesByCategoryNameAndUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain SubscribeUser(
            string categoryName,
            string targetUserId
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                categoryName,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            string categoryName
        )
        {
            return new DescribeRankingsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> RankingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            #endif
        #else
        public DescribeRankingsByUserIdIterator Rankings(
        #endif
            string categoryName
        )
        {
            return new DescribeRankingsByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> NearRankings(
            string categoryName,
            long? score
        )
        {
            return new DescribeNearRankingsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                score
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> NearRankingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> NearRankings(
            #endif
        #else
        public DescribeNearRankingsIterator NearRankings(
        #endif
            string categoryName,
            long? score
        )
        {
            return new DescribeNearRankingsIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                score
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Ranking.Domain.Model.RankingDomain Ranking(
            string categoryName
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.RankingDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                categoryName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Score> Scores(
            string categoryName,
            string scorerUserId
        )
        {
            return new DescribeScoresByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId,
                scorerUserId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Score> ScoresAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Score> Scores(
            #endif
        #else
        public DescribeScoresByUserIdIterator Scores(
        #endif
            string categoryName,
            string scorerUserId
        )
        {
            return new DescribeScoresByUserIdIterator(
                this._cache,
                this._client,
                this.NamespaceName,
                categoryName,
                this.UserId,
                scorerUserId
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public Gs2.Gs2Ranking.Domain.Model.ScoreDomain Score(
            string categoryName,
            string scorerUserId,
            string uniqueId
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.ScoreDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this.NamespaceName,
                this.UserId,
                categoryName,
                scorerUserId,
                uniqueId
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "ranking",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }
}
