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
using System.Collections.Generic; /* diff +++ */
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Model.Cache;
using Gs2.Gs2Distributor.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Distributor.Domain.SpeculativeExecutor
{
    public static class OrExpressionByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Distributor:OrExpressionByUserId";
        }

/* diff +++ start */
#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> ExecuteInverseRatedAsync(
#else
        internal static async Task<Func<object>> ExecuteInverseRatedAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OrExpressionByUserIdRequest request,
            BigInteger rate
        ) {
            var preparedRequest = request == null ? null :
                OrExpressionByUserIdRequest.FromJson(request.ToJson());
            if (request?.Actions?.Length == 0 && preparedRequest != null) {
                preparedRequest.Actions = Array.Empty<Gs2.Core.Model.VerifyAction>();
            }
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (preparedRequest?.Actions == null ||
                preparedRequest.UserId != preparedAccessToken?.UserId ||
                preparedRequest.Actions.Length == 0) {
                return null;
            }

            var commits = new List<Func<object>>();
            var hasUnknown = false;
            foreach (var action in preparedRequest.Actions) {
                var commit = await Gs2.Core.SpeculativeExecutor
                    .VerifyActionSpeculativeExecutor.ExecuteInverseAsync(
                        domain,
                        preparedAccessToken,
                        action,
                        rate
                    );
                if (commit == null) {
                    hasUnknown = true;
                    continue;
                }
                commits.Add(commit);
            }
            if (hasUnknown) return null;
            return Gs2.Core.SpeculativeExecutor.SpeculativeExecutor
                .BuildAtomicVerificationCommit(
                    commits,
                    preparedRequest.Actions.Length
                );
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OrExpressionByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
/* diff --- start
        public static async UniTask<Func<object>> ExecuteAsync(
 diff --- end */
        public static UniTask<Func<object>> ExecuteAsync( /* diff +++ */
#else
/* diff --- start
        public static async Task<Func<object>> ExecuteAsync(
 diff --- end */
        public static Task<Func<object>> ExecuteAsync( /* diff +++ */
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OrExpressionByUserIdRequest request
/* diff +++ start */
        ) => ExecuteRatedAsync(domain, accessToken, request, BigInteger.One);

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> ExecuteRatedAsync(
#else
        internal static async Task<Func<object>> ExecuteRatedAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OrExpressionByUserIdRequest request,
            BigInteger rate
/* diff +++ end */
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null ? null :
                OrExpressionByUserIdRequest.FromJson(request.ToJson());
            if (request?.Actions?.Length == 0 && preparedRequest != null) {
                preparedRequest.Actions = Array.Empty<Gs2.Core.Model.VerifyAction>();
            }
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (preparedRequest?.Actions == null ||
                preparedRequest.UserId != preparedAccessToken?.UserId) {
                return null;
            }

            Gs2Exception lastError = null;
            var hasUnknown = false;
            foreach (var action in preparedRequest.Actions) {
                try {
                    var commit = await Gs2.Core.SpeculativeExecutor
                        .VerifyActionSpeculativeExecutor.ExecuteAsync(
                            domain,
                            preparedAccessToken,
                            action,
                            rate
                        );
                    if (commit != null) {
                        return commit;
                    }
                    hasUnknown = true;
                }
                catch (Gs2Exception e) {
                    lastError = e;
                }
            }
            if (hasUnknown) {
                return null;
            }
            if (lastError != null) {
                throw lastError;
            }
            return Gs2.Core.SpeculativeExecutor.SpeculativeExecutor
                .BuildAtomicVerificationCommit(
                    Array.Empty<Func<object>>(),
                    0
                );
/* diff +++ end */
        }
    }
}
