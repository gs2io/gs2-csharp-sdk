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
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2LoginReward.Model; /* diff +++ */
using Gs2.Gs2LoginReward.Request;
using Gs2.Gs2LoginReward.Model.Cache;
using Gs2.Gs2LoginReward.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2LoginReward.Domain.SpeculativeExecutor
{
    public static class MarkReceivedByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2LoginReward:MarkReceivedByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReceivedByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReceivedByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.LoginReward.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).ReceiveStatus(
                request.BonusModelName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                MarkReceivedByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = accessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    accessToken.UserId,
                    request.BonusModelName,
                    accessToken.TimeOffset
                );
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                prepared?.UserId != accessToken.UserId ||
                prepared.StepNumber == null || prepared.StepNumber < 0) {
/* diff +++ end */
                return null;
/* diff --- start
            };
 diff --- end */
/* diff +++ start */
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var currentTimeMillis = UnixTime.ToUnixTime(DateTime.Now) +
                                    (long)(timeOffset ?? 0) * 1000L;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:loginReward:{prepared.NamespaceName}:" +
                $"user:{userId}:status:{prepared.BonusModelName}";
            var commit = new ReceiveStatusSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.BonusModelName,
                timeOffset,
                expectedId,
                item => item.SpeculativeExecutionAt(
                    prepared,
                    currentTimeMillis
                )
            );
            return commit.CanPrepare() ? commit.Invoke : null;
/* diff +++ end */
        }
    }
}
