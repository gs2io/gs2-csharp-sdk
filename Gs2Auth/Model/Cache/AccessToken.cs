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

namespace Gs2.Gs2Auth.Model.Cache
{
    public static partial class AccessTokenExt
    {
        public static string CacheParentKey(
            this AccessToken self,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "auth",
                timeOffset?.ToString() ?? "0",
                "AccessToken"
            );
        }

        public static string CacheKey(
            this AccessToken self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<AccessToken> FetchFuture(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset,
            Func<IFuture<AccessToken>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<AccessToken> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as AccessToken).PutCache(
                            cache,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "accessToken") {
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
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<AccessToken>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<AccessToken> FetchAsync(
    #else
        public static async Task<AccessToken> FetchAsync(
    #endif
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<AccessToken>> fetchImpl
    #else
            Func<Task<AccessToken>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as AccessToken).PutCache(
                    cache,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "accessToken") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<AccessToken, bool> GetCache(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset
        ) {
            return cache.Get<AccessToken>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset
        ) {
            cache.Delete<AccessToken>(
                self.CacheParentKey(
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset,
            Action<AccessToken[]> callback
        ) {
            cache.ListSubscribe<AccessToken>(
                self.CacheParentKey(
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this AccessToken self,
            CacheDatabase cache,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<AccessToken>(
                self.CacheParentKey(
                    timeOffset
                ),
                callbackId
            );
        }
    }
}