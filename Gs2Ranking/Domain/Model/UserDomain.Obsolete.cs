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

namespace Gs2.Gs2Ranking.Domain.Model
{
    public partial class UserDomain {

        public Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain SubscribeUser(
            string categoryName,
            string targetUserId
        ) {
            return this.RankingCategory(
                categoryName
            ).SubscribeUser(
                targetUserId
            );
        }
        
#if UNITY_2017_1_OR_NEWER
        public IFuture<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeFuture(
            Gs2.Gs2Ranking.Request.SubscribeByUserIdRequest request
        ) {
            return this.RankingCategory(
                request.CategoryName
            ).SubscribeFuture(
                request
            );
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public UniTask<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
    #else
        public Task<Gs2.Gs2Ranking.Domain.Model.SubscribeUserDomain> SubscribeAsync(
    #endif
            Gs2.Gs2Ranking.Request.SubscribeByUserIdRequest request
        ) {
            return this.RankingCategory(
                request.CategoryName
            ).SubscribeAsync(
                request
            );
        }
#endif

#if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryName(
            string categoryName
        ) {
            return this.RankingCategory(
                categoryName
            ).SubscribesByCategoryName();
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.SubscribeUser> SubscribesByCategoryNameAsync(
    #else
        public DescribeSubscribesByCategoryNameAndUserIdIterator SubscribesByCategoryNameAsync(
    #endif
            string categoryName
        ) {
            return this.RankingCategory(
                categoryName
            ).SubscribesByCategoryNameAsync();
        }
#endif

        public Gs2.Gs2Ranking.Domain.Model.RankingCategoryDomain Ranking(
            string categoryName,
            string additionalScopeName = null
        ) {
            return this.RankingCategory(
                categoryName,
                additionalScopeName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> Rankings(
            string categoryName,
            string additionalScopeName = null
        ) {
            return Ranking(
                categoryName,
                additionalScopeName
            ).Rankings();
        }
#endif
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
#if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> RankingsAsync(
#else
        public DescribeRankingsByUserIdIterator RankingsAsync(
#endif
            string categoryName,
            string additionalScopeName = null
        )
        {
            return Ranking(
                categoryName,
                additionalScopeName
            ).RankingsAsync();
        }
#endif

#if UNITY_2017_1_OR_NEWER
        public Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> NearRankings(
            string categoryName,
            long score,
            string additionalScopeName = null
        ) {
            return this.RankingCategory(
                categoryName,
                additionalScopeName
            ).NearRankings(
                score
            );
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> NearRankingsAsync(
    #else
        public DescribeNearRankingsIterator NearRankingsAsync(
    #endif
            string categoryName,
            long score,
            string additionalScopeName = null
        ) {
            return this.RankingCategory(
                categoryName,
                additionalScopeName
            ).NearRankingsAsync(
                score
            );
        }
#endif

    }
}
