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

namespace Gs2.Gs2Experience.Model.Cache
{
    public static partial class CurrentExperienceMasterExt
    {
        public static string CacheParentKey(
            this CurrentExperienceMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "experience",
                namespaceName,
                "CurrentExperienceMaster"
            );
        }

        public static string CacheKey(
            this CurrentExperienceMaster self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CurrentExperienceMaster> FetchFuture(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName,
            Func<IFuture<CurrentExperienceMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CurrentExperienceMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CurrentExperienceMaster).PutCache(
                            cache,
                            namespaceName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "currentExperienceMaster") {
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
                    namespaceName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CurrentExperienceMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CurrentExperienceMaster> FetchAsync(
    #else
        public static async Task<CurrentExperienceMaster> FetchAsync(
    #endif
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CurrentExperienceMaster>> fetchImpl
    #else
            Func<Task<CurrentExperienceMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<CurrentExperienceMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as CurrentExperienceMaster).PutCache(
                        cache,
                        namespaceName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "currentExperienceMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<CurrentExperienceMaster, bool> GetCache(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName
        ) {
            return cache.Get<CurrentExperienceMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName
        ) {
            cache.Delete<CurrentExperienceMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<CurrentExperienceMaster[]> callback
        ) {
            cache.ListSubscribe<CurrentExperienceMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this CurrentExperienceMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CurrentExperienceMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}