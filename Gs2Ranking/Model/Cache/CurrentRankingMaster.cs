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

namespace Gs2.Gs2Ranking.Model.Cache
{
    public static partial class CurrentRankingMasterExt
    {
        public static string CacheParentKey(
            this CurrentRankingMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "ranking",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "CurrentRankingMaster"
            );
        }

        public static string CacheKey(
            this CurrentRankingMaster self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CurrentRankingMaster> FetchFuture(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Func<IFuture<CurrentRankingMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CurrentRankingMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CurrentRankingMaster).PutCache(
                            cache,
                            namespaceName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "currentRankingMaster") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CurrentRankingMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CurrentRankingMaster> FetchAsync(
    #else
        public static async Task<CurrentRankingMaster> FetchAsync(
    #endif
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CurrentRankingMaster>> fetchImpl
    #else
            Func<Task<CurrentRankingMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as CurrentRankingMaster).PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "currentRankingMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<CurrentRankingMaster, bool> GetCache(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            return cache.Get<CurrentRankingMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Delete<CurrentRankingMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<CurrentRankingMaster[]> callback
        ) {
            cache.ListSubscribe<CurrentRankingMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this CurrentRankingMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CurrentRankingMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}