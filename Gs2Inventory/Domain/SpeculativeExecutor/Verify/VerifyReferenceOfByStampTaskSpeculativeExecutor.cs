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
using System.Collections.Generic; /* diff +++ */
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
/* diff +++ start */
using Gs2.Core.Model;
/* diff +++ end */
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
    public static class VerifyReferenceOfByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inventory:VerifyReferenceOfByUserId";
/* diff +++ start */
        }

        public static List<string> Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request,
            List<string> items
        ) {
            switch (request.VerifyType) {
                case "not_entry":
                    if (items.Contains(request.ReferenceOf)) {
                        throw new BadRequestException(new [] {
                            new RequestError("count", "invalid"),
                        });
                    }
                    break;
                case "already_entry":
                    if (!items.Contains(request.ReferenceOf)) {
                        throw new BadRequestException(new [] {
                            new RequestError("count", "invalid"),
                        });
                    }
                    break;
                case "empty":
                    if (items.Count != 0) {
                        throw new BadRequestException(new [] {
                            new RequestError("count", "invalid"),
                        });
                    }
                    break;
                case "not_empty":
                    if (items.Count == 0) {
                        throw new BadRequestException(new [] {
                            new RequestError("count", "invalid"),
                        });
                    }
                    break;
            }
            return items;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Inventory.Namespace(
 diff --- end */
            var items = await domain.Inventory.Namespace( /* diff +++ */
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Inventory(
                request.InventoryName
            ).ItemSet(
                request.ItemName,
                request.ItemSetName
/* diff --- start
            ).ReferenceOf(
                request.ReferenceOf
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            ).ReferenceOvesAsync(
            ).ToListAsync();
/* diff +++ end */

/* diff --- start
            if (item == null) {
 diff --- end */
            if (items == null) { /* diff +++ */
                return () => null;
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
            items = Transform(domain, accessToken, request, items); /* diff +++ */

            return () =>
            {
                return null;
            };
        }
    }
}
