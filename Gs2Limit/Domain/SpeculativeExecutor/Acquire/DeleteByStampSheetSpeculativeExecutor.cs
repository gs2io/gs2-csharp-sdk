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
    public static class DeleteCounterByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Limit:DeleteCounterByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteCounterByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteCounterByUserIdRequest request
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

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                DeleteCounterByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = accessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                prepared?.UserId != accessToken.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.LimitName) ||
                string.IsNullOrEmpty(prepared.CounterName)) {
                return null;
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var expectedCounterId = string.Join(
                ":", "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "limit", prepared.NamespaceName,
                "user", userId,
                "limit", prepared.LimitName,
                "counter", prepared.CounterName
            );
            var cached = ((Counter)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.LimitName,
                prepared.CounterName,
                timeOffset
            );
            if (!cached.Item2 || cached.Item1 != null &&
                (cached.Item1.CounterId != expectedCounterId ||
                 cached.Item1.UserId != userId ||
                 cached.Item1.LimitName != prepared.LimitName ||
                 cached.Item1.Name != prepared.CounterName)) {
                return null;
            }
            var expected = cached.Item1?.Clone() as Counter;
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var current = ((Counter)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.LimitName,
                    request.CounterName,
                    null
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    userId,
                    prepared.LimitName,
                    prepared.CounterName,
                    timeOffset
                );
                if (!current.Item2 || expected == null && current.Item1 != null ||
                    expected != null &&
                    (current.Item1 == null ||
                     current.Item1.CounterId != expectedCounterId ||
                     current.Item1.UserId != userId ||
                     current.Item1.LimitName != prepared.LimitName ||
                     current.Item1.Name != prepared.CounterName ||
                     current.Item1.Revision > 0 &&
                     current.Item1.ToJson().ToJson() !=
                     expected.ToJson().ToJson())) {
                    return null;
                }
                ((Counter)null).PutCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    userId,
                    prepared.LimitName,
                    prepared.CounterName,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
