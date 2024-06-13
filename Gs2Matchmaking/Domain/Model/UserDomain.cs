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

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;
        public string MatchmakingContextToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> Gatherings(
        )
        {
            return new DescribeGatheringsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> GatheringsAsync(
            #else
        public DescribeGatheringsIterator GatheringsAsync(
            #endif
        )
        {
            return new DescribeGatheringsIterator(
                this._gs2,
                this._client,
                this.NamespaceName
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeGatherings(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeGatheringsWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback
        )
        {
            var items = await GatheringsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeGatherings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeGatherings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayer(
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                player
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingByPlayerAsync(
            #else
        public DoMatchmakingByPlayerIterator DoMatchmakingByPlayerAsync(
            #endif
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return new DoMatchmakingByPlayerIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                player
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeDoMatchmakingByPlayer(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeDoMatchmakingByPlayerWithInitialCallAsync(
            Action<Gs2.Gs2Matchmaking.Model.Gathering[]> callback,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            var items = await DoMatchmakingByPlayerAsync(
                player
            ).ToArrayAsync();
            var callbackId = SubscribeDoMatchmakingByPlayer(
                callback,
                player
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeDoMatchmakingByPlayer(
            ulong callbackId,
            Gs2.Gs2Matchmaking.Model.Player player
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Matchmaking.Model.Gathering>(
                (null as Gs2.Gs2Matchmaking.Model.Gathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmaking(
            Gs2.Gs2Matchmaking.Model.Player player,
            string timeOffsetToken = null
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                player,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Gathering> DoMatchmakingAsync(
            #else
        public DoMatchmakingByUserIdIterator DoMatchmakingAsync(
            #endif
            Gs2.Gs2Matchmaking.Model.Player player,
            string timeOffsetToken = null
        )
        {
            return new DoMatchmakingByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                player,
                timeOffsetToken
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
                    this.UserId
                ),
                callback
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
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain Gathering(
            string gatheringName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                gatheringName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Matchmaking.Model.Rating> Ratings(
            string timeOffsetToken = null
        )
        {
            return new DescribeRatingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.Rating> RatingsAsync(
            #else
        public DescribeRatingsByUserIdIterator RatingsAsync(
            #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeRatingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                timeOffsetToken
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
                    this.UserId
                ),
                callback
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
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.RatingDomain Rating(
            string ratingName
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                ratingName
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.BallotDomain Ballot(
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.BallotDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                ratingName,
                gatheringName,
                numberOfPlayer,
                keyId
            );
        }

        public Gs2.Gs2Matchmaking.Domain.Model.SeasonDomain Season(
            string seasonName,
            long? season
        ) {
            return new Gs2.Gs2Matchmaking.Domain.Model.SeasonDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                seasonName,
                season
            );
        }

    }

    public partial class UserDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringFuture(
            CreateGatheringByUserIdRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithUserId(this.UserId);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.CreateGatheringByUserIdFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    this.NamespaceName,
                    request.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> CreateGatheringAsync(
            #endif
            CreateGatheringByUserIdRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.CreateGatheringByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._gs2,
                this.NamespaceName,
                request.UserId,
                result?.Item?.Name
            );

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringFuture(
            DeleteGatheringRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteGatheringFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    if (!(future.Error is NotFoundException)) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                var result = future.Result;
                var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                    this._gs2,
                    this.NamespaceName,
                    this.UserId,
                    result?.Item?.Name
                );

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain> DeleteGatheringAsync(
            #endif
            DeleteGatheringRequest request
        ) {
            try {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.DeleteGatheringAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = new Gs2.Gs2Matchmaking.Domain.Model.GatheringDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                request.GatheringName
            );
            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultFuture(
            PutResultRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> self)
            {
                request = request
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PutResultFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = result?.Items?.Select(v => new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                    this._gs2,
                    this.NamespaceName,
                    v?.UserId,
                    v?.Name
                )).ToArray() ?? Array.Empty<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain>();
                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
            #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain[]> PutResultAsync(
            #endif
            PutResultRequest request
        ) {
            request = request
                .WithContextStack(this._gs2.DefaultContextStack)
                .WithNamespaceName(this.NamespaceName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PutResultAsync(request)
            );
            var domain = result?.Items?.Select(v => new Gs2.Gs2Matchmaking.Domain.Model.RatingDomain(
                this._gs2,
                this.NamespaceName,
                v?.UserId,
                v?.Name
            )).ToArray() ?? Array.Empty<Gs2.Gs2Matchmaking.Domain.Model.RatingDomain>();
            return domain;
        }
        #endif

    }

    public partial class UserDomain {

    }
}
