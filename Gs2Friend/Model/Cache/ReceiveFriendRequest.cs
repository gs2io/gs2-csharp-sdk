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
    public static partial class ReceiveFriendRequestExt
    {
        public static string CacheParentKey(
            this ReceiveFriendRequest self,
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
                "ReceiveFriendRequest"
            );
        }

        public static string CacheKey(
            this ReceiveFriendRequest self,
            string fromUserId
        ) {
            return string.Join(
                ":",
                fromUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ReceiveFriendRequest> FetchFuture(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
            int? timeOffset,
            Func<IFuture<ReceiveFriendRequest>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ReceiveFriendRequest> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ReceiveFriendRequest).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            withProfile,
                            fromUserId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && (e.Errors[0].Component == "fromUserId" || e.Errors[0].Component == "inbox")) {
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
                    fromUserId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ReceiveFriendRequest>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<ReceiveFriendRequest> FetchAsync(
#else
        public static async Task<ReceiveFriendRequest> FetchAsync(
#endif
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<ReceiveFriendRequest>> fetchImpl
#else
            Func<Task<ReceiveFriendRequest>> fetchImpl
#endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    fromUserId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as ReceiveFriendRequest).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    fromUserId,
                    timeOffset
                );
                if (e.errors.Length == 0 || (e.errors[0].component != "fromUserId" && e.errors[0].component != "inbox")) {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<ReceiveFriendRequest, bool> GetCache(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                self.CacheKey(
                    fromUserId
                )
            );
        }

        public static void PutCache(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
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
                    fromUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static string PutUserData(
            this ReceiveFriendRequest self,
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
                self.UserId,
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
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile,
                    timeOffset
                ),
                self.CacheKey(
                    fromUserId
                )
            );
        }

        public static void ListSubscribe(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            int? timeOffset,
            Action<ReceiveFriendRequest[]> callback
        ) {
            cache.ListSubscribe<ReceiveFriendRequest>(
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
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ReceiveFriendRequest>(
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
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string fromUserId,
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
                    fromUserId,
                    timeOffset
                );
            }
            foreach (var position in new bool?[] {false, null}) {
                new ReceiveFriendRequest {
                    UserId = self.UserId,
                    TargetUserId = self.TargetUserId,
                }.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    position,
                    fromUserId,
                    timeOffset
                );
            }
        }

        public static void DeleteCacheForEachWithProfile(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string fromUserId,
            int? timeOffset
        ) {
            foreach (var withProfile in new bool?[] {false, true, null}) {
                self.DeleteCache(
                    cache,
                    namespaceName,
                    userId,
                    withProfile,
                    fromUserId,
                    timeOffset
                );
            }
        }

        public static void ClearListCacheForEachWithProfile(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            foreach (var withProfile in new bool?[] {false, true, null}) {
                cache.ClearListCache<ReceiveFriendRequest>(
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
