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
    public static partial class RatingModelMasterExt
    {
        public static string CacheParentKey(
            this RatingModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "RatingModelMaster"
            );
        }

        public static string CacheKey(
            this RatingModelMaster self,
            string ratingName
        ) {
            return string.Join(
                ":",
                ratingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RatingModelMaster> FetchFuture(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            int? timeOffset,
            Func<IFuture<RatingModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RatingModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RatingModelMaster).PutCache(
                            cache,
                            namespaceName,
                            ratingName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "ratingModelMaster") {
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
                    ratingName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RatingModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RatingModelMaster> FetchAsync(
    #else
        public static async Task<RatingModelMaster> FetchAsync(
    #endif
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RatingModelMaster>> fetchImpl
    #else
            Func<Task<RatingModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    ratingName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as RatingModelMaster).PutCache(
                    cache,
                    namespaceName,
                    ratingName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "ratingModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<RatingModelMaster, bool> GetCache(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            int? timeOffset
        ) {
            return cache.Get<RatingModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void PutCache(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<RatingModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    ratingName
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    ratingName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            int? timeOffset
        ) {
            cache.Delete<RatingModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void ListSubscribe(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<RatingModelMaster[]> callback
        ) {
            cache.ListSubscribe<RatingModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this RatingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RatingModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}