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
    public static partial class SendMemberRequestExt
    {
        public static string CacheParentKey(
            this SendMemberRequest self,
            string namespaceName,
            string guildModelName,
            string userId
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                guildModelName,
                userId,
                "SendMemberRequest"
            );
        }

        public static string CacheKey(
            this SendMemberRequest self,
            string guildName
        ) {
            return string.Join(
                ":",
                guildName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SendMemberRequest> FetchFuture(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName,
            Func<IFuture<SendMemberRequest>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SendMemberRequest> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SendMemberRequest).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            guildModelName,
                            guildName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "sendMemberRequest") {
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
                    guildModelName,
                    guildName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SendMemberRequest>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SendMemberRequest> FetchAsync(
    #else
        public static async Task<SendMemberRequest> FetchAsync(
    #endif
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SendMemberRequest>> fetchImpl
    #else
            Func<Task<SendMemberRequest>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SendMemberRequest>(
                       self.CacheParentKey(
                            namespaceName,
                            guildModelName,
                            userId
                       ),
                       self.CacheKey(
                            guildName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        guildModelName,
                        guildName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SendMemberRequest).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        guildModelName,
                        guildName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "sendMemberRequest") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SendMemberRequest, bool> GetCache(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SendMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    userId
                ),
                self.CacheKey(
                    guildName
                )
            );
        }

        public static void PutCache(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    userId
                ),
                self.CacheKey(
                    guildName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SendMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    userId
                ),
                self.CacheKey(
                    guildName
                )
            );
        }

        public static void ListSubscribe(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string userId,
            Action<SendMemberRequest[]> callback
        ) {
            cache.ListSubscribe<SendMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SendMemberRequest self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SendMemberRequest>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    userId
                ),
                callbackId
            );
        }
    }
}