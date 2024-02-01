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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
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
    public static partial class FriendUserExt
    {
        public static string CacheParentKey(
            this FriendUser self,
            string namespaceName,
            string userId,
            bool? withProfile
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
                withProfile.ToString(),
                "FriendUser"
            );
        }

        public static string CacheKey(
            this FriendUser self,
            string targetUserId
        ) {
            return string.Join(
                ":",
                targetUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<FriendUser> FetchFuture(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
            Func<IFuture<FriendUser>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<FriendUser> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as FriendUser).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            withProfile,
                            targetUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "friendUser") {
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
                    targetUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<FriendUser>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<FriendUser> FetchAsync(
    #else
        public static async Task<FriendUser> FetchAsync(
    #endif
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<FriendUser>> fetchImpl
    #else
            Func<Task<FriendUser>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<FriendUser>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            withProfile
                       ),
                       self.CacheKey(
                            targetUserId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        withProfile,
                        targetUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as FriendUser).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        withProfile,
                        targetUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "friendUser") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<FriendUser, bool> GetCache(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<FriendUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void PutCache(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile
                ),
                self.CacheKey(
                    targetUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<FriendUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void ListSubscribe(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            Action<FriendUser[]> callback
        ) {
            cache.ListSubscribe<FriendUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this FriendUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<FriendUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    withProfile
                ),
                callbackId
            );
        }
    }
}