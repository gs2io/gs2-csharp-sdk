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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
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
    public static class CountDownByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Limit:CountDownByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            CountDownByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            CountDownByUserIdRequest request
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
            var preparedRequest = request == null
                ? null
                : new CountDownByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithLimitName(request.LimitName)
                    .WithCounterName(request.CounterName)
                    .WithCountDownValue(request.CountDownValue);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(preparedRequest.NamespaceName) ||
                string.IsNullOrEmpty(preparedRequest.LimitName) ||
                string.IsNullOrEmpty(preparedRequest.CounterName) ||
                !preparedRequest.CountDownValue.HasValue) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
            var expectedCounterId = string.Join(
                ":", "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "limit", preparedRequest.NamespaceName,
                "user", userId,
                "limit", preparedRequest.LimitName,
                "counter", preparedRequest.CounterName
            );
            bool IsExpected(Counter item) {
                return item != null &&
                       item.CounterId == expectedCounterId &&
                       item.UserId == userId &&
                       item.LimitName == preparedRequest.LimitName &&
                       item.Name == preparedRequest.CounterName;
            }
            var (item, found) = ((Counter)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                preparedRequest.LimitName,
                preparedRequest.CounterName,
                timeOffset
            );
            if (!found || !IsExpected(item)) {
                return null;
            }
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var (cachedItem, find) = ((Counter)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.LimitName,
                    request.CounterName,
                    null
 diff --- end */
/* diff +++ start */
                    preparedRequest.NamespaceName,
                    userId,
                    preparedRequest.LimitName,
                    preparedRequest.CounterName,
                    timeOffset
                );
                if (!find || !IsExpected(cachedItem)) {
                    return null;
                }
                Counter committedItem;
                try {
                    committedItem = cachedItem.SpeculativeExecution(
                        preparedRequest
                    );
                }
                catch (System.Exception) {
                    return null;
                }
                committedItem.PutCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    userId,
                    preparedRequest.LimitName,
                    preparedRequest.CounterName,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
