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

namespace Gs2.Gs2Ranking2.Model.Cache
{
    public static partial class SubscribeUserExt
    {
        public static string CacheParentKey(
            this SubscribeUser self,
            string namespaceName,
            string userId,
            string rankingName
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                rankingName,
                "SubscribeUser"
            );
        }

        public static string CacheKey(
            this SubscribeUser self,
            string targetUserId
        ) {
            return string.Join(
                ":",
                targetUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeUser> FetchFuture(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string targetUserId,
            Func<IFuture<SubscribeUser>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeUser> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeUser).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName,
                            targetUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeUser") {
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
                    rankingName,
                    targetUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscribeUser>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeUser> FetchAsync(
    #else
        public static async Task<SubscribeUser> FetchAsync(
    #endif
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string targetUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeUser>> fetchImpl
    #else
            Func<Task<SubscribeUser>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SubscribeUser>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            rankingName
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
                        rankingName,
                        targetUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SubscribeUser).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        targetUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "subscribeUser") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SubscribeUser, bool> GetCache(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SubscribeUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void PutCache(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    targetUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string targetUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SubscribeUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    targetUserId
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            Action<SubscribeUser[]> callback
        ) {
            cache.ListSubscribe<SubscribeUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SubscribeUser self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeUser>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                callbackId
            );
        }
    }
}