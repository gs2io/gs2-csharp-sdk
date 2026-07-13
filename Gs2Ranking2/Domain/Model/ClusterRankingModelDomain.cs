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

    public partial class ClusterRankingModelDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public string RankingName { get; } = null!;

        public ClusterRankingModelDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string rankingName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.RankingName = rankingName;
        }

        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingSeasonDomain ClusterRankingSeason(
            string clusterName,
            long? season,
            string userId
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingSeasonDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                clusterName,
                season,
                userId
            );
        }

/* diff +++ start */
        public Gs2.Gs2Ranking2.Domain.Model.ClusterRankingSeasonAccessTokenDomain ClusterRankingSeason(
            string clusterName,
            long? season,
            AccessToken accessToken
        ) {
            return new Gs2.Gs2Ranking2.Domain.Model.ClusterRankingSeasonAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.RankingName,
                clusterName,
                season,
                accessToken
            );
        }

/* diff +++ end */
    }

    public partial class ClusterRankingModelDomain {

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking2.Model.ClusterRankingModel> GetFuture(
            GetClusterRankingModelRequest request
        ) => GetAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        private async UniTask<Gs2.Gs2Ranking2.Model.ClusterRankingModel> GetAsync(
        #else
        private async Task<Gs2.Gs2Ranking2.Model.ClusterRankingModel> GetAsync(
        #endif
            GetClusterRankingModelRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithRankingName(this.RankingName);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                null,
                null,
                () => this._client.GetClusterRankingModelAsync(request)
            );
            return result?.Item;
        }

    }

    public partial class ClusterRankingModelDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Model.ClusterRankingModel> ModelFuture() => ModelAsync().ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Ranking2.Model.ClusterRankingModel> ModelAsync()
        #else
        public async Task<Gs2.Gs2Ranking2.Model.ClusterRankingModel> ModelAsync()
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Ranking2.Model.ClusterRankingModel>(
                        (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheParentKey(
                            this.NamespaceName,
                            null
                        ),
                        (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheKey(
                            this.RankingName
                        )
                    ).LockAsync()) {
                var (value, find) = (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RankingName,
                    null
                );
                if (find) {
                    return value;
                }
                return await (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).FetchAsync(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.RankingName,
                    null,
                    () => this.GetAsync(
                        new GetClusterRankingModelRequest()
                    )
                );
            }
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public UniTask<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async UniTask<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
/* diff --- start
        public IFuture<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model() => ModelFuture();
 diff --- end */
/* diff +++ start */
        public IFuture<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model()
        {
            return ModelFuture();
        }
/* diff +++ end */
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
/* diff --- start
        public Task<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model() => ModelAsync();
 diff --- end */
/* diff +++ start */
        public async Task<Gs2.Gs2Ranking2.Model.ClusterRankingModel> Model()
        {
            return await ModelAsync();
        }
/* diff +++ end */
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.RankingName,
                null
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Ranking2.Model.ClusterRankingModel> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheKey(
                    this.RankingName
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
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking2.Model.ClusterRankingModel>(
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheParentKey(
                    this.NamespaceName,
                    null
                ),
                (null as Gs2.Gs2Ranking2.Model.ClusterRankingModel).CacheKey(
                    this.RankingName
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
/* diff --- start
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Ranking2.Model.ClusterRankingModel> callback) =>
            SubscribeWithInitialCallAsync(callback).ToGs2Future();
 diff --- end */
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Ranking2.Model.ClusterRankingModel> callback) => SubscribeWithInitialCallAsync(callback).ToGs2Future(); /* diff +++ */
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.ClusterRankingModel> callback)
        #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.ClusterRankingModel> callback)
        #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }

    }
}
