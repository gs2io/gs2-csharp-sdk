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

#pragma warning disable CS0618 // Obsolete with a message
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
    public static partial class UserExt
    {
        public static string CacheParentKey(
            this User self,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "identifier",
                timeOffset?.ToString() ?? "0",
                "User"
            );
        }

        public static string CacheKey(
            this User self,
            string userName
        ) {
            return string.Join(
                ":",
                userName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<User> FetchFuture(
            this User self,
            CacheDatabase cache,
            string userName,
            int? timeOffset,
            Func<IFuture<User>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<User> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as User).PutCache(
                            cache,
                            userName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "user") {
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
                    userName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<User>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<User> FetchAsync(
    #else
        public static async Task<User> FetchAsync(
    #endif
            this User self,
            CacheDatabase cache,
            string userName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<User>> fetchImpl
    #else
            Func<Task<User>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    userName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as User).PutCache(
                    cache,
                    userName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "user") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<User, bool> GetCache(
            this User self,
            CacheDatabase cache,
            string userName,
            int? timeOffset
        ) {
            return cache.Get<User>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    userName
                )
            );
        }

        public static void PutCache(
            this User self,
            CacheDatabase cache,
            string userName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<User>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    userName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    userName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this User self,
            CacheDatabase cache,
            string userName,
            int? timeOffset
        ) {
            cache.Delete<User>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    userName
                )
            );
        }

        public static void ListSubscribe(
            this User self,
            CacheDatabase cache,
            int? timeOffset,
            Action<User[]> callback
        ) {
            cache.ListSubscribe<User>(
                self.CacheParentKey(
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this User self,
            CacheDatabase cache,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<User>(
                self.CacheParentKey(
                    timeOffset
                ),
                callbackId
            );
        }
    }
}