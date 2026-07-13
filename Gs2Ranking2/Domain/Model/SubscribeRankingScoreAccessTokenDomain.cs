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
using System.Collections.Generic;
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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Ranking2.Domain.Model
{

    public partial class SubscribeRankingScoreAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public string RankingName { get; } = null!;
        public long? Season { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;

        public SubscribeRankingScoreAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rankingName,
            long? season,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RankingName = rankingName;
            this.Season = season;
            this.AccessToken = accessToken;
        }

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> GetFuture(
            GetSubscribeRankingScoreRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> GetAsync(
        #else
        private async Task<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> GetAsync(
        #endif
            GetSubscribeRankingScoreRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRankingName(this.RankingName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithSeason(this.Season);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.GetSubscribeRankingScoreAsync(request)
            );
            return result?.Item;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> VerifyFuture(
            VerifySubscribeRankingScoreRequest request
        ) => VerifyAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> VerifyAsync(
        #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.SubscribeRankingScoreAccessTokenDomain> VerifyAsync(
        #endif
            VerifySubscribeRankingScoreRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithRankingName(this.RankingName)
                .WithSeason(this.Season);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                this.AccessToken?.TimeOffset,
                () => this._client.VerifySubscribeRankingScoreAsync(request)
            );
            var domain = this;

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> ModelAsync()
        #else
        public async Task<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Ranking2.Model.SubscribeRankingScore>(
                        (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheParentKey(
                            this.NamespaceName,
                            this.UserId,
                            this.RankingName,
                            this.AccessToken?.TimeOffset
                        ),
                        (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheKey(
                            this.RankingName,
                            this.Season,
                            this.UserId
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RankingName,
                    this.Season,
                    this.UserId,
                    this.AccessToken?.TimeOffset
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RankingName,
                    this.Season,
                    this.UserId,
                    this.AccessToken?.TimeOffset,
                    () => this.GetAsync(
                        new GetSubscribeRankingScoreRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public UniTask<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> Model() => ModelAsync();
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> Model() => ModelFuture();
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public Task<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> Model() => ModelAsync();
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.RankingName,
                this.Season,
                this.UserId,
                this.AccessToken?.TimeOffset
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheKey(
                    this.RankingName,
                    this.Season,
                    this.UserId
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking2.Model.SubscribeRankingScore>(
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.AccessToken?.TimeOffset
                ),
                (null as Gs2.Gs2Ranking2.Model.SubscribeRankingScore).CacheKey(
                    this.RankingName,
                    this.Season,
                    this.UserId
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.SubscribeRankingScore> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
