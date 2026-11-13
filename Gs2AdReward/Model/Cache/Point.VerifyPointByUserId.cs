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
using Gs2.Gs2AdReward.Request;
using Gs2.Gs2AdReward.Result;
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

namespace Gs2.Gs2AdReward.Model.Cache
{
    public static partial class PointExt
    {
        public static void PutCache(
            this VerifyPointByUserIdResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            VerifyPointByUserIdRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.NamespaceName,
                self?.Item?.UserId,
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<VerifyPointByUserIdResult> InvokeFuture(
            this VerifyPointByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<VerifyPointByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<VerifyPointByUserIdResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    if (future.Error is Gs2.Core.Exception.Gs2Exception failed && failed.Result != null) {
                        VerifyPointByUserIdResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(failed.Result))?.PutCache(
                            cache,
                            userId,
                            timeOffset,
                            request
                        );
                    }
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
            return new Gs2InlineFuture<VerifyPointByUserIdResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<VerifyPointByUserIdResult> InvokeAsync(
#else
        public static async Task<VerifyPointByUserIdResult> InvokeAsync(
#endif
            this VerifyPointByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<VerifyPointByUserIdResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<VerifyPointByUserIdResult>> invokeImpl
#else
            Func<Task<VerifyPointByUserIdResult>> invokeImpl
#endif
        )
        {
            VerifyPointByUserIdResult result;
            try {
                result = await invokeImpl();
            }
            catch (Gs2.Core.Exception.Gs2Exception e) when (e.Result != null) {
                VerifyPointByUserIdResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(e.Result))?.PutCache(
                    cache,
                    userId,
                    timeOffset,
                    request
                );
                throw;
            }
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