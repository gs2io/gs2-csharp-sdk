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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class TakeOverTypeModelExt
    {
        public static string CacheParentKey(
            this TakeOverTypeModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "account",
                namespaceName,
                "TakeOverTypeModel"
            );
        }

        public static string CacheKey(
            this TakeOverTypeModel self,
            int? type
        ) {
            return string.Join(
                ":",
                type.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<TakeOverTypeModel> FetchFuture(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
            Func<IFuture<TakeOverTypeModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<TakeOverTypeModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as TakeOverTypeModel).PutCache(
                            cache,
                            namespaceName,
                            type
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "takeOverTypeModel") {
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
                    type
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<TakeOverTypeModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<TakeOverTypeModel> FetchAsync(
    #else
        public static async Task<TakeOverTypeModel> FetchAsync(
    #endif
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? type,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<TakeOverTypeModel>> fetchImpl
    #else
            Func<Task<TakeOverTypeModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<TakeOverTypeModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            type
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        type
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as TakeOverTypeModel).PutCache(
                        cache,
                        namespaceName,
                        type
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "takeOverTypeModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<TakeOverTypeModel, bool> GetCache(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? type
        ) {
            return cache.Get<TakeOverTypeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void PutCache(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? type
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    type
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? type
        ) {
            cache.Delete<TakeOverTypeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    type
                )
            );
        }

        public static void ListSubscribe(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<TakeOverTypeModel[]> callback
        ) {
            cache.ListSubscribe<TakeOverTypeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this TakeOverTypeModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<TakeOverTypeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}