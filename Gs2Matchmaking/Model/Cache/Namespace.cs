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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static partial class NamespaceExt
    {
        public static string CacheParentKey(
            this Namespace self,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "matchmaking",
                timeOffset?.ToString() ?? "0",
                "Namespace"
            );
        }

        public static string CacheKey(
            this Namespace self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                namespaceName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Namespace> FetchFuture(
            this Namespace self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Func<IFuture<Namespace>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Namespace> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Namespace).PutCache(
                            cache,
                            namespaceName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "namespace") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Namespace>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Namespace> FetchAsync(
    #else
        public static async Task<Namespace> FetchAsync(
    #endif
            this Namespace self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Namespace>> fetchImpl
    #else
            Func<Task<Namespace>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Namespace).PutCache(
                    cache,
                    namespaceName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "namespace") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Namespace, bool> GetCache(
            this Namespace self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            return cache.Get<Namespace>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    namespaceName
                )
            );
        }

        public static void PutCache(
            this Namespace self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<Namespace>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    namespaceName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    namespaceName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Namespace self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset
        ) {
            cache.Delete<Namespace>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    namespaceName
                )
            );
        }

        public static void ListSubscribe(
            this Namespace self,
            CacheDatabase cache,
            int? timeOffset,
            Action<Namespace[]> callback
        ) {
            cache.ListSubscribe<Namespace>(
                self.CacheParentKey(
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Namespace self,
            CacheDatabase cache,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Namespace>(
                self.CacheParentKey(
                    timeOffset
                ),
                callbackId
            );
        }
    }
}