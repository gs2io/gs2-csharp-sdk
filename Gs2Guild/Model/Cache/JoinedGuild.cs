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

namespace Gs2.Gs2Guild.Model.Cache
{
    public static partial class JoinedGuildExt
    {
        public static string CacheParentKey(
            this JoinedGuild self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                userId,
                "JoinedGuild"
            );
        }

        public static string CacheKey(
            this JoinedGuild self,
            string guildModelName,
            string guildName
        ) {
            return string.Join(
                ":",
                guildModelName,
                guildName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<JoinedGuild> FetchFuture(
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName,
            Func<IFuture<JoinedGuild>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<JoinedGuild> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as JoinedGuild).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            guildModelName,
                            guildName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "joinedGuild") {
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
            return new Gs2InlineFuture<JoinedGuild>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<JoinedGuild> FetchAsync(
    #else
        public static async Task<JoinedGuild> FetchAsync(
    #endif
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<JoinedGuild>> fetchImpl
    #else
            Func<Task<JoinedGuild>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<JoinedGuild>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            guildModelName,
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
                    (null as JoinedGuild).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        guildModelName,
                        guildName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "joinedGuild") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<JoinedGuild, bool> GetCache(
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<JoinedGuild>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
                )
            );
        }

        public static void PutCache(
            this JoinedGuild self,
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
                    userId
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string guildModelName,
            string guildName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<JoinedGuild>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
                )
            );
        }

        public static void ListSubscribe(
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<JoinedGuild[]> callback
        ) {
            cache.ListSubscribe<JoinedGuild>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this JoinedGuild self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<JoinedGuild>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}