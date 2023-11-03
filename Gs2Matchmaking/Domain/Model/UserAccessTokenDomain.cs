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
using Gs2.Gs2Matchmaking.Domain.Iterator;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string NextPageToken { get; set; }
        public string MatchmakingContextToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Matchmaking.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> CreateGatheringFuture(
            CreateGatheringRequest request
        ) {

            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> self)
            {
                request
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this._accessToken?.Token);
                var future = this._client.CreateGatheringFuture(
                    request
                );
                yield return future;
                if (future.Error != null)
                {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;

                var requestModel = request;
                var resultModel = result;
                var cache = this._gs2.Cache;
                if (resultModel != null) {
                    
                    if (resultModel.Item != null) {
                        var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                            this.NamespaceName,
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
                }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> CreateGatheringAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> CreateGatheringAsync(
            #endif
            CreateGatheringRequest request
        ) {
            request
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this._accessToken?.Token);
            CreateGatheringResult result = null;
                result = await this._client.CreateGatheringAsync(
                    request
                );

            var requestModel = request;
            var resultModel = result;
            var cache = this._gs2.Cache;
            if (resultModel != null) {
                
                if (resultModel.Item != null) {
                    var parentKey = Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                        this.NamespaceName,
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
            }
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                    this._gs2,
                    request.NamespaceName,
                    this._accessToken,
                    result?.Item?.Name
                );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        [Obsolete("The name has been changed to CreateGatheringFuture.")]
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> CreateGathering(
            CreateGatheringRequest request
        ) {
            return CreateGatheringFuture(request);
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                player
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            #endif
        #else
        public DoMatchmakingIterator DoMatchmakingAsync(
        #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken,
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

        public ulong SubscribeDoMatchmaking(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callback
            );
        }

        public void UnsubscribeDoMatchmaking(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                "matchmaking",
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain Gathering(
            string gatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                gatheringName
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain Ballot(
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.BallotAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
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
            return new DescribeRatingsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Rating> RatingsAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
            #endif
        #else
        public DescribeRatingsIterator RatingsAsync(
        #endif
        )
        {
            return new DescribeRatingsIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
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

        public ulong SubscribeRatings(Action callback)
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Rating"
                ),
                callback
            );
        }

        public void UnsubscribeRatings(ulong callbackId)
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                Gs2.Gs2Matchmaking.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Rating"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.RatingAccessTokenDomain Rating(
            string ratingName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.RatingAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
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
