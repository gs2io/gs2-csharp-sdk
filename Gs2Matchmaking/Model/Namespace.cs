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
        public string NamespaceId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public bool? EnableRating { set; get; } = null!;
        public string EnableDisconnectDetection { set; get; } = null!;
        public int? DisconnectDetectionTimeoutSeconds { set; get; } = null!;
        public string CreateGatheringTriggerType { set; get; } = null!;
        public string CreateGatheringTriggerRealtimeNamespaceId { set; get; } = null!;
        public string CreateGatheringTriggerScriptId { set; get; } = null!;
        public string CompleteMatchmakingTriggerType { set; get; } = null!;
        public string CompleteMatchmakingTriggerRealtimeNamespaceId { set; get; } = null!;
        public string CompleteMatchmakingTriggerScriptId { set; get; } = null!;
        public string EnableCollaborateSeasonRating { set; get; } = null!;
        public string CollaborateSeasonRatingNamespaceId { set; get; } = null!;
        public int? CollaborateSeasonRatingTtl { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.ScriptSetting ChangeRatingScript { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.NotificationSetting JoinNotification { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.NotificationSetting LeaveNotification { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.NotificationSetting CompleteNotification { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.NotificationSetting ChangeRatingNotification { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.LogSetting LogSetting { set; get; } = null!;
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
        public Namespace WithEnableRating(bool? enableRating) {
            this.EnableRating = enableRating;
            return this;
        }
        public Namespace WithEnableDisconnectDetection(string enableDisconnectDetection) {
            this.EnableDisconnectDetection = enableDisconnectDetection;
            return this;
        }
        public Namespace WithDisconnectDetectionTimeoutSeconds(int? disconnectDetectionTimeoutSeconds) {
            this.DisconnectDetectionTimeoutSeconds = disconnectDetectionTimeoutSeconds;
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
        public Namespace WithEnableCollaborateSeasonRating(string enableCollaborateSeasonRating) {
            this.EnableCollaborateSeasonRating = enableCollaborateSeasonRating;
            return this;
        }
        public Namespace WithCollaborateSeasonRatingNamespaceId(string collaborateSeasonRatingNamespaceId) {
            this.CollaborateSeasonRatingNamespaceId = collaborateSeasonRatingNamespaceId;
            return this;
        }
        public Namespace WithCollaborateSeasonRatingTtl(int? collaborateSeasonRatingTtl) {
            this.CollaborateSeasonRatingTtl = collaborateSeasonRatingTtl;
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
                .WithEnableDisconnectDetection(!data.Keys.Contains("enableDisconnectDetection") || data["enableDisconnectDetection"] == null ? null : data["enableDisconnectDetection"].ToString())
                .WithDisconnectDetectionTimeoutSeconds(!data.Keys.Contains("disconnectDetectionTimeoutSeconds") || data["disconnectDetectionTimeoutSeconds"] == null ? null : (int?)(data["disconnectDetectionTimeoutSeconds"].ToString().Contains(".") ? (int)double.Parse(data["disconnectDetectionTimeoutSeconds"].ToString()) : int.Parse(data["disconnectDetectionTimeoutSeconds"].ToString())))
                .WithCreateGatheringTriggerType(!data.Keys.Contains("createGatheringTriggerType") || data["createGatheringTriggerType"] == null ? null : data["createGatheringTriggerType"].ToString())
                .WithCreateGatheringTriggerRealtimeNamespaceId(!data.Keys.Contains("createGatheringTriggerRealtimeNamespaceId") || data["createGatheringTriggerRealtimeNamespaceId"] == null ? null : data["createGatheringTriggerRealtimeNamespaceId"].ToString())
                .WithCreateGatheringTriggerScriptId(!data.Keys.Contains("createGatheringTriggerScriptId") || data["createGatheringTriggerScriptId"] == null ? null : data["createGatheringTriggerScriptId"].ToString())
                .WithCompleteMatchmakingTriggerType(!data.Keys.Contains("completeMatchmakingTriggerType") || data["completeMatchmakingTriggerType"] == null ? null : data["completeMatchmakingTriggerType"].ToString())
                .WithCompleteMatchmakingTriggerRealtimeNamespaceId(!data.Keys.Contains("completeMatchmakingTriggerRealtimeNamespaceId") || data["completeMatchmakingTriggerRealtimeNamespaceId"] == null ? null : data["completeMatchmakingTriggerRealtimeNamespaceId"].ToString())
                .WithCompleteMatchmakingTriggerScriptId(!data.Keys.Contains("completeMatchmakingTriggerScriptId") || data["completeMatchmakingTriggerScriptId"] == null ? null : data["completeMatchmakingTriggerScriptId"].ToString())
                .WithEnableCollaborateSeasonRating(!data.Keys.Contains("enableCollaborateSeasonRating") || data["enableCollaborateSeasonRating"] == null ? null : data["enableCollaborateSeasonRating"].ToString())
                .WithCollaborateSeasonRatingNamespaceId(!data.Keys.Contains("collaborateSeasonRatingNamespaceId") || data["collaborateSeasonRatingNamespaceId"] == null ? null : data["collaborateSeasonRatingNamespaceId"].ToString())
                .WithCollaborateSeasonRatingTtl(!data.Keys.Contains("collaborateSeasonRatingTtl") || data["collaborateSeasonRatingTtl"] == null ? null : (int?)(data["collaborateSeasonRatingTtl"].ToString().Contains(".") ? (int)double.Parse(data["collaborateSeasonRatingTtl"].ToString()) : int.Parse(data["collaborateSeasonRatingTtl"].ToString())))
                .WithChangeRatingScript(!data.Keys.Contains("changeRatingScript") || data["changeRatingScript"] == null ? null : Gs2.Gs2Matchmaking.Model.ScriptSetting.FromJson(data["changeRatingScript"]))
                .WithJoinNotification(!data.Keys.Contains("joinNotification") || data["joinNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["joinNotification"]))
                .WithLeaveNotification(!data.Keys.Contains("leaveNotification") || data["leaveNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["leaveNotification"]))
                .WithCompleteNotification(!data.Keys.Contains("completeNotification") || data["completeNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["completeNotification"]))
                .WithChangeRatingNotification(!data.Keys.Contains("changeRatingNotification") || data["changeRatingNotification"] == null ? null : Gs2.Gs2Matchmaking.Model.NotificationSetting.FromJson(data["changeRatingNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2Matchmaking.Model.LogSetting.FromJson(data["logSetting"]))
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
                ["enableRating"] = EnableRating,
                ["enableDisconnectDetection"] = EnableDisconnectDetection,
                ["disconnectDetectionTimeoutSeconds"] = DisconnectDetectionTimeoutSeconds,
                ["createGatheringTriggerType"] = CreateGatheringTriggerType,
                ["createGatheringTriggerRealtimeNamespaceId"] = CreateGatheringTriggerRealtimeNamespaceId,
                ["createGatheringTriggerScriptId"] = CreateGatheringTriggerScriptId,
                ["completeMatchmakingTriggerType"] = CompleteMatchmakingTriggerType,
                ["completeMatchmakingTriggerRealtimeNamespaceId"] = CompleteMatchmakingTriggerRealtimeNamespaceId,
                ["completeMatchmakingTriggerScriptId"] = CompleteMatchmakingTriggerScriptId,
                ["enableCollaborateSeasonRating"] = EnableCollaborateSeasonRating,
                ["collaborateSeasonRatingNamespaceId"] = CollaborateSeasonRatingNamespaceId,
                ["collaborateSeasonRatingTtl"] = CollaborateSeasonRatingTtl,
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
            if (EnableDisconnectDetection != null) {
                writer.WritePropertyName("enableDisconnectDetection");
                writer.Write(EnableDisconnectDetection.ToString());
            }
            if (DisconnectDetectionTimeoutSeconds != null) {
                writer.WritePropertyName("disconnectDetectionTimeoutSeconds");
                writer.Write((DisconnectDetectionTimeoutSeconds.ToString().Contains(".") ? (int)double.Parse(DisconnectDetectionTimeoutSeconds.ToString()) : int.Parse(DisconnectDetectionTimeoutSeconds.ToString())));
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
            if (EnableCollaborateSeasonRating != null) {
                writer.WritePropertyName("enableCollaborateSeasonRating");
                writer.Write(EnableCollaborateSeasonRating.ToString());
            }
            if (CollaborateSeasonRatingNamespaceId != null) {
                writer.WritePropertyName("collaborateSeasonRatingNamespaceId");
                writer.Write(CollaborateSeasonRatingNamespaceId.ToString());
            }
            if (CollaborateSeasonRatingTtl != null) {
                writer.WritePropertyName("collaborateSeasonRatingTtl");
                writer.Write((CollaborateSeasonRatingTtl.ToString().Contains(".") ? (int)double.Parse(CollaborateSeasonRatingTtl.ToString()) : int.Parse(CollaborateSeasonRatingTtl.ToString())));
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
            if (EnableRating == null && EnableRating == other.EnableRating)
            {
                // null and null
            }
            else
            {
                diff += EnableRating == other.EnableRating ? 0 : 1;
            }
            if (EnableDisconnectDetection == null && EnableDisconnectDetection == other.EnableDisconnectDetection)
            {
                // null and null
            }
            else
            {
                diff += EnableDisconnectDetection.CompareTo(other.EnableDisconnectDetection);
            }
            if (DisconnectDetectionTimeoutSeconds == null && DisconnectDetectionTimeoutSeconds == other.DisconnectDetectionTimeoutSeconds)
            {
                // null and null
            }
            else
            {
                diff += (int)(DisconnectDetectionTimeoutSeconds - other.DisconnectDetectionTimeoutSeconds);
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
            if (EnableCollaborateSeasonRating == null && EnableCollaborateSeasonRating == other.EnableCollaborateSeasonRating)
            {
                // null and null
            }
            else
            {
                diff += EnableCollaborateSeasonRating.CompareTo(other.EnableCollaborateSeasonRating);
            }
            if (CollaborateSeasonRatingNamespaceId == null && CollaborateSeasonRatingNamespaceId == other.CollaborateSeasonRatingNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += CollaborateSeasonRatingNamespaceId.CompareTo(other.CollaborateSeasonRatingNamespaceId);
            }
            if (CollaborateSeasonRatingTtl == null && CollaborateSeasonRatingTtl == other.CollaborateSeasonRatingTtl)
            {
                // null and null
            }
            else
            {
                diff += (int)(CollaborateSeasonRatingTtl - other.CollaborateSeasonRatingTtl);
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

        public void Validate() {
            {
                if (NamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.namespaceId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.description.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                switch (EnableDisconnectDetection) {
                    case "disable":
                    case "enable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("namespace", "matchmaking.namespace.enableDisconnectDetection.error.invalid"),
                        });
                }
            }
            if (EnableDisconnectDetection == "enable") {
                if (DisconnectDetectionTimeoutSeconds < 15) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.disconnectDetectionTimeoutSeconds.error.invalid"),
                    });
                }
                if (DisconnectDetectionTimeoutSeconds > 600) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.disconnectDetectionTimeoutSeconds.error.invalid"),
                    });
                }
            }
            {
                switch (CreateGatheringTriggerType) {
                    case "none":
                    case "gs2_realtime":
                    case "gs2_script":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("namespace", "matchmaking.namespace.createGatheringTriggerType.error.invalid"),
                        });
                }
            }
            if (CreateGatheringTriggerType == "gs2_realtime") {
                if (CreateGatheringTriggerRealtimeNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.createGatheringTriggerRealtimeNamespaceId.error.tooLong"),
                    });
                }
            }
            if (CreateGatheringTriggerType == "gs2_script") {
                if (CreateGatheringTriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.createGatheringTriggerScriptId.error.tooLong"),
                    });
                }
            }
            {
                switch (CompleteMatchmakingTriggerType) {
                    case "none":
                    case "gs2_realtime":
                    case "gs2_script":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("namespace", "matchmaking.namespace.completeMatchmakingTriggerType.error.invalid"),
                        });
                }
            }
            if (CompleteMatchmakingTriggerType == "gs2_realtime") {
                if (CompleteMatchmakingTriggerRealtimeNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.completeMatchmakingTriggerRealtimeNamespaceId.error.tooLong"),
                    });
                }
            }
            if (CompleteMatchmakingTriggerType == "gs2_script") {
                if (CompleteMatchmakingTriggerScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.completeMatchmakingTriggerScriptId.error.tooLong"),
                    });
                }
            }
            {
                switch (EnableCollaborateSeasonRating) {
                    case "enable":
                    case "disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("namespace", "matchmaking.namespace.enableCollaborateSeasonRating.error.invalid"),
                        });
                }
            }
            if (EnableCollaborateSeasonRating == "enable") {
                if (CollaborateSeasonRatingNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.collaborateSeasonRatingNamespaceId.error.tooLong"),
                    });
                }
            }
            if (EnableCollaborateSeasonRating == "enable") {
                if (CollaborateSeasonRatingTtl < 60) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.collaborateSeasonRatingTtl.error.invalid"),
                    });
                }
                if (CollaborateSeasonRatingTtl > 7200) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.collaborateSeasonRatingTtl.error.invalid"),
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
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("namespace", "matchmaking.namespace.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Namespace {
                NamespaceId = NamespaceId,
                Name = Name,
                Description = Description,
                EnableRating = EnableRating,
                EnableDisconnectDetection = EnableDisconnectDetection,
                DisconnectDetectionTimeoutSeconds = DisconnectDetectionTimeoutSeconds,
                CreateGatheringTriggerType = CreateGatheringTriggerType,
                CreateGatheringTriggerRealtimeNamespaceId = CreateGatheringTriggerRealtimeNamespaceId,
                CreateGatheringTriggerScriptId = CreateGatheringTriggerScriptId,
                CompleteMatchmakingTriggerType = CompleteMatchmakingTriggerType,
                CompleteMatchmakingTriggerRealtimeNamespaceId = CompleteMatchmakingTriggerRealtimeNamespaceId,
                CompleteMatchmakingTriggerScriptId = CompleteMatchmakingTriggerScriptId,
                EnableCollaborateSeasonRating = EnableCollaborateSeasonRating,
                CollaborateSeasonRatingNamespaceId = CollaborateSeasonRatingNamespaceId,
                CollaborateSeasonRatingTtl = CollaborateSeasonRatingTtl,
                ChangeRatingScript = ChangeRatingScript.Clone() as Gs2.Gs2Matchmaking.Model.ScriptSetting,
                JoinNotification = JoinNotification.Clone() as Gs2.Gs2Matchmaking.Model.NotificationSetting,
                LeaveNotification = LeaveNotification.Clone() as Gs2.Gs2Matchmaking.Model.NotificationSetting,
                CompleteNotification = CompleteNotification.Clone() as Gs2.Gs2Matchmaking.Model.NotificationSetting,
                ChangeRatingNotification = ChangeRatingNotification.Clone() as Gs2.Gs2Matchmaking.Model.NotificationSetting,
                LogSetting = LogSetting.Clone() as Gs2.Gs2Matchmaking.Model.LogSetting,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}