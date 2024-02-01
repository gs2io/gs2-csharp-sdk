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

namespace Gs2.Gs2SerialKey.Model.Cache
{
    public static partial class SerialKeyExt
    {
        public static string CacheParentKey(
            this SerialKey self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "serialKey",
                namespaceName,
                "Singleton",
                "SerialKey"
            );
        }

        public static string CacheKey(
            this SerialKey self,
            string serialKeyCode
        ) {
            return string.Join(
                ":",
                serialKeyCode
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SerialKey> FetchFuture(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string serialKeyCode,
            Func<IFuture<SerialKey>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SerialKey> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SerialKey).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            serialKeyCode
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "serialKey") {
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
                    serialKeyCode
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SerialKey>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SerialKey> FetchAsync(
    #else
        public static async Task<SerialKey> FetchAsync(
    #endif
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string serialKeyCode,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SerialKey>> fetchImpl
    #else
            Func<Task<SerialKey>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SerialKey>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            serialKeyCode
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        serialKeyCode
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SerialKey).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        serialKeyCode
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "serialKey") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SerialKey, bool> GetCache(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string serialKeyCode
        ) {
            return cache.Get<SerialKey>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    serialKeyCode
                )
            );
        }

        public static void PutCache(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string serialKeyCode
        ) {
            var (value, find) = cache.Get<SerialKey>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    serialKeyCode
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
                    serialKeyCode
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string serialKeyCode
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SerialKey>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    serialKeyCode
                )
            );
        }

        public static void ListSubscribe(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<SerialKey[]> callback
        ) {
            cache.ListSubscribe<SerialKey>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SerialKey self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SerialKey>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}