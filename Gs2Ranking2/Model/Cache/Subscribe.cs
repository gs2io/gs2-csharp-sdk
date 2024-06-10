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

namespace Gs2.Gs2Ranking2.Model.Cache
{
    public static partial class SubscribeExt
    {
        public static string CacheParentKey(
            this Subscribe self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                userId,
                "Subscribe"
            );
        }

        public static string CacheKey(
            this Subscribe self,
            string rankingName
        ) {
            return string.Join(
                ":",
                rankingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Subscribe> FetchFuture(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
            Func<IFuture<Subscribe>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Subscribe> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Subscribe).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            rankingName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribe") {
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
                    rankingName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Subscribe>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Subscribe> FetchAsync(
    #else
        public static async Task<Subscribe> FetchAsync(
    #endif
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Subscribe>> fetchImpl
    #else
            Func<Task<Subscribe>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Subscribe>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            rankingName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Subscribe).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        rankingName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "subscribe") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Subscribe, bool> GetCache(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Subscribe>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void PutCache(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Subscribe>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    rankingName
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
                    rankingName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string rankingName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Subscribe>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void ListSubscribe(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Subscribe[]> callback
        ) {
            cache.ListSubscribe<Subscribe>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Subscribe self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Subscribe>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}