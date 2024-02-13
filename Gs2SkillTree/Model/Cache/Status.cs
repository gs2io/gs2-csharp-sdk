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

namespace Gs2.Gs2SkillTree.Model.Cache
{
    public static partial class StatusExt
    {
        public static string CacheParentKey(
            this Status self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "skillTree",
                namespaceName,
                userId,
                "Status"
            );
        }

        public static string CacheKey(
            this Status self,
            string propertyId
        ) {
            return string.Join(
                ":",
                propertyId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Status> FetchFuture(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            Func<IFuture<Status>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Status> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Status).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            propertyId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "status") {
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
                    propertyId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Status>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Status> FetchAsync(
    #else
        public static async Task<Status> FetchAsync(
    #endif
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Status>> fetchImpl
    #else
            Func<Task<Status>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Status>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            propertyId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        propertyId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Status).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        propertyId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "status") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Status, bool> GetCache(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Status>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyId
                )
            );
        }

        public static void PutCache(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Status>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyId
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
                    propertyId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Status>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    propertyId
                )
            );
        }

        public static void ListSubscribe(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Status[]> callback
        ) {
            cache.ListSubscribe<Status>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Status self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Status>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}