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
using Gs2.Gs2Distributor.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Distributor.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class BatchExecuteApiResult : IResult
	{
        public Gs2.Gs2Distributor.Model.BatchResultPayload[] Results { set; get; }
        public ResultMetadata Metadata { set; get; }

        public BatchExecuteApiResult WithResults(Gs2.Gs2Distributor.Model.BatchResultPayload[] results) {
            this.Results = results;
            return this;
        }

        public BatchExecuteApiResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchExecuteApiResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchExecuteApiResult()
                .WithResults(!data.Keys.Contains("results") || data["results"] == null || !data["results"].IsArray ? null : data["results"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Distributor.Model.BatchResultPayload.FromJson(v);
                }).ToArray())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            JsonData resultsJsonData = null;
            if (Results != null && Results.Length > 0)
            {
                resultsJsonData = new JsonData();
                foreach (var result in Results)
                {
                    resultsJsonData.Add(result.ToJson());
                }
            }
            return new JsonData {
                ["results"] = resultsJsonData,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Results != null) {
                writer.WritePropertyName("results");
                writer.WriteArrayStart();
                foreach (var result in Results)
                {
                    if (result != null) {
                        result.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}