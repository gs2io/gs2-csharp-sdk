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
    public static class AcquireItemSetWithGradeByUserIdSpeculativeExecutor {

        private sealed class PreparedInventoryCapacity
        {
            private readonly CacheDatabase _cache;
            private readonly string _namespaceName;
            private readonly string _userId;
            private readonly string _inventoryName;
            private readonly int? _timeOffset;
            private readonly string _expectedId;
            private readonly string _preparedSnapshot;
            private readonly long _updatedAt;

            internal PreparedInventoryCapacity(
                CacheDatabase cache,
                string namespaceName,
                string userId,
                string inventoryName,
                int? timeOffset,
                string expectedId,
                string preparedSnapshot,
                long updatedAt
            ) {
                _cache = cache;
                _namespaceName = namespaceName;
                _userId = userId;
                _inventoryName = inventoryName;
                _timeOffset = timeOffset;
                _expectedId = expectedId;
                _preparedSnapshot = preparedSnapshot;
                _updatedAt = updatedAt;
            }

            internal object Commit()
            {
                try {
                    var cached = ((Gs2.Gs2Inventory.Model.Inventory)null)
                        .GetCache(
                            _cache,
                            _namespaceName,
                            _userId,
                            _inventoryName,
                            _timeOffset
                        );
                    var inventory = cached.Item1;
                    if (!cached.Item2 || inventory == null ||
                        inventory.InventoryId != _expectedId ||
                        inventory.UserId != _userId ||
                        inventory.InventoryName != _inventoryName ||
                        !inventory.CurrentInventoryCapacityUsage.HasValue ||
                        !inventory.CurrentInventoryMaxCapacity.HasValue ||
                        inventory.CurrentInventoryCapacityUsage >=
                        inventory.CurrentInventoryMaxCapacity ||
                        !inventory.Revision.HasValue || inventory.Revision < 0 ||
                        (inventory.Revision > 0 && !string.Equals(
                            inventory.ToJson().ToJson(),
                            _preparedSnapshot,
                            StringComparison.Ordinal
                        ))) {
                        return null;
                    }
                    var changed = inventory.Clone() as
                        Gs2.Gs2Inventory.Model.Inventory;
                    if (changed == null) return null;
                    changed.CurrentInventoryCapacityUsage = checked(
                        inventory.CurrentInventoryCapacityUsage.Value + 1
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
                    // The generated ItemSet id is intentionally not predicted.
                }
                return null;
            }
        }

        public static string Action() {
            return "Gs2Inventory:AcquireItemSetWithGradeByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetWithGradeByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetWithGradeByUserIdRequest request
        ) {
            try {
                var token = accessToken?.Clone() as AccessToken;
                var prepared = request == null
                    ? null
                    : AcquireItemSetWithGradeByUserIdRequest.FromJson(
                        request.ToJson()
                    );
                if (prepared?.UserId == "#{userId}") {
                    prepared.UserId = token?.UserId;
                }
                if (domain?.RestSession == null ||
                    string.IsNullOrEmpty(token?.UserId) ||
                    prepared?.UserId != token.UserId) {
                    return null;
                }
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:inventory:" +
                    $"{prepared.NamespaceName}:user:{token.UserId}:" +
                    $"inventory:{prepared.InventoryName}";
                var cached = ((Gs2.Gs2Inventory.Model.Inventory)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    token.TimeOffset
                );
                var inventory = cached.Item1;
                if (!cached.Item2 || inventory == null ||
                    inventory.InventoryId != expectedId ||
                    inventory.UserId != token.UserId ||
                    inventory.InventoryName != prepared.InventoryName ||
                    !inventory.CurrentInventoryCapacityUsage.HasValue ||
                    !inventory.CurrentInventoryMaxCapacity.HasValue ||
                    inventory.CurrentInventoryCapacityUsage >=
                    inventory.CurrentInventoryMaxCapacity ||
                    !inventory.Revision.HasValue || inventory.Revision < 0) {
                    return null;
                }
                var updatedAt = UnixTime.ToUnixTime(DateTime.Now) +
                    (long)(token.TimeOffset ?? 0) * 1000L;
                return new PreparedInventoryCapacity(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.InventoryName,
                    token.TimeOffset,
                    expectedId,
                    inventory.ToJson().ToJson(),
                    updatedAt
                ).Commit;
            }
            catch (System.Exception) {
                return null;
            }
        }
    }
}
