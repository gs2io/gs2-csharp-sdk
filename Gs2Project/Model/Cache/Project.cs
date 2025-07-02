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

namespace Gs2.Gs2Project.Model.Cache
{
    public static partial class ProjectExt
    {
        public static string CacheParentKey(
            this Project self,
            string accountName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "project",
                accountName,
                timeOffset?.ToString() ?? "0",
                "Project"
            );
        }

        public static string CacheKey(
            this Project self,
            string projectName
        ) {
            return string.Join(
                ":",
                projectName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<Project> FetchFuture(
            this Project self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? timeOffset,
            Func<IFuture<Project>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<Project> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as Project).PutCache(
                            cache,
                            accountName,
                            projectName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "project") {
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
                    accountName,
                    projectName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<Project>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Project> FetchAsync(
    #else
        public static async Task<Project> FetchAsync(
    #endif
            this Project self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<Project>> fetchImpl
    #else
            Func<Task<Project>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    accountName,
                    projectName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as Project).PutCache(
                    cache,
                    accountName,
                    projectName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "project") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<Project, bool> GetCache(
            this Project self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? timeOffset
        ) {
            return cache.Get<Project>(
                self.CacheParentKey(
                    accountName,
                    timeOffset
                ),
                self.CacheKey(
                    projectName
                )
            );
        }

        public static void PutCache(
            this Project self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? timeOffset
        ) {
            cache.Put(
                self.CacheParentKey(
                    accountName,
                    timeOffset
                ),
                self.CacheKey(
                    projectName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this Project self,
            CacheDatabase cache,
            string accountName,
            string projectName,
            int? timeOffset
        ) {
            cache.Delete<Project>(
                self.CacheParentKey(
                    accountName,
                    timeOffset
                ),
                self.CacheKey(
                    projectName
                )
            );
        }

        public static void ListSubscribe(
            this Project self,
            CacheDatabase cache,
            string accountName,
            int? timeOffset,
            Action<Project[]> callback
        ) {
            cache.ListSubscribe<Project>(
                self.CacheParentKey(
                    accountName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this Project self,
            CacheDatabase cache,
            string accountName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<Project>(
                self.CacheParentKey(
                    accountName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}