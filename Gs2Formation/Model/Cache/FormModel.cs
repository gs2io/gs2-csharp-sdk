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
    public static partial class FormModelExt
    {
        public static string CacheParentKey(
            this FormModel self,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "formation",
                namespaceName,
                moldModelName,
                timeOffset?.ToString() ?? "0",
                "FormModel"
            );
        }

        public static string CacheKey(
            this FormModel self
        ) {
            return "Singleton";
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<FormModel> FetchFuture(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
            Func<IFuture<FormModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<FormModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as FormModel).PutCache(
                            cache,
                            namespaceName,
                            moldModelName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "formModel") {
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
            return new Gs2InlineFuture<FormModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<FormModel> FetchAsync(
    #else
        public static async Task<FormModel> FetchAsync(
    #endif
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<FormModel>> fetchImpl
    #else
            Func<Task<FormModel>> fetchImpl
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
                (null as FormModel).PutCache(
                    cache,
                    namespaceName,
                    moldModelName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "formModel") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<FormModel, bool> GetCache(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            return cache.Get<FormModel>(
                self.CacheParentKey(
                    namespaceName,
                    moldModelName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void PutCache(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    moldModelName,
                    timeOffset
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset
        ) {
            cache.Delete<FormModel>(
                self.CacheParentKey(
                    namespaceName,
                    moldModelName,
                    timeOffset
                ),
                self.CacheKey(
                )
            );
        }

        public static void ListSubscribe(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
            Action<FormModel[]> callback
        ) {
            cache.ListSubscribe<FormModel>(
                self.CacheParentKey(
                    namespaceName,
                    moldModelName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this FormModel self,
            CacheDatabase cache,
            string namespaceName,
            string moldModelName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<FormModel>(
                self.CacheParentKey(
                    namespaceName,
                    moldModelName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}