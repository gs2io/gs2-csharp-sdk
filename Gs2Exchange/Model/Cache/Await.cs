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

namespace Gs2.Gs2Exchange.Model.Cache
{
    public static partial class AwaitExt
    {
        public static string CacheParentKey(
            this Await self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "exchange",
                namespaceName,
                userId,
                "Await"
            );
        }

        public static string CacheKey(
            this Await self,
            string awaitName
        ) {
            return string.Join(
                ":",
                awaitName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Await> FetchFuture(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string awaitName,
            Func<IFuture<Await>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Await> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Await).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            awaitName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "await") {
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
                    awaitName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Await>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Await> FetchAsync(
    #else
        public static async Task<Await> FetchAsync(
    #endif
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string awaitName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Await>> fetchImpl
    #else
            Func<Task<Await>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Await>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            awaitName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        awaitName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Await).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        awaitName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "await") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Await, bool> GetCache(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string awaitName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Await>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    awaitName
                )
            );
        }

        public static void PutCache(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string awaitName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Await>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    awaitName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            if (self?.Count == 0) {
                self = null;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    awaitName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string awaitName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Await>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    awaitName
                )
            );
        }

        public static void ListSubscribe(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Await[]> callback
        ) {
            cache.ListSubscribe<Await>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Await self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Await>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}