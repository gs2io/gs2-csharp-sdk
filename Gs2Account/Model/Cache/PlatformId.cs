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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class PlatformIdExt
    {
        public static string CacheParentKey(
            this PlatformId self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "account",
                namespaceName,
                userId,
                "PlatformId"
            );
        }

        public static string CacheKey(
            this PlatformId self,
            int? type
        ) {
            return string.Join(
                ":",
                type.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PlatformId> FetchFuture(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            Func<IFuture<PlatformId>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PlatformId> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PlatformId).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            type
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "platformId") {
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
                    type
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PlatformId>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PlatformId> FetchAsync(
    #else
        public static async Task<PlatformId> FetchAsync(
    #endif
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PlatformId>> fetchImpl
    #else
            Func<Task<PlatformId>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PlatformId>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            type
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        type
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PlatformId).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        type
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "platformId") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PlatformId, bool> GetCache(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<PlatformId>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void PutCache(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<PlatformId>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    type
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    type
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<PlatformId>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void ListSubscribe(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<PlatformId[]> callback
        ) {
            cache.ListSubscribe<PlatformId>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PlatformId self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PlatformId>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}