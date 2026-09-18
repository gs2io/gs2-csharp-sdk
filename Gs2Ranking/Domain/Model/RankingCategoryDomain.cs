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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Ranking.Domain.Iterator;
using Gs2.Gs2Ranking.Model.Cache;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
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

namespace Gs2.Gs2Ranking.Domain.Model
{

    public partial class RankingCategoryDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2RankingRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string CategoryName { get; } = null!;
        public string AdditionalScopeName { get; } = null!;
        public string NextPageToken { get; set; } = null!;
        public bool? Processing { get; set; } = null!;

        public RankingCategoryDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2RankingRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.CategoryName = categoryName;
            this.AdditionalScopeName = additionalScopeName;
        }

        public Gs2.Gs2Ranking.Domain.Model.SubscribeDomain Subscribe(
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.AdditionalScopeName
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryName(
            string timeOffsetToken = null
        )
        {
            return new DescribeSubscribesByCategoryNameAndUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryNameAsync(
        #else
        public DescribeSubscribesByCategoryNameAndUserIdIterator SubscribesByCategoryNameAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeSubscribesByCategoryNameAndUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                this.UserId,
                timeOffsetToken
            );
        }

        public ulong SubscribeSubscribesByCategoryName(
            Action<Gs2.Gs2Ranking.Model.SubscribeUser[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await SubscribesByCategoryNameAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeSubscribesByCategoryNameWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeSubscribesByCategoryNameWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Ranking.Model.SubscribeUser[]> callback
        )
        {
            var items = await SubscribesByCategoryNameAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeSubscribesByCategoryName(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeSubscribesByCategoryName(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateSubscribesByCategoryName(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking.Model.SubscribeUser>(
                (null as Gs2.Gs2Ranking.Model.SubscribeUser).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                )
            );
        }

        public Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain SubscribeUser(
            string targetUserId
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.AdditionalScopeName,
                targetUserId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            string timeOffsetToken = null
        )
        {
            return new DescribeRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                this.UserId,
                this.AdditionalScopeName,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> RankingsAsync(
        #else
        public DescribeRankingsByUserIdIterator RankingsAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeRankingsByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                this.UserId,
                this.AdditionalScopeName,
                timeOffsetToken
            );
        }

        public ulong SubscribeRankings(
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
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
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await RankingsAsync(
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeRankingsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeRankingsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback
        )
        {
            var items = await RankingsAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeRankings(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeRankings(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateRankings(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                )
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> NearRankings(
            long? score
        )
        {
            return new DescribeNearRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                score,
                this.AdditionalScopeName
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> NearRankingsAsync(
        #else
        public DescribeNearRankingsIterator NearRankingsAsync(
        #endif
            long? score
        )
        {
            return new DescribeNearRankingsIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.CategoryName,
                score,
                this.AdditionalScopeName
            );
        }

        public ulong SubscribeNearRankings(
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback,
            long? score
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                ),
                items => callback.Invoke(items
                    .Where(item => score == null || item.Score == score)
                    .ToArray()),
                () =>
                {
        #if GS2_ENABLE_UNITASK
                    async UniTask Impl() {
        #else
                    async Task Impl() {
        #endif
                        try {
        #if GS2_ENABLE_UNITASK
                            await UniTask.SwitchToMainThread();
        #endif
                            callback.Invoke(await NearRankingsAsync(
                                score
                            ).ToArrayAsync());
                        }
                        catch (System.Exception) {
                            // ignored
                        }
                    }
                    Impl().Forget();
                }
            );
        }

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeNearRankingsWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeNearRankingsWithInitialCallAsync(
        #endif
            Action<Gs2.Gs2Ranking.Model.Ranking[]> callback,
            long? score
        )
        {
            var items = await NearRankingsAsync(
                score
            ).ToArrayAsync();
            var callbackId = SubscribeNearRankings(
                callback,
                score
            );
            callback.Invoke(items);
            return callbackId;
        }

        public void UnsubscribeNearRankings(
            ulong callbackId,
            long? score
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateNearRankings(
            long? score
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Ranking.Model.Ranking>(
                (null as Gs2.Gs2Ranking.Model.Ranking).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.CategoryName,
                    this.AdditionalScopeName,
                    null
                )
            );
        }

        public Gs2.Gs2Ranking.Domain.Model.RankingDomain Ranking(
            string scorerUserId = null,
            long? index = null
        ) {
            return new Gs2.Gs2Ranking.Domain.Model.RankingDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.CategoryName,
                this.AdditionalScopeName,
                scorerUserId,
                index
            );
        }

    }

    public partial class RankingCategoryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeFuture(
            SubscribeByUserIdRequest request
        ) => SubscribeAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
        #endif
            SubscribeByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.SubscribeByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain(
                this._gs2,
                this.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.CategoryName,
                this.AdditionalScopeName,
                result?.Item?.TargetUserId
            );

            return domain;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScoreFuture(
            PutScoreByUserIdRequest request
        ) => PutScoreAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScoreAsync(
        #else
        public async Task<Gs2.Gs2Ranking.Domain.Model.ScoreDomain> PutScoreAsync(
        #endif
            PutScoreByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithCategoryName(this.CategoryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.PutScoreByUserIdAsync(request)
            );
            var domain = new Gs2.Gs2Ranking.Domain.Model.ScoreDomain(
                this._gs2,
                request.NamespaceName,
                result?.Item?.UserId,
                result?.Item?.CategoryName,
                result?.Item?.ScorerUserId,
                result?.Item?.UniqueId
            );

            return domain;
        }

    }

    public partial class RankingCategoryDomain {

    }
}
