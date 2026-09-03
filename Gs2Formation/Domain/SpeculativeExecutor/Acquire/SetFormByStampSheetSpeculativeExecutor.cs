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
/* diff +++ start */
using System.Collections.Generic;
using System.Linq;
/* diff +++ end */
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Core.Exception;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Formation.Model; /* diff +++ */
using Gs2.Gs2Formation.Request;
using Gs2.Gs2Formation.Model.Cache;
using Gs2.Gs2Formation.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Formation.Domain.SpeculativeExecutor
{
/* diff +++ start */
    internal sealed class FormMutationSpeculativeCommit :
        IComposableSpeculativeCommit
    {
        private readonly CacheDatabase _cache;
        private readonly string _namespaceName;
        private readonly string _userId;
        private readonly string _moldModelName;
        private readonly int? _index;
        private readonly int? _timeOffset;
        private readonly string _expectedFormId;
        private readonly long? _preparedRevision;
        private readonly Func<Form, bool> _transform;

        internal FormMutationSpeculativeCommit(
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string moldModelName,
            int? index,
            int? timeOffset,
            string expectedFormId,
            long? preparedRevision,
            Func<Form, bool> transform
        ) {
            _cache = cache;
            _namespaceName = namespaceName;
            _userId = userId;
            _moldModelName = moldModelName;
            _index = index;
            _timeOffset = timeOffset;
            _expectedFormId = expectedFormId;
            _preparedRevision = preparedRevision;
            _transform = transform;
        }

        public string CompositionKey => ((Form)null).CacheParentKey(
            _namespaceName, _userId, _moldModelName, _timeOffset
        ) + ":" + ((Form)null).CacheKey(_index);

        private bool IsExpected(Form item) {
            return item != null && item.FormId == _expectedFormId &&
                   item.Name == _moldModelName && item.Index == _index;
        }

        public bool TryCompose(object current, bool hasCurrent, out object next) {
            try {
                Form item;
                if (hasCurrent) {
                    item = current as Form;
                }
                else {
                    var cached = ((Form)null).GetCache(
                        _cache, _namespaceName, _userId, _moldModelName,
                        _index, _timeOffset
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
                var changed = item.Clone() as Form;
                if (!_transform(changed) || !IsExpected(changed)) {
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
            if (value is Form item && IsExpected(item) &&
                item.Revision == 0) {
                _cache.Put(
                    item.CacheParentKey(
                        _namespaceName, _userId, _moldModelName, _timeOffset
                    ),
                    item.CacheKey(_index),
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

/* diff +++ end */
    public static class SetFormByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Formation:SetFormByUserId";
        }

#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetFormByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            SetFormByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Formation.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Mold(
                request.MoldModelName
            ).Form(
                request.Index
            ).ModelAsync();

            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            var token = AccessToken.FromJson(accessToken?.ToJson());
            var prepared = SetFormByUserIdRequest.FromJson(request?.ToJson());
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
                    request.UserId,
                    request.MoldModelName,
                    request.Index,
                    null
                );
                return null;
            };
 diff --- end */
/* diff +++ start */
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(token?.UserId) ||
                prepared == null || prepared.UserId != token.UserId ||
                !prepared.Index.HasValue || prepared.Slots == null) return null;
            var expectedFormId =
                $"grn:gs2:{domain.RestSession.Region.DisplayName()}:" +
                $"{domain.RestSession.OwnerId}:formation:" +
                $"{prepared.NamespaceName}:user:{token.UserId}:mold:" +
                $"{prepared.MoldModelName}:form:{prepared.Index}";
            var cached = ((Form)null).GetCache(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.MoldModelName, prepared.Index, token.TimeOffset
            );
            if (!cached.Item2 || cached.Item1 == null ||
                cached.Item1.FormId != expectedFormId ||
                cached.Item1.Name != prepared.MoldModelName ||
                cached.Item1.Index != prepared.Index ||
                cached.Item1.Slots == null) return null;
            var slots = prepared.Slots;
            return new FormMutationSpeculativeCommit(
                domain.Cache, prepared.NamespaceName, token.UserId,
                prepared.MoldModelName, prepared.Index, token.TimeOffset,
                expectedFormId, cached.Item1.Revision,
                current => {
                    if (current.Slots == null) return false;
                    var requestedNames = new HashSet<string>(
                        slots.Select(slot => slot.Name)
                    );
                    var merged = current.Slots
                        .Where(slot => !requestedNames.Contains(slot.Name))
                        .Select(slot => slot.Clone() as Slot)
                        .ToList();
                    merged.AddRange(slots
                        .Where(slot => slot.PropertyId != null)
                        .Select(slot => slot.Clone() as Slot));
                    current.Slots = merged.ToArray();
                    return true;
                }
            ).Invoke;
/* diff +++ end */
        }
    }
}
