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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2SkillTree.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#endif

namespace Gs2.Gs2SkillTree.Domain.SpeculativeExecutor
{
    public static class MarkReleaseByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2SkillTree:MarkReleaseByUserId";
        }

        public static Gs2.Gs2SkillTree.Model.Status Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReleaseByUserIdRequest request,
            Gs2.Gs2SkillTree.Model.Status item
        ) {
            if (item.ReleasedNodeNames.Count(v => request.NodeModelNames.Contains(v)) > 0) {
                throw new BadRequestException(new [] {
                    new RequestError("releasedNodeNames", "released"),
                });
            }

            item.ReleasedNodeNames = item.ReleasedNodeNames.Concat(request.NodeModelNames).ToArray();

            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            MarkReleaseByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {

                var future = domain.SkillTree.Namespace(
                    request.NamespaceName
                ).User(
                    request.UserId
                ).Status(
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var item = future.Result;

                item = Transform(domain, accessToken, request, item);

                var commit = new List<Func<object>>();
                foreach (var nodeModelName in request.NodeModelNames) {
                    var future2 = domain.SkillTree.Namespace(
                        request.NamespaceName
                    ).NodeModel(
                        nodeModelName
                    ).ModelFuture();
                    yield return future2;
                    if (future2.Error != null) {
                        result.OnError(future2.Error);
                        yield break;
                    }
                    var model = future2.Result;

                    var future3 = new Gs2.Core.SpeculativeExecutor.SpeculativeExecutor(
                        model.ReleaseConsumeActions,
                        Array.Empty<AcquireAction>()
                    ).ExecuteFuture(
                        domain,
                        accessToken
                    );
                    if (future3.Error != null) {
                        result.OnError(future3.Error);
                        yield break;
                    }
                    commit.Add(() =>
                    {
                        future3.Result.Invoke();
                        return null;
                    });
                }

                var parentKey = Gs2.Gs2SkillTree.Domain.Model.UserDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    "Status"
                );
                var key = Gs2.Gs2SkillTree.Domain.Model.StatusDomain.CreateCacheKey(
                );

                result.OnComplete(() =>
                {
                    foreach (var c in commit) {
                        c?.Invoke();
                    }
                    domain.Cache.Put<Gs2.Gs2SkillTree.Model.Status>(
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
            MarkReleaseByUserIdRequest request
        ) {
            var item = await domain.SkillTree.Namespace(
                request.NamespaceName
            ).User(
                request.UserId
            ).Status(
            ).ModelAsync();

            item = Transform(domain, accessToken, request, item);

            var parentKey = Gs2.Gs2SkillTree.Domain.Model.UserDomain.CreateCacheParentKey(
                request.NamespaceName,
                accessToken.UserId,
                "Status"
            );
            var key = Gs2.Gs2SkillTree.Domain.Model.StatusDomain.CreateCacheKey(
            );

            return () =>
            {
                domain.Cache.Put<Gs2.Gs2SkillTree.Model.Status>(
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
