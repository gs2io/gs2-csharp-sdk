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
    public static partial class DisplayItemExt
    {
        public static string CacheParentKey(
            this DisplayItem self,
            string namespaceName,
            string userId,
            string showcaseName
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                userId,
                showcaseName,
                "DisplayItem"
            );
        }

        public static string CacheKey(
            this DisplayItem self,
            string displayItemId
        ) {
            return string.Join(
                ":",
                displayItemId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DisplayItem> FetchFuture(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId,
            Func<IFuture<DisplayItem>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DisplayItem> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DisplayItem).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            showcaseName,
                            displayItemId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "displayItem") {
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
                    userId,
                    showcaseName,
                    displayItemId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DisplayItem>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DisplayItem> FetchAsync(
    #else
        public static async Task<DisplayItem> FetchAsync(
    #endif
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DisplayItem>> fetchImpl
    #else
            Func<Task<DisplayItem>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<DisplayItem>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            showcaseName
                       ),
                       self.CacheKey(
                            displayItemId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        showcaseName,
                        displayItemId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as DisplayItem).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        showcaseName,
                        displayItemId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "displayItem") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<DisplayItem, bool> GetCache(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<DisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                self.CacheKey(
                    displayItemId
                )
            );
        }

        public static void PutCache(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                self.CacheKey(
                    displayItemId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<DisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                self.CacheKey(
                    displayItemId
                )
            );
        }

        public static void ListSubscribe(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            Action<DisplayItem[]> callback
        ) {
            cache.ListSubscribe<DisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this DisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                callbackId
            );
        }
    }
}