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
    public static partial class QuestModelMasterExt
    {
        public static string CacheParentKey(
            this QuestModelMaster self,
            string namespaceName,
            string questGroupName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                questGroupName,
                timeOffset?.ToString() ?? "0",
                "QuestModelMaster"
            );
        }

        public static string CacheKey(
            this QuestModelMaster self,
            string questName
        ) {
            return string.Join(
                ":",
                questName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<QuestModelMaster> FetchFuture(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            int? timeOffset,
            Func<IFuture<QuestModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<QuestModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as QuestModelMaster).PutCache(
                            cache,
                            namespaceName,
                            questGroupName,
                            questName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "questModelMaster") {
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
                    questName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<QuestModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<QuestModelMaster> FetchAsync(
    #else
        public static async Task<QuestModelMaster> FetchAsync(
    #endif
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<QuestModelMaster>> fetchImpl
    #else
            Func<Task<QuestModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    questGroupName,
                    questName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as QuestModelMaster).PutCache(
                    cache,
                    namespaceName,
                    questGroupName,
                    questName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "questModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<QuestModelMaster, bool> GetCache(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            int? timeOffset
        ) {
            return cache.Get<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    questName
                )
            );
        }

        public static void PutCache(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    questName
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
                    questGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    questName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            int? timeOffset
        ) {
            cache.Delete<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName,
                    timeOffset
                ),
                self.CacheKey(
                    questName
                )
            );
        }

        public static void ListSubscribe(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset,
            Action<QuestModelMaster[]> callback
        ) {
            cache.ListSubscribe<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}