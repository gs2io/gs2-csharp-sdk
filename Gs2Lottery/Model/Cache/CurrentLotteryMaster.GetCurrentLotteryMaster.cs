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
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Result;
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

namespace Gs2.Gs2Lottery.Model.Cache
{
    public static partial class CurrentLotteryMasterExt
    {
        public static void PutCache(
            this GetCurrentLotteryMasterResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            GetCurrentLotteryMasterRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.NamespaceName,
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<GetCurrentLotteryMasterResult> InvokeFuture(
            this GetCurrentLotteryMasterRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<GetCurrentLotteryMasterResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<GetCurrentLotteryMasterResult> self)
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
            return new Gs2InlineFuture<GetCurrentLotteryMasterResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<GetCurrentLotteryMasterResult> InvokeAsync(
    #else
        public static async Task<GetCurrentLotteryMasterResult> InvokeAsync(
    #endif
            this GetCurrentLotteryMasterRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<GetCurrentLotteryMasterResult>> invokeImpl
    #else
            Func<Task<GetCurrentLotteryMasterResult>> invokeImpl
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