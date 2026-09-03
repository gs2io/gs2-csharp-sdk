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
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.SpeculativeExecutor
{
    public static class VerifyCompleteByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyCompleteByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyCompleteByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) =
                    ((Gs2.Gs2Mission.Model.Complete)null).GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _request.UserId,
                        _request.MissionGroupName,
                        _accessToken.TimeOffset
                    );
                if (!found || item == null ||
                    item.CompleteId != _expectedId ||
                    item.UserId != _request.UserId ||
                    item.MissionGroupName != _request.MissionGroupName ||
                    item.CompletedMissionTaskNames == null ||
                    item.ReceivedMissionTaskNames == null) {
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
            return "Gs2Mission:VerifyCompleteByUserId";
/* diff +++ start */
        }

        private static VerifyCompleteByUserIdRequest Snapshot(
            VerifyCompleteByUserIdRequest request
        ) {
            return request == null
                ? null
                : new VerifyCompleteByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithUserId(request.UserId)
                    .WithVerifyType(request.VerifyType)
                    .WithMissionTaskName(request.MissionTaskName)
                    .WithMultiplyValueSpecifyingQuantity(
                        request.MultiplyValueSpecifyingQuantity
                    );
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCompleteByUserIdRequest request
        ) {
            var inverse = Snapshot(request);
            switch (inverse?.VerifyType) {
                case "completed":
                    inverse.VerifyType = "notCompleted";
                    break;
                case "notCompleted":
                    inverse.VerifyType = "completed";
                    break;
                case "received":
                    inverse.VerifyType = "notReceived";
                    break;
                case "notReceived":
                    inverse.VerifyType = "received";
                    break;
                default:
                    return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Mission.Model.Complete Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCompleteByUserIdRequest request,
            Gs2.Gs2Mission.Model.Complete item
        ) {
            switch (request?.VerifyType) {
                case "completed":
                case "notCompleted":
                case "received":
                case "notReceived":
                case "completedAndNotReceived":
                    break;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("missionTaskName", "invalid"),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCompleteByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCompleteByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Mission.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Complete(
                request.MissionGroupName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null || accessToken == null ||
                request == null) {
                return null;
            }
            var prepared = Snapshot(request);
            var preparedAccessToken = new AccessToken()
                .WithUserId(accessToken.UserId)
                .WithTimeOffset(accessToken.TimeOffset);
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken.UserId;
            }
            if (string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MissionGroupName) ||
                string.IsNullOrEmpty(prepared.MissionTaskName) ||
                string.IsNullOrEmpty(prepared.UserId) ||
                prepared.UserId != preparedAccessToken.UserId ||
                !IsVerifyType(prepared.VerifyType)) {
                return null;
            }
            var cached = ((Gs2.Gs2Mission.Model.Complete)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.UserId,
                prepared.MissionGroupName,
                preparedAccessToken.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:group:{prepared.MissionGroupName}:complete";
            if (!cached.Item2 || item == null || item.CompleteId != expectedId ||
                item.UserId != prepared.UserId ||
                item.MissionGroupName != prepared.MissionGroupName ||
                item.CompletedMissionTaskNames == null ||
                item.ReceivedMissionTaskNames == null) {
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
        }

        private static bool IsVerifyType(string value) {
            return value == "completed" || value == "notCompleted" ||
                   value == "received" || value == "notReceived" ||
                   value == "completedAndNotReceived";
/* diff +++ end */
        }
    }
}
