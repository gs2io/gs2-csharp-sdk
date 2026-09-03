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
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class MoldCapacityMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _moldModelName;
        private readonly int? _timeOffset;
        private readonly string _expectedMoldId;
        private readonly string _expectedModelId;
        private readonly long? _preparedRevision;
        private readonly int? _preparedMaxCapacity;
        private readonly Func<Gs2.Gs2Formation.Model.Mold,
            Gs2.Gs2Formation.Model.Mold> _transform;

        internal MoldCapacityMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? timeOffset,
            string expectedMoldId,
            string expectedModelId,
            long? preparedRevision,
            int? preparedMaxCapacity,
            Func<Gs2.Gs2Formation.Model.Mold,
                Gs2.Gs2Formation.Model.Mold> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _moldModelName = moldModelName;
            _timeOffset = timeOffset;
            _expectedMoldId = expectedMoldId;
            _expectedModelId = expectedModelId;
            _preparedRevision = preparedRevision;
            _preparedMaxCapacity = preparedMaxCapacity;
            _transform = transform;
        }

        public string CompositionKey =>
            ((Gs2.Gs2Formation.Model.Mold)null).CacheParentKey(
                _namespaceName, _userId, _timeOffset
            ) + ":" + ((Gs2.Gs2Formation.Model.Mold)null)
                .CacheKey(_moldModelName);

        private bool IsExpectedMold(Gs2.Gs2Formation.Model.Mold mold) {
            return mold != null && mold.MoldId == _expectedMoldId &&
                   mold.Name == _moldModelName && mold.UserId == _userId;
        }

        private bool IsExpectedModel(Gs2.Gs2Formation.Model.MoldModel model) {
            return model != null && model.MoldModelId == _expectedModelId &&
                   model.Name == _moldModelName &&
                   model.MaxCapacity == _preparedMaxCapacity &&
                   model.MaxCapacity.HasValue;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                var modelCache = ((Gs2.Gs2Formation.Model.MoldModel)null)
                    .GetCache(
                        _cache, _namespaceName, _moldModelName, null
                    );
                if (!modelCache.Item2 || !IsExpectedModel(modelCache.Item1)) {
                    next = null;
                    return false;
                }
                Gs2.Gs2Formation.Model.Mold mold;
                if (hasCurrent) {
                    mold = current as Gs2.Gs2Formation.Model.Mold;
                }
                else {
                    var moldCache = ((Gs2.Gs2Formation.Model.Mold)null)
                        .GetCache(
                            _cache, _namespaceName, _userId,
                            _moldModelName, _timeOffset
                        );
                    mold = moldCache.Item1;
                    if (!moldCache.Item2 || !IsExpectedMold(mold) ||
                        (mold.Revision > 0 &&
                         mold.Revision != _preparedRevision)) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpectedMold(mold)) {
                    next = null;
                    return false;
                }
                var changed = _transform(mold);
                if (!IsExpectedMold(changed) ||
                    !changed.Capacity.HasValue || changed.Capacity.Value < 0 ||
                    changed.Capacity.Value > modelCache.Item1.MaxCapacity.Value) {
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
            if (value is Gs2.Gs2Formation.Model.Mold mold &&
                IsExpectedMold(mold) && mold.Capacity.HasValue &&
                mold.Capacity.Value >= 0 && mold.Revision == 0) {
                _cache.Put(
                    mold.CacheParentKey(
                        _namespaceName, _userId, _timeOffset
                    ),
                    mold.CacheKey(_moldModelName),
                    mold,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
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
    public static class AddMoldCapacityByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Formation:AddMoldCapacityByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddMoldCapacityByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddMoldCapacityByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Formation.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Mold(
                request.MoldModelName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = AddMoldCapacityByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                !prepared.Capacity.HasValue) {
                return null;
            }
            var expectedMoldId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:formation:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:mold:{prepared.MoldModelName}";
            var expectedModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:formation:{prepared.NamespaceName}:" +
                $"model:mold:{prepared.MoldModelName}";
            var moldCache = ((Gs2.Gs2Formation.Model.Mold)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.MoldModelName, token.TimeOffset
            );
            var modelCache = ((Gs2.Gs2Formation.Model.MoldModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.MoldModelName, null
            );
            if (!IsUsable(
                    moldCache.Item1, modelCache.Item1,
                    moldCache.Item2, modelCache.Item2,
                    expectedMoldId, expectedModelId, token.UserId,
                    prepared.MoldModelName
                )) {
                return null;
            }
            var preparedRevision = moldCache.Item1.Revision;
            var preparedMaxCapacity = modelCache.Item1.MaxCapacity;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            return new MoldCapacityMutationSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.MoldModelName,
                token.TimeOffset,
                expectedMoldId,
                expectedModelId,
                preparedRevision,
                preparedMaxCapacity,
                current => {
                    if (!current.Capacity.HasValue) return null;
                    var value = (long)current.Capacity.Value +
                                prepared.Capacity.Value;
                    if (value > int.MaxValue || value < int.MinValue) return null;
                    var changed = current.Clone() as
                        Gs2.Gs2Formation.Model.Mold;
                    changed.Capacity = (int)value;
                    return changed;
                }
            ).Invoke;
        }
/* diff +++ end */

/* diff --- start
            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.MoldModelName,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
        private static bool IsUsable(
            Gs2.Gs2Formation.Model.Mold mold,
            Gs2.Gs2Formation.Model.MoldModel model,
            bool foundMold,
            bool foundModel,
            string expectedMoldId,
            string expectedModelId,
            string userId,
            string moldModelName
        ) {
            return foundMold && foundModel && mold != null && model != null &&
                   mold.MoldId == expectedMoldId && mold.Name == moldModelName &&
                   mold.UserId == userId && mold.Capacity.HasValue &&
                   model.MoldModelId == expectedModelId &&
                   model.Name == moldModelName && model.MaxCapacity.HasValue;
/* diff +++ end */
        }
    }
}
