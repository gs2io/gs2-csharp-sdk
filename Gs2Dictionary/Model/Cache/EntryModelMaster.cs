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
    public static partial class EntryModelMasterExt
    {
        public static string CacheParentKey(
            this EntryModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "dictionary",
                namespaceName,
                "EntryModelMaster"
            );
        }

        public static string CacheKey(
            this EntryModelMaster self,
            string entryModelName
        ) {
            return string.Join(
                ":",
                entryModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<EntryModelMaster> FetchFuture(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string entryModelName,
            Func<IFuture<EntryModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<EntryModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as EntryModelMaster).PutCache(
                            cache,
                            namespaceName,
                            entryModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "entryModelMaster") {
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
                    entryModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<EntryModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<EntryModelMaster> FetchAsync(
    #else
        public static async Task<EntryModelMaster> FetchAsync(
    #endif
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string entryModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<EntryModelMaster>> fetchImpl
    #else
            Func<Task<EntryModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<EntryModelMaster>(
                       self.CacheParentKey(
                            namespaceName
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
                        entryModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as EntryModelMaster).PutCache(
                        cache,
                        namespaceName,
                        entryModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "entryModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<EntryModelMaster, bool> GetCache(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string entryModelName
        ) {
            return cache.Get<EntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    entryModelName
                )
            );
        }

        public static void PutCache(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string entryModelName
        ) {
            var (value, find) = cache.Get<EntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    entryModelName
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
                    entryModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string entryModelName
        ) {
            cache.Delete<EntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    entryModelName
                )
            );
        }

        public static void ListSubscribe(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<EntryModelMaster[]> callback
        ) {
            cache.ListSubscribe<EntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this EntryModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<EntryModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}