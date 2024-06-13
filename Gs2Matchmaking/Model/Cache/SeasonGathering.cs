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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static partial class SeasonGatheringExt
    {
        public static string CacheParentKey(
            this SeasonGathering self,
            string namespaceName,
            string userId,
            string seasonName,
            long? season
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                "Singleton",
                seasonName,
                season.ToString(),
                "SeasonGathering"
            );
        }

        public static string CacheKey(
            this SeasonGathering self,
            long? tier,
            string seasonGatheringName
        ) {
            return string.Join(
                ":",
                tier.ToString(),
                seasonGatheringName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SeasonGathering> FetchFuture(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName,
            Func<IFuture<SeasonGathering>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SeasonGathering> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SeasonGathering).PutCache(
                            cache,
                            namespaceName,
                            "Singleton",
                            seasonName,
                            season,
                            tier,
                            seasonGatheringName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "seasonGathering") {
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
                    "Singleton",
                    seasonName,
                    season,
                    tier,
                    seasonGatheringName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SeasonGathering>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SeasonGathering> FetchAsync(
    #else
        public static async Task<SeasonGathering> FetchAsync(
    #endif
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SeasonGathering>> fetchImpl
    #else
            Func<Task<SeasonGathering>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SeasonGathering>(
                       self.CacheParentKey(
                            namespaceName,
                            "Singleton",
                            seasonName,
                            season
                       ),
                       self.CacheKey(
                            tier,
                            seasonGatheringName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        "Singleton",
                        seasonName,
                        season,
                        tier,
                        seasonGatheringName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SeasonGathering).PutCache(
                        cache,
                        namespaceName,
                        "Singleton",
                        seasonName,
                        season,
                        tier,
                        seasonGatheringName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "seasonGathering") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SeasonGathering, bool> GetCache(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName
        ) {
            return cache.Get<SeasonGathering>(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                self.CacheKey(
                    tier,
                    seasonGatheringName
                )
            );
        }

        public static void PutCache(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName
        ) {
            var (value, find) = cache.Get<SeasonGathering>(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                self.CacheKey(
                    tier,
                    seasonGatheringName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                self.CacheKey(
                    tier,
                    seasonGatheringName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            long? tier,
            string seasonGatheringName
        ) {
            cache.Delete<SeasonGathering>(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                self.CacheKey(
                    tier,
                    seasonGatheringName
                )
            );
        }

        public static void ListSubscribe(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            Action<SeasonGathering[]> callback
        ) {
            cache.ListSubscribe<SeasonGathering>(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SeasonGathering self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SeasonGathering>(
                self.CacheParentKey(
                    namespaceName,
                    "Singleton",
                    seasonName,
                    season
                ),
                callbackId
            );
        }
    }
}