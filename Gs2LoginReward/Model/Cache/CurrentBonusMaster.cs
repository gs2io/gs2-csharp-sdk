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

namespace Gs2.Gs2LoginReward.Model.Cache
{
    public static partial class CurrentBonusMasterExt
    {
        public static string CacheParentKey(
            this CurrentBonusMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "loginReward",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "CurrentBonusMaster"
            );
        }

        public static string CacheKey(
            this CurrentBonusMaster self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CurrentBonusMaster> FetchFuture(
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Func<IFuture<CurrentBonusMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CurrentBonusMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CurrentBonusMaster).PutCache(
                            cache,
                            namespaceName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "currentBonusMaster") {
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
            return new Gs2InlineFuture<CurrentBonusMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CurrentBonusMaster> FetchAsync(
    #else
        public static async Task<CurrentBonusMaster> FetchAsync(
    #endif
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CurrentBonusMaster>> fetchImpl
    #else
            Func<Task<CurrentBonusMaster>> fetchImpl
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
                (null as CurrentBonusMaster).PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "currentBonusMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<CurrentBonusMaster, bool> GetCache(
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            return cache.Get<CurrentBonusMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this CurrentBonusMaster self,
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
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Delete<CurrentBonusMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<CurrentBonusMaster[]> callback
        ) {
            cache.ListSubscribe<CurrentBonusMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this CurrentBonusMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CurrentBonusMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}