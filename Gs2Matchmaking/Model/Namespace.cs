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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public bool? EnableRating { set; get; }
        public string CreateGatheringTriggerType { set; get; }
        public string CreateGatheringTriggerRealtimeNamespaceId { set; get; }
        public string CreateGatheringTriggerScriptId { set; get; }
        public string CompleteMatchmakingTriggerType { set; get; }
        public string CompleteMatchmakingTriggerRealtimeNamespaceId { set; get; }
        public string CompleteMatchmakingTriggerScriptId { set; get; }
        public Gs2.Gs2Matchmaking.Model.ScriptSetting ChangeRatingScript { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting JoinNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting LeaveNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting CompleteNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.NotificationSetting ChangeRatingNotification { set; get; }
        public Gs2.Gs2Matchmaking.Model.LogSetting LogSetting { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
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
        public Namespace WithEnableRating(bool? enableRating) {
            this.EnableRating = enableRating;
            return this;
        }
        public Namespace WithCreateGatheringTriggerType(string createGatheringTriggerType) {
            this.CreateGatheringTriggerType = createGatheringTriggerType;
            return this;
        }
        public Namespace WithCreateGatheringTriggerRealtimeNamespaceId(string createGatheringTriggerRealtimeNamespaceId) {
            this.CreateGatheringTriggerRealtimeNamespaceId = createGatheringTriggerRealtimeNamespaceId;
            return this;
        }
        public Namespace WithCreateGatheringTriggerScriptId(string createGatheringTriggerScriptId) {
            this.CreateGatheringTriggerScriptId = createGatheringTriggerScriptId;
            return this;
        }
        public Namespace WithCompleteMatchmakingTriggerType(string completeMatchmakingTriggerType) {
            this.CompleteMatchmakingTriggerType = completeMatchmakingTriggerType;
            return this;
        }
        public Namespace WithCompleteMatchmakingTriggerRealtimeNamespaceId(string completeMatchmakingTriggerRealtimeNamespaceId) {
            this.CompleteMatchmakingTriggerRealtimeNamespaceId = completeMatchmakingTriggerRealtimeNamespaceId;
            return this;
        }
        public Namespace WithCompleteMatchmakingTriggerScriptId(string completeMatchmakingTriggerScriptId) {
            this.CompleteMatchmakingTriggerScriptId = completeMatchmakingTriggerScriptId;
            return this;
        }
        public Namespace WithChangeRatingScript(Gs2.Gs2Matchmaking.Model.ScriptSetting changeRatingScript) {
            this.ChangeRatingScript = changeRatingScript;
            return this;
        }
        public Namespace WithJoinNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting joinNotification) {
            this.JoinNotification = joinNotification;
            return this;
        }
        public Namespace WithLeaveNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting leaveNotification) {
            this.LeaveNotification = leaveNotification;
            return this;
        }
        public Namespace WithCompleteNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting completeNotification) {
            this.CompleteNotification = completeNotification;
            return this;
        }
        public Namespace WithChangeRatingNotification(Gs2.Gs2Matchmaking.Model.NotificationSetting changeRatingNotification) {
            this.ChangeRatingNotification = changeRatingNotification;
            return this;
        }
        public Namespace WithLogSetting(Gs2.Gs2Matchmaking.Model.LogSetting logSetting) {
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+)",
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
                .WithEnableRating(!data.Keys.Contains("enableRating") || data["enableRating"] == null ? null : (bool?)bool.Parse(data["enableRating"].ToString()))
                .WithCreateGatheringTriggerType(!data.Keys.Contains("createGatheringTriggerType") || data["createGatheringTriggerType"] == null ? null : data["createGatheringTriggerType"].ToString())
                .WithCreateGatheringTriggerRealtimeNamespaceId(!data.Keys.Contains("createGatheringTriggerRealtimeNamespaceId") || data["createGatheringTriggerRealtimeNamespaceId"] == null ? null : data["createGatheringTriggerRealtimeNamespaceId"].ToString())
                .WithCreateGatheringTriggerScriptId(!data.Keys.Contains("createGatheringTriggerScriptId") || data["createGatheringTriggerScriptId"] == null ? null : data["createGatheringTriggerScriptId"].ToString())
                .WithCompleteMatchmakingTriggerType(!data.Keys.Contains("completeMatchmakingTriggerType") || data["completeMatchmakingTriggerType"] == null ? null : data["completeMatchmakingTriggerType"].ToString())
                .WithCompleteMatchmakingTriggerRealtimeNamespaceId(!data.Keys.Contains("completeMatchmakingTriggerRealtimeNamespaceId") || data["completeMatchmakingTriggerRealtimeNamespaceId"] == null ? null : data["completeMatchmakingTriggerRealtimeNamespaceId"].ToString())
                .WithCompleteMatchmakingTriggerScriptId(!data.Keys.Contains("completeMatchmakingTriggerScriptId") || data["completeMatchmakingTriggerScriptId"] == null ? null : data["completeMatchmakingTriggerScriptId"].ToString())
                .WithChangeRatingScript(!data.Keys.Contains("changeRatingScript") || data["changeRatingScript"] == null ? null : Gs2.Gs2Matchmaking.Model.ScriptSetting.FromJson(data["changeRatingScript"]))
                .WithJoinNotification(!data.Keys.Contains("joinNotification") || data["joinNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["joinNotification"]))
                .WithLeaveNotification(!data.Keys.Contains("leaveNotification") || data["leaveNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["leaveNotification"]))
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithChangeRatingNotification(!data.Keys.Contains("changeRatingNotification") || data["changeRatingNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["changeRatingNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Matchmaking.Model.LogSetting.FromJson(data["logSetting"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)long.Parse(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["enableRating"] = EnableRating,
                ["createGatheringTriggerType"] = CreateGatheringTriggerType,
                ["createGatheringTriggerRealtimeNamespaceId"] = CreateGatheringTriggerRealtimeNamespaceId,
                ["createGatheringTriggerScriptId"] = CreateGatheringTriggerScriptId,
                ["completeMatchmakingTriggerType"] = CompleteMatchmakingTriggerType,
                ["completeMatchmakingTriggerRealtimeNamespaceId"] = CompleteMatchmakingTriggerRealtimeNamespaceId,
                ["completeMatchmakingTriggerScriptId"] = CompleteMatchmakingTriggerScriptId,
                ["changeRatingScript"] = ChangeRatingScript?.ToJson(),
                ["joinNotification"] = JoinNotification?.ToJson(),
                ["leaveNotification"] = LeaveNotification?.ToJson(),
                ["completeNotification"] = CompleteNotification?.ToJson(),
                ["changeRatingNotification"] = ChangeRatingNotification?.ToJson(),
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
            if (EnableRating != null) {
                writer.WritePropertyName("enableRating");
                writer.Write(bool.Parse(EnableRating.ToString()));
            }
            if (CreateGatheringTriggerType != null) {
                writer.WritePropertyName("createGatheringTriggerType");
                writer.Write(CreateGatheringTriggerType.ToString());
            }
            if (CreateGatheringTriggerRealtimeNamespaceId != null) {
                writer.WritePropertyName("createGatheringTriggerRealtimeNamespaceId");
                writer.Write(CreateGatheringTriggerRealtimeNamespaceId.ToString());
            }
            if (CreateGatheringTriggerScriptId != null) {
                writer.WritePropertyName("createGatheringTriggerScriptId");
                writer.Write(CreateGatheringTriggerScriptId.ToString());
            }
            if (CompleteMatchmakingTriggerType != null) {
                writer.WritePropertyName("completeMatchmakingTriggerType");
                writer.Write(CompleteMatchmakingTriggerType.ToString());
            }
            if (CompleteMatchmakingTriggerRealtimeNamespaceId != null) {
                writer.WritePropertyName("completeMatchmakingTriggerRealtimeNamespaceId");
                writer.Write(CompleteMatchmakingTriggerRealtimeNamespaceId.ToString());
            }
            if (CompleteMatchmakingTriggerScriptId != null) {
                writer.WritePropertyName("completeMatchmakingTriggerScriptId");
                writer.Write(CompleteMatchmakingTriggerScriptId.ToString());
            }
            if (ChangeRatingScript != null) {
                writer.WritePropertyName("changeRatingScript");
                ChangeRatingScript.WriteJson(writer);
            }
            if (JoinNotification != null) {
                writer.WritePropertyName("joinNotification");
                JoinNotification.WriteJson(writer);
            }
            if (LeaveNotification != null) {
                writer.WritePropertyName("leaveNotification");
                LeaveNotification.WriteJson(writer);
            }
            if (CompleteNotification != null) {
                writer.WritePropertyName("completeNotification");
                CompleteNotification.WriteJson(writer);
            }
            if (ChangeRatingNotification != null) {
                writer.WritePropertyName("changeRatingNotification");
                ChangeRatingNotification.WriteJson(writer);
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
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write(long.Parse(Revision.ToString()));
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
            if (EnableRating == null && EnableRating == other.EnableRating)
            {
                // null and null
            }
            else
            {
                diff += EnableRating == other.EnableRating ? 0 : 1;
            }
            if (CreateGatheringTriggerType == null && CreateGatheringTriggerType == other.CreateGatheringTriggerType)
            {
                // null and null
            }
            else
            {
                diff += CreateGatheringTriggerType.CompareTo(other.CreateGatheringTriggerType);
            }
            if (CreateGatheringTriggerRealtimeNamespaceId == null && CreateGatheringTriggerRealtimeNamespaceId == other.CreateGatheringTriggerRealtimeNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += CreateGatheringTriggerRealtimeNamespaceId.CompareTo(other.CreateGatheringTriggerRealtimeNamespaceId);
            }
            if (CreateGatheringTriggerScriptId == null && CreateGatheringTriggerScriptId == other.CreateGatheringTriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += CreateGatheringTriggerScriptId.CompareTo(other.CreateGatheringTriggerScriptId);
            }
            if (CompleteMatchmakingTriggerType == null && CompleteMatchmakingTriggerType == other.CompleteMatchmakingTriggerType)
            {
                // null and null
            }
            else
            {
                diff += CompleteMatchmakingTriggerType.CompareTo(other.CompleteMatchmakingTriggerType);
            }
            if (CompleteMatchmakingTriggerRealtimeNamespaceId == null && CompleteMatchmakingTriggerRealtimeNamespaceId == other.CompleteMatchmakingTriggerRealtimeNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += CompleteMatchmakingTriggerRealtimeNamespaceId.CompareTo(other.CompleteMatchmakingTriggerRealtimeNamespaceId);
            }
            if (CompleteMatchmakingTriggerScriptId == null && CompleteMatchmakingTriggerScriptId == other.CompleteMatchmakingTriggerScriptId)
            {
                // null and null
            }
            else
            {
                diff += CompleteMatchmakingTriggerScriptId.CompareTo(other.CompleteMatchmakingTriggerScriptId);
            }
            if (ChangeRatingScript == null && ChangeRatingScript == other.ChangeRatingScript)
            {
                // null and null
            }
            else
            {
                diff += ChangeRatingScript.CompareTo(other.ChangeRatingScript);
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
            if (CompleteNotification == null && CompleteNotification == other.CompleteNotification)
            {
                // null and null
            }
            else
            {
                diff += CompleteNotification.CompareTo(other.CompleteNotification);
            }
            if (ChangeRatingNotification == null && ChangeRatingNotification == other.ChangeRatingNotification)
            {
                // null and null
            }
            else
            {
                diff += ChangeRatingNotification.CompareTo(other.ChangeRatingNotification);
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
    }
}