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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain> PutGlobalRankingScoreFuture(
            PutGlobalRankingScoreRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PutGlobalRankingScoreFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.RankingName,
                    result?.Item?.Season
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain> PutGlobalRankingScoreAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain> PutGlobalRankingScoreAsync(
            #endif
            PutGlobalRankingScoreRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PutGlobalRankingScoreAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.RankingName,
                result?.Item?.Season
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain> PutClusterRankingScoreFuture(
            PutClusterRankingScoreRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PutClusterRankingScoreFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.RankingName,
                    result?.Item?.ClusterName,
                    result?.Item?.Season
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain> PutClusterRankingScoreAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain> PutClusterRankingScoreAsync(
            #endif
            PutClusterRankingScoreRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PutClusterRankingScoreAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.RankingName,
                result?.Item?.ClusterName,
                result?.Item?.Season
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingScore> GlobalRankingScores(
            string rankingName = null
        )
        {
            return new DescribeGlobalRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingScore> GlobalRankingScoresAsync(
            #else
        public DescribeGlobalRankingScoresIterator GlobalRankingScoresAsync(
            #endif
            string rankingName = null
        )
        {
            return new DescribeGlobalRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankingScores(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingScore[]> callback,
            string rankingName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeGlobalRankingScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingScore[]> callback,
            string rankingName = null
        )
        {
            var items = await GlobalRankingScoresAsync(
                rankingName
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankingScores(
                callback,
                rankingName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankingScores(
            ulong callbackId,
            string rankingName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain GlobalRankingScore(
            string rankingName,
            long? season
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                season
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingSeasonAccessTokenDomain SubscribeRankingSeason(
            string rankingName,
            long? season
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingSeasonAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                season
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.SubscribeUser> Subscribes(
            string rankingName = null
        )
        {
            return new DescribeSubscribesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.SubscribeUser> SubscribesAsync(
            #else
        public DescribeSubscribesIterator SubscribesAsync(
            #endif
            string rankingName = null
        )
        {
            return new DescribeSubscribesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribes(
            Action<Gs2.Gs2Ranking2.Model.SubscribeUser[]> callback,
            string rankingName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribesWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.SubscribeUser[]> callback,
            string rankingName = null
        )
        {
            var items = await SubscribesAsync(
                rankingName
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribes(
                callback,
                rankingName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribes(
            ulong callbackId,
            string rankingName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.SubscribeAccessTokenDomain Subscribe(
            string rankingName
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.SubscribeAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GlobalRankingReceivedRewards(
            string rankingName = null,
            long? season = null
        )
        {
            return new DescribeGlobalRankingReceivedRewardsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                season
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GlobalRankingReceivedRewardsAsync(
            #else
        public DescribeGlobalRankingReceivedRewardsIterator GlobalRankingReceivedRewardsAsync(
            #endif
            string rankingName = null,
            long? season = null
        )
        {
            return new DescribeGlobalRankingReceivedRewardsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                season
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankingReceivedRewards(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward[]> callback,
            string rankingName = null,
            long? season = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeGlobalRankingReceivedRewardsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward[]> callback,
            string rankingName = null,
            long? season = null
        )
        {
            var items = await GlobalRankingReceivedRewardsAsync(
                rankingName,
                season
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankingReceivedRewards(
                callback,
                rankingName,
                season
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankingReceivedRewards(
            ulong callbackId,
            string rankingName = null,
            long? season = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain GlobalRankingReceivedReward(
            string rankingName,
            long? season
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                season
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward> ClusterRankingReceivedRewards(
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return new DescribeClusterRankingReceivedRewardsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward> ClusterRankingReceivedRewardsAsync(
            #else
        public DescribeClusterRankingReceivedRewardsIterator ClusterRankingReceivedRewardsAsync(
            #endif
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return new DescribeClusterRankingReceivedRewardsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeClusterRankingReceivedRewards(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward[]> callback,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeClusterRankingReceivedRewardsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward[]> callback,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            var items = await ClusterRankingReceivedRewardsAsync(
                rankingName,
                clusterName,
                season
            ).ToArrayAsync();
            var callbackId = SubscribeClusterRankingReceivedRewards(
                callback,
                rankingName,
                clusterName,
                season
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeClusterRankingReceivedRewards(
            ulong callbackId,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingReceivedRewardAccessTokenDomain ClusterRankingReceivedReward(
            string rankingName,
            string clusterName,
            long? season
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingReceivedRewardAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.ClusterRankingScore> ClusterRankingScores(
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return new DescribeClusterRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.ClusterRankingScore> ClusterRankingScoresAsync(
            #else
        public DescribeClusterRankingScoresIterator ClusterRankingScoresAsync(
            #endif
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return new DescribeClusterRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeClusterRankingScores(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingScore[]> callback,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName,
                    clusterName,
                    season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeClusterRankingScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.ClusterRankingScore[]> callback,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            var items = await ClusterRankingScoresAsync(
                rankingName,
                clusterName,
                season
            ).ToArrayAsync();
            var callbackId = SubscribeClusterRankingScores(
                callback,
                rankingName,
                clusterName,
                season
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeClusterRankingScores(
            ulong callbackId,
            string rankingName = null,
            string clusterName = null,
            long? season = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    rankingName,
                    clusterName,
                    season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain ClusterRankingScore(
            string rankingName,
            string clusterName,
            long? season
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                rankingName,
                clusterName,
                season
            );
        }

    }
}
