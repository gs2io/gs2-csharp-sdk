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

namespace Gs2.Gs2Project.Model.Cache
{
    public static partial class ReceiptExt
    {
        public static string CacheParentKey(
            this Receipt self,
            string accountName
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                "Receipt"
            );
        }

        public static string CacheKey(
            this Receipt self,
            string receiptName
        ) {
            return string.Join(
                ":",
                receiptName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Receipt> FetchFuture(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            string receiptName,
            Func<IFuture<Receipt>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Receipt> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Receipt).PutCache(
                            cache,
                            accountName,
                            receiptName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "receipt") {
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
                    accountName,
                    receiptName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Receipt>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Receipt> FetchAsync(
    #else
        public static async Task<Receipt> FetchAsync(
    #endif
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            string receiptName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Receipt>> fetchImpl
    #else
            Func<Task<Receipt>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Receipt>(
                       self.CacheParentKey(
                            accountName
                       ),
                       self.CacheKey(
                            receiptName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName,
                        receiptName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Receipt).PutCache(
                        cache,
                        accountName,
                        receiptName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "receipt") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Receipt, bool> GetCache(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            string receiptName
        ) {
            return cache.Get<Receipt>(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    receiptName
                )
            );
        }

        public static void PutCache(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            string receiptName
        ) {
            cache.Put(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    receiptName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            string receiptName
        ) {
            cache.Delete<Receipt>(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    receiptName
                )
            );
        }

        public static void ListSubscribe(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            Action<Receipt[]> callback
        ) {
            cache.ListSubscribe<Receipt>(
                self.CacheParentKey(
                    accountName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Receipt self,
            CacheDatabase cache,
            string accountName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Receipt>(
                self.CacheParentKey(
                    accountName
                ),
                callbackId
            );
        }
    }
}