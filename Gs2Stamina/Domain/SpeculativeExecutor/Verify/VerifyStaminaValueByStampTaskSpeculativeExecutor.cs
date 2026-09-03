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
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Model.Cache;
using Gs2.Gs2Stamina.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain.SpeculativeExecutor
{
    public static class VerifyStaminaValueByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyStaminaValueByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyStaminaValueByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2Stamina.Model.Stamina)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.StaminaName,
                        _accessToken.TimeOffset
                    );
                return found && IsUsable(
                           item,
                           _request,
                           _accessToken.UserId,
                           _expectedId
                       ) && item.IsExecutable(_request);
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */

        public static string Action() {
            return "Gs2Stamina:VerifyStaminaValueByUserId";
/* diff +++ start */
        }

        private static bool IsSupportedVerifyType(string verifyType)
        {
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

        private static bool IsUsable(
            Gs2.Gs2Stamina.Model.Stamina item,
            VerifyStaminaValueByUserIdRequest request,
            string userId,
            string expectedId
        ) {
            return item != null &&
                   item.StaminaId == expectedId &&
                   item.UserId == userId &&
                   item.StaminaName == request.StaminaName &&
                   item.Value.HasValue;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStaminaValueByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyStaminaValueByUserIdRequest.FromJson(request.ToJson());
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
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStaminaValueByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStaminaValueByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Stamina.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Stamina(
                request.StaminaName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyStaminaValueByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                !prepared.Value.HasValue ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            var cached = ((Gs2.Gs2Stamina.Model.Stamina)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.StaminaName,
                token.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:stamina:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:stamina:{prepared.StaminaName}";
            if (!cached.Item2 ||
                !IsUsable(item, prepared, token.UserId, expectedId)) {
                return null;
            }
            item.SpeculativeExecution(prepared);
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedId
            ).Invoke;
        }
/* diff +++ end */

/* diff --- start
            return () =>
            {
                return null;
            };
        }
 diff --- end */
    }
}
