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
using System.Linq;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.Model;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Request;
using PropertyForm = Gs2.Gs2Formation.Model.PropertyForm;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.Transaction.SpeculativeExecutor
{
    public static class AcquireActionsToPropertyFormPropertiesSpeculativeExecutor {

        public static string Action() {
            return "Gs2Formation:AcquireActionsToPropertyFormProperties";
        }

        private static string Snapshot(PropertyForm item) {
            if (item?.Clone() is not PropertyForm clone) {
                return null;
            }
            clone.Slots = item.Slots?
                .Where(slot => slot != null)
                .Select(slot => slot.Clone() as Gs2.Gs2Formation.Model.Slot)
                .ToArray();
            return clone.ToJson().ToJson();
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireActionsToPropertyFormPropertiesRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireActionsToPropertyFormPropertiesRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared =
                AcquireActionsToPropertyFormPropertiesRequest.FromJson(
                    request?.ToJson()
                );
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) || prepared == null ||
                prepared.UserId != token.UserId ||
                prepared.AcquireAction == null) {
                return null;
            }

            var cached = ((PropertyForm)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.PropertyFormModelName, prepared.PropertyId,
                token.TimeOffset
            );
            var item = cached.Item1;
            var expectedFormId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:formation:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:" +
                $"propertyForm:{prepared.PropertyFormModelName}:" +
                $"{prepared.PropertyId}";
            if (!cached.Item2 || item == null ||
                item.FormId != expectedFormId ||
                item.UserId != token.UserId ||
                item.Name != prepared.PropertyFormModelName ||
                item.PropertyId != prepared.PropertyId ||
                item.Slots == null ||
                prepared.AcquireAction.Action == Action() ||
                prepared.AcquireAction.Action ==
                    AcquireActionsToFormPropertiesSpeculativeExecutor.Action()) {
                return null;
            }

            AcquireAction[] actions;
            try {
                actions = item.Slots.Where(slot => slot != null).Select(slot => {
                    var action = prepared.AcquireAction.ApplyConfig(
                        "propertyId", slot.PropertyId ?? ""
                    ).ApplyConfig("userId", token.UserId);
                    foreach (var config in prepared.Config ??
                             Array.Empty<Gs2.Gs2Formation.Model.Config>()) {
                        if (config?.Value != null) {
                            action = action.ApplyConfig(
                                config.Key, config.Value
                            );
                        }
                    }
                    return action;
                }).ToArray();
                if (item.Slots.Length > 0 && actions.Length == 0) {
                    return null;
                }
            }
            catch (System.Exception) {
                return null;
            }

            Func<object> commit;
            try {
                commit = await new Core.SpeculativeExecutor
                    .SpeculativeExecutor(
                        Array.Empty<ConsumeAction>(), actions, BigInteger.One
                    )
                    .ExecuteAsync(domain, token);
            }
            catch (System.Exception) {
                return null;
            }
            if (commit == null) return null;

            var snapshot = Snapshot(item);
            return () => {
                var live = ((PropertyForm)null).GetCache(
                    domain.Cache, prepared.NamespaceName, token.UserId,
                    prepared.PropertyFormModelName, prepared.PropertyId,
                    token.TimeOffset
                );
                if (!live.Item2 ||
                    Snapshot(live.Item1) != snapshot) {
                    return null;
                }
                return commit();
            };
        }
    }
}
