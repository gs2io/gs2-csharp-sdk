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
    public static partial class PropertyFormExt
    {
        public static string CacheParentKey(
            this PropertyForm self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                userId,
                "PropertyForm"
            );
        }

        public static string CacheKey(
            this PropertyForm self,
            string propertyFormModelName,
            string propertyId
        ) {
            return string.Join(
                ":",
                propertyFormModelName,
                propertyId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PropertyForm> FetchFuture(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId,
            Func<IFuture<PropertyForm>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PropertyForm> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PropertyForm).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            propertyFormModelName,
                            propertyId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "propertyForm") {
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
                    propertyFormModelName,
                    propertyId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PropertyForm>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PropertyForm> FetchAsync(
    #else
        public static async Task<PropertyForm> FetchAsync(
    #endif
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PropertyForm>> fetchImpl
    #else
            Func<Task<PropertyForm>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PropertyForm>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            propertyFormModelName,
                            propertyId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        propertyFormModelName,
                        propertyId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PropertyForm).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        propertyFormModelName,
                        propertyId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "propertyForm") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PropertyForm, bool> GetCache(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<PropertyForm>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyFormModelName,
                    propertyId
                )
            );
        }

        public static void PutCache(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<PropertyForm>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyFormModelName,
                    propertyId
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyFormModelName,
                    propertyId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyFormModelName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<PropertyForm>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyFormModelName,
                    propertyId
                )
            );
        }

        public static void ListSubscribe(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<PropertyForm[]> callback
        ) {
            cache.ListSubscribe<PropertyForm>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PropertyForm self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PropertyForm>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}