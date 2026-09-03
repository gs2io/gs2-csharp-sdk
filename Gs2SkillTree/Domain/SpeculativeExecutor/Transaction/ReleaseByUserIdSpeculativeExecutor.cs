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
using System.Collections.Generic;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2SkillTree.Domain.SpeculativeExecutor;
using Gs2.Gs2SkillTree.Model;
using Gs2.Gs2SkillTree.Model.Cache;
using Gs2.Gs2SkillTree.Request;
using Gs2.Core.Model;
using AcquireAction = Gs2.Core.Model.AcquireAction;
using ConsumeAction = Gs2.Core.Model.ConsumeAction;
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
    public static class ReleaseByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2SkillTree:ReleaseByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReleaseByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReleaseByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = ReleaseByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId ||
                prepared.NodeModelNames == null) {
                return null;
            }

            var commits = new List<Func<object>>();
            var markRelease = await MarkReleaseByUserIdSpeculativeExecutor.ExecuteAsync(
                domain,
                token,
                new MarkReleaseByUserIdRequest()
                    .WithNamespaceName(prepared.NamespaceName)
                    .WithUserId(token.UserId)
                    .WithPropertyId(prepared.PropertyId)
                    .WithNodeModelNames(
                        prepared.NodeModelNames.Clone() as string[]
                    )
            );
            if (markRelease != null) commits.Add(markRelease);

            var consumeActions = new List<ConsumeAction>();
            foreach (var nodeModelName in prepared.NodeModelNames) {
                var cached = ((NodeModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    nodeModelName,
                    null
                );
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:skillTree:" +
                    $"{prepared.NamespaceName}:model:{nodeModelName}";
                var model = cached.Item1;
                if (!cached.Item2 || model == null ||
                    model.NodeModelId != expectedId || model.Name != nodeModelName ||
                    model.ReleaseConsumeActions == null) {
                    continue;
                }
                foreach (var action in model.ReleaseConsumeActions) {
                    var snapshot = action?.Clone() as ConsumeAction;
                    foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                        snapshot = snapshot?.ApplyConfig(config.Key, config.Value);
                    }
                    if (snapshot != null) consumeActions.Add(snapshot);
                }
            }
            if (consumeActions.Count > 0) {
                var consume = await new Gs2.Core.SpeculativeExecutor.SpeculativeExecutor(
                    consumeActions.ToArray(),
                    Array.Empty<AcquireAction>(),
                    1.0
                ).ExecuteAsync(domain, token);
                if (consume != null) commits.Add(consume);
            }

            return Gs2.Core.SpeculativeExecutor.SpeculativeExecutor.BuildAtomicCommit(
                commits,
                2
            );
        }
    }
}
