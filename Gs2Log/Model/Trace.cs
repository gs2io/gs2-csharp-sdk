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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Trace : IComparable
	{
        public string TraceId { set; get; }
        public Gs2.Gs2Log.Model.LogEntry[] Spans { set; get; }
        public bool? Truncated { set; get; }
        public Trace WithTraceId(string traceId) {
            this.TraceId = traceId;
            return this;
        }
        public Trace WithSpans(Gs2.Gs2Log.Model.LogEntry[] spans) {
            this.Spans = spans;
            return this;
        }
        public Trace WithTruncated(bool? truncated) {
            this.Truncated = truncated;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Trace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Trace()
                .WithTraceId(!data.Keys.Contains("traceId") || data["traceId"] == null ? null : data["traceId"].ToString())
                .WithSpans(!data.Keys.Contains("spans") || data["spans"] == null || !data["spans"].IsArray ? null : data["spans"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.LogEntry.FromJson(v);
                }).ToArray())
                .WithTruncated(!data.Keys.Contains("truncated") || data["truncated"] == null ? null : (bool?)bool.Parse(data["truncated"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData spansJsonData = null;
            if (Spans != null && Spans.Length > 0)
            {
                spansJsonData = new JsonData();
                foreach (var span in Spans)
                {
                    spansJsonData.Add(span.ToJson());
                }
            }
            return new JsonData {
                ["traceId"] = TraceId,
                ["spans"] = spansJsonData,
                ["truncated"] = Truncated,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TraceId != null) {
                writer.WritePropertyName("traceId");
                writer.Write(TraceId.ToString());
            }
            if (Spans != null) {
                writer.WritePropertyName("spans");
                writer.WriteArrayStart();
                foreach (var span in Spans)
                {
                    if (span != null) {
                        span.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Truncated != null) {
                writer.WritePropertyName("truncated");
                writer.Write(bool.Parse(Truncated.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Trace;
            var diff = 0;
            if (TraceId == null && TraceId == other.TraceId)
            {
                // null and null
            }
            else
            {
                diff += TraceId.CompareTo(other.TraceId);
            }
            if (Spans == null && Spans == other.Spans)
            {
                // null and null
            }
            else
            {
                diff += Spans.Length - other.Spans.Length;
                for (var i = 0; i < Spans.Length; i++)
                {
                    diff += Spans[i].CompareTo(other.Spans[i]);
                }
            }
            if (Truncated == null && Truncated == other.Truncated)
            {
                // null and null
            }
            else
            {
                diff += Truncated == other.Truncated ? 0 : 1;
            }
            return diff;
        }

        public void Validate() {
            {
                if (TraceId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("trace", "log.trace.traceId.error.tooLong"),
                    });
                }
            }
            {
                if (Spans.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("trace", "log.trace.spans.error.tooMany"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new Trace {
                TraceId = TraceId,
                Spans = Spans?.Clone() as Gs2.Gs2Log.Model.LogEntry[],
                Truncated = Truncated,
            };
        }
    }
}