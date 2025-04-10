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
    public static partial class SubscribeTransactionExt
    {
        public static string CacheParentKey(
            this SubscribeTransaction self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "money2",
                namespaceName,
                "SubscribeTransaction"
            );
        }

        public static string CacheKey(
            this SubscribeTransaction self,
            string contentName,
            string transactionId
        ) {
            return string.Join(
                ":",
                contentName,
                transactionId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubscribeTransaction> FetchFuture(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            string transactionId,
            Func<IFuture<SubscribeTransaction>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SubscribeTransaction> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SubscribeTransaction).PutCache(
                            cache,
                            namespaceName,
                            contentName,
                            transactionId
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "subscribeTransaction") {
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
                    contentName,
                    transactionId
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SubscribeTransaction>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SubscribeTransaction> FetchAsync(
    #else
        public static async Task<SubscribeTransaction> FetchAsync(
    #endif
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            string transactionId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SubscribeTransaction>> fetchImpl
    #else
            Func<Task<SubscribeTransaction>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    contentName,
                    transactionId
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as SubscribeTransaction).PutCache(
                    cache,
                    namespaceName,
                    contentName,
                    transactionId
                );
                if (e.errors.Length == 0 || e.errors[0].component != "subscribeTransaction") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<SubscribeTransaction, bool> GetCache(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            string transactionId
        ) {
            return cache.Get<SubscribeTransaction>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName,
                    transactionId
                )
            );
        }

        public static void PutCache(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            string transactionId
        ) {
            var (value, find) = cache.Get<SubscribeTransaction>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName,
                    transactionId
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
                    contentName,
                    transactionId
                ),
                self,
                ((self?.ExpiresAt ?? 0) == 0 ? null : self.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            string contentName,
            string transactionId
        ) {
            cache.Delete<SubscribeTransaction>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    contentName,
                    transactionId
                )
            );
        }

        public static void ListSubscribe(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            Action<SubscribeTransaction[]> callback
        ) {
            cache.ListSubscribe<SubscribeTransaction>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this SubscribeTransaction self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SubscribeTransaction>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}