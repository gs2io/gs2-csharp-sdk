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
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Inbox.Model; /* diff +++ */
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
    public static class OpenMessageByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Inbox:OpenMessageByUserId";
        }

/* diff +++ start */
        public static bool ValidateNamespace(
            OpenMessageByUserIdRequest request,
            Gs2.Gs2Inbox.Model.Namespace namespaceModel,
            string region,
            string ownerId
        ) {
            return namespaceModel != null &&
                   namespaceModel.Name == request.NamespaceName &&
                   namespaceModel.IsAutomaticDeletingEnabled != null &&
                   namespaceModel.CreatedAt != null &&
                   namespaceModel.NamespaceId ==
                       $"grn:gs2:{region}:{ownerId}:inbox:{request.NamespaceName}";
        }

        public static Message Transform(
            OpenMessageByUserIdRequest request,
            Message item,
            bool automaticDeletingEnabled,
            long namespaceCreatedAt,
            long currentTimeMillis,
            long physicalTimeMillis,
            string region = null,
            string ownerId = null
        ) {
            return item.SpeculativeExecutionAt(
                request,
                automaticDeletingEnabled,
                namespaceCreatedAt,
                currentTimeMillis,
                physicalTimeMillis,
                region,
                ownerId
            );
        }

        public static void Commit(
            CacheDatabase cache,
            OpenMessageByUserIdRequest request,
            string userId,
            int? timeOffset,
            Gs2.Gs2Inbox.Model.Namespace expectedNamespace,
            Message expected,
            Message item
        ) {
            var namespaceCached = ((Gs2.Gs2Inbox.Model.Namespace)null).GetCache(
                cache,
                request.NamespaceName,
                null
            );
            var messageCached = ((Message)null).GetCache(
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
            if (item == null) {
                (null as Message).PutCache(
                    cache,
                    request.NamespaceName,
                    userId,
                    request.MessageName,
                    timeOffset
                );
            }
            else {
                var cached = item.Clone() as Message;
                cached.Revision = 0;
                cache.Put(
                    cached.CacheParentKey(
                        request.NamespaceName,
                        userId,
                        timeOffset
                    ),
                    cached.CacheKey(request.MessageName),
                    cached,
                    ((cached.ExpiresAt ?? 0) == 0 ? null : cached.ExpiresAt) ??
                        UnixTime.ToUnixTime(DateTime.Now) +
                        1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OpenMessageByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            OpenMessageByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Inbox.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Message(
                request.MessageName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                OpenMessageByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
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
                !ValidateNamespace(prepared, namespaceModel, region, ownerId)) {
                return null;
            }
            var messageCached = ((Message)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                preparedAccessToken.UserId,
                prepared.MessageName,
                preparedAccessToken.TimeOffset
            );
            var item = messageCached.Item1;
            var expectedMessageId =
                $"grn:gs2:{region}:{ownerId}:inbox:{prepared.NamespaceName}:" +
                $"user:{prepared.UserId}:message:{prepared.MessageName}";
            var physicalTimeMillis = UnixTime.ToUnixTime(DateTime.Now);
            if (!messageCached.Item2 || item == null ||
                item.MessageId != expectedMessageId ||
                item.Name != prepared.MessageName ||
                item.UserId != prepared.UserId ||
                item.IsRead != false ||
                item.ReceivedAt == null ||
                item.Revision == null ||
                item.ReceivedAt < namespaceModel.CreatedAt ||
                item.ExpiresAt != null && item.ExpiresAt < physicalTimeMillis) {
                return null;
            }
            var expectedNamespace = namespaceModel.Clone() as
                Gs2.Gs2Inbox.Model.Namespace;
            var expected = item.Clone() as Message;
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            var logicalTimeMillis = physicalTimeMillis +
                                    (long)(preparedAccessToken.TimeOffset ?? 0) * 1000L;
            item = Transform(
                prepared,
                item,
                namespaceModel.IsAutomaticDeletingEnabled.Value,
                namespaceModel.CreatedAt.Value,
                logicalTimeMillis,
                physicalTimeMillis,
                region,
                ownerId
            );
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
                    expected,
                    item
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
