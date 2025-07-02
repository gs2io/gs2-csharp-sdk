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
    public static partial class MissionTaskModelExt
    {
        public static string CacheParentKey(
            this MissionTaskModel self,
            string namespaceName,
            string missionGroupName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                missionGroupName,
                timeOffset?.ToString() ?? "0",
                "MissionTaskModel"
            );
        }

        public static string CacheKey(
            this MissionTaskModel self,
            string missionTaskName
        ) {
            return string.Join(
                ":",
                missionTaskName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MissionTaskModel> FetchFuture(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            int? timeOffset,
            Func<IFuture<MissionTaskModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MissionTaskModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MissionTaskModel).PutCache(
                            cache,
                            namespaceName,
                            missionGroupName,
                            missionTaskName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "missionTaskModel") {
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
                    missionTaskName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MissionTaskModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MissionTaskModel> FetchAsync(
    #else
        public static async Task<MissionTaskModel> FetchAsync(
    #endif
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MissionTaskModel>> fetchImpl
    #else
            Func<Task<MissionTaskModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    missionGroupName,
                    missionTaskName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as MissionTaskModel).PutCache(
                    cache,
                    namespaceName,
                    missionGroupName,
                    missionTaskName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "missionTaskModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<MissionTaskModel, bool> GetCache(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            int? timeOffset
        ) {
            return cache.Get<MissionTaskModel>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    missionTaskName
                )
            );
        }

        public static void PutCache(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    missionTaskName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            string missionTaskName,
            int? timeOffset
        ) {
            cache.Delete<MissionTaskModel>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    missionTaskName
                )
            );
        }

        public static void ListSubscribe(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset,
            Action<MissionTaskModel[]> callback
        ) {
            cache.ListSubscribe<MissionTaskModel>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this MissionTaskModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MissionTaskModel>(
                self.CacheParentKey(
                    namespaceName,
                    missionGroupName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}