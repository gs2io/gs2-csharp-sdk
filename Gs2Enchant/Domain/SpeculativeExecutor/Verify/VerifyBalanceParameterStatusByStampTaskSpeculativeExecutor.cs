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
    public static class VerifyBalanceParameterStatusByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyBalanceParameterStatusByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyBalanceParameterStatusByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2Enchant.Model.BalanceParameterStatus)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.ParameterName,
                        _request.PropertyId,
                        _accessToken.TimeOffset
                    );
                return found && IsUsable(
                           item,
                           _accessToken.UserId,
                           _request,
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
            return "Gs2Enchant:VerifyBalanceParameterStatusByUserId";
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

        // 行に指定のパラメータ名が無いときは使えないものとする。サーバーは NotFound にするため。
        private static bool IsUsable(
            Gs2.Gs2Enchant.Model.BalanceParameterStatus item,
            string userId,
            VerifyBalanceParameterStatusByUserIdRequest request,
            string expectedId
        ) {
            return item != null &&
                   item.BalanceParameterStatusId == expectedId &&
                   item.UserId == userId &&
                   item.ParameterName == request.ParameterName &&
                   item.PropertyId == request.PropertyId &&
                   Gs2.Gs2Enchant.Model.Transaction.BalanceParameterStatusExt.BalanceParameterValueOf(
                       item, request.ParameterValueName
                   ) != null;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBalanceParameterStatusByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyBalanceParameterStatusByUserIdRequest.FromJson(request.ToJson());
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
            VerifyBalanceParameterStatusByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBalanceParameterStatusByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Enchant.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).BalanceParameterStatus(
                request.ParameterName,
                request.PropertyId
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            // キャッシュに正しい行があるときだけ手元で判定し、無ければ null を返してサーバーに任せる。
            // まだステータスが無いユーザーはサーバーが初期値で作って判定するので、ここでは判定しない。
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyBalanceParameterStatusByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.ParameterName) ||
                string.IsNullOrEmpty(prepared.PropertyId) ||
                string.IsNullOrEmpty(prepared.ParameterValueName) ||
                !prepared.ParameterValue.HasValue ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            // サーバーと同じく、propertyId の {region} {ownerId} {userId} を埋めてからキャッシュを引く。
            prepared.PropertyId = domain.Enchant.Namespace(
                prepared.NamespaceName
            ).AccessToken(
                token
            ).BalanceParameterStatus(
                prepared.ParameterName,
                prepared.PropertyId
            ).PropertyId;
            var cached = ((Gs2.Gs2Enchant.Model.BalanceParameterStatus)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.ParameterName,
                prepared.PropertyId,
                token.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:enchant:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:balance:{prepared.ParameterName}:{prepared.PropertyId}";
            if (!cached.Item2 ||
                !IsUsable(item, token.UserId, prepared, expectedId)) {
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
