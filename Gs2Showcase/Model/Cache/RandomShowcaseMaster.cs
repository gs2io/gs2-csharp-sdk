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

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class RandomShowcaseMasterExt
    {
        public static string CacheParentKey(
            this RandomShowcaseMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "RandomShowcaseMaster"
            );
        }

        public static string CacheKey(
            this RandomShowcaseMaster self,
            string showcaseName
        ) {
            return string.Join(
                ":",
                showcaseName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<RandomShowcaseMaster> FetchFuture(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            int? timeOffset,
            Func<IFuture<RandomShowcaseMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<RandomShowcaseMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as RandomShowcaseMaster).PutCache(
                            cache,
                            namespaceName,
                            showcaseName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "randomShowcaseMaster") {
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
                    showcaseName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<RandomShowcaseMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<RandomShowcaseMaster> FetchAsync(
    #else
        public static async Task<RandomShowcaseMaster> FetchAsync(
    #endif
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<RandomShowcaseMaster>> fetchImpl
    #else
            Func<Task<RandomShowcaseMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    showcaseName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as RandomShowcaseMaster).PutCache(
                    cache,
                    namespaceName,
                    showcaseName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "randomShowcaseMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<RandomShowcaseMaster, bool> GetCache(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            int? timeOffset
        ) {
            return cache.Get<RandomShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void PutCache(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<RandomShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    showcaseName
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
                    showcaseName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            string showcaseName,
            int? timeOffset
        ) {
            cache.Delete<RandomShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    showcaseName
                )
            );
        }

        public static void ListSubscribe(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<RandomShowcaseMaster[]> callback
        ) {
            cache.ListSubscribe<RandomShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this RandomShowcaseMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<RandomShowcaseMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}