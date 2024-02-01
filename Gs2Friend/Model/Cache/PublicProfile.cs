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

namespace Gs2.Gs2Friend.Model.Cache
{
    public static partial class PublicProfileExt
    {
        public static string CacheParentKey(
            this PublicProfile self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
                "PublicProfile"
            );
        }

        public static string CacheKey(
            this PublicProfile self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PublicProfile> FetchFuture(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Func<IFuture<PublicProfile>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<PublicProfile> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as PublicProfile).PutCache(
                            cache,
                            namespaceName,
                            userId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "publicProfile") {
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
                    userId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<PublicProfile>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<PublicProfile> FetchAsync(
    #else
        public static async Task<PublicProfile> FetchAsync(
    #endif
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<PublicProfile>> fetchImpl
    #else
            Func<Task<PublicProfile>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<PublicProfile>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as PublicProfile).PutCache(
                        cache,
                        namespaceName,
                        userId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "publicProfile") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<PublicProfile, bool> GetCache(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<PublicProfile>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId
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
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<PublicProfile>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<PublicProfile[]> callback
        ) {
            cache.ListSubscribe<PublicProfile>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this PublicProfile self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<PublicProfile>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}