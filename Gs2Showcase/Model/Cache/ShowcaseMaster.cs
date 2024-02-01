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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class ShowcaseMasterExt
    {
        public static string CacheParentKey(
            this ShowcaseMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                "ShowcaseMaster"
            );
        }

        public static string CacheKey(
            this ShowcaseMaster self,
            string showcaseName
        ) {
            return string.Join(
                ":",
                showcaseName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ShowcaseMaster> FetchFuture(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            Func<IFuture<ShowcaseMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ShowcaseMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ShowcaseMaster).PutCache(
                            cache,
                            namespaceName,
                            showcaseName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "showcaseMaster") {
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
                    showcaseName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ShowcaseMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ShowcaseMaster> FetchAsync(
    #else
        public static async Task<ShowcaseMaster> FetchAsync(
    #endif
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ShowcaseMaster>> fetchImpl
    #else
            Func<Task<ShowcaseMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ShowcaseMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            showcaseName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        showcaseName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ShowcaseMaster).PutCache(
                        cache,
                        namespaceName,
                        showcaseName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "showcaseMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ShowcaseMaster, bool> GetCache(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName
        ) {
            return cache.Get<ShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void PutCache(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName
        ) {
            var (value, find) = cache.Get<ShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    showcaseName
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
                    showcaseName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName
        ) {
            cache.Delete<ShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void ListSubscribe(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<ShowcaseMaster[]> callback
        ) {
            cache.ListSubscribe<ShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}