
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
// ReSharper disable InconsistentNaming

#pragma warning disable CS0414 // Field is assigned but its value is never used
#pragma warning disable 1998

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Gs2.Core;
using Gs2.Core.Model;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Gs2Log.Model.Cache;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Events;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Log.Domain.Iterator
{

    public class QueryTimeseriesIterator :
    #if UNITY_2017_1_OR_NEWER
        Gs2Iterator<Gs2.Gs2Log.Model.TimeseriesPoint>,
    #endif
    #if GS2_ENABLE_UNITASK
        IUniTaskAsyncEnumerable<Gs2.Gs2Log.Model.TimeseriesPoint>
    #else
        IAsyncEnumerable<Gs2.Gs2Log.Model.TimeseriesPoint>
    #endif
    {
        private readonly Gs2.Core.Domain.Gs2 _gs2;
        private readonly Gs2LogRestClient _client;
        public string NamespaceName { get; }
        public long? Begin { get; }
        public long? End { get; }
        public string Query { get; }
        public string[] GroupBy { get; }
        public Gs2.Gs2Log.Model.AggregationConfig Aggregation { get; }
        public int? Interval { get; }
        public int? SeriesLimit { get; }
        private string _pageToken;
        private bool _isCacheChecked;
        private bool _last;
        private Gs2.Gs2Log.Model.TimeseriesPoint[] _result;

        public static int? fetchSize;

        public QueryTimeseriesIterator(
            Gs2.Core.Domain.Gs2 gs2,
            Gs2LogRestClient client,
            string namespaceName,
            Gs2.Gs2Log.Model.AggregationConfig aggregation,
            long? begin = null,
            long? end = null,
            string query = null,
            string[] groupBy = null,
            int? interval = null,
            int? seriesLimit = null
        ) {
            this._gs2 = gs2;
            this._client = client;
            this.NamespaceName = namespaceName;
            this.Begin = begin;
            this.End = end;
            this.Query = query;
            this.GroupBy = groupBy;
            this.Aggregation = aggregation;
            this.Interval = interval;
            this.SeriesLimit = seriesLimit;
            this._pageToken = null;
            this._last = false;
            this._result = new Gs2.Gs2Log.Model.TimeseriesPoint[]{};
        }

        #if GS2_ENABLE_UNITASK
        private async UniTask _load() {
        #else
        private async Task _load() {
        #endif
            var isCacheChecked = this._isCacheChecked;
            this._isCacheChecked = true;
            if (!isCacheChecked && this._gs2.Cache.TryGetList
                    <Gs2.Gs2Log.Model.TimeseriesPoint>
            (
                    (null as Gs2.Gs2Log.Model.TimeseriesPoint).CacheParentKey(
                        null
                    ),
                    out var list
            )) {
                this._result = list
/* diff --- start
                    .Where(item => this.NamespaceName == null || item.NamespaceName == this.NamespaceName)
 diff --- end */
                    .Where(item => this.Begin == null || item.Timestamp >= this.Begin)
                    .Where(item => this.End == null || item.Timestamp <= this.End)
/* diff --- start
                    .Where(item => this.Query == null || item.Query == this.Query)
                    .Where(item => this.GroupBy == null || item.GroupBy == this.GroupBy)
                    .Where(item => this.Aggregation == null || item.Aggregation == this.Aggregation)
                    .Where(item => this.Interval == null || item.Interval == this.Interval)
                    .Where(item => this.SeriesLimit == null || item.SeriesLimit == this.SeriesLimit)
 diff --- end */
                    .ToArray();
                this._pageToken = null;
                this._last = true;
            } else {

                var request = new Gs2.Gs2Log.Request.QueryTimeseriesRequest()
                    .WithContextStack(this._gs2.DefaultContextStack)
                    .WithNamespaceName(this.NamespaceName)
                    .WithBegin(this.Begin)
                    .WithEnd(this.End)
                    .WithAggregation(this.Aggregation)
                    .WithInterval(this.Interval)
                    .WithSeriesLimit(this.SeriesLimit)
                    .WithPageToken(this._pageToken)
                    .WithLimit(fetchSize);
                var r = await this._client.QueryTimeseriesAsync(
                    request
                );
                this._result = r.Items
/* diff --- start
                    .Where(item => this.NamespaceName == null || item.NamespaceName == this.NamespaceName)
 diff --- end */
                    .Where(item => this.Begin == null || item.Timestamp >= this.Begin)
                    .Where(item => this.End == null || item.Timestamp <= this.End)
/* diff --- start
                    .Where(item => this.Query == null || item.Query == this.Query)
                    .Where(item => this.GroupBy == null || item.GroupBy == this.GroupBy)
                    .Where(item => this.Aggregation == null || item.Aggregation == this.Aggregation)
                    .Where(item => this.Interval == null || item.Interval == this.Interval)
                    .Where(item => this.SeriesLimit == null || item.SeriesLimit == this.SeriesLimit)
 diff --- end */
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
                    this._gs2.Cache.SetListCached<Gs2.Gs2Log.Model.TimeseriesPoint>(
                        (null as Gs2.Gs2Log.Model.TimeseriesPoint).CacheParentKey(
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

        protected override System.Collections.IEnumerator Next(
            Action<AsyncResult<Gs2.Gs2Log.Model.TimeseriesPoint>> callback
        )
        {
            if (this._result.Length == 0 && !this._last) {
                var future = this._load().ToGs2Future();
                yield return future;
                if (future.Error != null)
                {
                    Current = null;
                    Error = future.Error;
                    callback.Invoke(new AsyncResult<Gs2.Gs2Log.Model.TimeseriesPoint>(
                        Current,
                        Error
                    ));
                    yield break;
                }
            }
            if (this._result.Length == 0) {
                Current = null;
                callback.Invoke(new AsyncResult<Gs2.Gs2Log.Model.TimeseriesPoint>(
                    Current,
                    Error
                ));
                yield break;
            }
            var ret = this._result[0];
            this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
            Current = ret;
            callback.Invoke(new AsyncResult<Gs2.Gs2Log.Model.TimeseriesPoint>(
                Current,
                Error
            ));
        }
        #endif

        #if GS2_ENABLE_UNITASK
        public IUniTaskAsyncEnumerator<Gs2.Gs2Log.Model.TimeseriesPoint> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        ) => UniTaskAsyncEnumerable.Create<Gs2.Gs2Log.Model.TimeseriesPoint>(async (writer, token) =>
        #else
        public async IAsyncEnumerator<Gs2.Gs2Log.Model.TimeseriesPoint> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken()
        )
        #endif
        {
            using (await this._gs2.Cache.GetLockObject<Gs2.Gs2Log.Model.TimeseriesPoint>(
                    (null as Gs2.Gs2Log.Model.TimeseriesPoint).CacheParentKey(
                        null
                   ),
                   "ListTimeseriesPoint"
               ).LockAsync()) {
                while(this._hasNext()) {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (this._result.Length == 0 && !this._last) {
                        await this._load();
                    }
                    if (this._result.Length == 0) {
                        break;
                    }
                    var ret = this._result[0];
                    this._result = this._result.ToList().GetRange(1, this._result.Length - 1).ToArray();
            #if GS2_ENABLE_UNITASK
                    await writer.YieldAsync(ret);
            #else
                    yield return ret;
            #endif
                }
            }
        }
        #if GS2_ENABLE_UNITASK
        ).GetAsyncEnumerator();
        #endif
    }
}
