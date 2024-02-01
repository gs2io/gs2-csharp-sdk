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

namespace Gs2.Gs2Script.Model.Cache
{
    public static partial class ScriptExt
    {
        public static string CacheParentKey(
            this Script self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "script",
                namespaceName,
                "Script"
            );
        }

        public static string CacheKey(
            this Script self,
            string scriptName
        ) {
            return string.Join(
                ":",
                scriptName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Script> FetchFuture(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            string scriptName,
            Func<IFuture<Script>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Script> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Script).PutCache(
                            cache,
                            namespaceName,
                            scriptName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "script") {
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
                    scriptName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Script>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Script> FetchAsync(
    #else
        public static async Task<Script> FetchAsync(
    #endif
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            string scriptName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Script>> fetchImpl
    #else
            Func<Task<Script>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<Script>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            scriptName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        scriptName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as Script).PutCache(
                        cache,
                        namespaceName,
                        scriptName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "script") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<Script, bool> GetCache(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            string scriptName
        ) {
            return cache.Get<Script>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    scriptName
                )
            );
        }

        public static void PutCache(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            string scriptName
        ) {
            var (value, find) = cache.Get<Script>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    scriptName
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
                    scriptName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            string scriptName
        ) {
            cache.Delete<Script>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    scriptName
                )
            );
        }

        public static void ListSubscribe(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            Action<Script[]> callback
        ) {
            cache.ListSubscribe<Script>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this Script self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Script>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}