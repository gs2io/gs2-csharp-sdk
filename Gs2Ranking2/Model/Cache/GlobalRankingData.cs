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
    public static partial class GlobalRankingDataExt
    {
        public static string CacheParentKey(
            this GlobalRankingData self,
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
                "GlobalRankingData"
            );
        }

        public static string CacheKey(
            this GlobalRankingData self,
            string userId
        ) {
            return string.Join(
                ":",
                userId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GlobalRankingData> FetchFuture(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            Func<IFuture<GlobalRankingData>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GlobalRankingData> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GlobalRankingData).PutCache(
                            cache,
                            namespaceName,
                            rankingName,
                            season,
                            userId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "globalRankingData") {
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
                    season,
                    userId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GlobalRankingData>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GlobalRankingData> FetchAsync(
    #else
        public static async Task<GlobalRankingData> FetchAsync(
    #endif
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GlobalRankingData>> fetchImpl
    #else
            Func<Task<GlobalRankingData>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GlobalRankingData>(
                       self.CacheParentKey(
                            namespaceName,
                            rankingName,
                            season
                       ),
                       self.CacheKey(
                            userId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        rankingName,
                        season,
                        userId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GlobalRankingData).PutCache(
                        cache,
                        namespaceName,
                        rankingName,
                        season,
                        userId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "globalRankingData") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GlobalRankingData, bool> GetCache(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<GlobalRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    userId
                )
            );
        }

        public static void PutCache(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<GlobalRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    userId
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    userId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<GlobalRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    userId
                )
            );
        }

        public static void ListSubscribe(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            Action<GlobalRankingData[]> callback
        ) {
            cache.ListSubscribe<GlobalRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    rankingName,
                    season
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GlobalRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GlobalRankingData>(
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