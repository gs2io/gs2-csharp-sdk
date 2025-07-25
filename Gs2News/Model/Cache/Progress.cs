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

namespace Gs2.Gs2News.Model.Cache
{
    public static partial class ProgressExt
    {
        public static string CacheParentKey(
            this Progress self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "news",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "Progress"
            );
        }

        public static string CacheKey(
            this Progress self,
            string uploadToken
        ) {
            return string.Join(
                ":",
                uploadToken
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Progress> FetchFuture(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            int? timeOffset,
            Func<IFuture<Progress>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Progress> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Progress).PutCache(
                            cache,
                            namespaceName,
                            uploadToken,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "progress") {
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
                    uploadToken,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Progress>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Progress> FetchAsync(
    #else
        public static async Task<Progress> FetchAsync(
    #endif
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Progress>> fetchImpl
    #else
            Func<Task<Progress>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    uploadToken,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Progress).PutCache(
                    cache,
                    namespaceName,
                    uploadToken,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "progress") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Progress, bool> GetCache(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            int? timeOffset
        ) {
            return cache.Get<Progress>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    uploadToken
                )
            );
        }

        public static void PutCache(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<Progress>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    uploadToken
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
                    timeOffset
                ),
                self.CacheKey(
                    uploadToken
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            int? timeOffset
        ) {
            cache.Delete<Progress>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    uploadToken
                )
            );
        }

        public static void ListSubscribe(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<Progress[]> callback
        ) {
            cache.ListSubscribe<Progress>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Progress self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Progress>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}