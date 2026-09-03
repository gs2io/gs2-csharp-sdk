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
using Gs2.Gs2Schedule.Model; /* diff +++ */
using Gs2.Gs2Schedule.Request;
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
    public static class VerifyTriggerByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyTriggerByUserIdRequest _request;
            private readonly string _expectedTriggerId;
            private readonly bool _inverse;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyTriggerByUserIdRequest request,
                string expectedTriggerId,
                bool inverse
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedTriggerId = expectedTriggerId;
                _inverse = inverse;
            }

            public bool IsStillSatisfied()
            {
                return TryEvaluate(
                           _domain,
                           _accessToken,
                           _request,
                           _expectedTriggerId,
                           out var satisfied
                       ) && satisfied != _inverse;
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */

        public static string Action() {
            return "Gs2Schedule:VerifyTriggerByUserId";
/* diff +++ start */
        }

        private static bool CanEvaluate(VerifyTriggerByUserIdRequest request)
        {
            return request?.VerifyType == "notTriggerd" ||
                   (request?.VerifyType == "elapsed" ||
                    request?.VerifyType == "notElapsed") &&
                   request.ElapsedMinutes.HasValue;
        }

        private static long CurrentTimeMillis(AccessToken accessToken)
        {
            return UnixTime.ToUnixTime(DateTime.Now) +
                   (long)(accessToken?.TimeOffset ?? 0) * 1000L;
        }

        private static bool IsExpected(
            Trigger item,
            VerifyTriggerByUserIdRequest request,
            string userId,
            string expectedTriggerId
        ) {
            return item != null &&
                   item.TriggerId == expectedTriggerId &&
                   item.UserId == userId &&
                   item.Name == request.TriggerName;
        }

        private static bool TryEvaluate(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request,
            string expectedTriggerId,
            out bool satisfied
        ) {
            var cached = ((Trigger)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                accessToken.UserId,
                request.TriggerName,
                accessToken.TimeOffset
            );
            if (!cached.Item2) {
                satisfied = false;
                return false;
            }
            var item = cached.Item1;
            if (request.VerifyType == "notTriggerd" && item == null) {
                satisfied = true;
                return true;
            }
            if (!IsExpected(
                    item,
                    request,
                    accessToken.UserId,
                    expectedTriggerId
                )) {
                satisfied = false;
                return false;
            }
            if ((request.VerifyType == "notTriggerd" && !item.ExpiresAt.HasValue) ||
                (request.VerifyType != "notTriggerd" && !item.CreatedAt.HasValue)) {
                satisfied = false;
                return false;
            }
            satisfied = item.IsExecutable(
                request,
                CurrentTimeMillis(accessToken)
            );
            return true;
        }

        private static void ThrowUnsatisfied(
            VerifyTriggerByUserIdRequest request
        ) {
            var reason = request.VerifyType == "notTriggerd"
                ? "triggered"
                : request.VerifyType == "elapsed"
                    ? "notElapsed"
                    : "elapsed";
            throw new BadRequestException(new [] {
                new RequestError("trigger", reason),
            });
        }

        public static Gs2.Gs2Schedule.Model.Trigger Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Trigger item
        ) {
            return Transform(
                domain,
                accessToken,
                request,
                item,
                CurrentTimeMillis(accessToken)
            );
        }

        public static Gs2.Gs2Schedule.Model.Trigger Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Trigger item,
            long currentTimeMillis
        ) {
            if (!CanEvaluate(request)) {
                return item;
            }
            if (!item.IsExecutable(request, currentTimeMillis)) {
                ThrowUnsatisfied(request);
            }
            return item;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request
        ) {
            return await ExecuteInternalAsync(domain, accessToken, request, true);
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Schedule.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Trigger(
                request.TriggerName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            return await ExecuteInternalAsync(
                domain,
                accessToken,
                request,
                false
            );
        }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
#if GS2_ENABLE_UNITASK
        private static async UniTask<Func<object>> ExecuteInternalAsync(
#else
        private static async Task<Func<object>> ExecuteInternalAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyTriggerByUserIdRequest request,
            bool inverse
        ) {
            var token = accessToken?.Clone() as AccessToken;
            var prepared = request == null ? null :
                VerifyTriggerByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                !CanEvaluate(prepared)) {
                return null;
            }
            var userId = token.UserId;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var expectedTriggerId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "schedule",
                prepared.NamespaceName,
                "user",
                userId,
                "trigger",
                prepared.TriggerName
            );
            if (!TryEvaluate(
                    domain,
                    token,
                    prepared,
                    expectedTriggerId,
                    out var satisfied
                )) {
                return null;
            }
            if (satisfied == inverse) {
                ThrowUnsatisfied(prepared);
            }
/* diff +++ end */

/* diff --- start
            return () =>
            {
                return null;
            };
 diff --- end */
/* diff +++ start */
            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedTriggerId,
                inverse
            ).Invoke;
/* diff +++ end */
        }
    }
}
