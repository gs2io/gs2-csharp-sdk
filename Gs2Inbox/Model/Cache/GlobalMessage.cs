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

namespace Gs2.Gs2Inbox.Model.Cache
{
    public static partial class GlobalMessageExt
    {
        public static string CacheParentKey(
            this GlobalMessage self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "inbox",
                namespaceName,
                "GlobalMessage"
            );
        }

        public static string CacheKey(
            this GlobalMessage self,
            string globalMessageName
        ) {
            return string.Join(
                ":",
                globalMessageName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GlobalMessage> FetchFuture(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            string globalMessageName,
            Func<IFuture<GlobalMessage>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<GlobalMessage> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as GlobalMessage).PutCache(
                            cache,
                            namespaceName,
                            globalMessageName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "globalMessage") {
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
                    globalMessageName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<GlobalMessage>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GlobalMessage> FetchAsync(
    #else
        public static async Task<GlobalMessage> FetchAsync(
    #endif
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            string globalMessageName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GlobalMessage>> fetchImpl
    #else
            Func<Task<GlobalMessage>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<GlobalMessage>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            globalMessageName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        globalMessageName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as GlobalMessage).PutCache(
                        cache,
                        namespaceName,
                        globalMessageName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "globalMessage") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<GlobalMessage, bool> GetCache(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            string globalMessageName
        ) {
            return cache.Get<GlobalMessage>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    globalMessageName
                )
            );
        }

        public static void PutCache(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            string globalMessageName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    globalMessageName
                ),
                self,
                ((self?.ExpiresAt ?? 0) == 0 ? null : self.ExpiresAt) ?? UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            string globalMessageName
        ) {
            cache.Delete<GlobalMessage>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    globalMessageName
                )
            );
        }

        public static void ListSubscribe(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            Action<GlobalMessage[]> callback
        ) {
            cache.ListSubscribe<GlobalMessage>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this GlobalMessage self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<GlobalMessage>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}