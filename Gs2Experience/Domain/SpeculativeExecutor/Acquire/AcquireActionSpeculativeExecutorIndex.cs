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
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

#pragma warning disable 1998

using System;
using System.Collections;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Experience.Request;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Experience.Domain.SpeculativeExecutor
{
    public static class AcquireActionSpeculativeExecutorIndex
    {
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireAction acquireAction,
            BigInteger rate
        ) {
            acquireAction.Action = acquireAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            acquireAction.Action = acquireAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            acquireAction.Action = acquireAction.Action.Replace("{userId}", accessToken.UserId);
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                if (AddExperienceByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = AddExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    request = AddExperienceByUserIdSpeculativeExecutor.Rate(request, rate);
                    var future = AddExperienceByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (AddRankCapByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = AddRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    request = AddRankCapByUserIdSpeculativeExecutor.Rate(request, rate);
                    var future = AddRankCapByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (SetRankCapByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = SetRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    request = SetRankCapByUserIdSpeculativeExecutor.Rate(request, rate);
                    var future = SetRankCapByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                if (MultiplyAcquireActionsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                    var request = MultiplyAcquireActionsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                    request = MultiplyAcquireActionsByUserIdSpeculativeExecutor.Rate(request, rate);
                    var future = MultiplyAcquireActionsByUserIdSpeculativeExecutor.ExecuteFuture(
                        domain,
                        accessToken,
                        request
                    );
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    result.OnComplete(future.Result);
                    yield break;
                }
                result.OnComplete(null);
                yield return null;
            }

            return new Gs2InlineFuture<Func<object>>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Func<object>> ExecuteAsync(
    #else
        public static async Task<Func<object>> ExecuteAsync(
    #endif
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireAction acquireAction,
            BigInteger rate
        ) {
            acquireAction.Action = acquireAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            acquireAction.Action = acquireAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            acquireAction.Action = acquireAction.Action.Replace("{userId}", accessToken.UserId);
            if (AddExperienceByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AddExperienceByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                request = AddExperienceByUserIdSpeculativeExecutor.Rate(request, rate);
                return await AddExperienceByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (AddRankCapByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = AddRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                request = AddRankCapByUserIdSpeculativeExecutor.Rate(request, rate);
                return await AddRankCapByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (SetRankCapByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = SetRankCapByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                request = SetRankCapByUserIdSpeculativeExecutor.Rate(request, rate);
                return await SetRankCapByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (MultiplyAcquireActionsByUserIdSpeculativeExecutor.Action() == acquireAction.Action) {
                var request = MultiplyAcquireActionsByUserIdRequest.FromJson(JsonMapper.ToObject(acquireAction.Request));
                request = MultiplyAcquireActionsByUserIdSpeculativeExecutor.Rate(request, rate);
                return await MultiplyAcquireActionsByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            return null;
        }
#endif
    }
}