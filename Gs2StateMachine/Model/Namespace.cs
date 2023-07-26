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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting StartScript { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting PassScript { set; get; }
        public Gs2.Gs2StateMachine.Model.ScriptSetting ErrorScript { set; get; }
        public long? LowestStateMachineVersion { set; get; }
        public Gs2.Gs2StateMachine.Model.LogSetting LogSetting { set; get; }
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
        public Namespace WithStartScript(Gs2.Gs2StateMachine.Model.ScriptSetting startScript) {
            this.StartScript = startScript;
            return this;
        }
        public Namespace WithPassScript(Gs2.Gs2StateMachine.Model.ScriptSetting passScript) {
            this.PassScript = passScript;
            return this;
        }
        public Namespace WithErrorScript(Gs2.Gs2StateMachine.Model.ScriptSetting errorScript) {
            this.ErrorScript = errorScript;
            return this;
        }
        public Namespace WithLowestStateMachineVersion(long? lowestStateMachineVersion) {
            this.LowestStateMachineVersion = lowestStateMachineVersion;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2StateMachine.Model.LogSetting logSetting) {
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

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
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
                .WithStartScript(!data.Keys.Contains("startScript") || data["startScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["startScript"]))
                .WithPassScript(!data.Keys.Contains("passScript") || data["passScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["passScript"]))
                .WithErrorScript(!data.Keys.Contains("errorScript") || data["errorScript"] == null ? null : Gs2.Gs2StateMachine.Model.ScriptSetting.FromJson(data["errorScript"]))
                .WithLowestStateMachineVersion(!data.Keys.Contains("lowestStateMachineVersion") || data["lowestStateMachineVersion"] == null ? null : (long?)long.Parse(data["lowestStateMachineVersion"].ToString()))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2StateMachine.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["startScript"] = StartScript?.ToJson(),
                ["passScript"] = PassScript?.ToJson(),
                ["errorScript"] = ErrorScript?.ToJson(),
                ["lowestStateMachineVersion"] = LowestStateMachineVersion,
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
            if (StartScript != null) {
                writer.WritePropertyName("startScript");
                StartScript.WriteJson(writer);
            }
            if (PassScript != null) {
                writer.WritePropertyName("passScript");
                PassScript.WriteJson(writer);
            }
            if (ErrorScript != null) {
                writer.WritePropertyName("errorScript");
                ErrorScript.WriteJson(writer);
            }
            if (LowestStateMachineVersion != null) {
                writer.WritePropertyName("lowestStateMachineVersion");
                writer.Write(long.Parse(LowestStateMachineVersion.ToString()));
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
            if (StartScript == null && StartScript == other.StartScript)
            {
                // null and null
            }
            else
            {
                diff += StartScript.CompareTo(other.StartScript);
            }
            if (PassScript == null && PassScript == other.PassScript)
            {
                // null and null
            }
            else
            {
                diff += PassScript.CompareTo(other.PassScript);
            }
            if (ErrorScript == null && ErrorScript == other.ErrorScript)
            {
                // null and null
            }
            else
            {
                diff += ErrorScript.CompareTo(other.ErrorScript);
            }
            if (LowestStateMachineVersion == null && LowestStateMachineVersion == other.LowestStateMachineVersion)
            {
                // null and null
            }
            else
            {
                diff += (int)(LowestStateMachineVersion - other.LowestStateMachineVersion);
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