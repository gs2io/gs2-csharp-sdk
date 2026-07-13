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
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inbox.Request;
using Gs2.Core.Model; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inbox.Domain.Transaction.SpeculativeExecutor
{
    public static class ReadMessageByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inbox:ReadMessageByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReadMessageByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            ReadMessageByUserIdRequest request
        ) {
/* diff --- start
            // TODO: Speculative execution not supported
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: " + Action());
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: " + Action());
//#endif

 diff --- end */
            var item = await domain.Inbox.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Message(
                request.MessageName
            ).ModelAsync();

            var commit = await new Core.SpeculativeExecutor.SpeculativeExecutor(
/* diff --- start
                item?.ConsumeActions.Select(v =>
                {
                    foreach (var config in request.Config ?? Array.Empty<Gs2.Gs2Inbox.Model.Config>()) {
                        v = v.ApplyConfig(config.Key, config.Value);
 diff --- end */
/* diff +++ start */
                new ConsumeAction[] {
                    new ConsumeAction {
                        Action = "Gs2Inbox:OpenMessageByUserId",
                        Request = new OpenMessageByUserIdRequest {
                            NamespaceName = request.NamespaceName,
                            UserId = request.UserId,
                            MessageName = request.MessageName,
                        }.ToJson().ToJson()
/* diff +++ end */
                    }
/* diff --- start
                    return v;
                }).ToArray() ?? new Gs2.Core.Model.ConsumeAction[]{},
                item?.AcquireActions.Select(v =>
                {
                    foreach (var config in request.Config ?? Array.Empty<Gs2.Gs2Inbox.Model.Config>()) {
                        v = v.ApplyConfig(config.Key, config.Value);
                    }
                    return v;
                }).ToArray() ?? new Gs2.Core.Model.AcquireAction[]{},
 diff --- end */
/* diff +++ start */
                },
                item.ReadAcquireActions,
/* diff +++ end */
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
    }
}
