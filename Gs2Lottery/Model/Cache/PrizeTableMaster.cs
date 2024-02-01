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
    public static partial class PrizeTableMasterExt
    {
        public static string CacheParentKey(
            this PrizeTableMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                "PrizeTableMaster"
            );
        }

        public static string CacheKey(
            this PrizeTableMaster self,
            string prizeTableName
        ) {
            return string.Join(
                ":",
                prizeTableName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PrizeTableMaster> FetchFuture(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
            Func<IFuture<PrizeTableMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PrizeTableMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PrizeTableMaster).PutCache(
                            cache,
                            namespaceName,
                            prizeTableName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "prizeTableMaster") {
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
                    prizeTableName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PrizeTableMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PrizeTableMaster> FetchAsync(
    #else
        public static async Task<PrizeTableMaster> FetchAsync(
    #endif
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PrizeTableMaster>> fetchImpl
    #else
            Func<Task<PrizeTableMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PrizeTableMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            prizeTableName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        prizeTableName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PrizeTableMaster).PutCache(
                        cache,
                        namespaceName,
                        prizeTableName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "prizeTableMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PrizeTableMaster, bool> GetCache(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName
        ) {
            return cache.Get<PrizeTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    prizeTableName
                )
            );
        }

        public static void PutCache(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName
        ) {
            var (value, find) = cache.Get<PrizeTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    prizeTableName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    prizeTableName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string prizeTableName
        ) {
            cache.Delete<PrizeTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    prizeTableName
                )
            );
        }

        public static void ListSubscribe(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<PrizeTableMaster[]> callback
        ) {
            cache.ListSubscribe<PrizeTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PrizeTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PrizeTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}