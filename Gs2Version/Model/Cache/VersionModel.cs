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

namespace Gs2.Gs2Version.Model.Cache
{
    public static partial class VersionModelExt
    {
        public static string CacheParentKey(
            this VersionModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "version",
                namespaceName,
                "VersionModel"
            );
        }

        public static string CacheKey(
            this VersionModel self,
            string versionName
        ) {
            return string.Join(
                ":",
                versionName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<VersionModel> FetchFuture(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
            Func<IFuture<VersionModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<VersionModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as VersionModel).PutCache(
                            cache,
                            namespaceName,
                            versionName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "versionModel") {
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
                    versionName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<VersionModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<VersionModel> FetchAsync(
    #else
        public static async Task<VersionModel> FetchAsync(
    #endif
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            string versionName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<VersionModel>> fetchImpl
    #else
            Func<Task<VersionModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<VersionModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            versionName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        versionName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as VersionModel).PutCache(
                        cache,
                        namespaceName,
                        versionName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "versionModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<VersionModel, bool> GetCache(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            string versionName
        ) {
            return cache.Get<VersionModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void PutCache(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            string versionName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    versionName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            string versionName
        ) {
            cache.Delete<VersionModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    versionName
                )
            );
        }

        public static void ListSubscribe(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<VersionModel[]> callback
        ) {
            cache.ListSubscribe<VersionModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this VersionModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<VersionModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}