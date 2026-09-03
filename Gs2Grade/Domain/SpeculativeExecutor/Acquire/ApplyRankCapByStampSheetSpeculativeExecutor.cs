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
using Gs2.Gs2Grade.Model; /* diff +++ */
using Gs2.Gs2Grade.Request;
using Gs2.Gs2Grade.Model.Cache;
using Gs2.Gs2Grade.Model.Transaction;
using Gs2.Gs2Experience.Model.Cache; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Grade.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class ApplyRankCapSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _gradeNamespaceName;
        private readonly string _gradeName;
        private readonly string _experienceNamespaceName;
        private readonly string _experienceName;
        private readonly string _userId;
        private readonly string _propertyId;
        private readonly int? _timeOffset;
        private readonly string _expectedGradeStatusId;
        private readonly string _expectedGradeModelId;
        private readonly string _expectedExperienceModelId;
        private readonly string _expectedExperienceStatusId;

        internal ApplyRankCapSpeculativeCommit(
            CacheDatabase cache,
            string gradeNamespaceName,
            string gradeName,
            string experienceNamespaceName,
            string experienceName,
            string userId,
            string propertyId,
            int? timeOffset,
            string expectedGradeStatusId,
            string expectedGradeModelId,
            string expectedExperienceModelId,
            string expectedExperienceStatusId
        ) {
            _cache = cache;
            _gradeNamespaceName = gradeNamespaceName;
            _gradeName = gradeName;
            _experienceNamespaceName = experienceNamespaceName;
            _experienceName = experienceName;
            _userId = userId;
            _propertyId = propertyId;
            _timeOffset = timeOffset;
            _expectedGradeStatusId = expectedGradeStatusId;
            _expectedGradeModelId = expectedGradeModelId;
            _expectedExperienceModelId = expectedExperienceModelId;
            _expectedExperienceStatusId = expectedExperienceStatusId;
        }

        public string CompositionKey =>
            ((Gs2.Gs2Experience.Model.Status)null).CacheParentKey(
                _experienceNamespaceName, _userId, _timeOffset
            ) + ":" +
            ((Gs2.Gs2Experience.Model.Status)null).CacheKey(
                _experienceName, _propertyId
            );

        private bool TryResolveRankCap(out long rankCapValue) {
            rankCapValue = 0;
            var status = ((Status)null).GetCache(
                _cache, _gradeNamespaceName, _userId, _gradeName,
                _propertyId, _timeOffset
            );
            var model = ((GradeModel)null).GetCache(
                _cache, _gradeNamespaceName, _gradeName, null
            );
            if (!status.Item2 || status.Item1 == null ||
                status.Item1.StatusId != _expectedGradeStatusId ||
                status.Item1.UserId != _userId ||
                status.Item1.GradeName != _gradeName ||
                status.Item1.PropertyId != _propertyId ||
                !status.Item1.GradeValue.HasValue ||
                !model.Item2 || model.Item1 == null ||
                model.Item1.GradeModelId != _expectedGradeModelId ||
                model.Item1.Name != _gradeName ||
                model.Item1.ExperienceModelId != _expectedExperienceModelId ||
                model.Item1.GradeEntries == null ||
                model.Item1.GradeEntries.Length == 0 ||
                status.Item1.GradeValue.Value <= 0) return false;
            var index = (int)Math.Min(
                model.Item1.GradeEntries.LongLength - 1,
                status.Item1.GradeValue.Value - 1
            );
            var entry = model.Item1.GradeEntries[index];
            if (entry == null || !entry.RankCapValue.HasValue) return false;
            rankCapValue = entry.RankCapValue.Value;
            return true;
        }

        private bool IsExpectedExperienceStatus(
            Gs2.Gs2Experience.Model.Status item
        ) {
            return item != null &&
                   item.StatusId == _expectedExperienceStatusId &&
                   item.UserId == _userId &&
                   item.ExperienceName == _experienceName &&
                   item.PropertyId == _propertyId;
        }

        private Gs2.Gs2Experience.Model.ExperienceModel GetExperienceModel() {
            var model =
                ((Gs2.Gs2Experience.Model.ExperienceModel)null).GetCache(
                    _cache, _experienceNamespaceName, _experienceName, null
                );
            return model.Item2 && model.Item1 != null &&
                   model.Item1.ExperienceModelId ==
                       _expectedExperienceModelId &&
                   model.Item1.Name == _experienceName
                ? model.Item1
                : null;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                if (!TryResolveRankCap(out var rankCapValue)) {
                    next = null;
                    return false;
                }
                Gs2.Gs2Experience.Model.Status source;
                if (hasCurrent) {
                    source = current as Gs2.Gs2Experience.Model.Status;
                }
                else {
                    var cached =
                        ((Gs2.Gs2Experience.Model.Status)null).GetCache(
                            _cache, _experienceNamespaceName, _userId,
                            _experienceName, _propertyId, _timeOffset
                        );
                    source = cached.Item1;
                    if (!cached.Item2) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpectedExperienceStatus(source)) {
                    next = null;
                    return false;
                }
                var experienceModel = GetExperienceModel();
                Gs2.Gs2Experience.Model.Status changed;
                if (experienceModel != null && source.ExperienceValue.HasValue) {
                    changed = Gs2.Gs2Experience.Model.ExperienceModelEx
                        .RecalculateStatus(
                            experienceModel,
                            source,
                            source.ExperienceValue.Value,
                            rankCapValue
                        );
                }
                else {
                    changed = source.Clone() as Gs2.Gs2Experience.Model.Status;
                    changed.RankCapValue = rankCapValue;
                    if (rankCapValue <= 1) {
                        changed.ExperienceValue = 0;
                        changed.RankValue = 1;
                        changed.NextRankUpExperienceValue = 0;
                    }
                    changed.Revision = 0;
                }
                if (!IsExpectedExperienceStatus(changed)) {
                    next = null;
                    return false;
                }
                changed.Revision = 0;
                next = changed;
                return true;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Gs2.Gs2Experience.Model.Status item &&
                IsExpectedExperienceStatus(item) && item.Revision == 0) {
                item.PutCache(
                    _cache, _experienceNamespaceName, _userId,
                    _experienceName, _propertyId, _timeOffset
                );
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

/* diff +++ end */
    public static class ApplyRankCapByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Grade:ApplyRankCapByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ApplyRankCapByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ApplyRankCapByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Grade.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Status(
                request.GradeName,
                request.PropertyId
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = ApplyRankCapByUserIdRequest.FromJson(
                request?.ToJson()
            );
            var token = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null || prepared == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared.UserId != token.UserId ||
                prepared.PropertyId == null) return null;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var propertyId = prepared.PropertyId
                .Replace("{region}", region)
                .Replace("{ownerId}", ownerId ?? "")
                .Replace("{userId}", token.UserId);
            var expectedStatusId = $"grn:gs2:{region}:{ownerId}:grade:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:gradeModel:" +
                $"{prepared.GradeName}:property:{propertyId}";
            var expectedModelId = $"grn:gs2:{region}:{ownerId}:grade:" +
                $"{prepared.NamespaceName}:model:{prepared.GradeName}";
/* diff +++ end */

/* diff --- start
            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.GradeName,
                    request.PropertyId,
                    null
                );
 diff --- end */
/* diff +++ start */
            var status = ((Status)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.GradeName, propertyId, token.TimeOffset
            );
            var model = ((GradeModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.GradeName, null
            );
            if (!status.Item2 || status.Item1 == null ||
                status.Item1.StatusId != expectedStatusId ||
                status.Item1.UserId != token.UserId ||
                status.Item1.GradeName != prepared.GradeName ||
                status.Item1.PropertyId != propertyId ||
                !status.Item1.GradeValue.HasValue ||
                !model.Item2 || model.Item1 == null ||
                model.Item1.GradeModelId != expectedModelId ||
                model.Item1.Name != prepared.GradeName ||
                model.Item1.GradeEntries == null ||
                model.Item1.GradeEntries.Length == 0 ||
                status.Item1.GradeValue.Value <= 0) return null;

            var experienceNamespaceName =
                Gs2.Gs2Experience.Model.ExperienceModel
                    .GetNamespaceNameFromGrn(model.Item1.ExperienceModelId);
            var experienceName =
                Gs2.Gs2Experience.Model.ExperienceModel
                    .GetExperienceNameFromGrn(model.Item1.ExperienceModelId);
            var expectedExperienceModelId =
                $"grn:gs2:{region}:{ownerId}:experience:" +
                $"{experienceNamespaceName}:model:{experienceName}";
            if (model.Item1.ExperienceModelId != expectedExperienceModelId)
/* diff +++ end */
                return null;
/* diff --- start
            };
 diff --- end */
/* diff +++ start */
            var expectedExperienceStatusId =
                $"grn:gs2:{region}:{ownerId}:experience:" +
                $"{experienceNamespaceName}:user:{token.UserId}:" +
                $"experienceModel:{experienceName}:property:{propertyId}";
            var speculativeCommit = new ApplyRankCapSpeculativeCommit(
                domain.Cache, prepared.NamespaceName, prepared.GradeName,
                experienceNamespaceName, experienceName, token.UserId,
                propertyId, token.TimeOffset, expectedStatusId,
                expectedModelId, expectedExperienceModelId,
                expectedExperienceStatusId
            );
            if (!speculativeCommit.TryCompose(null, false, out _))
                return null;
            return speculativeCommit.Invoke;
/* diff +++ end */
        }
    }
}
