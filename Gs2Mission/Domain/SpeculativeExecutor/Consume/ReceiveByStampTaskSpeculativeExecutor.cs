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
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Request;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#endif

namespace Gs2.Gs2Mission.Domain.SpeculativeExecutor
{
    public static class ReceiveByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:ReceiveByUserId";
        }

        public static Gs2.Gs2Mission.Model.Complete Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReceiveByUserIdRequest request,
            Gs2.Gs2Mission.Model.Complete item
        ) {
            if (item.ReceivedMissionTaskNames.Count(v => v == request.MissionTaskName) != 0) {
                throw new BadRequestException(new [] {
                    new RequestError("receivedMissionTaskNames", "alreadyReceived"),
                });
            }

            item.ReceivedMissionTaskNames = item.ReceivedMissionTaskNames.Concat(
                new[] {
                    request.MissionTaskName,
                }
            ).ToArray();
            
            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReceiveByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {

                var future = domain.Mission.Namespace(
                    request.NamespaceName
                ).MissionGroupModel(
                    request.MissionGroupName
                ).MissionTaskModel(
                    request.MissionTaskName
                ).ModelFuture();
                yield return future;
                if (future.Error != null) {
                    result.OnError(future.Error);
                    yield break;
                }
                var model = future.Result;

                var future2 = domain.Mission.Namespace(
                    request.NamespaceName
                ).User(
                    request.UserId
                ).Complete(
                    request.MissionGroupName
                ).ModelFuture();
                yield return future2;
                if (future2.Error != null) {
                    result.OnError(future2.Error);
                    yield break;
                }
                var item = future2.Result;

                item = Transform(domain, accessToken, request, item);

                var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                    request.NamespaceName,
                    accessToken.UserId,
                    "Complete"
                );
                var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                    request.MissionGroupName.ToString()
                );

                var future3 = new Gs2.Core.SpeculativeExecutor.SpeculativeExecutor(
                    Array.Empty<ConsumeAction>(),
                    model.CompleteAcquireActions
                ).ExecuteFuture(
                    domain,
                    accessToken
                );
                if (future3.Error != null) {
                    result.OnError(future3.Error);
                    yield break;
                }
                var commit = future3.Result;
                
                result.OnComplete(() =>
                {
                    commit?.Invoke(); 
                    domain.Cache.Put<Gs2.Gs2Mission.Model.Complete>(
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
            ReceiveByUserIdRequest request
        ) {
            var item = await domain.Mission.Namespace(
                request.NamespaceName
            ).User(
                request.UserId
            ).Complete(
                request.MissionGroupName
            ).ModelAsync();

            item = Transform(domain, accessToken, request, item);

            var parentKey = Gs2.Gs2Mission.Domain.Model.UserDomain.CreateCacheParentKey(
                request.NamespaceName,
                accessToken.UserId,
                "Complete"
            );
            var key = Gs2.Gs2Mission.Domain.Model.CompleteDomain.CreateCacheKey(
                request.MissionGroupName.ToString()
            );

            return () =>
            {
                domain.Cache.Put<Gs2.Gs2Mission.Model.Complete>(
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
