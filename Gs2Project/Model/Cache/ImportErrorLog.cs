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
    public static partial class ImportErrorLogExt
    {
        public static string CacheParentKey(
            this ImportErrorLog self,
            string accountName,
            string projectName,
            string transactionId
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                projectName,
                transactionId,
                "ImportErrorLog"
            );
        }

        public static string CacheKey(
            this ImportErrorLog self,
            string errorLogName
        ) {
            return string.Join(
                ":",
                errorLogName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ImportErrorLog> FetchFuture(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            string errorLogName,
            Func<IFuture<ImportErrorLog>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ImportErrorLog> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ImportErrorLog).PutCache(
                            cache,
                            accountName,
                            projectName,
                            transactionId,
                            errorLogName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "importErrorLog") {
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
                    transactionId,
                    errorLogName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ImportErrorLog>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ImportErrorLog> FetchAsync(
    #else
        public static async Task<ImportErrorLog> FetchAsync(
    #endif
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            string errorLogName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ImportErrorLog>> fetchImpl
    #else
            Func<Task<ImportErrorLog>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ImportErrorLog>(
                       self.CacheParentKey(
                            accountName,
                            projectName,
                            transactionId
                       ),
                       self.CacheKey(
                            errorLogName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        accountName,
                        projectName,
                        transactionId,
                        errorLogName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ImportErrorLog).PutCache(
                        cache,
                        accountName,
                        projectName,
                        transactionId,
                        errorLogName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "importErrorLog") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ImportErrorLog, bool> GetCache(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            string errorLogName
        ) {
            return cache.Get<ImportErrorLog>(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                self.CacheKey(
                    errorLogName
                )
            );
        }

        public static void PutCache(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            string errorLogName
        ) {
            var (value, find) = cache.Get<ImportErrorLog>(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                self.CacheKey(
                    errorLogName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                self.CacheKey(
                    errorLogName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            string errorLogName
        ) {
            cache.Delete<ImportErrorLog>(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                self.CacheKey(
                    errorLogName
                )
            );
        }

        public static void ListSubscribe(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            Action<ImportErrorLog[]> callback
        ) {
            cache.ListSubscribe<ImportErrorLog>(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ImportErrorLog self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            string transactionId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ImportErrorLog>(
                self.CacheParentKey(
                    accountName,
                    projectName,
                    transactionId
                ),
                callbackId
            );
        }
    }
}