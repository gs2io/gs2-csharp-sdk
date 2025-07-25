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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class TakeOverTypeModelMasterExt
    {
        public static string CacheParentKey(
            this TakeOverTypeModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "account",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "TakeOverTypeModelMaster"
            );
        }

        public static string CacheKey(
            this TakeOverTypeModelMaster self,
            int? type
        ) {
            return string.Join(
                ":",
                type.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<TakeOverTypeModelMaster> FetchFuture(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            int? timeOffset,
            Func<IFuture<TakeOverTypeModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<TakeOverTypeModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as TakeOverTypeModelMaster).PutCache(
                            cache,
                            namespaceName,
                            type,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "takeOverTypeModelMaster") {
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
                    type,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<TakeOverTypeModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<TakeOverTypeModelMaster> FetchAsync(
    #else
        public static async Task<TakeOverTypeModelMaster> FetchAsync(
    #endif
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<TakeOverTypeModelMaster>> fetchImpl
    #else
            Func<Task<TakeOverTypeModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    type,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as TakeOverTypeModelMaster).PutCache(
                    cache,
                    namespaceName,
                    type,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "takeOverTypeModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<TakeOverTypeModelMaster, bool> GetCache(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            int? timeOffset
        ) {
            return cache.Get<TakeOverTypeModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void PutCache(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<TakeOverTypeModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    type
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
                    type
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            int? timeOffset
        ) {
            cache.Delete<TakeOverTypeModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void ListSubscribe(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<TakeOverTypeModelMaster[]> callback
        ) {
            cache.ListSubscribe<TakeOverTypeModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this TakeOverTypeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<TakeOverTypeModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}