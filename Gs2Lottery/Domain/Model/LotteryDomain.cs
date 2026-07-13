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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Lottery.Domain.Model
{

    public partial class LotteryDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string LotteryName { get; } = null!;

        public LotteryDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId,
            string lotteryName
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
            this.LotteryName = lotteryName;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
            string timeOffsetToken = null
        )
        {
            return new DescribeProbabilitiesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.LotteryName,
                this.UserId,
                timeOffsetToken
            );
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.Probability> ProbabilitiesAsync(
        #else
        public DescribeProbabilitiesByUserIdIterator ProbabilitiesAsync(
        #endif
            string timeOffsetToken = null
        )
        {
            return new DescribeProbabilitiesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.LotteryName,
                this.UserId,
                timeOffsetToken
            );
        }

        public ulong SubscribeProbabilities(
            Action<Gs2.Gs2Lottery.Model.Probability[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.Probability>(
                (null as Gs2.Gs2Lottery.Model.Probability).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.LotteryName,
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
                            callback.Invoke(await ProbabilitiesAsync(
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
        public async UniTask<ulong> SubscribeProbabilitiesWithInitialCallAsync(
        #else
        public async Task<ulong> SubscribeProbabilitiesWithInitialCallAsync(
        #endif
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

        public void UnsubscribeProbabilities(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.Probability>(
                (null as Gs2.Gs2Lottery.Model.Probability).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.LotteryName,
                    null
                ),
                callbackId
            );
        }

        public void InvalidateProbabilities(
        )
        {
            this._gs2.Cache.ClearListCache<Gs2.Gs2Lottery.Model.Probability>(
                (null as Gs2.Gs2Lottery.Model.Probability).CacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    this.LotteryName,
                    null
                )
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.ProbabilityDomain Probability(
            string prizeId
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.ProbabilityDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                this.LotteryName,
                prizeId
            );
        }

    }

    public partial class LotteryDomain {

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> DrawFuture(
            DrawByUserIdRequest request
        ) => DrawAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> DrawAsync(
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> DrawAsync(
        #endif
            DrawByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithLotteryName(this.LotteryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.DrawByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionFuture(
            PredictionByUserIdRequest request
        ) => PredictionAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionAsync(
        #else
        public async Task<Gs2.Gs2Lottery.Model.DrawnPrize[]> PredictionAsync(
        #endif
            PredictionByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithLotteryName(this.LotteryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.PredictionByUserIdAsync(request)
            );
            return result?.Items;
        }

        #if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Core.Domain.TransactionDomain> DrawWithRandomSeedFuture(
            DrawWithRandomSeedByUserIdRequest request
        ) => DrawWithRandomSeedAsync(request).ToGs2Future();
        #endif

        #if GS2_ENABLE_UNITASK
        public async UniTask<Gs2.Core.Domain.TransactionDomain> DrawWithRandomSeedAsync(
        #else
        public async Task<Gs2.Core.Domain.TransactionDomain> DrawWithRandomSeedAsync(
        #endif
            DrawWithRandomSeedByUserIdRequest request
        ) {
            request = request
                .WithContextStack(string.IsNullOrEmpty(request.ContextStack) ? this._gs2.DefaultContextStack : request.ContextStack)
                .WithNamespaceName(this.NamespaceName)
                .WithLotteryName(this.LotteryName)
                .WithUserId(this.UserId);
            var result = await request.InvokeAsync(
                _gs2.Cache,
                this.UserId,
                null,
                () => this._client.DrawWithRandomSeedByUserIdAsync(request)
            );
            var transaction = Gs2.Core.Domain.TransactionDomainFactory.ToTransaction(
                this._gs2,
                this.UserId,
                result.AutoRunStampSheet ?? false,
                result.TransactionId,
                result.StampSheet,
                result.StampSheetEncryptionKeyId,
                result.AtomicCommit,
                result.TransactionResult,
                result.Metadata
            );
            if (result.StampSheet != null) {
                await transaction.WaitAsync(true);
            }
            return transaction;
        }

    }

    public partial class LotteryDomain {

    }
}
