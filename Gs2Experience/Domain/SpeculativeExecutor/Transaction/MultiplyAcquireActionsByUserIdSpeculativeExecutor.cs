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
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Numerics;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Experience.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Experience.Domain.Transaction.SpeculativeExecutor
{
    public static class MultiplyAcquireActionsByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Experience:MultiplyAcquireActionsByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                
                var future = domain.Experience.Namespace(
                    request.NamespaceName
                ).ExperienceModel(
                    request.ExperienceName
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var model = future.Result;

                var future2 = domain.Experience.Namespace(
                    request.NamespaceName
                ).AccessToken(
                    accessToken
                ).Status(
                    request.ExperienceName,
                    request.PropertyId
                ).ModelFuture();
                yield return future2;
                if (future2.Error != null) {
                    result.OnError(future2.Error);
                    yield break;
                }
                var item = future2.Result;

                Gs2Future<Func<object>> future3;
                var rate = model.AcquireActionRates.FirstOrDefault(v => v.Name == request.RateName);
                
                if (rate == null) {
                    throw new NotFoundException(new RequestError[] {
                        new RequestError(
                            "rateName",
                            "experience.experienceModel.acquireActionRates.error.notFound"
                        )
                    });
                }
                else if (rate.Mode == "double") {
                    future3 = new Core.SpeculativeExecutor.SpeculativeExecutor(
                        Array.Empty<ConsumeAction>(),
                        request.AcquireActions,
                        rate.Rates[item.RankValue ?? 1]
                    ).ExecuteFuture(
                        domain,
                        accessToken
                    );
                }
                else {
                    future3 = new Core.SpeculativeExecutor.SpeculativeExecutor(
                        Array.Empty<ConsumeAction>(),
                        request.AcquireActions,
                        BigInteger.Parse(rate.BigRates[item.RankValue ?? 1] ?? "1")
                    ).ExecuteFuture(
                        domain,
                        accessToken
                    );
                }
                
                
                yield return future3;
                if (future3.Error != null) {
                    result.OnError(future3.Error);
                    yield break;
                }
                var commit = future3.Result;

                result.OnComplete(() =>
                {
                    commit?.Invoke();
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
            MultiplyAcquireActionsByUserIdRequest request
        ) {
            var model = await domain.Experience.Namespace(
                request.NamespaceName
            ).ExperienceModel(
                request.ExperienceName
            ).ModelAsync();

            var item = await domain.Experience.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Status(
                request.ExperienceName,
                request.PropertyId
            ).ModelAsync();

            Func<object> commit;
            var rate = model.AcquireActionRates.FirstOrDefault(v => v.Name == request.RateName);
            if (rate == null) {
                throw new NotFoundException(new RequestError[] {
                    new RequestError(
                        "rateName",
                        "experience.experienceModel.acquireActionRates.error.notFound"
                    )
                });
            }
            else if (rate.Mode == "double") {
                commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                    Array.Empty<ConsumeAction>(),
                    request.AcquireActions,
                    rate.Rates[item.RankValue ?? 1]
                ).ExecuteAsync(
                    domain,
                    accessToken
                );
            }
            else {
                commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                    Array.Empty<ConsumeAction>(),
                    request.AcquireActions,
                    BigInteger.Parse(rate.BigRates[item.RankValue ?? 1] ?? "1")
                ).ExecuteAsync(
                    domain,
                    accessToken
                );
            }

            return () =>
            {
                commit?.Invoke();
                return null;
            };
        }
#endif
    }
}
