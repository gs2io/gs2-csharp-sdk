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
    public static partial class LotteryModelMasterExt
    {
        public static string CacheParentKey(
            this LotteryModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                "LotteryModelMaster"
            );
        }

        public static string CacheKey(
            this LotteryModelMaster self,
            string lotteryName
        ) {
            return string.Join(
                ":",
                lotteryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<LotteryModelMaster> FetchFuture(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName,
            Func<IFuture<LotteryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<LotteryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as LotteryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            lotteryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "lotteryModelMaster") {
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
                    lotteryName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<LotteryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<LotteryModelMaster> FetchAsync(
    #else
        public static async Task<LotteryModelMaster> FetchAsync(
    #endif
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<LotteryModelMaster>> fetchImpl
    #else
            Func<Task<LotteryModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<LotteryModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            lotteryName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        lotteryName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as LotteryModelMaster).PutCache(
                        cache,
                        namespaceName,
                        lotteryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "lotteryModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<LotteryModelMaster, bool> GetCache(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
            return cache.Get<LotteryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                )
            );
        }

        public static void PutCache(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
            var (value, find) = cache.Get<LotteryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
            cache.Delete<LotteryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                )
            );
        }

        public static void ListSubscribe(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<LotteryModelMaster[]> callback
        ) {
            cache.ListSubscribe<LotteryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this LotteryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<LotteryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}