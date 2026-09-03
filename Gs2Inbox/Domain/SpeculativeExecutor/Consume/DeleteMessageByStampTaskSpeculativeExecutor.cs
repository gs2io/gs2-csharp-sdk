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
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inbox.Request;
using Gs2.Gs2Inbox.Model.Cache;
using Gs2.Gs2Inbox.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Inbox.Domain.SpeculativeExecutor
{
    public static class DeleteMessageByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inbox:DeleteMessageByUserId";
        }

/* diff +++ start */
        public static bool ValidateMessage(
            DeleteMessageByUserIdRequest request,
            Gs2.Gs2Inbox.Model.Message item,
            long namespaceCreatedAt,
            long physicalTimeMillis,
            string region,
            string ownerId
        ) {
            return item != null &&
                   item.MessageId ==
                       $"grn:gs2:{region}:{ownerId}:inbox:{request.NamespaceName}:" +
                       $"user:{request.UserId}:message:{request.MessageName}" &&
                   item.Name == request.MessageName &&
                   item.UserId == request.UserId &&
                   item.ReceivedAt >= namespaceCreatedAt &&
                   (item.ExpiresAt == null ||
                    item.ExpiresAt >= physicalTimeMillis);
        }

        public static void Commit(
            CacheDatabase cache,
            DeleteMessageByUserIdRequest request,
            string userId,
            int? timeOffset,
            Gs2.Gs2Inbox.Model.Namespace expectedNamespace,
            Gs2.Gs2Inbox.Model.Message expected
        ) {
            var namespaceCached = ((Gs2.Gs2Inbox.Model.Namespace)null).GetCache(
                cache,
                request.NamespaceName,
                null
            );
            var messageCached = ((Gs2.Gs2Inbox.Model.Message)null).GetCache(
                cache,
                request.NamespaceName,
                userId,
                request.MessageName,
                timeOffset
            );
            if (!namespaceCached.Item2 || namespaceCached.Item1 == null ||
                namespaceCached.Item1.ToJson().ToJson() !=
                    expectedNamespace.ToJson().ToJson() ||
                !messageCached.Item2 || messageCached.Item1 == null ||
                messageCached.Item1.ToJson().ToJson() !=
                    expected.ToJson().ToJson()) {
                return;
            }
            (null as Gs2.Gs2Inbox.Model.Message).PutCache(
                cache,
                request.NamespaceName,
                userId,
                request.MessageName,
                timeOffset
            );
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteMessageByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DeleteMessageByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Inbox.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Message(
                request.MessageName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                DeleteMessageByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                prepared?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.MessageName)) {
                return null;
            }
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var namespaceCached = ((Gs2.Gs2Inbox.Model.Namespace)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                null
            );
            var namespaceModel = namespaceCached.Item1;
            if (!namespaceCached.Item2 ||
                !OpenMessageByUserIdSpeculativeExecutor.ValidateNamespace(
                    new OpenMessageByUserIdRequest()
                        .WithNamespaceName(prepared.NamespaceName),
                    namespaceModel,
                    region,
                    ownerId
                )) {
                return null;
            }
            var timeOffset = preparedAccessToken.TimeOffset;
            var userId = preparedAccessToken.UserId;
            var messageCached = ((Gs2.Gs2Inbox.Model.Message)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.MessageName,
                timeOffset
            );
            var item = messageCached.Item1;
            if (!messageCached.Item2 ||
                !ValidateMessage(
                    prepared,
                    item,
                    namespaceModel.CreatedAt.Value,
                    UnixTime.ToUnixTime(DateTime.Now),
                    region,
                    ownerId
                )) {
                return null;
            }
            var expectedNamespace = namespaceModel.Clone() as
                Gs2.Gs2Inbox.Model.Namespace;
            var expected = item.Clone() as Gs2.Gs2Inbox.Model.Message;
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                Commit( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    accessToken.UserId,
                    request.MessageName,
                    accessToken.TimeOffset
 diff --- end */
/* diff +++ start */
                    prepared,
                    userId,
                    timeOffset,
                    expectedNamespace,
                    expected
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
