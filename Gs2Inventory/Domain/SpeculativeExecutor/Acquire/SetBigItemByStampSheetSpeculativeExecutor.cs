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
    public static class SetBigItemByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:SetBigItemByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetBigItemByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetBigItemByUserIdRequest request
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
            var prepared = SetBigItemByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                prepared.NamespaceName == null ||
                prepared.InventoryName == null ||
                prepared.ItemName == null ||
                prepared.Count == null) {
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
                    prepared.ItemName)) {
                return null;
            }
            var preparedRevision = item.Revision;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
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
                    var changed = current.Clone() as Gs2.Gs2Inventory.Model.BigItem;
                    changed.Count = prepared.Count;
                    changed.Revision = 0;
                    return changed;
                }
            ).Invoke;
        }
/* diff +++ end */

/* diff --- start
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
        private static bool IsUsable(
            Gs2.Gs2Inventory.Model.BigItem item,
            bool found,
            string expectedId,
            string userId,
            string itemName
        ) {
            return found && item != null && item.ItemId == expectedId &&
                   item.UserId == userId &&
                   item.ItemName == itemName;
/* diff +++ end */
        }
    }
}
