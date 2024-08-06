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

namespace Gs2.Gs2Log.Model.Cache
{
    public static partial class AccessLogWithTelemetryExt
    {
        public static string CacheParentKey(
            this AccessLogWithTelemetry self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "log",
                namespaceName,
                "AccessLogWithTelemetry"
            );
        }

        public static string CacheKey(
            this AccessLogWithTelemetry self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AccessLogWithTelemetry> FetchFuture(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName,
            Func<IFuture<AccessLogWithTelemetry>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<AccessLogWithTelemetry> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as AccessLogWithTelemetry).PutCache(
                            cache,
                            namespaceName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "accessLog") {
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
                    namespaceName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<AccessLogWithTelemetry>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<AccessLogWithTelemetry> FetchAsync(
    #else
        public static async Task<AccessLogWithTelemetry> FetchAsync(
    #endif
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<AccessLogWithTelemetry>> fetchImpl
    #else
            Func<Task<AccessLogWithTelemetry>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<AccessLogWithTelemetry>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as AccessLogWithTelemetry).PutCache(
                        cache,
                        namespaceName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "accessLog") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<AccessLogWithTelemetry, bool> GetCache(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName
        ) {
            return cache.Get<AccessLogWithTelemetry>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName
        ) {
            cache.Delete<AccessLogWithTelemetry>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName,
            Action<AccessLogWithTelemetry[]> callback
        ) {
            cache.ListSubscribe<AccessLogWithTelemetry>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this AccessLogWithTelemetry self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<AccessLogWithTelemetry>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}