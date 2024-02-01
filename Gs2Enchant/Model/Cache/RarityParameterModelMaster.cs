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

namespace Gs2.Gs2Enchant.Model.Cache
{
    public static partial class RarityParameterModelMasterExt
    {
        public static string CacheParentKey(
            this RarityParameterModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "enchant",
                namespaceName,
                "RarityParameterModelMaster"
            );
        }

        public static string CacheKey(
            this RarityParameterModelMaster self,
            string parameterName
        ) {
            return string.Join(
                ":",
                parameterName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RarityParameterModelMaster> FetchFuture(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string parameterName,
            Func<IFuture<RarityParameterModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RarityParameterModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RarityParameterModelMaster).PutCache(
                            cache,
                            namespaceName,
                            parameterName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "rarityParameterModelMaster") {
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
                    parameterName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RarityParameterModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RarityParameterModelMaster> FetchAsync(
    #else
        public static async Task<RarityParameterModelMaster> FetchAsync(
    #endif
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string parameterName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RarityParameterModelMaster>> fetchImpl
    #else
            Func<Task<RarityParameterModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<RarityParameterModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            parameterName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        parameterName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as RarityParameterModelMaster).PutCache(
                        cache,
                        namespaceName,
                        parameterName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "rarityParameterModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<RarityParameterModelMaster, bool> GetCache(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string parameterName
        ) {
            return cache.Get<RarityParameterModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    parameterName
                )
            );
        }

        public static void PutCache(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string parameterName
        ) {
            var (value, find) = cache.Get<RarityParameterModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    parameterName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    parameterName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string parameterName
        ) {
            cache.Delete<RarityParameterModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    parameterName
                )
            );
        }

        public static void ListSubscribe(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<RarityParameterModelMaster[]> callback
        ) {
            cache.ListSubscribe<RarityParameterModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this RarityParameterModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RarityParameterModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}