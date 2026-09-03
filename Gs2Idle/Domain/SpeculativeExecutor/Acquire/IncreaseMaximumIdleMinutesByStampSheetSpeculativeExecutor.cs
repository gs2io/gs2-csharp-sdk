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
/* diff +++ start */
using Gs2.Core.Model;
using Gs2.Core.Net;
/* diff +++ end */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Idle.Model; /* diff +++ */
using Gs2.Gs2Idle.Request;
using Gs2.Gs2Idle.Model.Cache;
using Gs2.Gs2Idle.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Idle.Domain.SpeculativeExecutor
{
    public static class IncreaseMaximumIdleMinutesByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Idle:IncreaseMaximumIdleMinutesByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseMaximumIdleMinutesByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseMaximumIdleMinutesByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Idle.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Status(
                request.CategoryName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                IncreaseMaximumIdleMinutesByUserIdRequest.FromJson(
                    request.ToJson()
                );
            var preparedAccessToken = accessToken?.Clone() as AccessToken;
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
            var now = UnixTime.ToUnixTime(DateTime.Now) +
                      (long)(preparedAccessToken?.TimeOffset ?? 0) * 1000L;
            return MaximumIdleMinutesSpeculativeExecutor.Prepare(
                domain,
                preparedAccessToken,
                prepared?.NamespaceName,
                prepared?.UserId,
                prepared?.CategoryName,
                item => item.SpeculativeExecutionAt(prepared, now)
            );
        }
    }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
    internal sealed class MaximumIdleMinutesSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _categoryName;
        private readonly int? _timeOffset;
        private readonly string _statusId;
        private readonly Func<Status, Status> _transform;

        internal MaximumIdleMinutesSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string categoryName,
            int? timeOffset,
            string statusId,
            Func<Status, Status> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _categoryName = categoryName;
            _timeOffset = timeOffset;
            _statusId = statusId;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "idle",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Status",
            _categoryName
        );

        private bool IsExpected(Status item) {
            return item != null && item.StatusId == _statusId &&
                   item.UserId == _userId &&
                   item.CategoryName == _categoryName;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                Status item;
                if (hasCurrent) {
                    item = current as Status;
                }
                else {
                    var cached = ((Status)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _categoryName,
                        _timeOffset
                    );
                    if (!cached.Item2) {
                        next = null;
                        return false;
                    }
                    item = cached.Item1;
                }
                if (!IsExpected(item)) {
                    next = null;
                    return false;
                }
                var changed = _transform(item);
                next = changed;
                return IsExpected(changed) && changed.Revision == 0;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            catch (Exception) {
                next = null;
                return false;
            }
        }
/* diff +++ end */

/* diff --- start
            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.CategoryName,
                    null
 diff --- end */
/* diff +++ start */
        public object Commit(object value) {
            if (value is Status item && IsExpected(item) && item.Revision == 0) {
                _cache.Put(
                    item.CacheParentKey(
                        _namespaceName,
                        _userId,
                        _timeOffset
                    ),
                    item.CacheKey(_categoryName),
                    item,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
/* diff +++ end */
                );
/* diff +++ start */
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

    internal static class MaximumIdleMinutesSpeculativeExecutor
    {
        internal static Func<object> Prepare(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            string namespaceName,
            string requestUserId,
            string categoryName,
            Func<Status, Status> transform
        ) {
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(accessToken?.UserId) ||
                accessToken.UserId != requestUserId ||
                string.IsNullOrEmpty(namespaceName) ||
                string.IsNullOrEmpty(categoryName) ||
                transform == null) {
/* diff +++ end */
                return null;
/* diff --- start
            };
 diff --- end */
/* diff +++ start */
            }
            var userId = accessToken.UserId;
            var timeOffset = accessToken.TimeOffset;
            var statusId = string.Join(
                ":",
                "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "idle", namespaceName,
                "user", userId,
                "categoryModel", categoryName
            );
            var cached = ((Status)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                categoryName,
                timeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.StatusId != statusId ||
                cached.Item1.UserId != userId ||
                cached.Item1.CategoryName != categoryName) {
                return null;
            }
            return new MaximumIdleMinutesSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                categoryName,
                timeOffset,
                statusId,
                transform
            ).Invoke;
/* diff +++ end */
        }
    }
}
