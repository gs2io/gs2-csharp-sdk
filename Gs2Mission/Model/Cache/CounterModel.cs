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

namespace Gs2.Gs2Mission.Model.Cache
{
    public static partial class CounterModelExt
    {
        public static string CacheParentKey(
            this CounterModel self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "mission",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "CounterModel"
            );
        }

        public static string CacheKey(
            this CounterModel self,
            string counterName
        ) {
            return string.Join(
                ":",
                counterName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CounterModel> FetchFuture(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            string counterName,
            int? timeOffset,
            Func<IFuture<CounterModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CounterModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CounterModel).PutCache(
                            cache,
                            namespaceName,
                            counterName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "counterModel") {
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
                    counterName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CounterModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CounterModel> FetchAsync(
    #else
        public static async Task<CounterModel> FetchAsync(
    #endif
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            string counterName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CounterModel>> fetchImpl
    #else
            Func<Task<CounterModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    counterName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as CounterModel).PutCache(
                    cache,
                    namespaceName,
                    counterName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "counterModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<CounterModel, bool> GetCache(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            string counterName,
            int? timeOffset
        ) {
            return cache.Get<CounterModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    counterName
                )
            );
        }

        public static void PutCache(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            string counterName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    counterName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            string counterName,
            int? timeOffset
        ) {
            cache.Delete<CounterModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    counterName
                )
            );
        }

        public static void ListSubscribe(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<CounterModel[]> callback
        ) {
            cache.ListSubscribe<CounterModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this CounterModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CounterModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}