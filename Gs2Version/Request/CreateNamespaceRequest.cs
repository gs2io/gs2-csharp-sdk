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
using Gs2.Gs2Version.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string AssumeUserId { set; get; } = null!;
         public Gs2.Gs2Version.Model.ScriptSetting AcceptVersionScript { set; get; } = null!;
         public string CheckVersionTriggerScriptId { set; get; } = null!;
         public Gs2.Gs2Version.Model.LogSetting LogSetting { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithAssumeUserId(string assumeUserId) {
            this.AssumeUserId = assumeUserId;
            return this;
        }
        public CreateNamespaceRequest WithAcceptVersionScript(Gs2.Gs2Version.Model.ScriptSetting acceptVersionScript) {
            this.AcceptVersionScript = acceptVersionScript;
            return this;
        }
        public CreateNamespaceRequest WithCheckVersionTriggerScriptId(string checkVersionTriggerScriptId) {
            this.CheckVersionTriggerScriptId = checkVersionTriggerScriptId;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Version.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateNamespaceRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAssumeUserId(!data.Keys.Contains("assumeUserId") || data["assumeUserId"] == null ? null : data["assumeUserId"].ToString())
                .WithAcceptVersionScript(!data.Keys.Contains("acceptVersionScript") || data["acceptVersionScript"] == null ? null : Gs2.Gs2Version.Model.ScriptSetting.FromJson(data["acceptVersionScript"]))
                .WithCheckVersionTriggerScriptId(!data.Keys.Contains("checkVersionTriggerScriptId") || data["checkVersionTriggerScriptId"] == null ? null : data["checkVersionTriggerScriptId"].ToString())
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Version.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["assumeUserId"] = AssumeUserId,
                ["acceptVersionScript"] = AcceptVersionScript?.ToJson(),
                ["checkVersionTriggerScriptId"] = CheckVersionTriggerScriptId,
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (AssumeUserId != null) {
                writer.WritePropertyName("assumeUserId");
                writer.Write(AssumeUserId.ToString());
            }
            if (AcceptVersionScript != null) {
                AcceptVersionScript.WriteJson(writer);
            }
            if (CheckVersionTriggerScriptId != null) {
                writer.WritePropertyName("checkVersionTriggerScriptId");
                writer.Write(CheckVersionTriggerScriptId.ToString());
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Description + ":";
            key += AssumeUserId + ":";
            key += AcceptVersionScript + ":";
            key += CheckVersionTriggerScriptId + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}