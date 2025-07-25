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
using Gs2.Gs2Ranking2.Domain.Iterator;
using Gs2.Gs2Ranking2.Model.Cache;
using Gs2.Gs2Ranking2.Request;
using Gs2.Gs2Ranking2.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Ranking2.Domain.Model
{

    public partial class ClusterRankingSeasonDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public string RankingName { get; } = null!;
        public string ClusterName { get; } = null!;
        public long? Season { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public ClusterRankingSeasonDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RankingName = rankingName;
            this.ClusterName = clusterName;
            this.Season = season;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.ClusterRankingData> ClusterRankings(
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.ClusterRankingData> ClusterRankingsAsync(
            #else
        public DescribeClusterRankingsByUserIdIterator ClusterRankingsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeClusterRankings(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingData[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingData>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.ClusterName,
                    this.Season,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await ClusterRankingsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeClusterRankingsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingData[]> callback
        )
        {
            var items = await ClusterRankingsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeClusterRankings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeClusterRankings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingData>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.ClusterName,
                    this.Season,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateClusterRankings(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.ClusterRankingData>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.ClusterName,
                    this.Season,
                    null
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain ClusterRankingData(
            string scorerUserId
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.ClusterName,
                this.Season,
                this.UserId,
                scorerUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward> ClusterRankingReceivedRewards(
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingReceivedRewardsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward> ClusterRankingReceivedRewardsAsync(
            #else
        public DescribeClusterRankingReceivedRewardsByUserIdIterator ClusterRankingReceivedRewardsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingReceivedRewardsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeClusterRankingReceivedRewards(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await ClusterRankingReceivedRewardsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeClusterRankingReceivedRewardsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward[]> callback
        )
        {
            var items = await ClusterRankingReceivedRewardsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeClusterRankingReceivedRewards(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeClusterRankingReceivedRewards(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateClusterRankingReceivedRewards(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingReceivedRewardDomain ClusterRankingReceivedReward(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingReceivedRewardDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.ClusterName,
                this.Season,
                this.UserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.ClusterRankingScore> ClusterRankingScores(
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingScoresByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.ClusterRankingScore> ClusterRankingScoresAsync(
            #else
        public DescribeClusterRankingScoresByUserIdIterator ClusterRankingScoresAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeClusterRankingScoresByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.ClusterName,
                this.Season,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeClusterRankingScores(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingScore[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await ClusterRankingScoresAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
        #endif
                }
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeClusterRankingScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingScore[]> callback
        )
        {
            var items = await ClusterRankingScoresAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeClusterRankingScores(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeClusterRankingScores(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateClusterRankingScores(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.ClusterRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    null
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain ClusterRankingScore(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.ClusterName,
                this.Season,
                this.UserId
            );
        }

    }

    public partial class ClusterRankingSeasonDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain> GetClusterRankingFuture(
            GetClusterRankingByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRankingName(this.RankingName)
                    .WithClusterName(this.ClusterName)
                    .WithUserId(this.UserId)
                    .WithSeason(this.Season);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.GetClusterRankingByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.RankingName,
                    result?.Item?.ClusterName,
                    result?.Item?.Season,
                    result?.Item?.UserId,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain> GetClusterRankingAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain> GetClusterRankingAsync(
            #endif
            GetClusterRankingByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRankingName(this.RankingName)
                .WithClusterName(this.ClusterName)
                .WithUserId(this.UserId)
                .WithSeason(this.Season);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetClusterRankingByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingDataDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.RankingName,
                result?.Item?.ClusterName,
                result?.Item?.Season,
                result?.Item?.UserId,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain> PutClusterRankingScoreFuture(
            PutClusterRankingScoreByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRankingName(this.RankingName)
                    .WithClusterName(this.ClusterName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.PutClusterRankingScoreByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.RankingName,
                    result?.Item?.ClusterName,
                    result?.Item?.Season,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain> PutClusterRankingScoreAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain> PutClusterRankingScoreAsync(
            #endif
            PutClusterRankingScoreByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRankingName(this.RankingName)
                .WithClusterName(this.ClusterName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.PutClusterRankingScoreByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.RankingName,
                result?.Item?.ClusterName,
                result?.Item?.Season,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

    }

    public partial class ClusterRankingSeasonDomain {

    }
}
