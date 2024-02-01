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

namespace Gs2.Gs2Deploy.Model.Cache
{
    public static partial class ResourceExt
    {
        public static string CacheParentKey(
            this Resource self,
            string stackName
        ) {
            return string.Join(
                ":",
                "deploy",
                stackName,
                "Resource"
            );
        }

        public static string CacheKey(
            this Resource self,
            string resourceName
        ) {
            return string.Join(
                ":",
                resourceName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Resource> FetchFuture(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            string resourceName,
            Func<IFuture<Resource>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Resource> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Resource).PutCache(
                            cache,
                            stackName,
                            resourceName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "resource") {
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
                    stackName,
                    resourceName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Resource>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Resource> FetchAsync(
    #else
        public static async Task<Resource> FetchAsync(
    #endif
            this Resource self,
            CacheDatabase cache,
            string stackName,
            string resourceName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Resource>> fetchImpl
    #else
            Func<Task<Resource>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Resource>(
                       self.CacheParentKey(
                            stackName
                       ),
                       self.CacheKey(
                            resourceName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        stackName,
                        resourceName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Resource).PutCache(
                        cache,
                        stackName,
                        resourceName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "resource") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Resource, bool> GetCache(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            string resourceName
        ) {
            return cache.Get<Resource>(
                self.CacheParentKey(
                    stackName
                ),
                self.CacheKey(
                    resourceName
                )
            );
        }

        public static void PutCache(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            string resourceName
        ) {
            cache.Put(
                self.CacheParentKey(
                    stackName
                ),
                self.CacheKey(
                    resourceName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            string resourceName
        ) {
            cache.Delete<Resource>(
                self.CacheParentKey(
                    stackName
                ),
                self.CacheKey(
                    resourceName
                )
            );
        }

        public static void ListSubscribe(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            Action<Resource[]> callback
        ) {
            cache.ListSubscribe<Resource>(
                self.CacheParentKey(
                    stackName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Resource self,
            CacheDatabase cache,
            string stackName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Resource>(
                self.CacheParentKey(
                    stackName
                ),
                callbackId
            );
        }
    }
}