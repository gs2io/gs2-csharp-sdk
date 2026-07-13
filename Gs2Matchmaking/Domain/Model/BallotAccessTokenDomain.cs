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
using System.Collections.Generic;
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
using Gs2.Gs2Matchmaking.Model; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Matchmaking.Domain.Model
{

    public partial class BallotAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string RatingName { get; } = null!;
        public string GatheringName { get; } = null!;
        public int? NumberOfPlayer { get; } = null!;
        public string KeyId { get; } = null!;
        public string Body { get; set; } = null!;
        public string Signature { get; set; } = null!;

        public BallotAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string ratingName,
            string gatheringName,
            int? numberOfPlayer,
            string keyId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.RatingName = ratingName;
            this.GatheringName = gatheringName;
            this.NumberOfPlayer = numberOfPlayer;
            this.KeyId = keyId;
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        private IFuture<Gs2.Gs2Matchmaking.Model.Ballot> GetFuture(
 diff --- end */
        private IFuture<Gs2.Gs2Matchmaking.Model.SignedBallot> GetFuture( /* diff +++ */
            GetBallotRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        private async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> GetAsync(
 diff --- end */
        private async UniTask<Gs2.Gs2Matchmaking.Model.SignedBallot> GetAsync( /* diff +++ */
        #else
/* diff --- start
        private async Task<Gs2.Gs2Matchmaking.Model.Ballot> GetAsync(
 diff --- end */
        private async Task<Gs2.Gs2Matchmaking.Model.SignedBallot> GetAsync( /* diff +++ */
        #endif
            GetBallotRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token) /* diff +++ */
                .WithRatingName(this.RatingName)
                .WithGatheringName(this.GatheringName)
/* diff --- start
                .WithAccessToken(this.AccessToken?.Token)
 diff --- end */
                .WithNumberOfPlayer(this.NumberOfPlayer)
                .WithKeyId(this.KeyId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetBallotAsync(request)
            );
/* diff --- start
            return result?.Item;
 diff --- end */
/* diff +++ start */
            return new SignedBallot {
                Body = result.Body,
                Signature = result.Signature
            };
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> ModelFuture() => ModelAsync().ToGs2Future();
 diff --- end */
        public IFuture<Gs2.Gs2Matchmaking.Model.SignedBallot> ModelFuture() => ModelAsync().ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
 diff --- end */
        public async UniTask<Gs2.Gs2Matchmaking.Model.SignedBallot> ModelAsync() /* diff +++ */
        #else
/* diff --- start
        public async Task<Gs2.Gs2Matchmaking.Model.Ballot> ModelAsync()
 diff --- end */
        public async Task<Gs2.Gs2Matchmaking.Model.SignedBallot> ModelAsync() /* diff +++ */
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Matchmaking.Model.Ballot>(
                        (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheKey(
                            this.RatingName,
                            this.GatheringName,
                            this.NumberOfPlayer,
                            this.KeyId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Matchmaking.Model.Ballot).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RatingName,
                    this.GatheringName,
                    this.NumberOfPlayer,
                    this.KeyId,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Matchmaking.Model.Ballot).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RatingName,
                    this.GatheringName,
                    this.NumberOfPlayer,
                    this.KeyId,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetBallotRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Matchmaking.Model.SignedBallot).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RatingName,
                this.GatheringName,
                this.AccessToken?.TimeOffset
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Matchmaking.Model.SignedBallot).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RatingName,
                this.GatheringName,
                this.AccessToken?.TimeOffset,
                () => this.GetAsync(
                    new GetBallotRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Matchmaking.Model.Ballot> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Matchmaking.Model.SignedBallot> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Matchmaking.Model.Ballot> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Matchmaking.Model.SignedBallot> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Matchmaking.Model.Ballot> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Matchmaking.Model.SignedBallot> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
/* diff --- start
            (null as Gs2.Gs2Matchmaking.Model.Ballot).DeleteCache(
 diff --- end */
            (null as Gs2.Gs2Matchmaking.Model.SignedBallot).DeleteCache( /* diff +++ */
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RatingName,
                this.GatheringName,
/* diff --- start
                this.NumberOfPlayer,
                this.KeyId,
 diff --- end */
                this.AccessToken?.TimeOffset
            );
        }

/* diff --- start
        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
 diff --- end */
        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.SignedBallot> callback) /* diff +++ */
        {
            return this._gs2.Cache.Subscribe(
/* diff --- start
                (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheParentKey(
 diff --- end */
                (null as Gs2.Gs2Matchmaking.Model.SignedBallot).CacheParentKey( /* diff +++ */
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
/* diff --- start
                (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheKey(
 diff --- end */
                (null as Gs2.Gs2Matchmaking.Model.SignedBallot).CacheKey( /* diff +++ */
                    this.RatingName,
/* diff --- start
                    this.GatheringName,
                    this.NumberOfPlayer,
                    this.KeyId
 diff --- end */
                    this.GatheringName /* diff +++ */
                ),
                callback,
                () =>
                {
            #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
            #else
                    async Task Impl() {
            #endif
                        try {
                            await ModelAsync();
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
/* diff --- start
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.Ballot>(
                (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheParentKey(
 diff --- end */
/* diff +++ start */
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.SignedBallot>(
                (null as Gs2.Gs2Matchmaking.Model.SignedBallot).CacheParentKey(
/* diff +++ end */
                    this.NamespaceName,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                ),
/* diff --- start
                (null as Gs2.Gs2Matchmaking.Model.Ballot).CacheKey(
 diff --- end */
                (null as Gs2.Gs2Matchmaking.Model.SignedBallot).CacheKey( /* diff +++ */
                    this.RatingName,
/* diff --- start
                    this.GatheringName,
                    this.NumberOfPlayer,
                    this.KeyId
 diff --- end */
                    this.GatheringName /* diff +++ */
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Matchmaking.Model.SignedBallot> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
/* diff --- start
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
 diff --- end */
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.SignedBallot> callback) /* diff +++ */
        #else
/* diff --- start
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.Ballot> callback)
 diff --- end */
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.SignedBallot> callback) /* diff +++ */
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
