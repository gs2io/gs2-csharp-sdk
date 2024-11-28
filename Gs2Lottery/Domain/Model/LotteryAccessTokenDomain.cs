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
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Model.Cache;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
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

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class LotteryAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        public string NamespaceName { get; } = null!;
        public AccessToken AccessToken { get; }
        public string UserId => this.AccessToken.UserId;
        public string LotteryName { get; } = null!;

        public LotteryAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken,
            string lotteryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.AccessToken = accessToken;
            this.LotteryName = lotteryName;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionFuture(
            PredictionRequest request
        ) {
            IEnumerator Impl(IFuture<Gs2.Gs2Lottery.Model.DrawnPrize[]> self)
            {
                request = request
                    .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithLotteryName(this.LotteryName)
                    .WithAccessToken(this.AccessToken?.Token);
                var future = request.InvokeFuture(
                    _gs2.Cache,
                    this.UserId,
                    () => this._client.PredictionFuture(request)
                );
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }
                var result = future.Result;
                self.OnComplete(result?.Items);
            }
            return new Gs2InlineFuture<Gs2.Gs2Lottery.Model.DrawnPrize[]>(Impl);
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if UNITY_2017_1_OR_NEWER
        public async UniTask<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionAsync(
            #else
        public async Task<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionAsync(
            #endif
            PredictionRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithLotteryName(this.LotteryName)
                .WithAccessToken(this.AccessToken?.Token);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                () => this._client.PredictionAsync(request)
            );
            return result?.Items;
        }
        #endif
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
        )
        {
            return new DescribeProbabilitiesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.LotteryName,
                this.AccessToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.Probability> ProbabilitiesAsync(
            #else
        public DescribeProbabilitiesIterator ProbabilitiesAsync(
            #endif
        )
        {
            return new DescribeProbabilitiesIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.LotteryName,
                this.AccessToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeProbabilities(
            Action<Gs2.Gs2Lottery.Model.Probability[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.Probability>(
                (null as Gs2.Gs2Lottery.Model.Probability).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.LotteryName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeProbabilitiesWithInitialCallAsync(
            Action<Gs2.Gs2Lottery.Model.Probability[]> callback
        )
        {
            var items = await ProbabilitiesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeProbabilities(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeProbabilities(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.Probability>(
                (null as Gs2.Gs2Lottery.Model.Probability).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.LotteryName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain Probability(
            string prizeId
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this.AccessToken,
                this.LotteryName,
                prizeId
            );
        }

    }
}
