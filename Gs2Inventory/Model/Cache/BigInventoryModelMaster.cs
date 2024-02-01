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
    public static partial class BigInventoryModelMasterExt
    {
        public static string CacheParentKey(
            this BigInventoryModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                "BigInventoryModelMaster"
            );
        }

        public static string CacheKey(
            this BigInventoryModelMaster self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BigInventoryModelMaster> FetchFuture(
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            Func<IFuture<BigInventoryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BigInventoryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BigInventoryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            inventoryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "bigInventoryModelMaster") {
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
            return new Gs2InlineFuture<BigInventoryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BigInventoryModelMaster> FetchAsync(
    #else
        public static async Task<BigInventoryModelMaster> FetchAsync(
    #endif
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BigInventoryModelMaster>> fetchImpl
    #else
            Func<Task<BigInventoryModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BigInventoryModelMaster>(
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
                    (null as BigInventoryModelMaster).PutCache(
                        cache,
                        namespaceName,
                        inventoryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "bigInventoryModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BigInventoryModelMaster, bool> GetCache(
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            return cache.Get<BigInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            var (value, find) = cache.Get<BigInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
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
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            cache.Delete<BigInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<BigInventoryModelMaster[]> callback
        ) {
            cache.ListSubscribe<BigInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BigInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BigInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}