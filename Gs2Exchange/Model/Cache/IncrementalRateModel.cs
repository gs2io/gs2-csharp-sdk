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

namespace Gs2.Gs2Exchange.Model.Cache
{
    public static partial class IncrementalRateModelExt
    {
        public static string CacheParentKey(
            this IncrementalRateModel self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "exchange",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "IncrementalRateModel"
            );
        }

        public static string CacheKey(
            this IncrementalRateModel self,
            string rateName
        ) {
            return string.Join(
                ":",
                rateName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<IncrementalRateModel> FetchFuture(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset,
            Func<IFuture<IncrementalRateModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<IncrementalRateModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as IncrementalRateModel).PutCache(
                            cache,
                            namespaceName,
                            rateName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "incrementalRateModel") {
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
                    rateName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<IncrementalRateModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<IncrementalRateModel> FetchAsync(
    #else
        public static async Task<IncrementalRateModel> FetchAsync(
    #endif
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<IncrementalRateModel>> fetchImpl
    #else
            Func<Task<IncrementalRateModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    rateName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as IncrementalRateModel).PutCache(
                    cache,
                    namespaceName,
                    rateName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "incrementalRateModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<IncrementalRateModel, bool> GetCache(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            return cache.Get<IncrementalRateModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void PutCache(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            string rateName,
            int? timeOffset
        ) {
            cache.Delete<IncrementalRateModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    rateName
                )
            );
        }

        public static void ListSubscribe(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<IncrementalRateModel[]> callback
        ) {
            cache.ListSubscribe<IncrementalRateModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this IncrementalRateModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<IncrementalRateModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}