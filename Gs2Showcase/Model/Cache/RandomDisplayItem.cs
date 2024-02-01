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
    public static partial class RandomDisplayItemExt
    {
        public static string CacheParentKey(
            this RandomDisplayItem self,
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
                "RandomDisplayItem"
            );
        }

        public static string CacheKey(
            this RandomDisplayItem self,
            string displayItemName
        ) {
            return string.Join(
                ":",
                displayItemName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RandomDisplayItem> FetchFuture(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName,
            Func<IFuture<RandomDisplayItem>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RandomDisplayItem> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RandomDisplayItem).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            showcaseName,
                            displayItemName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "randomDisplayItem") {
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
                    displayItemName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RandomDisplayItem>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RandomDisplayItem> FetchAsync(
    #else
        public static async Task<RandomDisplayItem> FetchAsync(
    #endif
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RandomDisplayItem>> fetchImpl
    #else
            Func<Task<RandomDisplayItem>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<RandomDisplayItem>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            showcaseName
                       ),
                       self.CacheKey(
                            displayItemName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        showcaseName,
                        displayItemName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as RandomDisplayItem).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        showcaseName,
                        displayItemName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "randomDisplayItem") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<RandomDisplayItem, bool> GetCache(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<RandomDisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                self.CacheKey(
                    displayItemName
                )
            );
        }

        public static void PutCache(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName
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
                    displayItemName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<RandomDisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                self.CacheKey(
                    displayItemName
                )
            );
        }

        public static void ListSubscribe(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            Action<RandomDisplayItem[]> callback
        ) {
            cache.ListSubscribe<RandomDisplayItem>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this RandomDisplayItem self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RandomDisplayItem>(
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