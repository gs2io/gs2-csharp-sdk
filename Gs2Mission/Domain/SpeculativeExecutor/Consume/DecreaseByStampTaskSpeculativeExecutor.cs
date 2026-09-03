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
using System.Collections.Generic; /* diff +++ */
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Mission.Request;
using Gs2.Gs2Mission.Model.Cache;
using Gs2.Gs2Mission.Model.Transaction;
using Gs2.Gs2Mission.Model; /* diff +++ */
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Mission.Domain.SpeculativeExecutor
{
    public static class DecreaseCounterByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Mission:DecreaseCounterByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DecreaseCounterByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            DecreaseCounterByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Mission.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Counter(
                request.CounterName
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var token = accessToken == null
                ? null
                : new AccessToken()
                    .WithUserId(accessToken.UserId)
                    .WithTimeOffset(accessToken.TimeOffset);
            var prepared = request == null
                ? null
                : new DecreaseCounterByUserIdRequest()
                    .WithNamespaceName(request.NamespaceName)
                    .WithCounterName(request.CounterName)
                    .WithUserId(request.UserId)
                    .WithValue(request.Value);
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = token?.UserId;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);

            return () =>
            {
                item.PutCache(
                    domain.Cache,
                    request.NamespaceName,
                    accessToken.UserId,
                    request.CounterName,
                    accessToken.TimeOffset
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared?.UserId != token.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.CounterName) ||
                !prepared.Value.HasValue) return null;
            var expectedId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:mission:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:counter:" +
                prepared.CounterName;
            var cached = ((Counter)null).GetCache(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.CounterName,
                token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.CounterId != expectedId ||
                cached.Item1.UserId != token.UserId ||
                cached.Item1.Name != prepared.CounterName ||
                cached.Item1.Values == null) return null;
            var logicalTimeMillis = UnixTime.ToUnixTime(DateTime.Now) +
                                    (long)(token.TimeOffset ?? 0) * 1000L;
            var commit = new CounterMutationSpeculativeCommit(
                domain.Cache,
                prepared.NamespaceName,
                token.UserId,
                prepared.CounterName,
                token.TimeOffset,
                expectedId,
                cached.Item1.Revision,
                current => {
                    var changed = current.Clone() as Counter;
                    var changedAny = false;
                    var values = new List<ScopedValue>();
                    foreach (var value in current.Values) {
                        if (value == null) continue;
                        var next = value.Clone() as ScopedValue;
                        if (next == null) continue;
                        values.Add(next);
                        if (next.ScopeType != "resetTiming" ||
                            next.NextResetAt.HasValue &&
                            next.NextResetAt <= logicalTimeMillis ||
                            !next.Value.HasValue) continue;
                        changedAny = true;
                        var decreased = new BigInteger(next.Value.Value) -
                                        prepared.Value.Value;
                        next.Value = decreased < 0 ? 0 : (long)decreased;
                        next.UpdatedAt = logicalTimeMillis;
                    }
                    if (!changedAny) return current;
                    changed.Values = values.ToArray();
                    changed.UpdatedAt = logicalTimeMillis;
                    changed.Revision = 0;
                    return changed;
                }
            );
            return commit.Invoke;
/* diff +++ end */
        }
    }
}
