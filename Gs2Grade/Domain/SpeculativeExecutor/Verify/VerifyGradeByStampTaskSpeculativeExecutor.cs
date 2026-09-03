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
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Model.Cache;
using Gs2.Gs2Grade.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Grade.Domain.SpeculativeExecutor
{
    public static class VerifyGradeByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyGradeByUserIdRequest _request;
            private readonly string _expectedStatusId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyGradeByUserIdRequest request,
                string expectedStatusId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedStatusId = expectedStatusId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2Grade.Model.Status)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.GradeName,
                        _request.PropertyId,
                        _accessToken.TimeOffset
                    );
                if (!found || item == null ||
                    item.StatusId != _expectedStatusId ||
                    item.UserId != _accessToken.UserId ||
                    item.GradeName != _request.GradeName ||
                    item.PropertyId != _request.PropertyId ||
                    !item.GradeValue.HasValue) {
                    return false;
                }
                try {
                    Transform(_domain, _accessToken, _request, item);
                    return true;
                }
                catch (Gs2Exception) {
                    return false;
                }
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */

        public static string Action() {
            return "Gs2Grade:VerifyGradeByUserId";
/* diff +++ start */
        }

        private static void ValidateVerifyType(
            VerifyGradeByUserIdRequest request
        ) {
            switch (request?.VerifyType) {
                case "less":
                case "lessEqual":
                case "greater":
                case "greaterEqual":
                case "equal":
                case "notEqual":
                    return;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
        }

        private static bool IsSupportedVerifyType(string verifyType) {
            switch (verifyType) {
                case "less":
                case "lessEqual":
                case "greater":
                case "greaterEqual":
                case "equal":
                case "notEqual":
                    return true;
                default:
                    return false;
            }
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyGradeByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "less": inverse.VerifyType = "greaterEqual"; break;
                case "lessEqual": inverse.VerifyType = "greater"; break;
                case "greater": inverse.VerifyType = "lessEqual"; break;
                case "greaterEqual": inverse.VerifyType = "less"; break;
                case "equal": inverse.VerifyType = "notEqual"; break;
                case "notEqual": inverse.VerifyType = "equal"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Grade.Model.Status Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeByUserIdRequest request,
            Gs2.Gs2Grade.Model.Status item
        ) {
            ValidateVerifyType(request);
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("gradeValue", "invalid"),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Grade.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Status(
                request.GradeName,
                request.PropertyId
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                VerifyGradeByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                prepared?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.GradeName) ||
                string.IsNullOrEmpty(prepared.PropertyId) ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId ?? "";
            var propertyId = prepared.PropertyId
                .Replace("{region}", region)
                .Replace("{ownerId}", ownerId)
                .Replace("{userId}", userId);
            prepared.PropertyId = propertyId;
            if (string.IsNullOrEmpty(propertyId)) {
                return null;
            }
            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "grade",
                prepared.NamespaceName,
                "user",
                userId,
                "gradeModel",
                prepared.GradeName,
                "property",
                propertyId
            );
            var (item, found) = ((Gs2.Gs2Grade.Model.Status)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.GradeName,
                propertyId,
                preparedAccessToken.TimeOffset
            );
            if (!found || item == null ||
                item.StatusId != expectedStatusId ||
                item.UserId != userId ||
                item.GradeName != prepared.GradeName ||
                item.PropertyId != propertyId ||
                !item.GradeValue.HasValue) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
            Transform(domain, preparedAccessToken, prepared, item); /* diff +++ */

/* diff --- start
            return () =>
            {
                return null;
            };
 diff --- end */
/* diff +++ start */
            return new PreparedVerification(
                domain,
                preparedAccessToken,
                prepared,
                expectedStatusId
            ).Invoke;
/* diff +++ end */
        }
    }
}
