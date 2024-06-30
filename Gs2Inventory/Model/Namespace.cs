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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting AcquireScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting OverflowScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting ConsumeScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting SimpleItemAcquireScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting SimpleItemConsumeScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting BigItemAcquireScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ScriptSetting BigItemConsumeScript { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.LogSetting LogSetting { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Namespace WithSimpleItemAcquireScript(Gs2.Gs2Inventory.Model.ScriptSetting simpleItemAcquireScript) {
            this.SimpleItemAcquireScript = simpleItemAcquireScript;
            return this;
        }
        public Namespace WithSimpleItemConsumeScript(Gs2.Gs2Inventory.Model.ScriptSetting simpleItemConsumeScript) {
            this.SimpleItemConsumeScript = simpleItemConsumeScript;
            return this;
        }
        public Namespace WithBigItemAcquireScript(Gs2.Gs2Inventory.Model.ScriptSetting bigItemAcquireScript) {
            this.BigItemAcquireScript = bigItemAcquireScript;
            return this;
        }
        public Namespace WithBigItemConsumeScript(Gs2.Gs2Inventory.Model.ScriptSetting bigItemConsumeScript) {
            this.BigItemConsumeScript = bigItemConsumeScript;
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
        public Namespace WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+)",
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
                .WithAcquireScript(!data.Keys.Contains("acquireScript") || data["acquireScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["acquireScript"]))
                .WithOverflowScript(!data.Keys.Contains("overflowScript") || data["overflowScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["overflowScript"]))
                .WithConsumeScript(!data.Keys.Contains("consumeScript") || data["consumeScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["consumeScript"]))
                .WithSimpleItemAcquireScript(!data.Keys.Contains("simpleItemAcquireScript") || data["simpleItemAcquireScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["simpleItemAcquireScript"]))
                .WithSimpleItemConsumeScript(!data.Keys.Contains("simpleItemConsumeScript") || data["simpleItemConsumeScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["simpleItemConsumeScript"]))
                .WithBigItemAcquireScript(!data.Keys.Contains("bigItemAcquireScript") || data["bigItemAcquireScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["bigItemAcquireScript"]))
                .WithBigItemConsumeScript(!data.Keys.Contains("bigItemConsumeScript") || data["bigItemConsumeScript"] == null ? null : Gs2.Gs2Inventory.Model.ScriptSetting.FromJson(data["bigItemConsumeScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Inventory.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["simpleItemAcquireScript"] = SimpleItemAcquireScript?.ToJson(),
                ["simpleItemConsumeScript"] = SimpleItemConsumeScript?.ToJson(),
                ["bigItemAcquireScript"] = BigItemAcquireScript?.ToJson(),
                ["bigItemConsumeScript"] = BigItemConsumeScript?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (SimpleItemAcquireScript != null) {
                writer.WritePropertyName("simpleItemAcquireScript");
                SimpleItemAcquireScript.WriteJson(writer);
            }
            if (SimpleItemConsumeScript != null) {
                writer.WritePropertyName("simpleItemConsumeScript");
                SimpleItemConsumeScript.WriteJson(writer);
            }
            if (BigItemAcquireScript != null) {
                writer.WritePropertyName("bigItemAcquireScript");
                BigItemAcquireScript.WriteJson(writer);
            }
            if (BigItemConsumeScript != null) {
                writer.WritePropertyName("bigItemConsumeScript");
                BigItemConsumeScript.WriteJson(writer);
            }
            if (LogSetting != null) {
                writer.WritePropertyName("logSetting");
                LogSetting.WriteJson(writer);
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (SimpleItemAcquireScript == null && SimpleItemAcquireScript == other.SimpleItemAcquireScript)
            {
                // null and null
            }
            else
            {
                diff += SimpleItemAcquireScript.CompareTo(other.SimpleItemAcquireScript);
            }
            if (SimpleItemConsumeScript == null && SimpleItemConsumeScript == other.SimpleItemConsumeScript)
            {
                // null and null
            }
            else
            {
                diff += SimpleItemConsumeScript.CompareTo(other.SimpleItemConsumeScript);
            }
            if (BigItemAcquireScript == null && BigItemAcquireScript == other.BigItemAcquireScript)
            {
                // null and null
            }
            else
            {
                diff += BigItemAcquireScript.CompareTo(other.BigItemAcquireScript);
            }
            if (BigItemConsumeScript == null && BigItemConsumeScript == other.BigItemConsumeScript)
            {
                // null and null
            }
            else
            {
                diff += BigItemConsumeScript.CompareTo(other.BigItemConsumeScript);
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (NamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.description.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "inventory.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                AcquireScript = AcquireScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                OverflowScript = OverflowScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                ConsumeScript = ConsumeScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                SimpleItemAcquireScript = SimpleItemAcquireScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                SimpleItemConsumeScript = SimpleItemConsumeScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                BigItemAcquireScript = BigItemAcquireScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                BigItemConsumeScript = BigItemConsumeScript.Clone() as Gs2.Gs2Inventory.Model.ScriptSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Inventory.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}