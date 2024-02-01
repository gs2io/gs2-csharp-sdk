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

namespace Gs2.Gs2Version.Model.Cache
{
    public static partial class AcceptVersionExt
    {
        public static string CacheParentKey(
            this AcceptVersion self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "version",
                namespaceName,
                userId,
                "AcceptVersion"
            );
        }

        public static string CacheKey(
            this AcceptVersion self,
            string versionName
        ) {
            return string.Join(
                ":",
                versionName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AcceptVersion> FetchFuture(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string versionName,
            Func<IFuture<AcceptVersion>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<AcceptVersion> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as AcceptVersion).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            versionName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "acceptVersion") {
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
                    versionName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<AcceptVersion>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<AcceptVersion> FetchAsync(
    #else
        public static async Task<AcceptVersion> FetchAsync(
    #endif
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string versionName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<AcceptVersion>> fetchImpl
    #else
            Func<Task<AcceptVersion>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<AcceptVersion>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            versionName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        versionName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as AcceptVersion).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        versionName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "acceptVersion") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<AcceptVersion, bool> GetCache(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string versionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<AcceptVersion>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void PutCache(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string versionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<AcceptVersion>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    versionName
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
                    versionName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string versionName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<AcceptVersion>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void ListSubscribe(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<AcceptVersion[]> callback
        ) {
            cache.ListSubscribe<AcceptVersion>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this AcceptVersion self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<AcceptVersion>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}