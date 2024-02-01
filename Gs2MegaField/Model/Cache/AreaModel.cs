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
    public static partial class AreaModelExt
    {
        public static string CacheParentKey(
            this AreaModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "megaField",
                namespaceName,
                "AreaModel"
            );
        }

        public static string CacheKey(
            this AreaModel self,
            string areaModelName
        ) {
            return string.Join(
                ":",
                areaModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AreaModel> FetchFuture(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            Func<IFuture<AreaModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<AreaModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as AreaModel).PutCache(
                            cache,
                            namespaceName,
                            areaModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "areaModel") {
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
                    areaModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<AreaModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<AreaModel> FetchAsync(
    #else
        public static async Task<AreaModel> FetchAsync(
    #endif
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<AreaModel>> fetchImpl
    #else
            Func<Task<AreaModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<AreaModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            areaModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        areaModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as AreaModel).PutCache(
                        cache,
                        namespaceName,
                        areaModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "areaModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<AreaModel, bool> GetCache(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName
        ) {
            return cache.Get<AreaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    areaModelName
                )
            );
        }

        public static void PutCache(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    areaModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName
        ) {
            cache.Delete<AreaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    areaModelName
                )
            );
        }

        public static void ListSubscribe(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<AreaModel[]> callback
        ) {
            cache.ListSubscribe<AreaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this AreaModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<AreaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}