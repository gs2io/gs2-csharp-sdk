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
using Gs2.Gs2Lottery.Model; /* diff +++ */
using Gs2.Gs2Lottery.Request;
using Gs2.Gs2Lottery.Model.Cache;
using Gs2.Gs2Lottery.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Lottery.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class ResetBoxSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _prizeTableName;
        private readonly int? _timeOffset;
        private readonly string _expectedBoxId;

        internal ResetBoxSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string prizeTableName,
            int? timeOffset,
            string expectedBoxId
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _prizeTableName = prizeTableName;
            _timeOffset = timeOffset;
            _expectedBoxId = expectedBoxId;
        }

        public string CompositionKey => string.Join(
            ":",
            "lottery",
            _namespaceName,
            _userId,
            _timeOffset?.ToString() ?? "0",
            "BoxItems",
            _prizeTableName
        );

        private bool IsExpectedBoxItems(BoxItems item) {
            return item != null &&
                   item.BoxId == _expectedBoxId &&
                   item.UserId == _userId &&
                   item.PrizeTableName == _prizeTableName;
        }

        private object Reset(BoxItems item) {
            if (!IsExpectedBoxItems(item) || !item.IsExecutable(null)) {
                return null;
            }
            var changed = item.SpeculativeExecution(null);
            return IsExpectedBoxItems(changed) ? changed : null;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                if (hasCurrent) {
                    next = Reset(current as BoxItems);
                    return next != null;
                }
                var (cachedItem, find) = ((BoxItems)null).GetCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _prizeTableName,
                    _timeOffset
                );
                next = find ? Reset(cachedItem) : null;
                return next != null;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is BoxItems item && IsExpectedBoxItems(item)) {
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _prizeTableName,
                    _timeOffset
                );
            }
            return null;
        }

        internal bool CanPrepare() {
            return TryCompose(null, false, out _);
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

/* diff +++ end */
    public static class ResetBoxByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Lottery:ResetBoxByUserId";
/* diff +++ start */
        }

        private static bool IsValidRequest(
            ResetBoxByUserIdRequest request,
            AccessToken accessToken
        ) {
            return request != null &&
                   accessToken != null &&
                   !string.IsNullOrEmpty(request.NamespaceName) &&
                   !string.IsNullOrEmpty(request.PrizeTableName) &&
                   !string.IsNullOrEmpty(request.UserId) &&
                   request.UserId == accessToken.UserId;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ResetBoxByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ResetBoxByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Lottery.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).BoxItems(
                request.PrizeTableName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = ResetBoxByUserIdRequest.FromJson(
                request?.ToJson()
            );
            var preparedAccessToken = AccessToken.FromJson(
                accessToken?.ToJson()
            );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
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
                    request.PrizeTableName,
                    null
                );
 diff --- end */
/* diff +++ start */
            if (string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                !IsValidRequest(preparedRequest, preparedAccessToken)) {
/* diff +++ end */
                return null;
/* diff --- start
            };
 diff --- end */
/* diff +++ start */
            }
            var region = domain?.RestSession?.Region.DisplayName();
            var ownerId = domain?.RestSession?.OwnerId;
            if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(ownerId)) {
                return null;
            }
            var namespaceName = preparedRequest.NamespaceName;
            var userId = preparedAccessToken.UserId;
            var prizeTableName = preparedRequest.PrizeTableName;
            var timeOffset = preparedAccessToken.TimeOffset;
            var expectedBoxId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "lottery",
                namespaceName,
                "user",
                userId,
                "box",
                "items",
                prizeTableName
            );
            var speculativeCommit = new ResetBoxSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                prizeTableName,
                timeOffset,
                expectedBoxId
            );
            return speculativeCommit.CanPrepare()
                ? speculativeCommit.Invoke
                : null;
/* diff +++ end */
        }
    }
}
