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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class BanStatusExt
    {
        public static string CacheParentKey(
            this BanStatus self
        ) {
            return string.Join(
                ":",
                "account",
                "BanStatus"
            );
        }

        public static string CacheKey(
            this BanStatus self,
            string name
        ) {
            return string.Join(
                ":",
                name
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BanStatus> FetchFuture(
            this BanStatus self,
            CacheDatabase cache,
            string name,
            Func<IFuture<BanStatus>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BanStatus> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BanStatus).PutCache(
                            cache,
                            name
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "banStatus") {
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
                    name
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BanStatus>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BanStatus> FetchAsync(
    #else
        public static async Task<BanStatus> FetchAsync(
    #endif
            this BanStatus self,
            CacheDatabase cache,
            string name,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BanStatus>> fetchImpl
    #else
            Func<Task<BanStatus>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BanStatus>(
                       self.CacheParentKey(
                       ),
                       self.CacheKey(
                            name
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        name
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BanStatus).PutCache(
                        cache,
                        name
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "banStatus") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BanStatus, bool> GetCache(
            this BanStatus self,
            CacheDatabase cache,
            string name
        ) {
            return cache.Get<BanStatus>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    name
                )
            );
        }

        public static void PutCache(
            this BanStatus self,
            CacheDatabase cache,
            string name
        ) {
            cache.Put(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    name
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BanStatus self,
            CacheDatabase cache,
            string name
        ) {
            cache.Delete<BanStatus>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    name
                )
            );
        }

        public static void ListSubscribe(
            this BanStatus self,
            CacheDatabase cache,
            Action<BanStatus[]> callback
        ) {
            cache.ListSubscribe<BanStatus>(
                self.CacheParentKey(
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BanStatus self,
            CacheDatabase cache,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BanStatus>(
                self.CacheParentKey(
                ),
                callbackId
            );
        }
    }
}