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
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Model.Cache;
using Gs2.Gs2Stamina.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class StaminaMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _staminaName;
        private readonly int? _timeOffset;
        private readonly string _expectedStaminaId;
        private readonly string _expectedModelId;
        private readonly long? _preparedRevision;
        private readonly bool _usesMaxCapacity;
        private readonly Func<Gs2.Gs2Stamina.Model.Stamina, int?, bool> _transform;

        internal StaminaMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string staminaName,
            int? timeOffset,
            string expectedStaminaId,
            string expectedModelId,
            long? preparedRevision,
            bool usesMaxCapacity,
            Func<Gs2.Gs2Stamina.Model.Stamina, int?, bool> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _staminaName = staminaName;
            _timeOffset = timeOffset;
            _expectedStaminaId = expectedStaminaId;
            _expectedModelId = expectedModelId;
            _preparedRevision = preparedRevision;
            _usesMaxCapacity = usesMaxCapacity;
            _transform = transform;
        }

        public string CompositionKey =>
            ((Gs2.Gs2Stamina.Model.Stamina)null).CacheParentKey(
                _namespaceName, _userId, _timeOffset
            ) + ":" + ((Gs2.Gs2Stamina.Model.Stamina)null)
                .CacheKey(_staminaName);

        private bool IsExpected(Gs2.Gs2Stamina.Model.Stamina item) {
            return item != null && item.StaminaId == _expectedStaminaId &&
                   item.UserId == _userId &&
                   item.StaminaName == _staminaName;
        }

        private bool TryGetMaxCapacity(out int? maxCapacity) {
            var cached = ((Gs2.Gs2Stamina.Model.StaminaModel)null).GetCache(
                _cache, _namespaceName, _staminaName, null
            );
            if (!cached.Item2 || cached.Item1 == null) {
                maxCapacity = null;
                return true;
            }
            if (cached.Item1.StaminaModelId != _expectedModelId ||
                cached.Item1.Name != _staminaName) {
                maxCapacity = null;
                return false;
            }
            maxCapacity = cached.Item1.MaxCapacity;
            return true;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                var maxCapacity = default(int?);
                if (_usesMaxCapacity && !TryGetMaxCapacity(out maxCapacity)) {
                    next = null;
                    return false;
                }
                Gs2.Gs2Stamina.Model.Stamina item;
                if (hasCurrent) {
                    item = current as Gs2.Gs2Stamina.Model.Stamina;
                }
                else {
                    var cached = ((Gs2.Gs2Stamina.Model.Stamina)null).GetCache(
                        _cache, _namespaceName, _userId, _staminaName,
                        _timeOffset
                    );
                    item = cached.Item1;
                    if (!cached.Item2 || !IsExpected(item) ||
                        (item.Revision > 0 &&
                         item.Revision != _preparedRevision)) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpected(item)) {
                    next = null;
                    return false;
                }
                var changed = item.Clone() as Gs2.Gs2Stamina.Model.Stamina;
                if (!_transform(changed, maxCapacity) || !IsExpected(changed)) {
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
            if (value is Gs2.Gs2Stamina.Model.Stamina item &&
                IsExpected(item) && item.Revision == 0) {
                _cache.Put(
                    item.CacheParentKey(
                        _namespaceName, _userId, _timeOffset
                    ),
                    item.CacheKey(_staminaName),
                    item,
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

        internal static int Clamp(long value, int? maxCapacity) {
            if (maxCapacity.HasValue && value > maxCapacity.Value) {
                value = maxCapacity.Value;
            }
            if (value > int.MaxValue) return int.MaxValue;
            if (value < int.MinValue) return int.MinValue;
            return (int)value;
        }
    }

/* diff +++ end */
    public static class RaiseMaxValueByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Stamina:RaiseMaxValueByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RaiseMaxValueByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RaiseMaxValueByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Stamina.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Stamina(
                request.StaminaName
            ).ModelAsync();

            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.StaminaName,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = RaiseMaxValueByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                !prepared.RaiseValue.HasValue) return null;
            var expectedStaminaId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:stamina:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:stamina:{prepared.StaminaName}";
            var expectedModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:stamina:{prepared.NamespaceName}:" +
                $"model:{prepared.StaminaName}";
            var cached = ((Gs2.Gs2Stamina.Model.Stamina)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.StaminaName, token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.StaminaId != expectedStaminaId ||
                cached.Item1.UserId != token.UserId ||
                cached.Item1.StaminaName != prepared.StaminaName ||
                !cached.Item1.MaxValue.HasValue) return null;
            var raiseValue = prepared.RaiseValue.Value;
            return new StaminaMutationSpeculativeCommit(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.StaminaName, token.TimeOffset, expectedStaminaId,
                expectedModelId, cached.Item1.Revision,
                true,
                (current, cap) => {
                    current.MaxValue = StaminaMutationSpeculativeCommit.Clamp(
                        (long)current.MaxValue.Value + raiseValue, cap
                    );
                    return true;
                }
            ).Invoke;
/* diff +++ end */
        }
    }
}
