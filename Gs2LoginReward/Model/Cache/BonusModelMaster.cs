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
    public static partial class BonusModelMasterExt
    {
        public static string CacheParentKey(
            this BonusModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "loginReward",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "BonusModelMaster"
            );
        }

        public static string CacheKey(
            this BonusModelMaster self,
            string bonusModelName
        ) {
            return string.Join(
                ":",
                bonusModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BonusModelMaster> FetchFuture(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string bonusModelName,
            int? timeOffset,
            Func<IFuture<BonusModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BonusModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BonusModelMaster).PutCache(
                            cache,
                            namespaceName,
                            bonusModelName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "bonusModelMaster") {
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
                    bonusModelName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BonusModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BonusModelMaster> FetchAsync(
    #else
        public static async Task<BonusModelMaster> FetchAsync(
    #endif
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string bonusModelName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BonusModelMaster>> fetchImpl
    #else
            Func<Task<BonusModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    bonusModelName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as BonusModelMaster).PutCache(
                    cache,
                    namespaceName,
                    bonusModelName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "bonusModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<BonusModelMaster, bool> GetCache(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string bonusModelName,
            int? timeOffset
        ) {
            return cache.Get<BonusModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    bonusModelName
                )
            );
        }

        public static void PutCache(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string bonusModelName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<BonusModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    bonusModelName
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    bonusModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string bonusModelName,
            int? timeOffset
        ) {
            cache.Delete<BonusModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    bonusModelName
                )
            );
        }

        public static void ListSubscribe(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<BonusModelMaster[]> callback
        ) {
            cache.ListSubscribe<BonusModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this BonusModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BonusModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}