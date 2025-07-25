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
    public static partial class AreaModelMasterExt
    {
        public static string CacheParentKey(
            this AreaModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "megaField",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "AreaModelMaster"
            );
        }

        public static string CacheKey(
            this AreaModelMaster self,
            string areaModelName
        ) {
            return string.Join(
                ":",
                areaModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AreaModelMaster> FetchFuture(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            int? timeOffset,
            Func<IFuture<AreaModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<AreaModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as AreaModelMaster).PutCache(
                            cache,
                            namespaceName,
                            areaModelName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "areaModelMaster") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<AreaModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<AreaModelMaster> FetchAsync(
    #else
        public static async Task<AreaModelMaster> FetchAsync(
    #endif
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<AreaModelMaster>> fetchImpl
    #else
            Func<Task<AreaModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    areaModelName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as AreaModelMaster).PutCache(
                    cache,
                    namespaceName,
                    areaModelName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "areaModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<AreaModelMaster, bool> GetCache(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            int? timeOffset
        ) {
            return cache.Get<AreaModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    areaModelName
                )
            );
        }

        public static void PutCache(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<AreaModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    areaModelName
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    areaModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string areaModelName,
            int? timeOffset
        ) {
            cache.Delete<AreaModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    areaModelName
                )
            );
        }

        public static void ListSubscribe(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<AreaModelMaster[]> callback
        ) {
            cache.ListSubscribe<AreaModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this AreaModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<AreaModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}