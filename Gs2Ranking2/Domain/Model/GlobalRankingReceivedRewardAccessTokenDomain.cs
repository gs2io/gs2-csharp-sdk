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

    public partial class GlobalRankingReceivedRewardAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2Ranking2RestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string RankingName { get; } = null!;
        public long? Season { get; } = null!;
        public Gs2.Core.Model.AcquireAction[] AcquireActions { get; set; } = null!;

        public GlobalRankingReceivedRewardAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string rankingName,
            long? season
        ) {
            this._gs2 = gs2;
            this._client = new Gs2Ranking2RestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.RankingName = rankingName;
            this.Season = season;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain> CreateFuture(
            CreateGlobalRankingReceivedRewardRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRankingName(this.RankingName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithSeason(this.Season);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.CreateGlobalRankingReceivedRewardFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var domain = this;

                self.OnComplete(domain);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain> CreateAsync(
            #else
        public async Task<Gs2.Gs2Ranking2.Domain.Model.GlobalRankingReceivedRewardAccessTokenDomain> CreateAsync(
            #endif
            CreateGlobalRankingReceivedRewardRequest request
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
                () => this._client.CreateGlobalRankingReceivedRewardAsync(request)
            );
            var domain = this;

            return domain;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveFuture(
            ReceiveGlobalRankingReceivedRewardRequest request,
            bool speculativeExecute = true
        ) {
            IEnumerator Impl(IFuture<Gs2.Core.Domain.TransactionAccessTokenDomain> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithRankingName(this.RankingName)
                    .WithSeason(this.Season);

                if (speculativeExecute) {
                    var speculativeExecuteFuture = Gs2.Gs2Ranking2.Domain.Transaction.SpeculativeExecutor.ReceiveGlobalRankingReceivedRewardByUserIdSpeculativeExecutor.ExecuteFuture(
                        this._gs2,
                        AccessToken,
                        ReceiveGlobalRankingReceivedRewardByUserIdRequest.FromJson(request.ToJson())
                    );
                    yield return speculativeExecuteFuture;
                    if (speculativeExecuteFuture.Error != null)
                    {
                        self.OnError(speculativeExecuteFuture.Error);
                        yield break;
                    }
                    var commit = speculativeExecuteFuture.Result;
                    commit?.Invoke();
                }
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.ReceiveGlobalRankingReceivedRewardFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                    this._gs2,
                    this.AccessToken,
                    result.AutoRunStampSheet ?? false,
                    result.TransactionId,
                    result.StampSheet,
                    result.StampSheetEncryptionKeyId,
                    result.AtomicCommit,
                    result.TransactionResult
                );
                if (result.StampSheet != null) {
                    var future2 = transaction.WaitFuture(true);
                    yield return future2;
                    if (future2.Error != null)
                    {
                        self.OnError(future2.Error);
                        yield break;
                    }
                }
                self.OnComplete(transaction);
            }
            return new Gs2InlineFuture<Gs2.Core.Domain.TransactionAccessTokenDomain>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveAsync(
            #else
        public async Task<Gs2.Core.Domain.TransactionAccessTokenDomain> ReceiveAsync(
            #endif
            ReceiveGlobalRankingReceivedRewardRequest request,
            bool speculativeExecute = true
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithAccessToken(this.AccessToken?.Token)
                .WithRankingName(this.RankingName)
                .WithSeason(this.Season);

            if (speculativeExecute) {
                var commit = await Gs2.Gs2Ranking2.Domain.Transaction.SpeculativeExecutor.ReceiveGlobalRankingReceivedRewardByUserIdSpeculativeExecutor.ExecuteAsync(
                    this._gs2,
                    AccessToken,
                    ReceiveGlobalRankingReceivedRewardByUserIdRequest.FromJson(request.ToJson())
                );
                commit?.Invoke();
            }
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.ReceiveGlobalRankingReceivedRewardAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.AccessToken,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        private IFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GetFuture(
            GetGlobalRankingReceivedRewardRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithRankingName(this.RankingName)
                    .WithAccessToken(this.AccessToken?.Token)
                    .WithSeason(this.Season);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.GetGlobalRankingReceivedRewardFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Item);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        private async UniTask<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GetAsync(
            #else
        private async Task<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> GetAsync(
            #endif
            GetGlobalRankingReceivedRewardRequest request
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
                () => this._client.GetGlobalRankingReceivedRewardAsync(request)
            );
            return result?.Item;
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> ModelFuture()
        {
            IEnumerator Impl(IFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> self)
            {
                var (value, find) = (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).GetCache(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season ?? default
                );
                if (find) {
                    self.OnComplete(value);
                    yield break;
                }
                var future = (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).FetchFuture(
                    this._gs2.Cache,
                    this.NamespaceName,
                    this.UserId,
                    this.RankingName,
                    this.Season ?? default,
                    () => this.GetFuture(
                        new GetGlobalRankingReceivedRewardRequest()
                    )
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> ModelAsync()
            #else
        public async Task<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> ModelAsync()
            #endif
        {
            var (value, find) = (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).GetCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season ?? default
            );
            if (find) {
                return value;
            }
            return await (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).FetchAsync(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season ?? default,
                () => this.GetAsync(
                    new GetGlobalRankingReceivedRewardRequest()
                )
            );
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        [Obsolete("The name has been changed to ModelAsync.")]
        public async UniTask<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> Model()
        {
            return await ModelAsync();
        }
            #else
        [Obsolete("The name has been changed to ModelFuture.")]
        public IFuture<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> Model()
        {
            return ModelFuture();
        }
            #endif
        #else
        [Obsolete("The name has been changed to ModelAsync.")]
        public async Task<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> Model()
        {
            return await ModelAsync();
        }
        #endif


        public void Invalidate()
        {
            (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).DeleteCache(
                this._gs2.Cache,
                this.NamespaceName,
                this.UserId,
                this.RankingName,
                this.Season ?? default
            );
        }

        public ulong Subscribe(Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> callback)
        {
            return this._gs2.Cache.Subscribe(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheKey(
                    this.RankingName,
                    this.Season ?? default
                ),
                callback,
                () =>
                {
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
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
            #if GS2_ENABLE_UNITASK
                    Impl().Forget();
            #else
                    Impl();
            #endif
        #endif
                }
            );
        }

        public void Unsubscribe(ulong callbackId)
        {
            this._gs2.Cache.Unsubscribe<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward>(
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                (null as Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward).CacheKey(
                    this.RankingName,
                    this.Season ?? default
                ),
                callbackId
            );
        }

        #if UNITY_2017_1_OR_NEWER
        public Gs2Future<ulong> SubscribeWithInitialCallFuture(Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> callback)
        {
            IEnumerator Impl(IFuture<ulong> self)
            {
                var future = ModelFuture();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                var callbackId = Subscribe(callback);
                callback.Invoke(item);
                self.OnComplete(callbackId);
            }
            return new Gs2InlineFuture<ulong>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> callback)
            #else
        public async Task<ulong> SubscribeWithInitialCallAsync(Action<Gs2.Gs2Ranking2.Model.GlobalRankingReceivedReward> callback)
            #endif
        {
            var item = await ModelAsync();
            var callbackId = Subscribe(callback);
            callback.Invoke(item);
            return callbackId;
        }
        #endif

    }
}
