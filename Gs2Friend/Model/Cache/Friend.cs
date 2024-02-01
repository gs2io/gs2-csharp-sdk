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
    public static partial class FriendExt
    {
        public static string CacheParentKey(
            this Friend self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "friend",
                namespaceName,
                userId,
                "Friend"
            );
        }

        public static string CacheKey(
            this Friend self,
            bool? withProfile
        ) {
            return string.Join(
                ":",
                withProfile.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Friend> FetchFuture(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
            Func<IFuture<Friend>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Friend> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Friend).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            withProfile
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "friend") {
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
                    withProfile
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Friend>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Friend> FetchAsync(
    #else
        public static async Task<Friend> FetchAsync(
    #endif
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Friend>> fetchImpl
    #else
            Func<Task<Friend>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Friend>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            withProfile
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        withProfile
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Friend).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        withProfile
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "friend") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Friend, bool> GetCache(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Friend>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    withProfile
                )
            );
        }

        public static void PutCache(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Friend>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    withProfile
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
                    withProfile
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool? withProfile
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Friend>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    withProfile
                )
            );
        }

        public static void ListSubscribe(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Friend[]> callback
        ) {
            cache.ListSubscribe<Friend>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Friend self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Friend>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}