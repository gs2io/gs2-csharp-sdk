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
using System.Collections.Generic;
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
    internal static class SimpleItemSpeculativeState
    {
        internal static Gs2.Gs2Inventory.Model.SimpleItem KnownZero(
            string expectedId,
            string userId,
            string itemName
        ) {
            return new Gs2.Gs2Inventory.Model.SimpleItem()
                .WithItemId(expectedId)
                .WithUserId(userId)
                .WithItemName(itemName)
                .WithCount(0)
                .WithRevision(0);
        }
    }

    internal sealed class SimpleItemCompositionState
    {
        internal readonly Gs2.Gs2Inventory.Model.SimpleItem Item;
        internal readonly bool HasAbsoluteSet;
        internal readonly long AbsoluteSetCount;

        internal SimpleItemCompositionState(
            Gs2.Gs2Inventory.Model.SimpleItem item,
            bool hasAbsoluteSet = false,
            long absoluteSetCount = 0
        ) {
            Item = item;
            HasAbsoluteSet = hasAbsoluteSet;
            AbsoluteSetCount = absoluteSetCount;
        }
    }

    internal sealed class SimpleItemMutationSpeculativeCommit :
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
        private readonly Func<Gs2.Gs2Inventory.Model.SimpleItem,
            Gs2.Gs2Inventory.Model.SimpleItem> _transform;
        private readonly long? _absoluteSetCount;

        internal SimpleItemMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            int? timeOffset,
            string expectedId,
            long? preparedRevision,
            bool preparedWasTombstone,
            Func<Gs2.Gs2Inventory.Model.SimpleItem,
                Gs2.Gs2Inventory.Model.SimpleItem> transform,
            long? absoluteSetCount = null
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
            _absoluteSetCount = absoluteSetCount;
        }

        public string CompositionKey => ((Gs2.Gs2Inventory.Model.SimpleItem)null)
            .CacheParentKey(
                _namespaceName, _userId, _inventoryName, _timeOffset
            ) + ":" + ((Gs2.Gs2Inventory.Model.SimpleItem)null).CacheKey(_itemName);

        internal string ItemName => _itemName;

        private bool IsExpected(Gs2.Gs2Inventory.Model.SimpleItem item) {
            return item != null && item.ItemId == _expectedId &&
                   item.UserId == _userId && item.ItemName == _itemName;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                SimpleItemCompositionState state;
                if (hasCurrent) {
                    state = current as SimpleItemCompositionState;
                    if (state == null || !IsExpected(state.Item)) {
                        next = null;
                        return false;
                    }
                }
                else {
                    var cached = ((Gs2.Gs2Inventory.Model.SimpleItem)null).GetCache(
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
                    var item = cached.Item1 ??
                        SimpleItemSpeculativeState.KnownZero(
                            _expectedId, _userId, _itemName
                        );
                    if (!IsExpected(item) ||
                        (item.Revision > 0 && item.Revision != _preparedRevision)) {
                        next = null;
                        return false;
                    }
                    state = new SimpleItemCompositionState(item);
                }
                if (_absoluteSetCount.HasValue && state.HasAbsoluteSet) {
                    if (state.AbsoluteSetCount != _absoluteSetCount.Value) {
                        next = null;
                        return false;
                    }
                    next = state;
                    return true;
                }
                var changed = _transform(state.Item);
                if (!IsExpected(changed) || !changed.Count.HasValue) {
                    next = null;
                    return false;
                }
                next = new SimpleItemCompositionState(
                    changed,
                    state.HasAbsoluteSet || _absoluteSetCount.HasValue,
                    _absoluteSetCount ?? state.AbsoluteSetCount
                );
                return true;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            var item = (value as SimpleItemCompositionState)?.Item;
            if (IsExpected(item) && item.Count.HasValue) {
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

    internal sealed class SimpleItemBatchCompositionState
    {
        internal readonly Dictionary<string, Tuple<
            object,
            SimpleItemMutationSpeculativeCommit
        >> Items;
        internal readonly Dictionary<string, long> AbsoluteSets;
        internal readonly int SetActionCount;
        internal readonly bool HasInternalSetConflict;

        internal SimpleItemBatchCompositionState(
            Dictionary<string, Tuple<
                object,
                SimpleItemMutationSpeculativeCommit
            >> items = null,
            Dictionary<string, long> absoluteSets = null,
            int setActionCount = 0,
            bool hasInternalSetConflict = false
        ) {
            Items = items ?? new Dictionary<string, Tuple<
                object,
                SimpleItemMutationSpeculativeCommit
            >>();
            AbsoluteSets = absoluteSets ?? new Dictionary<string, long>();
            SetActionCount = setActionCount;
            HasInternalSetConflict = hasInternalSetConflict;
        }
    }

    internal sealed class SimpleItemBatchSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly SimpleItemMutationSpeculativeCommit[] _commits;
        private readonly Dictionary<string, long> _absoluteSets;
        private readonly string _compositionKey;
        private readonly bool _isSetAction;
        private readonly bool _hasInternalSetConflict;

        internal SimpleItemBatchSpeculativeCommit(
            string compositionKey,
            List<SimpleItemMutationSpeculativeCommit> commits,
            Dictionary<string, long> absoluteSets = null,
            bool isSetAction = false,
            bool hasInternalSetConflict = false
        ) {
            _compositionKey = compositionKey;
            _commits = commits.ToArray();
            _absoluteSets = absoluteSets ?? new Dictionary<string, long>();
            _isSetAction = isSetAction;
            _hasInternalSetConflict = hasInternalSetConflict;
        }

        public string CompositionKey => _compositionKey;

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            var source = hasCurrent
                ? current as SimpleItemBatchCompositionState
                : new SimpleItemBatchCompositionState();
            if (source == null) {
                next = null;
                return false;
            }
            if (_isSetAction && source.SetActionCount > 0 &&
                (source.HasInternalSetConflict || _hasInternalSetConflict)) {
                next = null;
                return false;
            }
            var items = new Dictionary<string, Tuple<
                object,
                SimpleItemMutationSpeculativeCommit
            >>(source.Items);
            var absoluteSets = new Dictionary<string, long>(source.AbsoluteSets);
            foreach (var intent in _absoluteSets) {
                if (absoluteSets.TryGetValue(intent.Key, out var count) &&
                    count != intent.Value) {
                    next = null;
                    return false;
                }
                absoluteSets[intent.Key] = intent.Value;
            }
            foreach (var commit in _commits) {
                var hasItem = items.TryGetValue(commit.ItemName, out var item);
                if (!commit.TryCompose(
                        item?.Item1,
                        hasItem,
                        out var changed
                    )) {
                    next = null;
                    return false;
                }
                items[commit.ItemName] = Tuple.Create(changed, commit);
            }
            next = new SimpleItemBatchCompositionState(
                items,
                absoluteSets,
                source.SetActionCount + (_isSetAction ? 1 : 0),
                source.HasInternalSetConflict || _hasInternalSetConflict
            );
            return true;
        }

        public object Commit(object value) {
            if (value is SimpleItemBatchCompositionState state) {
                foreach (var item in state.Items.Values) {
                    item.Item2.Commit(item.Item1);
                }
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

    public static class AcquireSimpleItemsByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:AcquireSimpleItemsByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireSimpleItemsByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireSimpleItemsByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = AcquireSimpleItemsByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId || prepared.AcquireCounts == null) {
                return null;
            }

            var commits = new List<SimpleItemMutationSpeculativeCommit>();
            foreach (var acquireCount in prepared.AcquireCounts) {
                if (acquireCount?.ItemName == null || !acquireCount.Count.HasValue) {
                    continue;
                }
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                    $"user:{token.UserId}:simple:inventory:{prepared.InventoryName}:" +
                    $"item:{acquireCount.ItemName}";
                var cached = ((Gs2.Gs2Inventory.Model.SimpleItem)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    acquireCount.ItemName,
                    token.TimeOffset
                );
                var preparedWasTombstone = cached.Item1 == null;
                var item = cached.Item1 ?? SimpleItemSpeculativeState.KnownZero(
                    expectedId, token.UserId, acquireCount.ItemName
                );
                if (!cached.Item2 || item.ItemId != expectedId ||
                    item.UserId != token.UserId ||
                    item.ItemName != acquireCount.ItemName || !item.Count.HasValue) {
                    continue;
                }
                var count = acquireCount.Count.Value;
                commits.Add(new SimpleItemMutationSpeculativeCommit(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    acquireCount.ItemName,
                    token.TimeOffset,
                    expectedId,
                    item.Revision,
                    preparedWasTombstone,
                    current => {
                        var changed = current.Clone() as Gs2.Gs2Inventory.Model.SimpleItem;
                        changed.Count = checked(current.Count.Value + count);
                        changed.Revision = 0;
                        return changed;
                    }
                ));
            }

            if (commits.Count == 0) {
                return prepared.AcquireCounts.Length == 0 ? () => null : null;
            }
            var compositionKey = ((Gs2.Gs2Inventory.Model.SimpleItem)null)
                .CacheParentKey(
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    token.TimeOffset
                );
            return new SimpleItemBatchSpeculativeCommit(
                compositionKey,
                commits
            ).Invoke;
        }
    }
}
