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

namespace Gs2.Gs2News.Model.Cache
{
    public static partial class SetCookieRequestEntryExt
    {
        public static string CacheParentKey(
            this SetCookieRequestEntry self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "news",
                namespaceName,
                userId,
                "SetCookieRequestEntry"
            );
        }

        public static string CacheKey(
            this SetCookieRequestEntry self,
            string key,
            string value
        ) {
            return string.Join(
                ":",
                key,
                value
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SetCookieRequestEntry> FetchFuture(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string key,
            string value,
            Func<IFuture<SetCookieRequestEntry>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SetCookieRequestEntry> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SetCookieRequestEntry).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            key,
                            value
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "setCookieRequestEntry") {
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
                    key,
                    value
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SetCookieRequestEntry>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SetCookieRequestEntry> FetchAsync(
    #else
        public static async Task<SetCookieRequestEntry> FetchAsync(
    #endif
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string key,
            string value,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SetCookieRequestEntry>> fetchImpl
    #else
            Func<Task<SetCookieRequestEntry>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SetCookieRequestEntry>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            key,
                            value
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        key,
                        value
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SetCookieRequestEntry).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        key,
                        value
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "setCookieRequestEntry") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SetCookieRequestEntry, bool> GetCache(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string key,
            string value
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SetCookieRequestEntry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    key,
                    value
                )
            );
        }

        public static void PutCache(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string key,
            string value
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
                    key,
                    value
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string key,
            string value
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SetCookieRequestEntry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    key,
                    value
                )
            );
        }

        public static void ListSubscribe(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<SetCookieRequestEntry[]> callback
        ) {
            cache.ListSubscribe<SetCookieRequestEntry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SetCookieRequestEntry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SetCookieRequestEntry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}