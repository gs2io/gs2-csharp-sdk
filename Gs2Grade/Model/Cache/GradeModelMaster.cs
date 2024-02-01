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

namespace Gs2.Gs2Grade.Model.Cache
{
    public static partial class GradeModelMasterExt
    {
        public static string CacheParentKey(
            this GradeModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "grade",
                namespaceName,
                "GradeModelMaster"
            );
        }

        public static string CacheKey(
            this GradeModelMaster self,
            string gradeName
        ) {
            return string.Join(
                ":",
                gradeName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GradeModelMaster> FetchFuture(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            Func<IFuture<GradeModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GradeModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GradeModelMaster).PutCache(
                            cache,
                            namespaceName,
                            gradeName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "gradeModelMaster") {
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
                    gradeName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GradeModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GradeModelMaster> FetchAsync(
    #else
        public static async Task<GradeModelMaster> FetchAsync(
    #endif
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GradeModelMaster>> fetchImpl
    #else
            Func<Task<GradeModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GradeModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            gradeName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        gradeName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GradeModelMaster).PutCache(
                        cache,
                        namespaceName,
                        gradeName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "gradeModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GradeModelMaster, bool> GetCache(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName
        ) {
            return cache.Get<GradeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    gradeName
                )
            );
        }

        public static void PutCache(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName
        ) {
            var (value, find) = cache.Get<GradeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    gradeName
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
                    gradeName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName
        ) {
            cache.Delete<GradeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    gradeName
                )
            );
        }

        public static void ListSubscribe(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<GradeModelMaster[]> callback
        ) {
            cache.ListSubscribe<GradeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GradeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GradeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}