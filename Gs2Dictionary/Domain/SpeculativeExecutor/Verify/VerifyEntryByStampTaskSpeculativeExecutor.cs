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
using Gs2.Gs2Dictionary.Model;
using Gs2.Gs2Dictionary.Request;
using Gs2.Gs2Dictionary.Model.Cache;
using Gs2.Gs2Dictionary.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Dictionary.Domain.SpeculativeExecutor
{
    public static class VerifyEntryByUserIdSpeculativeExecutor {
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyEntryByUserIdRequest _request;
            private readonly string _expectedEntryId;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyEntryByUserIdRequest request,
                string expectedEntryId
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
                _expectedEntryId = expectedEntryId;
            }

            public bool IsStillSatisfied()
            {
                var (item, found) = ((Entry)null).GetCache(
                    _domain.Cache,
                    _request.NamespaceName,
                    _accessToken.UserId,
                    _request.EntryModelName,
                    _accessToken.TimeOffset
                );
                return IsUsable(
                           item,
                           found,
                           _request,
                           _accessToken.UserId,
                           _expectedEntryId
                       ) && Entries(item).IsExecutable(_request);
            }

            public object Invoke()
            {
                return null;
            }
        }

        public static string Action() {
            return "Gs2Dictionary:VerifyEntryByUserId";
        }

        public static Gs2.Gs2Dictionary.Model.Entry[] Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryByUserIdRequest request,
            Gs2.Gs2Dictionary.Model.Entry[] items
        ) {
            items.SpeculativeExecution(request);
            return items;
        }

        private static Entry[] Entries(Entry item)
        {
            return item == null ? new Entry[0] : new[] { item };
        }

        private static bool IsUsable(
            Entry item,
            bool found,
            VerifyEntryByUserIdRequest request,
            string userId,
            string expectedEntryId
        ) {
            return found && (item == null ||
                item.EntryId == expectedEntryId &&
                item.UserId == userId &&
                item.Name == request.EntryModelName);
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyEntryByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "have": inverse.VerifyType = "havent"; break;
                case "havent": inverse.VerifyType = "have"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryByUserIdRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            var prepared = request == null ? null :
                VerifyEntryByUserIdRequest.FromJson(request.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(domain.RestSession.OwnerId) ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.EntryModelName) ||
                Gs2.Gs2Dictionary.Model.Transaction.EntryExt
                    .EntryVerificationFailureReason(
                    prepared.VerifyType
                ) == null) {
                return null;
            }
            var userId = token.UserId;
            var timeOffset = token.TimeOffset;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var expectedEntryId = string.Join(
                ":",
                "grn",
                "gs2",
                region,
                ownerId,
                "dictionary",
                prepared.NamespaceName,
                "user",
                userId,
                "entry",
                prepared.EntryModelName
            );
            var cached = ((Entry)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                userId,
                prepared.EntryModelName,
                timeOffset
            );
            var item = cached.Item1;
            if (!IsUsable(
                    item,
                    cached.Item2,
                    prepared,
                    userId,
                    expectedEntryId
                )) {
                return null;
            }
            Transform(domain, token, prepared, Entries(item));

            return new PreparedVerification(
                domain,
                token,
                prepared,
                expectedEntryId
            ).Invoke;
        }
    }
}
