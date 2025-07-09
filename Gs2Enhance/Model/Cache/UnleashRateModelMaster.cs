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

namespace Gs2.Gs2Enhance.Model.Cache
{
    public static partial class UnleashRateModelMasterExt
    {
        public static string CacheParentKey(
            this UnleashRateModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "enhance",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "UnleashRateModelMaster"
            );
        }

        public static string CacheKey(
            this UnleashRateModelMaster self,
            string rateName
        ) {
            return string.Join(
                ":",
                rateName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<UnleashRateModelMaster> FetchFuture(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset,
            Func<IFuture<UnleashRateModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<UnleashRateModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as UnleashRateModelMaster).PutCache(
                            cache,
                            namespaceName,
                            rateName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "unleashRateModelMaster") {
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
                    rateName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<UnleashRateModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<UnleashRateModelMaster> FetchAsync(
    #else
        public static async Task<UnleashRateModelMaster> FetchAsync(
    #endif
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<UnleashRateModelMaster>> fetchImpl
    #else
            Func<Task<UnleashRateModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    rateName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as UnleashRateModelMaster).PutCache(
                    cache,
                    namespaceName,
                    rateName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "unleashRateModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<UnleashRateModelMaster, bool> GetCache(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            return cache.Get<UnleashRateModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void PutCache(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<UnleashRateModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
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
                    rateName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            cache.Delete<UnleashRateModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void ListSubscribe(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<UnleashRateModelMaster[]> callback
        ) {
            cache.ListSubscribe<UnleashRateModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this UnleashRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<UnleashRateModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}