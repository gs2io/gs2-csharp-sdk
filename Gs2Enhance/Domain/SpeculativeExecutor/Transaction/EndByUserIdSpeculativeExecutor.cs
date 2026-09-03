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
using System.Numerics;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Enhance.Domain.SpeculativeExecutor;
using Gs2.Gs2Enhance.Model;
using Gs2.Gs2Enhance.Model.Cache;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Experience.Domain.SpeculativeExecutor;
using Gs2.Gs2Experience.Request;
using AcquireAction = Gs2.Core.Model.AcquireAction;
using ConsumeAction = Gs2.Core.Model.ConsumeAction;
using Progress = Gs2.Gs2Enhance.Model.Progress;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enhance.Domain.Transaction.SpeculativeExecutor
{
    public static class EndByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Enhance:EndByUserId";
        }

        private static Progress GetProgress(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken token,
            EndByUserIdRequest request
        ) {
            var cached = ((Progress)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                token.UserId,
                token.TimeOffset
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:enhance:{request.NamespaceName}:" +
                $"user:{token.UserId}:progress";
            var progress = cached.Item1;
            return cached.Item2 && progress != null &&
                   progress.ProgressId == expectedId &&
                   progress.UserId == token.UserId
                ? progress
                : null;
        }

        private static bool TryGetRateModel(
            Gs2.Core.Domain.Gs2 domain,
            EndByUserIdRequest request,
            string rateName,
            out RateModel model,
            out bool found
        ) {
            var cached = ((RateModel)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                rateName,
                null
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                   $"{domain.RestSession.OwnerId}:enhance:{request.NamespaceName}:" +
                   $"rateModel:{rateName}";
            found = cached.Item2;
            model = cached.Item1;
            if (!found) {
                model = null;
                return true;
            }
            return model != null && model.RateModelId == expectedId &&
                   model.Name == rateName;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            EndByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            EndByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = EndByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) {
                return null;
            }
            var progress = GetProgress(domain, token, prepared);
            if (progress == null || progress.RateName == null) return null;
            var progressSnapshot = progress.ToJson().ToJson();
            if (!TryGetRateModel(
                    domain,
                    prepared,
                    progress.RateName,
                    out var rateModel,
                    out var rateFound
                )) {
                return null;
            }
            var rateSnapshot = rateModel?.ToJson().ToJson();

            ConsumeAction consume = new ConsumeAction()
                .WithAction(DeleteProgressByUserIdSpeculativeExecutor.Action())
                .WithRequest(new DeleteProgressByUserIdRequest()
                    .WithNamespaceName(prepared.NamespaceName)
                    .WithUserId(token.UserId)
                    .ToJson().ToJson());
            foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                consume = consume?.ApplyConfig(config.Key, config.Value);
            }

            var acquireActions = new List<AcquireAction>();
            if (rateModel != null) {
                var namespaceName = Gs2.Gs2Experience.Model.ExperienceModel
                    .GetNamespaceNameFromGrn(rateModel.ExperienceModelId);
                var experienceName = Gs2.Gs2Experience.Model.ExperienceModel
                    .GetExperienceNameFromGrn(rateModel.ExperienceModelId);
                if (namespaceName == null || experienceName == null ||
                    progress.PropertyId == null ||
                    progress.ExperienceValue == null) {
                    return null;
                }
                AcquireAction acquire = new AcquireAction()
                    .WithAction(AddExperienceByUserIdSpeculativeExecutor.Action())
                    .WithRequest(new AddExperienceByUserIdRequest()
                        .WithNamespaceName(namespaceName)
                        .WithUserId(token.UserId)
                        .WithExperienceName(experienceName)
                        .WithPropertyId(progress.PropertyId)
                        .WithExperienceValue(progress.ExperienceValue)
                        .WithTruncateExperienceWhenRankUp(false)
                        .ToJson().ToJson());
                foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                    acquire = acquire?.ApplyConfig(config.Key, config.Value);
                }
                if (acquire != null) acquireActions.Add(acquire);
            }

            var commits = new List<Func<object>> {
                consume == null ? null : await Gs2.Gs2Enhance.Domain
                    .SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex
                    .ExecuteAsync(domain, token, consume, BigInteger.One),
            };
            foreach (var acquire in acquireActions) {
                commits.Add(await Gs2.Gs2Experience.Domain.SpeculativeExecutor
                    .AcquireActionSpeculativeExecutorIndex.ExecuteAsync(
                        domain, token, acquire, BigInteger.One
                    ));
            }
            var commit = Core.SpeculativeExecutor.SpeculativeExecutor
                .BuildAtomicCommit(
                    commits,
                    (consume == null ? 0 : 1) + acquireActions.Count
                );
            if (commit == null) return null;
            return () => {
                var validRateState = TryGetRateModel(
                    domain,
                    prepared,
                    progress.RateName,
                    out var liveRate,
                    out var liveRateFound
                );
                if (GetProgress(domain, token, prepared)?.ToJson().ToJson() !=
                        progressSnapshot ||
                    !validRateState || liveRateFound != rateFound ||
                    liveRate?.ToJson().ToJson() != rateSnapshot) {
                    return null;
                }
                return commit();
            };
        }
    }
}
