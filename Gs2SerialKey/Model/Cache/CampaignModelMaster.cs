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

namespace Gs2.Gs2SerialKey.Model.Cache
{
    public static partial class CampaignModelMasterExt
    {
        public static string CacheParentKey(
            this CampaignModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "serialKey",
                namespaceName,
                "CampaignModelMaster"
            );
        }

        public static string CacheKey(
            this CampaignModelMaster self,
            string campaignModelName
        ) {
            return string.Join(
                ":",
                campaignModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<CampaignModelMaster> FetchFuture(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
            Func<IFuture<CampaignModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<CampaignModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as CampaignModelMaster).PutCache(
                            cache,
                            namespaceName,
                            campaignModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "campaignModelMaster") {
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
                    campaignModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<CampaignModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<CampaignModelMaster> FetchAsync(
    #else
        public static async Task<CampaignModelMaster> FetchAsync(
    #endif
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<CampaignModelMaster>> fetchImpl
    #else
            Func<Task<CampaignModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<CampaignModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            campaignModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        campaignModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as CampaignModelMaster).PutCache(
                        cache,
                        namespaceName,
                        campaignModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "campaignModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<CampaignModelMaster, bool> GetCache(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName
        ) {
            return cache.Get<CampaignModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    campaignModelName
                )
            );
        }

        public static void PutCache(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName
        ) {
            var (value, find) = cache.Get<CampaignModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    campaignModelName
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
                    campaignModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string campaignModelName
        ) {
            cache.Delete<CampaignModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    campaignModelName
                )
            );
        }

        public static void ListSubscribe(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<CampaignModelMaster[]> callback
        ) {
            cache.ListSubscribe<CampaignModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this CampaignModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<CampaignModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}