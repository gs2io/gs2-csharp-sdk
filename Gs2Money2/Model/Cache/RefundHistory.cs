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
    public static partial class RefundHistoryExt
    {
        public static string CacheParentKey(
            this RefundHistory self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "RefundHistory"
            );
        }

        public static string CacheKey(
            this RefundHistory self,
            string transactionId
        ) {
            return string.Join(
                ":",
                transactionId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RefundHistory> FetchFuture(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            string transactionId,
            int? timeOffset,
            Func<IFuture<RefundHistory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RefundHistory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RefundHistory).PutCache(
                            cache,
                            namespaceName,
                            transactionId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "refundHistory") {
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
                    transactionId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RefundHistory>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RefundHistory> FetchAsync(
    #else
        public static async Task<RefundHistory> FetchAsync(
    #endif
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            string transactionId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RefundHistory>> fetchImpl
    #else
            Func<Task<RefundHistory>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    transactionId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as RefundHistory).PutCache(
                    cache,
                    namespaceName,
                    transactionId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "refundHistory") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<RefundHistory, bool> GetCache(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            string transactionId,
            int? timeOffset
        ) {
            return cache.Get<RefundHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void PutCache(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            string transactionId,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            string transactionId,
            int? timeOffset
        ) {
            cache.Delete<RefundHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void ListSubscribe(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<RefundHistory[]> callback
        ) {
            cache.ListSubscribe<RefundHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this RefundHistory self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RefundHistory>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}