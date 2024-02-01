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
    public static partial class SimpleInventoryExt
    {
        public static string CacheParentKey(
            this SimpleInventory self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                userId,
                "SimpleInventory"
            );
        }

        public static string CacheKey(
            this SimpleInventory self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SimpleInventory> FetchFuture(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            Func<IFuture<SimpleInventory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SimpleInventory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SimpleInventory).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "simpleInventory") {
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
                    inventoryName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SimpleInventory>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SimpleInventory> FetchAsync(
    #else
        public static async Task<SimpleInventory> FetchAsync(
    #endif
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SimpleInventory>> fetchImpl
    #else
            Func<Task<SimpleInventory>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SimpleInventory>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
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
                        userId,
                        inventoryName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SimpleInventory).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        inventoryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "simpleInventory") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SimpleInventory, bool> GetCache(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SimpleInventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<SimpleInventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    inventoryName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    inventoryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SimpleInventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<SimpleInventory[]> callback
        ) {
            cache.ListSubscribe<SimpleInventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SimpleInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SimpleInventory>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}