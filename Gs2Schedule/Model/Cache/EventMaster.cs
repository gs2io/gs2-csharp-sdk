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

namespace Gs2.Gs2Schedule.Model.Cache
{
    public static partial class EventMasterExt
    {
        public static string CacheParentKey(
            this EventMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "schedule",
                namespaceName,
                "EventMaster"
            );
        }

        public static string CacheKey(
            this EventMaster self,
            string eventName
        ) {
            return string.Join(
                ":",
                eventName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<EventMaster> FetchFuture(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            string eventName,
            Func<IFuture<EventMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<EventMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as EventMaster).PutCache(
                            cache,
                            namespaceName,
                            eventName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "eventMaster") {
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
                    eventName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<EventMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<EventMaster> FetchAsync(
    #else
        public static async Task<EventMaster> FetchAsync(
    #endif
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            string eventName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<EventMaster>> fetchImpl
    #else
            Func<Task<EventMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<EventMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            eventName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        eventName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as EventMaster).PutCache(
                        cache,
                        namespaceName,
                        eventName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "eventMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<EventMaster, bool> GetCache(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            string eventName
        ) {
            return cache.Get<EventMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    eventName
                )
            );
        }

        public static void PutCache(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            string eventName
        ) {
            var (value, find) = cache.Get<EventMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    eventName
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
                    eventName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            string eventName
        ) {
            cache.Delete<EventMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    eventName
                )
            );
        }

        public static void ListSubscribe(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<EventMaster[]> callback
        ) {
            cache.ListSubscribe<EventMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this EventMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<EventMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}