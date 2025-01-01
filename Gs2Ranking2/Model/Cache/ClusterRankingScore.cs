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
    public static partial class ClusterRankingScoreExt
    {
        public static string CacheParentKey(
            this ClusterRankingScore self,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                rankingName,
                clusterName,
                season,
                "ClusterRankingScore"
            );
        }

        public static string CacheKey(
            this ClusterRankingScore self,
            string rankingName,
            string clusterName,
            long? season
        ) {
            return string.Join(
                ":",
                rankingName,
                clusterName,
                season.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ClusterRankingScore> FetchFuture(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
            Func<IFuture<ClusterRankingScore>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ClusterRankingScore> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ClusterRankingScore).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName,
                            clusterName,
                            season
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "clusterRankingScore") {
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
                    clusterName,
                    season
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ClusterRankingScore>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ClusterRankingScore> FetchAsync(
    #else
        public static async Task<ClusterRankingScore> FetchAsync(
    #endif
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ClusterRankingScore>> fetchImpl
    #else
            Func<Task<ClusterRankingScore>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ClusterRankingScore>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            rankingName,
                            clusterName,
                            season
                       ),
                       self.CacheKey(
                            rankingName,
                            clusterName,
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
                        clusterName,
                        season
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ClusterRankingScore).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        clusterName,
                        season
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "clusterRankingScore") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ClusterRankingScore, bool> GetCache(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                )
            );
        }

        public static void PutCache(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
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
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    null
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    null
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                )
            );
            cache.Delete<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    null
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    null
                )
            );
        }

        public static void ListSubscribe(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
            Action<ClusterRankingScore[]> callback
        ) {
            cache.ListSubscribe<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                ),
                callback
            );
            cache.ListSubscribe<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    null
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ClusterRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                ),
                callbackId
            );
            cache.ListUnsubscribe<ClusterRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    null
                ),
                callbackId
            );
        }
    }
}