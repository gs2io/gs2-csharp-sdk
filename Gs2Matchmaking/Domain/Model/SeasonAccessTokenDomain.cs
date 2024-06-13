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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Model.Cache;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class SeasonAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string SeasonName { get; } = null!;
        public long? Season { get; } = null!;
        public string NextPageToken { get; set; } = null!;
        public string MatchmakingContextToken { get; set; } = null!;

        public SeasonAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string seasonName,
            long? season
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.SeasonName = seasonName;
            this.Season = season;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.SeasonGathering> DoSeasonMatchmaking(
        )
        {
            return new DoSeasonMatchmakingIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.SeasonName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.SeasonGathering> DoSeasonMatchmakingAsync(
            #else
        public DoSeasonMatchmakingIterator DoSeasonMatchmakingAsync(
            #endif
        )
        {
            return new DoSeasonMatchmakingIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.SeasonName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDoSeasonMatchmaking(
            Action<Gs2.Gs2Matchmaking.Model.SeasonGathering[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDoSeasonMatchmakingWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.SeasonGathering[]> callback
        )
        {
            var items = await DoSeasonMatchmakingAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeDoSeasonMatchmaking(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDoSeasonMatchmaking(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringAccessTokenDomain SeasonGathering(
            long? tier,
            string seasonGatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.SeasonName,
                this.Season,
                tier,
                seasonGatheringName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering> JoinedSeasonGatherings(
        )
        {
            return new DescribeJoinedSeasonGatheringsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.SeasonName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering> JoinedSeasonGatheringsAsync(
            #else
        public DescribeJoinedSeasonGatheringsIterator JoinedSeasonGatheringsAsync(
            #endif
        )
        {
            return new DescribeJoinedSeasonGatheringsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                this.SeasonName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeJoinedSeasonGatherings(
            Action<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering>(
                (null as Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeJoinedSeasonGatheringsWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering[]> callback
        )
        {
            var items = await JoinedSeasonGatheringsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeJoinedSeasonGatherings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeJoinedSeasonGatherings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering>(
                (null as Gs2.Gs2Matchmaking.Model.JoinedSeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.JoinedSeasonGatheringAccessTokenDomain JoinedSeasonGathering(
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.JoinedSeasonGatheringAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.SeasonName,
                this.Season
            );
        }

    }
}
