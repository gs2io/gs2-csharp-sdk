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
    public static partial class ClusterRankingBorderExt
    {
        public static string CacheParentKey(
            this ClusterRankingBorder self,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                rankingName,
                clusterName,
                season.ToString(),
                "ClusterRankingBorder"
            );
        }

        public static string CacheKey(
            this ClusterRankingBorder self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ClusterRankingBorder> FetchFuture(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season,
            Func<IFuture<ClusterRankingBorder>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ClusterRankingBorder> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ClusterRankingBorder).PutCache(
                            cache,
                            namespaceName,
                            rankingName,
                            clusterName,
                            season
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "clusterRankingBorder") {
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
                    rankingName,
                    clusterName,
                    season
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ClusterRankingBorder>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ClusterRankingBorder> FetchAsync(
    #else
        public static async Task<ClusterRankingBorder> FetchAsync(
    #endif
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ClusterRankingBorder>> fetchImpl
    #else
            Func<Task<ClusterRankingBorder>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ClusterRankingBorder>(
                       self.CacheParentKey(
                            namespaceName,
                            rankingName,
                            clusterName,
                            season
                       ),
                       self.CacheKey(
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        rankingName,
                        clusterName,
                        season
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ClusterRankingBorder).PutCache(
                        cache,
                        namespaceName,
                        rankingName,
                        clusterName,
                        season
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "clusterRankingBorder") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ClusterRankingBorder, bool> GetCache(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season
        ) {
            return cache.Get<ClusterRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season
        ) {
            cache.Delete<ClusterRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    clusterName,
                    season
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season,
            Action<ClusterRankingBorder[]> callback
        ) {
            cache.ListSubscribe<ClusterRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    clusterName,
                    season
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this ClusterRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            string clusterName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ClusterRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    clusterName,
                    season
                ),
                callbackId
            );
        }
    }
}