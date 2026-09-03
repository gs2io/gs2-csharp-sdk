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
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Guild.Model;
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
    public static class SetMaximumCurrentMaximumMemberCountByGuildNameSpeculativeExecutor {

        public static string Action() {
            return "Gs2Guild:SetMaximumCurrentMaximumMemberCountByGuildName";
        }

        public static Guild Transform(
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            Guild item,
            GuildModel guildModel,
            long currentTimeMillis,
            string region = null,
            string ownerId = null
        ) {
            return item.SpeculativeExecutionAt(
                request,
                guildModel,
                currentTimeMillis,
                region,
                ownerId
            );
        }

        public static void Commit(
            CacheDatabase cache,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request,
            Guild item,
            int? timeOffset
        ) {
            item.PutCache(
                cache,
                request.NamespaceName,
                request.GuildModelName,
                request.GuildName,
                timeOffset
            );
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null || token == null)
                return null;
            var prepared = SetMaximumCurrentMaximumMemberCountByGuildNameRequest
                .FromJson(request.ToJson());
            if (!prepared.Value.HasValue) return null;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId;
            var expectedGuildId = $"grn:gs2:{region}:{ownerId}:guild:" +
                $"{prepared.NamespaceName}:guild:{prepared.GuildModelName}:" +
                prepared.GuildName;
            var expectedModelId = $"grn:gs2:{region}:{ownerId}:guild:" +
                $"{prepared.NamespaceName}:model:{prepared.GuildModelName}";
            var cached = ((Guild)null).GetCache(
                domain.Cache, prepared.NamespaceName, prepared.GuildModelName,
                prepared.GuildName, token.TimeOffset
            );
            var model = ((GuildModel)null).GetCache(
                domain.Cache, prepared.NamespaceName,
                prepared.GuildModelName, null
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.GuildId != expectedGuildId ||
                cached.Item1.GuildModelName != prepared.GuildModelName ||
                cached.Item1.Name != prepared.GuildName ||
                cached.Item1.Members == null ||
                !model.Item2 || model.Item1 == null ||
                model.Item1.GuildModelId != expectedModelId ||
                model.Item1.Name != prepared.GuildModelName) return null;
            var value = prepared.Value.Value;
            return new GuildMaximumMemberCountMutationSpeculativeCommit(
                domain.Cache, prepared.NamespaceName,
                prepared.GuildModelName, prepared.GuildName, token.TimeOffset,
                expectedGuildId, expectedModelId, cached.Item1.Revision, true,
                (current, maximum) => {
                    if (current.Members == null) return false;
                    var next = Math.Min((long)value, maximum);
                    next = Math.Max(next, current.Members.Length);
                    if (next > int.MaxValue || next < int.MinValue)
                        return false;
                    current.CurrentMaximumMemberCount = (int)next;
                    return true;
                }
            ).Invoke;
        }
    }
}
