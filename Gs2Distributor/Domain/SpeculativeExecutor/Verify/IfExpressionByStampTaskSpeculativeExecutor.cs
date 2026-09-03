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
    public static class IfExpressionByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedBranch :
            IPreparedSpeculativeVerification
        {
            private readonly Func<object> _conditionCommit;
            private readonly IPreparedSpeculativeVerification _condition;
            private readonly Func<object> _branchCommit;

            internal PreparedBranch(
                Func<object> conditionCommit,
                IPreparedSpeculativeVerification condition,
                Func<object> branchCommit
            ) {
                _conditionCommit = conditionCommit;
                _condition = condition;
                _branchCommit = branchCommit;
            }

            public bool IsStillSatisfied()
            {
                return _condition.IsStillSatisfied();
            }

            internal object Commit()
            {
                if (!IsStillSatisfied()) {
                    return null;
                }
                _conditionCommit.Invoke();
                _branchCommit?.Invoke();
                return null;
            }
        }

/* diff +++ end */
        public static string Action() {
            return "Gs2Distributor:IfExpressionByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IfExpressionByUserIdRequest request
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
            IfExpressionByUserIdRequest request
/* diff +++ start */
        ) => ExecuteRatedAsync(domain, accessToken, request, BigInteger.One);

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> ExecuteRatedAsync(
#else
        internal static async Task<Func<object>> ExecuteRatedAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IfExpressionByUserIdRequest request,
            BigInteger rate
/* diff +++ end */
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null ? null :
                IfExpressionByUserIdRequest.FromJson(request.ToJson());
            if (preparedRequest != null) {
                if (request?.TrueActions?.Length == 0) {
                    preparedRequest.TrueActions =
                        Array.Empty<Gs2.Core.Model.ConsumeAction>();
                }
                if (request?.FalseActions?.Length == 0) {
                    preparedRequest.FalseActions =
                        Array.Empty<Gs2.Core.Model.ConsumeAction>();
                }
            }
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (preparedRequest?.Condition == null ||
                preparedRequest.UserId != preparedAccessToken?.UserId) {
                return null;
            }

            Func<object> conditionCommit;
            Gs2.Core.Model.ConsumeAction[] selectedActions;
            try {
                conditionCommit = await Gs2.Core.SpeculativeExecutor
                    .VerifyActionSpeculativeExecutor.ExecuteAsync(
                        domain,
                        preparedAccessToken,
                        preparedRequest.Condition,
                        rate
                    );
                if (conditionCommit == null) {
                    return null;
                }
                selectedActions = preparedRequest.TrueActions;
            }
            catch (Gs2Exception) {
                try {
                    conditionCommit = await Gs2.Core.SpeculativeExecutor
                        .VerifyActionSpeculativeExecutor.ExecuteInverseAsync(
                            domain,
                            preparedAccessToken,
                            preparedRequest.Condition,
                            rate
                        );
                }
                catch (Gs2Exception) {
                    return null;
                }
                if (conditionCommit == null) {
                    return null;
                }
                selectedActions = preparedRequest.FalseActions;
            }
            if (conditionCommit.Target is not
                IPreparedSpeculativeVerification conditionGuard) {
                return null;
            }
            if (selectedActions == null) {
                return null;
            }

            var branchCommit = await new Gs2.Core.SpeculativeExecutor
                .SpeculativeExecutor(
                    selectedActions,
                    null,
                    rate
                )
                .ExecuteAsync(domain, preparedAccessToken);
            return new PreparedBranch(
                conditionCommit,
                conditionGuard,
                branchCommit
            ).Commit;
/* diff +++ end */
        }
    }
}
