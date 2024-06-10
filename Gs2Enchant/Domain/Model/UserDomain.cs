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
using Gs2.Gs2Enchant.Domain.Iterator;
using Gs2.Gs2Enchant.Model.Cache;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Result;
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

namespace Gs2.Gs2Enchant.Domain.Model
{

    public partial class UserDomain {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2EnchantRestClient _client;
        public string NamespaceName { get; } = null!;
        public string UserId { get; } = null!;
        public string NextPageToken { get; set; } = null!;

        public UserDomain(
            Gs2.Core.Domain.Gs2 gs2,
            string namespaceName,
            string userId
        ) {
            this._gs2 = gs2;
            this._client = new Gs2EnchantRestClient(
                gs2.RestSession
            );
            this.NamespaceName = namespaceName;
            this.UserId = userId;
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enchant.Model.BalanceParameterStatus> BalanceParameterStatuses(
            string parameterName = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeBalanceParameterStatusesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                parameterName,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.BalanceParameterStatus> BalanceParameterStatusesAsync(
            #else
        public DescribeBalanceParameterStatusesByUserIdIterator BalanceParameterStatusesAsync(
            #endif
            string parameterName = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeBalanceParameterStatusesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                parameterName,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeBalanceParameterStatuses(
            Action<Gs2.Gs2Enchant.Model.BalanceParameterStatus[]> callback,
            string parameterName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                (null as Gs2.Gs2Enchant.Model.BalanceParameterStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeBalanceParameterStatusesWithInitialCallAsync(
            Action<Gs2.Gs2Enchant.Model.BalanceParameterStatus[]> callback,
            string parameterName = null
        )
        {
            var items = await BalanceParameterStatusesAsync(
                parameterName
            ).ToArrayAsync();
            var callbackId = SubscribeBalanceParameterStatuses(
                callback,
                parameterName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeBalanceParameterStatuses(
            ulong callbackId,
            string parameterName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.BalanceParameterStatus>(
                (null as Gs2.Gs2Enchant.Model.BalanceParameterStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain BalanceParameterStatus(
            string parameterName,
            string propertyId
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.BalanceParameterStatusDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                parameterName,
                propertyId
            );
        }
        #if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Enchant.Model.RarityParameterStatus> RarityParameterStatuses(
            string parameterName = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeRarityParameterStatusesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                parameterName,
                timeOffsetToken
            );
        }
        #endif

        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Enchant.Model.RarityParameterStatus> RarityParameterStatusesAsync(
            #else
        public DescribeRarityParameterStatusesByUserIdIterator RarityParameterStatusesAsync(
            #endif
            string parameterName = null,
            string timeOffsetToken = null
        )
        {
            return new DescribeRarityParameterStatusesByUserIdIterator(
                this._gs2,
                this._client,
                this.NamespaceName,
                this.UserId,
                parameterName,
                timeOffsetToken
            #if GS2_ENABLE_UNITASK
            ).GetAsyncEnumerator();
            #else
            );
            #endif
        }
        #endif

        public ulong SubscribeRarityParameterStatuses(
            Action<Gs2.Gs2Enchant.Model.RarityParameterStatus[]> callback,
            string parameterName = null
        )
        {
            return this._gs2.Cache.ListSubscribe<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                (null as Gs2.Gs2Enchant.Model.RarityParameterStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callback
            );
        }

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<ulong> SubscribeRarityParameterStatusesWithInitialCallAsync(
            Action<Gs2.Gs2Enchant.Model.RarityParameterStatus[]> callback,
            string parameterName = null
        )
        {
            var items = await RarityParameterStatusesAsync(
                parameterName
            ).ToArrayAsync();
            var callbackId = SubscribeRarityParameterStatuses(
                callback,
                parameterName
            );
            callback.Invoke(items);
            return callbackId;
        }
        #endif

        public void UnsubscribeRarityParameterStatuses(
            ulong callbackId,
            string parameterName = null
        )
        {
            this._gs2.Cache.ListUnsubscribe<Gs2.Gs2Enchant.Model.RarityParameterStatus>(
                (null as Gs2.Gs2Enchant.Model.RarityParameterStatus).CacheParentKey(
                    this.NamespaceName,
                    this.UserId
                ),
                callbackId
            );
        }

        public Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain RarityParameterStatus(
            string parameterName,
            string propertyId
        ) {
            return new Gs2.Gs2Enchant.Domain.Model.RarityParameterStatusDomain(
                this._gs2,
                this.NamespaceName,
                this.UserId,
                parameterName,
                propertyId
            );
        }

    }

    public partial class UserDomain {

    }

    public partial class UserDomain {

    }
}
