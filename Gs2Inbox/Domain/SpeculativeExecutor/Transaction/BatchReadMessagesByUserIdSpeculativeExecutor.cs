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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inbox.Request;
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
    public static class BatchReadMessagesByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inbox:BatchReadMessagesByUserId";
        }

        private static ReadMessageByUserIdRequest BuildReadRequest(
            BatchReadMessagesByUserIdRequest request,
            string messageName,
            Gs2.Gs2Inbox.Model.Config[] configs
        ) {
            return new ReadMessageByUserIdRequest()
                .WithNamespaceName(request.NamespaceName)
                .WithUserId(request.UserId)
                .WithMessageName(messageName)
                .WithConfig(configs)
                .WithTimeOffsetToken(request.TimeOffsetToken);
        }

        public static ReadMessageByUserIdRequest[] BuildReadRequests(
            BatchReadMessagesByUserIdRequest request
        ) {
            var configs = (request.Config ?? Array.Empty<Gs2.Gs2Inbox.Model.Config>())
                .Where(config => config != null)
                .Select(config => config?.Clone() as Gs2.Gs2Inbox.Model.Config)
                .ToArray();
            return (request.MessageNames ?? Array.Empty<string>())
                .Where(messageName => !string.IsNullOrEmpty(messageName))
                .Select(messageName => BuildReadRequest(request, messageName, configs))
                .ToArray();
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchReadMessagesByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            BatchReadMessagesByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            var prepared = request == null ? null :
                new BatchReadMessagesByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithMessageNames(request.MessageNames?.ToArray())
                    .WithConfig(request.Config?
                        .Where(config => config != null)
                        .Select(config => config.Clone() as
                            Gs2.Gs2Inbox.Model.Config)
                        .ToArray())
                    .WithTimeOffsetToken(request.TimeOffsetToken);
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                prepared.MessageNames == null) {
                return null;
            }
            var readRequests = BuildReadRequests(prepared);
            var commits = new List<Func<object>>();
            foreach (var readRequest in readRequests) {
                var commit = await ReadMessageByUserIdSpeculativeExecutor
                    .ExecuteAsync(domain, token, readRequest);
                if (commit != null) {
                    commits.Add(commit);
                }
            }
            if (commits.Count == 0) {
                return null;
            }
            return () => {
                foreach (var commit in commits) {
                    commit();
                }
                return null;
            };
        }
    }
}
