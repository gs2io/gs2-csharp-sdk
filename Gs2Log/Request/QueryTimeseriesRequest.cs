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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class QueryTimeseriesRequest : Gs2Request<QueryTimeseriesRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public long? Begin { set; get; } = null!;
         public long? End { set; get; } = null!;
         public string Query { set; get; } = null!;
         public string[] GroupBy { set; get; } = null!;
         public Gs2.Gs2Log.Model.AggregationConfig Aggregation { set; get; } = null!;
         public int? Interval { set; get; } = null!;
         public int? SeriesLimit { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public QueryTimeseriesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public QueryTimeseriesRequest WithBegin(long? begin) {
            this.Begin = begin;
            return this;
        }
        public QueryTimeseriesRequest WithEnd(long? end) {
            this.End = end;
            return this;
        }
        public QueryTimeseriesRequest WithQuery(string query) {
            this.Query = query;
            return this;
        }
        public QueryTimeseriesRequest WithGroupBy(string[] groupBy) {
            this.GroupBy = groupBy;
            return this;
        }
        public QueryTimeseriesRequest WithAggregation(Gs2.Gs2Log.Model.AggregationConfig aggregation) {
            this.Aggregation = aggregation;
            return this;
        }
        public QueryTimeseriesRequest WithInterval(int? interval) {
            this.Interval = interval;
            return this;
        }
        public QueryTimeseriesRequest WithSeriesLimit(int? seriesLimit) {
            this.SeriesLimit = seriesLimit;
            return this;
        }
        public QueryTimeseriesRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public QueryTimeseriesRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static QueryTimeseriesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QueryTimeseriesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBegin(!data.Keys.Contains("begin") || data["begin"] == null ? null : (long?)(data["begin"].ToString().Contains(".") ? (long)double.Parse(data["begin"].ToString()) : long.Parse(data["begin"].ToString())))
                .WithEnd(!data.Keys.Contains("end") || data["end"] == null ? null : (long?)(data["end"].ToString().Contains(".") ? (long)double.Parse(data["end"].ToString()) : long.Parse(data["end"].ToString())))
                .WithQuery(!data.Keys.Contains("query") || data["query"] == null ? null : data["query"].ToString())
                .WithGroupBy(!data.Keys.Contains("groupBy") || data["groupBy"] == null || !data["groupBy"].IsArray ? null : data["groupBy"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithAggregation(!data.Keys.Contains("aggregation") || data["aggregation"] == null ? null : Gs2.Gs2Log.Model.AggregationConfig.FromJson(data["aggregation"]))
                .WithInterval(!data.Keys.Contains("interval") || data["interval"] == null ? null : (int?)(data["interval"].ToString().Contains(".") ? (int)double.Parse(data["interval"].ToString()) : int.Parse(data["interval"].ToString())))
                .WithSeriesLimit(!data.Keys.Contains("seriesLimit") || data["seriesLimit"] == null ? null : (int?)(data["seriesLimit"].ToString().Contains(".") ? (int)double.Parse(data["seriesLimit"].ToString()) : int.Parse(data["seriesLimit"].ToString())))
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            JsonData groupByJsonData = null;
            if (GroupBy != null && GroupBy.Length > 0)
            {
                groupByJsonData = new JsonData();
                foreach (var groupB in GroupBy)
                {
                    groupByJsonData.Add(groupB);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["begin"] = Begin,
                ["end"] = End,
                ["query"] = Query,
                ["groupBy"] = groupByJsonData,
                ["aggregation"] = Aggregation?.ToJson(),
                ["interval"] = Interval,
                ["seriesLimit"] = SeriesLimit,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Begin != null) {
                writer.WritePropertyName("begin");
                writer.Write((Begin.ToString().Contains(".") ? (long)double.Parse(Begin.ToString()) : long.Parse(Begin.ToString())));
            }
            if (End != null) {
                writer.WritePropertyName("end");
                writer.Write((End.ToString().Contains(".") ? (long)double.Parse(End.ToString()) : long.Parse(End.ToString())));
            }
            if (Query != null) {
                writer.WritePropertyName("query");
                writer.Write(Query.ToString());
            }
            if (GroupBy != null) {
                writer.WritePropertyName("groupBy");
                writer.WriteArrayStart();
                foreach (var groupB in GroupBy)
                {
                    writer.Write(groupB.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (Aggregation != null) {
                Aggregation.WriteJson(writer);
            }
            if (Interval != null) {
                writer.WritePropertyName("interval");
                writer.Write((Interval.ToString().Contains(".") ? (int)double.Parse(Interval.ToString()) : int.Parse(Interval.ToString())));
            }
            if (SeriesLimit != null) {
                writer.WritePropertyName("seriesLimit");
                writer.Write((SeriesLimit.ToString().Contains(".") ? (int)double.Parse(SeriesLimit.ToString()) : int.Parse(SeriesLimit.ToString())));
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Begin + ":";
            key += End + ":";
            key += Query + ":";
            key += GroupBy + ":";
            key += Aggregation + ":";
            key += Interval + ":";
            key += SeriesLimit + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}