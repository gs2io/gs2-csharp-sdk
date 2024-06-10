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

    public partial class SubscribeRankingSeasonAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string RankingName { get; } = null!;
        public long? Season { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public SubscribeRankingSeasonAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string rankingName,
            long? season
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.RankingName = rankingName;
            this.Season = season;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> PutSubscribeRankingScoreFuture(
            PutSubscribeRankingScoreRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithRankingName(this.RankingName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PutSubscribeRankingScoreFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
                    result?.Item?.RankingName,
                    result?.Item?.Season
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> PutSubscribeRankingScoreAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> PutSubscribeRankingScoreAsync(
            #endif
            PutSubscribeRankingScoreRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithRankingName(this.RankingName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PutSubscribeRankingScoreAsync(request)
            );
            var domain = new Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain(
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
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> SubscribeRankingScores(
        )
        {
            return new DescribeSubscribeRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> SubscribeRankingScoresAsync(
            #else
        public DescribeSubscribeRankingScoresIterator SubscribeRankingScoresAsync(
            #endif
        )
        {
            return new DescribeSubscribeRankingScoresIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribeRankingScores(
            Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.SubscribeRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribeRankingScoresWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore[]> callback
        )
        {
            var items = await SubscribeRankingScoresAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribeRankingScores(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribeRankingScores(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.SubscribeRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain SubscribeRankingScore(
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName,
                this.Season
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.SubscribeRankingData> SubscribeRankings(
        )
        {
            return new DescribeSubscribeRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName,
                this.Season
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.SubscribeRankingData> SubscribeRankingsAsync(
            #else
        public DescribeSubscribeRankingsIterator SubscribeRankingsAsync(
            #endif
        )
        {
            return new DescribeSubscribeRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName,
                this.Season
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeSubscribeRankings(
            Action<Gs2.Gs2Ranking2.Model.SubscribeRankingData[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.SubscribeRankingData>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribeRankingsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.SubscribeRankingData[]> callback
        )
        {
            var items = await SubscribeRankingsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribeRankings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeSubscribeRankings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.SubscribeRankingData>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingDataAccessTokenDomain SubscribeRankingData(
            string scorerUserId
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingDataAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.RankingName,
                this.Season,
                scorerUserId ?? this.AccessToken.UserId
            );
        }

    }
}
