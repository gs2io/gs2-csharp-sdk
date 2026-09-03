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
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Exchange.Model.Cache;
using Gs2.Gs2Exchange.Request;
using RateModel = Gs2.Gs2Exchange.Model.RateModel;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Exchange.Domain.Transaction.SpeculativeExecutor
{
    public static class ExchangeByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Exchange:ExchangeByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ExchangeByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ExchangeByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = ExchangeByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId ||
                !prepared.Count.HasValue) {
                return null;
            }

            var cached = ((RateModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.RateName, null
            );
            var item = cached.Item1;

            var expectedRateModelId = string.Join(
                ":",
                "grn",
                "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId,
                "exchange",
                prepared.NamespaceName,
                "model",
                prepared.RateName
            );
            if (!cached.Item2 || item == null ||
                item.Name != prepared.RateName ||
                item.RateModelId != expectedRateModelId ||
                (item.TimingType != "immediate" && item.TimingType != "await")) {
                return null;
            }

            ConsumeAction[] consumeActions;
            AcquireAction[] acquireActions;
            try {
                consumeActions = (item.ConsumeActions ?? Array.Empty<ConsumeAction>())
                    .Where(v => v != null)
                    .Select(v => {
                        var action = v.ApplyConfig("userId", token.UserId);
                        foreach (var config in prepared.Config ??
                                 Array.Empty<Gs2.Gs2Exchange.Model.Config>()) {
                            if (config?.Value != null) {
                                action = action.ApplyConfig(config.Key, config.Value);
                            }
                        }
                        return action;
                    })
                    .ToArray();
                acquireActions = item.TimingType == "await"
                    ? Array.Empty<AcquireAction>()
                    : (item.AcquireActions ?? Array.Empty<AcquireAction>())
                        .Where(v => v != null &&
                            v.Action != Gs2.Gs2Exchange.Domain
                                .SpeculativeExecutor.ExchangeByUserIdSpeculativeExecutor
                                .Action())
                        .Select(v => {
                            var action = v.ApplyConfig("userId", token.UserId);
                            foreach (var config in prepared.Config ??
                                     Array.Empty<Gs2.Gs2Exchange.Model.Config>()) {
                                if (config?.Value != null) {
                                    action = action.ApplyConfig(config.Key, config.Value);
                                }
                            }
                            return action;
                        })
                        .ToArray();
            }
            catch (System.Exception) {
                return null;
            }
            if (consumeActions.Length == 0 && acquireActions.Length == 0) {
                return null;
            }
            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                consumeActions,
                acquireActions,
                new BigInteger(prepared.Count.Value)
            ).ExecuteAsync(
                domain,
                token
            );
            if (commit == null) return null;
            var snapshot = item.ToJson().ToJson();
            return () => {
                var live = ((RateModel)null).GetCache(
                    domain.Cache, prepared.NamespaceName, prepared.RateName, null
                );
                if (!live.Item2 || live.Item1?.ToJson().ToJson() != snapshot) {
                    return null;
                }
                return commit();
            };
        }
    }
}
