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

namespace Gs2.Gs2Lottery.Model.Cache
{
    public static partial class BoxItemsExt
    {
        public static string CacheParentKey(
            this BoxItems self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                userId,
                "BoxItems"
            );
        }

        public static string CacheKey(
            this BoxItems self,
            string prizeTableName
        ) {
            return string.Join(
                ":",
                prizeTableName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BoxItems> FetchFuture(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName,
            Func<IFuture<BoxItems>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BoxItems> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BoxItems).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            prizeTableName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "boxItems") {
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
                    prizeTableName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BoxItems>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BoxItems> FetchAsync(
    #else
        public static async Task<BoxItems> FetchAsync(
    #endif
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BoxItems>> fetchImpl
    #else
            Func<Task<BoxItems>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BoxItems>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            prizeTableName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        prizeTableName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BoxItems).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        prizeTableName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "boxItems") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BoxItems, bool> GetCache(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<BoxItems>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    prizeTableName
                )
            );
        }

        public static void PutCache(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    prizeTableName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<BoxItems>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    prizeTableName
                )
            );
        }

        public static void ListSubscribe(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<BoxItems[]> callback
        ) {
            cache.ListSubscribe<BoxItems>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BoxItems self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BoxItems>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}