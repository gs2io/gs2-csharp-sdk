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
 *
 * deny overwrite
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
    public static partial class SignedBallotExt
    {
        public static string CacheParentKey(
            this SignedBallot self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "seasonRating",
                namespaceName,
                userId,
                "SignedBallot"
            );
        }

        public static string CacheKey(
            this SignedBallot self,
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
        public static IFuture<SignedBallot> FetchFuture(
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            string sessionName,
            Func<IFuture<SignedBallot>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SignedBallot> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SignedBallot).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            seasonName,
                            sessionName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "ballot") {
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
                    seasonName,
                    sessionName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SignedBallot>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SignedBallot> FetchAsync(
    #else
        public static async Task<SignedBallot> FetchAsync(
    #endif
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            string sessionName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SignedBallot>> fetchImpl
    #else
            Func<Task<SignedBallot>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SignedBallot>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
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
                        userId,
                        seasonName,
                        sessionName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SignedBallot).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        seasonName,
                        sessionName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "ballot") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SignedBallot, bool> GetCache(
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            string sessionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SignedBallot>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    seasonName,
                    sessionName
                )
            );
        }

        public static void PutCache(
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            string sessionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string seasonName,
            string sessionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SignedBallot>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    seasonName,
                    sessionName
                )
            );
        }

        public static void ListSubscribe(
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<SignedBallot[]> callback
        ) {
            cache.ListSubscribe<SignedBallot>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SignedBallot self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SignedBallot>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}