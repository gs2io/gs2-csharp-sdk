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
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Guild.Request;
using Gs2.Gs2Guild.Model.Cache;
using Gs2.Gs2Guild.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Guild.Domain.SpeculativeExecutor
{
    public static class VerifyIncludeMemberByUserIdSpeculativeExecutor {

        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyIncludeMemberByUserIdRequest _request;
            private readonly string _expectedId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyIncludeMemberByUserIdRequest request,
                string expectedId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedId = expectedId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Gs2.Gs2Guild.Model.Guild)null)
                    .GetCache(
                        _domain.Cache,
                        _request.NamespaceName,
                        _request.GuildModelName,
                        _request.GuildName,
                        _accessToken.TimeOffset
                    );
                if (!found || item == null || item.GuildId != _expectedId ||
                    item.GuildModelName != _request.GuildModelName ||
                    item.Name != _request.GuildName || item.Members == null) {
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

        public static string Action() {
            return "Gs2Guild:VerifyIncludeMemberByUserId";
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeMemberByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyIncludeMemberByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "include": inverse.VerifyType = "notInclude"; break;
                case "notInclude": inverse.VerifyType = "include"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

        public static Gs2.Gs2Guild.Model.Guild Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeMemberByUserIdRequest request,
            Gs2.Gs2Guild.Model.Guild item
        ) {
            switch (request?.VerifyType) {
                case "include":
                case "notInclude":
                    break;
                default:
                    throw new BadRequestException(new [] {
                        new RequestError("verifyType", "invalid"),
                    });
            }
            if (!item.IsExecutable(request)) {
                throw new BadRequestException(new [] {
                    new RequestError("userId", "invalid"),
                });
            }
            return item;
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeMemberByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyIncludeMemberByUserIdRequest request
        ) {
            if (domain?.RestSession == null || accessToken == null ||
                request == null) {
                return null;
            }
            var prepared = VerifyIncludeMemberByUserIdRequest.FromJson(
                request.ToJson()
            );
            var preparedAccessToken = AccessToken.FromJson(accessToken.ToJson());
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken.UserId;
            }
            if (string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.GuildModelName) ||
                string.IsNullOrEmpty(prepared.GuildName) ||
                string.IsNullOrEmpty(prepared.UserId) ||
                (prepared.VerifyType != "include" &&
                 prepared.VerifyType != "notInclude")) {
                return null;
            }
            var cached = ((Gs2.Gs2Guild.Model.Guild)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                prepared.GuildModelName,
                prepared.GuildName,
                preparedAccessToken.TimeOffset
            );
            var item = cached.Item1;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:guild:{prepared.NamespaceName}:" +
                $"guild:{prepared.GuildModelName}:{prepared.GuildName}";
            if (!cached.Item2 || item == null || item.GuildId != expectedId ||
                item.GuildModelName != prepared.GuildModelName ||
                item.Name != prepared.GuildName || item.Members == null) {
                return null;
            }

            Transform(domain, preparedAccessToken, prepared, item);

            return new PreparedVerification(
                domain,
                preparedAccessToken,
                prepared,
                expectedId
            ).Invoke;
        }

    }
}
