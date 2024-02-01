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
    public static partial class NodeModelExt
    {
        public static string CacheParentKey(
            this NodeModel self,
            string namespaceName
        ) {
            return string.Join(
                ":",
                "skillTree",
                namespaceName,
                "NodeModel"
            );
        }

        public static string CacheKey(
            this NodeModel self,
            string nodeModelName
        ) {
            return string.Join(
                ":",
                nodeModelName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<NodeModel> FetchFuture(
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName,
            Func<IFuture<NodeModel>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<NodeModel> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as NodeModel).PutCache(
                            cache,
                            namespaceName,
                            nodeModelName
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "nodeModel") {
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
            return new Gs2InlineFuture<NodeModel>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<NodeModel> FetchAsync(
    #else
        public static async Task<NodeModel> FetchAsync(
    #endif
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<NodeModel>> fetchImpl
    #else
            Func<Task<NodeModel>> fetchImpl
    #endif
        ) {
            using (await cache.GetLockObject<NodeModel>(
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
                    (null as NodeModel).PutCache(
                        cache,
                        namespaceName,
                        nodeModelName
                    );
                    if (e.errors.Length == 0 || e.errors[0].component != "nodeModel") {
                        throw;
                    }
                    return null;
                }
            }
        }
#endif

        public static Tuple<NodeModel, bool> GetCache(
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
            return cache.Get<NodeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    nodeModelName
                )
            );
        }

        public static void PutCache(
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
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
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            string nodeModelName
        ) {
            cache.Delete<NodeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                self.CacheKey(
                    nodeModelName
                )
            );
        }

        public static void ListSubscribe(
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            Action<NodeModel[]> callback
        ) {
            cache.ListSubscribe<NodeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callback
            );
        }

        public static void ListUnsubscribe(
            this NodeModel self,
            CacheDatabase cache,
            string namespaceName,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<NodeModel>(
                self.CacheParentKey(
                    namespaceName
                ),
                callbackId
            );
        }
    }
}