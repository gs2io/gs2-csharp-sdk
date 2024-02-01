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

namespace Gs2.Gs2Quest.Model.Cache
{
    public static partial class QuestModelMasterExt
    {
        public static string CacheParentKey(
            this QuestModelMaster self,
            string namespaceName,
            string questGroupName
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                questGroupName,
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
                            questName
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
                    questName
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
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<QuestModelMaster>> fetchImpl
    #else
            Func<Task<QuestModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<QuestModelMaster>(
                       self.CacheParentKey(
                            namespaceName,
                            questGroupName
                       ),
                       self.CacheKey(
                            questName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        questGroupName,
                        questName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as QuestModelMaster).PutCache(
                        cache,
                        namespaceName,
                        questGroupName,
                        questName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "questModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<QuestModelMaster, bool> GetCache(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName
        ) {
            return cache.Get<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
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
            string questName
        ) {
            var (value, find) = cache.Get<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
                ),
                self.CacheKey(
                    questName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
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
            string questName
        ) {
            cache.Delete<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
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
            Action<QuestModelMaster[]> callback
        ) {
            cache.ListSubscribe<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this QuestModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<QuestModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
                ),
                callbackId
            );
        }
    }
}