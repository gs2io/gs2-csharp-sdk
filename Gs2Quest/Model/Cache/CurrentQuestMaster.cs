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

#pragma warning disable CS0618 // Obsolete with a message
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

namespace Gs2.Gs2Quest.Model.Cache
{
    public static partial class CurrentQuestMasterExt
    {
        public static string CacheParentKey(
            this CurrentQuestMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "CurrentQuestMaster"
            );
        }

        public static string CacheKey(
            this CurrentQuestMaster self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CurrentQuestMaster> FetchFuture(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Func<IFuture<CurrentQuestMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CurrentQuestMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CurrentQuestMaster).PutCache(
                            cache,
                            namespaceName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "currentQuestMaster") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CurrentQuestMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CurrentQuestMaster> FetchAsync(
    #else
        public static async Task<CurrentQuestMaster> FetchAsync(
    #endif
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CurrentQuestMaster>> fetchImpl
    #else
            Func<Task<CurrentQuestMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as CurrentQuestMaster).PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "currentQuestMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<CurrentQuestMaster, bool> GetCache(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            return cache.Get<CurrentQuestMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Delete<CurrentQuestMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<CurrentQuestMaster[]> callback
        ) {
            cache.ListSubscribe<CurrentQuestMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this CurrentQuestMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CurrentQuestMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}