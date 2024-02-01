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

namespace Gs2.Gs2SkillTree.Model.Cache
{
    public static partial class NodeModelMasterExt
    {
        public static string CacheParentKey(
            this NodeModelMaster self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "skillTree",
                namespaceName,
                "NodeModelMaster"
            );
        }

        public static string CacheKey(
            this NodeModelMaster self,
            string nodeModelName
        ) {
            return string.Join(
                ":",
                nodeModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<NodeModelMaster> FetchFuture(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName,
            Func<IFuture<NodeModelMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<NodeModelMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as NodeModelMaster).PutCache(
                            cache,
                            namespaceName,
                            nodeModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "nodeModelMaster") {
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
                    nodeModelName
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<NodeModelMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<NodeModelMaster> FetchAsync(
    #else
        public static async Task<NodeModelMaster> FetchAsync(
    #endif
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<NodeModelMaster>> fetchImpl
    #else
            Func<Task<NodeModelMaster>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<NodeModelMaster>(
                       self.CacheParentKey(
                            namespaceName
                       ),
                       self.CacheKey(
                            nodeModelName
                       )
                   ).LockAsync()) {
                try {
                    var item = await fetchImpl();
                    item.PutCache(
                        cache,
                        namespaceName,
                        nodeModelName
                    );
                    return item;
                }
                catch (Gs2.Core.Exception.NotFoundException e) {
                    (null as NodeModelMaster).PutCache(
                        cache,
                        namespaceName,
                        nodeModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "nodeModelMaster") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<NodeModelMaster, bool> GetCache(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
            return cache.Get<NodeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    nodeModelName
                )
            );
        }

        public static void PutCache(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
            var (value, find) = cache.Get<NodeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    nodeModelName
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
                    nodeModelName
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
            cache.Delete<NodeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    nodeModelName
                )
            );
        }

        public static void ListSubscribe(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            Action<NodeModelMaster[]> callback
        ) {
            cache.ListSubscribe<NodeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this NodeModelMaster self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<NodeModelMaster>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}