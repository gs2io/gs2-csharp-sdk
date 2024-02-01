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
    public static partial class SimpleInventoryModelExt
    {
        public static string CacheParentKey(
            this SimpleInventoryModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                "SimpleInventoryModel"
            );
        }

        public static string CacheKey(
            this SimpleInventoryModel self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SimpleInventoryModel> FetchFuture(
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            Func<IFuture<SimpleInventoryModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SimpleInventoryModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SimpleInventoryModel).PutCache(
                            cache,
                            namespaceName,
                            inventoryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "simpleInventoryModel") {
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
            return new Gs2InlineFuture<SimpleInventoryModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SimpleInventoryModel> FetchAsync(
    #else
        public static async Task<SimpleInventoryModel> FetchAsync(
    #endif
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SimpleInventoryModel>> fetchImpl
    #else
            Func<Task<SimpleInventoryModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SimpleInventoryModel>(
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
                    (null as SimpleInventoryModel).PutCache(
                        cache,
                        namespaceName,
                        inventoryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "simpleInventoryModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SimpleInventoryModel, bool> GetCache(
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            return cache.Get<SimpleInventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void PutCache(
            this SimpleInventoryModel self,
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
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName
        ) {
            cache.Delete<SimpleInventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    inventoryName
                )
            );
        }

        public static void ListSubscribe(
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<SimpleInventoryModel[]> callback
        ) {
            cache.ListSubscribe<SimpleInventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SimpleInventoryModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SimpleInventoryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}