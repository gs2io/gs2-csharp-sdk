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
    public static partial class ItemModelMasterExt
    {
        public static string CacheParentKey(
            this ItemModelMaster self,
            string namespaceName,
            string inventoryName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                inventoryName,
                timeOffset?.ToString() ?? "0",
                "ItemModelMaster"
            );
        }

        public static string CacheKey(
            this ItemModelMaster self,
            string itemName
        ) {
            return string.Join(
                ":",
                itemName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ItemModelMaster> FetchFuture(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            int? timeOffset,
            Func<IFuture<ItemModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ItemModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ItemModelMaster).PutCache(
                            cache,
                            namespaceName,
                            inventoryName,
                            itemName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "itemModelMaster") {
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
                    inventoryName,
                    itemName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ItemModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ItemModelMaster> FetchAsync(
    #else
        public static async Task<ItemModelMaster> FetchAsync(
    #endif
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ItemModelMaster>> fetchImpl
    #else
            Func<Task<ItemModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    inventoryName,
                    itemName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as ItemModelMaster).PutCache(
                    cache,
                    namespaceName,
                    inventoryName,
                    itemName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "itemModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<ItemModelMaster, bool> GetCache(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            int? timeOffset
        ) {
            return cache.Get<ItemModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName,
                    timeOffset
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void PutCache(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<ItemModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName,
                    timeOffset
                ),
                self.CacheKey(
                    itemName
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
                    inventoryName,
                    timeOffset
                ),
                self.CacheKey(
                    itemName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            int? timeOffset
        ) {
            cache.Delete<ItemModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName,
                    timeOffset
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void ListSubscribe(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset,
            Action<ItemModelMaster[]> callback
        ) {
            cache.ListSubscribe<ItemModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this ItemModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ItemModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}