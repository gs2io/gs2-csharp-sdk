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
using System.Linq; /* diff +++ */
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Gs2Auth.Model;
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
    public static class VerifyEntryCountByUserIdSpeculativeExecutor {

/* diff +++ start */
        private sealed class PreparedVerification :
            IPreparedSpeculativeVerification
        {
            private readonly Gs2.Core.Domain.Gs2 _domain;
            private readonly AccessToken _accessToken;
            private readonly VerifyEntryCountByUserIdRequest _request;

            public PreparedVerification(
                Gs2.Core.Domain.Gs2 domain,
                AccessToken accessToken,
                VerifyEntryCountByUserIdRequest request
            ) {
                _domain = domain;
                _accessToken = accessToken;
                _request = request;
            }

            public bool IsStillSatisfied()
            {
                var items = LocalEntries(_domain, _request, _accessToken);
                return items != null && items.IsExecutable(_request);
            }

            public object Invoke()
            {
                return null;
            }
        }
/* diff +++ end */
        public static string Action() {
            return "Gs2Dictionary:VerifyEntryCountByUserId";
/* diff +++ start */
        }

        // 手元で数えられるときだけ、入手済みのエントリーの一覧を返す。数えられなければ null。
        // - エントリーの一覧は、最後まで読んだときだけ付く「全件そろった」印があるときに限る
        // - 名前を指定したときは、マスターに無い名前をサーバーが NotFound にするので、
        //   マスターの一覧もそろっていて、すべての名前がそこにあるときに限る
        private static Gs2.Gs2Dictionary.Model.Entry[] LocalEntries(
            Gs2.Core.Domain.Gs2 domain,
            VerifyEntryCountByUserIdRequest request,
            AccessToken accessToken
        ) {
            if (request.EntryModelNames != null && request.EntryModelNames.Length > 0) {
                if (!domain.Cache.TryGetList<Gs2.Gs2Dictionary.Model.EntryModel>(
                        (null as Gs2.Gs2Dictionary.Model.EntryModel).CacheParentKey(
                            request.NamespaceName,
                            null
                        ),
                        out var models
                    ) || models == null) {
                    return null;
                }
                var known = new System.Collections.Generic.HashSet<string>(
                    models.Where(v => v?.Name != null).Select(v => v.Name)
                );
                if (!request.EntryModelNames.All(v => v != null && known.Contains(v))) {
                    return null;
                }
            }
            if (!domain.Cache.TryGetList<Gs2.Gs2Dictionary.Model.Entry>(
                    (null as Gs2.Gs2Dictionary.Model.Entry).CacheParentKey(
                        request.NamespaceName,
                        accessToken.UserId,
                        accessToken.TimeOffset
                    ),
                    out var entries
                ) || entries == null || entries.Any(v => v?.Name == null)) {
                return null;
            }
            return entries;
        }

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteInverseAsync(
#else
        public static async Task<Func<object>> ExecuteInverseAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryCountByUserIdRequest request
        ) {
            var inverse = request == null ? null :
                VerifyEntryCountByUserIdRequest.FromJson(request.ToJson());
            switch (inverse?.VerifyType) {
                case "less": inverse.VerifyType = "greaterEqual"; break;
                case "lessEqual": inverse.VerifyType = "greater"; break;
                case "greater": inverse.VerifyType = "lessEqual"; break;
                case "greaterEqual": inverse.VerifyType = "less"; break;
                case "equal": inverse.VerifyType = "notEqual"; break;
                case "notEqual": inverse.VerifyType = "equal"; break;
                default: return null;
            }
            return await ExecuteAsync(domain, accessToken, inverse);
/* diff +++ end */
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryCountByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            VerifyEntryCountByUserIdRequest request
        ) {
/* diff --- start
            return () => null;
 diff --- end */
/* diff +++ start */
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null ||
                string.IsNullOrEmpty(token?.UserId)) return null;
            var prepared = VerifyEntryCountByUserIdRequest.FromJson(
                request.ToJson()
            );
            if (prepared.UserId == "#{userId}") {
                prepared.UserId = token.UserId;
            }
            if (prepared.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                prepared.Count == null ||
                Gs2.Gs2Dictionary.Model.Transaction.EntryExt
                    .EntryCountVerificationFailureReason(prepared.VerifyType) == null) {
                return null;
            }
            var items = LocalEntries(domain, prepared, token);
            if (items == null) {
                return null;
            }
            items.SpeculativeExecution(prepared);
            return new PreparedVerification(
                domain,
                token,
                prepared
            ).Invoke;
/* diff +++ end */
        }
    }
}
