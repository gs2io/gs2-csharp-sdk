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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Result;
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

namespace Gs2.Gs2Matchmaking.Model.Cache
{
    public static partial class SeasonGatheringExt
    {
        public static void PutCache(
            this DeleteSeasonGatheringResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            DeleteSeasonGatheringRequest request
        ) {
            (null as SeasonGathering).DeleteCache(
                cache,
                request.NamespaceName,
                userId,
                self.Item.SeasonName,
                self.Item.Season,
                self.Item.Tier,
                request.SeasonGatheringName,
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DeleteSeasonGatheringResult> InvokeFuture(
            this DeleteSeasonGatheringRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<DeleteSeasonGatheringResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DeleteSeasonGatheringResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    self.OnError(future.Error);
                    yield break;
                }

                future.Result.PutCache(
                    cache,
                    userId,
                    timeOffset,
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<DeleteSeasonGatheringResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DeleteSeasonGatheringResult> InvokeAsync(
    #else
        public static async Task<DeleteSeasonGatheringResult> InvokeAsync(
    #endif
            this DeleteSeasonGatheringRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DeleteSeasonGatheringResult>> invokeImpl
    #else
            Func<Task<DeleteSeasonGatheringResult>> invokeImpl
    #endif
        )
        {
            var result = await invokeImpl();
            result.PutCache(
                cache,
                userId,
                timeOffset,
                request
            );
            return result;
        }
#endif
    }
}