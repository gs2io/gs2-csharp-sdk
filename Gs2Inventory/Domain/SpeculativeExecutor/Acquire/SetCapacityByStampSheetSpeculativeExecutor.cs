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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
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
    public static class SetCapacityByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:SetCapacityByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetCapacityByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetCapacityByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Inventory(
                request.InventoryName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            var prepared = SetCapacityByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                !prepared.NewCapacityValue.HasValue) {
                return null;
            }
            var cachedItem = ((Gs2.Gs2Inventory.Model.Inventory)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.InventoryName,
                token.TimeOffset
            );
            var cachedModel = ((Gs2.Gs2Inventory.Model.InventoryModel)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.InventoryName,
                null
            );
            var item = cachedItem.Item1;
            var model = cachedModel.Item1;
            var expectedItemId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:inventory:{prepared.InventoryName}";
            var expectedModelId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"model:{prepared.InventoryName}";
            if (!cachedItem.Item2 || !cachedModel.Item2 || item == null || model == null ||
                item.InventoryId != expectedItemId || item.UserId != token.UserId ||
                item.InventoryName != prepared.InventoryName ||
                model.InventoryModelId != expectedModelId ||
                model.Name != prepared.InventoryName || !model.MaxCapacity.HasValue) {
                return null;
            }
            var preparedRevision = item.Revision;
            var preparedMaxCapacity = model.MaxCapacity;
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var liveItemCache = ((Gs2.Gs2Inventory.Model.Inventory)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.InventoryName,
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    token.TimeOffset
                );
                var liveModelCache = ((Gs2.Gs2Inventory.Model.InventoryModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    prepared.InventoryName,
/* diff +++ end */
                    null
/* diff +++ start */
                );
                var liveItem = liveItemCache.Item1;
                var liveModel = liveModelCache.Item1;
                if (!liveItemCache.Item2 || !liveModelCache.Item2 ||
                    liveItem == null || liveModel == null ||
                    liveItem.InventoryId != expectedItemId ||
                    liveItem.UserId != token.UserId ||
                    liveItem.InventoryName != prepared.InventoryName ||
                    liveModel.InventoryModelId != expectedModelId ||
                    liveModel.Name != prepared.InventoryName ||
                    !liveModel.MaxCapacity.HasValue ||
                    liveModel.MaxCapacity != preparedMaxCapacity ||
                    (liveItem.Revision > 0 && liveItem.Revision != preparedRevision)) {
                    return null;
                }
                liveItem = liveItem.Clone() as Gs2.Gs2Inventory.Model.Inventory;
                liveItem.CurrentInventoryMaxCapacity = Math.Min(
                    prepared.NewCapacityValue.Value,
                    liveModel.MaxCapacity.Value
                );
                liveItem.Revision = 0;
                domain.Cache.Put(
                    liveItem.CacheParentKey(
                        prepared.NamespaceName,
                        token.UserId,
                        token.TimeOffset
                    ),
                    liveItem.CacheKey(prepared.InventoryName),
                    liveItem,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
