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
using Gs2.Gs2Limit.Model; /* diff +++ */
using Gs2.Gs2Limit.Request;
using Gs2.Gs2Limit.Model.Cache;
using Gs2.Gs2Limit.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Limit.Domain.SpeculativeExecutor
{
    public static class VerifyCounterByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyCounterByUserIdRequest _request;
            private readonly string _expectedCounterId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyCounterByUserIdRequest request,
                string expectedCounterId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedCounterId = expectedCounterId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Counter)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _accessToken.UserId,
                    _request.LimitName,
                    _request.CounterName,
                    _accessToken.TimeOffset
                );
                if (!found || item == null ||
                    item.CounterId != _expectedCounterId ||
                    item.UserId != _accessToken.UserId ||
                    item.LimitName != _request.LimitName ||
                    item.Name != _request.CounterName) {
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
            return "Gs2Limit:VerifyCounterByUserId";
/* diff +++ start */
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCounterByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyCounterByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "less":
                    inverse.VerifyType = "greaterEqual";
                    break;
                case "lessEqual":
                    inverse.VerifyType = "greater";
                    break;
                case "greater":
                    inverse.VerifyType = "lessEqual";
                    break;
                case "greaterEqual":
                    inverse.VerifyType = "less";
                    break;
                case "equal":
                    inverse.VerifyType = "notEqual";
                    break;
                case "notEqual":
                    inverse.VerifyType = "equal";
                    break;
                default:
                    return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Limit.Model.Counter Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCounterByUserIdRequest request,
            Gs2.Gs2Limit.Model.Counter item
        ) {
            switch (request?.VerifyType) {
                case "less":
                case "lessEqual":
                case "greater":
                case "greaterEqual":
                case "equal":
                case "notEqual":
                    break;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCounterByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCounterByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Limit.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Counter(
                request.LimitName,
                request.CounterName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null ? null :
                VerifyCounterByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedRequest?.NamespaceName) ||
                string.IsNullOrEmpty(preparedRequest.LimitName) ||
                string.IsNullOrEmpty(preparedRequest.CounterName) ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest.UserId != preparedAccessToken.UserId) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var expectedCounterId = string.Join(
                ":", "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "limit", preparedRequest.NamespaceName,
                "user", userId,
                "limit", preparedRequest.LimitName,
                "counter", preparedRequest.CounterName
            );
            var (item, found) = ((Counter)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                preparedRequest.LimitName,
                preparedRequest.CounterName,
                preparedAccessToken.TimeOffset
            );
            if (!found || item == null ||
                item.CounterId != expectedCounterId ||
                item.UserId != userId ||
                item.LimitName != preparedRequest.LimitName ||
                item.Name != preparedRequest.CounterName) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
            Transform(domain, preparedAccessToken, preparedRequest, item); /* diff +++ */

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
                preparedRequest,
                expectedCounterId
            ).Invoke;
/* diff +++ end */
        }
    }
}
