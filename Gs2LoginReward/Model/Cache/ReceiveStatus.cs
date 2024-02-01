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

namespace Gs2.Gs2LoginReward.Model.Cache
{
    public static partial class ReceiveStatusExt
    {
        public static string CacheParentKey(
            this ReceiveStatus self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "loginReward",
                namespaceName,
                userId,
                "ReceiveStatus"
            );
        }

        public static string CacheKey(
            this ReceiveStatus self,
            string bonusModelName
        ) {
            return string.Join(
                ":",
                bonusModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ReceiveStatus> FetchFuture(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName,
            Func<IFuture<ReceiveStatus>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ReceiveStatus> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ReceiveStatus).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            bonusModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "receiveStatus") {
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
                    bonusModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ReceiveStatus>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ReceiveStatus> FetchAsync(
    #else
        public static async Task<ReceiveStatus> FetchAsync(
    #endif
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ReceiveStatus>> fetchImpl
    #else
            Func<Task<ReceiveStatus>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ReceiveStatus>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            bonusModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        bonusModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ReceiveStatus).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        bonusModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "receiveStatus") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ReceiveStatus, bool> GetCache(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<ReceiveStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    bonusModelName
                )
            );
        }

        public static void PutCache(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<ReceiveStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    bonusModelName
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
                    bonusModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string bonusModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<ReceiveStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    bonusModelName
                )
            );
        }

        public static void ListSubscribe(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<ReceiveStatus[]> callback
        ) {
            cache.ListSubscribe<ReceiveStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ReceiveStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ReceiveStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}