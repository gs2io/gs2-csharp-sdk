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

#pragma warning disable CS0618 // Obsolete with a message
#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
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
        public static string CacheParentKey(
            this SendFriendRequest self,
            string namespaceName,
            string userId,
            bool? withProfile,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
                withProfile.ToString(),
                timeOffset?.ToString() ?? "0",
                "SendFriendRequest"
            );
        }

        public static string CacheKey(
            this SendFriendRequest self,
            string targetUserId
        ) {
            return string.Join(
                ":",
                targetUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SendFriendRequest> FetchFuture(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset,
            Func<IFuture<SendFriendRequest>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SendFriendRequest> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SendFriendRequest).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            withProfile,
                            targetUserId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && (e.Errors[0].Component == "targetUserId" || e.Errors[0].Component == "sendBox")) {
                            self.OnComplete(default);
                            yield break;
                        }
                    }
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    targetUserId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SendFriendRequest>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<SendFriendRequest> FetchAsync(
#else
        public static async Task<SendFriendRequest> FetchAsync(
#endif
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<SendFriendRequest>> fetchImpl
#else
            Func<Task<SendFriendRequest>> fetchImpl
#endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    targetUserId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SendFriendRequest).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    targetUserId,
                    timeOffset
                );
                if (e.errors.Length == 0 || (e.errors[0].component != "targetUserId" && e.errors[0].component != "sendBox")) {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<SendFriendRequest, bool> GetCache(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void PutCache(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static string PutUserData(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            self.PutCache(
                cache,
                namespaceName,
                userId,
                default,
                self.TargetUserId,
                timeOffset
            );
            return self.CacheParentKey(
                namespaceName,
                userId,
                default,
                timeOffset
            );
        }

        public static void DeleteCache(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void ListSubscribe(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            int? timeOffset,
            Action<SendFriendRequest[]> callback
        ) {
            cache.ListSubscribe<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                callbackId
            );
        }
/* diff +++ start */

        // withProfile の 3 通りの位置への書き込みは FriendUser / FollowUser と同じ規則にする。
        // 公開プロフィール付きの値は withProfile=true の位置にだけ置き、false と null の位置には
        // 公開プロフィールを外した値を置く。
        public static void PutCacheForEachWithProfile(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            int? timeOffset
        ) {
            if (self == null) {
                return;
            }
            if (withProfile ?? false) {
                self.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    true,
                    targetUserId,
                    timeOffset
                );
            }
            foreach (var position in new bool?[] {false, null}) {
                new SendFriendRequest {
                    UserId = self.UserId,
                    TargetUserId = self.TargetUserId,
                }.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    position,
                    targetUserId,
                    timeOffset
                );
            }
        }

        public static void DeleteCacheForEachWithProfile(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
            int? timeOffset
        ) {
            foreach (var withProfile in new bool?[] {false, true, null}) {
                self.DeleteCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    targetUserId,
                    timeOffset
                );
            }
        }

        public static void ClearListCacheForEachWithProfile(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            foreach (var withProfile in new bool?[] {false, true, null}) {
                cache.ClearListCache<SendFriendRequest>(
                    self.CacheParentKey(
                        namespaceName,
                        userId,
                        withProfile,
                        timeOffset
                    )
                );
            }
        }
/* diff +++ end */
    }
}
