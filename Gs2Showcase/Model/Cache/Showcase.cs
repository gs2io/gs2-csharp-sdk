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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class ShowcaseExt
    {
        public static string CacheParentKey(
            this Showcase self,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                userId,
                timeOffset?.ToString() ?? "0",
                "Showcase"
            );
        }

        public static string CacheKey(
            this Showcase self,
            string showcaseName
        ) {
            return string.Join(
                ":",
                showcaseName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Showcase> FetchFuture(
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            int? timeOffset,
            Func<IFuture<Showcase>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Showcase> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Showcase).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            showcaseName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "showcase") {
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
                    showcaseName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Showcase>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Showcase> FetchAsync(
    #else
        public static async Task<Showcase> FetchAsync(
    #endif
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Showcase>> fetchImpl
    #else
            Func<Task<Showcase>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    userId,
                    showcaseName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Showcase).PutCache(
                    cache,
                    namespaceName,
                    userId,
                    showcaseName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "showcase") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Showcase, bool> GetCache(
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Showcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void PutCache(
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
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
                    showcaseName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            int? timeOffset
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Showcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    timeOffset
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void ListSubscribe(
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            Action<Showcase[]> callback
        ) {
            cache.ListSubscribe<Showcase>(
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
            this Showcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Showcase>(
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