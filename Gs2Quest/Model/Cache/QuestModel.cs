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
    public static partial class QuestModelExt
    {
        public static string CacheParentKey(
            this QuestModel self,
            string namespaceName,
            string questGroupName
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                questGroupName,
                "QuestModel"
            );
        }

        public static string CacheKey(
            this QuestModel self,
            string questName
        ) {
            return string.Join(
                ":",
                questName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<QuestModel> FetchFuture(
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
            Func<IFuture<QuestModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<QuestModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as QuestModel).PutCache(
                            cache,
                            namespaceName,
                            questGroupName,
                            questName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "questModel") {
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
            return new Gs2InlineFuture<QuestModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<QuestModel> FetchAsync(
    #else
        public static async Task<QuestModel> FetchAsync(
    #endif
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<QuestModel>> fetchImpl
    #else
            Func<Task<QuestModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<QuestModel>(
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
                    (null as QuestModel).PutCache(
                        cache,
                        namespaceName,
                        questGroupName,
                        questName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "questModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<QuestModel, bool> GetCache(
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName
        ) {
            return cache.Get<QuestModel>(
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
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName
        ) {
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
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            string questName
        ) {
            cache.Delete<QuestModel>(
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
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            Action<QuestModel[]> callback
        ) {
            cache.ListSubscribe<QuestModel>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this QuestModel self,
            CacheDatabase cache,
            string namespaceName,
            string questGroupName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<QuestModel>(
                self.CacheParentKey(
                    namespaceName,
                    questGroupName
                ),
                callbackId
            );
        }
    }
}