
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

#pragma warning disable 1998

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Domain;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
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
using Gs2.Core;
using Gs2.Core.Exception;
    #endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Identifier.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
    public class DescribeIdentifiersIterator : Gs2Iterator<Gs2.Gs2Identifier.Model.Identifier> {
    #else
    public class DescribeIdentifiersIterator : IAsyncEnumerable<Gs2.Gs2Identifier.Model.Identifier> {
    #endif
        private readonly CacheDatabase _cache;
        private readonly Gs2IdentifierRestClient _client;
        private readonly string _userName;
        private string _pageToken;
        private bool _last;
        private Gs2.Gs2Identifier.Model.Identifier[] _result;

        int? fetchSize;

        public DescribeIdentifiersIterator(
            CacheDatabase cache,
            Gs2IdentifierRestClient client,
            string userName
        ) {
            this._cache = cache;
            this._client = client;
            this._userName = userName;
            this._pageToken = null;
            this._last = false;
            this._result = new Gs2.Gs2Identifier.Model.Identifier[]{};

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
            var parentKey = Gs2.Gs2Identifier.Domain.Model.UserDomain.CreateCacheParentKey(
                this._userName?.ToString() ?? null,
                "Identifier"
            );
            string listParentKey = parentKey;
            if (this._cache.IsListCached<Gs2.Gs2Identifier.Model.Identifier>
            (
                    listParentKey
            )) {
                this._result = this._cache.List<Gs2.Gs2Identifier.Model.Identifier>
                (
                        parentKey
                )
                    .ToArray();
                this._pageToken = null;
                this._last = true;
            } else {

                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = this._client.DescribeIdentifiersFuture(
                #else
                var r = await this._client.DescribeIdentifiersAsync(
                #endif
                    new Gs2.Gs2Identifier.Request.DescribeIdentifiersRequest()
                        .WithUserName(this._userName)
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
                this._result = r.Items;
                this._pageToken = r.NextPageToken;
                this._last = this._pageToken == null;
                foreach (var item in this._result) {
                    this._cache.Put(
                            parentKey,
                            Gs2.Gs2Identifier.Domain.Model.IdentifierDomain.CreateCacheKey(
                                    item.ClientId?.ToString()
                            ),
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }

                if (this._last) {
                    this._cache.ListCached<Gs2.Gs2Identifier.Model.Identifier>(
                            listParentKey
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
            Action<Gs2.Gs2Identifier.Model.Identifier> callback
        )
        {
            yield return UniTask.ToCoroutine(
                async () => {
                    if (this._result.Length == 0 && !this._last) {
                        await this._load();
                    }
                    if (this._result.Length == 0) {
                        Current = null;
                        return;
                    }
                    Gs2.Gs2Identifier.Model.Identifier ret = this._result[0];
                    this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
                    if (this._result.Length == 0 && !this._last) {
                        await this._load();
                    }
                    Current = ret;
                }
            );
            callback.Invoke(Current);
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Identifier.Model.Identifier> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
            #else

        protected override IEnumerator Next(
            Action<Gs2.Gs2Identifier.Model.Identifier> callback
            #endif
        #else
        public async IAsyncEnumerator<Gs2.Gs2Identifier.Model.Identifier> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        #endif
        )
        {
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            return UniTaskAsyncEnumerable.Create<Gs2.Gs2Identifier.Model.Identifier>(async (writer, token) =>
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
                    yield break;
        #else
                    break;
        #endif
                }
                Gs2.Gs2Identifier.Model.Identifier ret = this._result[0];
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
