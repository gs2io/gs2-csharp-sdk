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
    public static partial class SubscribeRankingModelExt
    {
        public static string CacheParentKey(
            this SubscribeRankingModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                "SubscribeRankingModel"
            );
        }

        public static string CacheKey(
            this SubscribeRankingModel self,
            string rankingName
        ) {
            return string.Join(
                ":",
                rankingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeRankingModel> FetchFuture(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            Func<IFuture<SubscribeRankingModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeRankingModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeRankingModel).PutCache(
                            cache,
                            namespaceName,
                            rankingName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeRankingModel") {
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
                    rankingName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscribeRankingModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeRankingModel> FetchAsync(
    #else
        public static async Task<SubscribeRankingModel> FetchAsync(
    #endif
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeRankingModel>> fetchImpl
    #else
            Func<Task<SubscribeRankingModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SubscribeRankingModel>(
                       self.CacheParentKey(
                            namespaceName
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
                        rankingName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SubscribeRankingModel).PutCache(
                        cache,
                        namespaceName,
                        rankingName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "subscribeRankingModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SubscribeRankingModel, bool> GetCache(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            return cache.Get<SubscribeRankingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void PutCache(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rankingName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            cache.Delete<SubscribeRankingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<SubscribeRankingModel[]> callback
        ) {
            cache.ListSubscribe<SubscribeRankingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SubscribeRankingModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeRankingModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}