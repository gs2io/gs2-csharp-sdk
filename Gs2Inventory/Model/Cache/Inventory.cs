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

namespace Gs2.Gs2Inventory.Model.Cache
{
    public static partial class InventoryExt
    {
        public static string CacheParentKey(
            this Inventory self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Inventory"
            );
        }

        public static string CacheKey(
            this Inventory self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Inventory> FetchFuture(
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset,
            Func<IFuture<Inventory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Inventory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Inventory).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "inventory") {
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
                    inventoryName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Inventory>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Inventory> FetchAsync(
    #else
        public static async Task<Inventory> FetchAsync(
    #endif
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Inventory>> fetchImpl
    #else
            Func<Task<Inventory>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    inventoryName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Inventory).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    inventoryName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "inventory") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Inventory, bool> GetCache(
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Inventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Inventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
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
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Inventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Inventory[]> callback
        ) {
            cache.ListSubscribe<Inventory>(
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
            this Inventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Inventory>(
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