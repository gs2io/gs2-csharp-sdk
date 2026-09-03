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
/* diff --- start
using Gs2.Core.SpeculativeExecutor;
 diff --- end */
using Gs2.Core.Domain;
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Friend.Model; /* diff +++ */
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Friend.Domain.SpeculativeExecutor
{
    public static class UpdateProfileByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Friend:UpdateProfileByUserId";
        }

/* diff +++ start */
        public static Profile Transform(
            UpdateProfileByUserIdRequest request,
            Profile item,
            long currentTimeMillis,
            long createdAtMillis,
            string region = null,
            string ownerId = null
        ) {
            return item.SpeculativeExecutionAt(
                request,
                currentTimeMillis,
                createdAtMillis,
                region,
                ownerId
            );
        }

        public static void Commit(
            CacheDatabase cache,
            UpdateProfileByUserIdRequest request,
            Profile item,
            string userId,
            int? timeOffset
        ) {
            item.PutCache(
                cache,
                request.NamespaceName,
                userId,
                timeOffset
            );
            new PublicProfile {
                UserId = userId,
                Value = item.PublicProfile,
            }.PutCache(
                cache,
                request.NamespaceName,
                userId,
                timeOffset
            );
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            UpdateProfileByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            UpdateProfileByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Friend.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Profile(
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var preparedRequest = request == null
                ? null
                : new UpdateProfileByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithUserId(request.UserId)
                    .WithPublicProfile(request.PublicProfile)
                    .WithFollowerProfile(request.FollowerProfile)
                    .WithFriendProfile(request.FriendProfile);
            var preparedAccessToken = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest?.UserId != preparedAccessToken.UserId) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId ?? "";
            var expectedProfileId = string.Join(
                ":", "grn", "gs2",
                region,
                ownerId,
                "friend", preparedRequest.NamespaceName,
                "user", userId
            );
            var (item, find) = ((Profile)null).GetCache(
                domain.Cache,
                preparedRequest.NamespaceName,
                userId,
                timeOffset
            );
            if (!find || item == null ||
                item.ProfileId != expectedProfileId ||
                item.UserId != userId) {
                return null;
            }
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
            }
            item = item.SpeculativeExecution(request);

 diff --- end */
/* diff +++ start */
            var physicalTimeMillis = UnixTime.ToUnixTime(DateTime.Now);
            var logicalTimeMillis = physicalTimeMillis +
                                    (long)(timeOffset ?? 0) * 1000L;
            var preparedRevision = item.Revision;
/* diff +++ end */
            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
                var cached = ((Profile)null).GetCache( /* diff +++ */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    null
 diff --- end */
/* diff +++ start */
                    preparedRequest.NamespaceName,
                    userId,
                    timeOffset
/* diff +++ end */
                );
/* diff +++ start */
                var current = cached.Item2 ? cached.Item1 : null;
                if (current == null ||
                    current.ProfileId != expectedProfileId ||
                    current.UserId != userId ||
                    current.Revision > 0 &&
                    current.Revision != preparedRevision) {
                    return null;
                }
                try {
                    var changed = Transform(
                        preparedRequest,
                        current,
                        logicalTimeMillis,
                        physicalTimeMillis,
                        region,
                        ownerId
                    );
                    if (changed.ProfileId != expectedProfileId ||
                        changed.UserId != userId || changed.Revision != 0) {
                        return null;
                    }
                    Commit(
                        domain.Cache,
                        preparedRequest,
                        changed,
                        userId,
                        timeOffset
                    );
                }
                catch (System.Exception) {
                    return null;
                }
/* diff +++ end */
                return null;
            };
        }
    }
}
