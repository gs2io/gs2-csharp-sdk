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
using Gs2.Gs2Deploy.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Deploy.Request
{
	[Preserve]
	[System.Serializable]
	public class GetOutputRequest : Gs2Request<GetOutputRequest>
	{
        public string StackName { set; get; }
        public string OutputName { set; get; }

        public GetOutputRequest WithStackName(string stackName) {
            this.StackName = stackName;
            return this;
        }

        public GetOutputRequest WithOutputName(string outputName) {
            this.OutputName = outputName;
            return this;
        }

    	[Preserve]
        public static GetOutputRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetOutputRequest()
                .WithStackName(!data.Keys.Contains("stackName") || data["stackName"] == null ? null : data["stackName"].ToString())
                .WithOutputName(!data.Keys.Contains("outputName") || data["outputName"] == null ? null : data["outputName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["stackName"] = StackName,
                ["outputName"] = OutputName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StackName != null) {
                writer.WritePropertyName("stackName");
                writer.Write(StackName.ToString());
            }
            if (OutputName != null) {
                writer.WritePropertyName("outputName");
                writer.Write(OutputName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}