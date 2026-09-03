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
    public static class SetSimpleItemsByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:SetSimpleItemsByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetSimpleItemsByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetSimpleItemsByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = SetSimpleItemsByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId || prepared.Counts == null) {
                return null;
            }

            var commits = new List<SimpleItemMutationSpeculativeCommit>();
            var absoluteSets = new Dictionary<string, long>();
            var hasInternalSetConflict = false;
            foreach (var heldCount in prepared.Counts) {
                if (heldCount?.ItemName == null || !heldCount.Count.HasValue) {
                    continue;
                }
                if (absoluteSets.TryGetValue(heldCount.ItemName, out var count) &&
                    count != heldCount.Count.Value) {
                    hasInternalSetConflict = true;
                }
                absoluteSets[heldCount.ItemName] = heldCount.Count.Value;
            }
            for (var i = 0; i < prepared.Counts.Length; i++) {
                var heldCount = prepared.Counts[i];
                if (heldCount?.ItemName == null || !heldCount.Count.HasValue) {
                    continue;
                }
                var superseded = false;
                for (var j = i + 1; j < prepared.Counts.Length; j++) {
                    if (prepared.Counts[j]?.ItemName == heldCount.ItemName) {
                        superseded = true;
                        break;
                    }
                }
                if (superseded) continue;
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                    $"user:{token.UserId}:simple:inventory:{prepared.InventoryName}:" +
                    $"item:{heldCount.ItemName}";
                var cached = ((Gs2.Gs2Inventory.Model.SimpleItem)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    heldCount.ItemName,
                    token.TimeOffset
                );
                var preparedWasTombstone = cached.Item1 == null;
                var item = cached.Item1 ?? SimpleItemSpeculativeState.KnownZero(
                    expectedId, token.UserId, heldCount.ItemName
                );
                if (!cached.Item2 || item.ItemId != expectedId ||
                    item.UserId != token.UserId ||
                    item.ItemName != heldCount.ItemName) {
                    continue;
                }
                var count = heldCount.Count.Value;
                commits.Add(new SimpleItemMutationSpeculativeCommit(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    heldCount.ItemName,
                    token.TimeOffset,
                    expectedId,
                    item.Revision,
                    preparedWasTombstone,
                    current => {
                        var changed = current.Clone() as Gs2.Gs2Inventory.Model.SimpleItem;
                        changed.Count = count;
                        changed.Revision = 0;
                        return changed;
                    },
                    count
                ));
            }

            if (absoluteSets.Count == 0) {
                return prepared.Counts.Length == 0 ? () => null : null;
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
                commits,
                absoluteSets,
                true,
                hasInternalSetConflict
            ).Invoke;
        }
    }
}
