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

namespace Gs2.Gs2Distributor.Model.Cache
{
    public static partial class DistributorModelExt
    {
        public static string CacheParentKey(
            this DistributorModel self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "distributor",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "DistributorModel"
            );
        }

        public static string CacheKey(
            this DistributorModel self,
            string distributorName
        ) {
            return string.Join(
                ":",
                distributorName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DistributorModel> FetchFuture(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            int? timeOffset,
            Func<IFuture<DistributorModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<DistributorModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as DistributorModel).PutCache(
                            cache,
                            namespaceName,
                            distributorName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "distributorModel") {
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
                    distributorName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<DistributorModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DistributorModel> FetchAsync(
    #else
        public static async Task<DistributorModel> FetchAsync(
    #endif
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DistributorModel>> fetchImpl
    #else
            Func<Task<DistributorModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    distributorName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as DistributorModel).PutCache(
                    cache,
                    namespaceName,
                    distributorName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "distributorModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<DistributorModel, bool> GetCache(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            int? timeOffset
        ) {
            return cache.Get<DistributorModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    distributorName
                )
            );
        }

        public static void PutCache(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    distributorName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            string distributorName,
            int? timeOffset
        ) {
            cache.Delete<DistributorModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    distributorName
                )
            );
        }

        public static void ListSubscribe(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<DistributorModel[]> callback
        ) {
            cache.ListSubscribe<DistributorModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this DistributorModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<DistributorModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}