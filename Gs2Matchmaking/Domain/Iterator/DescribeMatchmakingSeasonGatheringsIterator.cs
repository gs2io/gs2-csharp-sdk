
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
using Gs2.Gs2Matchmaking.Model.Cache;
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

namespace Gs2.Gs2Matchmaking.Domain.Iterator
{

    #if UNITY_2017_1_OR_NEWER
    public class DescribeMatchmakingSeasonGatheringsIterator : Gs2Iterator<Gs2.Gs2Matchmaking.Model.SeasonGathering> {
    #else
    public class DescribeMatchmakingSeasonGatheringsIterator : IAsyncEnumerable<Gs2.Gs2Matchmaking.Model.SeasonGathering> {
    #endif
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2MatchmakingRestClient _client;
        public string NamespaceName { get; }
        public string SeasonName { get; }
        public long? Season { get; }
        public long? Tier { get; }
        private string _pageToken;
        private bool _isCacheChecked;
        private bool _last;
        private Gs2.Gs2Matchmaking.Model.SeasonGathering[] _result;

        public static int? fetchSize;

        public DescribeMatchmakingSeasonGatheringsIterator(
            Gs2.Core.Domain.Gs2 gs2,
            Gs2MatchmakingRestClient client,
            string namespaceName,
            string seasonName,
            long? season,
            long? tier = null
        ) {
            this._gs2 = gs2;
            this._client = client;
            this.NamespaceName = namespaceName;
            this.SeasonName = seasonName;
            this.Season = season;
            this.Tier = tier;
            this._pageToken = null;
            this._last = false;
            this._result = new Gs2.Gs2Matchmaking.Model.SeasonGathering[]{};
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
            if (!isCacheChecked && this._gs2.Cache.TryGetList
                    <Gs2.Gs2Matchmaking.Model.SeasonGathering>
            (
                    (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                        NamespaceName,
                        default,
                        SeasonName,
                        Season,
                        null
                    ),
                    out var list
            )) {
                this._result = list
                    .Where(item => this.Tier == null || item.Tier == this.Tier)
                    .ToArray();
                this._pageToken = null;
                this._last = true;
            } else {

                var request = new Gs2.Gs2Matchmaking.Request.DescribeMatchmakingSeasonGatheringsRequest()
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithSeasonName(this.SeasonName)
                    .WithSeason(this.Season)
                    .WithPageToken(this._pageToken)
                    .WithLimit(fetchSize);
                #if UNITY_2017_1_OR_NEWER && !GS2_ENABLE_UNITASK
                var future = this._client.DescribeMatchmakingSeasonGatheringsFuture(
                #else
                var r = await this._client.DescribeMatchmakingSeasonGatheringsAsync(
                #endif
                    request
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
                    .Where(item => this.Tier == null || item.Tier == this.Tier)
                    .ToArray();
                this._pageToken = r.NextPageToken;
                this._last = this._pageToken == null;
                r.PutCache(
                    this._gs2.Cache,
                    null,
                    null,
                    request
                );

                if (this._last) {
                    this._gs2.Cache.SetListCached<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                        (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                            NamespaceName,
                            default,
                            SeasonName,
                            Season,
                            null
                        )
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
            Action<AsyncResult<Gs2.Gs2Matchmaking.Model.SeasonGathering>> callback
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
                        var ret = this._result[0];
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
            callback.Invoke(new AsyncResult<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                Current,
                error
            ));
        }
        #endif

        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerable<Gs2.Gs2Matchmaking.Model.SeasonGathering> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
            #else

        protected override IEnumerator Next(
            Action<AsyncResult<Gs2.Gs2Matchmaking.Model.SeasonGathering>> callback
            #endif
        #else
        public async IAsyncEnumerator<Gs2.Gs2Matchmaking.Model.SeasonGathering> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        #endif
        )
        {
        #if UNITY_2017_1_OR_NEWER
            #if GS2_ENABLE_UNITASK
            return UniTaskAsyncEnumerable.Create<Gs2.Gs2Matchmaking.Model.SeasonGathering>(async (writer, token) =>
            {
            #endif
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
                using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                        (null as Gs2.Gs2Matchmaking.Model.SeasonGathering).CacheParentKey(
                            NamespaceName,
                            default,
                            SeasonName,
                            Season,
                            null
                       ),
                       "ListSeasonGathering"
                   ).LockAsync()) {
                while(this._hasNext()) {
                    cancellationToken.ThrowIfCancellationRequested();
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
                        callback.Invoke(new AsyncResult<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                            Current,
                            Error
                        ));
                        yield break;
        #else
                        break;
        #endif
                    }
                    var ret = this._result[0];
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
                    callback.Invoke(new AsyncResult<Gs2.Gs2Matchmaking.Model.SeasonGathering>(
                        Current,
                        Error
                    ));
            #endif
        #else
                    yield return ret;
        #endif
        #if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
                }
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
