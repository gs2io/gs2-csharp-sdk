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
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Model.Cache;
using Gs2.Gs2Dictionary.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq; /* diff +++ */
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
{
    public static class AddEntriesByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Dictionary:AddEntriesByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddEntriesByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddEntriesByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var items = await domain.Dictionary.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).EntriesAsync().ToListAsync();

            var items_ = items.ToArray().SpeculativeExecution(request);

            return () =>
            {
                foreach (var item in items_) {
                    item.PutCache(
                        domain.Cache,
                        request.NamespaceName,
                        accessToken.UserId,
                        item.Name,
                        accessToken?.TimeOffset
                    );
                }
                return null;
            };
/* diff +++ end */
        }
    }
}
