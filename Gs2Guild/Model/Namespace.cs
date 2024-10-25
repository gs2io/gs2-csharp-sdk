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

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting ChangeNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting JoinNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting LeaveNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting ChangeMemberNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting ReceiveRequestNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.NotificationSetting RemoveRequestNotification { set; get; } = null!;
        public Gs2.Gs2Guild.Model.ScriptSetting CreateGuildScript { set; get; } = null!;
        public Gs2.Gs2Guild.Model.ScriptSetting UpdateGuildScript { set; get; } = null!;
        public Gs2.Gs2Guild.Model.ScriptSetting JoinGuildScript { set; get; } = null!;
        public Gs2.Gs2Guild.Model.ScriptSetting LeaveGuildScript { set; get; } = null!;
        public Gs2.Gs2Guild.Model.ScriptSetting ChangeRoleScript { set; get; } = null!;
        public Gs2.Gs2Guild.Model.LogSetting LogSetting { set; get; } = null!;
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
        public Namespace WithChangeNotification(Gs2.Gs2Guild.Model.NotificationSetting changeNotification) {
            this.ChangeNotification = changeNotification;
            return this;
        }
        public Namespace WithJoinNotification(Gs2.Gs2Guild.Model.NotificationSetting joinNotification) {
            this.JoinNotification = joinNotification;
            return this;
        }
        public Namespace WithLeaveNotification(Gs2.Gs2Guild.Model.NotificationSetting leaveNotification) {
            this.LeaveNotification = leaveNotification;
            return this;
        }
        public Namespace WithChangeMemberNotification(Gs2.Gs2Guild.Model.NotificationSetting changeMemberNotification) {
            this.ChangeMemberNotification = changeMemberNotification;
            return this;
        }
        public Namespace WithReceiveRequestNotification(Gs2.Gs2Guild.Model.NotificationSetting receiveRequestNotification) {
            this.ReceiveRequestNotification = receiveRequestNotification;
            return this;
        }
        public Namespace WithRemoveRequestNotification(Gs2.Gs2Guild.Model.NotificationSetting removeRequestNotification) {
            this.RemoveRequestNotification = removeRequestNotification;
            return this;
        }
        public Namespace WithCreateGuildScript(Gs2.Gs2Guild.Model.ScriptSetting createGuildScript) {
            this.CreateGuildScript = createGuildScript;
            return this;
        }
        public Namespace WithUpdateGuildScript(Gs2.Gs2Guild.Model.ScriptSetting updateGuildScript) {
            this.UpdateGuildScript = updateGuildScript;
            return this;
        }
        public Namespace WithJoinGuildScript(Gs2.Gs2Guild.Model.ScriptSetting joinGuildScript) {
            this.JoinGuildScript = joinGuildScript;
            return this;
        }
        public Namespace WithLeaveGuildScript(Gs2.Gs2Guild.Model.ScriptSetting leaveGuildScript) {
            this.LeaveGuildScript = leaveGuildScript;
            return this;
        }
        public Namespace WithChangeRoleScript(Gs2.Gs2Guild.Model.ScriptSetting changeRoleScript) {
            this.ChangeRoleScript = changeRoleScript;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Guild.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+)",
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
                .WithChangeNotification(!data.Keys.Contains("changeNotification") || data["changeNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["changeNotification"]))
                .WithJoinNotification(!data.Keys.Contains("joinNotification") || data["joinNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["joinNotification"]))
                .WithLeaveNotification(!data.Keys.Contains("leaveNotification") || data["leaveNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["leaveNotification"]))
                .WithChangeMemberNotification(!data.Keys.Contains("changeMemberNotification") || data["changeMemberNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["changeMemberNotification"]))
                .WithReceiveRequestNotification(!data.Keys.Contains("receiveRequestNotification") || data["receiveRequestNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["receiveRequestNotification"]))
                .WithRemoveRequestNotification(!data.Keys.Contains("removeRequestNotification") || data["removeRequestNotification"] == null ? null : Gs2.Gs2Guild.Model.NotificationSetting.FromJson(data["removeRequestNotification"]))
                .WithCreateGuildScript(!data.Keys.Contains("createGuildScript") || data["createGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["createGuildScript"]))
                .WithUpdateGuildScript(!data.Keys.Contains("updateGuildScript") || data["updateGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["updateGuildScript"]))
                .WithJoinGuildScript(!data.Keys.Contains("joinGuildScript") || data["joinGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["joinGuildScript"]))
                .WithLeaveGuildScript(!data.Keys.Contains("leaveGuildScript") || data["leaveGuildScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["leaveGuildScript"]))
                .WithChangeRoleScript(!data.Keys.Contains("changeRoleScript") || data["changeRoleScript"] == null ? null : Gs2.Gs2Guild.Model.ScriptSetting.FromJson(data["changeRoleScript"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Guild.Model.LogSetting.FromJson(data["logSetting"]))
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
                ["changeNotification"] = ChangeNotification?.ToJson(),
                ["joinNotification"] = JoinNotification?.ToJson(),
                ["leaveNotification"] = LeaveNotification?.ToJson(),
                ["changeMemberNotification"] = ChangeMemberNotification?.ToJson(),
                ["receiveRequestNotification"] = ReceiveRequestNotification?.ToJson(),
                ["removeRequestNotification"] = RemoveRequestNotification?.ToJson(),
                ["createGuildScript"] = CreateGuildScript?.ToJson(),
                ["updateGuildScript"] = UpdateGuildScript?.ToJson(),
                ["joinGuildScript"] = JoinGuildScript?.ToJson(),
                ["leaveGuildScript"] = LeaveGuildScript?.ToJson(),
                ["changeRoleScript"] = ChangeRoleScript?.ToJson(),
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
            if (ChangeNotification != null) {
                writer.WritePropertyName("changeNotification");
                ChangeNotification.WriteJson(writer);
            }
            if (JoinNotification != null) {
                writer.WritePropertyName("joinNotification");
                JoinNotification.WriteJson(writer);
            }
            if (LeaveNotification != null) {
                writer.WritePropertyName("leaveNotification");
                LeaveNotification.WriteJson(writer);
            }
            if (ChangeMemberNotification != null) {
                writer.WritePropertyName("changeMemberNotification");
                ChangeMemberNotification.WriteJson(writer);
            }
            if (ReceiveRequestNotification != null) {
                writer.WritePropertyName("receiveRequestNotification");
                ReceiveRequestNotification.WriteJson(writer);
            }
            if (RemoveRequestNotification != null) {
                writer.WritePropertyName("removeRequestNotification");
                RemoveRequestNotification.WriteJson(writer);
            }
            if (CreateGuildScript != null) {
                writer.WritePropertyName("createGuildScript");
                CreateGuildScript.WriteJson(writer);
            }
            if (UpdateGuildScript != null) {
                writer.WritePropertyName("updateGuildScript");
                UpdateGuildScript.WriteJson(writer);
            }
            if (JoinGuildScript != null) {
                writer.WritePropertyName("joinGuildScript");
                JoinGuildScript.WriteJson(writer);
            }
            if (LeaveGuildScript != null) {
                writer.WritePropertyName("leaveGuildScript");
                LeaveGuildScript.WriteJson(writer);
            }
            if (ChangeRoleScript != null) {
                writer.WritePropertyName("changeRoleScript");
                ChangeRoleScript.WriteJson(writer);
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
            if (ChangeNotification == null && ChangeNotification == other.ChangeNotification)
            {
                // null and null
            }
            else
            {
                diff += ChangeNotification.CompareTo(other.ChangeNotification);
            }
            if (JoinNotification == null && JoinNotification == other.JoinNotification)
            {
                // null and null
            }
            else
            {
                diff += JoinNotification.CompareTo(other.JoinNotification);
            }
            if (LeaveNotification == null && LeaveNotification == other.LeaveNotification)
            {
                // null and null
            }
            else
            {
                diff += LeaveNotification.CompareTo(other.LeaveNotification);
            }
            if (ChangeMemberNotification == null && ChangeMemberNotification == other.ChangeMemberNotification)
            {
                // null and null
            }
            else
            {
                diff += ChangeMemberNotification.CompareTo(other.ChangeMemberNotification);
            }
            if (ReceiveRequestNotification == null && ReceiveRequestNotification == other.ReceiveRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += ReceiveRequestNotification.CompareTo(other.ReceiveRequestNotification);
            }
            if (RemoveRequestNotification == null && RemoveRequestNotification == other.RemoveRequestNotification)
            {
                // null and null
            }
            else
            {
                diff += RemoveRequestNotification.CompareTo(other.RemoveRequestNotification);
            }
            if (CreateGuildScript == null && CreateGuildScript == other.CreateGuildScript)
            {
                // null and null
            }
            else
            {
                diff += CreateGuildScript.CompareTo(other.CreateGuildScript);
            }
            if (UpdateGuildScript == null && UpdateGuildScript == other.UpdateGuildScript)
            {
                // null and null
            }
            else
            {
                diff += UpdateGuildScript.CompareTo(other.UpdateGuildScript);
            }
            if (JoinGuildScript == null && JoinGuildScript == other.JoinGuildScript)
            {
                // null and null
            }
            else
            {
                diff += JoinGuildScript.CompareTo(other.JoinGuildScript);
            }
            if (LeaveGuildScript == null && LeaveGuildScript == other.LeaveGuildScript)
            {
                // null and null
            }
            else
            {
                diff += LeaveGuildScript.CompareTo(other.LeaveGuildScript);
            }
            if (ChangeRoleScript == null && ChangeRoleScript == other.ChangeRoleScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeRoleScript.CompareTo(other.ChangeRoleScript);
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
                        new RequestError("namespace", "guild.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.description.error.tooLong"),
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
                        new RequestError("namespace", "guild.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "guild.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                ChangeNotification = ChangeNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                JoinNotification = JoinNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                LeaveNotification = LeaveNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                ChangeMemberNotification = ChangeMemberNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                ReceiveRequestNotification = ReceiveRequestNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                RemoveRequestNotification = RemoveRequestNotification.Clone() as Gs2.Gs2Guild.Model.NotificationSetting,
                CreateGuildScript = CreateGuildScript.Clone() as Gs2.Gs2Guild.Model.ScriptSetting,
                UpdateGuildScript = UpdateGuildScript.Clone() as Gs2.Gs2Guild.Model.ScriptSetting,
                JoinGuildScript = JoinGuildScript.Clone() as Gs2.Gs2Guild.Model.ScriptSetting,
                LeaveGuildScript = LeaveGuildScript.Clone() as Gs2.Gs2Guild.Model.ScriptSetting,
                ChangeRoleScript = ChangeRoleScript.Clone() as Gs2.Gs2Guild.Model.ScriptSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Guild.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}