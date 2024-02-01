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

namespace Gs2.Gs2Stamina.Model.Cache
{
    public static partial class RecoverIntervalTableMasterExt
    {
        public static string CacheParentKey(
            this RecoverIntervalTableMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "stamina",
                namespaceName,
                "RecoverIntervalTableMaster"
            );
        }

        public static string CacheKey(
            this RecoverIntervalTableMaster self,
            string recoverIntervalTableName
        ) {
            return string.Join(
                ":",
                recoverIntervalTableName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RecoverIntervalTableMaster> FetchFuture(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string recoverIntervalTableName,
            Func<IFuture<RecoverIntervalTableMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RecoverIntervalTableMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RecoverIntervalTableMaster).PutCache(
                            cache,
                            namespaceName,
                            recoverIntervalTableName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "recoverIntervalTableMaster") {
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
                    recoverIntervalTableName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RecoverIntervalTableMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RecoverIntervalTableMaster> FetchAsync(
    #else
        public static async Task<RecoverIntervalTableMaster> FetchAsync(
    #endif
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string recoverIntervalTableName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RecoverIntervalTableMaster>> fetchImpl
    #else
            Func<Task<RecoverIntervalTableMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<RecoverIntervalTableMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            recoverIntervalTableName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        recoverIntervalTableName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as RecoverIntervalTableMaster).PutCache(
                        cache,
                        namespaceName,
                        recoverIntervalTableName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "recoverIntervalTableMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<RecoverIntervalTableMaster, bool> GetCache(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string recoverIntervalTableName
        ) {
            return cache.Get<RecoverIntervalTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    recoverIntervalTableName
                )
            );
        }

        public static void PutCache(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string recoverIntervalTableName
        ) {
            var (value, find) = cache.Get<RecoverIntervalTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    recoverIntervalTableName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    recoverIntervalTableName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string recoverIntervalTableName
        ) {
            cache.Delete<RecoverIntervalTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    recoverIntervalTableName
                )
            );
        }

        public static void ListSubscribe(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<RecoverIntervalTableMaster[]> callback
        ) {
            cache.ListSubscribe<RecoverIntervalTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this RecoverIntervalTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RecoverIntervalTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}