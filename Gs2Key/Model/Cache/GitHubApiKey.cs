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

namespace Gs2.Gs2Key.Model.Cache
{
    public static partial class GitHubApiKeyExt
    {
        public static string CacheParentKey(
            this GitHubApiKey self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "key",
                namespaceName,
                "GitHubApiKey"
            );
        }

        public static string CacheKey(
            this GitHubApiKey self,
            string apiKeyName
        ) {
            return string.Join(
                ":",
                apiKeyName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GitHubApiKey> FetchFuture(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            string apiKeyName,
            Func<IFuture<GitHubApiKey>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GitHubApiKey> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GitHubApiKey).PutCache(
                            cache,
                            namespaceName,
                            apiKeyName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "gitHubApiKey") {
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
                    apiKeyName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GitHubApiKey>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GitHubApiKey> FetchAsync(
    #else
        public static async Task<GitHubApiKey> FetchAsync(
    #endif
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            string apiKeyName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GitHubApiKey>> fetchImpl
    #else
            Func<Task<GitHubApiKey>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GitHubApiKey>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            apiKeyName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        apiKeyName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GitHubApiKey).PutCache(
                        cache,
                        namespaceName,
                        apiKeyName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "gitHubApiKey") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GitHubApiKey, bool> GetCache(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            string apiKeyName
        ) {
            return cache.Get<GitHubApiKey>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    apiKeyName
                )
            );
        }

        public static void PutCache(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            string apiKeyName
        ) {
            var (value, find) = cache.Get<GitHubApiKey>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    apiKeyName
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
                    apiKeyName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            string apiKeyName
        ) {
            cache.Delete<GitHubApiKey>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    apiKeyName
                )
            );
        }

        public static void ListSubscribe(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            Action<GitHubApiKey[]> callback
        ) {
            cache.ListSubscribe<GitHubApiKey>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GitHubApiKey self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GitHubApiKey>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}