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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Model.Cache
{
    public static partial class ReceiveFriendRequestExt
    {
        public static void PutCache(
            this AcceptRequestByUserIdResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            AcceptRequestByUserIdRequest request
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
/* diff --- start
            cache.Delete<FriendRequest>(
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheParentKey(
                    request.NamespaceName,
                    request.UserId,
                    timeOffset
                ),
                (null as Gs2.Gs2Friend.Model.ReceiveFriendRequest).CacheKey(
                    self.Item.TargetUserId
                )
 diff --- end */
/* diff +++ start */
            if (self.Item == null) {
                return;
            }
            // 受信したフレンドリクエスト
            (null as ReceiveFriendRequest).DeleteCacheForEachWithProfile(
                cache,
                request.NamespaceName,
                self.Item.TargetUserId,
                self.Item.UserId,
                timeOffset
/* diff +++ end */
            );
/* diff +++ start */
            // 送信したフレンドリクエスト
            (null as SendFriendRequest).DeleteCacheForEachWithProfile(
                cache,
                request.NamespaceName,
                self.Item.UserId,
                self.Item.TargetUserId,
                timeOffset
            );

            foreach (var withProfile in new bool?[] {false, true, null}) {
                // 送信したユーザー側のフレンドリストのキャッシュを削除
                cache.ClearListCache<FriendUser>(
                    (null as FriendUser).CacheParentKey(
                        request.NamespaceName,
                        self.Item.UserId,
                        withProfile,
                        timeOffset
                    )
                );
                // 受信したユーザー側のフレンドリストのキャッシュを削除
                cache.ClearListCache<FriendUser>(
                    (null as FriendUser).CacheParentKey(
                        request.NamespaceName,
                        self.Item.TargetUserId,
                        withProfile,
                        timeOffset
                    )
                );
            }
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AcceptRequestByUserIdResult> InvokeFuture(
            this AcceptRequestByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<AcceptRequestByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<AcceptRequestByUserIdResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    if (future.Error is Gs2.Core.Exception.NotFoundException) {
/* diff --- start
                        (null as ReceiveFriendRequest).DeleteCache(
 diff --- end */
                        (null as ReceiveFriendRequest).DeleteCacheForEachWithProfile( /* diff +++ */
                            cache,
                            request.NamespaceName,
                            request.UserId,
                            request.FromUserId,
                            timeOffset
                        );
/* diff --- start
                        cache.ClearListCache<ReceiveFriendRequest>(
                            (null as ReceiveFriendRequest).CacheParentKey(
                                request.NamespaceName,
                                request.UserId,
                                timeOffset
                            )
 diff --- end */
/* diff +++ start */
                        (null as ReceiveFriendRequest).ClearListCacheForEachWithProfile(
                            cache,
                            request.NamespaceName,
                            request.UserId,
                            timeOffset
/* diff +++ end */
                        );
                    }
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
            return new Gs2InlineFuture<AcceptRequestByUserIdResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<AcceptRequestByUserIdResult> InvokeAsync(
#else
        public static async Task<AcceptRequestByUserIdResult> InvokeAsync(
#endif
            this AcceptRequestByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<AcceptRequestByUserIdResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<AcceptRequestByUserIdResult>> invokeImpl
#else
            Func<Task<AcceptRequestByUserIdResult>> invokeImpl
#endif
        )
        {
            AcceptRequestByUserIdResult result;
            try {
                result = await invokeImpl();
            }
            catch (Gs2.Core.Exception.NotFoundException) {
/* diff --- start
                (null as ReceiveFriendRequest).DeleteCache(
 diff --- end */
                (null as ReceiveFriendRequest).DeleteCacheForEachWithProfile( /* diff +++ */
                    cache,
                    request.NamespaceName,
                    request.UserId,
                    request.FromUserId,
                    timeOffset
                );
/* diff --- start
                cache.ClearListCache<ReceiveFriendRequest>(
                    (null as ReceiveFriendRequest).CacheParentKey(
                        request.NamespaceName,
                        request.UserId,
                        timeOffset
                    )
 diff --- end */
/* diff +++ start */
                (null as ReceiveFriendRequest).ClearListCacheForEachWithProfile(
                    cache,
                    request.NamespaceName,
                    request.UserId,
                    timeOffset
/* diff +++ end */
                );
                throw;
            }
            result.PutCache(
                cache,
                userId,
                timeOffset,
                request
            );
            return result;
        }
    }
}