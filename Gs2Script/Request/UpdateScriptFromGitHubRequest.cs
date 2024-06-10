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

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Script.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateScriptFromGitHubRequest : Gs2Request<UpdateScriptFromGitHubRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ScriptName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2Script.Model.GitHubCheckoutSetting CheckoutSetting { set; get; } = null!;
        public UpdateScriptFromGitHubRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateScriptFromGitHubRequest WithScriptName(string scriptName) {
            this.ScriptName = scriptName;
            return this;
        }
        public UpdateScriptFromGitHubRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateScriptFromGitHubRequest WithCheckoutSetting(Gs2.Gs2Script.Model.GitHubCheckoutSetting checkoutSetting) {
            this.CheckoutSetting = checkoutSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateScriptFromGitHubRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateScriptFromGitHubRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithScriptName(!data.Keys.Contains("scriptName") || data["scriptName"] == null ? null : data["scriptName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithCheckoutSetting(!data.Keys.Contains("checkoutSetting") || data["checkoutSetting"] == null ? null : Gs2.Gs2Script.Model.GitHubCheckoutSetting.FromJson(data["checkoutSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["scriptName"] = ScriptName,
                ["description"] = Description,
                ["checkoutSetting"] = CheckoutSetting?.ToJson(),
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (CheckoutSetting != null) {
                CheckoutSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ScriptName + ":";
            key += Description + ":";
            key += CheckoutSetting + ":";
            return key;
        }
    }
}