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
    public static partial class LotteryModelExt
    {
        public static string CacheParentKey(
            this LotteryModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                "LotteryModel"
            );
        }

        public static string CacheKey(
            this LotteryModel self,
            string lotteryName
        ) {
            return string.Join(
                ":",
                lotteryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<LotteryModel> FetchFuture(
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName,
            Func<IFuture<LotteryModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<LotteryModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as LotteryModel).PutCache(
                            cache,
                            namespaceName,
                            lotteryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "lotteryModel") {
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
            return new Gs2InlineFuture<LotteryModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<LotteryModel> FetchAsync(
    #else
        public static async Task<LotteryModel> FetchAsync(
    #endif
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<LotteryModel>> fetchImpl
    #else
            Func<Task<LotteryModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<LotteryModel>(
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
                    (null as LotteryModel).PutCache(
                        cache,
                        namespaceName,
                        lotteryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "lotteryModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<LotteryModel, bool> GetCache(
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
            return cache.Get<LotteryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                )
            );
        }

        public static void PutCache(
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
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
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            string lotteryName
        ) {
            cache.Delete<LotteryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    lotteryName
                )
            );
        }

        public static void ListSubscribe(
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<LotteryModel[]> callback
        ) {
            cache.ListSubscribe<LotteryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this LotteryModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<LotteryModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}