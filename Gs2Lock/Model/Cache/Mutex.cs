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

namespace Gs2.Gs2Lock.Model.Cache
{
    public static partial class MutexExt
    {
        public static string CacheParentKey(
            this Mutex self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "lock",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Mutex"
            );
        }

        public static string CacheKey(
            this Mutex self,
            string propertyId
        ) {
            return string.Join(
                ":",
                propertyId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Mutex> FetchFuture(
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset,
            Func<IFuture<Mutex>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Mutex> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Mutex).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            propertyId,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "mutex") {
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
                    propertyId,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Mutex>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Mutex> FetchAsync(
    #else
        public static async Task<Mutex> FetchAsync(
    #endif
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Mutex>> fetchImpl
    #else
            Func<Task<Mutex>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    propertyId,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Mutex).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    propertyId,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "mutex") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Mutex, bool> GetCache(
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Mutex>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    propertyId
                )
            );
        }

        public static void PutCache(
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Mutex>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
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
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    propertyId
                ),
                self,
                ((self?.TtlAt ?? 0) == 0 ? null : self.TtlAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string propertyId,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Mutex>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    propertyId
                )
            );
        }

        public static void ListSubscribe(
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Mutex[]> callback
        ) {
            cache.ListSubscribe<Mutex>(
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
            this Mutex self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Mutex>(
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