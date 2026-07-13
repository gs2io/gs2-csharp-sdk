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
using System.Collections;
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
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
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

    public partial class SeasonGatheringDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string SeasonName { get; } = null!;
        public long? Season { get; } = null!;
        public long? Tier { get; } = null!;
        public string SeasonGatheringName { get; } = null!;

        public SeasonGatheringDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2MatchmakingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.SeasonName = seasonName;
            this.Season = season;
            this.Tier = tier;
            this.SeasonGatheringName = seasonGatheringName;
        }

    }

    public partial class SeasonGatheringDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Matchmaking.Model.SeasonGathering> GetFuture(
            GetSeasonGatheringRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Matchmaking.Model.SeasonGathering> GetAsync(
        #else
        private async Task<Gs2.Gs2Matchmaking.Model.SeasonGathering> GetAsync(
        #endif
            GetSeasonGatheringRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithSeasonName(this.SeasonName)
                .WithSeason(this.Season)
                .WithTier(this.Tier)
                .WithSeasonGatheringName(this.SeasonGatheringName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.GetSeasonGatheringAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> VerifyIncludeParticipantFuture(
            VerifyIncludeParticipantByUserIdRequest request
        ) => VerifyIncludeParticipantAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> VerifyIncludeParticipantAsync(
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> VerifyIncludeParticipantAsync(
        #endif
            VerifyIncludeParticipantByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithUserId(this.UserId) /* diff +++ */
                .WithSeasonName(this.SeasonName)
                .WithSeason(this.Season)
                .WithTier(this.Tier)
/* diff --- start
                .WithSeasonGatheringName(this.SeasonGatheringName)
                .WithUserId(this.UserId);
 diff --- end */
                .WithSeasonGatheringName(this.SeasonGatheringName); /* diff +++ */
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.VerifyIncludeParticipantByUserIdAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> DeleteFuture(
            DeleteSeasonGatheringRequest request
        ) => DeleteAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> DeleteAsync(
        #else
        public async Task<Gs2.Gs2Matchmaking.Domain.Model.SeasonGatheringDomain> DeleteAsync(
        #endif
            DeleteSeasonGatheringRequest request
        ) {
            try {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithSeasonName(this.SeasonName)
                    .WithSeason(this.Season)
                    .WithTier(this.Tier)
                    .WithSeasonGatheringName(this.SeasonGatheringName);
                var result = await request.InvokeAsync(
                    _gs2.Cache,
                    this.UserId,
                    null,
                    () => this._client.DeleteSeasonGatheringAsync(request)
                );
            }
            catch (NotFoundException e) {}
            var domain = this;
            return domain;
        }

    }

    public partial class SeasonGatheringDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Matchmaking.Model.SeasonGathering> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Matchmaking.Model.SeasonGathering> ModelAsync()
        #else
        public async Task<Gs2.Gs2Matchmaking.Model.SeasonGathering> ModelAsync()
        #endif
        {
/* diff --- start
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                        (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.SeasonName,
                            this.Season,
                            null
                        ),
                        (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheKey(
                            this.Tier,
                            this.SeasonGatheringName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season,
                    this.Tier,
                    this.SeasonGatheringName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season,
                    this.Tier,
                    this.SeasonGatheringName,
                    null,
                    () => this.GetAsync(
                        new GetSeasonGatheringRequest()
                    )
                );
 diff --- end */
/* diff +++ start */
            var (value, find) = (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.SeasonName,
                this.Season,
                this.Tier,
                this.SeasonGatheringName,
                null
            );
            if (find) {
                return value;
/* diff +++ end */
            }
/* diff +++ start */
            return await (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.SeasonName,
                this.Season,
                this.Tier,
                this.SeasonGatheringName,
                null,
                () => this.GetAsync(
                    new GetSeasonGatheringRequest()
                )
            );
/* diff +++ end */
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Matchmaking.Model.SeasonGathering> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.SeasonName,
                this.Season,
                this.Tier,
                this.SeasonGatheringName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Matchmaking.Model.SeasonGathering> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season,
                    null
                ),
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheKey(
                    this.Tier,
                    this.SeasonGatheringName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.SeasonName,
                    this.Season,
                    null
                ),
                (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheKey(
                    this.Tier,
                    this.SeasonGatheringName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Matchmaking.Model.SeasonGathering> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Matchmaking.Model.SeasonGathering> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.SeasonGathering> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Matchmaking.Model.SeasonGathering> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
