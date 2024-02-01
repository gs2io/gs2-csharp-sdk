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

namespace Gs2.Gs2Enchant.Model.Cache
{
    public static partial class BalanceParameterStatusExt
    {
        public static string CacheParentKey(
            this BalanceParameterStatus self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "enchant",
                namespaceName,
                userId,
                "BalanceParameterStatus"
            );
        }

        public static string CacheKey(
            this BalanceParameterStatus self,
            string parameterName,
            string propertyId
        ) {
            return string.Join(
                ":",
                parameterName,
                propertyId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BalanceParameterStatus> FetchFuture(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId,
            Func<IFuture<BalanceParameterStatus>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BalanceParameterStatus> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BalanceParameterStatus).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            parameterName,
                            propertyId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "balanceParameterStatus") {
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
                    parameterName,
                    propertyId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BalanceParameterStatus>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BalanceParameterStatus> FetchAsync(
    #else
        public static async Task<BalanceParameterStatus> FetchAsync(
    #endif
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BalanceParameterStatus>> fetchImpl
    #else
            Func<Task<BalanceParameterStatus>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BalanceParameterStatus>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            parameterName,
                            propertyId
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        parameterName,
                        propertyId
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BalanceParameterStatus).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        parameterName,
                        propertyId
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "balanceParameterStatus") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BalanceParameterStatus, bool> GetCache(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<BalanceParameterStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    parameterName,
                    propertyId
                )
            );
        }

        public static void PutCache(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<BalanceParameterStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    parameterName,
                    propertyId
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
                    parameterName,
                    propertyId
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<BalanceParameterStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    parameterName,
                    propertyId
                )
            );
        }

        public static void ListSubscribe(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<BalanceParameterStatus[]> callback
        ) {
            cache.ListSubscribe<BalanceParameterStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BalanceParameterStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BalanceParameterStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}