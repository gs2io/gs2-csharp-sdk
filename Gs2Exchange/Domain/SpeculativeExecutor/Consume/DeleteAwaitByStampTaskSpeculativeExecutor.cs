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
using Gs2.Gs2Exchange.Request;
using Gs2.Gs2Exchange.Model.Cache;
using Gs2.Gs2Exchange.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Exchange.Domain.SpeculativeExecutor
{
    public static class DeleteAwaitByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Exchange:DeleteAwaitByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteAwaitByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteAwaitByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Exchange.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Await(
                request.AwaitName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = DeleteAwaitByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.AwaitName)) {
                return null;
            }
            var userId = token.UserId;
            var timeOffset = token.TimeOffset;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:exchange:{prepared.NamespaceName}:" +
                $"user:{userId}:await:{prepared.AwaitName}";
            var cached = ((Gs2.Gs2Exchange.Model.Await)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.AwaitName,
                timeOffset
            );
            var item = cached.Item1;
            if (!cached.Item2 || item == null || item.AwaitId != expectedId ||
                item.UserId != userId || item.Name != prepared.AwaitName) {
                return null;
            }
            var expected = item.Clone() as Gs2.Gs2Exchange.Model.Await;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
 diff --- end */
/* diff +++ start */
            return () => {
                var current = ((Gs2.Gs2Exchange.Model.Await)null).GetCache(
/* diff +++ end */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    accessToken.UserId,
                    request.AwaitName,
                    accessToken.TimeOffset
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    userId,
                    prepared.AwaitName,
                    timeOffset
/* diff +++ end */
                );
/* diff +++ start */
                if (current.Item2 && current.Item1 != null &&
                    current.Item1.ToJson().ToJson() == expected.ToJson().ToJson()) {
                    (null as Gs2.Gs2Exchange.Model.Await).PutCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        userId,
                        prepared.AwaitName,
                        timeOffset
                    );
                }
/* diff +++ end */
                return null;
            };
        }
    }
}
