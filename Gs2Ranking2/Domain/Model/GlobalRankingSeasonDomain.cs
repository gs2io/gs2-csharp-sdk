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

    public partial class GlobalRankingSeasonDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public string RankingName { get; } = null!;
        public long? Season { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public GlobalRankingSeasonDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rankingName,
            long? season,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RankingName = rankingName;
            this.Season = season;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingScore> GlobalRankingScores(
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingScoresByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingScore> GlobalRankingScoresAsync(
            #else
        public DescribeGlobalRankingScoresByUserIdIterator GlobalRankingScoresAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingScoresByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankingScores(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingScore[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await GlobalRankingScoresAsync(
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
        public async UniTask<ulong> SubscribeGlobalRankingScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingScore[]> callback
        )
        {
            var items = await GlobalRankingScoresAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankingScores(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankingScores(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                ),
                callbackId
            );
        }

        public void InvalidateGlobalRankingScores(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.GlobalRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain GlobalRankingScore(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                this.UserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingData> GlobalRankings(
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingData> GlobalRankingsAsync(
            #else
        public DescribeGlobalRankingsByUserIdIterator GlobalRankingsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankings(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingData[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingData>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.Season ?? default
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await GlobalRankingsAsync(
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
        public async UniTask<ulong> SubscribeGlobalRankingsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingData[]> callback
        )
        {
            var items = await GlobalRankingsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingData>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.Season ?? default
                ),
                callbackId
            );
        }

        public void InvalidateGlobalRankings(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.GlobalRankingData>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.Season ?? default
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataDomain GlobalRankingData(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                this.UserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GlobalRankingReceivedRewards(
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingReceivedRewardsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GlobalRankingReceivedRewardsAsync(
            #else
        public DescribeGlobalRankingReceivedRewardsByUserIdIterator GlobalRankingReceivedRewardsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingReceivedRewardsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankingReceivedRewards(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await GlobalRankingReceivedRewardsAsync(
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
        public async UniTask<ulong> SubscribeGlobalRankingReceivedRewardsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward[]> callback
        )
        {
            var items = await GlobalRankingReceivedRewardsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankingReceivedRewards(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankingReceivedRewards(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                ),
                callbackId
            );
        }

        public void InvalidateGlobalRankingReceivedRewards(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName
                )
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardDomain GlobalRankingReceivedReward(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                this.UserId
            );
        }

    }

    public partial class GlobalRankingSeasonDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain> PutGlobalRankingScoreFuture(
            PutGlobalRankingScoreByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRankingName(this.RankingName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PutGlobalRankingScoreByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain(
                    this._gs2,
                    this.NamespaceName,
                    result?.Item?.RankingName,
                    result?.Item?.Season,
                    result?.Item?.UserId
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain> PutGlobalRankingScoreAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain> PutGlobalRankingScoreAsync(
            #endif
            PutGlobalRankingScoreByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRankingName(this.RankingName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PutGlobalRankingScoreByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.RankingName,
                result?.Item?.Season,
                result?.Item?.UserId
            );

            return domain;
        }
        #endif

    }

    public partial class GlobalRankingSeasonDomain {

    }
}
