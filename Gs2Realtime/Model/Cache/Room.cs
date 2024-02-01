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

namespace Gs2.Gs2Realtime.Model.Cache
{
    public static partial class RoomExt
    {
        public static string CacheParentKey(
            this Room self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "realtime",
                namespaceName,
                "Room"
            );
        }

        public static string CacheKey(
            this Room self,
            string roomName
        ) {
            return string.Join(
                ":",
                roomName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Room> FetchFuture(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            string roomName,
            Func<IFuture<Room>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Room> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Room).PutCache(
                            cache,
                            namespaceName,
                            roomName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "room") {
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
                    roomName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Room>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Room> FetchAsync(
    #else
        public static async Task<Room> FetchAsync(
    #endif
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            string roomName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Room>> fetchImpl
    #else
            Func<Task<Room>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Room>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            roomName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        roomName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Room).PutCache(
                        cache,
                        namespaceName,
                        roomName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "room") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Room, bool> GetCache(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            string roomName
        ) {
            return cache.Get<Room>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    roomName
                )
            );
        }

        public static void PutCache(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            string roomName
        ) {
            var (value, find) = cache.Get<Room>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    roomName
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
                    roomName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            string roomName
        ) {
            cache.Delete<Room>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    roomName
                )
            );
        }

        public static void ListSubscribe(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            Action<Room[]> callback
        ) {
            cache.ListSubscribe<Room>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Room self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Room>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}