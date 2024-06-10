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
    public static partial class GlobalRankingBorderExt
    {
        public static string CacheParentKey(
            this GlobalRankingBorder self,
            string namespaceName,
            string rankingName,
            long? season
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                rankingName,
                season.ToString(),
                "GlobalRankingBorder"
            );
        }

        public static string CacheKey(
            this GlobalRankingBorder self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GlobalRankingBorder> FetchFuture(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            Func<IFuture<GlobalRankingBorder>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GlobalRankingBorder> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GlobalRankingBorder).PutCache(
                            cache,
                            namespaceName,
                            rankingName,
                            season
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "globalRankingBorder") {
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
                    season
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GlobalRankingBorder>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GlobalRankingBorder> FetchAsync(
    #else
        public static async Task<GlobalRankingBorder> FetchAsync(
    #endif
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GlobalRankingBorder>> fetchImpl
    #else
            Func<Task<GlobalRankingBorder>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GlobalRankingBorder>(
                       self.CacheParentKey(
                            namespaceName,
                            rankingName,
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
                        season
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GlobalRankingBorder).PutCache(
                        cache,
                        namespaceName,
                        rankingName,
                        season
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "globalRankingBorder") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GlobalRankingBorder, bool> GetCache(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season
        ) {
            return cache.Get<GlobalRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season
        ) {
            cache.Delete<GlobalRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            Action<GlobalRankingBorder[]> callback
        ) {
            cache.ListSubscribe<GlobalRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GlobalRankingBorder self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GlobalRankingBorder>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                callbackId
            );
        }
    }
}