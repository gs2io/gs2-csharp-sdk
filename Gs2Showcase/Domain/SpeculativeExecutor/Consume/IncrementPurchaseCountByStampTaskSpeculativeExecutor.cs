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
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Showcase.Model; /* diff +++ */
using Gs2.Gs2Showcase.Request;
using Gs2.Gs2Showcase.Model.Cache;
using Gs2.Gs2Showcase.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Showcase.Domain.SpeculativeExecutor
{
    public static class IncrementPurchaseCountByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Showcase:IncrementPurchaseCountByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncrementPurchaseCountByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncrementPurchaseCountByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null
                ? null
                : IncrementPurchaseCountByUserIdRequest.FromJson(
                    request.ToJson()
                );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = accessToken?.UserId;
            }
            if (preparedRequest == null) {
                return null;
            }
            return await PurchaseCountSpeculativeExecutor.PrepareAsync(
                domain,
                accessToken,
                preparedRequest.NamespaceName,
                preparedRequest.UserId,
                preparedRequest.ShowcaseName,
                preparedRequest.DisplayItemName,
                item => item.SpeculativeExecution(preparedRequest)
            );
        }
    }

    internal sealed class PurchaseCountSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _showcaseName;
        private readonly string _displayItemName;
        private readonly int? _timeOffset;
        private readonly RandomDisplayItem _preparedItem;
        private readonly Func<RandomDisplayItem, RandomDisplayItem> _transform;

        internal PurchaseCountSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemName,
            int? timeOffset,
            RandomDisplayItem preparedItem,
            Func<RandomDisplayItem, RandomDisplayItem> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _showcaseName = showcaseName;
            _displayItemName = displayItemName;
            _timeOffset = timeOffset;
            _preparedItem = preparedItem;
            _transform = transform;
        }

        public string CompositionKey => string.Join(
            ":",
            "showcase",
            _namespaceName,
            _userId,
            _showcaseName,
            _timeOffset?.ToString() ?? "0",
            "RandomDisplayItem",
            _displayItemName
        );

        private bool IsExpected(RandomDisplayItem item) {
            return item != null &&
                   item.ShowcaseName == _showcaseName &&
                   item.Name == _displayItemName;
        }

        public bool TryCompose(
            object current,
            bool hasCurrent,
            out object next
        ) {
            try {
                RandomDisplayItem item;
                if (hasCurrent) {
                    item = current as RandomDisplayItem;
                }
                else {
                    var cached = ((RandomDisplayItem)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _showcaseName,
                        _displayItemName,
                        _timeOffset
                    );
                    if (!cached.Item2 ||
                        !ReferenceEquals(cached.Item1, _preparedItem)) {
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
                return IsExpected(changed);
            }
            catch (Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is not RandomDisplayItem item || !IsExpected(item)) {
                return null;
            }
            var current = ((RandomDisplayItem)null).GetCache(
                _cache,
                _namespaceName,
                _userId,
                _showcaseName,
                _displayItemName,
                _timeOffset
            );
            if (!current.Item2 ||
                !ReferenceEquals(current.Item1, _preparedItem)) {
                return null;
            }
            item.PutCache(
                _cache,
                _namespaceName,
                _userId,
                _showcaseName,
                _displayItemName,
                _timeOffset
            );
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

    internal static class PurchaseCountSpeculativeExecutor
    {
#if GS2_ENABLE_UNITASK
        internal static async UniTask<Func<object>> PrepareAsync(
#else
        internal static async Task<Func<object>> PrepareAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            string namespaceName,
            string requestUserId,
            string showcaseName,
            string displayItemName,
            Func<RandomDisplayItem, RandomDisplayItem> transform
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                token.UserId != requestUserId || transform == null) {
                return null;
            }

            var userId = token.UserId;
            var timeOffset = token.TimeOffset;
            var cached = ((RandomDisplayItem)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                showcaseName,
                displayItemName,
                timeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.ShowcaseName != showcaseName ||
                cached.Item1.Name != displayItemName) {
                return null;
            }

            return new PurchaseCountSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                showcaseName,
                displayItemName,
                timeOffset,
                cached.Item1,
                transform
            ).Invoke;
/* diff +++ end */
        }
    }
}
