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
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Domain.SpeculativeExecutor;
using Gs2.Gs2Mission.Model;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Request;
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

namespace Gs2.Gs2Mission.Domain.Transaction.SpeculativeExecutor
{
    public static class CompleteByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:CompleteByUserId";
        }

        private static bool IsKnownReceiveFailure(
            Complete complete,
            MissionTaskModel model,
            string missionTaskName
        ) {
            if (complete?.ReceivedMissionTaskNames == null) {
                return false;
            }
            if (complete.ReceivedMissionTaskNames.Contains(missionTaskName)) {
                return true;
            }
            return model.VerifyCompleteType != "consumeActions" &&
                   model.VerifyCompleteType != "verifyActions" &&
                   complete.CompletedMissionTaskNames != null &&
                   !complete.CompletedMissionTaskNames.Contains(
                       missionTaskName
                   );
        }

        private static bool HasKnownReceiveFailure(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken token,
            CompleteByUserIdRequest request,
            MissionTaskModel model
        ) {
            var cached = ((Complete)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                token.UserId,
                request.MissionGroupName,
                token.TimeOffset
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:{request.NamespaceName}:" +
                $"user:{token.UserId}:group:{request.MissionGroupName}:complete";
            var complete = cached.Item1;
            return cached.Item2 && complete != null &&
                   complete.CompleteId == expectedId &&
                   complete.UserId == token.UserId &&
                   complete.MissionGroupName == request.MissionGroupName &&
                   model != null && IsKnownReceiveFailure(
                       complete,
                       model,
                       request.MissionTaskName
                   );
        }

        private static bool IsPreparedModelCurrent(
            Gs2.Core.Domain.Gs2 domain,
            CompleteByUserIdRequest request,
            string expectedId,
            string preparedSnapshot
        ) {
            var cached = ((MissionTaskModel)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                request.MissionGroupName,
                request.MissionTaskName,
                null
            );
            var current = cached.Item1;
            return cached.Item2 && current != null &&
                   current.MissionTaskId == expectedId &&
                   current.Name == request.MissionTaskName &&
                   string.Equals(
                       current.ToJson().ToJson(),
                       preparedSnapshot,
                       StringComparison.Ordinal
                   );
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            CompleteByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            CompleteByUserIdRequest request
        ) {
            var token = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            var prepared = request == null
                ? null
                : new CompleteByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithMissionTaskName(request.MissionTaskName)
                    .WithUserId(request.UserId)
                    .WithConfig((request.Config ?? Array.Empty<Config>())
                        .Where(config => config != null)
                        .Select(config => new Config()
                            .WithKey(config.Key)
                            .WithValue(config.Value)
                        ).ToArray());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MissionGroupName) ||
                string.IsNullOrEmpty(prepared.MissionTaskName)) {
                return null;
            }

            var acquireActions = new List<AcquireAction>();
            var cached = ((MissionTaskModel)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.MissionGroupName,
                prepared.MissionTaskName,
                null
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:{prepared.NamespaceName}:" +
                $"group:{prepared.MissionGroupName}:missionTaskModel:" +
                prepared.MissionTaskName;
            var model = cached.Item1;
            if (!cached.Item2 || model == null ||
                model.MissionTaskId != expectedId ||
                model.Name != prepared.MissionTaskName) {
                return null;
            }
            if (model?.CompleteAcquireActions != null) {
                foreach (var action in model.CompleteAcquireActions) {
                    var snapshot = action?.Clone() as AcquireAction;
                    foreach (var config in prepared.Config) {
                        snapshot = snapshot?.ApplyConfig(config.Key, config.Value);
                    }
                    if (snapshot != null) acquireActions.Add(snapshot);
                }
            }

            var verification = await CompleteSpeculativeExecutor
                .PrepareVerifyCompleteConsumeActionsAsync(
                    domain,
                    token,
                    model.VerifyCompleteConsumeActions,
                    prepared.Config
                );
            if (verification.KnownFalse) {
                return null;
            }
            var modelSnapshot = model?.ToJson().ToJson();
            if (HasKnownReceiveFailure(domain, token, prepared, model)) {
                return null;
            }

            var receive = new ConsumeAction()
                .WithAction(ReceiveByUserIdSpeculativeExecutor.Action())
                .WithRequest(new ReceiveByUserIdRequest()
                    .WithNamespaceName(prepared.NamespaceName)
                    .WithMissionGroupName(prepared.MissionGroupName)
                    .WithMissionTaskName(prepared.MissionTaskName)
                    .WithUserId(token.UserId)
                    .ToJson().ToJson());
            var commit = await new Gs2.Core.SpeculativeExecutor.SpeculativeExecutor(
                new[] { receive },
                acquireActions.ToArray(),
                1.0
            ).ExecuteAsync(domain, token);
            if (commit == null) return null;
            return () => {
                if (!IsPreparedModelCurrent(
                        domain,
                        prepared,
                        expectedId,
                        modelSnapshot
                    ) || HasKnownReceiveFailure(
                        domain,
                        token,
                        prepared,
                        model
                    )) {
                    return null;
                }
                if (verification.Commit?.Target is
                        IPreparedSpeculativeVerification guard &&
                    !guard.IsStillSatisfied()) {
                    return null;
                }
                verification.Commit?.Invoke();
                return commit();
            };
        }
    }
}
