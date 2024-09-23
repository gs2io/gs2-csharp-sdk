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
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Model;
using ConsumeAction = Gs2.Core.Model.ConsumeAction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.Transaction.SpeculativeExecutor
{
    public static class BatchCompleteByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:BatchCompleteByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchCompleteByUserIdRequest request
        ) {
            IEnumerator Impl(Gs2Future<Func<object>> result) {
                var acquireActions = new List<Gs2.Core.Model.AcquireAction>();
                foreach (var missionTaskName in request.MissionTaskNames) {
                    var future = domain.Mission.Namespace(
                        request.NamespaceName
                    ).MissionGroupModel(
                        request.MissionGroupName
                    ).MissionTaskModel(
                        missionTaskName
                    ).ModelFuture();
                    yield return future;
                    if (future.Error != null) {
                        result.OnError(future.Error);
                        yield break;
                    }
                    acquireActions.AddRange(future.Result.CompleteAcquireActions);
                }

                var future2 = new Core.SpeculativeExecutor.SpeculativeExecutor(
                    new ConsumeAction[] {
                        new ConsumeAction {
                            Action = "Gs2Mission:BatchReceiveByUserId",
                            Request = new BatchReceiveByUserIdRequest {
                                NamespaceName = request.NamespaceName,
                                MissionGroupName = request.MissionGroupName,
                                MissionTaskNames = request.MissionTaskNames,
                                UserId = request.UserId,
                            }.ToJson().ToJson()
                        }
                    },
                    acquireActions.ToArray(),
                    1.0
                ).ExecuteFuture(
                    domain,
                    accessToken
                );
                yield return future2;
                if (future2.Error != null) {
                    result.OnError(future2.Error);
                    yield break;
                }
                var commit = future2.Result;

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
            BatchCompleteByUserIdRequest request
        ) {
            var acquireActions = new List<Gs2.Core.Model.AcquireAction>();
            foreach (var missionTaskName in request.MissionTaskNames) {
                var item = await domain.Mission.Namespace(
                    request.NamespaceName
                ).MissionGroupModel(
                    request.MissionGroupName
                ).MissionTaskModel(
                    missionTaskName
                ).ModelAsync();
                acquireActions.AddRange(item.CompleteAcquireActions);
            }

            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
                new ConsumeAction[] {
                    new ConsumeAction {
                        Action = "Gs2Mission:BatchReceiveByUserId",
                        Request = new BatchReceiveByUserIdRequest {
                            NamespaceName = request.NamespaceName,
                            MissionGroupName = request.MissionGroupName,
                            MissionTaskNames = request.MissionTaskNames,
                            UserId = request.UserId,
                        }.ToJson().ToJson()
                    }
                },
                acquireActions.ToArray(),
                1.0
            ).ExecuteAsync(
                domain,
                accessToken
            );

            return () =>
            {
                commit?.Invoke();
                return null;
            };
        }
#endif
    }
}
