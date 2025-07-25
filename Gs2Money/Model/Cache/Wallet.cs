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
 *
 * deny overwrite
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

namespace Gs2.Gs2Money.Model.Cache
{
    public static partial class WalletExt
    {
        public static string CacheParentKey(
            this Wallet self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Wallet"
            );
        }

        public static string CacheKey(
            this Wallet self,
            int? slot
        ) {
            return string.Join(
                ":",
                slot.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Wallet> FetchFuture(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset,
            Func<IFuture<Wallet>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Wallet> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Wallet).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            slot,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "wallet") {
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
                    userId,
                    slot,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Wallet>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Wallet> FetchAsync(
    #else
        public static async Task<Wallet> FetchAsync(
    #endif
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Wallet>> fetchImpl
    #else
            Func<Task<Wallet>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    slot,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Wallet).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    slot,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "wallet") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Wallet, bool> GetCache(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Wallet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    slot
                )
            );
        }

        public static void PutCache(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Wallet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    slot
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    slot
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
            if ((self?.ShareFree ?? false) && (slot ?? 0) == 0) {
                cache.ClearListCache<Wallet>(
                    self.CacheParentKey(
                        namespaceName,
                        userId,
                        timeOffset
                    )
                );
            }
        }

        public static void DeleteCache(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? slot,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Wallet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    slot
                )
            );
        }

        public static void ListSubscribe(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Wallet[]> callback
        ) {
            cache.ListSubscribe<Wallet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Wallet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Wallet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}