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

namespace Gs2.Gs2Money2.Model.Cache
{
    public static partial class DailyTransactionHistoryExt
    {
        public static string CacheParentKey(
            this DailyTransactionHistory self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "DailyTransactionHistory"
            );
        }

        public static string CacheKey(
            this DailyTransactionHistory self,
            int? year,
            int? month,
            int? day,
            string currency
        ) {
            return string.Join(
                ":",
                year.ToString(),
                month.ToString(),
                day.ToString(),
                currency
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DailyTransactionHistory> FetchFuture(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? year,
            int? month,
            int? day,
            string currency,
            int? timeOffset,
            Func<IFuture<DailyTransactionHistory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DailyTransactionHistory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DailyTransactionHistory).PutCache(
                            cache,
                            namespaceName,
                            year,
                            month,
                            day,
                            currency,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "dailyTransactionHistory") {
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
                    year,
                    month,
                    day,
                    currency,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DailyTransactionHistory>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DailyTransactionHistory> FetchAsync(
    #else
        public static async Task<DailyTransactionHistory> FetchAsync(
    #endif
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? year,
            int? month,
            int? day,
            string currency,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DailyTransactionHistory>> fetchImpl
    #else
            Func<Task<DailyTransactionHistory>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    year,
                    month,
                    day,
                    currency,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as DailyTransactionHistory).PutCache(
                    cache,
                    namespaceName,
                    year,
                    month,
                    day,
                    currency,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "dailyTransactionHistory") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<DailyTransactionHistory, bool> GetCache(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? year,
            int? month,
            int? day,
            string currency,
            int? timeOffset
        ) {
            return cache.Get<DailyTransactionHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    year,
                    month,
                    day,
                    currency
                )
            );
        }

        public static void PutCache(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? year,
            int? month,
            int? day,
            string currency,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<DailyTransactionHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    year,
                    month,
                    day,
                    currency
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
                    timeOffset
                ),
                self.CacheKey(
                    year,
                    month,
                    day,
                    currency
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? year,
            int? month,
            int? day,
            string currency,
            int? timeOffset
        ) {
            cache.Delete<DailyTransactionHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    year,
                    month,
                    day,
                    currency
                )
            );
        }

        public static void ListSubscribe(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<DailyTransactionHistory[]> callback
        ) {
            cache.ListSubscribe<DailyTransactionHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this DailyTransactionHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DailyTransactionHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}