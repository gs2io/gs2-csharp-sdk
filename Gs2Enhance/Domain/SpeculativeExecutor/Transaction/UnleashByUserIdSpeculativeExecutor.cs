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
using System.Text.RegularExpressions;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Enhance.Model;
using Gs2.Gs2Enhance.Model.Cache;
using Gs2.Gs2Enhance.Request;
using Gs2.Gs2Grade.Model.Cache;
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Inventory.Request;
using GradeAcquireIndex = Gs2.Gs2Grade.Domain.SpeculativeExecutor.AcquireActionSpeculativeExecutorIndex;
using GradeAddExecutor = Gs2.Gs2Grade.Domain.SpeculativeExecutor.AddGradeByUserIdSpeculativeExecutor;
using InventoryConsumeIndex = Gs2.Gs2Inventory.Domain.SpeculativeExecutor.ConsumeActionSpeculativeExecutorIndex;
using InventoryConsumeExecutor = Gs2.Gs2Inventory.Domain.SpeculativeExecutor.ConsumeItemSetByUserIdSpeculativeExecutor;
using GradeStatus = Gs2.Gs2Grade.Model.Status;
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
    public static class UnleashByUserIdSpeculativeExecutor {

        private static readonly Regex ItemSetIdRegex = new Regex(
            @"\Agrn:gs2:[-_.{}a-zA-Z0-9]+:[-_.{}a-zA-Z0-9]+:" +
            @"inventory:(?<namespaceName>[-_.{}a-zA-Z0-9]+):" +
            @"user:[-_.{}a-zA-Z0-9]+:" +
            @"inventory:(?<inventoryName>[-_.{}a-zA-Z0-9]+):" +
            @"item:(?<itemName>[-_.{}a-zA-Z0-9]+):" +
            @"itemSet:(?<itemSetName>[-_.{}a-zA-Z0-9]+)\z",
            RegexOptions.CultureInvariant
        );

        private static readonly Regex GradeModelIdRegex = new Regex(
            @"\Agrn:gs2:[-_.{}a-zA-Z0-9]+:[-_.{}a-zA-Z0-9]+:" +
            @"grade:(?<namespaceName>[-_.{}a-zA-Z0-9]+):" +
            @"model:(?<gradeName>[-_.{}a-zA-Z0-9]+)\z",
            RegexOptions.CultureInvariant
        );

        public static string Action() {
            return "Gs2Enhance:UnleashByUserId";
        }

        private static bool TryGetRateModel(
            Gs2.Core.Domain.Gs2 domain,
            UnleashByUserIdRequest request,
            out UnleashRateModel model,
            out bool found
        ) {
            var cached = ((UnleashRateModel)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                request.RateName,
                null
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:enhance:{request.NamespaceName}:" +
                $"unleashRateModel:{request.RateName}";
            found = cached.Item2;
            model = cached.Item1;
            if (!found) {
                model = null;
                return true;
            }
            return model != null && model.UnleashRateModelId == expectedId &&
                   model.Name == request.RateName;
        }

        private static ConsumeAction BuildConsumeAction(
            AccessToken token,
            string materialItemSetId
        ) {
            if (materialItemSetId == null) return null;
            var match = ItemSetIdRegex.Match(materialItemSetId);
            if (!match.Success) return null;
            return new ConsumeAction()
                .WithAction(InventoryConsumeExecutor.Action())
                .WithRequest(new ConsumeItemSetByUserIdRequest()
                    .WithNamespaceName(match.Groups["namespaceName"].Value)
                    .WithInventoryName(match.Groups["inventoryName"].Value)
                    .WithUserId(token.UserId)
                    .WithItemName(match.Groups["itemName"].Value)
                    .WithItemSetName(match.Groups["itemSetName"].Value)
                    .WithConsumeCount(1)
                    .ToJson().ToJson());
        }

        private static bool TryGetGradeStatus(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken token,
            string namespaceName,
            string gradeName,
            string propertyId,
            out GradeStatus status,
            out bool found
        ) {
            var cached = ((GradeStatus)null).GetCache(
                domain.Cache,
                namespaceName,
                token.UserId,
                gradeName,
                propertyId,
                token.TimeOffset
            );
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:grade:{namespaceName}:" +
                $"user:{token.UserId}:gradeModel:{gradeName}:" +
                $"property:{propertyId}";
            found = cached.Item2;
            status = cached.Item1;
            if (!found) {
                status = null;
                return true;
            }
            return status != null && status.StatusId == expectedId &&
                   status.UserId == token.UserId &&
                   status.GradeName == gradeName &&
                   status.PropertyId == propertyId;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            UnleashByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            UnleashByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = UnleashByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) {
                return null;
            }
            if (!TryGetRateModel(
                    domain, prepared, out var rateModel, out var rateFound
                )) {
                return null;
            }
            var rateSnapshot = rateModel?.ToJson().ToJson();
            string gradeNamespace = null;
            string gradeName = null;
            GradeStatus gradeStatus = null;
            var gradeFound = false;
            string gradeSnapshot = null;
            if (rateModel != null) {
                if (rateModel.GradeEntries == null ||
                    rateModel.GradeEntries.Length == 0) {
                    return null;
                }
                var match = GradeModelIdRegex.Match(rateModel.GradeModelId ?? "");
                if (!match.Success) return null;
                gradeNamespace = match.Groups["namespaceName"].Value;
                gradeName = match.Groups["gradeName"].Value;
                if (!TryGetGradeStatus(
                        domain,
                        token,
                        gradeNamespace,
                        gradeName,
                        prepared.TargetItemSetId,
                        out gradeStatus,
                        out gradeFound
                    )) {
                    return null;
                }
                gradeSnapshot = gradeStatus?.ToJson().ToJson();
                if (gradeStatus != null) {
                    UnleashRateEntryModel entry = null;
                    foreach (var candidate in rateModel.GradeEntries) {
                        if (candidate?.GradeValue == gradeStatus.GradeValue) {
                            entry = candidate;
                            break;
                        }
                    }
                    if (entry?.NeedCount != (prepared.Materials?.Length ?? 0)) {
                        return null;
                    }
                }
            }

            var commits = new List<Func<object>>();
            foreach (var material in prepared.Materials ?? Array.Empty<string>()) {
                var action = BuildConsumeAction(token, material);
                foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                    action = action?.ApplyConfig(config.Key, config.Value);
                }
                if (action == null) continue;
                var commit = await InventoryConsumeIndex.ExecuteAsync(
                    domain, token, action, BigInteger.One
                );
                if (commit != null) commits.Add(commit);
            }

            if (gradeStatus != null) {
                AcquireAction action = new AcquireAction()
                    .WithAction(GradeAddExecutor.Action())
                    .WithRequest(new AddGradeByUserIdRequest()
                        .WithNamespaceName(gradeNamespace)
                        .WithUserId(token.UserId)
                        .WithGradeName(gradeName)
                        .WithPropertyId(prepared.TargetItemSetId)
                        .WithGradeValue(1)
                        .ToJson().ToJson());
                foreach (var config in prepared.Config ?? Array.Empty<Config>()) {
                    action = action?.ApplyConfig(config.Key, config.Value);
                }
                if (action != null) {
                    var commit = await GradeAcquireIndex.ExecuteAsync(
                        domain, token, action, BigInteger.One
                    );
                    if (commit != null) commits.Add(commit);
                }
            }
            if (commits.Count == 0) return null;
            var commitAll = Core.SpeculativeExecutor.SpeculativeExecutor
                .BuildAtomicCommit(commits, commits.Count);
            if (commitAll == null) return null;
            return () => {
                var validRateState = TryGetRateModel(
                    domain, prepared, out var liveRate, out var liveRateFound
                );
                if (!validRateState || liveRateFound != rateFound ||
                    liveRate?.ToJson().ToJson() != rateSnapshot) {
                    return null;
                }
                if (rateModel != null) {
                    var validGradeState = TryGetGradeStatus(
                        domain,
                        token,
                        gradeNamespace,
                        gradeName,
                        prepared.TargetItemSetId,
                        out var liveGrade,
                        out var liveGradeFound
                    );
                    if (!validGradeState || liveGradeFound != gradeFound ||
                        liveGrade?.ToJson().ToJson() != gradeSnapshot) {
                        return null;
                    }
                }
                return commitAll();
            };
        }
    }
}
