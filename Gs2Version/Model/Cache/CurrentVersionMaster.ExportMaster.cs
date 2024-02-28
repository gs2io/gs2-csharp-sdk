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
using Gs2.Gs2Version.Request;
using Gs2.Gs2Version.Result;
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

namespace Gs2.Gs2Version.Model.Cache
{
    public static partial class CurrentVersionMasterExt
    {
        public static void PutCache(
            this ExportMasterResult self,
            CacheDatabase cache,
            string userId,
            ExportMasterRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.NamespaceName
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<ExportMasterResult> InvokeFuture(
            this ExportMasterRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<ExportMasterResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<ExportMasterResult> self)
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
            return new Gs2InlineFuture<ExportMasterResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<ExportMasterResult> InvokeAsync(
    #else
        public static async Task<ExportMasterResult> InvokeAsync(
    #endif
            this ExportMasterRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<ExportMasterResult>> invokeImpl
    #else
            Func<Task<ExportMasterResult>> invokeImpl
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