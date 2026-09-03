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
    public static class VerifyGradeUpMaterialByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyGradeUpMaterialByUserIdRequest _request;
            private readonly string _expectedStatusId;
            private readonly string _expectedGradeModelId;
            private readonly string _region;
            private readonly string _ownerId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyGradeUpMaterialByUserIdRequest request,
                string expectedStatusId,
                string expectedGradeModelId,
                string region,
                string ownerId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedStatusId = expectedStatusId;
                _expectedGradeModelId = expectedGradeModelId;
                _region = region;
                _ownerId = ownerId;
            }

            public bool IsStillSatisfied()
            {
                var (item, statusFound) =
                    ((Gs2.Gs2Grade.Model.Status)null).GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.GradeName,
                        _request.PropertyId,
                        _accessToken.TimeOffset
                    );
                if (!statusFound || item == null ||
                    item.StatusId != _expectedStatusId ||
                    item.UserId != _accessToken.UserId ||
                    item.GradeName != _request.GradeName ||
                    item.PropertyId != _request.PropertyId) {
                    return false;
                }

                var (gradeModel, gradeModelFound) =
                    ((Gs2.Gs2Grade.Model.GradeModel)null).GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _request.GradeName,
                        null
                    );
                if (!gradeModelFound || gradeModel == null ||
                    gradeModel.GradeModelId != _expectedGradeModelId ||
                    gradeModel.Name != _request.GradeName) {
                    return false;
                }
                var preparedGradeModel = PrepareGradeModel(
                    gradeModel,
                    _region,
                    _ownerId,
                    _accessToken.UserId
                );
                if (!item.TryEvaluateGradeUpMaterial(
                        _request,
                        preparedGradeModel,
                        out _)) {
                    return false;
                }
                try {
                    Transform(
                        _domain,
                        _accessToken,
                        _request,
                        item,
                        preparedGradeModel
                    );
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
            return "Gs2Grade:VerifyGradeUpMaterialByUserId";
/* diff +++ start */
        }

        private static void ValidateVerifyType(
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            switch (request?.VerifyType) {
                case "match":
                case "notMatch":
                    return;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
        }

        private static string NormalizeContext(
            string value,
            string region,
            string ownerId,
            string userId
        ) {
            return value?
                .Replace("{region}", region ?? "")
                .Replace("{ownerId}", ownerId ?? "")
                .Replace("{userId}", userId ?? "");
        }

        private static Gs2.Gs2Grade.Model.GradeModel PrepareGradeModel(
            Gs2.Gs2Grade.Model.GradeModel gradeModel,
            string region,
            string ownerId,
            string userId
        ) {
            var prepared = Gs2.Gs2Grade.Model.GradeModel.FromJson(
                gradeModel?.ToJson()
            );
            if (prepared?.GradeEntries == null) {
                return prepared;
            }
            foreach (var entry in prepared.GradeEntries) {
                if (entry == null) {
                    continue;
                }
                entry.PropertyIdRegex = NormalizeContext(
                    entry.PropertyIdRegex,
                    region,
                    ownerId,
                    userId
                );
                entry.GradeUpPropertyIdRegex = NormalizeContext(
                    entry.GradeUpPropertyIdRegex,
                    region,
                    ownerId,
                    userId
                );
            }
            return prepared;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyGradeUpMaterialByUserIdRequest.FromJson(
                    request.ToJson()
                );
            switch (inverse?.VerifyType) {
                case "match": inverse.VerifyType = "notMatch"; break;
                case "notMatch": inverse.VerifyType = "match"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Grade.Model.Status Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeUpMaterialByUserIdRequest request,
            Gs2.Gs2Grade.Model.Status item,
            Gs2.Gs2Grade.Model.GradeModel gradeModel
        ) {
            ValidateVerifyType(request);
            if (!item.IsExecutable(request, gradeModel)) {
                throw new BadRequestException(new [] {
                    new RequestError("materialPropertyId", "invalid"),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeUpMaterialByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyGradeUpMaterialByUserIdRequest request
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
            var preparedRequest = VerifyGradeUpMaterialByUserIdRequest.FromJson(
                request?.ToJson()
            );
            var preparedAccessToken = AccessToken.FromJson(
                accessToken?.ToJson()
            );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(preparedRequest.NamespaceName) ||
                string.IsNullOrEmpty(preparedRequest.GradeName) ||
                string.IsNullOrEmpty(preparedRequest.PropertyId) ||
                string.IsNullOrEmpty(preparedRequest.MaterialPropertyId) ||
                (preparedRequest.VerifyType != "match" &&
                 preparedRequest.VerifyType != "notMatch") ||
                domain?.RestSession == null) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId ?? "";
            preparedRequest.PropertyId = NormalizeContext(
                preparedRequest.PropertyId,
                region,
                ownerId,
                preparedAccessToken.UserId
            );
            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "grade",
                preparedRequest.NamespaceName,
                "user",
                preparedAccessToken.UserId,
                "gradeModel",
                preparedRequest.GradeName,
                "property",
                preparedRequest.PropertyId
            );
            var expectedGradeModelId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "grade",
                preparedRequest.NamespaceName,
                "model",
                preparedRequest.GradeName
            );
            var (item, statusFound) = ((Gs2.Gs2Grade.Model.Status)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                preparedAccessToken.UserId,
                preparedRequest.GradeName,
                preparedRequest.PropertyId,
                preparedAccessToken.TimeOffset
            );
            if (!statusFound) {
                return null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (item == null ||
                item.StatusId != expectedStatusId ||
                item.UserId != preparedAccessToken.UserId ||
                item.GradeName != preparedRequest.GradeName ||
                item.PropertyId != preparedRequest.PropertyId) {
                return null;
            }
            var (gradeModel, gradeModelFound) =
                ((Gs2.Gs2Grade.Model.GradeModel)null).GetCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    preparedRequest.GradeName,
                    null
                );
            if (!gradeModelFound) {
                return null;
            }
            if (gradeModel == null ||
                gradeModel.GradeModelId != expectedGradeModelId ||
                gradeModel.Name != preparedRequest.GradeName) {
                return null;
            }
            var preparedGradeModel = PrepareGradeModel(
                gradeModel,
                region,
                ownerId,
                preparedAccessToken.UserId
            );
            if (!item.TryEvaluateGradeUpMaterial(
                    preparedRequest,
                    preparedGradeModel,
                    out _)) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            return () =>
            {
                return null;
            };
 diff --- end */
/* diff +++ start */
            item = Transform(
                domain,
                preparedAccessToken,
                preparedRequest,
                item,
                preparedGradeModel
            );

            return new PreparedVerification(
                domain,
                preparedAccessToken,
                preparedRequest,
                expectedStatusId,
                expectedGradeModelId,
                region,
                ownerId
            ).Invoke;
/* diff +++ end */
        }
    }
}
