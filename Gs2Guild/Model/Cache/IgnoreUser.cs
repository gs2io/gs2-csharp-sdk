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
    public static partial class IgnoreUserExt
    {
        public static string CacheParentKey(
            this IgnoreUser self,
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
                "IgnoreUser"
            );
        }

        public static string CacheKey(
            this IgnoreUser self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<IgnoreUser> FetchFuture(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            Func<IFuture<IgnoreUser>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<IgnoreUser> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as IgnoreUser).PutCache(
                            cache,
                            namespaceName,
                            guildModelName,
                            guildName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "ignoreUser") {
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
            return new Gs2InlineFuture<IgnoreUser>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<IgnoreUser> FetchAsync(
    #else
        public static async Task<IgnoreUser> FetchAsync(
    #endif
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<IgnoreUser>> fetchImpl
    #else
            Func<Task<IgnoreUser>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<IgnoreUser>(
                       self.CacheParentKey(
                            namespaceName,
                            guildModelName,
                            guildName
                       ),
                       self.CacheKey(
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
                    (null as IgnoreUser).PutCache(
                        cache,
                        namespaceName,
                        guildModelName,
                        guildName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "ignoreUser") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<IgnoreUser, bool> GetCache(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            return cache.Get<IgnoreUser>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName
        ) {
            cache.Delete<IgnoreUser>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            Action<IgnoreUser[]> callback
        ) {
            cache.ListSubscribe<IgnoreUser>(
                self.CacheParentKey(
                    namespaceName,
                    guildModelName,
                    guildName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this IgnoreUser self,
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<IgnoreUser>(
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