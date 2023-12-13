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
using Gs2.Gs2AdReward.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2AdReward.Domain.SpeculativeExecutor
{
    public static class ConsumePointByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2AdReward:ConsumePointByUserId";
        }

        public static Gs2.Gs2AdReward.Model.Point Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumePointByUserIdRequest request,
            Gs2.Gs2AdReward.Model.Point item
        ) {
            item.Value -= request.Point;
            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ConsumePointByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {

                var future = domain.AdReward.Namespace(
                    request.NamespaceName
                ).AccessToken(
                    accessToken
                ).Point(
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;

                if (item == null) {
                    result.OnComplete(() =>
                    {
                        return null;
                    });
                    yield break;
                }
                try {
                    item = Transform(domain, accessToken, request, item);
                }
                catch (Gs2Exception e) {
                    result.OnError(e);
                    yield break;
                }

                var parentKey = Gs2.Gs2AdReward.Domain.Model.UserDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    "Point"
                );
                var key = Gs2.Gs2AdReward.Domain.Model.PointDomain.CreateCacheKey(
                );

                result.OnComplete(() =>
                {
                    domain.Cache.Put<Gs2.Gs2AdReward.Model.Point>(
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
            ConsumePointByUserIdRequest request
        ) {
            var item = await domain.AdReward.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Point(
            ).ModelAsync();

            if (item == null) {
                return () => null;
            }
            item = Transform(domain, accessToken, request, item);

            var parentKey = Gs2.Gs2AdReward.Domain.Model.UserDomain.CreateCacheParentKey(
                request.NamespaceName,
                accessToken.UserId,
                "Point"
            );
            var key = Gs2.Gs2AdReward.Domain.Model.PointDomain.CreateCacheKey(
            );

            return () =>
            {
                domain.Cache.Put<Gs2.Gs2AdReward.Model.Point>(
                    parentKey,
                    key,
                    item,
                    UnixTime.ToUnixTime(DateTime.Now) + 1000 * 10
                );
                return null;
            };
        }
#endif

        public static ConsumePointByUserIdRequest Rate(
            ConsumePointByUserIdRequest request,
            double rate
        ) {
            return request;
        }

        public static ConsumePointByUserIdRequest Rate(
            ConsumePointByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}