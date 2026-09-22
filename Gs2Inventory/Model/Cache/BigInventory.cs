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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Model.Cache
{
    public static partial class BigInventoryExt
    {
        public static string CacheParentKey(
            this BigInventory self,
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
                "BigInventory"
            );
        }

        public static string CacheKey(
            this BigInventory self,
            string inventoryName
        ) {
            return string.Join(
                ":",
                inventoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BigInventory> FetchFuture(
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset,
            Func<IFuture<BigInventory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BigInventory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BigInventory).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            inventoryName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "bigInventory") {
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
            return new Gs2InlineFuture<BigInventory>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<BigInventory> FetchAsync(
#else
        public static async Task<BigInventory> FetchAsync(
#endif
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<BigInventory>> fetchImpl
#else
            Func<Task<BigInventory>> fetchImpl
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
                (null as BigInventory).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    inventoryName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "bigInventory") {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<BigInventory, bool> GetCache(
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<BigInventory>(
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
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
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

        /// <summary>
        /// Gs2Distributor:DescribeUserData（ユーザーの全データの一括取得）の 1 エントリ（この kind）をキャッシュへ入れる。
        /// 鍵はエントリの namespaceName / 読み込むユーザーの userId / モデル自身のプロパティ / 主キー GRN から取る
        /// （sdk-gen の BaseModel.user_data_cache_keys）。戻り値は親キーで、呼び手が全ページを読み終えてから
        /// Gs2Inventory.SetListCached(cache, timeOffset, kind, parentKey) で「リストが揃った印」を立てる。
        /// </summary>
        public static string PutUserData(
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            self.PutCache(
                cache,
                namespaceName,
                userId,
                self.InventoryName,
                timeOffset
            );
            return self.CacheParentKey(
                namespaceName,
                userId,
                timeOffset
            );
        }

        public static void DeleteCache(
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<BigInventory>(
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
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<BigInventory[]> callback
        ) {
            cache.ListSubscribe<BigInventory>(
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
            this BigInventory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BigInventory>(
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