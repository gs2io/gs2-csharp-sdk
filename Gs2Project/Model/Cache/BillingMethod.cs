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
    public static partial class BillingMethodExt
    {
        public static string CacheParentKey(
            this BillingMethod self,
            string accountName
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                "BillingMethod"
            );
        }

        public static string CacheKey(
            this BillingMethod self,
            string billingMethodName
        ) {
            return string.Join(
                ":",
                billingMethodName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BillingMethod> FetchFuture(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            string billingMethodName,
            Func<IFuture<BillingMethod>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BillingMethod> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BillingMethod).PutCache(
                            cache,
                            accountName,
                            billingMethodName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "billingMethod") {
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
                    billingMethodName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BillingMethod>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BillingMethod> FetchAsync(
    #else
        public static async Task<BillingMethod> FetchAsync(
    #endif
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            string billingMethodName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BillingMethod>> fetchImpl
    #else
            Func<Task<BillingMethod>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BillingMethod>(
                       self.CacheParentKey(
                            accountName
                       ),
                       self.CacheKey(
                            billingMethodName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName,
                        billingMethodName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BillingMethod).PutCache(
                        cache,
                        accountName,
                        billingMethodName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "billingMethod") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BillingMethod, bool> GetCache(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            string billingMethodName
        ) {
            return cache.Get<BillingMethod>(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    billingMethodName
                )
            );
        }

        public static void PutCache(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            string billingMethodName
        ) {
            cache.Put(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    billingMethodName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            string billingMethodName
        ) {
            cache.Delete<BillingMethod>(
                self.CacheParentKey(
                    accountName
                ),
                self.CacheKey(
                    billingMethodName
                )
            );
        }

        public static void ListSubscribe(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            Action<BillingMethod[]> callback
        ) {
            cache.ListSubscribe<BillingMethod>(
                self.CacheParentKey(
                    accountName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BillingMethod self,
            CacheDatabase cache,
            string accountName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BillingMethod>(
                self.CacheParentKey(
                    accountName
                ),
                callbackId
            );
        }
    }
}