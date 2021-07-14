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
using UnityEngine.Scripting;

namespace Gs2.Gs2Inventory.Model
{

	[Preserve]
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public Gs2.Gs2Inventory.Model.ScriptSetting AcquireScript { set; get; }
        public Gs2.Gs2Inventory.Model.ScriptSetting OverflowScript { set; get; }
        public Gs2.Gs2Inventory.Model.ScriptSetting ConsumeScript { set; get; }
        public Gs2.Gs2Inventory.Model.LogSetting LogSetting { set; get; }
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

        public Namespace WithAcquireScript(Gs2.Gs2Inventory.Model.ScriptSetting acquireScript) {
            this.AcquireScript = acquireScript;
            return this;
        }

        public Namespace WithOverflowScript(Gs2.Gs2Inventory.Model.ScriptSetting overflowScript) {
            this.OverflowScript = overflowScript;
            return this;
        }

        public Namespace WithConsumeScript(Gs2.Gs2Inventory.Model.ScriptSetting consumeScript) {
            this.ConsumeScript = consumeScript;
            return this;
        }

        public Namespace WithLogSetting(Gs2.Gs2Inventory.Model.LogSetting logSetting) {
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

    	[Preserve]
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAcquireScript(!data.Keys.Contains("acquireScript") || data["acquireScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["acquireScript"]))
                .WithOverflowScript(!data.Keys.Contains("overflowScript") || data["overflowScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["overflowScript"]))
                .WithConsumeScript(!data.Keys.Contains("consumeScript") || data["consumeScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["consumeScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Inventory.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["acquireScript"] = AcquireScript?.ToJson(),
                ["overflowScript"] = OverflowScript?.ToJson(),
                ["consumeScript"] = ConsumeScript?.ToJson(),
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
            if (AcquireScript != null) {
                writer.WritePropertyName("acquireScript");
                AcquireScript.WriteJson(writer);
            }
            if (OverflowScript != null) {
                writer.WritePropertyName("overflowScript");
                OverflowScript.WriteJson(writer);
            }
            if (ConsumeScript != null) {
                writer.WritePropertyName("consumeScript");
                ConsumeScript.WriteJson(writer);
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
            if (AcquireScript == null && AcquireScript == other.AcquireScript)
            {
                // null and null
            }
            else
            {
                diff += AcquireScript.CompareTo(other.AcquireScript);
            }
            if (OverflowScript == null && OverflowScript == other.OverflowScript)
            {
                // null and null
            }
            else
            {
                diff += OverflowScript.CompareTo(other.OverflowScript);
            }
            if (ConsumeScript == null && ConsumeScript == other.ConsumeScript)
            {
                // null and null
            }
            else
            {
                diff += ConsumeScript.CompareTo(other.ConsumeScript);
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