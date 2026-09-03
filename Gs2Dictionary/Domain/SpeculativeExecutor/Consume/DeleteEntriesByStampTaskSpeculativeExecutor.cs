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
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Model.Cache;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
{
    public static class DeleteEntriesByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Dictionary:DeleteEntriesByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteEntriesByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteEntriesByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain == null || request == null || string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = DeleteEntriesByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId) {
                return null;
            }

            var entries = new List<Tuple<string, string>>();
            var seen = new HashSet<string>();
            foreach (var entryModelName in prepared.EntryModelNames ?? Array.Empty<string>()) {
                if (entryModelName == null || !seen.Add(entryModelName)) {
                    continue;
                }
                var cached = ((Gs2.Gs2Dictionary.Model.Entry)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    entryModelName,
                    token.TimeOffset
                );
                var item = cached.Item1;
                var expectedId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:dictionary:{prepared.NamespaceName}:" +
                    $"user:{token.UserId}:entry:{entryModelName}";
                if (cached.Item2 && item != null &&
                    item.EntryId == expectedId && item.UserId == token.UserId &&
                    item.Name == entryModelName) {
                    entries.Add(Tuple.Create(entryModelName, item.ToJson().ToJson()));
                }
            }
            if (entries.Count == 0) {
                return null;
            }

            return () => {
                foreach (var entry in entries) {
                    var current = ((Gs2.Gs2Dictionary.Model.Entry)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        token.UserId,
                        entry.Item1,
                        token.TimeOffset
                    );
                    if (current.Item2 && current.Item1 != null &&
                        current.Item1.ToJson().ToJson() == entry.Item2) {
                        (null as Gs2.Gs2Dictionary.Model.Entry).PutCache(
                            domain.Cache,
                            prepared.NamespaceName,
                            token.UserId,
                            entry.Item1,
                            token.TimeOffset
                        );
                    }
                }
                return null;
            };
        }
    }
}
