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

namespace Gs2.Gs2Lottery.Model.Cache
{
    public static partial class PrizeLimitExt
    {
        public static string CacheParentKey(
            this PrizeLimit self,
            string namespaceName,
            string prizeTableName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                prizeTableName,
                "PrizeLimit"
            );
        }

        public static string CacheKey(
            this PrizeLimit self,
            string prizeId
        ) {
            return string.Join(
                ":",
                prizeId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PrizeLimit> FetchFuture(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            string prizeId,
            Func<IFuture<PrizeLimit>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PrizeLimit> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PrizeLimit).PutCache(
                            cache,
                            namespaceName,
                            prizeTableName,
                            prizeId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "prizeLimit") {
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
                    prizeTableName,
                    prizeId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PrizeLimit>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PrizeLimit> FetchAsync(
    #else
        public static async Task<PrizeLimit> FetchAsync(
    #endif
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            string prizeId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PrizeLimit>> fetchImpl
    #else
            Func<Task<PrizeLimit>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PrizeLimit>(
                       self.CacheParentKey(
                            namespaceName,
                            prizeTableName
                       ),
                       self.CacheKey(
                            prizeId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        prizeTableName,
                        prizeId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PrizeLimit).PutCache(
                        cache,
                        namespaceName,
                        prizeTableName,
                        prizeId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "prizeLimit") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PrizeLimit, bool> GetCache(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            string prizeId
        ) {
            return cache.Get<PrizeLimit>(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                self.CacheKey(
                    prizeId
                )
            );
        }

        public static void PutCache(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            string prizeId
        ) {
            var (value, find) = cache.Get<PrizeLimit>(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                self.CacheKey(
                    prizeId
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                self.CacheKey(
                    prizeId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            string prizeId
        ) {
            cache.Delete<PrizeLimit>(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                self.CacheKey(
                    prizeId
                )
            );
        }

        public static void ListSubscribe(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            Action<PrizeLimit[]> callback
        ) {
            cache.ListSubscribe<PrizeLimit>(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PrizeLimit self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PrizeLimit>(
                self.CacheParentKey(
                    namespaceName,
                    prizeTableName
                ),
                callbackId
            );
        }
    }
}