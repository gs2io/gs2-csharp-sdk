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

namespace Gs2.Gs2Ranking.Model.Cache
{
    public static partial class CategoryModelMasterExt
    {
        public static string CacheParentKey(
            this CategoryModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "ranking",
                namespaceName,
                "CategoryModelMaster"
            );
        }

        public static string CacheKey(
            this CategoryModelMaster self,
            string categoryName
        ) {
            return string.Join(
                ":",
                categoryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CategoryModelMaster> FetchFuture(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string categoryName,
            Func<IFuture<CategoryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CategoryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CategoryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            categoryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "categoryModelMaster") {
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
                    categoryName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CategoryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CategoryModelMaster> FetchAsync(
    #else
        public static async Task<CategoryModelMaster> FetchAsync(
    #endif
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string categoryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CategoryModelMaster>> fetchImpl
    #else
            Func<Task<CategoryModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<CategoryModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            categoryName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        categoryName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as CategoryModelMaster).PutCache(
                        cache,
                        namespaceName,
                        categoryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "categoryModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<CategoryModelMaster, bool> GetCache(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string categoryName
        ) {
            return cache.Get<CategoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    categoryName
                )
            );
        }

        public static void PutCache(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string categoryName
        ) {
            var (value, find) = cache.Get<CategoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    categoryName
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
                    categoryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string categoryName
        ) {
            cache.Delete<CategoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    categoryName
                )
            );
        }

        public static void ListSubscribe(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<CategoryModelMaster[]> callback
        ) {
            cache.ListSubscribe<CategoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this CategoryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CategoryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}