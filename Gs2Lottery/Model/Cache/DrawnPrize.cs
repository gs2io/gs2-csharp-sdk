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
 *
 * deny overwrite
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

namespace Gs2.Gs2Lottery.Model.Cache
{
    public static partial class DrawnPrizeExt
    {
        public static string CacheParentKey(
            this DrawnPrize self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                "DrawnPrize"
            );
        }

        public static string CacheKey(
            this DrawnPrize self,
            int index
        ) {
            return string.Join(
                ":", 
                index.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DrawnPrize> FetchFuture(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            int index,
            Func<IFuture<DrawnPrize>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DrawnPrize> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DrawnPrize).PutCache(
                            cache,
                            namespaceName,
                            index
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "drawnPrize") {
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
                    index
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DrawnPrize>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DrawnPrize> FetchAsync(
    #else
        public static async Task<DrawnPrize> FetchAsync(
    #endif
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            int index,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DrawnPrize>> fetchImpl
    #else
            Func<Task<DrawnPrize>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<DrawnPrize>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                           index
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        index
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as DrawnPrize).PutCache(
                        cache,
                        namespaceName,
                        index
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "drawnPrize") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<DrawnPrize, bool> GetCache(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            int index
        ) {
            return cache.Get<DrawnPrize>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    index
                )
            );
        }

        public static void PutCache(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            int index
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    index
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            int index
        ) {
            cache.Delete<DrawnPrize>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    index
                )
            );
        }

        public static void ListSubscribe(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            Action<DrawnPrize[]> callback
        ) {
            cache.ListSubscribe<DrawnPrize>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this DrawnPrize self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DrawnPrize>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}