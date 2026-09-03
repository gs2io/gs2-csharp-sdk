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
using System.Linq;
using System.Numerics;
using Gs2.Core.Domain;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Exchange.Model.Cache;
using Gs2.Gs2Exchange.Request;
using Await = Gs2.Gs2Exchange.Model.Await;
using Config = Gs2.Gs2Exchange.Model.Config;
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
    public static class AcquireByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Exchange:AcquireByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AcquireByUserIdRequest request
        ) {
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = AcquireByUserIdRequest.FromJson(request?.ToJson());
            if (prepared?.UserId == "#{userId}") prepared.UserId = token?.UserId;
            if (domain?.RestSession == null || string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId) {
                return null;
            }

            var cachedAwait = ((Await)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.AwaitName, token.TimeOffset
            );
            var item = cachedAwait.Item1;
            var expectedAwaitId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:exchange:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:await:{prepared.AwaitName}";
            if (!cachedAwait.Item2 || item == null ||
                item.AwaitId != expectedAwaitId || item.UserId != token.UserId ||
                item.Name != prepared.AwaitName || item.RateName == null) {
                return null;
            }
            var cachedRate = ((RateModel)null).GetCache(
                domain.Cache, prepared.NamespaceName, item.RateName, null
            );
            var rateFound = cachedRate.Item2;
            var rateModel = cachedRate.Item1;
            var expectedRateId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:exchange:{prepared.NamespaceName}:" +
                $"model:{item.RateName}";
            if (rateFound && (rateModel == null ||
                rateModel.RateModelId != expectedRateId ||
                rateModel.Name != item.RateName)) {
                return null;
            }

            long acquirableAt;
            if (rateFound) {
                if (!rateModel.LockTime.HasValue ||
                    !item.ExchangedAt.HasValue ||
                    !item.SkipSeconds.HasValue) {
                    return null;
                }
                var lockSeconds = unchecked(rateModel.LockTime.Value * 60);
                acquirableAt = item.ExchangedAt.Value +
                    ((long)lockSeconds - item.SkipSeconds.Value) * 1000L;
            }
            else {
                if (!item.AcquirableAt.HasValue) return null;
                acquirableAt = item.AcquirableAt.Value;
            }
            var now = UnixTime.ToUnixTime(DateTime.Now) +
                      (long)(token.TimeOffset ?? 0) * 1000L;
            if (acquirableAt > now) return null;

            var configs = new List<Config>(
                (prepared.Config ?? Array.Empty<Config>())
                    .Where(v => v != null)
            );
            foreach (var config in item.Config ?? Array.Empty<Config>()) {
                if (config != null && configs.All(v => v.Key != config.Key)) {
                    configs.Add(config);
                }
            }

            AcquireAction[] acquireActions;
            try {
                acquireActions = (rateModel?.AcquireActions ??
                        Array.Empty<AcquireAction>())
                    .Where(v => v != null)
                    .Select(v => {
                        var action = v.ApplyConfig("userId", token.UserId);
                        foreach (var config in configs) {
                            if (config.Value != null) {
                                action = action.ApplyConfig(
                                    config.Key, config.Value
                                );
                            }
                        }
                        return action;
                    })
                    .ToArray();
            }
            catch (System.Exception) {
                return null;
            }

            var delete = new ConsumeAction()
                .WithAction(Gs2.Gs2Exchange.Domain.SpeculativeExecutor
                    .DeleteAwaitByUserIdSpeculativeExecutor.Action())
                .WithRequest(new DeleteAwaitByUserIdRequest()
                    .WithNamespaceName(prepared.NamespaceName)
                    .WithUserId(token.UserId)
                    .WithAwaitName(prepared.AwaitName)
                    .ToJson().ToJson());
            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                new[] { delete }, acquireActions,
                new BigInteger(item.Count ?? 1)
            ).ExecuteAsync(domain, token);
            if (commit == null) return null;

            var awaitSnapshot = item.ToJson().ToJson();
            var rateSnapshot = rateModel?.ToJson().ToJson();
            return () => {
                var liveAwait = ((Await)null).GetCache(
                    domain.Cache, prepared.NamespaceName, token.UserId,
                    prepared.AwaitName, token.TimeOffset
                );
                var liveRate = ((RateModel)null).GetCache(
                    domain.Cache, prepared.NamespaceName, item.RateName, null
                );
                if (!liveAwait.Item2 ||
                    liveAwait.Item1?.ToJson().ToJson() != awaitSnapshot ||
                    liveRate.Item2 != rateFound ||
                    liveRate.Item1?.ToJson().ToJson() != rateSnapshot) {
                    return null;
                }
                return commit();
            };
        }
    }
}
