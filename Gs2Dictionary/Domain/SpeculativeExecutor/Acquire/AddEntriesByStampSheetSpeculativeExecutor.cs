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
using Gs2.Core.Model; /* diff +++ */
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
    public static class AddEntriesByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Dictionary:AddEntriesByUserId";
        }

        private static long CurrentTimeMillis(AccessToken accessToken) {
            return UnixTime.ToUnixTime(DateTime.Now) +
                   (long)(accessToken?.TimeOffset ?? 0) * 1000L;
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
            var token = accessToken?.Clone() as AccessToken;
            if (domain == null || request == null || string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = AddEntriesByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId) {
                return null;
            }

            var additions = new List<Tuple<string, Gs2.Gs2Dictionary.Model.Entry>>();
            var handled = 0;
            var seen = new HashSet<string>();
            foreach (var entryModelName in prepared.EntryModelNames ?? Array.Empty<string>()) {
                if (entryModelName == null || !seen.Add(entryModelName)) {
                    continue;
                }
                var modelCached = ((Gs2.Gs2Dictionary.Model.EntryModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    entryModelName,
                    null
                );
                var model = modelCached.Item1;
                var expectedModelId =
                    $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                    $"{domain.RestSession.OwnerId}:dictionary:{prepared.NamespaceName}:" +
                    $"model:{entryModelName}";
                if (!modelCached.Item2 || model == null ||
                    model.EntryModelId != expectedModelId || model.Name != entryModelName) {
                    continue;
                }

                var entryCached = ((Gs2.Gs2Dictionary.Model.Entry)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    entryModelName,
                    token.TimeOffset
                );
                if (!entryCached.Item2) {
                    continue;
                }
                if (entryCached.Item1 != null) {
                    var expectedEntryId =
                        $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                        $"{domain.RestSession.OwnerId}:dictionary:{prepared.NamespaceName}:" +
                        $"user:{token.UserId}:entry:{entryModelName}";
                    if (entryCached.Item1.EntryId != expectedEntryId ||
                        entryCached.Item1.UserId != token.UserId ||
                        entryCached.Item1.Name != entryModelName) {
                        continue;
                    }
                }

                handled++;
                additions.Add(Tuple.Create(
                    entryModelName,
                    new Gs2.Gs2Dictionary.Model.Entry {
                        EntryId =
                            $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                            $"{domain.RestSession.OwnerId}:dictionary:{prepared.NamespaceName}:" +
                            $"user:{token.UserId}:entry:{entryModelName}",
                        UserId = token.UserId,
                        Name = entryModelName,
                        AcquiredAt = CurrentTimeMillis(token),
                    }
                ));
            }
            if (handled == 0) {
                return null;
            }

            return () => {
                foreach (var addition in additions) {
                    var model = ((Gs2.Gs2Dictionary.Model.EntryModel)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        addition.Item1,
                        null
                    );
                    var entry = ((Gs2.Gs2Dictionary.Model.Entry)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        token.UserId,
                        addition.Item1,
                        token.TimeOffset
                    );
                    if (model.Item2 && model.Item1 != null &&
                        model.Item1.EntryModelId ==
                            $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                            $"{domain.RestSession.OwnerId}:dictionary:{prepared.NamespaceName}:" +
                            $"model:{addition.Item1}" &&
                        model.Item1.Name == addition.Item1 &&
                        entry.Item2 && entry.Item1 == null) {
                        addition.Item2.PutCache(
                            domain.Cache,
                            prepared.NamespaceName,
                            token.UserId,
                            addition.Item1,
                            token.TimeOffset
                        );
                    }
                }
                return null;
            };
        }
    }
}
