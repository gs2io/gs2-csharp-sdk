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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class UserDomain {
        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _stampSheetConfiguration;
        private readonly Gs2RestSession _session;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _userId;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string MatchmakingContextToken { get; set; }
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
            this._client = new Gs2MatchmakingRestClient(
                session
            );
            this._namespaceName = namespaceName;
            this._userId = userId;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this._namespaceName?.ToString() ?? null,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
            #else
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGathering(
            #endif
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
        #endif
            CreateGatheringByUserIdRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName)
                .WithUserId(this._userId);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.CreateGatheringByUserIdFuture(
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
            var result = await this._client.CreateGatheringByUserIdAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;

            {
                var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this._namespaceName?.ToString() ?? null,
                    "Singleton",
                    "Gathering"
                );
                var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    resultModel.Item,
                    resultModel.Item.ExpiresAt ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                request.UserId,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
            #else
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGathering(
            #endif
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
        #endif
            DeleteGatheringRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.DeleteGatheringFuture(
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
            DeleteGatheringResult result = null;
            try {
                result = await this._client.DeleteGatheringAsync(
                    request
                );
            } catch(Gs2.Core.Exception.NotFoundException e) {
                if (e.errors[0].component == "gathering")
                {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this._namespaceName?.ToString() ?? null,
                    "Singleton",
                    "Gathering"
                );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                        request.GatheringName.ToString()
                    );
                    _cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
                }
                else
                {
                    throw e;
                }
            }
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;

            {
                var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this._namespaceName?.ToString() ?? null,
                    "Singleton",
                    "Gathering"
                );
                var key = Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain.CreateCacheKey(
                    resultModel.Item.Name.ToString()
                );
                cache.Delete<Gs2.Gs2Matchmaking.Model.Gathering>(parentKey, key);
            }
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                request.NamespaceName,
                this._userId,
                result?.Item?.Name
            );

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        #endif
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
            #else
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResult(
            #endif
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
        #endif
            PutResultRequest request
        ) {

        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> self)
            {
        #endif
            request
                .WithNamespaceName(this._namespaceName);
            #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            var future = this._client.PutResultFuture(
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
            var result = await this._client.PutResultAsync(
                request
            );
            #endif
            var requestModel = request;
            var resultModel = result;
            var cache = _cache;
            {
                foreach (var item in resultModel.Items) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this._namespaceName?.ToString() ?? null,
                        this._userId?.ToString() ?? null,
                        "Rating"
                    );
                    var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
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
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[result?.Items.Length ?? 0];
            for (int i=0; i<result?.Items.Length; i++)
            {
                domain[i] = new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                    this._cache,
                    this._jobQueueDomain,
                    this._stampSheetConfiguration,
                    this._session,
                    request.NamespaceName,
                    result.Items[i]?.UserId,
                    result.Items[i]?.Name
                );
                var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName?.ToString() ?? null,
                this._userId?.ToString() ?? null,
                "Rating"
            );
                var key = Gs2.Gs2Matchmaking.Domain.Model.RatingDomain.CreateCacheKey(
                    result.Items[i].Name.ToString()
                );
                cache.Put(
                    parentKey,
                    key,
                    result.Items[i],
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            self.OnComplete(domain);
            yield return null;
        #else
            return domain;
        #endif
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]>(Impl);
        #endif
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> Gatherings(
        )
        {
            return new DescribeGatheringsIterator(
                this._cache,
                this._client,
                this._namespaceName
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> GatheringsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> Gatherings(
            #endif
        #else
        public DescribeGatheringsIterator Gatherings(
        #endif
        )
        {
            return new DescribeGatheringsIterator(
                this._cache,
                this._client,
                this._namespaceName
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
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayer(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._cache,
                this._client,
                this._namespaceName,
                player
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayerAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayer(
            #endif
        #else
        public DoMatchmakingByPlayerIterator DoMatchmakingByPlayer(
        #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._cache,
                this._client,
                this._namespaceName,
                player
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
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId,
                player
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            #endif
        #else
        public DoMatchmakingByUserIdIterator DoMatchmaking(
        #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId,
                player
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

        public Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain Gathering(
            string gatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._userId,
                gatheringName
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.BallotDomain Ballot(
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.BallotDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._userId,
                ratingName,
                gatheringName,
                numberOfPlayer,
                keyId
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
        )
        {
            return new DescribeRatingsByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Rating> RatingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
            #endif
        #else
        public DescribeRatingsByUserIdIterator Ratings(
        #endif
        )
        {
            return new DescribeRatingsByUserIdIterator(
                this._cache,
                this._client,
                this._namespaceName,
                this._userId
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

        public Gs2.Gs2Matchmaking.Domain.Model.RatingDomain Rating(
            string ratingName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                this._cache,
                this._jobQueueDomain,
                this._stampSheetConfiguration,
                this._session,
                this._namespaceName,
                this._userId,
                ratingName
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
                "matchmaking",
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
