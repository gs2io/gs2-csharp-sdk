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
using Gs2.Gs2Project.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Project.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetServiceVersionResult : IResult
	{
        public string Item { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetServiceVersionResult WithItem(string item) {
            this.Item = item;
            return this;
        }

        public GetServiceVersionResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetServiceVersionResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetServiceVersionResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : data["item"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                writer.WritePropertyName("item");
                writer.Write(Item.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}