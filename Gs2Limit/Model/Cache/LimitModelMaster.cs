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

namespace Gs2.Gs2Limit.Model.Cache
{
    public static partial class LimitModelMasterExt
    {
        public static string CacheParentKey(
            this LimitModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "limit",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "LimitModelMaster"
            );
        }

        public static string CacheKey(
            this LimitModelMaster self,
            string limitName
        ) {
            return string.Join(
                ":",
                limitName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<LimitModelMaster> FetchFuture(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            int? timeOffset,
            Func<IFuture<LimitModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<LimitModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as LimitModelMaster).PutCache(
                            cache,
                            namespaceName,
                            limitName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "limitModelMaster") {
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
                    limitName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<LimitModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<LimitModelMaster> FetchAsync(
    #else
        public static async Task<LimitModelMaster> FetchAsync(
    #endif
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<LimitModelMaster>> fetchImpl
    #else
            Func<Task<LimitModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    limitName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as LimitModelMaster).PutCache(
                    cache,
                    namespaceName,
                    limitName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "limitModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<LimitModelMaster, bool> GetCache(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            int? timeOffset
        ) {
            return cache.Get<LimitModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    limitName
                )
            );
        }

        public static void PutCache(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<LimitModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    limitName
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
                    limitName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            int? timeOffset
        ) {
            cache.Delete<LimitModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    limitName
                )
            );
        }

        public static void ListSubscribe(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<LimitModelMaster[]> callback
        ) {
            cache.ListSubscribe<LimitModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this LimitModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<LimitModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}