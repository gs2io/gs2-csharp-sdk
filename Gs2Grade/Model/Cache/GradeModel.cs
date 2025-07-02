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

namespace Gs2.Gs2Grade.Model.Cache
{
    public static partial class GradeModelExt
    {
        public static string CacheParentKey(
            this GradeModel self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "grade",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "GradeModel"
            );
        }

        public static string CacheKey(
            this GradeModel self,
            string gradeName
        ) {
            return string.Join(
                ":",
                gradeName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GradeModel> FetchFuture(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            int? timeOffset,
            Func<IFuture<GradeModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GradeModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GradeModel).PutCache(
                            cache,
                            namespaceName,
                            gradeName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "gradeModel") {
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
                    gradeName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GradeModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GradeModel> FetchAsync(
    #else
        public static async Task<GradeModel> FetchAsync(
    #endif
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GradeModel>> fetchImpl
    #else
            Func<Task<GradeModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    gradeName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as GradeModel).PutCache(
                    cache,
                    namespaceName,
                    gradeName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "gradeModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<GradeModel, bool> GetCache(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            int? timeOffset
        ) {
            return cache.Get<GradeModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    gradeName
                )
            );
        }

        public static void PutCache(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    gradeName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            string gradeName,
            int? timeOffset
        ) {
            cache.Delete<GradeModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    gradeName
                )
            );
        }

        public static void ListSubscribe(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<GradeModel[]> callback
        ) {
            cache.ListSubscribe<GradeModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this GradeModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GradeModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}