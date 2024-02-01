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

namespace Gs2.Gs2Key.Model.Cache
{
    public static partial class KeyExt
    {
        public static string CacheParentKey(
            this Key self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "key",
                namespaceName,
                "Key"
            );
        }

        public static string CacheKey(
            this Key self,
            string keyName
        ) {
            return string.Join(
                ":",
                keyName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Key> FetchFuture(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            string keyName,
            Func<IFuture<Key>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Key> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Key).PutCache(
                            cache,
                            namespaceName,
                            keyName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "key") {
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
                    keyName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Key>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Key> FetchAsync(
    #else
        public static async Task<Key> FetchAsync(
    #endif
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            string keyName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Key>> fetchImpl
    #else
            Func<Task<Key>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Key>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            keyName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        keyName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Key).PutCache(
                        cache,
                        namespaceName,
                        keyName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "key") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Key, bool> GetCache(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            string keyName
        ) {
            return cache.Get<Key>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    keyName
                )
            );
        }

        public static void PutCache(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            string keyName
        ) {
            var (value, find) = cache.Get<Key>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    keyName
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
                    keyName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            string keyName
        ) {
            cache.Delete<Key>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    keyName
                )
            );
        }

        public static void ListSubscribe(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            Action<Key[]> callback
        ) {
            cache.ListSubscribe<Key>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Key self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Key>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}