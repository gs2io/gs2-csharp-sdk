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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static partial class RatingExt
    {
        public static string CacheParentKey(
            this Rating self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                userId,
                "Rating"
            );
        }

        public static string CacheKey(
            this Rating self,
            string ratingName
        ) {
            return string.Join(
                ":",
                ratingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Rating> FetchFuture(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string ratingName,
            Func<IFuture<Rating>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Rating> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Rating).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            ratingName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "rating") {
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
                    ratingName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Rating>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Rating> FetchAsync(
    #else
        public static async Task<Rating> FetchAsync(
    #endif
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string ratingName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Rating>> fetchImpl
    #else
            Func<Task<Rating>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Rating>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            ratingName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        ratingName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Rating).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        ratingName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "rating") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Rating, bool> GetCache(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string ratingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Rating>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void PutCache(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string ratingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Rating>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    ratingName
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
                    ratingName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string ratingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Rating>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void ListSubscribe(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Rating[]> callback
        ) {
            cache.ListSubscribe<Rating>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Rating self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Rating>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}