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
    public static partial class RatingModelExt
    {
        public static string CacheParentKey(
            this RatingModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                "RatingModel"
            );
        }

        public static string CacheKey(
            this RatingModel self,
            string ratingName
        ) {
            return string.Join(
                ":",
                ratingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RatingModel> FetchFuture(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
            Func<IFuture<RatingModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RatingModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RatingModel).PutCache(
                            cache,
                            namespaceName,
                            ratingName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "ratingModel") {
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
                    ratingName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RatingModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RatingModel> FetchAsync(
    #else
        public static async Task<RatingModel> FetchAsync(
    #endif
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RatingModel>> fetchImpl
    #else
            Func<Task<RatingModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<RatingModel>(
                       self.CacheParentKey(
                            namespaceName
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
                        ratingName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as RatingModel).PutCache(
                        cache,
                        namespaceName,
                        ratingName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "ratingModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<RatingModel, bool> GetCache(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName
        ) {
            return cache.Get<RatingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void PutCache(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    ratingName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            string ratingName
        ) {
            cache.Delete<RatingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    ratingName
                )
            );
        }

        public static void ListSubscribe(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<RatingModel[]> callback
        ) {
            cache.ListSubscribe<RatingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this RatingModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RatingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}