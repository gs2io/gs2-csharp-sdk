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

namespace Gs2.Gs2Datastore.Model.Cache
{
    public static partial class DataObjectExt
    {
        public static string CacheParentKey(
            this DataObject self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "datastore",
                namespaceName,
                userId,
                "DataObject"
            );
        }

        public static string CacheKey(
            this DataObject self,
            string dataObjectName
        ) {
            return string.Join(
                ":",
                dataObjectName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DataObject> FetchFuture(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
            Func<IFuture<DataObject>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DataObject> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DataObject).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            dataObjectName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "dataObject") {
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
                    dataObjectName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DataObject>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DataObject> FetchAsync(
    #else
        public static async Task<DataObject> FetchAsync(
    #endif
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DataObject>> fetchImpl
    #else
            Func<Task<DataObject>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<DataObject>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            dataObjectName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        dataObjectName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as DataObject).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        dataObjectName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "dataObject") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<DataObject, bool> GetCache(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<DataObject>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    dataObjectName
                )
            );
        }

        public static void PutCache(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<DataObject>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    dataObjectName
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
                    dataObjectName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string dataObjectName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<DataObject>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    dataObjectName
                )
            );
        }

        public static void ListSubscribe(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<DataObject[]> callback
        ) {
            cache.ListSubscribe<DataObject>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this DataObject self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DataObject>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}