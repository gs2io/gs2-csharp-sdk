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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static partial class MissionTaskModelMasterExt
    {
        public static string CacheParentKey(
            this MissionTaskModelMaster self,
            string namespaceName,
            string missionGroupName
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                missionGroupName,
                "MissionTaskModelMaster"
            );
        }

        public static string CacheKey(
            this MissionTaskModelMaster self,
            string missionTaskName
        ) {
            return string.Join(
                ":",
                missionTaskName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MissionTaskModelMaster> FetchFuture(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            Func<IFuture<MissionTaskModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MissionTaskModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MissionTaskModelMaster).PutCache(
                            cache,
                            namespaceName,
                            missionGroupName,
                            missionTaskName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "missionTaskModelMaster") {
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
                    missionTaskName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MissionTaskModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MissionTaskModelMaster> FetchAsync(
    #else
        public static async Task<MissionTaskModelMaster> FetchAsync(
    #endif
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MissionTaskModelMaster>> fetchImpl
    #else
            Func<Task<MissionTaskModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<MissionTaskModelMaster>(
                       self.CacheParentKey(
                            namespaceName,
                            missionGroupName
                       ),
                       self.CacheKey(
                            missionTaskName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        missionGroupName,
                        missionTaskName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as MissionTaskModelMaster).PutCache(
                        cache,
                        namespaceName,
                        missionGroupName,
                        missionTaskName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "missionTaskModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<MissionTaskModelMaster, bool> GetCache(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName
        ) {
            return cache.Get<MissionTaskModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                self.CacheKey(
                    missionTaskName
                )
            );
        }

        public static void PutCache(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName
        ) {
            var (value, find) = cache.Get<MissionTaskModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                self.CacheKey(
                    missionTaskName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                self.CacheKey(
                    missionTaskName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName
        ) {
            cache.Delete<MissionTaskModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                self.CacheKey(
                    missionTaskName
                )
            );
        }

        public static void ListSubscribe(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            Action<MissionTaskModelMaster[]> callback
        ) {
            cache.ListSubscribe<MissionTaskModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this MissionTaskModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MissionTaskModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName
                ),
                callbackId
            );
        }
    }
}