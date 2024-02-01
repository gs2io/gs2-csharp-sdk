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
    public static partial class BigItemModelExt
    {
        public static string CacheParentKey(
            this BigItemModel self,
            string namespaceName,
            string inventoryName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                inventoryName,
                "BigItemModel"
            );
        }

        public static string CacheKey(
            this BigItemModel self,
            string itemName
        ) {
            return string.Join(
                ":",
                itemName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BigItemModel> FetchFuture(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
            Func<IFuture<BigItemModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BigItemModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BigItemModel).PutCache(
                            cache,
                            namespaceName,
                            inventoryName,
                            itemName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "bigItemModel") {
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
                    itemName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BigItemModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BigItemModel> FetchAsync(
    #else
        public static async Task<BigItemModel> FetchAsync(
    #endif
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BigItemModel>> fetchImpl
    #else
            Func<Task<BigItemModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BigItemModel>(
                       self.CacheParentKey(
                            namespaceName,
                            inventoryName
                       ),
                       self.CacheKey(
                            itemName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        inventoryName,
                        itemName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BigItemModel).PutCache(
                        cache,
                        namespaceName,
                        inventoryName,
                        itemName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "bigItemModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BigItemModel, bool> GetCache(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName
        ) {
            return cache.Get<BigItemModel>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void PutCache(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            string itemName
        ) {
            cache.Delete<BigItemModel>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void ListSubscribe(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            Action<BigItemModel[]> callback
        ) {
            cache.ListSubscribe<BigItemModel>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BigItemModel self,
            CacheDatabase cache,
            string namespaceName,
            string inventoryName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BigItemModel>(
                self.CacheParentKey(
                    namespaceName,
                    inventoryName
                ),
                callbackId
            );
        }
    }
}