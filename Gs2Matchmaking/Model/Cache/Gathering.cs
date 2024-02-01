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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static partial class GatheringExt
    {
        public static string CacheParentKey(
            this Gathering self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                "Singleton",
                "Gathering"
            );
        }

        public static string CacheKey(
            this Gathering self,
            string gatheringName
        ) {
            return string.Join(
                ":",
                gatheringName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Gathering> FetchFuture(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gatheringName,
            Func<IFuture<Gathering>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Gathering> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Gathering).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            gatheringName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "gathering") {
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
                    gatheringName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Gathering>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Gathering> FetchAsync(
    #else
        public static async Task<Gathering> FetchAsync(
    #endif
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gatheringName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Gathering>> fetchImpl
    #else
            Func<Task<Gathering>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Gathering>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            gatheringName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        gatheringName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Gathering).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        gatheringName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "gathering") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Gathering, bool> GetCache(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gatheringName
        ) {
            return cache.Get<Gathering>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    gatheringName
                )
            );
        }

        public static void PutCache(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gatheringName
        ) {
            var (value, find) = cache.Get<Gathering>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    gatheringName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    gatheringName
                ),
                self,
                ((self?.ExpiresAt ?? 0) == 0 ? null : self.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string gatheringName
        ) {
            cache.Delete<Gathering>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    gatheringName
                )
            );
        }

        public static void ListSubscribe(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Gathering[]> callback
        ) {
            cache.ListSubscribe<Gathering>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Gathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Gathering>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}