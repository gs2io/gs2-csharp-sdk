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
using System.Linq; /* diff +++ */
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.SpeculativeExecutor
{
    public static class VerifyFollowByUserIdSpeculativeExecutor {
/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyFollowByUserIdRequest _request;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyFollowByUserIdRequest request
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
            }

            public bool IsStillSatisfied()
            {
                var item = LocalFollow(
                    _domain,
                    _request.NamespaceName,
                    _accessToken.UserId,
                    _accessToken.TimeOffset
                );
                return item != null && item.IsExecutable(_request);
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */

        public static string Action() {
            return "Gs2Friend:VerifyFollowByUserId";
/* diff +++ start */
        }

        private static bool IsSupportedVerifyType(string verifyType)
        {
            switch (verifyType) {
                case "include":
                case "notInclude":
                    return true;
                default:
                    return false;
            }
        }

        // 一覧を最後まで読んだときだけ付く「全件そろった」印があるキャッシュから、Follow を組み立てる。
        // 相手の承認や解除の通知を受けると、この印は消える (Gs2Friend.HandleNotification)。
        internal static Gs2.Gs2Friend.Model.Follow LocalFollow(
            Gs2.Core.Domain.Gs2 domain,
            string namespaceName,
            string userId,
            int? timeOffset
        ) {
            foreach (var withProfile in new bool?[] { false, true }) {
                if (domain.Cache.TryGetList<Gs2.Gs2Friend.Model.FollowUser>(
                        (null as Gs2.Gs2Friend.Model.FollowUser).CacheParentKey(
                            namespaceName,
                            userId,
                            withProfile,
                            timeOffset
                        ),
                        out var list
                    ) && list != null) {
                    return new Gs2.Gs2Friend.Model.Follow {
                        UserId = userId,
                        TargetUserIds = list
                            .Where(v => v?.UserId != null)
                            .Select(v => v.UserId)
                            .ToArray(),
                    };
                }
            }
            return null;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFollowByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyFollowByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "include": inverse.VerifyType = "notInclude"; break;
                case "notInclude": inverse.VerifyType = "include"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFollowByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFollowByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            // 全件そろった一覧のキャッシュがあるときだけ手元で判定し、無ければ null を返してサーバーに任せる。
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyFollowByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.TargetUserId) ||
                prepared.TargetUserId.Contains("#{") ||
                !IsSupportedVerifyType(prepared.VerifyType)) {
                return null;
            }
            var item = LocalFollow(
                domain,
                prepared.NamespaceName,
                token.UserId,
                token.TimeOffset
            );
            if (item == null) {
                return null;
            }
            item.SpeculativeExecution(prepared);
            return new PreparedVerification(
                domain,
                token,
                prepared
            ).Invoke;
/* diff +++ end */
        }
    }
}
