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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class TakeOverExt
    {
        public static string CacheParentKey(
            this TakeOver self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "account",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "TakeOver"
            );
        }

        public static string CacheKey(
            this TakeOver self,
            int? type
        ) {
            return string.Join(
                ":",
                type.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<TakeOver> FetchFuture(
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            int? timeOffset,
            Func<IFuture<TakeOver>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<TakeOver> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as TakeOver).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            type,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "takeOver") {
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
                    type,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<TakeOver>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<TakeOver> FetchAsync(
    #else
        public static async Task<TakeOver> FetchAsync(
    #endif
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<TakeOver>> fetchImpl
    #else
            Func<Task<TakeOver>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    type,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as TakeOver).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    type,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "takeOver") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<TakeOver, bool> GetCache(
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<TakeOver>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void PutCache(
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<TakeOver>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    type
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
                    type
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? type,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<TakeOver>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void ListSubscribe(
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<TakeOver[]> callback
        ) {
            cache.ListSubscribe<TakeOver>(
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
            this TakeOver self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<TakeOver>(
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