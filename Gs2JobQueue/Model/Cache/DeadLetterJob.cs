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

namespace Gs2.Gs2JobQueue.Model.Cache
{
    public static partial class DeadLetterJobExt
    {
        public static string CacheParentKey(
            this DeadLetterJob self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName,
                userId,
                "DeadLetterJob"
            );
        }

        public static string CacheKey(
            this DeadLetterJob self,
            string deadLetterJobName
        ) {
            return string.Join(
                ":",
                deadLetterJobName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DeadLetterJob> FetchFuture(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string deadLetterJobName,
            Func<IFuture<DeadLetterJob>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DeadLetterJob> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DeadLetterJob).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            deadLetterJobName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "deadLetterJob") {
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
                    deadLetterJobName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DeadLetterJob>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DeadLetterJob> FetchAsync(
    #else
        public static async Task<DeadLetterJob> FetchAsync(
    #endif
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string deadLetterJobName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DeadLetterJob>> fetchImpl
    #else
            Func<Task<DeadLetterJob>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<DeadLetterJob>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            deadLetterJobName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        deadLetterJobName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as DeadLetterJob).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        deadLetterJobName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "deadLetterJob") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<DeadLetterJob, bool> GetCache(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string deadLetterJobName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<DeadLetterJob>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    deadLetterJobName
                )
            );
        }

        public static void PutCache(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string deadLetterJobName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    deadLetterJobName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string deadLetterJobName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<DeadLetterJob>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    deadLetterJobName
                )
            );
        }

        public static void ListSubscribe(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<DeadLetterJob[]> callback
        ) {
            cache.ListSubscribe<DeadLetterJob>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this DeadLetterJob self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DeadLetterJob>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}