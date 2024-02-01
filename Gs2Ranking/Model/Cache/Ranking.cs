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

namespace Gs2.Gs2Ranking.Model.Cache
{
    public static partial class RankingExt
    {
        public static string CacheParentKey(
            this Ranking self,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName
        ) {
            return string.Join(
                ":",
                "ranking",
                namespaceName,
                userId,
                categoryName,
                additionalScopeName,
                "Ranking"
            );
        }

        public static string CacheKey(
            this Ranking self,
            string scorerUserId,
            long? index
        ) {
            return string.Join(
                ":",
                scorerUserId,
                index.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Ranking> FetchFuture(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            string scorerUserId,
            long? index,
            Func<IFuture<Ranking>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Ranking> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Ranking).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            categoryName,
                            additionalScopeName,
                            scorerUserId,
                            index
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "ranking") {
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
                    categoryName,
                    additionalScopeName,
                    scorerUserId,
                    index
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Ranking>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Ranking> FetchAsync(
    #else
        public static async Task<Ranking> FetchAsync(
    #endif
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            string scorerUserId,
            long? index,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Ranking>> fetchImpl
    #else
            Func<Task<Ranking>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Ranking>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            categoryName,
                            additionalScopeName
                       ),
                       self.CacheKey(
                            scorerUserId,
                            index
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        categoryName,
                        additionalScopeName,
                        scorerUserId,
                        index
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Ranking).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        categoryName,
                        additionalScopeName,
                        scorerUserId,
                        index
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "ranking") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Ranking, bool> GetCache(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            string scorerUserId,
            long? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Ranking>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    categoryName,
                    additionalScopeName
                ),
                self.CacheKey(
                    scorerUserId,
                    index
                )
            );
        }

        public static void PutCache(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            string scorerUserId,
            long? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    categoryName,
                    additionalScopeName
                ),
                self.CacheKey(
                    scorerUserId,
                    index
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            string scorerUserId,
            long? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Ranking>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    categoryName,
                    additionalScopeName
                ),
                self.CacheKey(
                    scorerUserId,
                    index
                )
            );
        }

        public static void ListSubscribe(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            Action<Ranking[]> callback
        ) {
            cache.ListSubscribe<Ranking>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    categoryName,
                    additionalScopeName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Ranking self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            string additionalScopeName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Ranking>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    categoryName,
                    additionalScopeName
                ),
                callbackId
            );
        }
    }
}