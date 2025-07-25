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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string NextPageToken { get; set; } = null!;
        public string MatchmakingContextToken { get; set; } = null!;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> CreateGatheringFuture(
            CreateGatheringRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this._client.CreateGatheringFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.AccessToken,
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
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.CreateGatheringAsync(request)
            );
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                result?.Item?.Name
            );

            return domain;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                player
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingAsync(
            #else
        public DoMatchmakingIterator DoMatchmakingAsync(
            #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken,
                player
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDoMatchmaking(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await DoMatchmakingAsync(
                                player
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
        public async UniTask<ulong> SubscribeDoMatchmakingWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            var items = await DoMatchmakingAsync(
                player
            ).ToArrayAsync();
            var callbackId = SubscribeDoMatchmaking(
                callback,
                player
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDoMatchmaking(
            ulong callbackId,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateDoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain Gathering(
            string gatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.GatheringAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                gatheringName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
        )
        {
            return new DescribeRatingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Rating> RatingsAsync(
            #else
        public DescribeRatingsIterator RatingsAsync(
            #endif
        )
        {
            return new DescribeRatingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeRatings(
            Action<Gs2.Gs2Matchmaking.Model.Rating[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                (null as Gs2.Gs2Matchmaking.Model.Rating).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callback,
                () =>
                {
        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
                    async UniTask Impl() {
                        try {
                            await UniTask.SwitchToMainThread();
                            callback.Invoke(await RatingsAsync(
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
        public async UniTask<ulong> SubscribeRatingsWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.Rating[]> callback
        )
        {
            var items = await RatingsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeRatings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeRatings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Rating>(
                (null as Gs2.Gs2Matchmaking.Model.Rating).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
                callbackId
            );
        }

        public void InvalidateRatings(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Matchmaking.Model.Rating>(
                (null as Gs2.Gs2Matchmaking.Model.Rating).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                )
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.RatingAccessTokenDomain Rating(
            string ratingName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.RatingAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                ratingName
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
                this.AccessToken,
                ratingName,
                gatheringName,
                numberOfPlayer,
                keyId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.SeasonAccessTokenDomain Season(
            string seasonName,
            long? season
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.SeasonAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                seasonName,
                season
            );
        }

    }
}
