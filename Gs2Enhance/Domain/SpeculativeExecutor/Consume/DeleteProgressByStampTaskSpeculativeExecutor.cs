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
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Enhance.Model; /* diff +++ */
using Gs2.Gs2Enhance.Model.Cache;
using Gs2.Gs2Enhance.Model.Transaction;
using Progress = Gs2.Gs2Enhance.Model.Progress; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enhance.Domain.SpeculativeExecutor
{
    public static class DeleteProgressByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Enhance:DeleteProgressByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteProgressByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteProgressByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Enhance.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Progress(
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = DeleteProgressByUserIdRequest.FromJson(
                request?.ToJson()
            );
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) return null;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:enhance:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:progress";
            bool IsExpected(Progress value) {
                return value != null && value.ProgressId == expectedId &&
                       value.UserId == token.UserId;
            }
            var cached = ((Progress)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                token.TimeOffset
            );
            if (!cached.Item2 || !IsExpected(cached.Item1)) return null;
            var preparedSnapshot = cached.Item1.ToJson().ToJson();
            return () => {
                var live = ((Progress)null).GetCache(
/* diff +++ end */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    accessToken.UserId,
                    accessToken.TimeOffset
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    token.UserId,
                    token.TimeOffset
                );
                if (!live.Item2 || !IsExpected(live.Item1) ||
                    live.Item1.ToJson().ToJson() != preparedSnapshot) return null;
                ((Progress)null).PutCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    token.TimeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
