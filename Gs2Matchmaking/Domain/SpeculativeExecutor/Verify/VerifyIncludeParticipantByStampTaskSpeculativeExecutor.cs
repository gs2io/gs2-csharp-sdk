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
using Gs2.Gs2Matchmaking.Model; /* diff +++ */
using Gs2.Gs2Matchmaking.Request;
using Gs2.Gs2Matchmaking.Model.Cache;
using Gs2.Gs2Matchmaking.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Matchmaking.Domain.SpeculativeExecutor
{
    public static class VerifyIncludeParticipantByUserIdSpeculativeExecutor {

/* diff +++ start */
        private static bool HasParticipants(SeasonGathering item)
        {
            return item?.Participants != null;
        }

        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyIncludeParticipantByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyIncludeParticipantByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((SeasonGathering)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _accessToken.UserId,
                    _request.SeasonName,
                    _request.Season,
                    _request.Tier,
                    _request.SeasonGatheringName,
                    _accessToken.TimeOffset
                );
                if (!found || !HasParticipants(item) ||
                    item.SeasonGatheringId != _expectedId ||
                    item.SeasonName != _request.SeasonName ||
                    item.Season != _request.Season ||
                    item.Tier != _request.Tier ||
                    item.Name != _request.SeasonGatheringName) {
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
            return "Gs2Matchmaking:VerifyIncludeParticipantByUserId";
/* diff +++ start */
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeParticipantByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyIncludeParticipantByUserIdRequest.FromJson(
                    request.ToJson()
                );
            switch (inverse?.VerifyType) {
                case "include":
                    inverse.VerifyType = "notInclude";
                    break;
                case "notInclude":
                    inverse.VerifyType = "include";
                    break;
                default:
                    return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        private static void ValidateVerifyType(
            VerifyIncludeParticipantByUserIdRequest request
        ) {
            switch (request?.VerifyType) {
                case "include":
                case "notInclude":
                    return;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
        }

        public static Gs2.Gs2Matchmaking.Model.SeasonGathering Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeParticipantByUserIdRequest request,
            Gs2.Gs2Matchmaking.Model.SeasonGathering item
        ) {
            ValidateVerifyType(request);
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("userId", "invalid"),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeParticipantByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeParticipantByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Matchmaking.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Season(
                request.SeasonName,
                request.Season
            ).SeasonGathering(
                request.Tier,
                request.SeasonGatheringName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                VerifyIncludeParticipantByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.VerifyType != "include" &&
                prepared?.VerifyType != "notInclude") {
                return null;
            }
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.SeasonName) ||
                prepared.Season == null || prepared.Tier == null ||
                string.IsNullOrEmpty(prepared.SeasonGatheringName) ||
                string.IsNullOrEmpty(prepared.UserId)) {
                return null;
            }
            var timeOffset = preparedAccessToken.TimeOffset;
            var expectedId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId ?? "",
                "matchmaking",
                prepared.NamespaceName,
                "season",
                prepared.SeasonName,
                prepared.Season,
                prepared.Tier,
                "gathering",
                prepared.SeasonGatheringName
            );
            var cached = ((SeasonGathering)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                preparedAccessToken.UserId,
                prepared.SeasonName,
                prepared.Season,
                prepared.Tier,
                prepared.SeasonGatheringName,
                timeOffset
            );
            var item = cached.Item1;
            if (!cached.Item2 || !HasParticipants(item) ||
                item.SeasonGatheringId != expectedId ||
                item.SeasonName != prepared.SeasonName ||
                item.Season != prepared.Season ||
                item.Tier != prepared.Tier ||
                item.Name != prepared.SeasonGatheringName) {
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
                expectedId
            ).Invoke;
/* diff +++ end */
        }
    }
}
