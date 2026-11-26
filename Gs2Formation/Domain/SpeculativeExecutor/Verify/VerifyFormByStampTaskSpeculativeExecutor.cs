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
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using System.Linq; /* diff +++ */
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.SpeculativeExecutor
{
    public static class VerifyFormByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyFormByUserIdRequest _request;
            private readonly string _expectedFormId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyFormByUserIdRequest request,
                string expectedFormId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedFormId = expectedFormId;
            }

            public bool IsStillSatisfied()
            {
                var item = LocalForm(_domain, _request, _accessToken, _expectedFormId);
                return item != null && item.IsExecutable(_request);
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */
        public static string Action() {
            return "Gs2Formation:VerifyFormByUserId";
/* diff +++ start */
        }

        // キャッシュに正しいフォームの行があるときだけ返す。無ければ null。
        private static Gs2.Gs2Formation.Model.Form LocalForm(
            Gs2.Core.Domain.Gs2 domain,
            VerifyFormByUserIdRequest request,
            AccessToken accessToken,
            string expectedFormId
        ) {
            var (item, found) = ((Gs2.Gs2Formation.Model.Form)null).GetCache(
                domain.Cache,
                request.NamespaceName,
                accessToken.UserId,
                request.MoldModelName,
                request.Index,
                accessToken.TimeOffset
            );
            if (!found || item == null ||
                item.FormId != expectedFormId ||
                item.Slots == null ||
                item.Slots.Any(v => v == null)) {
                return null;
            }
            return item;
        }

        // スロット名がマスターに無いとサーバーは NotFound にするので、マスターのモールドモデルが
        // キャッシュにあって、そのフォームモデルにスロット名があるときだけ手元で判定する。
        private static bool IsKnownSlotName(
            Gs2.Core.Domain.Gs2 domain,
            VerifyFormByUserIdRequest request,
            AccessToken accessToken
        ) {
            if (string.IsNullOrEmpty(request.SlotName)) {
                return true;
            }
            foreach (var timeOffset in new [] { accessToken.TimeOffset, null }) {
                var (moldModel, found) = ((Gs2.Gs2Formation.Model.MoldModel)null).GetCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.MoldModelName,
                    timeOffset
                );
                if (found && moldModel?.FormModel?.Slots != null) {
                    return moldModel.FormModel.Slots.Any(v => v?.Name == request.SlotName);
                }
            }
            return false;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFormByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyFormByUserIdRequest.FromJson(request.ToJson());
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
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFormByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyFormByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Formation.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Mold(
                request.MoldModelName
            ).Form(
                request.Index
            ).ModelAsync();

            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

            return () =>
            {
                return null;
            };
 diff --- end */
/* diff +++ start */
            // キャッシュに正しいフォームがあるときだけ手元で判定し、無ければ null を返してサーバーに任せる。
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyFormByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MoldModelName) ||
                prepared.Index == null ||
                prepared.PropertyIdRegex == null ||
                prepared.Count == null ||
                Gs2.Gs2Formation.Model.Transaction.FormExt
                    .FormCountVerificationFailureReason(prepared.VerifyType) == null ||
                Gs2.Gs2Formation.Model.Transaction.FormExt
                    .FormCountMatched(prepared.CountType) == null) {
                return null;
            }
            // サーバーと同じく、スロットモデルの propertyRegex と同じ置き換えをしてから照合する。
            prepared.PropertyIdRegex = prepared.PropertyIdRegex
                .Replace("{region}", region)
                .Replace("{ownerId}", ownerId)
                .Replace("{userId}", token.UserId);
            if (Gs2.Gs2Formation.Model.Transaction.FormExt
                    .FormPropertyIdRegex(prepared.PropertyIdRegex) == null ||
                !IsKnownSlotName(domain, prepared, token)) {
                return null;
            }
            var expectedFormId =
                $"grn:gs2:{region}:{ownerId}:formation:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:mold:{prepared.MoldModelName}:form:{prepared.Index}";
            var item = LocalForm(domain, prepared, token, expectedFormId);
            if (item == null) {
                return null;
            }
            item.SpeculativeExecution(prepared);
            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedFormId
            ).Invoke;
/* diff +++ end */
        }
    }
}
