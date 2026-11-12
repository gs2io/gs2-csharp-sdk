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
using Gs2.Gs2Account.Request;
using Gs2.Gs2Account.Result;
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

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class AccountExt
    {
        public static void PutCache(
            this PreviewTakeOverResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            PreviewTakeOverRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.NamespaceName,
                self.Item.UserId,
                timeOffset
            );
            foreach (var item in self.BanStatuses ?? Array.Empty<BanStatus>())
            {
                item.PutCache(
                    cache,
                    item.Name,
                    timeOffset
                );
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<PreviewTakeOverResult> InvokeFuture(
            this PreviewTakeOverRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<PreviewTakeOverResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<PreviewTakeOverResult> self)
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
            return new Gs2InlineFuture<PreviewTakeOverResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<PreviewTakeOverResult> InvokeAsync(
#else
        public static async Task<PreviewTakeOverResult> InvokeAsync(
#endif
            this PreviewTakeOverRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<PreviewTakeOverResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<PreviewTakeOverResult>> invokeImpl
#else
            Func<Task<PreviewTakeOverResult>> invokeImpl
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
    }
}