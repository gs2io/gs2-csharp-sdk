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
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Distributor.Model.Transaction;
using Gs2.Gs2Distributor.Request;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Domain.SpeculativeExecutor
{
    public static class VerifyActionSpeculativeExecutorIndex
    {
/* diff +++ start */
#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyAction verifyAction,
            BigInteger rate
        ) {
            verifyAction.Action = verifyAction.Action.Replace("{region}", domain.RestSession.Region.DisplayName());
            verifyAction.Action = verifyAction.Action.Replace("{ownerId}", domain.RestSession.OwnerId);
            verifyAction.Action = verifyAction.Action.Replace("{userId}", accessToken.UserId);
            if (AndExpressionByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = AndExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                return await AndExpressionByUserIdSpeculativeExecutor.ExecuteInverseRatedAsync(
                    domain,
                    accessToken,
                    request,
                    rate
                );
            }
            if (OrExpressionByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = OrExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
                return await OrExpressionByUserIdSpeculativeExecutor.ExecuteInverseRatedAsync(
                    domain,
                    accessToken,
                    request,
                    rate
                );
            }
            return null;
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyAction verifyAction,
            BigInteger rate
        ) => ExecuteAsync(domain, accessToken, verifyAction, rate).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
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
            if (IfExpressionByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = IfExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
/* diff --- start
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await IfExpressionByUserIdSpeculativeExecutor.ExecuteAsync(
 diff --- end */
                return await IfExpressionByUserIdSpeculativeExecutor.ExecuteRatedAsync( /* diff +++ */
                    domain,
                    accessToken,
/* diff --- start
                    request
 diff --- end */
/* diff +++ start */
                    request,
                    rate
/* diff +++ end */
                );
            }
            if (AndExpressionByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = AndExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
/* diff --- start
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await AndExpressionByUserIdSpeculativeExecutor.ExecuteAsync(
 diff --- end */
                return await AndExpressionByUserIdSpeculativeExecutor.ExecuteRatedAsync( /* diff +++ */
                    domain,
                    accessToken,
/* diff --- start
                    request
 diff --- end */
/* diff +++ start */
                    request,
                    rate
/* diff +++ end */
                );
            }
            if (OrExpressionByUserIdSpeculativeExecutor.Action() == verifyAction.Action) {
                var request = OrExpressionByUserIdRequest.FromJson(JsonMapper.ToObject(verifyAction.Request));
/* diff --- start
                if (rate != 1) {
                    request = request.Rate(rate);
                }
                return await OrExpressionByUserIdSpeculativeExecutor.ExecuteAsync(
 diff --- end */
                return await OrExpressionByUserIdSpeculativeExecutor.ExecuteRatedAsync( /* diff +++ */
                    domain,
                    accessToken,
/* diff --- start
                    request
 diff --- end */
/* diff +++ start */
                    request,
                    rate
/* diff +++ end */
                );
            }
            return null;
        }
    }
}