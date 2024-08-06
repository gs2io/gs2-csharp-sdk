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

namespace Gs2.Gs2Log.Model.Cache
{
    public static partial class InGameLogExt
    {
        public static string CacheParentKey(
            this InGameLog self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "log",
                namespaceName,
                "InGameLog"
            );
        }

        public static string CacheKey(
            this InGameLog self,
            string requestId
        ) {
            return string.Join(
                ":",
                requestId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<InGameLog> FetchFuture(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string requestId,
            Func<IFuture<InGameLog>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<InGameLog> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as InGameLog).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            requestId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "inGameLog") {
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
                    requestId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<InGameLog>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<InGameLog> FetchAsync(
    #else
        public static async Task<InGameLog> FetchAsync(
    #endif
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string requestId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<InGameLog>> fetchImpl
    #else
            Func<Task<InGameLog>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<InGameLog>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            requestId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        requestId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as InGameLog).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        requestId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "inGameLog") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<InGameLog, bool> GetCache(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string requestId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<InGameLog>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    requestId
                )
            );
        }

        public static void PutCache(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string requestId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    requestId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string requestId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<InGameLog>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    requestId
                )
            );
        }

        public static void ListSubscribe(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<InGameLog[]> callback
        ) {
            cache.ListSubscribe<InGameLog>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this InGameLog self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<InGameLog>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}