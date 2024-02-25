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

namespace Gs2.Gs2SeasonRating.Model.Cache
{
    public static partial class VoteExt
    {
        public static string CacheParentKey(
            this Vote self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "seasonRating",
                namespaceName,
                "Vote"
            );
        }

        public static string CacheKey(
            this Vote self,
            string seasonName,
            string sessionName
        ) {
            return string.Join(
                ":",
                seasonName,
                sessionName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Vote> FetchFuture(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            string sessionName,
            Func<IFuture<Vote>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Vote> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Vote).PutCache(
                            cache,
                            namespaceName,
                            seasonName,
                            sessionName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "vote") {
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
                    seasonName,
                    sessionName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Vote>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Vote> FetchAsync(
    #else
        public static async Task<Vote> FetchAsync(
    #endif
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            string sessionName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Vote>> fetchImpl
    #else
            Func<Task<Vote>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Vote>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            seasonName,
                            sessionName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        seasonName,
                        sessionName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Vote).PutCache(
                        cache,
                        namespaceName,
                        seasonName,
                        sessionName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "vote") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Vote, bool> GetCache(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            string sessionName
        ) {
            return cache.Get<Vote>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName,
                    sessionName
                )
            );
        }

        public static void PutCache(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            string sessionName
        ) {
            var (value, find) = cache.Get<Vote>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName,
                    sessionName
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
                    seasonName,
                    sessionName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            string seasonName,
            string sessionName
        ) {
            cache.Delete<Vote>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    seasonName,
                    sessionName
                )
            );
        }

        public static void ListSubscribe(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            Action<Vote[]> callback
        ) {
            cache.ListSubscribe<Vote>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Vote self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Vote>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}