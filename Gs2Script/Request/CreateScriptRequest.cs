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
using Gs2.Gs2Script.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Script.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateScriptRequest : Gs2Request<CreateScriptRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Script { set; get; } = null!;
         public bool? DisableStringNumberToNumber { set; get; } = null!;
        public CreateScriptRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateScriptRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateScriptRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateScriptRequest WithScript(string script) {
            this.Script = script;
            return this;
        }
        public CreateScriptRequest WithDisableStringNumberToNumber(bool? disableStringNumberToNumber) {
            this.DisableStringNumberToNumber = disableStringNumberToNumber;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateScriptRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateScriptRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithScript(!data.Keys.Contains("script") || data["script"] == null ? null : data["script"].ToString())
                .WithDisableStringNumberToNumber(!data.Keys.Contains("disableStringNumberToNumber") || data["disableStringNumberToNumber"] == null ? null : (bool?)bool.Parse(data["disableStringNumberToNumber"].ToString()));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["script"] = Script,
                ["disableStringNumberToNumber"] = DisableStringNumberToNumber,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Script != null) {
                writer.WritePropertyName("script");
                writer.Write(Script.ToString());
            }
            if (DisableStringNumberToNumber != null) {
                writer.WritePropertyName("disableStringNumberToNumber");
                writer.Write(bool.Parse(DisableStringNumberToNumber.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Script + ":";
            key += DisableStringNumberToNumber + ":";
            return key;
        }
    }
}