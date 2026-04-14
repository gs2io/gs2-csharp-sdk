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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetTraceResult : IResult
	{
        public Gs2.Gs2Log.Model.Trace Trace { set; get; }
        public Gs2.Gs2Log.Model.Trace[] Parallels { set; get; }
        public bool? ParallelTruncated { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetTraceResult WithTrace(Gs2.Gs2Log.Model.Trace trace) {
            this.Trace = trace;
            return this;
        }

        public GetTraceResult WithParallels(Gs2.Gs2Log.Model.Trace[] parallels) {
            this.Parallels = parallels;
            return this;
        }

        public GetTraceResult WithParallelTruncated(bool? parallelTruncated) {
            this.ParallelTruncated = parallelTruncated;
            return this;
        }

        public GetTraceResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetTraceResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetTraceResult()
                .WithTrace(!data.Keys.Contains("trace") || data["trace"] == null ? null : Gs2.Gs2Log.Model.Trace.FromJson(data["trace"]))
                .WithParallels(!data.Keys.Contains("parallels") || data["parallels"] == null || !data["parallels"].IsArray ? null : data["parallels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.Trace.FromJson(v);
                }).ToArray())
                .WithParallelTruncated(!data.Keys.Contains("parallelTruncated") || data["parallelTruncated"] == null ? null : (bool?)bool.Parse(data["parallelTruncated"].ToString()))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            JsonData parallelsJsonData = null;
            if (Parallels != null && Parallels.Length > 0)
            {
                parallelsJsonData = new JsonData();
                foreach (var parallel in Parallels)
                {
                    parallelsJsonData.Add(parallel.ToJson());
                }
            }
            return new JsonData {
                ["trace"] = Trace?.ToJson(),
                ["parallels"] = parallelsJsonData,
                ["parallelTruncated"] = ParallelTruncated,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Trace != null) {
                Trace.WriteJson(writer);
            }
            if (Parallels != null) {
                writer.WritePropertyName("parallels");
                writer.WriteArrayStart();
                foreach (var parallel in Parallels)
                {
                    if (parallel != null) {
                        parallel.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ParallelTruncated != null) {
                writer.WritePropertyName("parallelTruncated");
                writer.Write(bool.Parse(ParallelTruncated.ToString()));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}