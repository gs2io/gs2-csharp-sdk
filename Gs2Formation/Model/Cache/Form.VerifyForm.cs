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
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Result;
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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static partial class FormExt
    {
        public static void PutCache(
            this VerifyFormResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            VerifyFormRequest request
        ) {
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<VerifyFormResult> InvokeFuture(
            this VerifyFormRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<VerifyFormResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<VerifyFormResult> self)
            {
                var future = invokeImpl();
                yield return future;
                if (future.Error != null) {
                    if (future.Error is Gs2.Core.Exception.Gs2Exception failed && failed.Result != null) {
                        VerifyFormResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(failed.Result))?.PutCache(
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
            return new Gs2InlineFuture<VerifyFormResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<VerifyFormResult> InvokeAsync(
#else
        public static async Task<VerifyFormResult> InvokeAsync(
#endif
            this VerifyFormRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<VerifyFormResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<VerifyFormResult>> invokeImpl
#else
            Func<Task<VerifyFormResult>> invokeImpl
#endif
        )
        {
            VerifyFormResult result;
            try {
                result = await invokeImpl();
            }
            catch (Gs2.Core.Exception.Gs2Exception e) when (e.Result != null) {
                VerifyFormResult.FromJson(Gs2.Util.LitJson.JsonMapper.ToObject(e.Result))?.PutCache(
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