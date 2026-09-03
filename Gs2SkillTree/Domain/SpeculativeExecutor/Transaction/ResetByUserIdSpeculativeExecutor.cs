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
using System.Collections;
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2SkillTree.Domain.SpeculativeExecutor;
using Gs2.Gs2SkillTree.Model;
using Gs2.Gs2SkillTree.Model.Cache; /* diff +++ */
using Gs2.Gs2SkillTree.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2SkillTree.Domain.Transaction.SpeculativeExecutor
{
    public static class ResetByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2SkillTree:ResetByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ResetByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ResetByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = ResetByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) return null;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:skillTree:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:status:" +
                prepared.PropertyId;
            var cached = ((Status)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.PropertyId,
                token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.StatusId != expectedId ||
                cached.Item1.UserId != token.UserId ||
                cached.Item1.PropertyId != prepared.PropertyId ||
                cached.Item1.ReleasedNodeNames == null) return null;
            var commit = new StatusSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.PropertyId,
                token.TimeOffset,
                expectedId,
                item => {
                    var changed = item.Clone() as Status;
                    changed.ReleasedNodeNames = Array.Empty<string>();
                    changed.Revision = 0;
                    return changed;
                },
                true
            );
            return commit.CanPrepare() ? commit.Invoke : null;
        }
    }
}
