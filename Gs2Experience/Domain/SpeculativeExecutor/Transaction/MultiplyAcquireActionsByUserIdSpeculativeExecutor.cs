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
using System.Numerics;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Experience.Model;
using Gs2.Gs2Experience.Model.Cache;
using Gs2.Gs2Experience.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Experience.Domain.Transaction.SpeculativeExecutor
{
    public static class MultiplyAcquireActionsByUserIdSpeculativeExecutor {
        private static readonly BigInteger MaxExactEnclosingRate =
            new BigInteger(1 << 24);

        public static string Action() {
            return "Gs2Experience:MultiplyAcquireActionsByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MultiplyAcquireActionsByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            return await ExecuteAsync(
                domain,
                accessToken,
                request,
                BigInteger.One
            );
        }

#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> ExecuteAsync(
#else
        internal static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MultiplyAcquireActionsByUserIdRequest request,
            BigInteger enclosingRate
        ) {
            try {
                var token = accessToken?.Clone() as AccessToken;
                var prepared = MultiplyAcquireActionsByUserIdRequest.FromJson(
                    request?.ToJson()
                );
                if (domain == null || prepared == null ||
                    string.IsNullOrEmpty(token?.UserId)) {
                    return null;
                }
                if (prepared.UserId == "#{userId}") {
                    prepared.UserId = token.UserId;
                }
                if (prepared.UserId != token.UserId || enclosingRate <= 0 ||
                    enclosingRate > MaxExactEnclosingRate) {
                    return null;
                }

                var propertyId = prepared.PropertyId
                    ?.Replace("{region}", domain.RestSession.Region.DisplayName())
                    .Replace("{ownerId}", domain.RestSession.OwnerId ?? "")
                    .Replace("{userId}", token.UserId);
                var expectedModelId = string.Join(
                    ":",
                    "grn",
                    "gs2",
                    domain.RestSession.Region.DisplayName(),
                    domain.RestSession.OwnerId,
                    "experience",
                    prepared.NamespaceName,
                    "model",
                    prepared.ExperienceName
                );
                var expectedStatusId = string.Join(
                    ":",
                    "grn",
                    "gs2",
                    domain.RestSession.Region.DisplayName(),
                    domain.RestSession.OwnerId,
                    "experience",
                    prepared.NamespaceName,
                    "user",
                    token.UserId,
                    "experienceModel",
                    prepared.ExperienceName,
                    "property",
                    propertyId
                );

                var modelCache = ((ExperienceModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    prepared.ExperienceName,
                    null
                );
                var statusCache = ((Status)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.ExperienceName,
                    propertyId,
                    token.TimeOffset
                );
                var model = modelCache.Item1;
                var status = statusCache.Item1;
                if (!modelCache.Item2 || model == null ||
                    model.ExperienceModelId != expectedModelId ||
                    model.Name != prepared.ExperienceName ||
                    !statusCache.Item2 || status == null ||
                    status.StatusId != expectedStatusId ||
                    status.UserId != token.UserId ||
                    status.ExperienceName != prepared.ExperienceName ||
                    status.PropertyId != propertyId ||
                    status.RankValue == null || status.RankValue <= 0 ||
                    status.RankValue > int.MaxValue) {
                    return null;
                }

                var rateDefinition = model.AcquireActionRates?.FirstOrDefault(
                    value => value?.Name == prepared.RateName
                );
                var rankIndex = (int)status.RankValue.Value - 1;
                if (!IsUnitRate(rateDefinition, rankIndex)) {
                    return null;
                }

                var scaledBaseRate = (double)(prepared.BaseRate ?? 1f) *
                                     (double)enclosingRate;
                var serverBaseRate = (float)scaledBaseRate;
                if (float.IsNaN(serverBaseRate) ||
                    float.IsInfinity(serverBaseRate) ||
                    serverBaseRate != 1f) {
                    return null;
                }

                var actions = (prepared.AcquireActions ??
                               Array.Empty<Gs2.Core.Model.AcquireAction>())
                    .Select(action => (action?.Clone() as Gs2.Core.Model.AcquireAction)
                        ?.ApplyConfig("userId", token.UserId))
                    .Where(action => action != null)
                    .ToArray();
                if (actions.Length == 0) {
                    return null;
                }
                var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                    Array.Empty<ConsumeAction>(),
                    actions,
                    BigInteger.One
                ).ExecuteAsync(domain, token);
                if (commit == null) {
                    return null;
                }

                var expectedModel = model.ToJson().ToJson();
                var expectedStatus = status.ToJson().ToJson();
                return () => {
                    var currentModel = ((ExperienceModel)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        prepared.ExperienceName,
                        null
                    );
                    var currentStatus = ((Status)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        token.UserId,
                        prepared.ExperienceName,
                        propertyId,
                        token.TimeOffset
                    );
                    if (currentModel.Item2 && currentModel.Item1 != null &&
                        currentModel.Item1.ToJson().ToJson() == expectedModel &&
                        currentStatus.Item2 && currentStatus.Item1 != null &&
                        currentStatus.Item1.ToJson().ToJson() == expectedStatus) {
                        commit();
                    }
                    return null;
                };
            }
            catch (Exception) {
                return null;
            }
        }

        private static bool IsUnitRate(
            AcquireActionRate definition,
            int rankIndex
        ) {
            if (definition == null || rankIndex < 0) {
                return false;
            }
            if (definition.Mode == "double") {
                if (definition.Rates == null || rankIndex >= definition.Rates.Length) {
                    return false;
                }
                var value = definition.Rates[rankIndex];
                return value == 1d;
            }
            return definition.Mode == "big" &&
                   definition.BigRates != null &&
                   rankIndex < definition.BigRates.Length &&
                   definition.BigRates[rankIndex] == "1";
        }
    }
}
