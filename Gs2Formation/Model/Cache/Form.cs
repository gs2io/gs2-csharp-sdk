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
    public static partial class FormExt
    {
        public static string CacheParentKey(
            this Form self,
            string namespaceName,
            string userId,
            string moldModelName
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                userId,
                moldModelName,
                "Form"
            );
        }

        public static string CacheKey(
            this Form self,
            int? index
        ) {
            return string.Join(
                ":",
                index.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Form> FetchFuture(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index,
            Func<IFuture<Form>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Form> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Form).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            moldModelName,
                            index
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "form") {
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
                    moldModelName,
                    index
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Form>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Form> FetchAsync(
    #else
        public static async Task<Form> FetchAsync(
    #endif
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Form>> fetchImpl
    #else
            Func<Task<Form>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Form>(
                       self.CacheParentKey(
                            namespaceName,
                            userId,
                            moldModelName
                       ),
                       self.CacheKey(
                            index
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        moldModelName,
                        index
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Form).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        moldModelName,
                        index
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "form") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Form, bool> GetCache(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Form>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                self.CacheKey(
                    index
                )
            );
        }

        public static void PutCache(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            var (value, find) = cache.Get<Form>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                self.CacheKey(
                    index
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                self.CacheKey(
                    index
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Form>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                self.CacheKey(
                    index
                )
            );
        }

        public static void ListSubscribe(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            Action<Form[]> callback
        ) {
            cache.ListSubscribe<Form>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Form self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Form>(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    moldModelName
                ),
                callbackId
            );
        }
    }
}