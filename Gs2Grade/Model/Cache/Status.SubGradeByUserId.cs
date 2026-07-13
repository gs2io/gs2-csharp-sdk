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
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Result;
using Gs2.Gs2Experience.Model.Cache;
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

namespace Gs2.Gs2Grade.Model.Cache
{
    public static partial class StatusExt
    {
        public static void PutCache(
            this SubGradeByUserIdResult self,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            SubGradeByUserIdRequest request
        ) {
            self.Item?.PutCache(
                cache,
                request.NamespaceName,
                self?.Item?.UserId,
/* diff --- start
                self.Item.GradeName,
 diff --- end */
                request.GradeName, /* diff +++ */
                self.Item.PropertyId,
                timeOffset
            );
            self.ExperienceStatus?.PutCache(
                cache,
/* diff --- start
                Gs2.Gs2Experience.Model.Status.GetNamespaceNameFromGrn(self.ExperienceStatus?.StatusId),
 diff --- end */
                self.ExperienceNamespaceName, /* diff +++ */
                self?.Item?.UserId,
/* diff --- start
                self.ExperienceStatus.ExperienceName,
                self.Item.PropertyId,
 diff --- end */
/* diff +++ start */
                self.ExperienceStatus?.ExperienceName,
                self.ExperienceStatus?.PropertyId,
/* diff +++ end */
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static IFuture<SubGradeByUserIdResult> InvokeFuture(
            this SubGradeByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
            Func<IFuture<SubGradeByUserIdResult>> invokeImpl
        )
        {
            IEnumerator Impl(IFuture<SubGradeByUserIdResult> self)
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
            return new Gs2InlineFuture<SubGradeByUserIdResult>(Impl);
        }
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<SubGradeByUserIdResult> InvokeAsync(
#else
        public static async Task<SubGradeByUserIdResult> InvokeAsync(
#endif
            this SubGradeByUserIdRequest request,
            CacheDatabase cache,
            string userId,
            int? timeOffset,
#if GS2_ENABLE_UNITASK
            Func<UniTask<SubGradeByUserIdResult>> invokeImpl
#elif UNITY_2017_1_OR_NEWER
            Func<TaskFuture<SubGradeByUserIdResult>> invokeImpl
#else
            Func<Task<SubGradeByUserIdResult>> invokeImpl
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