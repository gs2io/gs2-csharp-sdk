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

namespace Gs2.Gs2Guild.Model.Cache
{
    public static partial class ReceiveMemberRequestExt
    {
        public static string CacheParentKey(
            this ReceiveMemberRequest self,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                guildModelName,
                guildName,
                "ReceiveMemberRequest"
            );
        }

        public static string CacheKey(
            this ReceiveMemberRequest self,
            string fromUserId
        ) {
            return string.Join(
                ":",
                fromUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ReceiveMemberRequest> FetchFuture(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId,
            Func<IFuture<ReceiveMemberRequest>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ReceiveMemberRequest> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ReceiveMemberRequest).PutCache(
                            cache,
                            namespaceName,
                            guildModelName,
                            guildName,
                            fromUserId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "receiveMemberRequest") {
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
                    guildModelName,
                    guildName,
                    fromUserId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ReceiveMemberRequest>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ReceiveMemberRequest> FetchAsync(
    #else
        public static async Task<ReceiveMemberRequest> FetchAsync(
    #endif
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ReceiveMemberRequest>> fetchImpl
    #else
            Func<Task<ReceiveMemberRequest>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ReceiveMemberRequest>(
                       self.CacheParentKey(
                            namespaceName,
                            guildModelName,
                            guildName
                       ),
                       self.CacheKey(
                            fromUserId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        guildModelName,
                        guildName,
                        fromUserId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ReceiveMemberRequest).PutCache(
                        cache,
                        namespaceName,
                        guildModelName,
                        guildName,
                        fromUserId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "receiveMemberRequest") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ReceiveMemberRequest, bool> GetCache(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId
        ) {
            return cache.Get<ReceiveMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                    fromUserId
                )
            );
        }

        public static void PutCache(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                    fromUserId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            string fromUserId
        ) {
            cache.Delete<ReceiveMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                    fromUserId
                )
            );
        }

        public static void ListSubscribe(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            Action<ReceiveMemberRequest[]> callback
        ) {
            cache.ListSubscribe<ReceiveMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ReceiveMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ReceiveMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                callbackId
            );
        }
    }
}