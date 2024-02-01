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

namespace Gs2.Gs2News.Model.Cache
{
    public static partial class OutputExt
    {
        public static string CacheParentKey(
            this Output self,
            string namespaceName,
            string uploadToken
        ) {
            return string.Join(
                ":",
                "news",
                namespaceName,
                uploadToken,
                "Output"
            );
        }

        public static string CacheKey(
            this Output self,
            string outputName
        ) {
            return string.Join(
                ":",
                outputName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Output> FetchFuture(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            string outputName,
            Func<IFuture<Output>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Output> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Output).PutCache(
                            cache,
                            namespaceName,
                            uploadToken,
                            outputName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "output") {
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
                    uploadToken,
                    outputName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Output>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Output> FetchAsync(
    #else
        public static async Task<Output> FetchAsync(
    #endif
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            string outputName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Output>> fetchImpl
    #else
            Func<Task<Output>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Output>(
                       self.CacheParentKey(
                            namespaceName,
                            uploadToken
                       ),
                       self.CacheKey(
                            outputName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        uploadToken,
                        outputName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Output).PutCache(
                        cache,
                        namespaceName,
                        uploadToken,
                        outputName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "output") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Output, bool> GetCache(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            string outputName
        ) {
            return cache.Get<Output>(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                self.CacheKey(
                    outputName
                )
            );
        }

        public static void PutCache(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            string outputName
        ) {
            var (value, find) = cache.Get<Output>(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                self.CacheKey(
                    outputName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                self.CacheKey(
                    outputName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            string outputName
        ) {
            cache.Delete<Output>(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                self.CacheKey(
                    outputName
                )
            );
        }

        public static void ListSubscribe(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            Action<Output[]> callback
        ) {
            cache.ListSubscribe<Output>(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Output self,
            CacheDatabase cache,
            string namespaceName,
            string uploadToken,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Output>(
                self.CacheParentKey(
                    namespaceName,
                    uploadToken
                ),
                callbackId
            );
        }
    }
}