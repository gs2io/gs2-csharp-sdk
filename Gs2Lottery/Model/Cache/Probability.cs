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

#pragma warning disable CS0618 // Obsolete with a message
#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using System.Collections;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
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
            string lotteryName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "lottery",
                namespaceName,
                userId,
                lotteryName,
                timeOffset?.ToString() ?? "0",
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
            int? timeOffset,
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
                            prizeId,
                            timeOffset
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
                    prizeId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Probability>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
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
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<Probability>> fetchImpl
#else
            Func<Task<Probability>> fetchImpl
#endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    lotteryName,
                    prizeId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Probability).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    lotteryName,
                    prizeId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "probability") {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<Probability, bool> GetCache(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName,
                    timeOffset
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
            string prizeId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName,
                    timeOffset
                ),
                self.CacheKey(
                    prizeId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        /// <summary>
        /// Gs2Distributor:DescribeUserData（ユーザーの全データの一括取得）の 1 エントリ（この kind）をキャッシュへ入れる。
        /// 鍵はエントリの namespaceName / 読み込むユーザーの userId / モデル自身のプロパティ / 主キー GRN から取る
        /// （sdk-gen の BaseModel.user_data_cache_keys）。戻り値は親キーで、呼び手が全ページを読み終えてから
        /// Gs2Lottery.SetListCached(cache, timeOffset, kind, parentKey) で「リストが揃った印」を立てる。
        /// </summary>
        public static string PutUserData(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            self.PutCache(
                cache,
                namespaceName,
                userId,
                default /* TODO: 一括取得のエントリから lotteryName を決められない。手書きで値を入れる */,
                default /* TODO: 一括取得のエントリから prizeId を決められない。手書きで値を入れる */,
                timeOffset
            );
            return self.CacheParentKey(
                namespaceName,
                userId,
                default /* TODO: 一括取得のエントリから lotteryName を決められない。手書きで値を入れる */,
                timeOffset
            );
        }

        public static void DeleteCache(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            string prizeId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName,
                    timeOffset
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
            int? timeOffset,
            Action<Probability[]> callback
        ) {
            cache.ListSubscribe<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Probability self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string lotteryName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Probability>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    lotteryName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}