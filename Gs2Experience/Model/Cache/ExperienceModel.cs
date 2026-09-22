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
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
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
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "experience",
                namespaceName,
                timeOffset?.ToString() ?? "0",
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
            int? timeOffset,
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
                            experienceName,
                            timeOffset
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
                    experienceName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<ExperienceModel>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<ExperienceModel> FetchAsync(
#else
        public static async Task<ExperienceModel> FetchAsync(
#endif
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<ExperienceModel>> fetchImpl
#else
            Func<Task<ExperienceModel>> fetchImpl
#endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    experienceName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as ExperienceModel).PutCache(
                    cache,
                    namespaceName,
                    experienceName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "experienceModel") {
                    throw;
                }
                return null;
            }
        }

        public static Tuple<ExperienceModel, bool> GetCache(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName,
            int? timeOffset
        ) {
            return cache.Get<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
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
            string experienceName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    experienceName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        /// <summary>
        /// Gs2Distributor:DescribeUserData（ユーザーの全データの一括取得）の 1 エントリ（この kind）をキャッシュへ入れる。
        /// 鍵はエントリの namespaceName / 読み込むユーザーの userId / モデル自身のプロパティ / 主キー GRN から取る
        /// （sdk-gen の BaseModel.user_data_cache_keys）。戻り値は親キーで、呼び手が全ページを読み終えてから
        /// Gs2Experience.SetListCached(cache, timeOffset, kind, parentKey) で「リストが揃った印」を立てる。
        /// </summary>
        public static string PutUserData(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            self.PutCache(
                cache,
                namespaceName,
                self.Name,
                timeOffset
            );
            return self.CacheParentKey(
                namespaceName,
                timeOffset
            );
        }

        public static void DeleteCache(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            string experienceName,
            int? timeOffset
        ) {
            cache.Delete<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
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
            int? timeOffset,
            Action<ExperienceModel[]> callback
        ) {
            cache.ListSubscribe<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this ExperienceModel self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<ExperienceModel>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}