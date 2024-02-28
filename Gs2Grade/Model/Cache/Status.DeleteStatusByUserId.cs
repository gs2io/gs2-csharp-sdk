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
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
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

namespace Gs2.Gs2Grade.Model.Cache
{
    public static partial class StatusExt
    {
        public static void PutCache(
            this DeleteStatusByUserIdResult self,
            CacheDatabase cache,
            string userId,
            DeleteStatusByUserIdRequest request
        ) {
            (null as Status).DeleteCache(
                cache,
                request.NamespaceName,
                self.Item.UserId,
                self.Item.GradeName,
                self.Item.PropertyId
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DeleteStatusByUserIdResult> InvokeFuture(
            this DeleteStatusByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<DeleteStatusByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DeleteStatusByUserIdResult> self)
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
                    request
                );

                self.OnComplete(future.Result);
            }
            return new Gs2InlineFuture<DeleteStatusByUserIdResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DeleteStatusByUserIdResult> InvokeAsync(
    #else
        public static async Task<DeleteStatusByUserIdResult> InvokeAsync(
    #endif
            this DeleteStatusByUserIdRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DeleteStatusByUserIdResult>> invokeImpl
    #else
            Func<Task<DeleteStatusByUserIdResult>> invokeImpl
    #endif
        )
        {
            var result = await invokeImpl();
            result.PutCache(
                cache,
                userId,
                request
            );
            return result;
        }
#endif
    }
}