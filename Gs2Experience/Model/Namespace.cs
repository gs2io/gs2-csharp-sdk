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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ExperienceCapScriptId { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeExperienceScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeRankScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting ChangeRankCapScript { set; get; }
        public Gs2.Gs2Experience.Model.ScriptSetting OverflowExperienceScript { set; get; }
        public Gs2.Gs2Experience.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Namespace WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }

        public Namespace WithName(string name) {
            this.Name = name;
            return this;
        }

        public Namespace WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public Namespace WithExperienceCapScriptId(string experienceCapScriptId) {
            this.ExperienceCapScriptId = experienceCapScriptId;
            return this;
        }

        public Namespace WithChangeExperienceScript(Gs2.Gs2Experience.Model.ScriptSetting changeExperienceScript) {
            this.ChangeExperienceScript = changeExperienceScript;
            return this;
        }

        public Namespace WithChangeRankScript(Gs2.Gs2Experience.Model.ScriptSetting changeRankScript) {
            this.ChangeRankScript = changeRankScript;
            return this;
        }

        public Namespace WithChangeRankCapScript(Gs2.Gs2Experience.Model.ScriptSetting changeRankCapScript) {
            this.ChangeRankCapScript = changeRankCapScript;
            return this;
        }

        public Namespace WithOverflowExperienceScript(Gs2.Gs2Experience.Model.ScriptSetting overflowExperienceScript) {
            this.OverflowExperienceScript = overflowExperienceScript;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Experience.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

        public Namespace WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Namespace WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithExperienceCapScriptId(!data.Keys.Contains("experienceCapScriptId") || data["experienceCapScriptId"] == null ? null : data["experienceCapScriptId"].ToString())
                .WithChangeExperienceScript(!data.Keys.Contains("changeExperienceScript") || data["changeExperienceScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeExperienceScript"]))
                .WithChangeRankScript(!data.Keys.Contains("changeRankScript") || data["changeRankScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeRankScript"]))
                .WithChangeRankCapScript(!data.Keys.Contains("changeRankCapScript") || data["changeRankCapScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["changeRankCapScript"]))
                .WithOverflowExperienceScript(!data.Keys.Contains("overflowExperienceScript") || data["overflowExperienceScript"] == null ? null : Gs2.Gs2Experience.Model.ScriptSetting.FromJson(data["overflowExperienceScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Experience.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["experienceCapScriptId"] = ExperienceCapScriptId,
                ["changeExperienceScript"] = ChangeExperienceScript?.ToJson(),
                ["changeRankScript"] = ChangeRankScript?.ToJson(),
                ["changeRankCapScript"] = ChangeRankCapScript?.ToJson(),
                ["overflowExperienceScript"] = OverflowExperienceScript?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
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
                writer.WritePropertyName("changeExperienceScript");
                ChangeExperienceScript.WriteJson(writer);
            }
            if (ChangeRankScript != null) {
                writer.WritePropertyName("changeRankScript");
                ChangeRankScript.WriteJson(writer);
            }
            if (ChangeRankCapScript != null) {
                writer.WritePropertyName("changeRankCapScript");
                ChangeRankCapScript.WriteJson(writer);
            }
            if (OverflowExperienceScript != null) {
                writer.WritePropertyName("overflowExperienceScript");
                OverflowExperienceScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Namespace;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (ExperienceCapScriptId == null && ExperienceCapScriptId == other.ExperienceCapScriptId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceCapScriptId.CompareTo(other.ExperienceCapScriptId);
            }
            if (ChangeExperienceScript == null && ChangeExperienceScript == other.ChangeExperienceScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeExperienceScript.CompareTo(other.ChangeExperienceScript);
            }
            if (ChangeRankScript == null && ChangeRankScript == other.ChangeRankScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeRankScript.CompareTo(other.ChangeRankScript);
            }
            if (ChangeRankCapScript == null && ChangeRankCapScript == other.ChangeRankCapScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeRankCapScript.CompareTo(other.ChangeRankCapScript);
            }
            if (OverflowExperienceScript == null && OverflowExperienceScript == other.OverflowExperienceScript)
            {
                // null and null
            }
            else
            {
                diff += OverflowExperienceScript.CompareTo(other.OverflowExperienceScript);
            }
            if (LogSetting == null && LogSetting == other.LogSetting)
            {
                // null and null
            }
            else
            {
                diff += LogSetting.CompareTo(other.LogSetting);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}