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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static partial class MoldExt
    {
        public static string CacheParentKey(
            this Mold self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Mold"
            );
        }

        public static string CacheKey(
            this Mold self,
            string moldModelName
        ) {
            return string.Join(
                ":",
                moldModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Mold> FetchFuture(
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset,
            Func<IFuture<Mold>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Mold> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Mold).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            moldModelName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "mold") {
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
                    moldModelName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Mold>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Mold> FetchAsync(
    #else
        public static async Task<Mold> FetchAsync(
    #endif
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Mold>> fetchImpl
    #else
            Func<Task<Mold>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    moldModelName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Mold).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    moldModelName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "mold") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Mold, bool> GetCache(
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Mold>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
                )
            );
        }

        public static void PutCache(
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Mold>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
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
                    moldModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Mold>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
                )
            );
        }

        public static void ListSubscribe(
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Mold[]> callback
        ) {
            cache.ListSubscribe<Mold>(
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
            this Mold self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Mold>(
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