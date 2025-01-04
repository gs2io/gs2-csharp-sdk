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

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static partial class DistributorModelMasterExt
    {
        public static string CacheParentKey(
            this DistributorModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "distributor",
                namespaceName,
                "DistributorModelMaster"
            );
        }

        public static string CacheKey(
            this DistributorModelMaster self,
            string distributorName
        ) {
            return string.Join(
                ":",
                distributorName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DistributorModelMaster> FetchFuture(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            Func<IFuture<DistributorModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DistributorModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DistributorModelMaster).PutCache(
                            cache,
                            namespaceName,
                            distributorName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "distributorModelMaster") {
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
                    distributorName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DistributorModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DistributorModelMaster> FetchAsync(
    #else
        public static async Task<DistributorModelMaster> FetchAsync(
    #endif
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DistributorModelMaster>> fetchImpl
    #else
            Func<Task<DistributorModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<DistributorModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            distributorName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        distributorName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as DistributorModelMaster).PutCache(
                        cache,
                        namespaceName,
                        distributorName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "distributorModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<DistributorModelMaster, bool> GetCache(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName
        ) {
            return cache.Get<DistributorModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    distributorName
                )
            );
        }

        public static void PutCache(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName
        ) {
            var (value, find) = cache.Get<DistributorModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    distributorName
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
                    distributorName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName
        ) {
            cache.Delete<DistributorModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    distributorName
                )
            );
        }

        public static void ListSubscribe(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<DistributorModelMaster[]> callback
        ) {
            cache.ListSubscribe<DistributorModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this DistributorModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DistributorModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}