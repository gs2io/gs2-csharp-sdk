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

namespace Gs2.Gs2Freeze.Model.Cache
{
    public static partial class StageExt
    {
        public static string CacheParentKey(
            this Stage self,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "freeze",
                timeOffset?.ToString() ?? "0",
                "Stage"
            );
        }

        public static string CacheKey(
            this Stage self,
            string stageName
        ) {
            return string.Join(
                ":",
                stageName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Stage> FetchFuture(
            this Stage self,
            CacheDatabase cache,
            string stageName,
            int? timeOffset,
            Func<IFuture<Stage>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Stage> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Stage).PutCache(
                            cache,
                            stageName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "stage") {
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
                    stageName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Stage>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Stage> FetchAsync(
    #else
        public static async Task<Stage> FetchAsync(
    #endif
            this Stage self,
            CacheDatabase cache,
            string stageName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Stage>> fetchImpl
    #else
            Func<Task<Stage>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    stageName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Stage).PutCache(
                    cache,
                    stageName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "stage") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Stage, bool> GetCache(
            this Stage self,
            CacheDatabase cache,
            string stageName,
            int? timeOffset
        ) {
            return cache.Get<Stage>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    stageName
                )
            );
        }

        public static void PutCache(
            this Stage self,
            CacheDatabase cache,
            string stageName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<Stage>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    stageName
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
                    timeOffset
                ),
                self.CacheKey(
                    stageName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Stage self,
            CacheDatabase cache,
            string stageName,
            int? timeOffset
        ) {
            cache.Delete<Stage>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                    stageName
                )
            );
        }

        public static void ListSubscribe(
            this Stage self,
            CacheDatabase cache,
            int? timeOffset,
            Action<Stage[]> callback
        ) {
            cache.ListSubscribe<Stage>(
                self.CacheParentKey(
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Stage self,
            CacheDatabase cache,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Stage>(
                self.CacheParentKey(
                    timeOffset
                ),
                callbackId
            );
        }
    }
}