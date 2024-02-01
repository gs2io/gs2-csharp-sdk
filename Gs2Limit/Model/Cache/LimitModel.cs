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

namespace Gs2.Gs2Limit.Model.Cache
{
    public static partial class LimitModelExt
    {
        public static string CacheParentKey(
            this LimitModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "limit",
                namespaceName,
                "LimitModel"
            );
        }

        public static string CacheKey(
            this LimitModel self,
            string limitName
        ) {
            return string.Join(
                ":",
                limitName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<LimitModel> FetchFuture(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
            Func<IFuture<LimitModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<LimitModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as LimitModel).PutCache(
                            cache,
                            namespaceName,
                            limitName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "limitModel") {
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
                    limitName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<LimitModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<LimitModel> FetchAsync(
    #else
        public static async Task<LimitModel> FetchAsync(
    #endif
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            string limitName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<LimitModel>> fetchImpl
    #else
            Func<Task<LimitModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<LimitModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            limitName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        limitName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as LimitModel).PutCache(
                        cache,
                        namespaceName,
                        limitName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "limitModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<LimitModel, bool> GetCache(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            string limitName
        ) {
            return cache.Get<LimitModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    limitName
                )
            );
        }

        public static void PutCache(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            string limitName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    limitName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            string limitName
        ) {
            cache.Delete<LimitModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    limitName
                )
            );
        }

        public static void ListSubscribe(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<LimitModel[]> callback
        ) {
            cache.ListSubscribe<LimitModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this LimitModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<LimitModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}