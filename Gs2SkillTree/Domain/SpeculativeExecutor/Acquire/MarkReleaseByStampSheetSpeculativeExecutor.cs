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
using System.Linq; /* diff +++ */
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
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
    public static class MarkReleaseByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2SkillTree:MarkReleaseByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReleaseByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReleaseByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.SkillTree.Namespace(
                request.NamespaceName
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = new MarkReleaseByUserIdRequest()
                .WithNamespaceName(request.NamespaceName)
                .WithUserId(request.UserId)
                .WithPropertyId(request.PropertyId)
                .WithNodeModelNames((request.NodeModelNames ?? Array.Empty<string>())
                    .Where(value => value != null)
                    .ToArray())
                .WithTimeOffsetToken(request.TimeOffsetToken);
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId) {
                return null;
            }
            if (prepared.NodeModelNames.Length == 0) {
                return null;
            }
            var propertyId = domain.SkillTree.Namespace(
                prepared.NamespaceName
/* diff +++ end */
            ).AccessToken(
/* diff --- start
                accessToken
 diff --- end */
                token /* diff +++ */
            ).Status(
/* diff --- start
                request.PropertyId
            ).ModelAsync();

            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.PropertyId,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
                prepared.PropertyId
            ).PropertyId;
            prepared.PropertyId = propertyId;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:skillTree:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:status:{prepared.PropertyId}";
            var commit = new StatusSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.PropertyId,
                token.TimeOffset,
                expectedId,
                item => item.SpeculativeExecution(prepared),
                preserveAuthoritativeReplacement: true
            );
            return commit.CanPrepare() ? commit.Invoke : null;
/* diff +++ end */
        }
    }
}
