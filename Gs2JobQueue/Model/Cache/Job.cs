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

namespace Gs2.Gs2JobQueue.Model.Cache
{
    public static partial class JobExt
    {
        public static string CacheParentKey(
            this Job self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Job"
            );
        }

        public static string CacheKey(
            this Job self,
            string jobName
        ) {
            return string.Join(
                ":",
                jobName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Job> FetchFuture(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? timeOffset,
            Func<IFuture<Job>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Job> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Job).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            jobName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "job") {
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
                    jobName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Job>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Job> FetchAsync(
    #else
        public static async Task<Job> FetchAsync(
    #endif
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Job>> fetchImpl
    #else
            Func<Task<Job>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    jobName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Job).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    jobName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "job") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Job, bool> GetCache(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Job>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    jobName
                )
            );
        }

        public static void PutCache(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    jobName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Job>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    jobName
                )
            );
        }

        public static void ListSubscribe(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Job[]> callback
        ) {
            cache.ListSubscribe<Job>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Job self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Job>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}