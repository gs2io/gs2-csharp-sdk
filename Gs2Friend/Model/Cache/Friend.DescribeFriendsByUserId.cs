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
    public static partial class FriendExt
    {
        public static void PutCache(
            this DescribeFriendsByUserIdResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            DescribeFriendsByUserIdRequest request
        ) {
            foreach (var item in self.Items ?? Array.Empty<FriendUser>())
            {
                if (request.WithProfile ?? false) {
                    item?.PutCache(
                        cache,
                        request.NamespaceName,
                        userId,
                        true,
                        item.UserId,
                        timeOffset
                    );
                }
                if (item != null) {
                    new FriendUser {
                        UserId = item.UserId,
                    }.PutCache(
                        cache,
                        request.NamespaceName,
                        userId,
                        false,
                        item.UserId,
                        timeOffset
                    );
                    new FriendUser {
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
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DescribeFriendsByUserIdResult> InvokeFuture(
            this DescribeFriendsByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<DescribeFriendsByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DescribeFriendsByUserIdResult> self)
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
            return new Gs2InlineFuture<DescribeFriendsByUserIdResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DescribeFriendsByUserIdResult> InvokeAsync(
    #else
        public static async Task<DescribeFriendsByUserIdResult> InvokeAsync(
    #endif
            this DescribeFriendsByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DescribeFriendsByUserIdResult>> invokeImpl
    #else
            Func<Task<DescribeFriendsByUserIdResult>> invokeImpl
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