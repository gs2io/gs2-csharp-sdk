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
    public static partial class FriendRequestExt
    {
/* diff --- start
        public static string CacheParentKey(
            this FriendRequest self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "FriendRequest"
            );
        }

        public static string CacheKey(
            this FriendRequest self,
            string targetUserId
        ) {
            return string.Join(
                ":",
                targetUserId
            );
        }

//#if UNITY_2017_1_OR_NEWER
        public static IFuture<FriendRequest> FetchFuture(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
            int? timeOffset,
            Func<IFuture<FriendRequest>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<FriendRequest> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as FriendRequest).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            targetUserId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "friendRequest") {
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
                    targetUserId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<FriendRequest>(Impl);
        }
//#endif

//#if GS2_ENABLE_UNITASK
        public static async UniTask<FriendRequest> FetchAsync(
//#else
        public static async Task<FriendRequest> FetchAsync(
//#endif
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
            int? timeOffset,
//#if GS2_ENABLE_UNITASK
            Func<UniTask<FriendRequest>> fetchImpl
//#else
            Func<Task<FriendRequest>> fetchImpl
//#endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    targetUserId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as FriendRequest).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    targetUserId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "friendRequest") {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<FriendRequest, bool> GetCache(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<FriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void PutCache(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
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
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<FriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void ListSubscribe(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<FriendRequest[]> callback
        ) {
            cache.ListSubscribe<FriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this FriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<FriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callbackId
            );
        }
 diff --- end */
/* diff +++ start */
        
/* diff +++ end */
    }
}