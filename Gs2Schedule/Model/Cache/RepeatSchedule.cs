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

namespace Gs2.Gs2Schedule.Model.Cache
{
    public static partial class RepeatScheduleExt
    {
        public static string CacheParentKey(
            this RepeatSchedule self,
            string namespaceName,
            string userId,
            bool isInSchedule
        ) {
            return string.Join(
                ":",
                "schedule",
                namespaceName,
                userId,
                isInSchedule.ToString(),
                "RepeatSchedule"
            );
        }

        public static string CacheKey(
            this RepeatSchedule self,
            string eventName
        ) {
            return string.Join(
                ":",
                eventName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RepeatSchedule> FetchFuture(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string eventName,
            bool isInSchedule,
            Func<IFuture<RepeatSchedule>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RepeatSchedule> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RepeatSchedule).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            eventName,
                            isInSchedule
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "event") {
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
                    eventName,
                    isInSchedule
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RepeatSchedule>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RepeatSchedule> FetchAsync(
    #else
        public static async Task<RepeatSchedule> FetchAsync(
    #endif
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string eventName,
            bool isInSchedule,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RepeatSchedule>> fetchImpl
    #else
            Func<Task<RepeatSchedule>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    eventName,
                    isInSchedule
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as RepeatSchedule).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    eventName,
                    isInSchedule
                );
                if (e.errors.Length == 0 || e.errors[0].component != "event") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<RepeatSchedule, bool> GetCache(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string eventName,
            bool isInSchedule
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<RepeatSchedule>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    isInSchedule
                ),
                self.CacheKey(
                    eventName
                )
            );
        }

        public static void PutCache(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string eventName,
            bool isInSchedule
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    isInSchedule
                ),
                self.CacheKey(
                    eventName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string eventName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<RepeatSchedule>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    false
                ),
                self.CacheKey(
                    eventName
                )
            );
            cache.Delete<RepeatSchedule>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    true
                ),
                self.CacheKey(
                    eventName
                )
            );
        }

        public static void ListSubscribe(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool isInSchedule,
            Action<RepeatSchedule[]> callback
        ) {
            cache.ListSubscribe<RepeatSchedule>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    isInSchedule
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this RepeatSchedule self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            bool isInSchedule,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RepeatSchedule>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    isInSchedule
                ),
                callbackId
            );
        }
    }
}