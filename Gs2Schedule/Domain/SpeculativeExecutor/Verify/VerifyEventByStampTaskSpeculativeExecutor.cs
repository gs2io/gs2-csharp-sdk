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
using Event = Gs2.Gs2Schedule.Model.Event; /* diff +++ */
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
    public static class VerifyEventByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyEventByUserIdRequest _request;
            private readonly string _expectedEventId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyEventByUserIdRequest request,
                string expectedEventId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedEventId = expectedEventId;
            }

            public bool IsStillSatisfied()
            {
                return TryResolveCachedState(
                           _domain,
                           _accessToken,
                           _request,
                           _expectedEventId,
                           out var item,
                           out var inSchedule
                       ) && item.IsExecutable(_request, inSchedule);
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */

        public static string Action() {
            return "Gs2Schedule:VerifyEventByUserId";
/* diff +++ start */
        }

        private static bool IsSupportedVerifyType(string verifyType)
        {
            return verifyType == "inSchedule" || verifyType == "notInSchedule";
        }

        private static bool IsExpected(
            Event value,
            VerifyEventByUserIdRequest request,
            string expectedEventId
        ) {
            return value != null &&
                   value.EventId == expectedEventId &&
                   value.Name == request.EventName;
        }

        private static bool TryResolveCachedState(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEventByUserIdRequest request,
            string expectedEventId,
            out Event item,
            out bool inSchedule
        ) {
            var active = ((Event)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                accessToken.UserId,
                request.EventName,
                true,
                accessToken.TimeOffset
            );
            if (!active.Item2) {
                item = null;
                inSchedule = false;
                return false;
            }
            if (active.Item1 != null) {
                item = active.Item1;
                inSchedule = true;
                return IsExpected(item, request, expectedEventId);
            }

            var all = ((Event)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                accessToken.UserId,
                request.EventName,
                false,
                accessToken.TimeOffset
            );
            item = all.Item1;
            inSchedule = false;
            return all.Item2 && IsExpected(item, request, expectedEventId);
        }

        private static void ValidateVerifyType(
            VerifyEventByUserIdRequest request
        ) {
            switch (request?.VerifyType) {
                case "inSchedule":
                case "notInSchedule":
                    return;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
        }

        public static Gs2.Gs2Schedule.Model.Event Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEventByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Event item,
            bool? inSchedule
        ) {
            ValidateVerifyType(request);
            if (!item.IsExecutable(request, inSchedule)) {
                var reason = request.VerifyType == "inSchedule"
                    ? "notInSchedule"
                    : "inSchedule";
                throw new BadRequestException(new [] {
                    new RequestError("event", reason),
                });
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
            VerifyEventByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyEventByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "inSchedule": inverse.VerifyType = "notInSchedule"; break;
                case "notInSchedule": inverse.VerifyType = "inSchedule"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEventByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEventByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Schedule.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Event(
                request.EventName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            var prepared = request == null ? null :
                VerifyEventByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            var userId = token.UserId;
            var expectedEventId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId ?? "",
                "schedule",
                prepared.NamespaceName,
                "event",
                prepared.EventName
            );
            if (!TryResolveCachedState(
                    domain,
                    token,
                    prepared,
                    expectedEventId,
                    out var item,
                    out var inSchedule
                )) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
            Transform(domain, token, prepared, item, inSchedule); /* diff +++ */

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
                expectedEventId
            ).Invoke;
/* diff +++ end */
        }
    }
}
