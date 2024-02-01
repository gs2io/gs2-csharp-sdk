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

namespace Gs2.Gs2Identifier.Model.Cache
{
    public static partial class SecurityPolicyExt
    {
        public static string CacheParentKey(
            this SecurityPolicy self
        ) {
            return string.Join(
                ":",
                "identifier",
                "SecurityPolicy"
            );
        }

        public static string CacheKey(
            this SecurityPolicy self,
            string securityPolicyName
        ) {
            return string.Join(
                ":",
                securityPolicyName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SecurityPolicy> FetchFuture(
            this SecurityPolicy self,
            CacheDatabase cache,
            string securityPolicyName,
            Func<IFuture<SecurityPolicy>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<SecurityPolicy> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as SecurityPolicy).PutCache(
                            cache,
                            securityPolicyName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "securityPolicy") {
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
                    securityPolicyName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<SecurityPolicy>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<SecurityPolicy> FetchAsync(
    #else
        public static async Task<SecurityPolicy> FetchAsync(
    #endif
            this SecurityPolicy self,
            CacheDatabase cache,
            string securityPolicyName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<SecurityPolicy>> fetchImpl
    #else
            Func<Task<SecurityPolicy>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<SecurityPolicy>(
                       self.CacheParentKey(
                       ),
                       self.CacheKey(
                            securityPolicyName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        securityPolicyName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as SecurityPolicy).PutCache(
                        cache,
                        securityPolicyName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "securityPolicy") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<SecurityPolicy, bool> GetCache(
            this SecurityPolicy self,
            CacheDatabase cache,
            string securityPolicyName
        ) {
            return cache.Get<SecurityPolicy>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    securityPolicyName
                )
            );
        }

        public static void PutCache(
            this SecurityPolicy self,
            CacheDatabase cache,
            string securityPolicyName
        ) {
            cache.Put(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    securityPolicyName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this SecurityPolicy self,
            CacheDatabase cache,
            string securityPolicyName
        ) {
            cache.Delete<SecurityPolicy>(
                self.CacheParentKey(
                ),
                self.CacheKey(
                    securityPolicyName
                )
            );
        }

        public static void ListSubscribe(
            this SecurityPolicy self,
            CacheDatabase cache,
            Action<SecurityPolicy[]> callback
        ) {
            cache.ListSubscribe<SecurityPolicy>(
                self.CacheParentKey(
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this SecurityPolicy self,
            CacheDatabase cache,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<SecurityPolicy>(
                self.CacheParentKey(
                ),
                callbackId
            );
        }
    }
}