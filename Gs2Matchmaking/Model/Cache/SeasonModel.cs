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
    public static partial class SeasonModelExt
    {
        public static string CacheParentKey(
            this SeasonModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "matchmaking",
                namespaceName,
                "SeasonModel"
            );
        }

        public static string CacheKey(
            this SeasonModel self,
            string seasonName
        ) {
            return string.Join(
                ":",
                seasonName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SeasonModel> FetchFuture(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            Func<IFuture<SeasonModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SeasonModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SeasonModel).PutCache(
                            cache,
                            namespaceName,
                            seasonName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "seasonModel") {
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
                    seasonName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SeasonModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SeasonModel> FetchAsync(
    #else
        public static async Task<SeasonModel> FetchAsync(
    #endif
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SeasonModel>> fetchImpl
    #else
            Func<Task<SeasonModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SeasonModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            seasonName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        seasonName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SeasonModel).PutCache(
                        cache,
                        namespaceName,
                        seasonName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "seasonModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SeasonModel, bool> GetCache(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName
        ) {
            return cache.Get<SeasonModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName
                )
            );
        }

        public static void PutCache(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName
        ) {
            cache.Delete<SeasonModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName
                )
            );
        }

        public static void ListSubscribe(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<SeasonModel[]> callback
        ) {
            cache.ListSubscribe<SeasonModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SeasonModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SeasonModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}