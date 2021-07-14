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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Script.Request
{
	[Preserve]
	[System.Serializable]
	public class GetScriptRequest : Gs2Request<GetScriptRequest>
	{
        public string NamespaceName { set; get; }
        public string ScriptName { set; get; }

        public GetScriptRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public GetScriptRequest WithScriptName(string scriptName) {
            this.ScriptName = scriptName;
            return this;
        }

    	[Preserve]
        public static GetScriptRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetScriptRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithScriptName(!data.Keys.Contains("scriptName") || data["scriptName"] == null ? null : data["scriptName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["scriptName"] = ScriptName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ScriptName != null) {
                writer.WritePropertyName("scriptName");
                writer.Write(ScriptName.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}