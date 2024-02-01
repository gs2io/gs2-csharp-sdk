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

namespace Gs2.Gs2Exchange.Model.Cache
{
    public static partial class IncrementalRateModelMasterExt
    {
        public static string CacheParentKey(
            this IncrementalRateModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "exchange",
                namespaceName,
                "IncrementalRateModelMaster"
            );
        }

        public static string CacheKey(
            this IncrementalRateModelMaster self,
            string rateName
        ) {
            return string.Join(
                ":",
                rateName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<IncrementalRateModelMaster> FetchFuture(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            Func<IFuture<IncrementalRateModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<IncrementalRateModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as IncrementalRateModelMaster).PutCache(
                            cache,
                            namespaceName,
                            rateName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "incrementalRateModelMaster") {
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
                    rateName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<IncrementalRateModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<IncrementalRateModelMaster> FetchAsync(
    #else
        public static async Task<IncrementalRateModelMaster> FetchAsync(
    #endif
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<IncrementalRateModelMaster>> fetchImpl
    #else
            Func<Task<IncrementalRateModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<IncrementalRateModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            rateName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        rateName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as IncrementalRateModelMaster).PutCache(
                        cache,
                        namespaceName,
                        rateName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "incrementalRateModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<IncrementalRateModelMaster, bool> GetCache(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName
        ) {
            return cache.Get<IncrementalRateModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void PutCache(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName
        ) {
            var (value, find) = cache.Get<IncrementalRateModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rateName
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
                    rateName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rateName
        ) {
            cache.Delete<IncrementalRateModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void ListSubscribe(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<IncrementalRateModelMaster[]> callback
        ) {
            cache.ListSubscribe<IncrementalRateModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this IncrementalRateModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<IncrementalRateModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}