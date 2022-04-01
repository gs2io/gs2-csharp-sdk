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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Watch.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StatsEvent : IComparable
	{
        public string Grn { set; get; }
        public string Service { set; get; }
        public string Method { set; get; }
        public string Metric { set; get; }
        public bool? Cumulative { set; get; }
        public double? Value { set; get; }
        public string[] Tags { set; get; }
        public long? CallAt { set; get; }

        public StatsEvent WithGrn(string grn) {
            this.Grn = grn;
            return this;
        }

        public StatsEvent WithService(string service) {
            this.Service = service;
            return this;
        }

        public StatsEvent WithMethod(string method) {
            this.Method = method;
            return this;
        }

        public StatsEvent WithMetric(string metric) {
            this.Metric = metric;
            return this;
        }

        public StatsEvent WithCumulative(bool? cumulative) {
            this.Cumulative = cumulative;
            return this;
        }

        public StatsEvent WithValue(double? value) {
            this.Value = value;
            return this;
        }

        public StatsEvent WithTags(string[] tags) {
            this.Tags = tags;
            return this;
        }

        public StatsEvent WithCallAt(long? callAt) {
            this.CallAt = callAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StatsEvent FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StatsEvent()
                .WithGrn(!data.Keys.Contains("grn") || data["grn"] == null ? null : data["grn"].ToString())
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithMetric(!data.Keys.Contains("metric") || data["metric"] == null ? null : data["metric"].ToString())
                .WithCumulative(!data.Keys.Contains("cumulative") || data["cumulative"] == null ? null : (bool?)bool.Parse(data["cumulative"].ToString()))
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (double?)double.Parse(data["value"].ToString()))
                .WithTags(!data.Keys.Contains("tags") || data["tags"] == null ? new string[]{} : data["tags"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCallAt(!data.Keys.Contains("callAt") || data["callAt"] == null ? null : (long?)long.Parse(data["callAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["grn"] = Grn,
                ["service"] = Service,
                ["method"] = Method,
                ["metric"] = Metric,
                ["cumulative"] = Cumulative,
                ["value"] = Value,
                ["tags"] = new JsonData(Tags == null ? new JsonData[]{} :
                        Tags.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["callAt"] = CallAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Grn != null) {
                writer.WritePropertyName("grn");
                writer.Write(Grn.ToString());
            }
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (Method != null) {
                writer.WritePropertyName("method");
                writer.Write(Method.ToString());
            }
            if (Metric != null) {
                writer.WritePropertyName("metric");
                writer.Write(Metric.ToString());
            }
            if (Cumulative != null) {
                writer.WritePropertyName("cumulative");
                writer.Write(bool.Parse(Cumulative.ToString()));
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(double.Parse(Value.ToString()));
            }
            if (Tags != null) {
                writer.WritePropertyName("tags");
                writer.WriteArrayStart();
                foreach (var tag in Tags)
                {
                    if (tag != null) {
                        writer.Write(tag.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CallAt != null) {
                writer.WritePropertyName("callAt");
                writer.Write(long.Parse(CallAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StatsEvent;
            var diff = 0;
            if (Grn == null && Grn == other.Grn)
            {
                // null and null
            }
            else
            {
                diff += Grn.CompareTo(other.Grn);
            }
            if (Service == null && Service == other.Service)
            {
                // null and null
            }
            else
            {
                diff += Service.CompareTo(other.Service);
            }
            if (Method == null && Method == other.Method)
            {
                // null and null
            }
            else
            {
                diff += Method.CompareTo(other.Method);
            }
            if (Metric == null && Metric == other.Metric)
            {
                // null and null
            }
            else
            {
                diff += Metric.CompareTo(other.Metric);
            }
            if (Cumulative == null && Cumulative == other.Cumulative)
            {
                // null and null
            }
            else
            {
                diff += Cumulative == other.Cumulative ? 0 : 1;
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            if (Tags == null && Tags == other.Tags)
            {
                // null and null
            }
            else
            {
                diff += Tags.Length - other.Tags.Length;
                for (var i = 0; i < Tags.Length; i++)
                {
                    diff += Tags[i].CompareTo(other.Tags[i]);
                }
            }
            if (CallAt == null && CallAt == other.CallAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CallAt - other.CallAt);
            }
            return diff;
        }
    }
}