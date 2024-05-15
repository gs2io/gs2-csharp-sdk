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
    public static partial class GuildModelMasterExt
    {
        public static string CacheParentKey(
            this GuildModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                "GuildModelMaster"
            );
        }

        public static string CacheKey(
            this GuildModelMaster self,
            string guildModelName
        ) {
            return string.Join(
                ":",
                guildModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GuildModelMaster> FetchFuture(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            Func<IFuture<GuildModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GuildModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GuildModelMaster).PutCache(
                            cache,
                            namespaceName,
                            guildModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "guildModelMaster") {
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
                    guildModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GuildModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GuildModelMaster> FetchAsync(
    #else
        public static async Task<GuildModelMaster> FetchAsync(
    #endif
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GuildModelMaster>> fetchImpl
    #else
            Func<Task<GuildModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GuildModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            guildModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        guildModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GuildModelMaster).PutCache(
                        cache,
                        namespaceName,
                        guildModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "guildModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GuildModelMaster, bool> GetCache(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
            return cache.Get<GuildModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName
                )
            );
        }

        public static void PutCache(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
            var (value, find) = cache.Get<GuildModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName
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
                    guildModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
            cache.Delete<GuildModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName
                )
            );
        }

        public static void ListSubscribe(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<GuildModelMaster[]> callback
        ) {
            cache.ListSubscribe<GuildModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GuildModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GuildModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}