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
    public static partial class GuildModelExt
    {
        public static string CacheParentKey(
            this GuildModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "guild",
                namespaceName,
                "GuildModel"
            );
        }

        public static string CacheKey(
            this GuildModel self,
            string guildModelName
        ) {
            return string.Join(
                ":",
                guildModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GuildModel> FetchFuture(
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            Func<IFuture<GuildModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GuildModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GuildModel).PutCache(
                            cache,
                            namespaceName,
                            guildModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "guildModel") {
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
            return new Gs2InlineFuture<GuildModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GuildModel> FetchAsync(
    #else
        public static async Task<GuildModel> FetchAsync(
    #endif
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GuildModel>> fetchImpl
    #else
            Func<Task<GuildModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GuildModel>(
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
                    (null as GuildModel).PutCache(
                        cache,
                        namespaceName,
                        guildModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "guildModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GuildModel, bool> GetCache(
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
            return cache.Get<GuildModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName
                )
            );
        }

        public static void PutCache(
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
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
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName
        ) {
            cache.Delete<GuildModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    guildModelName
                )
            );
        }

        public static void ListSubscribe(
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<GuildModel[]> callback
        ) {
            cache.ListSubscribe<GuildModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GuildModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GuildModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}