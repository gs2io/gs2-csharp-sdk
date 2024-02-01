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
    public static partial class AccountExt
    {
        public static string CacheParentKey(
            this Account self
        ) {
            return string.Join(
                ":",
                "project",
                "Account"
            );
        }

        public static string CacheKey(
            this Account self,
            string accountName
        ) {
            return string.Join(
                ":",
                accountName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Account> FetchFuture(
            this Account self,
            CacheDatabase cache,
            string accountName,
            Func<IFuture<Account>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Account> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Account).PutCache(
                            cache,
                            accountName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "account") {
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
                    accountName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Account>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Account> FetchAsync(
    #else
        public static async Task<Account> FetchAsync(
    #endif
            this Account self,
            CacheDatabase cache,
            string accountName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Account>> fetchImpl
    #else
            Func<Task<Account>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Account>(
                       self.CacheParentKey(
                       ),
                       self.CacheKey(
                            accountName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Account).PutCache(
                        cache,
                        accountName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "account") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Account, bool> GetCache(
            this Account self,
            CacheDatabase cache,
            string accountName
        ) {
            return cache.Get<Account>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    accountName
                )
            );
        }

        public static void PutCache(
            this Account self,
            CacheDatabase cache,
            string accountName
        ) {
            cache.Put(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    accountName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Account self,
            CacheDatabase cache,
            string accountName
        ) {
            cache.Delete<Account>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    accountName
                )
            );
        }

        public static void ListSubscribe(
            this Account self,
            CacheDatabase cache,
            Action<Account[]> callback
        ) {
            cache.ListSubscribe<Account>(
                self.CacheParentKey(
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Account self,
            CacheDatabase cache,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Account>(
                self.CacheParentKey(
                ),
                callbackId
            );
        }
    }
}