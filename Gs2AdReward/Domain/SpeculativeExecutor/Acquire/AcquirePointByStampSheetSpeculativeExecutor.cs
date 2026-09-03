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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2AdReward.Model; /* diff +++ */
using Gs2.Gs2AdReward.Request;
using Gs2.Gs2AdReward.Model.Cache;
using Gs2.Gs2AdReward.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2AdReward.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class PointSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly int? _timeOffset;
        private readonly string _expectedPointId;
        private readonly long? _preparedRevision;
        private readonly Func<Point, Point> _transform;

        internal PointSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            int? timeOffset,
            string expectedPointId,
            long? preparedRevision,
            Func<Point, Point> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _timeOffset = timeOffset;
            _expectedPointId = expectedPointId;
            _preparedRevision = preparedRevision;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "adReward",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "Point",
            "Singleton"
        );

        private bool IsExpectedPoint(Point item) {
            return item != null &&
                   item.PointId == _expectedPointId &&
                   item.UserId == _userId;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                if (hasCurrent) {
                    if (current is not Point currentPoint ||
                        !IsExpectedPoint(currentPoint)) {
                        next = null;
                        return false;
                    }
                    next = _transform(currentPoint);
                    return next is Point composed &&
                           IsExpectedPoint(composed) && composed.Revision == 0;
                }
                var (cachedItem, find) = ((Point)null).GetCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _timeOffset
                );
                if (!find || !IsExpectedPoint(cachedItem)) {
                    next = null;
                    return false;
                }
                if (cachedItem.Revision > 0 &&
                    cachedItem.Revision != _preparedRevision) {
                    next = null;
                    return false;
                }
                next = _transform(cachedItem);
                return next is Point transformed &&
                       IsExpectedPoint(transformed) &&
                       transformed.Revision == 0;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Point item && IsExpectedPoint(item) &&
                item.Revision == 0) {
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
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
    public static class AcquirePointByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2AdReward:AcquirePointByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquirePointByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquirePointByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.AdReward.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Point(
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null
                ? null
                : new AcquirePointByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithPoint(request.Point);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(preparedRequest.NamespaceName) ||
                !preparedRequest.Point.HasValue) return null;
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
            var currentTimeMillis = UnixTime.ToUnixTime(DateTime.Now) +
                                    (long)(timeOffset ?? 0) * 1000L;
            var expectedPointId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "adReward",
                preparedRequest.NamespaceName,
                "user",
                userId,
                "point"
            );
            var (item, find) = ((Point)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                timeOffset
            );
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            if (!find || item == null || item.PointId != expectedPointId ||
                item.UserId != userId) {
                return null;
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
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            var speculativeCommit = new PointSpeculativeCommit(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                timeOffset,
                expectedPointId,
                item.Revision,
                cachedItem => cachedItem.SpeculativeExecutionAt(
                        preparedRequest,
                        currentTimeMillis
                    )
            );
            return speculativeCommit.Invoke;
/* diff +++ end */
        }
    }
}
