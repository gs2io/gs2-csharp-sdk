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
using Gs2.Gs2Experience.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Experience.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
        public string Name { set; get; }
        public string Description { set; get; }
        public string ExperienceCapScriptId { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeExperienceScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeRankScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeRankCapScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting OverflowExperienceScript { set; get; }
        public Gs2.Gs2Experience.Model.LogSetting LogSetting { set; get; }

        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }

        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public CreateNamespaceRequest WithExperienceCapScriptId(string experienceCapScriptId) {
            this.ExperienceCapScriptId = experienceCapScriptId;
            return this;
        }

        public CreateNamespaceRequest WithChangeExperienceScript(Gs2.Gs2Experience.Model.ScriptSetting changeExperienceScript) {
            this.ChangeExperienceScript = changeExperienceScript;
            return this;
        }

        public CreateNamespaceRequest WithChangeRankScript(Gs2.Gs2Experience.Model.ScriptSetting changeRankScript) {
            this.ChangeRankScript = changeRankScript;
            return this;
        }

        public CreateNamespaceRequest WithChangeRankCapScript(Gs2.Gs2Experience.Model.ScriptSetting changeRankCapScript) {
            this.ChangeRankCapScript = changeRankCapScript;
            return this;
        }

        public CreateNamespaceRequest WithOverflowExperienceScript(Gs2.Gs2Experience.Model.ScriptSetting overflowExperienceScript) {
            this.OverflowExperienceScript = overflowExperienceScript;
            return this;
        }

        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2Experience.Model.LogSetting logSetting) {
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
                .WithExperienceCapScriptId(!data.Keys.Contains("experienceCapScriptId") || data["experienceCapScriptId"] == null ? null : data["experienceCapScriptId"].ToString())
                .WithChangeExperienceScript(!data.Keys.Contains("changeExperienceScript") || data["changeExperienceScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeExperienceScript"]))
                .WithChangeRankScript(!data.Keys.Contains("changeRankScript") || data["changeRankScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeRankScript"]))
                .WithChangeRankCapScript(!data.Keys.Contains("changeRankCapScript") || data["changeRankCapScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeRankCapScript"]))
                .WithOverflowExperienceScript(!data.Keys.Contains("overflowExperienceScript") || data["overflowExperienceScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["overflowExperienceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Experience.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["description"] = Description,
                ["experienceCapScriptId"] = ExperienceCapScriptId,
                ["changeExperienceScript"] = ChangeExperienceScript?.ToJson(),
                ["changeRankScript"] = ChangeRankScript?.ToJson(),
                ["changeRankCapScript"] = ChangeRankCapScript?.ToJson(),
                ["overflowExperienceScript"] = OverflowExperienceScript?.ToJson(),
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
            if (ExperienceCapScriptId != null) {
                writer.WritePropertyName("experienceCapScriptId");
                writer.Write(ExperienceCapScriptId.ToString());
            }
            if (ChangeExperienceScript != null) {
                ChangeExperienceScript.WriteJson(writer);
            }
            if (ChangeRankScript != null) {
                ChangeRankScript.WriteJson(writer);
            }
            if (ChangeRankCapScript != null) {
                ChangeRankCapScript.WriteJson(writer);
            }
            if (OverflowExperienceScript != null) {
                OverflowExperienceScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}