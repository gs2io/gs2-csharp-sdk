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
 *
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
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inventory.Model;
using Gs2.Gs2Inventory.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
    public static class SetSimpleItemsByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:SetSimpleItemsByUserId";
        }
        public static List<Gs2.Gs2Inventory.Model.SimpleItem> Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetSimpleItemsByUserIdRequest request,
            List<Gs2.Gs2Inventory.Model.SimpleItem> items
        ) {
            foreach (var count in request.Counts) {
                var item = items.FirstOrDefault(v => v.ItemName == count.ItemName);
                if (item == null) {
                    item = new SimpleItem {
                        UserId = accessToken.UserId,
                        ItemName = count.ItemName,
                        Count = 0,
                        Revision = 0
                    };
                }
                item.Count = count.Count;
                items = items.Where(v => v.ItemName != count.ItemName).Concat(new []{ item }).ToList();
            }
            items.Sort((v1, v2) => string.Compare(v1.ItemName, v2.ItemName, StringComparison.Ordinal));
            return items;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetSimpleItemsByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {

                var it = domain.Inventory.Namespace(
                    request.NamespaceName
                ).AccessToken(
                    accessToken
                ).SimpleInventory(
                    request.InventoryName
                ).SimpleItems(
                );
                var items = new List<Gs2.Gs2Inventory.Model.SimpleItem>();
                while (it.HasNext()) {
                    yield return it.Next();
                    if (it.Error != null) {
                        result.OnError(it.Error);
                        yield break;
                    }
                    if (it.Current != null) {
                        items.Add(it.Current);
                    }
                }

                if (items == null) {
                    result.OnComplete(() =>
                    {
                        return null;
                    });
                    yield break;
                }
                items = Transform(domain, accessToken, request, items);
                
                result.OnComplete(() =>
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                        request.NamespaceName,
                        accessToken.UserId,
                        request.InventoryName,
                        "SimpleItem"
                    );
                    foreach (var acquireCount in request.Counts) {
                        var item = items.First(v => v.ItemName == acquireCount.ItemName);
                        var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                            item.ItemName.ToString()
                        );
                        domain.Cache.Put<Gs2.Gs2Inventory.Model.SimpleItem>(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                        );
                    }
                    return null;
                });
                yield return null;
            }

            return new Gs2InlineFuture<Func<object>>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Func<object>> ExecuteAsync(
    #else
        public static async Task<Func<object>> ExecuteAsync(
    #endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetSimpleItemsByUserIdRequest request
        ) {
            var items = await domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).SimpleInventory(
                request.InventoryName
            ).SimpleItemsAsync(
            ).ToListAsync();

            if (items == null) {
                return () => null;
            }
            items = Transform(domain, accessToken, request, items);

            return () =>
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.SimpleInventoryDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    request.InventoryName,
                    "SimpleItem"
                );
                foreach (var acquireCount in request.Counts) {
                    var item = items.First(v => v.ItemName == acquireCount.ItemName);
                    var key = Gs2.Gs2Inventory.Domain.Model.SimpleItemDomain.CreateCacheKey(
                        item.ItemName.ToString()
                    );
                    domain.Cache.Put<Gs2.Gs2Inventory.Model.SimpleItem>(
                        parentKey,
                        key,
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                    );
                }
                return null;
            };
        }
#endif

        public static SetSimpleItemsByUserIdRequest Rate(
            SetSimpleItemsByUserIdRequest request,
            double rate
        ) {
            return request;
        }

        public static SetSimpleItemsByUserIdRequest Rate(
            SetSimpleItemsByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}
