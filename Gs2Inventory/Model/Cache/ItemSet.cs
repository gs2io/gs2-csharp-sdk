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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
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
    public static partial class ItemSetExt
    {
        public static string CacheParentKey(
            this ItemSet self,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                userId,
                inventoryName,
                "ItemSet"
            );
        }

        public static string CacheKey(
            this ItemSet self,
            string itemName,
            string itemSetName
        ) {
            return string.Join(
                ":",
                itemName,
                itemSetName
            );
        }
        
        public static string CacheParentKey(
            this ItemSet[] self,
            string namespaceName,
            string userId,
            string inventoryName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                userId,
                inventoryName,
                "ItemSet"
            );
        }

        public static string CacheKey(
            this ItemSet[] self,
            string itemName
        ) {
            return string.Join(
                ":",
                itemName,
                "any"
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ItemSet[]> FetchFuture(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            Func<IFuture<ItemSet[]>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ItemSet[]> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ItemSet).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName,
                            itemSetName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "itemSet") {
                            self.OnComplete(Array.Empty<ItemSet>());
                            yield break;
                        }
                    }
                    self.OnError(future.Error);
                    yield break;
                }
                var items = future.Result ?? Array.Empty<ItemSet>();
                foreach (var item in items) {
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        inventoryName,
                        itemName,
                        itemSetName
                    );
                }
                self.OnComplete(items);
            }
            return new Gs2InlineFuture<ItemSet[]>(Impl);
        }
        
        public static IFuture<ItemSet[]> FetchFuture(
            this ItemSet[] self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            Func<IFuture<ItemSet[]>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ItemSet[]> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ItemSet[]).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "itemSet") {
                            self.OnComplete(Array.Empty<ItemSet>());
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
                    itemName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ItemSet[]>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ItemSet[]> FetchAsync(
    #else
        public static async Task<ItemSet[]> FetchAsync(
    #endif
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ItemSet[]>> fetchImpl
    #else
            Func<Task<ItemSet[]>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ItemSet>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            inventoryName
                       ),
                       self.CacheKey(
                            itemName,
                            itemSetName
                       )
                   ).LockAsync()) {
                try {
                    var items = await fetchImpl();
                    foreach (var item in items) {
                        item.PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName,
                            itemSetName
                        );
                    }
                    return items;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ItemSet).PutCache(
                        cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName,
                            itemSetName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "itemSet") {
                        throw;
                    }
                    return Array.Empty<ItemSet>();
                }
            }
        }
        
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ItemSet[]> FetchAsync(
    #else
        public static async Task<ItemSet[]> FetchAsync(
    #endif
            this ItemSet[] self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ItemSet[]>> fetchImpl
    #else
            Func<Task<ItemSet[]>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ItemSet[]>(
                       self.CacheParentKey(
                           namespaceName,
                           userId,
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
                        userId,
                        inventoryName,
                        itemName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ItemSet[]).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        inventoryName,
                        itemName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "itemSet") {
                        throw;
                    }
                    return Array.Empty<ItemSet>();
                }
            }
        }
#endif

        public static Tuple<ItemSet, bool> GetCache(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ItemSet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName,
                    itemSetName
                )
            );
        }

        public static Tuple<ItemSet[], bool> GetCache(
            this ItemSet[] self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ItemSet[]>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void PutCache(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            if (self?.Count == 0) {
                self = null;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName,
                    itemSetName
                ),
                self,
                ((self?.ExpiresAt ?? 0) == 0 ? null : self.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void PutCache(
            this ItemSet[] self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            if (self == null) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                ),
                self.Where(v => v.Count > 0).OrderByDescending(v => v.Count).ToArray(),
                self?.Where(v => (v.ExpiresAt == 0 ? null : v.ExpiresAt) != null).Min(v => v.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ItemSet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName,
                    itemSetName
                )
            );
        }

        public static void DeleteCache(
            this ItemSet[] self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ItemSet[]>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                self.CacheKey(
                    itemName
                )
            );
        }

        public static void ListSubscribe(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            Action<ItemSet[]> callback
        ) {
            cache.ListSubscribe<ItemSet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ItemSet self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ItemSet>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName
                ),
                callbackId
            );
        }
    }
}