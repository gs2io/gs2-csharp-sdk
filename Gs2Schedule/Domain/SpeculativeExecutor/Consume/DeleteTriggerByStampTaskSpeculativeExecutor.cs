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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Model; /* diff +++ */
using Gs2.Gs2Schedule.Model.Cache;
using Gs2.Gs2Schedule.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Schedule.Domain.SpeculativeExecutor
{
    public static class DeleteTriggerByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Schedule:DeleteTriggerByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteTriggerByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteTriggerByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Schedule.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Trigger(
                request.TriggerName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            if (string.IsNullOrEmpty(accessToken?.UserId) || request == null) {
                return null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            var prepared = DeleteTriggerByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = accessToken.UserId;
            }
            if (prepared.UserId != accessToken.UserId || domain == null) {
                return null;
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var expectedTriggerId = string.Join(
                ":", "grn", "gs2", domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId, "schedule", prepared.NamespaceName,
                "user", userId, "trigger", prepared.TriggerName
            );
            bool IsExpected(Trigger value) {
                return value != null &&
                       value.TriggerId == expectedTriggerId &&
                       value.UserId == userId &&
                       value.Name == prepared.TriggerName;
            }
            var cached = ((Trigger)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.TriggerName,
                timeOffset
            );
            if (!cached.Item2 || !IsExpected(cached.Item1)) {
                return null;
            }
            var preparedSnapshot = cached.Item1.ToJson().ToJson();
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var live = ((Trigger)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    accessToken.UserId,
                    request.TriggerName,
                    accessToken.TimeOffset
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    userId,
                    prepared.TriggerName,
                    timeOffset
                );
                if (!live.Item2 || !IsExpected(live.Item1) ||
                    live.Item1.ToJson().ToJson() != preparedSnapshot) {
                    return null;
                }
                ((Trigger)null).PutCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    userId,
                    prepared.TriggerName,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
