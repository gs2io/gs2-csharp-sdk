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
    public static partial class LayerModelExt
    {
        public static string CacheParentKey(
            this LayerModel self,
            string namespaceName,
            string areaModelName
        ) {
            return string.Join(
                ":",
                "megaField",
                namespaceName,
                areaModelName,
                "LayerModel"
            );
        }

        public static string CacheKey(
            this LayerModel self,
            string layerModelName
        ) {
            return string.Join(
                ":",
                layerModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<LayerModel> FetchFuture(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            string layerModelName,
            Func<IFuture<LayerModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<LayerModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as LayerModel).PutCache(
                            cache,
                            namespaceName,
                            areaModelName,
                            layerModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "layerModel") {
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
                    areaModelName,
                    layerModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<LayerModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<LayerModel> FetchAsync(
    #else
        public static async Task<LayerModel> FetchAsync(
    #endif
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            string layerModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<LayerModel>> fetchImpl
    #else
            Func<Task<LayerModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<LayerModel>(
                       self.CacheParentKey(
                            namespaceName,
                            areaModelName
                       ),
                       self.CacheKey(
                            layerModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        areaModelName,
                        layerModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as LayerModel).PutCache(
                        cache,
                        namespaceName,
                        areaModelName,
                        layerModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "layerModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<LayerModel, bool> GetCache(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            string layerModelName
        ) {
            return cache.Get<LayerModel>(
                self.CacheParentKey(
                    namespaceName,
                    areaModelName
                ),
                self.CacheKey(
                    layerModelName
                )
            );
        }

        public static void PutCache(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            string layerModelName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    areaModelName
                ),
                self.CacheKey(
                    layerModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            string layerModelName
        ) {
            cache.Delete<LayerModel>(
                self.CacheParentKey(
                    namespaceName,
                    areaModelName
                ),
                self.CacheKey(
                    layerModelName
                )
            );
        }

        public static void ListSubscribe(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            Action<LayerModel[]> callback
        ) {
            cache.ListSubscribe<LayerModel>(
                self.CacheParentKey(
                    namespaceName,
                    areaModelName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this LayerModel self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<LayerModel>(
                self.CacheParentKey(
                    namespaceName,
                    areaModelName
                ),
                callbackId
            );
        }
    }
}