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

namespace Gs2.Gs2MegaField.Model.Cache
{
    public static partial class SpatialExt
    {
        public static string CacheParentKey(
            this Spatial self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "megaField",
                namespaceName,
                userId,
                "Spatial"
            );
        }

        public static string CacheKey(
            this Spatial self,
            string areaModelName,
            string layerModelName
        ) {
            return string.Join(
                ":",
                areaModelName,
                layerModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Spatial> FetchFuture(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName,
            Func<IFuture<Spatial>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Spatial> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Spatial).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            areaModelName,
                            layerModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "spatial") {
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
                    areaModelName,
                    layerModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Spatial>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Spatial> FetchAsync(
    #else
        public static async Task<Spatial> FetchAsync(
    #endif
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Spatial>> fetchImpl
    #else
            Func<Task<Spatial>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Spatial>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            areaModelName,
                            layerModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        areaModelName,
                        layerModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Spatial).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        areaModelName,
                        layerModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "spatial") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Spatial, bool> GetCache(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Spatial>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    areaModelName,
                    layerModelName
                )
            );
        }

        public static void PutCache(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    areaModelName,
                    layerModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string areaModelName,
            string layerModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Spatial>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    areaModelName,
                    layerModelName
                )
            );
        }

        public static void ListSubscribe(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Spatial[]> callback
        ) {
            cache.ListSubscribe<Spatial>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Spatial self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Spatial>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}