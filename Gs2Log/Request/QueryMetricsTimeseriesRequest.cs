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
	public class QueryMetricsTimeseriesRequest : Gs2Request<QueryMetricsTimeseriesRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public long? Begin { set; get; } = null!;
         public long? End { set; get; } = null!;
         public string Query { set; get; } = null!;
         public string[] GroupBy { set; get; } = null!;
         public Gs2.Gs2Log.Model.AggregationConfig[] Aggregations { set; get; } = null!;
         public int? Interval { set; get; } = null!;
         public int? SeriesLimit { set; get; } = null!;
         public string OrderKey { set; get; } = null!;
         public string OrderBy { set; get; } = null!;
        public QueryMetricsTimeseriesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithBegin(long? begin) {
            this.Begin = begin;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithEnd(long? end) {
            this.End = end;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithQuery(string query) {
            this.Query = query;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithGroupBy(string[] groupBy) {
            this.GroupBy = groupBy;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithAggregations(Gs2.Gs2Log.Model.AggregationConfig[] aggregations) {
            this.Aggregations = aggregations;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithInterval(int? interval) {
            this.Interval = interval;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithSeriesLimit(int? seriesLimit) {
            this.SeriesLimit = seriesLimit;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithOrderKey(string orderKey) {
            this.OrderKey = orderKey;
            return this;
        }
        public QueryMetricsTimeseriesRequest WithOrderBy(string orderBy) {
            this.OrderBy = orderBy;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static QueryMetricsTimeseriesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new QueryMetricsTimeseriesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBegin(!data.Keys.Contains("begin") || data["begin"] == null ? null : (long?)(data["begin"].ToString().Contains(".") ? (long)double.Parse(data["begin"].ToString()) : long.Parse(data["begin"].ToString())))
                .WithEnd(!data.Keys.Contains("end") || data["end"] == null ? null : (long?)(data["end"].ToString().Contains(".") ? (long)double.Parse(data["end"].ToString()) : long.Parse(data["end"].ToString())))
                .WithQuery(!data.Keys.Contains("query") || data["query"] == null ? null : data["query"].ToString())
                .WithGroupBy(!data.Keys.Contains("groupBy") || data["groupBy"] == null || !data["groupBy"].IsArray ? null : data["groupBy"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithAggregations(!data.Keys.Contains("aggregations") || data["aggregations"] == null || !data["aggregations"].IsArray ? null : data["aggregations"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.AggregationConfig.FromJson(v);
                }).ToArray())
                .WithInterval(!data.Keys.Contains("interval") || data["interval"] == null ? null : (int?)(data["interval"].ToString().Contains(".") ? (int)double.Parse(data["interval"].ToString()) : int.Parse(data["interval"].ToString())))
                .WithSeriesLimit(!data.Keys.Contains("seriesLimit") || data["seriesLimit"] == null ? null : (int?)(data["seriesLimit"].ToString().Contains(".") ? (int)double.Parse(data["seriesLimit"].ToString()) : int.Parse(data["seriesLimit"].ToString())))
                .WithOrderKey(!data.Keys.Contains("orderKey") || data["orderKey"] == null ? null : data["orderKey"].ToString())
                .WithOrderBy(!data.Keys.Contains("orderBy") || data["orderBy"] == null ? null : data["orderBy"].ToString());
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
            JsonData aggregationsJsonData = null;
            if (Aggregations != null && Aggregations.Length > 0)
            {
                aggregationsJsonData = new JsonData();
                foreach (var aggregation in Aggregations)
                {
                    aggregationsJsonData.Add(aggregation.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["begin"] = Begin,
                ["end"] = End,
                ["query"] = Query,
                ["groupBy"] = groupByJsonData,
                ["aggregations"] = aggregationsJsonData,
                ["interval"] = Interval,
                ["seriesLimit"] = SeriesLimit,
                ["orderKey"] = OrderKey,
                ["orderBy"] = OrderBy,
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
            if (Aggregations != null) {
                writer.WritePropertyName("aggregations");
                writer.WriteArrayStart();
                foreach (var aggregation in Aggregations)
                {
                    if (aggregation != null) {
                        aggregation.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Interval != null) {
                writer.WritePropertyName("interval");
                writer.Write((Interval.ToString().Contains(".") ? (int)double.Parse(Interval.ToString()) : int.Parse(Interval.ToString())));
            }
            if (SeriesLimit != null) {
                writer.WritePropertyName("seriesLimit");
                writer.Write((SeriesLimit.ToString().Contains(".") ? (int)double.Parse(SeriesLimit.ToString()) : int.Parse(SeriesLimit.ToString())));
            }
            if (OrderKey != null) {
                writer.WritePropertyName("orderKey");
                writer.Write(OrderKey.ToString());
            }
            if (OrderBy != null) {
                writer.WritePropertyName("orderBy");
                writer.Write(OrderBy.ToString());
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
            key += Aggregations + ":";
            key += Interval + ":";
            key += SeriesLimit + ":";
            key += OrderKey + ":";
            key += OrderBy + ":";
            return key;
        }
    }
}