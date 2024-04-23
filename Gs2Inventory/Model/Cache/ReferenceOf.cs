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
    public static partial class ReferenceOfExt
    {
        public static string CacheParentKey(
            this ReferenceOf self,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName
        ) {
            return string.Join(
                ":",
                "inventory",
                namespaceName,
                userId,
                inventoryName,
                itemName,
                itemSetName,
                "ReferenceOf"
            );
        }

        public static string CacheKey(
            this ReferenceOf self,
            string referenceOf
        ) {
            return string.Join(
                ":",
                referenceOf
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<string> FetchFuture(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf,
            Func<IFuture<string>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<string> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ReferenceOf).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName,
                            itemSetName,
                            referenceOf
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "referenceOf") {
                            self.OnComplete(default);
                            yield break;
                        }
                    }
                    self.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;
                new ReferenceOf {
                    Name = item,
                }.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName,
                    referenceOf
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<string>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<string> FetchAsync(
    #else
        public static async Task<string> FetchAsync(
    #endif
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<string>> fetchImpl
    #else
            Func<Task<string>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ReferenceOf>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            inventoryName,
                            itemName,
                            itemSetName
                       ),
                       self.CacheKey(
                            referenceOf
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    new ReferenceOf {
                        Name = item,
                    }.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        inventoryName,
                        itemName,
                        itemSetName,
                        referenceOf
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ReferenceOf).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        inventoryName,
                        itemName,
                        itemSetName,
                        referenceOf
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "referenceOf") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<string, bool> GetCache(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<ReferenceOf>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName
                ),
                self.CacheKey(
                    referenceOf
                )
            );
            if (!find) {
                return Tuple.Create<string, bool>(null, false);
            }
            return Tuple.Create(value.Name, true);
        }

        public static void PutCache(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName
                ),
                self.CacheKey(
                    referenceOf
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ReferenceOf>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName
                ),
                self.CacheKey(
                    referenceOf
                )
            );
        }

        public static void ListSubscribe(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            Action<ReferenceOf[]> callback
        ) {
            cache.ListSubscribe<ReferenceOf>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ReferenceOf self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ReferenceOf>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    inventoryName,
                    itemName,
                    itemSetName
                ),
                callbackId
            );
        }
    }
}