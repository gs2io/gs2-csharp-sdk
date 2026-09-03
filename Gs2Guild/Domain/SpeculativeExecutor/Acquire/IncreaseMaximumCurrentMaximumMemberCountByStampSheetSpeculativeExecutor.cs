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
    internal sealed class GuildMaximumMemberCountMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _guildModelName;
        private readonly string _guildName;
        private readonly int? _timeOffset;
        private readonly string _expectedGuildId;
        private readonly string _expectedModelId;
        private readonly long? _preparedRevision;
        private readonly bool _usesModel;
        private readonly Func<Guild, int, bool> _transform;

        internal GuildMaximumMemberCountMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string guildModelName,
            string guildName,
            int? timeOffset,
            string expectedGuildId,
            string expectedModelId,
            long? preparedRevision,
            bool usesModel,
            Func<Guild, int, bool> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _guildModelName = guildModelName;
            _guildName = guildName;
            _timeOffset = timeOffset;
            _expectedGuildId = expectedGuildId;
            _expectedModelId = expectedModelId;
            _preparedRevision = preparedRevision;
            _usesModel = usesModel;
            _transform = transform;
        }

        public string CompositionKey => ((Guild)null).CacheParentKey(
            _namespaceName, _timeOffset
        ) + ":" + ((Guild)null).CacheKey(
            _guildModelName, _guildName
        );

        private bool IsExpected(Guild item) {
            return item != null && item.GuildId == _expectedGuildId &&
                   item.GuildModelName == _guildModelName &&
                   item.Name == _guildName;
        }

        private bool TryGetMaximum(out int maximum) {
            maximum = int.MaxValue;
            if (!_usesModel) return true;
            var cached = ((GuildModel)null).GetCache(
                _cache, _namespaceName, _guildModelName, null
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.GuildModelId != _expectedModelId ||
                cached.Item1.Name != _guildModelName) return false;
            maximum = cached.Item1.MaximumMemberCount ?? int.MaxValue;
            return true;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                if (!TryGetMaximum(out var maximum)) {
                    next = null;
                    return false;
                }
                Guild item;
                if (hasCurrent) {
                    item = current as Guild;
                }
                else {
                    var cached = ((Guild)null).GetCache(
                        _cache, _namespaceName, _guildModelName, _guildName,
                        _timeOffset
                    );
                    item = cached.Item1;
                    if (!cached.Item2 || !IsExpected(item) ||
                        (item.Revision > 0 &&
                         item.Revision != _preparedRevision)) {
                        next = null;
                        return false;
                    }
                }
                if (!IsExpected(item)) {
                    next = null;
                    return false;
                }
                var changed = item.Clone() as Guild;
                if (!_transform(changed, maximum) || !IsExpected(changed)) {
                    next = null;
                    return false;
                }
                changed.Revision = 0;
                next = changed;
                return true;
            }
            catch (System.Exception) {
                next = null;
                return false;
            }
        }

        public object Commit(object value) {
            if (value is Guild item && IsExpected(item) &&
                item.Revision == 0) {
                _cache.Put(
                    item.CacheParentKey(_namespaceName, _timeOffset),
                    item.CacheKey(_guildModelName, _guildName),
                    item,
                    UnixTime.ToUnixTime(DateTime.Now) +
                    1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                );
            }
            return null;
        }

        public object Invoke() {
            return TryCompose(null, false, out var next)
                ? Commit(next)
                : null;
        }
    }

    public static class IncreaseMaximumCurrentMaximumMemberCountByGuildNameSpeculativeExecutor {

        public static string Action() {
            return "Gs2Guild:IncreaseMaximumCurrentMaximumMemberCountByGuildName";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest request
        ) {
            var token = accessToken?.Clone() as AccessToken;
            if (domain?.RestSession == null || request == null || token == null)
                return null;
            var prepared = IncreaseMaximumCurrentMaximumMemberCountByGuildNameRequest
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
                !cached.Item1.CurrentMaximumMemberCount.HasValue ||
                !model.Item2 || model.Item1 == null ||
                model.Item1.GuildModelId != expectedModelId ||
                model.Item1.Name != prepared.GuildModelName) return null;
            var value = prepared.Value.Value;
            return new GuildMaximumMemberCountMutationSpeculativeCommit(
                domain.Cache, prepared.NamespaceName,
                prepared.GuildModelName, prepared.GuildName, token.TimeOffset,
                expectedGuildId, expectedModelId, cached.Item1.Revision, true,
                (current, maximum) => {
                    if (!current.CurrentMaximumMemberCount.HasValue)
                        return false;
                    var previous = (long)current.CurrentMaximumMemberCount.Value;
                    var next = Math.Min(previous + value, maximum);
                    if (next < previous) next = previous;
                    if (next > int.MaxValue || next < int.MinValue)
                        return false;
                    current.CurrentMaximumMemberCount = (int)next;
                    return true;
                }
            ).Invoke;
        }
    }
}
