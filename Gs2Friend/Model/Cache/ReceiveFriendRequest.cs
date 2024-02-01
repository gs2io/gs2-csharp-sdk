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
    public static partial class ReceiveFriendRequestExt
    {
        public static string CacheParentKey(
            this ReceiveFriendRequest self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
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
            string fromUserId,
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
                            fromUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "receiveFriendRequest") {
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
                    fromUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ReceiveFriendRequest>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ReceiveFriendRequest> FetchAsync(
    #else
        public static async Task<ReceiveFriendRequest> FetchAsync(
    #endif
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string fromUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ReceiveFriendRequest>> fetchImpl
    #else
            Func<Task<ReceiveFriendRequest>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ReceiveFriendRequest>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            fromUserId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        fromUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ReceiveFriendRequest).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        fromUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "receiveFriendRequest") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ReceiveFriendRequest, bool> GetCache(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string fromUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
            string fromUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    fromUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string fromUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
            Action<ReceiveFriendRequest[]> callback
        ) {
            cache.ListSubscribe<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ReceiveFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ReceiveFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}