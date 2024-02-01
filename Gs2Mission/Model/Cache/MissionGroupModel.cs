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
    public static partial class MissionGroupModelExt
    {
        public static string CacheParentKey(
            this MissionGroupModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                "MissionGroupModel"
            );
        }

        public static string CacheKey(
            this MissionGroupModel self,
            string missionGroupName
        ) {
            return string.Join(
                ":",
                missionGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MissionGroupModel> FetchFuture(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
            Func<IFuture<MissionGroupModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MissionGroupModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MissionGroupModel).PutCache(
                            cache,
                            namespaceName,
                            missionGroupName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "missionGroupModel") {
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
                    missionGroupName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MissionGroupModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MissionGroupModel> FetchAsync(
    #else
        public static async Task<MissionGroupModel> FetchAsync(
    #endif
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MissionGroupModel>> fetchImpl
    #else
            Func<Task<MissionGroupModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<MissionGroupModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            missionGroupName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        missionGroupName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as MissionGroupModel).PutCache(
                        cache,
                        namespaceName,
                        missionGroupName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "missionGroupModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<MissionGroupModel, bool> GetCache(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName
        ) {
            return cache.Get<MissionGroupModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void PutCache(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    missionGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            string missionGroupName
        ) {
            cache.Delete<MissionGroupModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    missionGroupName
                )
            );
        }

        public static void ListSubscribe(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<MissionGroupModel[]> callback
        ) {
            cache.ListSubscribe<MissionGroupModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this MissionGroupModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MissionGroupModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}