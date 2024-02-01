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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class SalesItemGroupMasterExt
    {
        public static string CacheParentKey(
            this SalesItemGroupMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                "SalesItemGroupMaster"
            );
        }

        public static string CacheKey(
            this SalesItemGroupMaster self,
            string salesItemGroupName
        ) {
            return string.Join(
                ":",
                salesItemGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SalesItemGroupMaster> FetchFuture(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemGroupName,
            Func<IFuture<SalesItemGroupMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SalesItemGroupMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SalesItemGroupMaster).PutCache(
                            cache,
                            namespaceName,
                            salesItemGroupName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "salesItemGroupMaster") {
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
                    salesItemGroupName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SalesItemGroupMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SalesItemGroupMaster> FetchAsync(
    #else
        public static async Task<SalesItemGroupMaster> FetchAsync(
    #endif
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemGroupName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SalesItemGroupMaster>> fetchImpl
    #else
            Func<Task<SalesItemGroupMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SalesItemGroupMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            salesItemGroupName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        salesItemGroupName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SalesItemGroupMaster).PutCache(
                        cache,
                        namespaceName,
                        salesItemGroupName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "salesItemGroupMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SalesItemGroupMaster, bool> GetCache(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemGroupName
        ) {
            return cache.Get<SalesItemGroupMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    salesItemGroupName
                )
            );
        }

        public static void PutCache(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemGroupName
        ) {
            var (value, find) = cache.Get<SalesItemGroupMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    salesItemGroupName
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
                    salesItemGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            string salesItemGroupName
        ) {
            cache.Delete<SalesItemGroupMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    salesItemGroupName
                )
            );
        }

        public static void ListSubscribe(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<SalesItemGroupMaster[]> callback
        ) {
            cache.ListSubscribe<SalesItemGroupMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SalesItemGroupMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SalesItemGroupMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}