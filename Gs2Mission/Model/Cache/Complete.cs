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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static partial class CompleteExt
    {
        public static string CacheParentKey(
            this Complete self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                userId,
                "Complete"
            );
        }

        public static string CacheKey(
            this Complete self,
            string missionGroupName
        ) {
            return string.Join(
                ":",
                missionGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Complete> FetchFuture(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName,
            Func<IFuture<Complete>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Complete> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Complete).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            missionGroupName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "complete") {
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
                    missionGroupName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Complete>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Complete> FetchAsync(
    #else
        public static async Task<Complete> FetchAsync(
    #endif
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Complete>> fetchImpl
    #else
            Func<Task<Complete>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Complete>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            missionGroupName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        missionGroupName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Complete).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        missionGroupName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "complete") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Complete, bool> GetCache(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Complete>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void PutCache(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Complete>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    missionGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string missionGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Complete>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void ListSubscribe(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Complete[]> callback
        ) {
            cache.ListSubscribe<Complete>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Complete self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Complete>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}