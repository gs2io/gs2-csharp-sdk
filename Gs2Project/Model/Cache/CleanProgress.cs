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

namespace Gs2.Gs2Project.Model.Cache
{
    public static partial class CleanProgressExt
    {
        public static string CacheParentKey(
            this CleanProgress self,
            string accountName,
            string projectName
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                projectName,
                "CleanProgress"
            );
        }

        public static string CacheKey(
            this CleanProgress self,
            string transactionId
        ) {
            return string.Join(
                ":",
                transactionId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CleanProgress> FetchFuture(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            Func<IFuture<CleanProgress>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CleanProgress> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CleanProgress).PutCache(
                            cache,
                            accountName,
                            projectName,
                            transactionId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "cleanProgress") {
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
                    accountName,
                    projectName,
                    transactionId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CleanProgress>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CleanProgress> FetchAsync(
    #else
        public static async Task<CleanProgress> FetchAsync(
    #endif
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CleanProgress>> fetchImpl
    #else
            Func<Task<CleanProgress>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<CleanProgress>(
                       self.CacheParentKey(
                            accountName,
                            projectName
                       ),
                       self.CacheKey(
                            transactionId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName,
                        projectName,
                        transactionId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as CleanProgress).PutCache(
                        cache,
                        accountName,
                        projectName,
                        transactionId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "cleanProgress") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<CleanProgress, bool> GetCache(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId
        ) {
            return cache.Get<CleanProgress>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void PutCache(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId
        ) {
            var (value, find) = cache.Get<CleanProgress>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    transactionId
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    transactionId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId
        ) {
            cache.Delete<CleanProgress>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void ListSubscribe(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            Action<CleanProgress[]> callback
        ) {
            cache.ListSubscribe<CleanProgress>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this CleanProgress self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CleanProgress>(
                self.CacheParentKey(
                    accountName,
                    projectName
                ),
                callbackId
            );
        }
    }
}