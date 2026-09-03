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
    public static class DeleteReceiveStatusByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2LoginReward:DeleteReceiveStatusByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteReceiveStatusByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteReceiveStatusByUserIdRequest request
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
                DeleteReceiveStatusByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = accessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                prepared?.UserId != accessToken.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.BonusModelName)) {
                return null;
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var cached = ((ReceiveStatus)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.BonusModelName,
                timeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:loginReward:{prepared.NamespaceName}:" +
                $"user:{userId}:status:{prepared.BonusModelName}";
            if (!cached.Item2 || !item.IsExecutable(prepared) ||
                item.ReceiveStatusId != expectedId) {
                return null;
            }
            var expected = item.Clone() as ReceiveStatus;
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var current = ((ReceiveStatus)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.BonusModelName,
                    null
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    userId,
                    prepared.BonusModelName,
                    timeOffset
                );
                if (!current.Item2 || current.Item1 == null ||
                    current.Item1.ReceiveStatusId != expectedId ||
                    (current.Item1.Revision > 0 &&
                     current.Item1.ToJson().ToJson() !=
                     expected.ToJson().ToJson())) {
                    return null;
                }
                ((ReceiveStatus)null).PutCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    userId,
                    prepared.BonusModelName,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
