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

namespace Gs2.Gs2Inventory.Model.Cache
{
    public static partial class InventoryModelExt
    {
        public static string CacheParentKey(
            this InventoryModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                "InventoryModel"
            );
        }

        public static string CacheKey(
            this InventoryModel self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<InventoryModel> FetchFuture(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            Func<IFuture<InventoryModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<InventoryModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as InventoryModel).PutCache(
                            cache,
                            namespaceName,
                            inventoryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "inventoryModel") {
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
                    inventoryName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<InventoryModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<InventoryModel> FetchAsync(
    #else
        public static async Task<InventoryModel> FetchAsync(
    #endif
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<InventoryModel>> fetchImpl
    #else
            Func<Task<InventoryModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<InventoryModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            inventoryName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        inventoryName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as InventoryModel).PutCache(
                        cache,
                        namespaceName,
                        inventoryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "inventoryModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<InventoryModel, bool> GetCache(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            return cache.Get<InventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            cache.Delete<InventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<InventoryModel[]> callback
        ) {
            cache.ListSubscribe<InventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this InventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<InventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}