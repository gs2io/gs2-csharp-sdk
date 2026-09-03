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
using System.Collections.Generic;
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
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
    internal sealed class ItemSetAcquireSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;
        private readonly int? _timeOffset;
        private readonly string _expectedItemSetId;
        private readonly string _expectedItemModelId;
        private readonly string _preparedItem;
        private readonly string[] _preparedAggregate;
        private readonly long _acquireCount;
        private readonly long _stackingLimit;
        private readonly int _sortValue;
        private readonly long? _expiresAt;
        private readonly long _updatedAt;

        internal ItemSetAcquireSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            int? timeOffset,
            string expectedItemSetId,
            string expectedItemModelId,
            string preparedItem,
            string[] preparedAggregate,
            long acquireCount,
            long stackingLimit,
            int sortValue,
            long? expiresAt,
            long updatedAt
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _inventoryName = inventoryName;
            _itemName = itemName;
            _itemSetName = itemSetName;
            _timeOffset = timeOffset;
            _expectedItemSetId = expectedItemSetId;
            _expectedItemModelId = expectedItemModelId;
            _preparedItem = preparedItem;
            _preparedAggregate = preparedAggregate;
            _acquireCount = acquireCount;
            _stackingLimit = stackingLimit;
            _sortValue = sortValue;
            _expiresAt = expiresAt;
            _updatedAt = updatedAt;
        }

        public string CompositionKey => ((Gs2.Gs2Inventory.Model.ItemSet)null)
            .CacheParentKey(_namespaceName, _userId, _inventoryName, _timeOffset) +
            ":" + ((Gs2.Gs2Inventory.Model.ItemSet)null)
                .CacheKey(_itemName, _itemSetName);

        private bool IsExpected(Gs2.Gs2Inventory.Model.ItemSet item) {
            return item != null && item.ItemSetId == _expectedItemSetId &&
                   item.Name == _itemSetName && item.UserId == _userId &&
                   item.InventoryName == _inventoryName &&
                   item.ItemName == _itemName && item.Count.HasValue;
        }

        private bool IsExpected(Gs2.Gs2Inventory.Model.ItemModel model) {
            return model != null && model.ItemModelId == _expectedItemModelId &&
                   model.Name == _itemName &&
                   model.StackingLimit == _stackingLimit &&
                   model.SortValue == _sortValue;
        }

        private bool TryGetExpectedAggregate(
            out Gs2.Gs2Inventory.Model.ItemSet[] aggregate,
            out int index
        ) {
            var cached = ((Gs2.Gs2Inventory.Model.ItemSet[])null).GetCache(
                _cache, _namespaceName, _userId, _inventoryName,
                _itemName, _timeOffset
            );
            aggregate = cached.Item1;
            index = -1;
            if (!cached.Item2 || aggregate == null ||
                (_preparedAggregate != null &&
                 aggregate.Length != _preparedAggregate.Length)) {
                return false;
            }
            var names = new HashSet<string>();
            for (var i = 0; i < aggregate.Length; i++) {
                var candidate = aggregate[i];
                if (candidate == null || string.IsNullOrEmpty(candidate.Name) ||
                    !candidate.Count.HasValue || !names.Add(candidate.Name)) {
                    return false;
                }
                var serialized = candidate.ToJson().ToJson();
                if (_preparedAggregate != null &&
                    serialized != _preparedAggregate[i]) {
                    return false;
                }
                var direct = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                    _cache, _namespaceName, _userId, _inventoryName,
                    _itemName, candidate.Name, _timeOffset
                );
                if (!direct.Item2 || direct.Item1 == null ||
                    direct.Item1.ToJson().ToJson() != serialized) {
                    return false;
                }
                if (candidate.Name == _itemSetName) {
                    if (!IsExpected(candidate) || serialized != _preparedItem) {
                        return false;
                    }
                    index = i;
                }
            }
            return index >= 0;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                if (_preparedAggregate != null) {
                    if (!TryGetExpectedAggregate(out _, out _)) {
                        next = null;
                        return false;
                    }
                }
                Gs2.Gs2Inventory.Model.ItemSet item;
                if (hasCurrent) {
                    item = current as Gs2.Gs2Inventory.Model.ItemSet;
                }
                else {
                    var cached = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                        _cache, _namespaceName, _userId, _inventoryName,
                        _itemName, _itemSetName, _timeOffset
                    );
                    item = cached.Item1;
                    if (!cached.Item2 || !IsExpected(item) ||
                        item.ToJson().ToJson() != _preparedItem) {
                        next = null;
                        return false;
                    }
                }
                var cachedModel = ((Gs2.Gs2Inventory.Model.ItemModel)null).GetCache(
                    _cache, _namespaceName, _inventoryName, _itemName, null
                );
                if (!IsExpected(item) || !cachedModel.Item2 ||
                    !IsExpected(cachedModel.Item1) ||
                    (_expiresAt.HasValue && item.ExpiresAt != _expiresAt)) {
                    next = null;
                    return false;
                }
                var count = checked(item.Count.Value + _acquireCount);
                var changed = item.Clone() as Gs2.Gs2Inventory.Model.ItemSet;
                changed.Count = Math.Min(count, _stackingLimit);
                changed.SortValue = _sortValue;
                changed.UpdatedAt = _updatedAt;
                next = changed;
                return IsExpected(changed);
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Gs2.Gs2Inventory.Model.ItemSet item && IsExpected(item)) {
                var updateAggregate = TryGetExpectedAggregate(
                    out var aggregate,
                    out var aggregateIndex
                );
                if (_preparedAggregate != null && !updateAggregate) {
                    return null;
                }
                item.PutCache(
                    _cache, _namespaceName, _userId, _inventoryName,
                    _itemName, _itemSetName, _timeOffset
                );
                if (updateAggregate) {
                    var changedAggregate = aggregate.Clone() as
                        Gs2.Gs2Inventory.Model.ItemSet[];
                    changedAggregate[aggregateIndex] = item;
                    changedAggregate.PutCache(
                        _cache, _namespaceName, _userId, _inventoryName,
                        _itemName, _timeOffset
                    );
                }
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next) ? Commit(next) : null;
        }
    }

    public static class AcquireItemSetByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:AcquireItemSetByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = AcquireItemSetByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId ||
                !prepared.AcquireCount.HasValue ||
                prepared.CreateNewItemSet == true) {
                return null;
            }
            var updatedAt = UnixTime.ToUnixTime(DateTime.Now) +
                            (long)(token.TimeOffset ?? 0) * 1000L;
            string selectedUnnamedItem = null;
            string[] selectedUnnamedAggregate = null;

            if (string.IsNullOrEmpty(prepared.ItemSetName)) {
                var aggregate = ((Gs2.Gs2Inventory.Model.ItemSet[])null)
                    .GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        token.UserId,
                        prepared.InventoryName,
                        prepared.ItemName,
                        token.TimeOffset
                    );
                if (!aggregate.Item2 || aggregate.Item1 == null) {
                    return null;
                }
                if (aggregate.Item1.Length == 0 ||
                    prepared.AcquireCount.Value <= 0) {
                    return null;
                }
                var aggregateExpectedItemModelId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:inventory:" +
                    $"{prepared.NamespaceName}:model:{prepared.InventoryName}:" +
                    $"item:{prepared.ItemName}";
                var aggregateModel = ((Gs2.Gs2Inventory.Model.ItemModel)null)
                    .GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        prepared.InventoryName,
                        prepared.ItemName,
                        null
                    );
                if (!aggregateModel.Item2 || aggregateModel.Item1 == null ||
                    aggregateModel.Item1.ItemModelId !=
                    aggregateExpectedItemModelId ||
                    aggregateModel.Item1.Name != prepared.ItemName ||
                    !aggregateModel.Item1.StackingLimit.HasValue ||
                    !aggregateModel.Item1.SortValue.HasValue) {
                    return null;
                }
                var names = new HashSet<string>();
                var effectiveExpiresAt = prepared.ExpiresAt ?? 0;
                Gs2.Gs2Inventory.Model.ItemSet selected = null;
                selectedUnnamedAggregate = new string[aggregate.Item1.Length];
                for (var i = 0; i < aggregate.Item1.Length; i++) {
                    var candidate = aggregate.Item1[i];
                    var expectedId =
                        $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                        $"{domain.RestSession.OwnerId}:inventory:" +
                        $"{prepared.NamespaceName}:user:{token.UserId}:" +
                        $"inventory:{prepared.InventoryName}:item:" +
                        $"{prepared.ItemName}:itemSet:{candidate?.Name}";
                    if (candidate == null ||
                        string.IsNullOrEmpty(candidate.Name) ||
                        !names.Add(candidate.Name) ||
                        candidate.ItemSetId != expectedId ||
                        candidate.UserId != token.UserId ||
                        candidate.InventoryName != prepared.InventoryName ||
                        candidate.ItemName != prepared.ItemName ||
                        !candidate.Count.HasValue || candidate.Count <= 0 ||
                        candidate.Count >
                        aggregateModel.Item1.StackingLimit.Value ||
                        !candidate.ExpiresAt.HasValue ||
                        (candidate.ExpiresAt != 0 &&
                         candidate.ExpiresAt <= updatedAt)) {
                        return null;
                    }
                    var direct = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                        domain.Cache, prepared.NamespaceName, token.UserId,
                        prepared.InventoryName, prepared.ItemName,
                        candidate.Name, token.TimeOffset
                    );
                    var serialized = candidate.ToJson().ToJson();
                    selectedUnnamedAggregate[i] = serialized;
                    if (!direct.Item2 || direct.Item1 == null ||
                        direct.Item1.ToJson().ToJson() != serialized) {
                        return null;
                    }
                    if (candidate.ExpiresAt != effectiveExpiresAt) continue;
                    var room = new BigInteger(
                        aggregateModel.Item1.StackingLimit.Value
                    ) - candidate.Count.Value;
                    if (room <= 0) continue;
                    if (prepared.AcquireCount.Value > room || selected != null) {
                        return null;
                    }
                    selected = candidate;
                }
                if (selected == null) return null;
                prepared.ItemSetName = selected.Name;
                selectedUnnamedItem = selected.ToJson().ToJson();
            }

            var cachedItem = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.InventoryName, prepared.ItemName,
                prepared.ItemSetName, token.TimeOffset
            );
            var cachedModel = ((Gs2.Gs2Inventory.Model.ItemModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.InventoryName,
                prepared.ItemName, null
            );
            var expectedItemSetId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:inventory:{prepared.InventoryName}:" +
                $"item:{prepared.ItemName}:itemSet:{prepared.ItemSetName}";
            var expectedItemModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"model:{prepared.InventoryName}:item:{prepared.ItemName}";
            var item = cachedItem.Item1;
            var model = cachedModel.Item1;
            if (!cachedItem.Item2 || item == null ||
                item.ItemSetId != expectedItemSetId ||
                item.Name != prepared.ItemSetName || item.UserId != token.UserId ||
                item.InventoryName != prepared.InventoryName ||
                item.ItemName != prepared.ItemName || !item.Count.HasValue ||
                !cachedModel.Item2 || model == null ||
                model.ItemModelId != expectedItemModelId ||
                model.Name != prepared.ItemName || !model.StackingLimit.HasValue ||
                !model.SortValue.HasValue ||
                (prepared.ExpiresAt.HasValue && item.ExpiresAt != prepared.ExpiresAt)) {
                return null;
            }
            if (selectedUnnamedItem != null &&
                (item.ToJson().ToJson() != selectedUnnamedItem ||
                 new BigInteger(item.Count.Value) + prepared.AcquireCount.Value >
                 model.StackingLimit.Value)) {
                return null;
            }

            return new ItemSetAcquireSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.InventoryName,
                prepared.ItemName,
                prepared.ItemSetName,
                token.TimeOffset,
                expectedItemSetId,
                expectedItemModelId,
                item.ToJson().ToJson(),
                selectedUnnamedAggregate,
                prepared.AcquireCount.Value,
                model.StackingLimit.Value,
                model.SortValue.Value,
                prepared.ExpiresAt,
                updatedAt
            ).Invoke;
        }
    }
}
