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
    public static partial class SendFriendRequestExt
    {
        public static string CacheParentKey(
            this SendFriendRequest self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
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
            string targetUserId,
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
                            targetUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "sendFriendRequest") {
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
                    targetUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SendFriendRequest>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SendFriendRequest> FetchAsync(
    #else
        public static async Task<SendFriendRequest> FetchAsync(
    #endif
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SendFriendRequest>> fetchImpl
    #else
            Func<Task<SendFriendRequest>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SendFriendRequest>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
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
                        targetUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SendFriendRequest).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        targetUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "sendFriendRequest") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SendFriendRequest, bool> GetCache(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
            string targetUserId
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
                    targetUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
            Action<SendFriendRequest[]> callback
        ) {
            cache.ListSubscribe<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SendFriendRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SendFriendRequest>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}