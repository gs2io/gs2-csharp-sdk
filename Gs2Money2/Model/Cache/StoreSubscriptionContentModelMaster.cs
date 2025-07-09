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

namespace Gs2.Gs2Money2.Model.Cache
{
    public static partial class StoreSubscriptionContentModelMasterExt
    {
        public static string CacheParentKey(
            this StoreSubscriptionContentModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "StoreSubscriptionContentModelMaster"
            );
        }

        public static string CacheKey(
            this StoreSubscriptionContentModelMaster self,
            string contentName
        ) {
            return string.Join(
                ":",
                contentName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<StoreSubscriptionContentModelMaster> FetchFuture(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            int? timeOffset,
            Func<IFuture<StoreSubscriptionContentModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<StoreSubscriptionContentModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as StoreSubscriptionContentModelMaster).PutCache(
                            cache,
                            namespaceName,
                            contentName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "storeSubscriptionContentModelMaster") {
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
                    contentName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<StoreSubscriptionContentModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<StoreSubscriptionContentModelMaster> FetchAsync(
    #else
        public static async Task<StoreSubscriptionContentModelMaster> FetchAsync(
    #endif
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<StoreSubscriptionContentModelMaster>> fetchImpl
    #else
            Func<Task<StoreSubscriptionContentModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    contentName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as StoreSubscriptionContentModelMaster).PutCache(
                    cache,
                    namespaceName,
                    contentName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "storeSubscriptionContentModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<StoreSubscriptionContentModelMaster, bool> GetCache(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            int? timeOffset
        ) {
            return cache.Get<StoreSubscriptionContentModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void PutCache(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<StoreSubscriptionContentModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    contentName
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
                    contentName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            int? timeOffset
        ) {
            cache.Delete<StoreSubscriptionContentModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void ListSubscribe(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<StoreSubscriptionContentModelMaster[]> callback
        ) {
            cache.ListSubscribe<StoreSubscriptionContentModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this StoreSubscriptionContentModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<StoreSubscriptionContentModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}