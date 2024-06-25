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

namespace Gs2.Gs2Money2.Model.Cache
{
    public static partial class StoreContentModelMasterExt
    {
        public static string CacheParentKey(
            this StoreContentModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                "StoreContentModelMaster"
            );
        }

        public static string CacheKey(
            this StoreContentModelMaster self,
            string contentName
        ) {
            return string.Join(
                ":",
                contentName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<StoreContentModelMaster> FetchFuture(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            Func<IFuture<StoreContentModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<StoreContentModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as StoreContentModelMaster).PutCache(
                            cache,
                            namespaceName,
                            contentName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "storeContentModelMaster") {
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
                    contentName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<StoreContentModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<StoreContentModelMaster> FetchAsync(
    #else
        public static async Task<StoreContentModelMaster> FetchAsync(
    #endif
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<StoreContentModelMaster>> fetchImpl
    #else
            Func<Task<StoreContentModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<StoreContentModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            contentName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        contentName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as StoreContentModelMaster).PutCache(
                        cache,
                        namespaceName,
                        contentName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "storeContentModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<StoreContentModelMaster, bool> GetCache(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName
        ) {
            return cache.Get<StoreContentModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void PutCache(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName
        ) {
            var (value, find) = cache.Get<StoreContentModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName
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
                    contentName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName
        ) {
            cache.Delete<StoreContentModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void ListSubscribe(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<StoreContentModelMaster[]> callback
        ) {
            cache.ListSubscribe<StoreContentModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this StoreContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<StoreContentModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}