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

namespace Gs2.Gs2Gateway.Model.Cache
{
    public static partial class FirebaseTokenExt
    {
        public static string CacheParentKey(
            this FirebaseToken self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "gateway",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "FirebaseToken"
            );
        }

        public static string CacheKey(
            this FirebaseToken self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<FirebaseToken> FetchFuture(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Func<IFuture<FirebaseToken>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<FirebaseToken> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as FirebaseToken).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "firebaseToken") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<FirebaseToken>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<FirebaseToken> FetchAsync(
    #else
        public static async Task<FirebaseToken> FetchAsync(
    #endif
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<FirebaseToken>> fetchImpl
    #else
            Func<Task<FirebaseToken>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as FirebaseToken).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "firebaseToken") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<FirebaseToken, bool> GetCache(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<FirebaseToken>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<FirebaseToken>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<FirebaseToken>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<FirebaseToken[]> callback
        ) {
            cache.ListSubscribe<FirebaseToken>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this FirebaseToken self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<FirebaseToken>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}