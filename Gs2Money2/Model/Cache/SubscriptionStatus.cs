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

namespace Gs2.Gs2Money2.Model.Cache
{
    public static partial class SubscriptionStatusExt
    {
        public static string CacheParentKey(
            this SubscriptionStatus self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "SubscriptionStatus"
            );
        }

        public static string CacheKey(
            this SubscriptionStatus self,
            string contentName
        ) {
            return string.Join(
                ":",
                contentName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscriptionStatus> FetchFuture(
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string contentName,
            int? timeOffset,
            Func<IFuture<SubscriptionStatus>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscriptionStatus> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscriptionStatus).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            contentName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscriptionStatus") {
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
                    contentName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscriptionStatus>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscriptionStatus> FetchAsync(
    #else
        public static async Task<SubscriptionStatus> FetchAsync(
    #endif
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string contentName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscriptionStatus>> fetchImpl
    #else
            Func<Task<SubscriptionStatus>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    contentName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SubscriptionStatus).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    contentName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "subscriptionStatus") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<SubscriptionStatus, bool> GetCache(
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string contentName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<SubscriptionStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void PutCache(
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string contentName,
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
                    contentName
                ),
                self,
                ((self?.ExpiresAt ?? 0) == 0 ? null : self.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string contentName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<SubscriptionStatus>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    contentName
                )
            );
        }

        public static void ListSubscribe(
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<SubscriptionStatus[]> callback
        ) {
            cache.ListSubscribe<SubscriptionStatus>(
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
            this SubscriptionStatus self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscriptionStatus>(
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