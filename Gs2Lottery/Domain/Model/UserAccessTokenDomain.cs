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
using Gs2.Gs2Lottery.Domain.Iterator;
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
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

    public partial class UserAccessTokenDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LotteryRestClient _client;
        private readonly string _namespaceName;
        private AccessToken _accessToken;
        public AccessToken AccessToken => _accessToken;

        private readonly String _parentKey;
        public string TransactionId { get; set; }
        public bool? AutoRunStampSheet { get; set; }
        public string NextPageToken { get; set; }
        public string NamespaceName => _namespaceName;
        public string UserId => _accessToken.UserId;

        public UserAccessTokenDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            AccessToken accessToken
        ) {
            this._gs2 = gs2;
            this._client = new Gs2LotteryRestClient(
                gs2.RestSession
            );
            this._namespaceName = namespaceName;
            this._accessToken = accessToken;
            this._parentKey = Gs2.Gs2Lottery.Domain.Model.NamespaceDomain.CreateCacheParentKey(
                this.NamespaceName,
                "User"
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.LotteryAccessTokenDomain Lottery(
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.LotteryAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Lottery.Model.BoxItems> Boxes(
        )
        {
            return new DescribeBoxesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.BoxItems> BoxesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Lottery.Model.BoxItems> Boxes(
            #endif
        #else
        public DescribeBoxesIterator BoxesAsync(
        #endif
        )
        {
            return new DescribeBoxesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                this.AccessToken
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeBoxes(
            Action<Gs2.Gs2Lottery.Model.BoxItems[]> callback
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.BoxItems>(
                Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "BoxItems"
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeBoxesWithInitialCallAsync(
            Action<Gs2.Gs2Lottery.Model.BoxItems[]> callback
        )
        {
            var items = await BoxesAsync(
            ).ToArrayAsync();
            var callbackId = SubscribeBoxes(
                callback
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeBoxes(
            ulong callbackId
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.BoxItems>(
                Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "BoxItems"
                ),
                callbackId
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain BoxItems(
            string prizeTableName
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.BoxItemsAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                prizeTableName
            );
        }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
            string lotteryName
        )
        {
            return new DescribeProbabilitiesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                lotteryName,
                this.AccessToken
            );
        }

        public IUniTaskAsyncEnumerable<Gs2.Gs2Lottery.Model.Probability> ProbabilitiesAsync(
            #else
        public Gs2Iterator<Gs2.Gs2Lottery.Model.Probability> Probabilities(
            #endif
        #else
        public DescribeProbabilitiesIterator ProbabilitiesAsync(
        #endif
            string lotteryName
        )
        {
            return new DescribeProbabilitiesIterator(
                this._gs2.Cache,
                this._client,
                this.NamespaceName,
                lotteryName,
                this.AccessToken
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        #else
            );
        #endif
        }

        public ulong SubscribeProbabilities(
            Action<Gs2.Gs2Lottery.Model.Probability[]> callback,
            string lotteryName
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Lottery.Model.Probability>(
                Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Probability:" + lotteryName
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeProbabilitiesWithInitialCallAsync(
            Action<Gs2.Gs2Lottery.Model.Probability[]> callback,
            string lotteryName
        )
        {
            var items = await ProbabilitiesAsync(
                lotteryName
            ).ToArrayAsync();
            var callbackId = SubscribeProbabilities(
                callback,
                lotteryName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeProbabilities(
            ulong callbackId,
            string lotteryName
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Lottery.Model.Probability>(
                Gs2.Gs2Lottery.Domain.Model.UserDomain.CreateCacheParentKey(
                    this.NamespaceName,
                    this.UserId,
                    "Probability:" + lotteryName
                ),
                callbackId
            );
        }

        public Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain Probability(
            string lotteryName,
            string prizeId
        ) {
            return new Gs2.Gs2Lottery.Domain.Model.ProbabilityAccessTokenDomain(
                this._gs2,
                this.NamespaceName,
                this._accessToken,
                lotteryName,
                prizeId
            );
        }

        public static string CreateCacheParentKey(
            string namespaceName,
            string userId,
            string childType
        )
        {
            return string.Join(
                ":",
                "lottery",
                namespaceName ?? "null",
                userId ?? "null",
                childType
            );
        }

        public static string CreateCacheKey(
            string userId
        )
        {
            return string.Join(
                ":",
                userId ?? "null"
            );
        }

    }
}
