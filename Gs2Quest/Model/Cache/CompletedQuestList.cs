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
    public static partial class CompletedQuestListExt
    {
        public static string CacheParentKey(
            this CompletedQuestList self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "quest",
                namespaceName,
                userId,
                "CompletedQuestList"
            );
        }

        public static string CacheKey(
            this CompletedQuestList self,
            string questGroupName
        ) {
            return string.Join(
                ":",
                questGroupName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CompletedQuestList> FetchFuture(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string questGroupName,
            Func<IFuture<CompletedQuestList>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CompletedQuestList> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CompletedQuestList).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            questGroupName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "completedQuestList") {
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
                    userId,
                    questGroupName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CompletedQuestList>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CompletedQuestList> FetchAsync(
    #else
        public static async Task<CompletedQuestList> FetchAsync(
    #endif
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string questGroupName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CompletedQuestList>> fetchImpl
    #else
            Func<Task<CompletedQuestList>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<CompletedQuestList>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            questGroupName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        questGroupName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as CompletedQuestList).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        questGroupName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "completedQuestList") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<CompletedQuestList, bool> GetCache(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string questGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<CompletedQuestList>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    questGroupName
                )
            );
        }

        public static void PutCache(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string questGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<CompletedQuestList>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    questGroupName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    questGroupName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string questGroupName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<CompletedQuestList>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    questGroupName
                )
            );
        }

        public static void ListSubscribe(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<CompletedQuestList[]> callback
        ) {
            cache.ListSubscribe<CompletedQuestList>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this CompletedQuestList self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CompletedQuestList>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}