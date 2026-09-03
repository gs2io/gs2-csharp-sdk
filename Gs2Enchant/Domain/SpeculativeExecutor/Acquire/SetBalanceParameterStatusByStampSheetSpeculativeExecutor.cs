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
using System.Linq; /* diff +++ */
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Enchant.Model; /* diff +++ */
using Gs2.Gs2Enchant.Request;
using Gs2.Gs2Enchant.Model.Cache;
using Gs2.Gs2Enchant.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Enchant.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class BalanceParameterStatusSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _parameterName;
        private readonly string _propertyId;
        private readonly int? _timeOffset;
        private readonly string _expectedStatusId;
        private readonly BalanceParameterStatus _preparedItem;
        private readonly Func<BalanceParameterStatus, BalanceParameterStatus>
            _transform;

        internal BalanceParameterStatusSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string parameterName,
            string propertyId,
            int? timeOffset,
            string expectedStatusId,
            BalanceParameterStatus preparedItem,
            Func<BalanceParameterStatus, BalanceParameterStatus> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _parameterName = parameterName;
            _propertyId = propertyId;
            _timeOffset = timeOffset;
            _expectedStatusId = expectedStatusId;
            _preparedItem = preparedItem;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "enchant",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "BalanceParameterStatus",
            _parameterName,
            _propertyId
        );

        private bool IsExpectedStatus(BalanceParameterStatus item) {
            return item != null &&
                   item.BalanceParameterStatusId == _expectedStatusId &&
                   item.UserId == _userId &&
                   item.ParameterName == _parameterName &&
                   item.PropertyId == _propertyId;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                if (hasCurrent) {
                    if (current is not BalanceParameterStatus currentStatus ||
                        !IsExpectedStatus(currentStatus)) {
                        next = null;
                        return false;
                    }
                    next = _transform(currentStatus);
                    return IsExpectedStatus(next as BalanceParameterStatus);
                }
                var (cachedItem, find) = ((BalanceParameterStatus)null).GetCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _parameterName,
                    _propertyId,
                    _timeOffset
                );
                if (!find) {
                    next = null;
                    return false;
                }
                if (find && cachedItem != null &&
                    !IsExpectedStatus(cachedItem)) {
                    next = null;
                    return false;
                }
                next = cachedItem == null
                    ? _preparedItem
                    : _transform(cachedItem);
                return IsExpectedStatus(next as BalanceParameterStatus);
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is BalanceParameterStatus item &&
                IsExpectedStatus(item)) {
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _parameterName,
                    _propertyId,
                    _timeOffset
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
    public static class SetBalanceParameterStatusByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Enchant:SetBalanceParameterStatusByUserId";
/* diff +++ start */
        }

        private static SetBalanceParameterStatusByUserIdRequest Snapshot(
            SetBalanceParameterStatusByUserIdRequest request
        ) {
            if (request?.ParameterValues == null) {
                return null;
            }
            return new SetBalanceParameterStatusByUserIdRequest()
                .WithNamespaceName(request.NamespaceName)
                .WithUserId(request.UserId)
                .WithParameterName(request.ParameterName)
                .WithPropertyId(request.PropertyId)
                .WithParameterValues(request.ParameterValues
                    .Where(value => value != null)
                    .Select(value => value.Clone() as BalanceParameterValue)
                    .ToArray())
                .WithTimeOffsetToken(request.TimeOffsetToken);
        }

        private static bool HasCacheKey(
            SetBalanceParameterStatusByUserIdRequest request,
            AccessToken accessToken
        ) {
            if (request == null || accessToken == null ||
                string.IsNullOrEmpty(request.NamespaceName) ||
                string.IsNullOrEmpty(request.UserId) ||
                request.UserId != accessToken.UserId ||
                string.IsNullOrEmpty(request.ParameterName) ||
                string.IsNullOrEmpty(request.PropertyId) ||
                request.ParameterValues == null ||
                request.ParameterValues.Length == 0) {
                return false;
            }
            return true;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetBalanceParameterStatusByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetBalanceParameterStatusByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Enchant.Namespace(
                request.NamespaceName
 diff --- end */
/* diff +++ start */
            var preparedRequest = Snapshot(request);
            var preparedAccessToken = AccessToken.FromJson(
                accessToken?.ToJson()
            );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (string.IsNullOrEmpty(preparedAccessToken?.UserId)) {
                return null;
            }
            if (string.IsNullOrEmpty(preparedRequest?.PropertyId)) {
                return null;
            }
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var statusDomain = domain.Enchant.Namespace(
                preparedRequest?.NamespaceName
/* diff +++ end */
            ).AccessToken(
/* diff --- start
                accessToken
 diff --- end */
                preparedAccessToken /* diff +++ */
            ).BalanceParameterStatus(
/* diff --- start
                request.ParameterName,
                request.PropertyId
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
                preparedRequest?.ParameterName,
                preparedRequest?.PropertyId
            );
            preparedRequest.PropertyId = statusDomain.PropertyId;
            if (!HasCacheKey(preparedRequest, preparedAccessToken)) {
                return null;
            }
            var namespaceName = preparedRequest.NamespaceName;
            var userId = preparedAccessToken.UserId;
            var parameterName = preparedRequest.ParameterName;
            var propertyId = preparedRequest.PropertyId;
            var timeOffset = preparedAccessToken.TimeOffset;
            var currentTimeMillis = UnixTime.ToUnixTime(DateTime.Now) +
                                    (long)(timeOffset ?? 0) * 1000L;
            var expectedStatusId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "enchant",
                namespaceName,
                "user",
                userId,
                "balance",
                parameterName,
                propertyId
            );
            BalanceParameterStatus Transform(BalanceParameterStatus source) {
                var changed = source.SpeculativeExecution(preparedRequest);
                changed.UpdatedAt = currentTimeMillis;
                return changed;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var cached = ((BalanceParameterStatus)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                parameterName,
                propertyId,
                timeOffset
            );
            if (!cached.Item2) {
                return null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
            var item = cached.Item1; /* diff +++ */

/* diff --- start
            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.ParameterName,
                    request.PropertyId,
                    null
                );
 diff --- end */
/* diff +++ start */
            if (item != null &&
                (item.BalanceParameterStatusId != expectedStatusId ||
                 item.UserId != userId ||
                 item.ParameterName != parameterName ||
                 item.PropertyId != propertyId)) {
/* diff +++ end */
                return null;
/* diff --- start
            };
 diff --- end */
/* diff +++ start */
            }
            var source = item ?? new BalanceParameterStatus()
                .WithBalanceParameterStatusId(expectedStatusId)
                .WithUserId(userId)
                .WithParameterName(parameterName)
                .WithPropertyId(propertyId)
                .WithParameterValues(Array.Empty<BalanceParameterValue>())
                .WithCreatedAt(currentTimeMillis)
                .WithUpdatedAt(currentTimeMillis)
                .WithRevision(0);
            var preparedItem = Transform(source);
            var speculativeCommit = new BalanceParameterStatusSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                parameterName,
                propertyId,
                timeOffset,
                expectedStatusId,
                preparedItem,
                Transform
            );
            return speculativeCommit.Invoke;
/* diff +++ end */
        }
    }
}
