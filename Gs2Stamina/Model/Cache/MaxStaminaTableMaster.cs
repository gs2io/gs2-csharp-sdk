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
    public static partial class MaxStaminaTableMasterExt
    {
        public static string CacheParentKey(
            this MaxStaminaTableMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "stamina",
                namespaceName,
                "MaxStaminaTableMaster"
            );
        }

        public static string CacheKey(
            this MaxStaminaTableMaster self,
            string maxStaminaTableName
        ) {
            return string.Join(
                ":",
                maxStaminaTableName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MaxStaminaTableMaster> FetchFuture(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string maxStaminaTableName,
            Func<IFuture<MaxStaminaTableMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MaxStaminaTableMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MaxStaminaTableMaster).PutCache(
                            cache,
                            namespaceName,
                            maxStaminaTableName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "maxStaminaTableMaster") {
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
                    maxStaminaTableName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MaxStaminaTableMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MaxStaminaTableMaster> FetchAsync(
    #else
        public static async Task<MaxStaminaTableMaster> FetchAsync(
    #endif
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string maxStaminaTableName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MaxStaminaTableMaster>> fetchImpl
    #else
            Func<Task<MaxStaminaTableMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<MaxStaminaTableMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            maxStaminaTableName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        maxStaminaTableName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as MaxStaminaTableMaster).PutCache(
                        cache,
                        namespaceName,
                        maxStaminaTableName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "maxStaminaTableMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<MaxStaminaTableMaster, bool> GetCache(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string maxStaminaTableName
        ) {
            return cache.Get<MaxStaminaTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    maxStaminaTableName
                )
            );
        }

        public static void PutCache(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string maxStaminaTableName
        ) {
            var (value, find) = cache.Get<MaxStaminaTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    maxStaminaTableName
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
                    maxStaminaTableName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            string maxStaminaTableName
        ) {
            cache.Delete<MaxStaminaTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    maxStaminaTableName
                )
            );
        }

        public static void ListSubscribe(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<MaxStaminaTableMaster[]> callback
        ) {
            cache.ListSubscribe<MaxStaminaTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this MaxStaminaTableMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MaxStaminaTableMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}