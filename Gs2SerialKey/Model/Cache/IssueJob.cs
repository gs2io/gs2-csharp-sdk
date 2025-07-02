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

namespace Gs2.Gs2SerialKey.Model.Cache
{
    public static partial class IssueJobExt
    {
        public static string CacheParentKey(
            this IssueJob self,
            string namespaceName,
            string campaignModelName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "serialKey",
                namespaceName,
                campaignModelName,
                timeOffset?.ToString() ?? "0",
                "IssueJob"
            );
        }

        public static string CacheKey(
            this IssueJob self,
            string issueJobName
        ) {
            return string.Join(
                ":",
                issueJobName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<IssueJob> FetchFuture(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            string issueJobName,
            int? timeOffset,
            Func<IFuture<IssueJob>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<IssueJob> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as IssueJob).PutCache(
                            cache,
                            namespaceName,
                            campaignModelName,
                            issueJobName,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "issueJob") {
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
                    campaignModelName,
                    issueJobName,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<IssueJob>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<IssueJob> FetchAsync(
    #else
        public static async Task<IssueJob> FetchAsync(
    #endif
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            string issueJobName,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<IssueJob>> fetchImpl
    #else
            Func<Task<IssueJob>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    campaignModelName,
                    issueJobName,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as IssueJob).PutCache(
                    cache,
                    namespaceName,
                    campaignModelName,
                    issueJobName,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "issueJob") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<IssueJob, bool> GetCache(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            string issueJobName,
            int? timeOffset
        ) {
            return cache.Get<IssueJob>(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                self.CacheKey(
                    issueJobName
                )
            );
        }

        public static void PutCache(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            string issueJobName,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<IssueJob>(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                self.CacheKey(
                    issueJobName
                )
            );
            if (find && (value?.Revision ?? 0) > (self?.Revision ?? 0) && (self?.Revision ?? 0) > 1) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                self.CacheKey(
                    issueJobName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            string issueJobName,
            int? timeOffset
        ) {
            cache.Delete<IssueJob>(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                self.CacheKey(
                    issueJobName
                )
            );
        }

        public static void ListSubscribe(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            int? timeOffset,
            Action<IssueJob[]> callback
        ) {
            cache.ListSubscribe<IssueJob>(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this IssueJob self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<IssueJob>(
                self.CacheParentKey(
                    namespaceName,
                    campaignModelName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}