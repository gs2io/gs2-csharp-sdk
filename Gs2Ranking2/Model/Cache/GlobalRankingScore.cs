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

namespace Gs2.Gs2Ranking2.Model.Cache
{
    public static partial class GlobalRankingScoreExt
    {
        public static string CacheParentKey(
            this GlobalRankingScore self,
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
                "GlobalRankingScore"
            );
        }

        public static string CacheKey(
            this GlobalRankingScore self,
            string rankingName,
            long? season
        ) {
            return string.Join(
                ":",
                rankingName,
                season.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GlobalRankingScore> FetchFuture(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            Func<IFuture<GlobalRankingScore>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GlobalRankingScore> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GlobalRankingScore).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName,
                            season
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "globalRankingScore") {
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
                    season
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GlobalRankingScore>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GlobalRankingScore> FetchAsync(
    #else
        public static async Task<GlobalRankingScore> FetchAsync(
    #endif
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GlobalRankingScore>> fetchImpl
    #else
            Func<Task<GlobalRankingScore>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GlobalRankingScore>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            rankingName
                       ),
                       self.CacheKey(
                            rankingName,
                            season
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        season
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GlobalRankingScore).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        season
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "globalRankingScore") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GlobalRankingScore, bool> GetCache(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<GlobalRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    rankingName,
                    season
                )
            );
        }

        public static void PutCache(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<GlobalRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    rankingName,
                    season
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    rankingName,
                    season
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<GlobalRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                self.CacheKey(
                    rankingName,
                    season
                )
            );
        }

        public static void ListSubscribe(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            Action<GlobalRankingScore[]> callback
        ) {
            cache.ListSubscribe<GlobalRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GlobalRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GlobalRankingScore>(
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