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
    public static partial class GuildExt
    {
        public static string CacheParentKey(
            this Guild self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                "Guild"
            );
        }

        public static string CacheKey(
            this Guild self,
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
        public static IFuture<Guild> FetchFuture(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            Func<IFuture<Guild>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Guild> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Guild).PutCache(
                            cache,
                            namespaceName,
                            guildModelName,
                            guildName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "guild") {
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
                    guildName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Guild>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Guild> FetchAsync(
    #else
        public static async Task<Guild> FetchAsync(
    #endif
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Guild>> fetchImpl
    #else
            Func<Task<Guild>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Guild>(
                       self.CacheParentKey(
                            namespaceName
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
                        guildModelName,
                        guildName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Guild).PutCache(
                        cache,
                        namespaceName,
                        guildModelName,
                        guildName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "guild") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Guild, bool> GetCache(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            return cache.Get<Guild>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
                )
            );
        }

        public static void PutCache(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            var (value, find) = cache.Get<Guild>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
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
                    guildModelName,
                    guildName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            cache.Delete<Guild>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName,
                    guildName
                )
            );
        }

        public static void ListSubscribe(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            Action<Guild[]> callback
        ) {
            cache.ListSubscribe<Guild>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Guild self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Guild>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}