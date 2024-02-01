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
    public static partial class IdentifierExt
    {
        public static string CacheParentKey(
            this Identifier self,
            string userName
        ) {
            return string.Join(
                ":",
                "identifier",
                userName,
                "Identifier"
            );
        }

        public static string CacheKey(
            this Identifier self,
            string clientId
        ) {
            return string.Join(
                ":",
                clientId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Identifier> FetchFuture(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            string clientId,
            Func<IFuture<Identifier>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Identifier> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Identifier).PutCache(
                            cache,
                            userName,
                            clientId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "identifier") {
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
                    clientId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Identifier>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Identifier> FetchAsync(
    #else
        public static async Task<Identifier> FetchAsync(
    #endif
            this Identifier self,
            CacheDatabase cache,
            string userName,
            string clientId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Identifier>> fetchImpl
    #else
            Func<Task<Identifier>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Identifier>(
                       self.CacheParentKey(
                            userName
                       ),
                       self.CacheKey(
                            clientId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        userName,
                        clientId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Identifier).PutCache(
                        cache,
                        userName,
                        clientId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "identifier") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Identifier, bool> GetCache(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            string clientId
        ) {
            return cache.Get<Identifier>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                    clientId
                )
            );
        }

        public static void PutCache(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            string clientId
        ) {
            var (value, find) = cache.Get<Identifier>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                    clientId
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
                    clientId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            string clientId
        ) {
            cache.Delete<Identifier>(
                self.CacheParentKey(
                    userName
                ),
                self.CacheKey(
                    clientId
                )
            );
        }

        public static void ListSubscribe(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            Action<Identifier[]> callback
        ) {
            cache.ListSubscribe<Identifier>(
                self.CacheParentKey(
                    userName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Identifier self,
            CacheDatabase cache,
            string userName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Identifier>(
                self.CacheParentKey(
                    userName
                ),
                callbackId
            );
        }
    }
}