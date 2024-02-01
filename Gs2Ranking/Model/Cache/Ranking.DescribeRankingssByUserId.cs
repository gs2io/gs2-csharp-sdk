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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Ranking.Request;
using Gs2.Gs2Ranking.Result;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Ranking.Model.Cache
{
    public static partial class RankingExt
    {
        public static void PutCache(
            this DescribeRankingssByUserIdResult self,
            CacheDatabase cache,
            string userId,
            DescribeRankingssByUserIdRequest request
        ) {
            foreach (var item in self.Items ?? Array.Empty<Ranking>())
            {
                item.PutCache(
                    cache,
                    request.NamespaceName,
                    request.UserId,
                    request.CategoryName,
                    request.AdditionalScopeName,
                    item.UserId,
                    item.Index
                );
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DescribeRankingssByUserIdResult> InvokeFuture(
            this DescribeRankingssByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<DescribeRankingssByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DescribeRankingssByUserIdResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }

                future.Result.PutCache(
                    cache,
                    userId,
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<DescribeRankingssByUserIdResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DescribeRankingssByUserIdResult> InvokeAsync(
    #else
        public static async Task<DescribeRankingssByUserIdResult> InvokeAsync(
    #endif
            this DescribeRankingssByUserIdRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DescribeRankingssByUserIdResult>> invokeImpl
    #else
            Func<Task<DescribeRankingssByUserIdResult>> invokeImpl
    #endif
        )
        {
            var result = await invokeImpl();
            result.PutCache(
                cache,
                userId,
                request
            );
            return result;
        }
#endif
    }
}