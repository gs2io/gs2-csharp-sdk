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
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Experience.Model;
using Gs2.Gs2Experience.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Experience.Domain.SpeculativeExecutor
{
    public static class AddExperienceByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Experience:AddExperienceByUserId";
        }

        public static Gs2.Gs2Experience.Model.Status Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddExperienceByUserIdRequest request,
            Gs2.Gs2Experience.Model.ExperienceModel model,
            Gs2.Gs2Experience.Model.Status item
        ) {
            item.ExperienceValue += item.ExperienceValue;
            item.RankValue = model.Rank(item);
            item.NextRankUpExperienceValue = model.NextRankExperienceValue(item);

            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            AddExperienceByUserIdRequest request
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
                ).User(
                    request.UserId
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

                item = Transform(domain, accessToken, request, model, item);

                var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    "Status"
                );
                var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                    request.ExperienceName.ToString(),
                    request.PropertyId.ToString()
                );

                result.OnComplete(() =>
                {
                    domain.Cache.Put<Gs2.Gs2Experience.Model.Status>(
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
            AddExperienceByUserIdRequest request
        ) {
            var model = await domain.Experience.Namespace(
                request.NamespaceName
            ).ExperienceModel(
                request.ExperienceName
            ).ModelAsync();

            var item = await domain.Experience.Namespace(
                request.NamespaceName
            ).User(
                request.UserId
            ).Status(
                request.ExperienceName,
                request.PropertyId
            ).ModelAsync();

            item = Transform(domain, accessToken, request, model, item);

            var parentKey = Gs2.Gs2Experience.Domain.Model.UserDomain.CreateCacheParentKey(
                request.NamespaceName,
                accessToken.UserId,
                "Status"
            );
            var key = Gs2.Gs2Experience.Domain.Model.StatusDomain.CreateCacheKey(
                request.ExperienceName.ToString(),
                request.PropertyId.ToString()
            );

            return () =>
            {
                domain.Cache.Put<Gs2.Gs2Experience.Model.Status>(
                    parentKey,
                    key,
                    item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                );
                return null;
            };
        }
#endif
    }
}
