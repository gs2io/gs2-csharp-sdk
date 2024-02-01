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
    public static partial class RandomShowcaseExt
    {
        public static string CacheParentKey(
            this RandomShowcase self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                userId,
                "RandomShowcase"
            );
        }

        public static string CacheKey(
            this RandomShowcase self,
            string showcaseName
        ) {
            return string.Join(
                ":",
                showcaseName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RandomShowcase> FetchFuture(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            Func<IFuture<RandomShowcase>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RandomShowcase> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RandomShowcase).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            showcaseName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "randomShowcase") {
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
                    userId,
                    showcaseName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RandomShowcase>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RandomShowcase> FetchAsync(
    #else
        public static async Task<RandomShowcase> FetchAsync(
    #endif
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RandomShowcase>> fetchImpl
    #else
            Func<Task<RandomShowcase>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<RandomShowcase>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
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
                        userId,
                        showcaseName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as RandomShowcase).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        showcaseName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "randomShowcase") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<RandomShowcase, bool> GetCache(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<RandomShowcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void PutCache(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    showcaseName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<RandomShowcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void ListSubscribe(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<RandomShowcase[]> callback
        ) {
            cache.ListSubscribe<RandomShowcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this RandomShowcase self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RandomShowcase>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}