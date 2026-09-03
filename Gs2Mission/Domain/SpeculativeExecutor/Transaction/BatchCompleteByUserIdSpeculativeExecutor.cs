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
using System.Collections.Generic; /* diff +++ */
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Domain.SpeculativeExecutor;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Model;
using Gs2.Gs2Mission.Model.Cache;
using AcquireAction = Gs2.Core.Model.AcquireAction;
using ConsumeAction = Gs2.Core.Model.ConsumeAction;
using VerifyAction = Gs2.Core.Model.VerifyAction;
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
    public static class BatchCompleteByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:BatchCompleteByUserId";
        }

        private static string MissionTaskId(
            Gs2.Core.Domain.Gs2 domain,
            BatchCompleteByUserIdRequest request,
            string missionTaskName
        ) {
            return $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                   $"{domain.RestSession.OwnerId}:mission:{request.NamespaceName}:" +
                   $"group:{request.MissionGroupName}:missionTaskModel:" +
                   missionTaskName;
        }

        private static MissionTaskModel GetModel(
            Gs2.Core.Domain.Gs2 domain,
            BatchCompleteByUserIdRequest request,
            string missionTaskName
        ) {
            var cached = ((MissionTaskModel)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                request.MissionGroupName,
                missionTaskName,
                null
            );
            var model = cached.Item1;
            return cached.Item2 && model != null &&
                   model.MissionTaskId == MissionTaskId(
                       domain, request, missionTaskName
                   ) && model.Name == missionTaskName
                ? model
                : null;
        }

        private static MissionGroupModel GetGroup(
            Gs2.Core.Domain.Gs2 domain,
            BatchCompleteByUserIdRequest request
        ) {
            var cached = ((MissionGroupModel)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                request.MissionGroupName,
                null
            );
            var group = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:{request.NamespaceName}:" +
                $"group:{request.MissionGroupName}";
            return cached.Item2 && group != null &&
                   group.MissionGroupId == expectedId &&
                   group.Name == request.MissionGroupName
                ? group
                : null;
        }

        private static bool ArePreparedModelsCurrent(
            Gs2.Core.Domain.Gs2 domain,
            BatchCompleteByUserIdRequest request,
            IReadOnlyDictionary<string, string> snapshots
        ) {
            foreach (var snapshot in snapshots) {
                var current = GetModel(domain, request, snapshot.Key);
                if (!string.Equals(
                        current?.ToJson().ToJson(),
                        snapshot.Value,
                        StringComparison.Ordinal
                    )) {
                    return false;
                }
            }
            return true;
        }

        private static bool IsPreparedGroupCurrent(
            Gs2.Core.Domain.Gs2 domain,
            BatchCompleteByUserIdRequest request,
            string preparedSnapshot
        ) {
            return string.Equals(
                GetGroup(domain, request)?.ToJson().ToJson(),
                preparedSnapshot,
                StringComparison.Ordinal
            );
        }

        private static bool HasKnownReceiveFailure(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken token,
            BatchCompleteByUserIdRequest request,
            IReadOnlyCollection<MissionTaskModel> models
        ) {
            var cached = ((Complete)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                token.UserId,
                request.MissionGroupName,
                token.TimeOffset
            );
            var complete = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:{request.NamespaceName}:" +
                $"user:{token.UserId}:group:{request.MissionGroupName}:complete";
            if (!cached.Item2 || complete == null ||
                complete.CompleteId != expectedId ||
                complete.UserId != token.UserId ||
                complete.MissionGroupName != request.MissionGroupName) {
                return false;
            }
            if (complete.ReceivedMissionTaskNames == null) {
                return false;
            }
            if (request.MissionTaskNames.Any(
                    complete.ReceivedMissionTaskNames.Contains
                )) {
                return true;
            }
            return models.Any(model =>
                model.VerifyCompleteType != "consumeActions" &&
                model.VerifyCompleteType != "verifyActions" &&
                complete.CompletedMissionTaskNames != null &&
                !complete.CompletedMissionTaskNames.Contains(model.Name)
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchCompleteByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchCompleteByUserIdRequest request
        ) {
            var token = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            var prepared = request == null
                ? null
                : new BatchCompleteByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithMissionGroupName(request.MissionGroupName)
                    .WithMissionTaskNames(request.MissionTaskNames?.ToArray())
                    .WithUserId(request.UserId)
                    .WithConfig((request.Config ?? Array.Empty<Config>())
                        .Where(config => config != null)
                        .Select(config => new Config()
                            .WithKey(config.Key)
                            .WithValue(config.Value)
                        ).ToArray());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared?.MissionTaskNames == null ||
                prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MissionGroupName)) {
                return null;
            }

            var requested = new HashSet<string>();
            foreach (var missionTaskName in prepared.MissionTaskNames) {
                if (!string.IsNullOrEmpty(missionTaskName)) {
                    requested.Add(missionTaskName);
                }
            }
            if (requested.Count == 0) return null;
            var group = GetGroup(domain, prepared);
            if (group?.Tasks == null) return null;
            var ordered = new List<MissionTaskModel>();
            foreach (var task in group.Tasks) {
                if (task?.Name != null && requested.Remove(task.Name)) {
                    if (task.MissionTaskId != MissionTaskId(
                            domain, prepared, task.Name
                        )) {
                        return null;
                    }
                    ordered.Add(task);
                }
            }
            if (ordered.Count == 0) return null;
            prepared.MissionTaskNames = ordered.Select(
                model => model.Name
            ).ToArray();
            var groupSnapshot = group.ToJson().ToJson();
            var snapshots = new Dictionary<string, string>();
            var acquireActions = new List<AcquireAction>();
            var verifyActions = new List<VerifyAction>();
            foreach (var model in ordered) {
                var direct = GetModel(domain, prepared, model.Name);
                var directSnapshot = direct?.ToJson().ToJson();
                snapshots[model.Name] = directSnapshot;
                if (direct != null && !string.Equals(
                        directSnapshot,
                        model.ToJson().ToJson(),
                        StringComparison.Ordinal
                    )) {
                    return null;
                }
                verifyActions.AddRange(
                    model.VerifyCompleteConsumeActions ??
                    Array.Empty<VerifyAction>()
                );
                foreach (var action in model.CompleteAcquireActions ??
                         Array.Empty<AcquireAction>()) {
                    var snapshot = action?.Clone() as AcquireAction;
                    foreach (var config in prepared.Config) {
                        snapshot = snapshot?.ApplyConfig(
                            config.Key, config.Value
                        );
                    }
                    if (snapshot != null) acquireActions.Add(snapshot);
                }
            }
            var verification = await CompleteSpeculativeExecutor
                .PrepareVerifyCompleteConsumeActionsAsync(
                    domain,
                    token,
                    verifyActions,
                    prepared.Config
                );
            if (verification.KnownFalse) return null;
            if (HasKnownReceiveFailure(domain, token, prepared, ordered)) {
                return null;
            }

            var receive = new ConsumeAction()
                .WithAction(BatchReceiveByUserIdSpeculativeExecutor.Action())
                .WithRequest(new BatchReceiveByUserIdRequest()
                    .WithNamespaceName(prepared.NamespaceName)
                    .WithMissionGroupName(prepared.MissionGroupName)
                    .WithMissionTaskNames(prepared.MissionTaskNames)
                    .WithUserId(token.UserId)
                    .ToJson().ToJson());
            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                new[] { receive },
                acquireActions.ToArray(),
                1.0
            ).ExecuteAsync(domain, token);
            if (commit == null) return null;
            return () => {
                if (!IsPreparedGroupCurrent(
                        domain, prepared, groupSnapshot
                    ) || !ArePreparedModelsCurrent(
                        domain, prepared, snapshots
                    ) ||
                    HasKnownReceiveFailure(domain, token, prepared, ordered)) {
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
