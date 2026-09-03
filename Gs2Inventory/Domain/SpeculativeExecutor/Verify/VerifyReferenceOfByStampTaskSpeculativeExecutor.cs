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
    public static class VerifyReferenceOfByUserIdSpeculativeExecutor {
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyReferenceOfByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyReferenceOfByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                if (!TryGetReferences(
                        _domain,
                        _accessToken,
                        _request,
                        _expectedId,
                        out var references)) {
                    return false;
                }
                try {
                    Transform(_domain, _accessToken, _request, references);
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
            return "Gs2Inventory:VerifyReferenceOfByUserId";
        }

        public static List<string> Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request,
            List<string> items
        ) {
            if (!items.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("count", "invalid"),
                });
            }
            return items;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyReferenceOfByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "not_entry": inverse.VerifyType = "already_entry"; break;
                case "already_entry": inverse.VerifyType = "not_entry"; break;
                case "empty": inverse.VerifyType = "not_empty"; break;
                case "not_empty": inverse.VerifyType = "empty"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
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
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) {
                return null;
            }
            var prepared = VerifyReferenceOfByUserIdRequest.FromJson(request.ToJson());
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.InventoryName) ||
                string.IsNullOrEmpty(prepared.ItemName) ||
                string.IsNullOrEmpty(prepared.ItemSetName) ||
                prepared.ReferenceOf == null ||
                (prepared.VerifyType != "not_entry" &&
                 prepared.VerifyType != "already_entry" &&
                 prepared.VerifyType != "empty" &&
                 prepared.VerifyType != "not_empty")) {
                return null;
            }
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:inventory:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:inventory:{prepared.InventoryName}:" +
                $"item:{prepared.ItemName}:itemSet:{prepared.ItemSetName}";
            if (!TryGetReferences(
                    domain,
                    token,
                    prepared,
                    expectedId,
                    out var references)) {
                return null;
            }
            if (!references.IsExecutable(prepared)) {
                return null;
            }
            Transform(domain, token, prepared, references);

            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedId
            ).Invoke;
        }

        private static bool TryGetReferences(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyReferenceOfByUserIdRequest request,
            string expectedId,
            out List<string> references
        ) {
            references = null;
            var (item, found) = ((Gs2.Gs2Inventory.Model.ItemSet)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                accessToken.UserId,
                request.InventoryName,
                request.ItemName,
                request.ItemSetName,
                accessToken.TimeOffset
            );
            if (!found || item == null || item.ItemSetId != expectedId ||
                item.UserId != accessToken.UserId ||
                item.InventoryName != request.InventoryName ||
                item.ItemName != request.ItemName ||
                item.Name != request.ItemSetName || item.ReferenceOf == null) {
                return false;
            }
            references = item.ReferenceOf.ToList();
            return true;
        }
    }
}
