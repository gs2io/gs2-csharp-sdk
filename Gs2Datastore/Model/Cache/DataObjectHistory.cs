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

namespace Gs2.Gs2Datastore.Model.Cache
{
    public static partial class DataObjectHistoryExt
    {
        public static string CacheParentKey(
            this DataObjectHistory self,
            string namespaceName,
            string userId,
            string dataObjectName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "datastore",
                namespaceName,
                userId,
                dataObjectName,
                timeOffset?.ToString() ?? "0",
                "DataObjectHistory"
            );
        }

        public static string CacheKey(
            this DataObjectHistory self,
            string generation
        ) {
            return string.Join(
                ":",
                generation
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DataObjectHistory> FetchFuture(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            int? timeOffset,
            Func<IFuture<DataObjectHistory>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DataObjectHistory> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DataObjectHistory).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            dataObjectName,
                            generation,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "dataObjectHistory") {
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
                    dataObjectName,
                    generation,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DataObjectHistory>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DataObjectHistory> FetchAsync(
    #else
        public static async Task<DataObjectHistory> FetchAsync(
    #endif
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DataObjectHistory>> fetchImpl
    #else
            Func<Task<DataObjectHistory>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    dataObjectName,
                    generation,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as DataObjectHistory).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    dataObjectName,
                    generation,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "dataObjectHistory") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<DataObjectHistory, bool> GetCache(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<DataObjectHistory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                self.CacheKey(
                    generation
                )
            );
        }

        public static void PutCache(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<DataObjectHistory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                self.CacheKey(
                    generation
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                self.CacheKey(
                    generation
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            string generation,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<DataObjectHistory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                self.CacheKey(
                    generation
                )
            );
        }

        public static void ListSubscribe(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            int? timeOffset,
            Action<DataObjectHistory[]> callback
        ) {
            cache.ListSubscribe<DataObjectHistory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this DataObjectHistory self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DataObjectHistory>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    dataObjectName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}