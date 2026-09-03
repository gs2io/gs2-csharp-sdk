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
using System.Collections.Generic; /* diff +++ */
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Model.Transaction;
using Gs2.Gs2Mission.Model; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class CounterMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private sealed class CounterCompositionState
        {
            internal Counter Item { get; }
            internal bool Dirty { get; }

            internal CounterCompositionState(Counter item, bool dirty) {
                Item = item;
                Dirty = dirty;
            }
        }

        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _counterName;
        private readonly int? _timeOffset;
        private readonly string _expectedId;
        private readonly long? _preparedRevision;
        private readonly Func<Counter, Counter> _transform;

        internal CounterMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string counterName,
            int? timeOffset,
            string expectedId,
            long? preparedRevision,
            Func<Counter, Counter> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _counterName = counterName;
            _timeOffset = timeOffset;
            _expectedId = expectedId;
            _preparedRevision = preparedRevision;
            _transform = transform;
        }

        public string CompositionKey => ((Counter)null).CacheParentKey(
            _namespaceName, _userId, _timeOffset
        ) + ":" + ((Counter)null).CacheKey(_counterName);

        private bool IsExpected(Counter item) {
            return item != null && item.CounterId == _expectedId &&
                   item.UserId == _userId && item.Name == _counterName;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                Counter source;
                var dirty = false;
                if (hasCurrent) {
                    if (current is not CounterCompositionState state ||
                        !IsExpected(state.Item)) {
                        next = null;
                        return false;
                    }
                    source = state.Item;
                    dirty = state.Dirty;
                }
                else {
                    var cached = ((Counter)null).GetCache(
                        _cache, _namespaceName, _userId, _counterName,
                        _timeOffset
                    );
                    source = cached.Item2 ? cached.Item1 : null;
                    if (source?.Revision > 0 &&
                        source.Revision != _preparedRevision) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpected(source)) {
                    next = null;
                    return false;
                }
                var changed = _transform(source);
                if (ReferenceEquals(changed, source)) {
                    next = new CounterCompositionState(source, dirty);
                    return true;
                }
                if (!IsExpected(changed)) {
                    next = null;
                    return false;
                }
                changed.Revision = 0;
                next = new CounterCompositionState(changed, true);
                return true;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is CounterCompositionState state && state.Dirty &&
                IsExpected(state.Item) && state.Item.Revision == 0) {
                state.Item.PutCache(
                    _cache, _namespaceName, _userId, _counterName, _timeOffset
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
    public static class IncreaseCounterByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:IncreaseCounterByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseCounterByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseCounterByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Mission.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Counter(
                request.CounterName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var token = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            var prepared = request == null
                ? null
                : new IncreaseCounterByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithCounterName(request.CounterName)
                    .WithUserId(request.UserId)
                    .WithValue(request.Value);
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.CounterName,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.CounterName) ||
                !prepared.Value.HasValue) return null;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:counter:" +
                prepared.CounterName;
            var cached = ((Counter)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.CounterName,
                token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.CounterId != expectedId ||
                cached.Item1.UserId != token.UserId ||
                cached.Item1.Name != prepared.CounterName ||
                cached.Item1.Values == null) return null;
            var logicalTimeMillis = UnixTime.ToUnixTime(DateTime.Now) +
                                    (long)(token.TimeOffset ?? 0) * 1000L;
            var commit = new CounterMutationSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.CounterName,
                token.TimeOffset,
                expectedId,
                cached.Item1.Revision,
                current => {
                    var changed = current.Clone() as Counter;
                    var changedAny = false;
                    var values = new List<ScopedValue>();
                    foreach (var value in current.Values) {
                        if (value == null) continue;
                        var next = value.Clone() as ScopedValue;
                        if (next == null) continue;
                        values.Add(next);
                        if (next.ScopeType != "resetTiming" ||
                            next.NextResetAt.HasValue &&
                            next.NextResetAt <= logicalTimeMillis ||
                            !next.Value.HasValue) continue;
                        changedAny = true;
                        var increased = new BigInteger(next.Value.Value) +
                                        prepared.Value.Value;
                        next.Value = increased > 9223372036854775805L
                            ? 9223372036854775805L
                            : (long)increased;
                        next.UpdatedAt = logicalTimeMillis;
                    }
                    if (!changedAny) return current;
                    changed.Values = values.ToArray();
                    changed.UpdatedAt = logicalTimeMillis;
                    changed.Revision = 0;
                    return changed;
                }
            );
            return commit.Invoke;
/* diff +++ end */
        }
    }
}
