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
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
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

namespace Gs2.Gs2Friend.Model.Cache
{
    public static partial class FollowUserExt
    {
        public static void PutCache(
            this DescribeFollowsResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            DescribeFollowsRequest request
        ) {
            foreach (var item in self.Items ?? Array.Empty<FollowUser>())
            {
                if (request.WithProfile ?? false) {
                    item.PutCache(
                        cache,
                        request.NamespaceName,
                        userId,
                        true,
                        item.UserId,
                        timeOffset
                    );
                }
                new FollowUser {
                    UserId = item.UserId,
                }.PutCache(
                    cache,
                    request.NamespaceName,
                    userId,
                    false,
                    item.UserId,
                    timeOffset
                );
                new FollowUser {
                    UserId = item.UserId,
                }.PutCache(
                    cache,
                    request.NamespaceName,
                    userId,
                    null,
                    item.UserId,
                    timeOffset
                );
                if (request.WithProfile ?? false) {
                    new PublicProfile {
                        UserId = item.UserId,
                        Value = item.PublicProfile
                    }.PutCache(
                        cache,
                        request.NamespaceName,
                        item.UserId,
                        timeOffset
                    );
                }
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DescribeFollowsResult> InvokeFuture(
            this DescribeFollowsRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<DescribeFollowsResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DescribeFollowsResult> self)
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
                    timeOffset,
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<DescribeFollowsResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DescribeFollowsResult> InvokeAsync(
    #else
        public static async Task<DescribeFollowsResult> InvokeAsync(
    #endif
            this DescribeFollowsRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DescribeFollowsResult>> invokeImpl
    #else
            Func<Task<DescribeFollowsResult>> invokeImpl
    #endif
        )
        {
            var result = await invokeImpl();
            result.PutCache(
                cache,
                userId,
                timeOffset,
                request
            );
            return result;
        }
#endif
    }
}