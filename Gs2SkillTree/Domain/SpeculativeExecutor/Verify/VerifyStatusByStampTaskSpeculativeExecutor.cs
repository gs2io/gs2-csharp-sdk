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
using Gs2.Gs2SkillTree.Request;
using Gs2.Gs2SkillTree.Model.Cache;
using Gs2.Gs2SkillTree.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
{
    public static class VerifyStatusByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyStatusByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyStatusByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2SkillTree.Model.Status)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.PropertyId,
                        _accessToken.TimeOffset
                    );
                return found && IsUsable(
                           item,
                           _accessToken.UserId,
                           _request.PropertyId,
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
            return "Gs2SkillTree:VerifyStatusByUserId";
/* diff +++ start */
        }

        private static bool IsSupportedVerifyType(string verifyType)
        {
            switch (verifyType) {
                case "released":
                case "notReleased":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsUsable(
            Gs2.Gs2SkillTree.Model.Status item,
            string userId,
            string propertyId,
            string expectedId
        ) {
            return item != null &&
                   item.StatusId == expectedId &&
                   item.UserId == userId &&
                   item.PropertyId == propertyId &&
                   item.ReleasedNodeNames != null;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStatusByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyStatusByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "released": inverse.VerifyType = "notReleased"; break;
                case "notReleased": inverse.VerifyType = "released"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStatusByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyStatusByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.SkillTree.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Status(
                request.PropertyId
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            // キャッシュに正しい行があるときだけ手元で判定し、無ければ null を返してサーバーに任せる。
            // まだステータスが無いユーザーや、マスターに無いノード名はサーバーが判定するので、ここでは判定しない。
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyStatusByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.PropertyId) ||
                string.IsNullOrEmpty(prepared.NodeModelName) ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            // サーバーと同じく、propertyId の {region} {ownerId} {userId} を埋めてからキャッシュを引く。
            prepared.PropertyId = domain.SkillTree.Namespace(
                prepared.NamespaceName
            ).AccessToken(
                token
            ).Status(
                prepared.PropertyId
            ).PropertyId;
            var cached = ((Gs2.Gs2SkillTree.Model.Status)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.PropertyId,
                token.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:skillTree:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:status:{prepared.PropertyId}";
            if (!cached.Item2 ||
                !IsUsable(item, token.UserId, prepared.PropertyId, expectedId)) {
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
