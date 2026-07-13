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
/* diff +++ start */
using System.Collections.Generic;
using System.Linq;
/* diff +++ end */
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
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
using Cysharp.Threading.Tasks.Linq; /* diff +++ */
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
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
/* diff --- start
            var item = await domain.Inventory.Namespace(
 diff --- end */
/* diff +++ start */
    #if UNITY_2017_1_OR_NEWER
            var items = await domain.Inventory.Namespace(
/* diff +++ end */
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).SimpleInventory(
                request.InventoryName
/* diff --- start
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            ).SimpleItemsAsync(
            ).ToArrayAsync();
    #else
            var it = domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).SimpleInventory(
                request.InventoryName
            ).SimpleItemsAsync(
            );
            var collection = new List<Gs2.Gs2Inventory.Model.SimpleItem>();
            await foreach (var item in it)
            {
                collection.Add(item);
            }
            var items = collection.ToArray();
    #endif
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            items = items.Where(v => request.AcquireCounts.Select(v => v.ItemName).Contains(v.ItemName)).ToArray();
            items = items.SpeculativeExecution(request);
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.InventoryName,
                    request.ItemName,
                    null
                );
 diff --- end */
/* diff +++ start */
                foreach (var item in items) {
                    item.PutCache(
                        domain.Cache,
                        request.NamespaceName,
                        accessToken.UserId,
                        request.InventoryName,
                        item.ItemName,
                        accessToken.TimeOffset
                    );
                }
/* diff +++ end */
                return null;
            };
        }
    }
}
