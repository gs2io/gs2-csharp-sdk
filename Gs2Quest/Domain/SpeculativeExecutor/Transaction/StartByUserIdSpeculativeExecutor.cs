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
using Gs2.Gs2Quest.Request;
using Gs2.Gs2Quest.Model;
using Gs2.Gs2Quest.Model.Cache;
using Gs2.Util.LitJson;
using AcquireAction = Gs2.Core.Model.AcquireAction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Quest.Domain.Transaction.SpeculativeExecutor
{
    public static class StartByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Quest:StartByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            StartByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            StartByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain == null || request == null || string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = StartByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId) {
                return null;
            }
            var cached = ((QuestModel)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.QuestGroupName,
                prepared.QuestName,
                null
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:quest:{prepared.NamespaceName}:" +
                $"group:{prepared.QuestGroupName}:quest:{prepared.QuestName}";
            if (!cached.Item2 || item == null ||
                item.QuestModelId != expectedId || item.Name != prepared.QuestName) {
                return null;
            }

            var consumeActions = item.ConsumeActions?.Select(v =>
            {
                var action = v?.Clone() as Gs2.Core.Model.ConsumeAction;
                foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                    action = action?.ApplyConfig(config.Key, config.Value);
                }
                return action;
            }).Where(v => v != null).ToArray() ?? Array.Empty<Gs2.Core.Model.ConsumeAction>();

            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                consumeActions,
                new [] {
                    new AcquireAction {
                        Action = "Gs2Quest:CreateProgressByUserId",
                        Request = new CreateProgressByUserIdRequest {
                            NamespaceName = prepared.NamespaceName,
                            UserId = token.UserId,
                            Force = prepared.Force,
                            Config = prepared.Config,
                        }.ToJson().ToJson()
                    }
                },
                1.0
            ).ExecuteAsync(
                domain,
                token
            );

            return commit;
        }
    }
}
