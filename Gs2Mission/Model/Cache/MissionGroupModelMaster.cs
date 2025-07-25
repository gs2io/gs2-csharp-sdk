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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static partial class MissionGroupModelMasterExt
    {
        public static string CacheParentKey(
            this MissionGroupModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "MissionGroupModelMaster"
            );
        }

        public static string CacheKey(
            this MissionGroupModelMaster self,
            string missionGroupName
        ) {
            return string.Join(
                ":",
                missionGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MissionGroupModelMaster> FetchFuture(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset,
            Func<IFuture<MissionGroupModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MissionGroupModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MissionGroupModelMaster).PutCache(
                            cache,
                            namespaceName,
                            missionGroupName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "missionGroupModelMaster") {
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
                    missionGroupName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MissionGroupModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MissionGroupModelMaster> FetchAsync(
    #else
        public static async Task<MissionGroupModelMaster> FetchAsync(
    #endif
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MissionGroupModelMaster>> fetchImpl
    #else
            Func<Task<MissionGroupModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    missionGroupName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as MissionGroupModelMaster).PutCache(
                    cache,
                    namespaceName,
                    missionGroupName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "missionGroupModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<MissionGroupModelMaster, bool> GetCache(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset
        ) {
            return cache.Get<MissionGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void PutCache(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<MissionGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    missionGroupName
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
                    missionGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset
        ) {
            cache.Delete<MissionGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void ListSubscribe(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<MissionGroupModelMaster[]> callback
        ) {
            cache.ListSubscribe<MissionGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this MissionGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MissionGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}