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
using Gs2.Gs2Inventory.Request;
using Gs2.Gs2Inventory.Model.Cache;
using Gs2.Gs2Inventory.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal static class BigItemSpeculativeState
    {
        internal static Gs2.Gs2Inventory.Model.BigItem KnownZero(
            string expectedId,
            string userId,
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Model.BigItem()
                .WithItemId(expectedId)
                .WithUserId(userId)
                .WithItemName(itemName)
                .WithCount("0")
                .WithRevision(0);
        }
    }

    internal sealed class BigItemMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly int? _timeOffset;
        private readonly string _expectedId;
        private readonly long? _preparedRevision;
        private readonly bool _preparedWasTombstone;
        private readonly Func<Gs2.Gs2Inventory.Model.BigItem,
            Gs2.Gs2Inventory.Model.BigItem> _transform;

        internal BigItemMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            int? timeOffset,
            string expectedId,
            long? preparedRevision,
            bool preparedWasTombstone,
            Func<Gs2.Gs2Inventory.Model.BigItem,
                Gs2.Gs2Inventory.Model.BigItem> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _inventoryName = inventoryName;
            _itemName = itemName;
            _timeOffset = timeOffset;
            _expectedId = expectedId;
            _preparedRevision = preparedRevision;
            _preparedWasTombstone = preparedWasTombstone;
            _transform = transform;
        }

        public string CompositionKey => ((Gs2.Gs2Inventory.Model.BigItem)null)
            .CacheParentKey(
                _namespaceName, _userId, _inventoryName, _timeOffset
            ) + ":" + ((Gs2.Gs2Inventory.Model.BigItem)null).CacheKey(_itemName);

        private bool IsExpected(Gs2.Gs2Inventory.Model.BigItem item) {
            return item != null && item.ItemId == _expectedId &&
                   item.UserId == _userId && item.ItemName == _itemName;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                Gs2.Gs2Inventory.Model.BigItem item;
                if (hasCurrent) {
                    item = current as Gs2.Gs2Inventory.Model.BigItem;
                }
                else {
                    var cached = ((Gs2.Gs2Inventory.Model.BigItem)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _inventoryName,
                        _itemName,
                        _timeOffset
                    );
                    if (!cached.Item2 ||
                        (cached.Item1 == null && !_preparedWasTombstone)) {
                        next = null;
                        return false;
                    }
                    item = cached.Item1 ?? BigItemSpeculativeState.KnownZero(
                        _expectedId, _userId, _itemName
                    );
                    if (!IsExpected(item) ||
                        (item.Revision > 0 && item.Revision != _preparedRevision)) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpected(item)) {
                    next = null;
                    return false;
                }
                next = _transform(item);
                return IsExpected(next as Gs2.Gs2Inventory.Model.BigItem) &&
                       ((Gs2.Gs2Inventory.Model.BigItem)next).Count != null;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Gs2.Gs2Inventory.Model.BigItem item &&
                IsExpected(item) && item.Count != null) {
                item.Revision = 0;
                _cache.Put(
                    item.CacheParentKey(
                        _namespaceName, _userId, _inventoryName, _timeOffset
                    ),
                    item.CacheKey(_itemName),
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
    }

/* diff +++ end */
    public static class AcquireBigItemByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:AcquireBigItemByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireBigItemByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireBigItemByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).BigInventory(
                request.InventoryName
            ).BigItem(
                request.ItemName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = AcquireBigItemByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                prepared.NamespaceName == null ||
                prepared.InventoryName == null ||
                prepared.ItemName == null ||
                prepared.AcquireCount == null) {
                return null;
            }
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:big:inventory:{prepared.InventoryName}:" +
                $"item:{prepared.ItemName}";
            var cached = ((Gs2.Gs2Inventory.Model.BigItem)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.InventoryName,
                prepared.ItemName,
                token.TimeOffset
            );
            var preparedWasTombstone = cached.Item1 == null;
            var item = cached.Item1 ?? BigItemSpeculativeState.KnownZero(
                expectedId, token.UserId, prepared.ItemName
            );
            if (!IsUsable(item, cached.Item2, expectedId, token.UserId,
                    prepared.ItemName) || item.Count == null) {
                return null;
            }
            var preparedRevision = item.Revision;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            return new BigItemMutationSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.InventoryName,
                prepared.ItemName,
                token.TimeOffset,
                expectedId,
                preparedRevision,
                preparedWasTombstone,
                current => {
                    if (!TryAdd(
                            current.Count,
                            prepared.AcquireCount,
                            out var count
                        )) return null;
                    var changed = current.Clone() as Gs2.Gs2Inventory.Model.BigItem;
                    changed.Count = count;
                    changed.Revision = 0;
                    return changed;
                }
            ).Invoke;
        }

        private static bool IsUsable(
            Gs2.Gs2Inventory.Model.BigItem item,
            bool found,
            string expectedId,
            string userId,
            string itemName
        ) {
            return found && item != null && item.ItemId == expectedId &&
                   item.UserId == userId && item.ItemName == itemName;
        }

        private static bool TryAdd(string left, string right, out string result) {
            result = null;
            if (!BigInteger.TryParse(left, out var leftValue) ||
                !BigInteger.TryParse(right, out var rightValue)) {
                return false;
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
                    request.InventoryName,
                    request.ItemName,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            result = (leftValue + rightValue).ToString("D");
            return true;
/* diff +++ end */
        }
    }
}
