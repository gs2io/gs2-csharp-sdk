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
    public static partial class JobResultExt
    {
        public static string CacheParentKey(
            this JobResult self,
            string namespaceName,
            string userId,
            string jobName
        ) {
            return string.Join(
                ":",
                "jobQueue",
                namespaceName,
                userId,
                jobName,
                "JobResult"
            );
        }

        public static string CacheKey(
            this JobResult self,
            int? tryNumber
        ) {
            return string.Join(
                ":",
                tryNumber.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<JobResult> FetchFuture(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber,
            Func<IFuture<JobResult>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<JobResult> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as JobResult).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            jobName,
                            tryNumber
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "jobResult") {
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
                    tryNumber
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<JobResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<JobResult> FetchAsync(
    #else
        public static async Task<JobResult> FetchAsync(
    #endif
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<JobResult>> fetchImpl
    #else
            Func<Task<JobResult>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<JobResult>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            jobName
                       ),
                       self.CacheKey(
                            tryNumber
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        jobName,
                        tryNumber
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as JobResult).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        jobName,
                        tryNumber
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "jobResult") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<JobResult, bool> GetCache(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<JobResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    jobName
                ),
                self.CacheKey(
                    tryNumber
                )
            );
        }

        public static void PutCache(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    jobName
                ),
                self.CacheKey(
                    tryNumber
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            int? tryNumber
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<JobResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    jobName
                ),
                self.CacheKey(
                    tryNumber
                )
            );
        }

        public static void ListSubscribe(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            Action<JobResult[]> callback
        ) {
            cache.ListSubscribe<JobResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    jobName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this JobResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string jobName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<JobResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    jobName
                ),
                callbackId
            );
        }
    }
}