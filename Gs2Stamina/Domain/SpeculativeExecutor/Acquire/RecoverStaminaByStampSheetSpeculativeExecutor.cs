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
 *
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
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Stamina.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Stamina.Domain.SpeculativeExecutor
{
    public static class RecoverStaminaByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Stamina:RecoverStaminaByUserId";
        }
        public static Gs2.Gs2Stamina.Model.Stamina Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RecoverStaminaByUserIdRequest request,
            Gs2.Gs2Stamina.Model.StaminaModel model,
            Gs2.Gs2Stamina.Model.Stamina item
        ) {
            item.Value += request.RecoverValue;
            if (item.Value > item.MaxValue) {
                item.OverflowValue = item.Value - item.MaxValue;

                if (item.Value > model.MaxCapacity) {
                    item.OverflowValue = model.MaxCapacity - item.Value;
                    item.Value = model.MaxCapacity;
                }
            }
            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RecoverStaminaByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                var future = domain.Stamina.Namespace(
                    request.NamespaceName
                ).StaminaModel(
                    request.StaminaName
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var model = future.Result;

                if (model == null) {
                    result.OnComplete(() => null);
                    yield break;
                }

                var future2 = domain.Stamina.Namespace(
                    request.NamespaceName
                ).AccessToken(
                    accessToken
                ).Stamina(
                    request.StaminaName
                ).ModelFuture();
                yield return future2;
                if (future2.Error != null) {
                    result.OnError(future2.Error);
                    yield break;
                }
                var item = future2.Result;

                if (item == null) {
                    result.OnComplete(() => null);
                    yield break;
                }

                try {
                    item = Transform(domain, accessToken, request, model, item);
                }
                catch (Gs2Exception e) {
                    result.OnError(e);
                    yield break;
                }
                
                var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    "Stamina"
                );
                var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                    request.StaminaName.ToString()
                );

                result.OnComplete(() =>
                {
                    domain.Cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                        parentKey,
                        key,
                        item,
                        UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                    );
                    return null;
                });
                yield return null;
            }

            return new Gs2InlineFuture<Func<object>>(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public static async UniTask<Func<object>> ExecuteAsync(
    #else
        public static async Task<Func<object>> ExecuteAsync(
    #endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RecoverStaminaByUserIdRequest request
        ) {
            var model = await domain.Stamina.Namespace(
                request.NamespaceName
            ).StaminaModel(
                request.StaminaName
            ).ModelAsync();

            if (model == null) {
                return () => null;
            }

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

            item = Transform(domain, accessToken, request, model, item);

            var parentKey = Gs2.Gs2Stamina.Domain.Model.UserDomain.CreateCacheParentKey(
                request.NamespaceName,
                accessToken.UserId,
                "Stamina"
            );
            var key = Gs2.Gs2Stamina.Domain.Model.StaminaDomain.CreateCacheKey(
                request.StaminaName.ToString()
            );

            return () =>
            {
                domain.Cache.Put<Gs2.Gs2Stamina.Model.Stamina>(
                    parentKey,
                    key,
                    item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                );
                return null;
            };
        }
#endif

        public static RecoverStaminaByUserIdRequest Rate(
            RecoverStaminaByUserIdRequest request,
            double rate
        ) {
            request.RecoverValue = (int?) (request.RecoverValue * rate);
            return request;
        }

        public static RecoverStaminaByUserIdRequest Rate(
            RecoverStaminaByUserIdRequest request,
            BigInteger rate
        ) {
            request.RecoverValue = (int?) ((request.RecoverValue ?? 0) * rate);
            return request;
        }
    }
}
