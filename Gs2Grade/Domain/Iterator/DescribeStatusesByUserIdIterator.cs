
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
// ReSharper disable InconsistentNaming

#pragma warning disable CS0414 // Field is assigned but its value is never used
#pragma warning disable 1998

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
    #if GS2_ENABLE_UNITASK
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
    #else
using System.Collections;
using UnityEngine.Events;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Grade.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
    public class DescribeStatusesByUserIdIterator : Gs2Iterator<Gs2.Gs2Grade.Model.Status> {
    #else
    public class DescribeStatusesByUserIdIterator : IAsyncEnumerable<Gs2.Gs2Grade.Model.Status> {
    #endif
        private readonly CacheDatabase _cache;
        private readonly Gs2GradeRestClient _client;
        private readonly string _namespaceName;
        private readonly string _gradeName;
        private readonly string _userId;
        public string NamespaceName => _namespaceName;
        public string GradeName => _gradeName;
        public string UserId => _userId;
        private string _pageToken;
        private bool _isCacheChecked;
        private bool _last;
        private Gs2.Gs2Grade.Model.Status[] _result;

        int? fetchSize;

        public DescribeStatusesByUserIdIterator(
            CacheDatabase cache,
            Gs2GradeRestClient client,
            string namespaceName,
            string gradeName,
            string userId
        ) {
            this._cache = cache;
            this._client = client;
            this._namespaceName = namespaceName;
            this._gradeName = gradeName;
            this._userId = userId;
            this._pageToken = null;
            this._last = false;
            this._result = new Gs2.Gs2Grade.Model.Status[]{};

            this.fetchSize = null;
        }

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        private async UniTask _load() {
            #else
        private IEnumerator _load() {
            #endif
        #else
        private async Task _load() {
        #endif
            var isCacheChecked = this._isCacheChecked;
            this._isCacheChecked = true;
            var parentKey = Gs2.Gs2Grade.Domain.Model.UserDomain.CreateCacheParentKey(
                this.NamespaceName,
                this.UserId,
                "Status"
            );
            if (!isCacheChecked && this._cache.TryGetList<Gs2.Gs2Grade.Model.Status>
            (
                    parentKey,
                    out var list
            )) {
                this._result = list
                    .Where(item => this._gradeName == null || item.GradeName == this._gradeName)
                    .ToArray();
                this._pageToken = null;
                this._last = true;
            } else {

                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = this._client.DescribeStatusesByUserIdFuture(
                #else
                var r = await this._client.DescribeStatusesByUserIdAsync(
                #endif
                    new Gs2.Gs2Grade.Request.DescribeStatusesByUserIdRequest()
                        .WithNamespaceName(this._namespaceName)
                        .WithUserId(this._userId)
                        .WithPageToken(this._pageToken)
                        .WithLimit(this.fetchSize)
                );
                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                yield return future;
                if (future.Error != null)
                {
                    Error = future.Error;
                    yield break;
                }
                var r = future.Result;
                #endif
                this._result = r.Items
                    .Where(item => this._gradeName == null || item.GradeName == this._gradeName)
                    .ToArray();
                this._pageToken = r.NextPageToken;
                this._last = this._pageToken == null;
                foreach (var item in r.Items) {
                    this._cache.Put(
                            parentKey,
                            Gs2.Gs2Grade.Domain.Model.StatusDomain.CreateCacheKey(
                                    item.GradeName?.ToString(),
                                    item.PropertyId?.ToString()
                            ),
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }

                if (this._last) {
                    this._cache.SetListCached<Gs2.Gs2Grade.Model.Status>(
                            parentKey
                    );
                }
            }
        }

        private bool _hasNext()
        {
            return this._result.Length != 0 || !this._last;
        }

        #if UNITY_2017_1_OR_NEWER
        public override bool HasNext()
        {
            if (Error != null) return false;
            return _hasNext();
        }
        #endif

        #if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK

        protected override System.Collections.IEnumerator Next(
            Action<AsyncResult<Gs2.Gs2Grade.Model.Status>> callback
        )
        {
            Gs2Exception error = null;
            yield return UniTask.ToCoroutine(
                async () => {
                    try {
                        if (this._result.Length == 0 && !this._last) {
                            await this._load();
                        }
                        if (this._result.Length == 0) {
                            Current = null;
                            return;
                        }
                        Gs2.Gs2Grade.Model.Status ret = this._result[0];
                        this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
                        if (this._result.Length == 0 && !this._last) {
                            await this._load();
                        }
                        Current = ret;
                    }
                    catch (Gs2Exception e) {
                        Current = null;
                        error = e;
                    }
                }
            );
            callback.Invoke(new AsyncResult<Gs2.Gs2Grade.Model.Status>(
                Current,
                error
            ));
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Grade.Model.Status> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
            #else

        protected override IEnumerator Next(
            Action<AsyncResult<Gs2.Gs2Grade.Model.Status>> callback
            #endif
        #else
        public async IAsyncEnumerator<Gs2.Gs2Grade.Model.Status> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        #endif
        )
        {
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            return UniTaskAsyncEnumerable.Create<Gs2.Gs2Grade.Model.Status>(async (writer, token) =>
            {
            #endif
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
            while(this._hasNext()) {
        #endif
                if (this._result.Length == 0 && !this._last) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return this._load();
        #else
                    await this._load();
        #endif
                }
                if (this._result.Length == 0) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    Current = null;
                    callback.Invoke(new AsyncResult<Gs2.Gs2Grade.Model.Status>(
                        Current,
                        Error
                    ));
                    yield break;
        #else
                    break;
        #endif
                }
                Gs2.Gs2Grade.Model.Status ret = this._result[0];
                this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
                if (this._result.Length == 0 && !this._last) {
        #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                    yield return this._load();
        #else
                    await this._load();
        #endif
                }
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
                await writer.YieldAsync(ret);
            #else
                Current = ret;
                callback.Invoke(new AsyncResult<Gs2.Gs2Grade.Model.Status>(
                    Current,
                    Error
                ));
            #endif
        #else
                yield return ret;
        #endif
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            }
            });
            #endif
        #else
            }
        #endif
        }
    }
}