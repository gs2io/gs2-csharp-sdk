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
    public static partial class SendFriendRequestExt
    {
        public static void PutCache(
            this SendRequestResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            SendRequestRequest request
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
/* diff --- start
            cache.Put(
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheParentKey(
                    request.NamespaceName,
                    userId,
                    request.WithProfile ?? default,
                    timeOffset
                ),
                (null as Gs2.Gs2Friend.Model.SendFriendRequest).CacheKey(
                    self.Item.TargetUserId
                ),
                self.Item,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
 diff --- end */
/* diff +++ start */
            if (self.Item == null) {
                return;
            }
            new SendFriendRequest {
                UserId = self.Item.UserId,
                TargetUserId = self.Item.TargetUserId,
                PublicProfile = self.Item.PublicProfile,
            }.PutCacheForEachWithProfile(
                cache,
                request.NamespaceName,
                self.Item.UserId,
                request.WithProfile,
                self.Item.TargetUserId,
                timeOffset
            );
            // 相手の受信ボックス。応答の公開プロフィールは相手のものなので、公開プロフィール無しで置く
            new ReceiveFriendRequest {
                UserId = self.Item.UserId,
                TargetUserId = self.Item.TargetUserId,
            }.PutCacheForEachWithProfile(
                cache,
                request.NamespaceName,
                self.Item.TargetUserId,
                false,
                self.Item.UserId,
                timeOffset
/* diff +++ end */
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SendRequestResult> InvokeFuture(
            this SendRequestRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<SendRequestResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<SendRequestResult> self)
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
            return new Gs2InlineFuture<SendRequestResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<SendRequestResult> InvokeAsync(
#else
        public static async Task<SendRequestResult> InvokeAsync(
#endif
            this SendRequestRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<SendRequestResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<SendRequestResult>> invokeImpl
#else
            Func<Task<SendRequestResult>> invokeImpl
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
    }
}