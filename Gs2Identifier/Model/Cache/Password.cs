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

namespace Gs2.Gs2Identifier.Model.Cache
{
    public static partial class PasswordExt
    {
        public static string CacheParentKey(
            this Password self,
            string userName
        ) {
            return string.Join(
                ":",
                "identifier",
                userName,
                "Password"
            );
        }

        public static string CacheKey(
            this Password self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Password> FetchFuture(
            this Password self,
            CacheDatabase cache,
            string userName,
            Func<IFuture<Password>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Password> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Password).PutCache(
                            cache,
                            userName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "password") {
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
                    userName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Password>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Password> FetchAsync(
    #else
        public static async Task<Password> FetchAsync(
    #endif
            this Password self,
            CacheDatabase cache,
            string userName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Password>> fetchImpl
    #else
            Func<Task<Password>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Password>(
                       self.CacheParentKey(
                            userName
                       ),
                       self.CacheKey(
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        userName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Password).PutCache(
                        cache,
                        userName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "password") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Password, bool> GetCache(
            this Password self,
            CacheDatabase cache,
            string userName
        ) {
            return cache.Get<Password>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this Password self,
            CacheDatabase cache,
            string userName
        ) {
            var (value, find) = cache.Get<Password>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Password self,
            CacheDatabase cache,
            string userName
        ) {
            cache.Delete<Password>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this Password self,
            CacheDatabase cache,
            string userName,
            Action<Password[]> callback
        ) {
            cache.ListSubscribe<Password>(
                self.CacheParentKey(
                    userName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Password self,
            CacheDatabase cache,
            string userName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Password>(
                self.CacheParentKey(
                    userName
                ),
                callbackId
            );
        }
    }
}