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
    public static partial class SimpleInventoryModelMasterExt
    {
        public static string CacheParentKey(
            this SimpleInventoryModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "SimpleInventoryModelMaster"
            );
        }

        public static string CacheKey(
            this SimpleInventoryModelMaster self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SimpleInventoryModelMaster> FetchFuture(
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset,
            Func<IFuture<SimpleInventoryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SimpleInventoryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SimpleInventoryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            inventoryName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "simpleInventoryModelMaster") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SimpleInventoryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SimpleInventoryModelMaster> FetchAsync(
    #else
        public static async Task<SimpleInventoryModelMaster> FetchAsync(
    #endif
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SimpleInventoryModelMaster>> fetchImpl
    #else
            Func<Task<SimpleInventoryModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    inventoryName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SimpleInventoryModelMaster).PutCache(
                    cache,
                    namespaceName,
                    inventoryName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "simpleInventoryModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<SimpleInventoryModelMaster, bool> GetCache(
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset
        ) {
            return cache.Get<SimpleInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<SimpleInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName,
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
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            int? timeOffset
        ) {
            cache.Delete<SimpleInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<SimpleInventoryModelMaster[]> callback
        ) {
            cache.ListSubscribe<SimpleInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this SimpleInventoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SimpleInventoryModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}