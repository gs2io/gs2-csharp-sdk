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
    public static partial class ProbabilityExt
    {
        public static string CacheParentKey(
            this Probability self,
            string namespaceName,
            string userId,
            string lotteryName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                userId,
                lotteryName,
                "Probability"
            );
        }

        public static string CacheKey(
            this Probability self,
            string prizeId
        ) {
            return string.Join(
                ":",
                prizeId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Probability> FetchFuture(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId,
            Func<IFuture<Probability>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Probability> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Probability).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            lotteryName,
                            prizeId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "probability") {
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
                    lotteryName,
                    prizeId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Probability>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Probability> FetchAsync(
    #else
        public static async Task<Probability> FetchAsync(
    #endif
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Probability>> fetchImpl
    #else
            Func<Task<Probability>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Probability>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            lotteryName
                       ),
                       self.CacheKey(
                            prizeId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        lotteryName,
                        prizeId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Probability).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        lotteryName,
                        prizeId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "probability") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Probability, bool> GetCache(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName
                ),
                self.CacheKey(
                    prizeId
                )
            );
        }

        public static void PutCache(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName
                ),
                self.CacheKey(
                    prizeId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName
                ),
                self.CacheKey(
                    prizeId
                )
            );
        }

        public static void ListSubscribe(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            Action<Probability[]> callback
        ) {
            cache.ListSubscribe<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName
                ),
                callbackId
            );
        }
    }
}