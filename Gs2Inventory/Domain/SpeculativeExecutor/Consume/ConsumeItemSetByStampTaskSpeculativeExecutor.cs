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
    internal sealed class ItemSetCountSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;
        private readonly int? _timeOffset;
        private readonly string _expectedId;
        private readonly string _expectedItemModelId;
        private readonly string _expectedInventoryModelId;
        private readonly string _preparedItem;
        private string _preparedAggregate;
        private readonly long _consumeCount;
        private readonly bool? _protectReferencedItem;
        private readonly int? _sortValue;
        private readonly long _updatedAt;
        private readonly string _expectedInventoryId;
        private readonly string _preparedInventory;
        private readonly bool _startedPositive;

        internal ItemSetCountSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            int? timeOffset,
            string expectedId,
            string expectedItemModelId,
            string expectedInventoryModelId,
            string preparedItem,
            long consumeCount,
            bool? protectReferencedItem,
            int? sortValue,
            long updatedAt,
            string expectedInventoryId,
            string preparedInventory,
            bool startedPositive
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _inventoryName = inventoryName;
            _itemName = itemName;
            _itemSetName = itemSetName;
            _timeOffset = timeOffset;
            _expectedId = expectedId;
            _expectedItemModelId = expectedItemModelId;
            _expectedInventoryModelId = expectedInventoryModelId;
            _preparedItem = preparedItem;
            _consumeCount = consumeCount;
            _protectReferencedItem = protectReferencedItem;
            _sortValue = sortValue;
            _updatedAt = updatedAt;
            _expectedInventoryId = expectedInventoryId;
            _preparedInventory = preparedInventory;
            _startedPositive = startedPositive;
        }

        internal ItemSetCountSpeculativeCommit RequireAggregate(
            string preparedAggregate
        ) {
            _preparedAggregate = preparedAggregate;
            return this;
        }

        public string CompositionKey => ((Gs2.Gs2Inventory.Model.ItemSet)null)
            .CacheParentKey(_namespaceName, _userId, _inventoryName, _timeOffset) +
            ":" + ((Gs2.Gs2Inventory.Model.ItemSet)null)
                .CacheKey(_itemName, _itemSetName);

        private bool IsExpected(Gs2.Gs2Inventory.Model.ItemSet item) {
            return item != null && item.ItemSetId == _expectedId &&
                   item.Name == _itemSetName && item.UserId == _userId &&
                   item.InventoryName == _inventoryName &&
                   item.ItemName == _itemName && item.Count.HasValue;
        }

        private bool IsExpected(Gs2.Gs2Inventory.Model.ItemModel model) {
            return model != null && model.ItemModelId == _expectedItemModelId &&
                   model.Name == _itemName && model.SortValue == _sortValue.Value;
        }

        private bool IsExpected(Gs2.Gs2Inventory.Model.InventoryModel model) {
            return model != null &&
                   model.InventoryModelId == _expectedInventoryModelId &&
                   model.Name == _inventoryName &&
                   model.ProtectReferencedItem == _protectReferencedItem.Value;
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
            if (!cached.Item2 || aggregate == null) return false;
            var snapshot = _preparedAggregate ?? _preparedItem;
            var names = new HashSet<string>();
            for (var i = 0; i < aggregate.Length; i++) {
                var candidate = aggregate[i];
                if (candidate == null || string.IsNullOrEmpty(candidate.Name) ||
                    !candidate.Count.HasValue || !names.Add(candidate.Name)) {
                    return false;
                }
                if (candidate.Name == _itemSetName) {
                    if (!IsExpected(candidate) ||
                        candidate.ToJson().ToJson() != snapshot) {
                        return false;
                    }
                    index = i;
                }
            }
            return index >= 0;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                if (_preparedAggregate != null &&
                    (!TryGetExpectedAggregate(out var aggregate, out var index) ||
                     aggregate.Length != 1 || index != 0)) {
                    next = null;
                    return false;
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
                    if (!cached.Item2 || cached.Item1 == null ||
                        (_preparedAggregate != null &&
                         cached.Item1.ToJson().ToJson() != _preparedItem)) {
                        next = null;
                        return false;
                    }
                    item = cached.Item1;
                }
                if (!IsExpected(item)) {
                    next = null;
                    return false;
                }
                if (_consumeCount <= 0) {
                    next = item;
                    return true;
                }
                if ((item.ReferenceOf?.Length ?? 0) > 0) {
                    if (!_protectReferencedItem.HasValue) {
                        next = null;
                        return false;
                    }
                    var inventoryModel =
                        ((Gs2.Gs2Inventory.Model.InventoryModel)null).GetCache(
                            _cache, _namespaceName, _inventoryName, null
                        );
                    if (!inventoryModel.Item2 ||
                        !IsExpected(inventoryModel.Item1) ||
                        _protectReferencedItem.Value) {
                        next = null;
                        return false;
                    }
                }
                if (item.Count.Value < _consumeCount) {
                    next = null;
                    return false;
                }
                var changed = item.Clone() as Gs2.Gs2Inventory.Model.ItemSet;
                changed.Count = checked(item.Count.Value - _consumeCount);
                if (_sortValue.HasValue) {
                    var itemModel = ((Gs2.Gs2Inventory.Model.ItemModel)null).GetCache(
                        _cache, _namespaceName, _inventoryName, _itemName, null
                    );
                    if (itemModel.Item2 && IsExpected(itemModel.Item1)) {
                        changed.SortValue = _sortValue;
                    }
                }
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
                if (_preparedAggregate != null &&
                    (!updateAggregate || aggregate.Length != 1 ||
                     aggregateIndex != 0)) {
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
                if (_startedPositive && item.Count == 0) {
                    TryDecrementInventoryCapacity();
                }
            }
            return null;
        }

        private void TryDecrementInventoryCapacity() {
            if (_preparedInventory == null) return;
            try {
                var cached = ((Gs2.Gs2Inventory.Model.Inventory)null).GetCache(
                    _cache, _namespaceName, _userId, _inventoryName, _timeOffset
                );
                var inventory = cached.Item1;
                if (!cached.Item2 || inventory == null ||
                    inventory.InventoryId != _expectedInventoryId ||
                    inventory.UserId != _userId ||
                    inventory.InventoryName != _inventoryName ||
                    !inventory.CurrentInventoryCapacityUsage.HasValue ||
                    !inventory.Revision.HasValue || inventory.Revision < 0 ||
                    (inventory.Revision > 0 && !string.Equals(
                        inventory.ToJson().ToJson(),
                        _preparedInventory,
                        StringComparison.Ordinal
                    ))) {
                    return;
                }
                var changed = inventory.Clone() as
                    Gs2.Gs2Inventory.Model.Inventory;
                if (changed == null) return;
                changed.CurrentInventoryCapacityUsage = (int)Math.Max(
                    (long)changed.CurrentInventoryCapacityUsage.Value - 1L,
                    0L
                );
                changed.UpdatedAt = _updatedAt;
                changed.Revision = 0;
                _cache.Put(
                    changed.CacheParentKey(
                        _namespaceName, _userId, _timeOffset
                    ),
                    changed.CacheKey(_inventoryName),
                    changed,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            catch (System.Exception) {
                // The primary ItemSet prediction remains useful on its own.
            }
        }

        public object Invoke() {
            return TryCompose(null, false, out var next) ? Commit(next) : null;
        }
    }

    internal sealed class ItemSetAggregateConsumeSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly int? _timeOffset;
        private readonly string[] _preparedItems;
        private readonly string _preparedInventoryModel;
        private readonly ItemSetCountSpeculativeCommit[] _commits;

        internal ItemSetAggregateConsumeSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            int? timeOffset,
            string[] preparedItems,
            string preparedInventoryModel,
            ItemSetCountSpeculativeCommit[] commits
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _inventoryName = inventoryName;
            _itemName = itemName;
            _timeOffset = timeOffset;
            _preparedItems = preparedItems;
            _preparedInventoryModel = preparedInventoryModel;
            _commits = commits;
        }

        private bool IsExpectedState() {
            if (_preparedInventoryModel != null) {
                var inventoryModel =
                    ((Gs2.Gs2Inventory.Model.InventoryModel)null).GetCache(
                        _cache, _namespaceName, _inventoryName, null
                    );
                if (!inventoryModel.Item2 || inventoryModel.Item1 == null ||
                    inventoryModel.Item1.ToJson().ToJson() !=
                    _preparedInventoryModel) {
                    return false;
                }
            }
            var cached = ((Gs2.Gs2Inventory.Model.ItemSet[])null).GetCache(
                _cache, _namespaceName, _userId, _inventoryName,
                _itemName, _timeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.Length != _preparedItems.Length) {
                return false;
            }
            for (var i = 0; i < cached.Item1.Length; i++) {
                if (cached.Item1[i] == null ||
                    cached.Item1[i].ToJson().ToJson() != _preparedItems[i]) {
                    return false;
                }
                var direct = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                    _cache, _namespaceName, _userId, _inventoryName,
                    _itemName, cached.Item1[i].Name, _timeOffset
                );
                if (!direct.Item2 || direct.Item1 == null ||
                    direct.Item1.ToJson().ToJson() != _preparedItems[i]) {
                    return false;
                }
            }
            return true;
        }

        public object Invoke() {
            try {
                if (!IsExpectedState()) return null;
                var values = new object[_commits.Length];
                for (var i = 0; i < _commits.Length; i++) {
                    if (!_commits[i].TryCompose(null, false, out values[i])) {
                        return null;
                    }
                }
                if (!IsExpectedState()) return null;
                var remaining = new List<Gs2.Gs2Inventory.Model.ItemSet>();
                for (var i = 0; i < _commits.Length; i++) {
                    _commits[i].Commit(values[i]);
                    if (values[i] is Gs2.Gs2Inventory.Model.ItemSet item &&
                        item.Count > 0) {
                        remaining.Add(item);
                    }
                }
                remaining.ToArray().PutCache(
                    _cache, _namespaceName, _userId, _inventoryName,
                    _itemName, _timeOffset
                );
            }
            catch (System.Exception) {
                // Preserve the server-authoritative cache on any uncertainty.
            }
            return null;
        }
    }

    public static class ConsumeItemSetByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:ConsumeItemSetByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeItemSetByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeItemSetByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = ConsumeItemSetByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                prepared.NamespaceName == null ||
                prepared.InventoryName == null ||
                prepared.ItemName == null ||
                !prepared.ConsumeCount.HasValue) {
                return null;
            }
            if (string.IsNullOrEmpty(prepared.ItemSetName)) {
                var aggregate = ((Gs2.Gs2Inventory.Model.ItemSet[])null)
                    .GetCache(
                        domain.Cache, prepared.NamespaceName, token.UserId,
                        prepared.InventoryName, prepared.ItemName,
                        token.TimeOffset
                    );
                if (!aggregate.Item2 || aggregate.Item1 == null ||
                    aggregate.Item1.Length == 0) {
                    return null;
                }
                var multiple = aggregate.Item1.Length > 1;
                var aggregateUpdatedAt = UnixTime.ToUnixTime(DateTime.Now) +
                                         (long)(token.TimeOffset ?? 0) * 1000L;
                var total = BigInteger.Zero;
                var names = new HashSet<string>();
                var preparedItems = new string[aggregate.Item1.Length];
                var hasReferences = false;
                for (var i = 0; i < aggregate.Item1.Length; i++) {
                    var aggregateItem = aggregate.Item1[i];
                    var aggregateExpectedId =
                        $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                        $"{domain.RestSession.OwnerId}:inventory:" +
                        $"{prepared.NamespaceName}:user:{token.UserId}:" +
                        $"inventory:{prepared.InventoryName}:item:" +
                        $"{prepared.ItemName}:itemSet:{aggregateItem?.Name}";
                    if (aggregateItem == null ||
                        string.IsNullOrEmpty(aggregateItem.Name) ||
                        !names.Add(aggregateItem.Name) ||
                        aggregateItem.ItemSetId != aggregateExpectedId ||
                        aggregateItem.UserId != token.UserId ||
                        aggregateItem.InventoryName != prepared.InventoryName ||
                        aggregateItem.ItemName != prepared.ItemName ||
                        !aggregateItem.Count.HasValue || aggregateItem.Count <= 0) {
                        return null;
                    }
                    if (aggregateItem.ExpiresAt != 0 &&
                        aggregateItem.ExpiresAt <= aggregateUpdatedAt) {
                        return null;
                    }
                    var direct = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                        domain.Cache, prepared.NamespaceName, token.UserId,
                        prepared.InventoryName, prepared.ItemName,
                        aggregateItem.Name, token.TimeOffset
                    );
                    preparedItems[i] = aggregateItem.ToJson().ToJson();
                    if (!direct.Item2 || direct.Item1 == null ||
                        direct.Item1.ToJson().ToJson() != preparedItems[i]) {
                        return null;
                    }
                    hasReferences |= (aggregateItem.ReferenceOf?.Length ?? 0) > 0;
                }
                var aggregateProtectReferencedItem = false;
                string preparedInventoryModel = null;
                if (hasReferences) {
                    var aggregateExpectedInventoryModelId =
                        $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                        $"{domain.RestSession.OwnerId}:inventory:" +
                        $"{prepared.NamespaceName}:model:{prepared.InventoryName}";
                    var aggregateCachedInventoryModel =
                        ((Gs2.Gs2Inventory.Model.InventoryModel)null).GetCache(
                            domain.Cache, prepared.NamespaceName,
                            prepared.InventoryName, null
                        );
                    var aggregateInventoryModel =
                        aggregateCachedInventoryModel.Item1;
                    if (!aggregateCachedInventoryModel.Item2 ||
                        aggregateInventoryModel == null ||
                        aggregateInventoryModel.InventoryModelId !=
                        aggregateExpectedInventoryModelId ||
                        aggregateInventoryModel.Name != prepared.InventoryName ||
                        !aggregateInventoryModel.ProtectReferencedItem.HasValue) {
                        return null;
                    }
                    aggregateProtectReferencedItem =
                        aggregateInventoryModel.ProtectReferencedItem.Value;
                    preparedInventoryModel =
                        aggregateInventoryModel.ToJson().ToJson();
                }
                var eligible = new bool[aggregate.Item1.Length];
                for (var i = 0; i < aggregate.Item1.Length; i++) {
                    eligible[i] = !aggregateProtectReferencedItem ||
                        (aggregate.Item1[i].ReferenceOf?.Length ?? 0) == 0;
                    if (eligible[i]) total += aggregate.Item1[i].Count.Value;
                }
                var consumeCounts = new long[aggregate.Item1.Length];
                if (multiple) {
                    if (prepared.ConsumeCount.Value <= 0 ||
                        total < prepared.ConsumeCount.Value) {
                        return null;
                    }
                    if (total == prepared.ConsumeCount.Value) {
                        for (var i = 0; i < aggregate.Item1.Length; i++) {
                            if (eligible[i]) {
                                consumeCounts[i] = aggregate.Item1[i].Count.Value;
                            }
                        }
                    }
                    else {
                        var order = new List<int>();
                        for (var i = 0; i < aggregate.Item1.Length; i++) {
                            if (!eligible[i]) continue;
                            if (!aggregate.Item1[i].SortValue.HasValue ||
                                !aggregate.Item1[i].ExpiresAt.HasValue ||
                                aggregate.Item1[i].ExpiresAt <= 0) {
                                return null;
                            }
                            order.Add(i);
                        }
                        Comparison<int> compare = (left, right) => {
                            var result = aggregate.Item1[right].SortValue.Value
                                .CompareTo(
                                    aggregate.Item1[left].SortValue.Value
                                );
                            if (result != 0) return result;
                            result = aggregate.Item1[left].ExpiresAt.Value
                                .CompareTo(
                                    aggregate.Item1[right].ExpiresAt.Value
                                );
                            if (result != 0) return result;
                            return aggregate.Item1[left].Count.Value.CompareTo(
                                aggregate.Item1[right].Count.Value
                            );
                        };
                        order.Sort(compare);
                        for (var i = 1; i < order.Count; i++) {
                            if (compare(order[i - 1], order[i]) == 0) return null;
                        }
                        var remaining = prepared.ConsumeCount.Value;
                        foreach (var index in order) {
                            var count = aggregate.Item1[index].Count.Value;
                            consumeCounts[index] = Math.Min(remaining, count);
                            remaining -= consumeCounts[index];
                            if (remaining == 0) break;
                        }
                    }
                }
                var commits = new ItemSetCountSpeculativeCommit[
                    aggregate.Item1.Length
                ];
                for (var i = 0; i < aggregate.Item1.Length; i++) {
                    var named = ConsumeItemSetByUserIdRequest.FromJson(
                        prepared.ToJson()
                    );
                    named.ItemSetName = aggregate.Item1[i].Name;
                    if (multiple) named.ConsumeCount = consumeCounts[i];
                    var commit = await ExecuteAsync(domain, token, named);
                    if (!(commit?.Target is ItemSetCountSpeculativeCommit target)) {
                        return null;
                    }
                    commits[i] = target;
                }
                if (!multiple) {
                    return commits[0].RequireAggregate(preparedItems[0]).Invoke;
                }
                return new ItemSetAggregateConsumeSpeculativeCommit(
                    domain.Cache, prepared.NamespaceName, token.UserId,
                    prepared.InventoryName, prepared.ItemName,
                    token.TimeOffset, preparedItems, preparedInventoryModel,
                    commits
                ).Invoke;
            }
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:inventory:{prepared.InventoryName}:" +
                $"item:{prepared.ItemName}:itemSet:{prepared.ItemSetName}";
            var expectedItemModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"model:{prepared.InventoryName}:item:{prepared.ItemName}";
            var expectedInventoryModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"model:{prepared.InventoryName}";
            var expectedInventoryId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:inventory:{prepared.InventoryName}";
            var cached = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.InventoryName, prepared.ItemName,
                prepared.ItemSetName, token.TimeOffset
            );
            var cachedItemModel = ((Gs2.Gs2Inventory.Model.ItemModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.InventoryName,
                prepared.ItemName, null
            );
            var cachedInventoryModel = ((Gs2.Gs2Inventory.Model.InventoryModel)null)
                .GetCache(
                    domain.Cache, prepared.NamespaceName,
                    prepared.InventoryName, null
                );
            var cachedInventory = ((Gs2.Gs2Inventory.Model.Inventory)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.InventoryName, token.TimeOffset
            );
            var item = cached.Item1;
            var itemModel = cachedItemModel.Item1;
            var inventoryModel = cachedInventoryModel.Item1;
            if (!cached.Item2 || item == null || item.ItemSetId != expectedId ||
                item.Name != prepared.ItemSetName ||
                item.UserId != token.UserId ||
                item.InventoryName != prepared.InventoryName ||
                item.ItemName != prepared.ItemName || !item.Count.HasValue) {
                return null;
            }
            int? sortValue = null;
            if (cachedItemModel.Item2 && itemModel != null &&
                itemModel.ItemModelId == expectedItemModelId &&
                itemModel.Name == prepared.ItemName &&
                itemModel.SortValue.HasValue) {
                sortValue = itemModel.SortValue.Value;
            }
            bool? protectReferencedItem = null;
            if (cachedInventoryModel.Item2 && inventoryModel != null &&
                inventoryModel.InventoryModelId == expectedInventoryModelId &&
                inventoryModel.Name == prepared.InventoryName &&
                inventoryModel.ProtectReferencedItem.HasValue) {
                protectReferencedItem = inventoryModel.ProtectReferencedItem.Value;
            }
            if ((item.ReferenceOf?.Length ?? 0) > 0 &&
                !protectReferencedItem.HasValue) {
                return null;
            }
            string preparedInventory = null;
            var inventory = cachedInventory.Item1;
            if (cachedInventory.Item2 && inventory != null &&
                inventory.InventoryId == expectedInventoryId &&
                inventory.UserId == token.UserId &&
                inventory.InventoryName == prepared.InventoryName &&
                inventory.CurrentInventoryCapacityUsage.HasValue &&
                inventory.Revision.HasValue && inventory.Revision >= 0) {
                preparedInventory = inventory.ToJson().ToJson();
            }
            var updatedAt = UnixTime.ToUnixTime(DateTime.Now) +
                            (long)(token.TimeOffset ?? 0) * 1000L;

            return new ItemSetCountSpeculativeCommit(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.InventoryName, prepared.ItemName,
                prepared.ItemSetName, token.TimeOffset, expectedId,
                expectedItemModelId, expectedInventoryModelId,
                item.ToJson().ToJson(),
                prepared.ConsumeCount.Value,
                protectReferencedItem, sortValue, updatedAt,
                expectedInventoryId, preparedInventory,
                item.Count.Value > 0
            ).Invoke;
        }
    }
}
