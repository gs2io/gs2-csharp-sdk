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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static partial class FormModelMasterExt
    {
        public static string CacheParentKey(
            this FormModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                "FormModelMaster"
            );
        }

        public static string CacheKey(
            this FormModelMaster self,
            string formModelName
        ) {
            return string.Join(
                ":",
                formModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<FormModelMaster> FetchFuture(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string formModelName,
            Func<IFuture<FormModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<FormModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as FormModelMaster).PutCache(
                            cache,
                            namespaceName,
                            formModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "formModelMaster") {
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
                    formModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<FormModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<FormModelMaster> FetchAsync(
    #else
        public static async Task<FormModelMaster> FetchAsync(
    #endif
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string formModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<FormModelMaster>> fetchImpl
    #else
            Func<Task<FormModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<FormModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            formModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        formModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as FormModelMaster).PutCache(
                        cache,
                        namespaceName,
                        formModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "formModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<FormModelMaster, bool> GetCache(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string formModelName
        ) {
            return cache.Get<FormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    formModelName
                )
            );
        }

        public static void PutCache(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string formModelName
        ) {
            var (value, find) = cache.Get<FormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    formModelName
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
                    formModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string formModelName
        ) {
            cache.Delete<FormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    formModelName
                )
            );
        }

        public static void ListSubscribe(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<FormModelMaster[]> callback
        ) {
            cache.ListSubscribe<FormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this FormModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<FormModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}