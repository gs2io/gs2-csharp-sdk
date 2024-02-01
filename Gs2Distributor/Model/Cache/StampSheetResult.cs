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

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static partial class StampSheetResultExt
    {
        public static string CacheParentKey(
            this StampSheetResult self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "distributor",
                namespaceName,
                userId,
                "StampSheetResult"
            );
        }

        public static string CacheKey(
            this StampSheetResult self,
            string transactionId
        ) {
            return string.Join(
                ":",
                transactionId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<StampSheetResult> FetchFuture(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
            Func<IFuture<StampSheetResult>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<StampSheetResult> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as StampSheetResult).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            transactionId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "stampSheetResult") {
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
                    transactionId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<StampSheetResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<StampSheetResult> FetchAsync(
    #else
        public static async Task<StampSheetResult> FetchAsync(
    #endif
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<StampSheetResult>> fetchImpl
    #else
            Func<Task<StampSheetResult>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<StampSheetResult>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            transactionId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        transactionId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as StampSheetResult).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        transactionId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "stampSheetResult") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<StampSheetResult, bool> GetCache(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<StampSheetResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void PutCache(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<StampSheetResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId
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
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    transactionId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string transactionId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<StampSheetResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    transactionId
                )
            );
        }

        public static void ListSubscribe(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<StampSheetResult[]> callback
        ) {
            cache.ListSubscribe<StampSheetResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this StampSheetResult self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<StampSheetResult>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}