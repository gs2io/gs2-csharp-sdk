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
    public static partial class BillingExt
    {
        public static string CacheParentKey(
            this Billing self,
            string accountName,
            string projectName
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                projectName,
                "Billing"
            );
        }

        public static string CacheKey(
            this Billing self,
            int? year,
            int? month
        ) {
            return string.Join(
                ":",
                year.ToString(),
                month.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Billing> FetchFuture(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? year,
            int? month,
            Func<IFuture<Billing>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Billing> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Billing).PutCache(
                            cache,
                            accountName,
                            projectName,
                            year,
                            month
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "billing") {
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
                    projectName,
                    year,
                    month
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Billing>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Billing> FetchAsync(
    #else
        public static async Task<Billing> FetchAsync(
    #endif
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? year,
            int? month,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Billing>> fetchImpl
    #else
            Func<Task<Billing>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Billing>(
                       self.CacheParentKey(
                            accountName,
                            projectName
                       ),
                       self.CacheKey(
                            year,
                            month
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName,
                        projectName,
                        year,
                        month
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Billing).PutCache(
                        cache,
                        accountName,
                        projectName,
                        year,
                        month
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "billing") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Billing, bool> GetCache(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? year,
            int? month
        ) {
            return cache.Get<Billing>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    year,
                    month
                )
            );
        }

        public static void PutCache(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? year,
            int? month
        ) {
            cache.Put(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    year,
                    month
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? year,
            int? month
        ) {
            cache.Delete<Billing>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    year,
                    month
                )
            );
        }

        public static void ListSubscribe(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            Action<Billing[]> callback
        ) {
            cache.ListSubscribe<Billing>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Billing self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Billing>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                callbackId
            );
        }
    }
}