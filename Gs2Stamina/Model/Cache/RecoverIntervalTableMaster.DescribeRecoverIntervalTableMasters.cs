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
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Result;
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

namespace Gs2.Gs2Stamina.Model.Cache
{
    public static partial class RecoverIntervalTableMasterExt
    {
        public static void PutCache(
            this DescribeRecoverIntervalTableMastersResult self,
            CacheDatabase cache,
            string userId,
            DescribeRecoverIntervalTableMastersRequest request
        ) {
            foreach (var item in self.Items ?? Array.Empty<RecoverIntervalTableMaster>())
            {
                item.PutCache(
                    cache,
                    request.NamespaceName,
                    item.Name
                );
            }
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<DescribeRecoverIntervalTableMastersResult> InvokeFuture(
            this DescribeRecoverIntervalTableMastersRequest request,
            CacheDatabase cache,
            string userId,
            Func<IFuture<DescribeRecoverIntervalTableMastersResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<DescribeRecoverIntervalTableMastersResult> self)
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
            return new Gs2InlineFuture<DescribeRecoverIntervalTableMastersResult>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<DescribeRecoverIntervalTableMastersResult> InvokeAsync(
    #else
        public static async Task<DescribeRecoverIntervalTableMastersResult> InvokeAsync(
    #endif
            this DescribeRecoverIntervalTableMastersRequest request,
            CacheDatabase cache,
            string userId,
    #if UNITY_2017_1_OR_NEWER
            Func<UniTask<DescribeRecoverIntervalTableMastersResult>> invokeImpl
    #else
            Func<Task<DescribeRecoverIntervalTableMastersResult>> invokeImpl
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