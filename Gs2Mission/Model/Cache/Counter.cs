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
 *
 * deny overwrite
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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static partial class CounterExt
    {
        public static string CacheParentKey(
            this Counter self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                userId,
                "Counter"
            );
        }

        public static string CacheKey(
            this Counter self,
            string counterName
        ) {
            return string.Join(
                ":",
                counterName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Counter> FetchFuture(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName,
            Func<IFuture<Counter>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Counter> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Counter).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            counterName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "counter") {
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
                    counterName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Counter>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Counter> FetchAsync(
    #else
        public static async Task<Counter> FetchAsync(
    #endif
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Counter>> fetchImpl
    #else
            Func<Task<Counter>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Counter>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            counterName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        counterName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Counter).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        counterName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "counter") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Counter, bool> GetCache(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Counter>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    counterName
                )
            );
        }

        public static void PutCache(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Counter>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    counterName
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
                    counterName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
            cache.ClearListCache<Complete>(
                (null as Complete).CacheParentKey(
                    namespaceName,
                    userId
                )
            );
        }

        public static void DeleteCache(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Counter>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    counterName
                )
            );
        }

        public static void ListSubscribe(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Counter[]> callback
        ) {
            cache.ListSubscribe<Counter>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Counter self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Counter>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}