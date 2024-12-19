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

namespace Gs2.Gs2Distributor.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class BatchExecuteApiRequest : Gs2Request<BatchExecuteApiRequest>
	{
         public Gs2.Gs2Distributor.Model.BatchRequestPayload[] RequestPayloads { set; get; } = null!;
        public BatchExecuteApiRequest WithRequestPayloads(Gs2.Gs2Distributor.Model.BatchRequestPayload[] requestPayloads) {
            this.RequestPayloads = requestPayloads;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchExecuteApiRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchExecuteApiRequest()
                .WithRequestPayloads(!data.Keys.Contains("requestPayloads") || data["requestPayloads"] == null || !data["requestPayloads"].IsArray ? null : data["requestPayloads"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Distributor.Model.BatchRequestPayload.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData requestPayloadsJsonData = null;
            if (RequestPayloads != null && RequestPayloads.Length > 0)
            {
                requestPayloadsJsonData = new JsonData();
                foreach (var requestPayload in RequestPayloads)
                {
                    requestPayloadsJsonData.Add(requestPayload.ToJson());
                }
            }
            return new JsonData {
                ["requestPayloads"] = requestPayloadsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RequestPayloads != null) {
                writer.WritePropertyName("requestPayloads");
                writer.WriteArrayStart();
                foreach (var requestPayload in RequestPayloads)
                {
                    if (requestPayload != null) {
                        requestPayload.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += RequestPayloads + ":";
            return key;
        }
    }
}