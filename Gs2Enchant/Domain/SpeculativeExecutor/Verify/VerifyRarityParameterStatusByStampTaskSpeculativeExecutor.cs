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
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Enchant.Model;
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Model.Cache;
using Gs2.Gs2Enchant.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enchant.Domain.SpeculativeExecutor
{
    public static class VerifyRarityParameterStatusByUserIdSpeculativeExecutor {
        private static bool HasParameterValues(
            RarityParameterStatus item
        ) {
            return item?.ParameterValues != null;
        }

        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyRarityParameterStatusByUserIdRequest _request;
            private readonly string _expectedStatusId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyRarityParameterStatusByUserIdRequest request,
                string expectedStatusId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedStatusId = expectedStatusId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((RarityParameterStatus)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _accessToken.UserId,
                    _request.ParameterName,
                    _request.PropertyId,
                    _accessToken.TimeOffset
                );
                if (!found || !HasParameterValues(item) ||
                    item.RarityParameterStatusId != _expectedStatusId ||
                    item.UserId != _accessToken.UserId ||
                    item.ParameterName != _request.ParameterName ||
                    item.PropertyId != _request.PropertyId) {
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

        public static string Action() {
            return "Gs2Enchant:VerifyRarityParameterStatusByUserId";
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyRarityParameterStatusByUserIdRequest.FromJson(
                    request.ToJson()
                );
            switch (inverse?.VerifyType) {
                case "have":
                    inverse.VerifyType = "havent";
                    break;
                case "havent":
                    inverse.VerifyType = "have";
                    break;
                default:
                    return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Enchant.Model.RarityParameterStatus Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyRarityParameterStatusByUserIdRequest request,
            Gs2.Gs2Enchant.Model.RarityParameterStatus item
        ) {
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            return item;
        }

        private static bool IsValidRequest(
            VerifyRarityParameterStatusByUserIdRequest request,
            AccessToken accessToken
        ) {
            if (request == null || accessToken == null ||
                string.IsNullOrEmpty(request.NamespaceName) ||
                string.IsNullOrEmpty(request.ParameterName) ||
                string.IsNullOrEmpty(request.UserId) ||
                request.UserId != accessToken.UserId ||
                string.IsNullOrEmpty(request.PropertyId) ||
                (request.VerifyType != "havent" &&
                 request.VerifyType != "have" &&
                 request.VerifyType != "count")) {
                return false;
            }
            if (request.VerifyType == "have" ||
                request.VerifyType == "havent") {
                return !string.IsNullOrEmpty(request.ParameterValueName);
            }
            return request.ParameterCount.HasValue;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyRarityParameterStatusByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyRarityParameterStatusByUserIdRequest request
        ) {
            var preparedRequest = VerifyRarityParameterStatusByUserIdRequest
                .FromJson(request?.ToJson());
            var preparedAccessToken = AccessToken.FromJson(
                accessToken?.ToJson()
            );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (preparedRequest != null &&
                preparedRequest.MultiplyValueSpecifyingQuantity == null) {
                preparedRequest.MultiplyValueSpecifyingQuantity = false;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                string.IsNullOrEmpty(preparedRequest?.PropertyId)) {
                return null;
            }
            var statusDomain = domain.Enchant.Namespace(
                preparedRequest.NamespaceName
            ).AccessToken(
                preparedAccessToken
            ).RarityParameterStatus(
                preparedRequest.ParameterName,
                preparedRequest.PropertyId
            );
            preparedRequest.PropertyId = statusDomain.PropertyId;
            if (!IsValidRequest(preparedRequest, preparedAccessToken)) {
                return null;
            }
            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "enchant",
                preparedRequest.NamespaceName,
                "user",
                preparedAccessToken.UserId,
                "rarity",
                preparedRequest.ParameterName,
                preparedRequest.PropertyId
            );
            var cached = ((Gs2.Gs2Enchant.Model.RarityParameterStatus)null)
                .GetCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    preparedAccessToken.UserId,
                    preparedRequest.ParameterName,
                    preparedRequest.PropertyId,
                    preparedAccessToken.TimeOffset
                );
            var item = cached.Item1;
            if (!cached.Item2 || !HasParameterValues(item) ||
                item.RarityParameterStatusId != expectedStatusId ||
                item.UserId != preparedAccessToken.UserId ||
                item.ParameterName != preparedRequest.ParameterName ||
                item.PropertyId != preparedRequest.PropertyId) {
                return null;
            }

            Transform(
                domain,
                preparedAccessToken,
                preparedRequest,
                item
            );

            return new PreparedVerification(
                domain,
                preparedAccessToken,
                preparedRequest,
                expectedStatusId
            ).Invoke;
        }

        public static VerifyRarityParameterStatusByUserIdRequest Rate(
            VerifyRarityParameterStatusByUserIdRequest request,
            double rate
        ) {
            return request;
        }

        public static VerifyRarityParameterStatusByUserIdRequest Rate(
            VerifyRarityParameterStatusByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}
