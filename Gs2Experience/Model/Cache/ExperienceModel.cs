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

namespace Gs2.Gs2Experience.Model.Cache
{
    public static partial class ExperienceModelExt
    {
        public static string CacheParentKey(
            this ExperienceModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "experience",
                namespaceName,
                "ExperienceModel"
            );
        }

        public static string CacheKey(
            this ExperienceModel self,
            string experienceName
        ) {
            return string.Join(
                ":",
                experienceName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ExperienceModel> FetchFuture(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName,
            Func<IFuture<ExperienceModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<ExperienceModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as ExperienceModel).PutCache(
                            cache,
                            namespaceName,
                            experienceName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "experienceModel") {
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
                    experienceName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ExperienceModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ExperienceModel> FetchAsync(
    #else
        public static async Task<ExperienceModel> FetchAsync(
    #endif
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ExperienceModel>> fetchImpl
    #else
            Func<Task<ExperienceModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<ExperienceModel>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            experienceName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        experienceName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as ExperienceModel).PutCache(
                        cache,
                        namespaceName,
                        experienceName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "experienceModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<ExperienceModel, bool> GetCache(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName
        ) {
            return cache.Get<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    experienceName
                )
            );
        }

        public static void PutCache(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    experienceName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName
        ) {
            cache.Delete<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    experienceName
                )
            );
        }

        public static void ListSubscribe(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<ExperienceModel[]> callback
        ) {
            cache.ListSubscribe<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}