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
using Gs2.Gs2Freeze.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Freeze.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetOutputRequest : Gs2Request<GetOutputRequest>
	{
         public string StageName { set; get; } = null!;
         public string OutputName { set; get; } = null!;
        public GetOutputRequest WithStageName(string stageName) {
            this.StageName = stageName;
            return this;
        }
        public GetOutputRequest WithOutputName(string outputName) {
            this.OutputName = outputName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetOutputRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetOutputRequest()
                .WithStageName(!data.Keys.Contains("stageName") || data["stageName"] == null ? null : data["stageName"].ToString())
                .WithOutputName(!data.Keys.Contains("outputName") || data["outputName"] == null ? null : data["outputName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["stageName"] = StageName,
                ["outputName"] = OutputName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StageName != null) {
                writer.WritePropertyName("stageName");
                writer.Write(StageName.ToString());
            }
            if (OutputName != null) {
                writer.WritePropertyName("outputName");
                writer.Write(OutputName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += StageName + ":";
            key += OutputName + ":";
            return key;
        }
    }
}