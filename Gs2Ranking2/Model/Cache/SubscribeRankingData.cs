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
    public static partial class SubscribeRankingDataExt
    {
        public static string CacheParentKey(
            this SubscribeRankingData self,
            string namespaceName,
            string userId,
            string rankingName,
            long? season
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                rankingName,
                season,
                "SubscribeRankingData"
            );
        }

        public static string CacheKey(
            this SubscribeRankingData self,
            string scorerUserId
        ) {
            return string.Join(
                ":",
                scorerUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeRankingData> FetchFuture(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            string scorerUserId,
            Func<IFuture<SubscribeRankingData>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeRankingData> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeRankingData).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName,
                            season,
                            scorerUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeRankingData") {
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
                    season,
                    scorerUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscribeRankingData>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeRankingData> FetchAsync(
    #else
        public static async Task<SubscribeRankingData> FetchAsync(
    #endif
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            string scorerUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeRankingData>> fetchImpl
    #else
            Func<Task<SubscribeRankingData>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SubscribeRankingData>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            rankingName,
                            season
                       ),
                       self.CacheKey(
                            scorerUserId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        season,
                        scorerUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SubscribeRankingData).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName,
                        season,
                        scorerUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "subscribeRankingData") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SubscribeRankingData, bool> GetCache(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            string scorerUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SubscribeRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    scorerUserId
                )
            );
        }

        public static void PutCache(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            string scorerUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<SubscribeRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    scorerUserId
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
                    season
                ),
                self.CacheKey(
                    scorerUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            string scorerUserId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SubscribeRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    season
                ),
                self.CacheKey(
                    scorerUserId
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            Action<SubscribeRankingData[]> callback
        ) {
            cache.ListSubscribe<SubscribeRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    season
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SubscribeRankingData self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            long? season,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeRankingData>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    season
                ),
                callbackId
            );
        }
    }
}