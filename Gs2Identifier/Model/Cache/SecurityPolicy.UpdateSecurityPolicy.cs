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
using Gs2.Gs2Identifier.Request;
using Gs2.Gs2Identifier.Result;
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

namespace Gs2.Gs2Identifier.Model.Cache
{
    public static partial class SecurityPolicyExt
    {
        public static void PutCache(
            this UpdateSecurityPolicyResult self,
            CacheDatabase cache,
            string userId,
            UpdateSecurityPolicyRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.SecurityPolicyName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<UpdateSecurityPolicyResult> InvokeFuture(
            this UpdateSecurityPolicyRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<UpdateSecurityPolicyResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<UpdateSecurityPolicyResult> self)
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
            return new Gs2InlineFuture<UpdateSecurityPolicyResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<UpdateSecurityPolicyResult> InvokeAsync(
    #else
        public static async Task<UpdateSecurityPolicyResult> InvokeAsync(
    #endif
            this UpdateSecurityPolicyRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<UpdateSecurityPolicyResult>> invokeImpl
    #else
            Func<Task<UpdateSecurityPolicyResult>> invokeImpl
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