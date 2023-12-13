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
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inventory.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
    public static class AcquireItemSetByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:AcquireItemSetByUserId";
        }

        public static Gs2.Gs2Inventory.Model.ItemSet[] Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetByUserIdRequest request,
            Gs2.Gs2Inventory.Model.ItemSet[] items
        ) {
            if (items.Length > 0) {
                // ReSharper disable once UseIndexFromEndExpression
                var item = items[items.Length-1];
                item.Count += request.AcquireCount;
            }
            return items;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireItemSetByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                var future = domain.Inventory.Namespace(
                    request.NamespaceName
                ).AccessToken(
                    accessToken
                ).Inventory(
                    request.InventoryName
                ).ItemSet(
                    request.ItemName,
                    request.ItemSetName
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var items = future.Result;

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
                    if (items.Length > 0) {
                        // ReSharper disable once UseIndexFromEndExpression
                        var item = items[items.Length-1];
                        var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                            request.NamespaceName,
                            accessToken.UserId,
                            request.InventoryName,
                            "ItemSet"
                        );
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            request.ItemName.ToString(),
                            item.Name
                        );

                        domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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
            AcquireItemSetByUserIdRequest request
        ) {
            var items = await domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Inventory(
                request.InventoryName
            ).ItemSet(
                request.ItemName,
                request.ItemSetName
            ).ModelAsync();

            if (items == null) {
                return () => null;
            }
            items = Transform(domain, accessToken, request, items);

            return () =>
            {
                if (items.Length > 0) {
                    // ReSharper disable once UseIndexFromEndExpression
                    var item = items[items.Length-1];
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        request.NamespaceName,
                        accessToken.UserId,
                        request.InventoryName,
                        "ItemSet"
                    );
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName.ToString(),
                        item.Name
                    );

                    domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
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

        public static AcquireItemSetByUserIdRequest Rate(
            AcquireItemSetByUserIdRequest request,
            double rate
        ) {
            request.AcquireCount = (long?) (request.AcquireCount * rate);
            return request;
        }

        public static AcquireItemSetByUserIdRequest Rate(
            AcquireItemSetByUserIdRequest request,
            BigInteger rate
        ) {
            request.AcquireCount = (long?) ((request.AcquireCount ?? 0) * rate);
            return request;
        }
    }
}