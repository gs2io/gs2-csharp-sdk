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
    public static partial class SubscribeRankingModelMasterExt
    {
        public static string CacheParentKey(
            this SubscribeRankingModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "ranking2",
                namespaceName,
                "SubscribeRankingModelMaster"
            );
        }

        public static string CacheKey(
            this SubscribeRankingModelMaster self,
            string rankingName
        ) {
            return string.Join(
                ":",
                rankingName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeRankingModelMaster> FetchFuture(
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
            Func<IFuture<SubscribeRankingModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeRankingModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeRankingModelMaster).PutCache(
                            cache,
                            namespaceName,
                            rankingName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeRankingModelMaster") {
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
            return new Gs2InlineFuture<SubscribeRankingModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeRankingModelMaster> FetchAsync(
    #else
        public static async Task<SubscribeRankingModelMaster> FetchAsync(
    #endif
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeRankingModelMaster>> fetchImpl
    #else
            Func<Task<SubscribeRankingModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SubscribeRankingModelMaster>(
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
                    (null as SubscribeRankingModelMaster).PutCache(
                        cache,
                        namespaceName,
                        rankingName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "subscribeRankingModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SubscribeRankingModelMaster, bool> GetCache(
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            return cache.Get<SubscribeRankingModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void PutCache(
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            var (value, find) = cache.Get<SubscribeRankingModelMaster>(
                self.CacheParentKey(
                    namespaceName
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
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string rankingName
        ) {
            cache.Delete<SubscribeRankingModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    rankingName
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<SubscribeRankingModelMaster[]> callback
        ) {
            cache.ListSubscribe<SubscribeRankingModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SubscribeRankingModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeRankingModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}