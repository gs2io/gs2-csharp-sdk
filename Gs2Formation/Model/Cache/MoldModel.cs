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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static partial class MoldModelExt
    {
        public static string CacheParentKey(
            this MoldModel self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "MoldModel"
            );
        }

        public static string CacheKey(
            this MoldModel self,
            string moldModelName
        ) {
            return string.Join(
                ":",
                moldModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<MoldModel> FetchFuture(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
            Func<IFuture<MoldModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<MoldModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as MoldModel).PutCache(
                            cache,
                            namespaceName,
                            moldModelName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "moldModel") {
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
                    moldModelName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<MoldModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<MoldModel> FetchAsync(
    #else
        public static async Task<MoldModel> FetchAsync(
    #endif
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<MoldModel>> fetchImpl
    #else
            Func<Task<MoldModel>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    moldModelName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as MoldModel).PutCache(
                    cache,
                    namespaceName,
                    moldModelName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "moldModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<MoldModel, bool> GetCache(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            return cache.Get<MoldModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
                )
            );
        }

        public static void PutCache(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            cache.Delete<MoldModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    moldModelName
                )
            );
        }

        public static void ListSubscribe(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<MoldModel[]> callback
        ) {
            cache.ListSubscribe<MoldModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this MoldModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<MoldModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}