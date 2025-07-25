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

#pragma warning disable CS0618 // Obsolete with a message
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
    public static partial class SubscribeRankingScoreExt
    {
        public static string CacheParentKey(
            this SubscribeRankingScore self,
            string namespaceName,
            string userId,
            string rankingName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                rankingName,
                timeOffset?.ToString() ?? "0",
                "SubscribeRankingScore"
            );
        }

        public static string CacheKey(
            this SubscribeRankingScore self,
            string rankingName,
            long? season,
            string userId
        ) {
            return string.Join(
                ":",
                rankingName,
                season.ToString(),
                userId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeRankingScore> FetchFuture(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            int? timeOffset,
            Func<IFuture<SubscribeRankingScore>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeRankingScore> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeRankingScore).PutCache(
                            cache,
                            namespaceName,
                            rankingName,
                            season,
                            userId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeRankingScore") {
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
                    userId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscribeRankingScore>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeRankingScore> FetchAsync(
    #else
        public static async Task<SubscribeRankingScore> FetchAsync(
    #endif
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeRankingScore>> fetchImpl
    #else
            Func<Task<SubscribeRankingScore>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    rankingName,
                    season,
                    userId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SubscribeRankingScore).PutCache(
                    cache,
                    namespaceName,
                    rankingName,
                    season,
                    userId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "subscribeRankingScore") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<SubscribeRankingScore, bool> GetCache(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SubscribeRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                self.CacheKey(
                    rankingName,
                    season,
                    userId
                )
            );
        }

        public static void PutCache(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<SubscribeRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                self.CacheKey(
                    rankingName,
                    season,
                    userId
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                self.CacheKey(
                    rankingName,
                    season,
                    userId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            long? season,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SubscribeRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                self.CacheKey(
                    rankingName,
                    season,
                    userId
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            int? timeOffset,
            Action<SubscribeRankingScore[]> callback
        ) {
            cache.ListSubscribe<SubscribeRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this SubscribeRankingScore self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeRankingScore>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    rankingName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}