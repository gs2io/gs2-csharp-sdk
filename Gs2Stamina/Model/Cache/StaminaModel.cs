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

namespace Gs2.Gs2Stamina.Model.Cache
{
    public static partial class StaminaModelExt
    {
        public static string CacheParentKey(
            this StaminaModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "stamina",
                namespaceName,
                "StaminaModel"
            );
        }

        public static string CacheKey(
            this StaminaModel self,
            string staminaName
        ) {
            return string.Join(
                ":",
                staminaName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<StaminaModel> FetchFuture(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            string staminaName,
            Func<IFuture<StaminaModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<StaminaModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as StaminaModel).PutCache(
                            cache,
                            namespaceName,
                            staminaName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "staminaModel") {
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
                    staminaName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<StaminaModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<StaminaModel> FetchAsync(
    #else
        public static async Task<StaminaModel> FetchAsync(
    #endif
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            string staminaName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<StaminaModel>> fetchImpl
    #else
            Func<Task<StaminaModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<StaminaModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            staminaName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        staminaName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as StaminaModel).PutCache(
                        cache,
                        namespaceName,
                        staminaName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "staminaModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<StaminaModel, bool> GetCache(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            string staminaName
        ) {
            return cache.Get<StaminaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    staminaName
                )
            );
        }

        public static void PutCache(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            string staminaName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    staminaName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            string staminaName
        ) {
            cache.Delete<StaminaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    staminaName
                )
            );
        }

        public static void ListSubscribe(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<StaminaModel[]> callback
        ) {
            cache.ListSubscribe<StaminaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this StaminaModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<StaminaModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}