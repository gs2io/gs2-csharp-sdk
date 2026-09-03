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
using Gs2.Gs2SerialKey.Request;
using Gs2.Gs2SerialKey.Model.Cache;
using Gs2.Gs2SerialKey.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2SerialKey.Domain.SpeculativeExecutor
{
    public static class RevertUseByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2SerialKey:RevertUseByUserId";
        }

/* diff +++ start */
        private static long CurrentTimeMillis(AccessToken accessToken)
        {
            return UnixTime.ToUnixTime(DateTime.Now) +
                   (long)(accessToken?.TimeOffset ?? 0) * 1000L;
        }

        public static Gs2.Gs2SerialKey.Model.SerialKey Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RevertUseByUserIdRequest request,
            Gs2.Gs2SerialKey.Model.SerialKey item
        ) {
            return Transform(
                domain,
                accessToken,
                request,
                item,
                CurrentTimeMillis(accessToken)
            );
        }

        public static Gs2.Gs2SerialKey.Model.SerialKey Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RevertUseByUserIdRequest request,
            Gs2.Gs2SerialKey.Model.SerialKey item,
            long currentTimeMillis
        ) {
            return item.SpeculativeRevertUseAt(request, currentTimeMillis);
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RevertUseByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            RevertUseByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var preparedRequest = RevertUseByUserIdRequest.FromJson(
                request?.ToJson()
            );
            var preparedAccessToken = AccessToken.FromJson(
                accessToken?.ToJson()
            );
            if (preparedRequest?.UserId == "#{userId}") {
                preparedRequest.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                preparedRequest?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(preparedRequest.NamespaceName) ||
                string.IsNullOrEmpty(preparedRequest.Code)) {
                return null;
            }
            var timeOffset = preparedAccessToken?.TimeOffset;
            var (campaignModel, campaignFound) =
                ((Gs2.Gs2SerialKey.Model.CampaignModel)null).GetCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    preparedRequest.Code,
                    null
                );
            if (!campaignFound) {
                return null;
            }
            if (campaignModel != null) {
                var expectedCampaignId = string.Join(
                    ":", "grn", "gs2",
                    domain.RestSession.Region.DisplayName(),
                    domain.RestSession.OwnerId ?? "",
                    "serialKey", preparedRequest.NamespaceName,
                    "model", "campaign", preparedRequest.Code
                );
                if (campaignModel.CampaignId != expectedCampaignId ||
                    campaignModel.Name != preparedRequest.Code) {
                    return null;
                }
                return () => null;
            }
            var (item, serialKeyFound) =
                ((Gs2.Gs2SerialKey.Model.SerialKey)null).GetCache(
                    domain.Cache,
                    preparedRequest.NamespaceName,
                    preparedRequest.UserId,
                    preparedRequest.Code,
                    timeOffset
                );
            var expectedSerialKeyId = string.Join(
                ":", "grn", "gs2",
                domain.RestSession.Region.DisplayName(),
                domain.RestSession.OwnerId ?? "",
                "serialKey", preparedRequest.NamespaceName,
                "serialKey", preparedRequest.Code
            );
            if (!serialKeyFound || item == null ||
                item.SerialKeyId != expectedSerialKeyId ||
                item.Code != preparedRequest.Code) {
                return null;
            }
            var currentTimeMillis = CurrentTimeMillis(preparedAccessToken);
            var commit = new SerialKeySpeculativeCommit(
                domain.Cache,
                preparedRequest.NamespaceName,
                preparedRequest.UserId,
                preparedRequest.Code,
                timeOffset,
                expectedSerialKeyId,
                current => Transform(
                    domain,
                    preparedAccessToken,
                    preparedRequest,
                    current,
                    currentTimeMillis
                )
            );
            return commit.Invoke;
/* diff +++ end */
        }
    }
}
