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
    public static partial class UnusedBalanceExt
    {
        public static string CacheParentKey(
            this UnusedBalance self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "UnusedBalance"
            );
        }

        public static string CacheKey(
            this UnusedBalance self,
            string currency
        ) {
            return string.Join(
                ":",
                currency
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<UnusedBalance> FetchFuture(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            string currency,
            int? timeOffset,
            Func<IFuture<UnusedBalance>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<UnusedBalance> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as UnusedBalance).PutCache(
                            cache,
                            namespaceName,
                            currency,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "unusedBalance") {
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
                    currency,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<UnusedBalance>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<UnusedBalance> FetchAsync(
    #else
        public static async Task<UnusedBalance> FetchAsync(
    #endif
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            string currency,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<UnusedBalance>> fetchImpl
    #else
            Func<Task<UnusedBalance>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    currency,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as UnusedBalance).PutCache(
                    cache,
                    namespaceName,
                    currency,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "unusedBalance") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<UnusedBalance, bool> GetCache(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            string currency,
            int? timeOffset
        ) {
            return cache.Get<UnusedBalance>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    currency
                )
            );
        }

        public static void PutCache(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            string currency,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<UnusedBalance>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
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
                    currency
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            string currency,
            int? timeOffset
        ) {
            cache.Delete<UnusedBalance>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    currency
                )
            );
        }

        public static void ListSubscribe(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<UnusedBalance[]> callback
        ) {
            cache.ListSubscribe<UnusedBalance>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this UnusedBalance self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<UnusedBalance>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}