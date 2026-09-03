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
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Stamina.Request;
using Gs2.Gs2Stamina.Model.Cache;
using Gs2.Gs2Stamina.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain.SpeculativeExecutor
{
    public static class SetRecoverValueByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Stamina:SetRecoverValueByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetRecoverValueByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetRecoverValueByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Stamina.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Stamina(
                request.StaminaName
            ).ModelAsync();

            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    request.UserId,
                    request.StaminaName,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = SetRecoverValueByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") prepared.UserId = token.UserId;
            if (prepared.UserId != token.UserId ||
                !prepared.RecoverValue.HasValue) return null;
            var expectedStaminaId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:stamina:{prepared.NamespaceName}:" +
                $"user:{token.UserId}:stamina:{prepared.StaminaName}";
            var cached = ((Gs2.Gs2Stamina.Model.Stamina)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.StaminaName, token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.StaminaId != expectedStaminaId ||
                cached.Item1.UserId != token.UserId ||
                cached.Item1.StaminaName != prepared.StaminaName) return null;
            var recoverValue = prepared.RecoverValue.Value;
            return new StaminaMutationSpeculativeCommit(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.StaminaName, token.TimeOffset, expectedStaminaId,
                null, cached.Item1.Revision, false,
                (current, _) => {
                    current.RecoverValue = recoverValue;
                    return true;
                }
            ).Invoke;
/* diff +++ end */
        }
    }
}
