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
    public static class VerifyItemSetByUserIdSpeculativeExecutor {
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyItemSetByUserIdRequest _request;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyItemSetByUserIdRequest request
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
            }

            public bool IsStillSatisfied()
            {
                if (!TryGetItems(
                        _domain,
                        _accessToken,
                        _request,
                        out var items)) {
                    return false;
                }
                try {
                    Transform(_domain, _accessToken, _request, items);
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
            return "Gs2Inventory:VerifyItemSetByUserId";
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyItemSetByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyItemSetByUserIdRequest.FromJson(request.ToJson());
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

        public static Gs2.Gs2Inventory.Model.ItemSet[] Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyItemSetByUserIdRequest request,
            Gs2.Gs2Inventory.Model.ItemSet[] items
        ) {
            if (request.Count == null || items == null || items.Any(v => v?.Count == null)) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            var count = 0L;
            foreach (var item in items) {
                count = unchecked(count + item.Count.Value);
            }
            var expectedCount = request.Count.Value;
            var executable = request.VerifyType switch {
                "less" => count < expectedCount,
                "lessEqual" => count <= expectedCount,
                "greater" => count > expectedCount,
                "greaterEqual" => count >= expectedCount,
                "equal" => count == expectedCount,
                "notEqual" => count != expectedCount,
                _ => false,
            };
            if (!executable) {
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
            VerifyItemSetByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyItemSetByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = VerifyItemSetByUserIdRequest.FromJson(request.ToJson());
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
                 prepared.VerifyType != "notEqual") ||
                !prepared.Count.HasValue) {
                return null;
            }
            if (!TryGetItems(domain, token, prepared, out var items)) {
                return null;
            }
            Transform(domain, token, prepared, items);

            return new PreparedVerification(domain, token, prepared).Invoke;
        }

        private static bool TryGetItems(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyItemSetByUserIdRequest request,
            out Gs2.Gs2Inventory.Model.ItemSet[] items
        ) {
            items = null;
            if (request.ItemSetName == null) {
                var cached = ((Gs2.Gs2Inventory.Model.ItemSet[])null).GetCache(
                    domain.Cache,
                    request.NamespaceName,
                    accessToken.UserId,
                    request.InventoryName,
                    request.ItemName,
                    accessToken.TimeOffset
                );
                if (!cached.Item2 || cached.Item1 == null) {
                    return false;
                }
                items = cached.Item1;
            } else {
                var cached = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                    domain.Cache,
                    request.NamespaceName,
                    accessToken.UserId,
                    request.InventoryName,
                    request.ItemName,
                    request.ItemSetName,
                    accessToken.TimeOffset
                );
                if (!cached.Item2) {
                    return false;
                }
                items = cached.Item1 == null
                    ? Array.Empty<Gs2.Gs2Inventory.Model.ItemSet>()
                    : new[] { cached.Item1 };
            }
            return items.All(v => IsCanonical(
                domain,
                accessToken.UserId,
                request,
                v
            ) && v.Count.HasValue);
        }

        private static bool IsCanonical(
            Gs2.Core.Domain.Gs2 domain,
            string userId,
            VerifyItemSetByUserIdRequest request,
            Gs2.Gs2Inventory.Model.ItemSet item
        ) {
            if (item == null || string.IsNullOrEmpty(item.Name) ||
                (request.ItemSetName != null &&
                 item.Name != request.ItemSetName)) {
                return false;
            }
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{request.NamespaceName}:" +
                $"user:{userId}:inventory:{request.InventoryName}:" +
                $"item:{request.ItemName}:itemSet:{item.Name}";
            return item.ItemSetId == expectedId && item.UserId == userId &&
                   item.InventoryName == request.InventoryName &&
                   item.ItemName == request.ItemName;
        }

    }
}
