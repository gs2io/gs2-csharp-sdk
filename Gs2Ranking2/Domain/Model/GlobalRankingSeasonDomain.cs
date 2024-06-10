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

    public partial class GlobalRankingSeasonDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; }
        public string RankingName { get; }
        public long? Season { get; }
        public string NextPageToken { get; set; }

        public GlobalRankingSeasonDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rankingName,
            long? season
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RankingName = rankingName;
            this.Season = season;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingData> GlobalRankings(
            string userId,
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
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
            string userId,
            string timeOffsetToken = null
        )
        {
            return new DescribeGlobalRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                userId,
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

        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking2.Model.GlobalRankingData> GlobalRankings(
            AccessToken accessToken
        )
        {
            return new DescribeGlobalRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                accessToken,
                this.RankingName,
                this.Season
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking2.Model.GlobalRankingData> GlobalRankingsAsync(
            #else
        public DescribeGlobalRankingsIterator GlobalRankingsAsync(
            #endif
            AccessToken accessToken
        )
        {
            return new DescribeGlobalRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                accessToken,
                this.RankingName,
                this.Season
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGlobalRankings(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingData[]> callback,
            string userId
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingData>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.Season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeGlobalRankingsWithInitialCallAsync(
            Action<Gs2.Gs2Ranking2.Model.GlobalRankingData[]> callback,
            string userId
        )
        {
            var items = await GlobalRankingsAsync(
                userId
            ).ToArrayAsync();
            var callbackId = SubscribeGlobalRankings(
                callback,
                userId
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGlobalRankings(
            ulong callbackId,
            string userId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingData>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingData).CacheParentKey(
                    this.NamespaceName,
                    this.RankingName,
                    this.Season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataDomain GlobalRankingData(
            string userId
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                userId
            );
        }

        public Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataAccessTokenDomain GlobalRankingData(
             AccessToken accessToken
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.GlobalRankingDataAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                accessToken
            );
        }

    }

    public partial class GlobalRankingSeasonDomain {

    }

    public partial class GlobalRankingSeasonDomain {

    }
}
