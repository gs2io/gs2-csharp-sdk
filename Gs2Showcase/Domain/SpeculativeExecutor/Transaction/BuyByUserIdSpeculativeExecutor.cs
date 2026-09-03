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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Showcase.Model.Cache;
using Gs2.Gs2Showcase.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Showcase.Domain.Transaction.SpeculativeExecutor
{
    public static class BuyByUserIdSpeculativeExecutor {
        private const int MaxSpeculativeActionCount = 4096;

        public static string Action() {
            return "Gs2Showcase:BuyByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BuyByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BuyByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain == null || request == null || string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = BuyByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId || prepared.Quantity == null ||
                prepared.Quantity <= 0) {
                return null;
            }

            var cached = ((Gs2.Gs2Showcase.Model.DisplayItem)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.ShowcaseName,
                prepared.DisplayItemId,
                token.TimeOffset
            );
            var item = cached.Item1;
            if (!cached.Item2 || item == null ||
                item.DisplayItemId != prepared.DisplayItemId ||
                item.Type != "salesItem" || item.SalesItem == null) {
                return null;
            }
            var expected = item.ToJson().ToJson();

            var consumeSources = item.SalesItem.ConsumeActions ??
                                 Array.Empty<ConsumeAction>();
            var acquireSources = item.SalesItem.AcquireActions ??
                                 Array.Empty<AcquireAction>();
            var sourceCount = consumeSources.Length + acquireSources.Length;
            if (sourceCount == 0 ||
                (long)sourceCount * prepared.Quantity.Value > MaxSpeculativeActionCount) {
                // This is a local resource budget, not server-side request validation.
                return null;
            }

            Func<object> commit;
            try {
                var consumeActions = new List<ConsumeAction>();
                var acquireActions = new List<AcquireAction>();
                for (var i = 0; i < prepared.Quantity.Value; i++) {
                    foreach (var source in consumeSources) {
                        var action = (source?.Clone() as ConsumeAction)
                            ?.ApplyConfig("userId", token.UserId);
                        foreach (var config in prepared.Config ??
                                 Array.Empty<Gs2.Gs2Showcase.Model.Config>()) {
                            if (config != null) {
                                action = action?.ApplyConfig(config.Key, config.Value);
                            }
                        }
                        if (action != null) {
                            consumeActions.Add(action);
                        }
                    }
                    foreach (var source in acquireSources) {
                        var action = (source?.Clone() as AcquireAction)
                            ?.ApplyConfig("userId", token.UserId);
                        foreach (var config in prepared.Config ??
                                 Array.Empty<Gs2.Gs2Showcase.Model.Config>()) {
                            if (config != null) {
                                action = action?.ApplyConfig(config.Key, config.Value);
                            }
                        }
                        if (action != null) {
                            acquireActions.Add(action);
                        }
                    }
                }
                if (consumeActions.Count == 0 && acquireActions.Count == 0) {
                    return null;
                }

                commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                    consumeActions.ToArray(),
                    acquireActions.ToArray(),
                    1.0
                ).ExecuteAsync(domain, token);
            }
            catch (Exception) {
                return null;
            }
            if (commit == null) {
                return null;
            }

            return () => {
                var current = ((Gs2.Gs2Showcase.Model.DisplayItem)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    token.UserId,
                    prepared.ShowcaseName,
                    prepared.DisplayItemId,
                    token.TimeOffset
                );
                if (current.Item2 && current.Item1 != null &&
                    current.Item1.ToJson().ToJson() == expected) {
                    commit();
                }
                return null;
            };
        }
    }
}
