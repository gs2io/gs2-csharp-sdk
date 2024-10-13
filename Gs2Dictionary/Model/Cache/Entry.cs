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

namespace Gs2.Gs2Dictionary.Model.Cache
{
    public static partial class EntryExt
    {
        public static string CacheParentKey(
            this Entry self,
            string namespaceName,
            string userId
        ) {
            return string.Join(
                ":",
                "dictionary",
                namespaceName,
                userId,
                "Entry"
            );
        }

        public static string CacheKey(
            this Entry self,
            string entryModelName
        ) {
            return string.Join(
                ":",
                entryModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Entry> FetchFuture(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string entryModelName,
            Func<IFuture<Entry>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Entry> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Entry).PutCache(
                            cache,
                            namespaceName,
                            userId,
                            entryModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "entry") {
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
                    entryModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Entry>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Entry> FetchAsync(
    #else
        public static async Task<Entry> FetchAsync(
    #endif
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string entryModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Entry>> fetchImpl
    #else
            Func<Task<Entry>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Entry>(
                       self.CacheParentKey(
                            namespaceName,
                            userId
                       ),
                       self.CacheKey(
                            entryModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        userId,
                        entryModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Entry).PutCache(
                        cache,
                        namespaceName,
                        userId,
                        entryModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "entry") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Entry, bool> GetCache(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string entryModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            return cache.Get<Entry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    entryModelName
                )
            );
        }

        public static void PutCache(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string entryModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    entryModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string entryModelName
        ) {
            if (userId == null) {
                throw new NullReferenceException();
            }
            cache.Delete<Entry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                self.CacheKey(
                    entryModelName
                )
            );
        }

        public static void ListSubscribe(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            Action<Entry[]> callback
        ) {
            cache.ListSubscribe<Entry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Entry self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Entry>(
                self.CacheParentKey(
                    namespaceName,
                    userId
                ),
                callbackId
            );
        }
    }
}