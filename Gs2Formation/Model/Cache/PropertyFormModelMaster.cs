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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static partial class PropertyFormModelMasterExt
    {
        public static string CacheParentKey(
            this PropertyFormModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                "PropertyFormModelMaster"
            );
        }

        public static string CacheKey(
            this PropertyFormModelMaster self,
            string propertyFormModelName
        ) {
            return string.Join(
                ":",
                propertyFormModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PropertyFormModelMaster> FetchFuture(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string propertyFormModelName,
            Func<IFuture<PropertyFormModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PropertyFormModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PropertyFormModelMaster).PutCache(
                            cache,
                            namespaceName,
                            propertyFormModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "propertyFormModelMaster") {
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
                    propertyFormModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PropertyFormModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PropertyFormModelMaster> FetchAsync(
    #else
        public static async Task<PropertyFormModelMaster> FetchAsync(
    #endif
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string propertyFormModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PropertyFormModelMaster>> fetchImpl
    #else
            Func<Task<PropertyFormModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PropertyFormModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            propertyFormModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        propertyFormModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PropertyFormModelMaster).PutCache(
                        cache,
                        namespaceName,
                        propertyFormModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "propertyFormModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PropertyFormModelMaster, bool> GetCache(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string propertyFormModelName
        ) {
            return cache.Get<PropertyFormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    propertyFormModelName
                )
            );
        }

        public static void PutCache(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string propertyFormModelName
        ) {
            var (value, find) = cache.Get<PropertyFormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    propertyFormModelName
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
                    propertyFormModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string propertyFormModelName
        ) {
            cache.Delete<PropertyFormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    propertyFormModelName
                )
            );
        }

        public static void ListSubscribe(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<PropertyFormModelMaster[]> callback
        ) {
            cache.ListSubscribe<PropertyFormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PropertyFormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PropertyFormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}