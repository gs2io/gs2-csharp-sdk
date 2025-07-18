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

namespace Gs2.Gs2StateMachine.Model.Cache
{
    public static partial class StateMachineMasterExt
    {
        public static string CacheParentKey(
            this StateMachineMaster self,
            string namespaceName,
            int? timeOffset
        ) {
            return string.Join(
                ":",
                "stateMachine",
                namespaceName,
                timeOffset?.ToString() ?? "0",
                "StateMachineMaster"
            );
        }

        public static string CacheKey(
            this StateMachineMaster self,
            long? version
        ) {
            return string.Join(
                ":",
                version.ToString()
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<StateMachineMaster> FetchFuture(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            long? version,
            int? timeOffset,
            Func<IFuture<StateMachineMaster>> fetchImpl
        ) {
            IEnumerator Impl(IFuture<StateMachineMaster> self)
            {
                var future = fetchImpl();
                yield return future;
                if (future.Error != null)
                {
                    if (future.Error is Gs2.Core.Exception.NotFoundException e)
                    {
                        (null as StateMachineMaster).PutCache(
                            cache,
                            namespaceName,
                            version,
                            timeOffset
                        );
                        if (e.Errors.Length != 0 && e.Errors[0].Component == "stateMachineMaster") {
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
                    version,
                    timeOffset
                );
                self.OnComplete(item);
            }
            return new Gs2InlineFuture<StateMachineMaster>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<StateMachineMaster> FetchAsync(
    #else
        public static async Task<StateMachineMaster> FetchAsync(
    #endif
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            long? version,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<StateMachineMaster>> fetchImpl
    #else
            Func<Task<StateMachineMaster>> fetchImpl
    #endif
        ) {
            try {
                var item = await fetchImpl();
                item.PutCache(
                    cache,
                    namespaceName,
                    version,
                    timeOffset
                );
                return item;
            }
            catch (Gs2.Core.Exception.NotFoundException e) {
                (null as StateMachineMaster).PutCache(
                    cache,
                    namespaceName,
                    version,
                    timeOffset
                );
                if (e.errors.Length == 0 || e.errors[0].component != "stateMachineMaster") {
                    throw;
                }
                return null;
            }
        }
#endif

        public static Tuple<StateMachineMaster, bool> GetCache(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            long? version,
            int? timeOffset
        ) {
            return cache.Get<StateMachineMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    version
                )
            );
        }

        public static void PutCache(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            long? version,
            int? timeOffset
        ) {
            var (value, find) = cache.Get<StateMachineMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    version
                )
            );
            if (find && (value?.Revision ?? -1) > (self?.Revision ?? -1) && (self?.Revision ?? -1) > 1) {
                return;
            }
            if (find && (value?.Revision ?? -1) == (self?.Revision ?? -1)) {
                return;
            }
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    version
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }

        public static void DeleteCache(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            long? version,
            int? timeOffset
        ) {
            cache.Delete<StateMachineMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                self.CacheKey(
                    version
                )
            );
        }

        public static void ListSubscribe(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            Action<StateMachineMaster[]> callback
        ) {
            cache.ListSubscribe<StateMachineMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callback,
                () => {}
            );
        }

        public static void ListUnsubscribe(
            this StateMachineMaster self,
            CacheDatabase cache,
            string namespaceName,
            int? timeOffset,
            ulong callbackId
        ) {
            cache.ListUnsubscribe<StateMachineMaster>(
                self.CacheParentKey(
                    namespaceName,
                    timeOffset
                ),
                callbackId
            );
        }
    }
}