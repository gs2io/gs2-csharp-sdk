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
using System.Numerics;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model;
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
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inventory.Domain.SpeculativeExecutor
{
    public static class VerifyBigItemByUserIdSpeculativeExecutor {
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyBigItemByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyBigItemByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2Inventory.Model.BigItem)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _accessToken.UserId,
                        _request.InventoryName,
                        _request.ItemName,
                        _accessToken.TimeOffset
                    );
                item ??= BigItemSpeculativeState.KnownZero(
                    _expectedId, _accessToken.UserId, _request.ItemName
                );
                if (!found || item.ItemId != _expectedId ||
                    item.UserId != _accessToken.UserId ||
                    item.ItemName != _request.ItemName ||
                    !CanEvaluate(item, _request)) {
                    return false;
                }
                try {
                    Transform(_domain, _accessToken, _request, item);
                    return true;
                }
                catch (Gs2Exception) {
                    return false;
                }
            }

            public object Invoke()
            {
                return null;
            }
        }

        public static string Action() {
            return "Gs2Inventory:VerifyBigItemByUserId";
        }

        private static bool CanEvaluate(
            Gs2.Gs2Inventory.Model.BigItem item,
            VerifyBigItemByUserIdRequest request
        ) {
            return BigInteger.TryParse(item?.Count, out _) &&
                   BigInteger.TryParse(request?.Count, out _);
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBigItemByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyBigItemByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "less": inverse.VerifyType = "greaterEqual"; break;
                case "lessEqual": inverse.VerifyType = "greater"; break;
                case "greater": inverse.VerifyType = "lessEqual"; break;
                case "greaterEqual": inverse.VerifyType = "less"; break;
                case "equal": inverse.VerifyType = "notEqual"; break;
                case "notEqual": inverse.VerifyType = "equal"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Inventory.Model.BigItem Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBigItemByUserIdRequest request,
            Gs2.Gs2Inventory.Model.BigItem item
        ) {
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBigItemByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyBigItemByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = VerifyBigItemByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.InventoryName) ||
                string.IsNullOrEmpty(prepared.ItemName) ||
                (prepared.VerifyType != "less" &&
                 prepared.VerifyType != "lessEqual" &&
                 prepared.VerifyType != "greater" &&
                 prepared.VerifyType != "greaterEqual" &&
                 prepared.VerifyType != "equal" &&
                 prepared.VerifyType != "notEqual")) {
                return null;
            }
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:big:inventory:{prepared.InventoryName}:" +
                $"item:{prepared.ItemName}";
            var cached = ((Gs2.Gs2Inventory.Model.BigItem)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.InventoryName,
                prepared.ItemName,
                token.TimeOffset
            );
            var item = cached.Item1 ?? BigItemSpeculativeState.KnownZero(
                expectedId, token.UserId, prepared.ItemName
            );
            if (!cached.Item2 || item.ItemId != expectedId ||
                item.UserId != token.UserId || item.ItemName != prepared.ItemName ||
                !CanEvaluate(item, prepared)) {
                return null;
            }
            Transform(domain, token, prepared, item);

            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedId
            ).Invoke;
        }
    }
}
