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
    public static partial class QuestGroupModelMasterExt
    {
        public static string CacheParentKey(
            this QuestGroupModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "QuestGroupModelMaster"
            );
        }

        public static string CacheKey(
            this QuestGroupModelMaster self,
            string questGroupName
        ) {
            return string.Join(
                ":",
                questGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<QuestGroupModelMaster> FetchFuture(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset,
            Func<IFuture<QuestGroupModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<QuestGroupModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as QuestGroupModelMaster).PutCache(
                            cache,
                            namespaceName,
                            questGroupName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "questGroupModelMaster") {
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
                    questGroupName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<QuestGroupModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<QuestGroupModelMaster> FetchAsync(
    #else
        public static async Task<QuestGroupModelMaster> FetchAsync(
    #endif
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<QuestGroupModelMaster>> fetchImpl
    #else
            Func<Task<QuestGroupModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    questGroupName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as QuestGroupModelMaster).PutCache(
                    cache,
                    namespaceName,
                    questGroupName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "questGroupModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<QuestGroupModelMaster, bool> GetCache(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset
        ) {
            return cache.Get<QuestGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    questGroupName
                )
            );
        }

        public static void PutCache(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<QuestGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    questGroupName
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
                    questGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset
        ) {
            cache.Delete<QuestGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    questGroupName
                )
            );
        }

        public static void ListSubscribe(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<QuestGroupModelMaster[]> callback
        ) {
            cache.ListSubscribe<QuestGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this QuestGroupModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<QuestGroupModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}