
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
 *
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

namespace Gs2.Gs2Ranking.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
    public class DescribeNearRankingsIterator : Gs2Iterator<Gs2.Gs2Ranking.Model.Ranking> {
    #else
    public class DescribeNearRankingsIterator : IAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> {
    #endif
        private readonly CacheDatabase _cache;
        private readonly Gs2RankingRestClient _client;
        private readonly string _namespaceName;
        private readonly string _categoryName;
        private readonly long? _score;
        private bool _last;
        private Gs2.Gs2Ranking.Model.Ranking[] _result;

        int? fetchSize;

        public DescribeNearRankingsIterator(
            CacheDatabase cache,
            Gs2RankingRestClient client,
            string namespaceName,
            string categoryName,
            long? score
        ) {
            this._cache = cache;
            this._client = client;
            this._namespaceName = namespaceName;
            this._categoryName = categoryName;
            this._score = score;
            this._last = false;
            this._result = new Gs2.Gs2Ranking.Model.Ranking[]{};

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
            var parentKey = Gs2.Gs2Ranking.Domain.Model.UserDomain.CreateCacheParentKey(
                this._namespaceName != null ? this._namespaceName.ToString() : null,
                "Singleton",
                "NearRanking"
            );
            string listParentKey = parentKey;
            if (this._cache.IsListCached<Gs2.Gs2Ranking.Model.Ranking>
            (
                    listParentKey
            )) {
                this._result = this._cache.List<Gs2.Gs2Ranking.Model.Ranking>
                (
                        listParentKey
                )
                    .Where(item => this._categoryName == null || item.CategoryName == this._categoryName)
                    .Where(item => this._score == null || item.Score == this._score)
                    .ToArray();
                this._last = true;
            } else {

                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = this._client.DescribeNearRankingsFuture(
                #else
                var r = await this._client.DescribeNearRankingsAsync(
                #endif
                    new Gs2.Gs2Ranking.Request.DescribeNearRankingsRequest()
                        .WithNamespaceName(this._namespaceName)
                        .WithCategoryName(this._categoryName)
                        .WithScore(this._score)
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
                this._last = true;
                foreach (var item in this._result) {
                    this._cache.Put(
                            listParentKey,
                            Gs2.Gs2Ranking.Domain.Model.RankingDomain.CreateCacheKey(
                                    item.CategoryName?.ToString()
                            ),
                            item,
                            UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
                    );
                }

                if (this._last) {
                    this._cache.ListCached<Gs2.Gs2Ranking.Model.Ranking>(
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
            Action<Gs2.Gs2Ranking.Model.Ranking> callback
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
                    Gs2.Gs2Ranking.Model.Ranking ret = this._result[0];
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
        public IUniTaskAsyncEnumerable<Gs2.Gs2Ranking.Model.Ranking> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
            #else

        protected override IEnumerator Next(
            Action<Gs2.Gs2Ranking.Model.Ranking> callback
            #endif
        #else
        public async IAsyncEnumerator<Gs2.Gs2Ranking.Model.Ranking> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        #endif
        )
        {
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            return UniTaskAsyncEnumerable.Create<Gs2.Gs2Ranking.Model.Ranking>(async (writer, token) =>
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
                Gs2.Gs2Ranking.Model.Ranking ret = this._result[0];
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
