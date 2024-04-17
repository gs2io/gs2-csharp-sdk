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

namespace Gs2.Gs2Buff.Model.Cache
{
    public static partial class BuffEntryModelMasterExt
    {
        public static string CacheParentKey(
            this BuffEntryModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "buff",
                namespaceName,
                "BuffEntryModelMaster"
            );
        }

        public static string CacheKey(
            this BuffEntryModelMaster self,
            string buffEntryName
        ) {
            return string.Join(
                ":",
                buffEntryName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<BuffEntryModelMaster> FetchFuture(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string buffEntryName,
            Func<IFuture<BuffEntryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<BuffEntryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as BuffEntryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            buffEntryName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "buffEntryModelMaster") {
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
                    buffEntryName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<BuffEntryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<BuffEntryModelMaster> FetchAsync(
    #else
        public static async Task<BuffEntryModelMaster> FetchAsync(
    #endif
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string buffEntryName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<BuffEntryModelMaster>> fetchImpl
    #else
            Func<Task<BuffEntryModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<BuffEntryModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            buffEntryName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        buffEntryName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as BuffEntryModelMaster).PutCache(
                        cache,
                        namespaceName,
                        buffEntryName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "buffEntryModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<BuffEntryModelMaster, bool> GetCache(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string buffEntryName
        ) {
            return cache.Get<BuffEntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    buffEntryName
                )
            );
        }

        public static void PutCache(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string buffEntryName
        ) {
            var (value, find) = cache.Get<BuffEntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    buffEntryName
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
                    buffEntryName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string buffEntryName
        ) {
            cache.Delete<BuffEntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    buffEntryName
                )
            );
        }

        public static void ListSubscribe(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<BuffEntryModelMaster[]> callback
        ) {
            cache.ListSubscribe<BuffEntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this BuffEntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<BuffEntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}