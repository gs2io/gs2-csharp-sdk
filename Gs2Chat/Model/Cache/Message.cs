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

namespace Gs2.Gs2Chat.Model.Cache
{
    public static partial class MessageExt
    {
        public static string CacheParentKey(
            this Message self,
            string namespaceName,
            string userId,
            string roomName
        ) {
            return string.Join(
                ":",
                "chat",
                namespaceName,
                "Singleton",
                roomName,
                "Message"
            );
        }

        public static string CacheKey(
            this Message self,
            string messageName
        ) {
            return string.Join(
                ":",
                messageName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Message> FetchFuture(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            string messageName,
            Func<IFuture<Message>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Message> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Message).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            roomName,
                            messageName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "message") {
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
                    roomName,
                    messageName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Message>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Message> FetchAsync(
    #else
        public static async Task<Message> FetchAsync(
    #endif
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            string messageName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Message>> fetchImpl
    #else
            Func<Task<Message>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Message>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            roomName
                       ),
                       self.CacheKey(
                            messageName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        roomName,
                        messageName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Message).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        roomName,
                        messageName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "message") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Message, bool> GetCache(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            string messageName
        ) {
            return cache.Get<Message>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                self.CacheKey(
                    messageName
                )
            );
        }

        public static void PutCache(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            string messageName
        ) {
            var (value, find) = cache.Get<Message>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                self.CacheKey(
                    messageName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                self.CacheKey(
                    messageName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            string messageName
        ) {
            cache.Delete<Message>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                self.CacheKey(
                    messageName
                )
            );
        }

        public static void ListSubscribe(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            Action<Message[]> callback
        ) {
            cache.ListSubscribe<Message>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Message self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string roomName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Message>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    roomName
                ),
                callbackId
            );
        }
    }
}