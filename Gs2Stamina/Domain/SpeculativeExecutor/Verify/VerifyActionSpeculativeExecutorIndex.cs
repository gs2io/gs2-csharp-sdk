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
using Gs2.Gs2Stamina.Model.Transaction;
using Gs2.Gs2Stamina.Request;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain.SpeculativeExecutor
{
    public static class VerifyActionSpeculativeExecutorIndex
    {
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyAction verifyAction,
            BigInteger rate
        ) {
            verifyAction.Action = verifyAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            verifyAction.Action = verifyAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            verifyAction.Action = verifyAction.Action.Replace("{userId}", accessToken.UserId);
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                if (VerifyStaminaValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                    var request = VerifyStaminaValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = VerifyStaminaValueByUserIdSpeculativeExecutor.ExecuteFuture(
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
                if (VerifyStaminaMaxValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                    var request = VerifyStaminaMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = VerifyStaminaMaxValueByUserIdSpeculativeExecutor.ExecuteFuture(
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
                if (VerifyStaminaRecoverIntervalMinutesByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                    var request = VerifyStaminaRecoverIntervalMinutesByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = VerifyStaminaRecoverIntervalMinutesByUserIdSpeculativeExecutor.ExecuteFuture(
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
                if (VerifyStaminaRecoverValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                    var request = VerifyStaminaRecoverValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = VerifyStaminaRecoverValueByUserIdSpeculativeExecutor.ExecuteFuture(
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
                if (VerifyStaminaOverflowValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                    var request = VerifyStaminaOverflowValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                    if (rate != 1) {
                        request = request.Rate(rate);
                    }
                    var future = VerifyStaminaOverflowValueByUserIdSpeculativeExecutor.ExecuteFuture(
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
            VerifyAction verifyAction,
            BigInteger rate
        ) {
            verifyAction.Action = verifyAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            verifyAction.Action = verifyAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            verifyAction.Action = verifyAction.Action.Replace("{userId}", accessToken.UserId);
            if (VerifyStaminaValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = VerifyStaminaValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await VerifyStaminaValueByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (VerifyStaminaMaxValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = VerifyStaminaMaxValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await VerifyStaminaMaxValueByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (VerifyStaminaRecoverIntervalMinutesByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = VerifyStaminaRecoverIntervalMinutesByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await VerifyStaminaRecoverIntervalMinutesByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (VerifyStaminaRecoverValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = VerifyStaminaRecoverValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await VerifyStaminaRecoverValueByUserIdSpeculativeExecutor.ExecuteAsync(
                    domain,
                    accessToken,
                    request
                );
            }
            if (VerifyStaminaOverflowValueByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = VerifyStaminaOverflowValueByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await VerifyStaminaOverflowValueByUserIdSpeculativeExecutor.ExecuteAsync(
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