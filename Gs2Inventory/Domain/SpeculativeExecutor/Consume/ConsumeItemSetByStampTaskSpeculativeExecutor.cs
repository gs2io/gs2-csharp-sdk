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
using Gs2.Core.Exception;
using Gs2.Core.Model;
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
    public static class ConsumeItemSetByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:ConsumeItemSetByUserId";
        }

        public static Gs2.Gs2Inventory.Model.ItemSet[] Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeItemSetByUserIdRequest request,
            Gs2.Gs2Inventory.Model.ItemSet[] items
        ) {
            var consumeCount = request.ConsumeCount;
            foreach (var item in items.Reverse()) {
                if (consumeCount <= item.Count) {
                    item.Count -= consumeCount;
                    consumeCount = 0;
                    break;   
                }
                item.Count = 0;
                consumeCount -= item.Count;
            }
            if (consumeCount > 0) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            return items;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumeItemSetByUserIdRequest request
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
                    string.IsNullOrEmpty(request.ItemSetName) ? null : request.ItemSetName
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
                try {
                    items = Transform(domain, accessToken, request, items);
                }
                catch (Gs2Exception e) {
                    result.OnError(e);
                    yield break;
                }

                result.OnComplete(() =>
                {
                    var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                        request.NamespaceName,
                        accessToken.UserId,
                        request.InventoryName,
                        "ItemSet"
                    );
                    foreach (var item in items) {
                        var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                            request.ItemName.ToString(),
                            item.Name.ToString()
                        );
                        if (item.Count == 0) {
                            domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                null,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                            );
                        }
                        else {
                            domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                                parentKey,
                                key,
                                item,
                                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                            );
                        }
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
            ConsumeItemSetByUserIdRequest request
        ) {
            var items = await domain.Inventory.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Inventory(
                request.InventoryName
            ).ItemSet(
                request.ItemName,
                string.IsNullOrEmpty(request.ItemSetName) ? null : request.ItemSetName
            ).ModelAsync();

            if (items == null) {
                return () => null;
            }
            items = Transform(domain, accessToken, request, items);

            return () =>
            {
                var parentKey = Gs2.Gs2Inventory.Domain.Model.InventoryDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    request.InventoryName,
                    "ItemSet"
                );
                foreach (var item in items) {
                    var key = Gs2.Gs2Inventory.Domain.Model.ItemSetDomain.CreateCacheKey(
                        request.ItemName.ToString(),
                        item.Name.ToString()
                    );
                    if (item.Count == 0) {
                        domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            null,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                        );
                    }
                    else {
                        domain.Cache.Put<Gs2.Gs2Inventory.Model.ItemSet>(
                            parentKey,
                            key,
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                        );
                    }
                }
                return null;
            };
        }
#endif

        public static ConsumeItemSetByUserIdRequest Rate(
            ConsumeItemSetByUserIdRequest request,
            double rate
        ) {
            request.ConsumeCount = (long?) (request.ConsumeCount * rate);
            return request;
        }

        public static ConsumeItemSetByUserIdRequest Rate(
            ConsumeItemSetByUserIdRequest request,
            BigInteger rate
        ) {
            request.ConsumeCount = (long?) ((request.ConsumeCount ?? 0) * rate);
            return request;
        }
    }
}
