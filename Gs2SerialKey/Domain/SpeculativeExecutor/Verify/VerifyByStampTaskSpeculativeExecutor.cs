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
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2SerialKey.Model; /* diff +++ */
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
    public static class VerifyCodeByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedCampaignVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly VerifyCodeByUserIdRequest _request;
            private readonly string _expectedCampaignId;

            public PreparedCampaignVerification(
                Gs2.Core.Domain.Gs2 domain,
                VerifyCodeByUserIdRequest request,
                string expectedCampaignId
            ) {
                _domain = domain;
                _request = request;
                _expectedCampaignId = expectedCampaignId;
            }

            public bool IsStillSatisfied()
            {
                var (campaign, found) = ((CampaignModel)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _request.Code,
                    null
                );
                return found && campaign != null &&
                    campaign.CampaignId == _expectedCampaignId &&
                    campaign.Name == _request.Code &&
                    (_request.CampaignModelName == null ||
                     campaign.Name == _request.CampaignModelName);
            }

            public object Invoke()
            {
                return null;
            }
        }

        private sealed class PreparedSerialKeyVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyCodeByUserIdRequest _request;
            private readonly string _expectedSerialKeyId;
            private readonly string _campaignModelName;
            private readonly string _expectedCampaignId;

            public PreparedSerialKeyVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyCodeByUserIdRequest request,
                string expectedSerialKeyId,
                string campaignModelName,
                string expectedCampaignId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedSerialKeyId = expectedSerialKeyId;
                _campaignModelName = campaignModelName;
                _expectedCampaignId = expectedCampaignId;
            }

            public bool IsStillSatisfied()
            {
                var (campaignByCode, aliasFound) =
                    ((CampaignModel)null).GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _request.Code,
                        null
                    );
                if (!aliasFound || campaignByCode != null) {
                    return false;
                }

                var (item, itemFound) = ((SerialKey)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _request.UserId,
                    _request.Code,
                    _accessToken.TimeOffset
                );
                if (!itemFound || item == null ||
                    item.SerialKeyId != _expectedSerialKeyId ||
                    item.Code != _request.Code ||
                    item.CampaignModelName != _campaignModelName ||
                    string.IsNullOrEmpty(item.Status)) {
                    return false;
                }

                var (campaign, campaignFound) =
                    ((CampaignModel)null).GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _campaignModelName,
                        null
                    );
                if (!campaignFound || campaign == null ||
                    campaign.CampaignId != _expectedCampaignId ||
                    campaign.Name != _campaignModelName) {
                    return false;
                }

                try {
                    Transform(_domain, _accessToken, _request, item);
                    return true;
                }
                catch (Gs2Exception) {
                    return false;
                }
            }

            public object Invoke()
            {
                return null;
            }
        }

/* diff +++ end */
        public static string Action() {
            return "Gs2SerialKey:VerifyCodeByUserId";
/* diff +++ start */
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCodeByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyCodeByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "active":
                    inverse.VerifyType = "inactive";
                    break;
                case "inactive":
                    inverse.VerifyType = "active";
                    break;
                default:
                    return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse, false);
        }

        private static void ValidateRequest(
            VerifyCodeByUserIdRequest request
        ) {
            switch (request?.VerifyType) {
                case "active":
                case "inactive":
                    break;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
            if (string.IsNullOrEmpty(request.UserId)) {
                throw new BadRequestException(new [] {
                    new RequestError("userId", "invalid"),
                });
            }
            if (string.IsNullOrEmpty(request.Code)) {
                throw new BadRequestException(new [] {
                    new RequestError("code", "invalid"),
                });
            }
        }

        public static Gs2.Gs2SerialKey.Model.SerialKey Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCodeByUserIdRequest request,
            Gs2.Gs2SerialKey.Model.SerialKey item
        ) {
            ValidateRequest(request);
            if (item == null || item.Code != request.Code) {
                throw new BadRequestException(new [] {
                    new RequestError("code", "invalid"),
                });
            }
            if (request.CampaignModelName != null &&
                item.CampaignModelName != request.CampaignModelName) {
                throw new BadRequestException(new [] {
                    new RequestError("campaignModelName", "invalid"),
                });
            }
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError(
                        "status",
                        request.VerifyType == "inactive" ? "active" : "inactive"
                    ),
                });
            }
            return item;
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCodeByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCodeByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            return await ExecuteAsync(domain, accessToken, request, true);
        }

#if GS2_ENABLE_UNITASK
        private static async UniTask<Func<object>> ExecuteAsync(
#else
        private static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyCodeByUserIdRequest request,
            bool allowCampaignAlias
        ) {
            var prepared = request == null ? null :
                VerifyCodeByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                string.IsNullOrEmpty(prepared?.NamespaceName) ||
                string.IsNullOrEmpty(prepared.UserId) ||
                string.IsNullOrEmpty(prepared.Code) ||
                prepared.CampaignModelName?.Length == 0 ||
                (prepared.VerifyType != "active" &&
                 prepared.VerifyType != "inactive")) {
                return null;
            }
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId ?? "";
            var campaignByCode = ((CampaignModel)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.Code,
                null
            );
            if (!campaignByCode.Item2) {
                return null;
            }
            var campaign = campaignByCode.Item1;
            var expectedCampaignByCodeId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "serialKey",
                prepared.NamespaceName,
                "model",
                "campaign",
                prepared.Code
            );
            if (campaign != null) {
                if (!allowCampaignAlias) {
                    return null;
                }
                if (campaign.CampaignId != expectedCampaignByCodeId ||
                    campaign.Name != prepared.Code) {
                    return null;
                }
                if (prepared.CampaignModelName != null &&
                    campaign.Name != prepared.CampaignModelName) {
                    throw new BadRequestException(new [] {
                        new RequestError("campaignModelName", "invalid"),
                    });
                }
                return new PreparedCampaignVerification(
                    domain,
                    prepared,
                    expectedCampaignByCodeId
                ).Invoke;
            }

            var (item, itemFound) = ((SerialKey)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.UserId,
                prepared.Code,
                preparedAccessToken.TimeOffset
            );
            var expectedSerialKeyId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "serialKey",
                prepared.NamespaceName,
                "serialKey",
                prepared.Code
            );
            if (!itemFound || item == null ||
                item.SerialKeyId != expectedSerialKeyId ||
                item.Code != prepared.Code ||
                string.IsNullOrEmpty(item.CampaignModelName) ||
                string.IsNullOrEmpty(item.Status)) {
                return null;
            }

            var (itemCampaign, itemCampaignFound) =
                ((CampaignModel)null).GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    item.CampaignModelName,
                    null
                );
            var expectedItemCampaignId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "serialKey",
                prepared.NamespaceName,
                "model",
                "campaign",
                item.CampaignModelName
            );
            if (!itemCampaignFound || itemCampaign == null ||
                itemCampaign.CampaignId != expectedItemCampaignId ||
                itemCampaign.Name != item.CampaignModelName) {
                return null;
            }

            Transform(domain, preparedAccessToken, prepared, item);

            return new PreparedSerialKeyVerification(
                domain,
                preparedAccessToken,
                prepared,
                expectedSerialKeyId,
                item.CampaignModelName,
                expectedItemCampaignId
            ).Invoke;
/* diff +++ end */
        }
    }
}
