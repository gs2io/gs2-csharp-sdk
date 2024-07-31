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
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Account.Model.Cache
{
    public static partial class TakeOverTypeModelMasterExt
    {
        public static void PutCache(
            this DescribeTakeOverTypeModelMastersResult self,
            CacheDatabase cache,
            string userId,
            DescribeTakeOverTypeModelMastersRequest request
        ) {
            foreach (var item in self.Items ?? Array.Empty<TakeOverTypeModelMaster>())
            {
                item.PutCache(
                    cache,
                    request.NamespaceName,
                    item.Type
                );
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DescribeTakeOverTypeModelMastersResult> InvokeFuture(
            this DescribeTakeOverTypeModelMastersRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<DescribeTakeOverTypeModelMastersResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DescribeTakeOverTypeModelMastersResult> self)
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
            return new Gs2InlineFuture<DescribeTakeOverTypeModelMastersResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DescribeTakeOverTypeModelMastersResult> InvokeAsync(
    #else
        public static async Task<DescribeTakeOverTypeModelMastersResult> InvokeAsync(
    #endif
            this DescribeTakeOverTypeModelMastersRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DescribeTakeOverTypeModelMastersResult>> invokeImpl
    #else
            Func<Task<DescribeTakeOverTypeModelMastersResult>> invokeImpl
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