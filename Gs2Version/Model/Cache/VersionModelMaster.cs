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

namespace Gs2.Gs2Version.Model.Cache
{
    public static partial class VersionModelMasterExt
    {
        public static string CacheParentKey(
            this VersionModelMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "version",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "VersionModelMaster"
            );
        }

        public static string CacheKey(
            this VersionModelMaster self,
            string versionName
        ) {
            return string.Join(
                ":",
                versionName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<VersionModelMaster> FetchFuture(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            int? timeOffset,
            Func<IFuture<VersionModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<VersionModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as VersionModelMaster).PutCache(
                            cache,
                            namespaceName,
                            versionName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "versionModelMaster") {
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
                    versionName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<VersionModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<VersionModelMaster> FetchAsync(
    #else
        public static async Task<VersionModelMaster> FetchAsync(
    #endif
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<VersionModelMaster>> fetchImpl
    #else
            Func<Task<VersionModelMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    versionName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as VersionModelMaster).PutCache(
                    cache,
                    namespaceName,
                    versionName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "versionModelMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<VersionModelMaster, bool> GetCache(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            int? timeOffset
        ) {
            return cache.Get<VersionModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void PutCache(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<VersionModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    versionName
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
                    versionName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            int? timeOffset
        ) {
            cache.Delete<VersionModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void ListSubscribe(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<VersionModelMaster[]> callback
        ) {
            cache.ListSubscribe<VersionModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this VersionModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<VersionModelMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}