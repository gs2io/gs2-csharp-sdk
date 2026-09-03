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
    public static class ConsumeSimpleItemsByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:ConsumeSimpleItemsByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeSimpleItemsByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeSimpleItemsByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = ConsumeSimpleItemsByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId || prepared.ConsumeCounts == null) {
                return null;
            }

            var commits = new List<SimpleItemMutationSpeculativeCommit>();
            foreach (var consumeCount in prepared.ConsumeCounts) {
                if (consumeCount?.ItemName == null || !consumeCount.Count.HasValue) {
                    continue;
                }
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                    $"user:{token.UserId}:simple:inventory:{prepared.InventoryName}:" +
                    $"item:{consumeCount.ItemName}";
                var cached = ((Gs2.Gs2Inventory.Model.SimpleItem)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    consumeCount.ItemName,
                    token.TimeOffset
                );
                var item = cached.Item1;
                if (!cached.Item2 || item == null || item.ItemId != expectedId ||
                    item.UserId != token.UserId ||
                    item.ItemName != consumeCount.ItemName || !item.Count.HasValue) {
                    continue;
                }
                var count = consumeCount.Count.Value;
                commits.Add(new SimpleItemMutationSpeculativeCommit(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    consumeCount.ItemName,
                    token.TimeOffset,
                    expectedId,
                    item.Revision,
                    false,
                    current => {
                        var value = checked(current.Count.Value - count);
                        if (value < 0) return null;
                        var changed = current.Clone() as Gs2.Gs2Inventory.Model.SimpleItem;
                        changed.Count = value;
                        changed.Revision = 0;
                        return changed;
                    }
                ));
            }

            if (commits.Count == 0) {
                return prepared.ConsumeCounts.Length == 0 ? () => null : null;
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
