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
using Gs2.Gs2Watch.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Watch.Request
{
	[Preserve]
	[System.Serializable]
	public class GetCumulativeRequest : Gs2Request<GetCumulativeRequest>
	{
        public string Name { set; get; }
        public string ResourceGrn { set; get; }

        public GetCumulativeRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public GetCumulativeRequest WithResourceGrn(string resourceGrn) {
            this.ResourceGrn = resourceGrn;
            return this;
        }

    	[Preserve]
        public static GetCumulativeRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetCumulativeRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithResourceGrn(!data.Keys.Contains("resourceGrn") || data["resourceGrn"] == null ? null : data["resourceGrn"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["resourceGrn"] = ResourceGrn,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (ResourceGrn != null) {
                writer.WritePropertyName("resourceGrn");
                writer.Write(ResourceGrn.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}