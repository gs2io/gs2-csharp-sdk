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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class SalesItemMasterExt
    {
        public static string CacheParentKey(
            this SalesItemMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "SalesItemMaster"
            );
        }

        public static string CacheKey(
            this SalesItemMaster self,
            string salesItemName
        ) {
            return string.Join(
                ":",
                salesItemName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SalesItemMaster> FetchFuture(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemName,
            int? timeOffset,
            Func<IFuture<SalesItemMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SalesItemMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SalesItemMaster).PutCache(
                            cache,
                            namespaceName,
                            salesItemName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "salesItemMaster") {
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
                    salesItemName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SalesItemMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SalesItemMaster> FetchAsync(
    #else
        public static async Task<SalesItemMaster> FetchAsync(
    #endif
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SalesItemMaster>> fetchImpl
    #else
            Func<Task<SalesItemMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    salesItemName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SalesItemMaster).PutCache(
                    cache,
                    namespaceName,
                    salesItemName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "salesItemMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<SalesItemMaster, bool> GetCache(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemName,
            int? timeOffset
        ) {
            return cache.Get<SalesItemMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    salesItemName
                )
            );
        }

        public static void PutCache(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<SalesItemMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    salesItemName
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
                    salesItemName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemName,
            int? timeOffset
        ) {
            cache.Delete<SalesItemMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    salesItemName
                )
            );
        }

        public static void ListSubscribe(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<SalesItemMaster[]> callback
        ) {
            cache.ListSubscribe<SalesItemMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this SalesItemMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SalesItemMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}