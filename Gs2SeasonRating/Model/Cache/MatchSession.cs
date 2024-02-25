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
    public static partial class MatchSessionExt
    {
        public static string CacheParentKey(
            this MatchSession self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "seasonRating",
                namespaceName,
                "MatchSession"
            );
        }

        public static string CacheKey(
            this MatchSession self,
            string sessionName
        ) {
            return string.Join(
                ":",
                sessionName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MatchSession> FetchFuture(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            string sessionName,
            Func<IFuture<MatchSession>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MatchSession> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MatchSession).PutCache(
                            cache,
                            namespaceName,
                            sessionName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "matchSession") {
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
                    sessionName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MatchSession>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MatchSession> FetchAsync(
    #else
        public static async Task<MatchSession> FetchAsync(
    #endif
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            string sessionName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MatchSession>> fetchImpl
    #else
            Func<Task<MatchSession>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<MatchSession>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            sessionName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        sessionName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as MatchSession).PutCache(
                        cache,
                        namespaceName,
                        sessionName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "matchSession") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<MatchSession, bool> GetCache(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            string sessionName
        ) {
            return cache.Get<MatchSession>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    sessionName
                )
            );
        }

        public static void PutCache(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            string sessionName
        ) {
            var (value, find) = cache.Get<MatchSession>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
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
                    sessionName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            string sessionName
        ) {
            cache.Delete<MatchSession>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    sessionName
                )
            );
        }

        public static void ListSubscribe(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            Action<MatchSession[]> callback
        ) {
            cache.ListSubscribe<MatchSession>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this MatchSession self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MatchSession>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}