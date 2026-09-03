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
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inventory.Model;
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
    public static class AddReferenceOfByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:AddReferenceOfByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddReferenceOfByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddReferenceOfByUserIdRequest request
        ) {
            var prepared = request == null ? null : new AddReferenceOfByUserIdRequest()
                .WithNamespaceName(request.NamespaceName)
                .WithInventoryName(request.InventoryName)
                .WithUserId(request.UserId)
                .WithItemName(request.ItemName)
                .WithItemSetName(request.ItemSetName)
                .WithReferenceOf(request.ReferenceOf)
                .WithTimeOffsetToken(request.TimeOffsetToken)
                .WithDuplicationAvoider(request.DuplicationAvoider);
            var token = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            var now = UnixTime.ToUnixTime(DateTime.Now) +
                      (long)(token?.TimeOffset ?? 0) * 1000L;
            return ReferenceOfMutationSpeculativeExecutor.Prepare(
                domain,
                token,
                prepared?.NamespaceName,
                prepared?.UserId,
                prepared?.InventoryName,
                prepared?.ItemName,
                prepared?.ItemSetName,
                prepared?.ReferenceOf,
                item => {
                    var changed = item.SpeculativeExecution(prepared);
                    changed.UpdatedAt = now;
                    return changed;
                }
            );
        }
    }

    internal sealed class ReferenceOfMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _inventoryName;
        private readonly string _itemName;
        private readonly string _itemSetName;
        private readonly int? _timeOffset;
        private readonly string _itemSetId;
        private readonly Func<ItemSet, ItemSet> _transform;

        internal ReferenceOfMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string inventoryName,
            string itemName,
            string itemSetName,
            int? timeOffset,
            string itemSetId,
            Func<ItemSet, ItemSet> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _inventoryName = inventoryName;
            _itemName = itemName;
            _itemSetName = itemSetName;
            _timeOffset = timeOffset;
            _itemSetId = itemSetId;
            _transform = transform;
        }

        public string CompositionKey => ((ItemSet)null).CacheParentKey(
            _namespaceName,
            _userId,
            _inventoryName,
            _timeOffset
        ) + ":" + ((ItemSet)null).CacheKey(_itemName, _itemSetName);

        private bool IsExpected(ItemSet item) {
            if (item == null || item.ItemSetId != _itemSetId ||
                item.Name != _itemSetName || item.UserId != _userId ||
                item.InventoryName != _inventoryName ||
                item.ItemName != _itemName || item.ReferenceOf == null) {
                return false;
            }
            return true;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                ItemSet item;
                if (hasCurrent) {
                    item = current as ItemSet;
                }
                else {
                    var cached = ((ItemSet)null).GetCache(
                        _cache,
                        _namespaceName,
                        _userId,
                        _inventoryName,
                        _itemName,
                        _itemSetName,
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
                next = _transform(item);
                return IsExpected(next as ItemSet);
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is ItemSet item && IsExpected(item)) {
                item.PutCache(
                    _cache,
                    _namespaceName,
                    _userId,
                    _inventoryName,
                    _itemName,
                    _itemSetName,
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

    internal static class ReferenceOfMutationSpeculativeExecutor
    {
        internal static Func<object> Prepare(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            string namespaceName,
            string requestUserId,
            string inventoryName,
            string itemName,
            string itemSetName,
            string referenceOf,
            Func<ItemSet, ItemSet> transform
        ) {
            if (string.IsNullOrEmpty(accessToken?.UserId) ||
                accessToken.UserId != requestUserId) {
                return null;
            }
            if (domain?.RestSession == null ||
                referenceOf == null ||
                transform == null) {
                return null;
            }
            var userId = accessToken.UserId;
            var itemSetId = string.Join(
                ":",
                "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "inventory", namespaceName,
                "user", userId,
                "inventory", inventoryName,
                "item", itemName,
                "itemSet", itemSetName
            );
            var timeOffset = accessToken.TimeOffset;
            var cached = ((ItemSet)null).GetCache(
                domain.Cache,
                namespaceName,
                userId,
                inventoryName,
                itemName,
                itemSetName,
                timeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.ItemSetId != itemSetId ||
                cached.Item1.ReferenceOf == null) {
                return null;
            }
            return new ReferenceOfMutationSpeculativeCommit(
                domain.Cache,
                namespaceName,
                userId,
                inventoryName,
                itemName,
                itemSetName,
                timeOffset,
                itemSetId,
                transform
            ).Invoke;
        }
    }
}
