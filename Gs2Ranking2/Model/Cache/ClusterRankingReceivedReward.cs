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
    public static partial class ClusterRankingReceivedRewardExt
    {
        public static string CacheParentKey(
            this ClusterRankingReceivedReward self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                "ClusterRankingReceivedReward"
            );
        }

        public static string CacheKey(
            this ClusterRankingReceivedReward self,
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
        public static IFuture<ClusterRankingReceivedReward> FetchFuture(
            this ClusterRankingReceivedReward self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
            Func<IFuture<ClusterRankingReceivedReward>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ClusterRankingReceivedReward> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ClusterRankingReceivedReward).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName,
                            clusterName,
                            season
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "clusterRankingReceivedReward") {
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
            return new Gs2InlineFuture<ClusterRankingReceivedReward>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ClusterRankingReceivedReward> FetchAsync(
    #else
        public static async Task<ClusterRankingReceivedReward> FetchAsync(
    #endif
            this ClusterRankingReceivedReward self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            string clusterName,
            long? season,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ClusterRankingReceivedReward>> fetchImpl
    #else
            Func<Task<ClusterRankingReceivedReward>> fetchImpl
    #endif
        ) {
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
                (null as ClusterRankingReceivedReward).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    rankingName,
                    clusterName,
                    season
                );
                if (e.errors.Length == 0 || e.errors[0].component != "clusterRankingReceivedReward") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<ClusterRankingReceivedReward, bool> GetCache(
            this ClusterRankingReceivedReward self,
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
            return cache.Get<ClusterRankingReceivedReward>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                )
            );
        }

        public static void PutCache(
            this ClusterRankingReceivedReward self,
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
            var (value, find) = cache.Get<ClusterRankingReceivedReward>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
                    userId
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ClusterRankingReceivedReward self,
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
            cache.Delete<ClusterRankingReceivedReward>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    rankingName,
                    clusterName,
                    season
                )
            );
        }

        public static void ListSubscribe(
            this ClusterRankingReceivedReward self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<ClusterRankingReceivedReward[]> callback
        ) {
            cache.ListSubscribe<ClusterRankingReceivedReward>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this ClusterRankingReceivedReward self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ClusterRankingReceivedReward>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}