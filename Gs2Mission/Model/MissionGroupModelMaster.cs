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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class MissionGroupModelMaster : IComparable
	{
        public string MissionGroupId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Description { set; get; }
        public string ResetType { set; get; }
        public int? ResetDayOfMonth { set; get; }
        public string ResetDayOfWeek { set; get; }
        public int? ResetHour { set; get; }
        public string CompleteNotificationNamespaceId { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public MissionGroupModelMaster WithMissionGroupId(string missionGroupId) {
            this.MissionGroupId = missionGroupId;
            return this;
        }

        public MissionGroupModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public MissionGroupModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public MissionGroupModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public MissionGroupModelMaster WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }

        public MissionGroupModelMaster WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }

        public MissionGroupModelMaster WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }

        public MissionGroupModelMaster WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }

        public MissionGroupModelMaster WithCompleteNotificationNamespaceId(string completeNotificationNamespaceId) {
            this.CompleteNotificationNamespaceId = completeNotificationNamespaceId;
            return this;
        }

        public MissionGroupModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public MissionGroupModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MissionGroupModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MissionGroupModelMaster()
                .WithMissionGroupId(!data.Keys.Contains("missionGroupId") || data["missionGroupId"] == null ? null : data["missionGroupId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)int.Parse(data["resetDayOfMonth"].ToString()))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)int.Parse(data["resetHour"].ToString()))
                .WithCompleteNotificationNamespaceId(!data.Keys.Contains("completeNotificationNamespaceId") || data["completeNotificationNamespaceId"] == null ? null : data["completeNotificationNamespaceId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["missionGroupId"] = MissionGroupId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["resetType"] = ResetType,
                ["resetDayOfMonth"] = ResetDayOfMonth,
                ["resetDayOfWeek"] = ResetDayOfWeek,
                ["resetHour"] = ResetHour,
                ["completeNotificationNamespaceId"] = CompleteNotificationNamespaceId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MissionGroupId != null) {
                writer.WritePropertyName("missionGroupId");
                writer.Write(MissionGroupId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (ResetType != null) {
                writer.WritePropertyName("resetType");
                writer.Write(ResetType.ToString());
            }
            if (ResetDayOfMonth != null) {
                writer.WritePropertyName("resetDayOfMonth");
                writer.Write(int.Parse(ResetDayOfMonth.ToString()));
            }
            if (ResetDayOfWeek != null) {
                writer.WritePropertyName("resetDayOfWeek");
                writer.Write(ResetDayOfWeek.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write(int.Parse(ResetHour.ToString()));
            }
            if (CompleteNotificationNamespaceId != null) {
                writer.WritePropertyName("completeNotificationNamespaceId");
                writer.Write(CompleteNotificationNamespaceId.ToString());
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
            var other = obj as MissionGroupModelMaster;
            var diff = 0;
            if (MissionGroupId == null && MissionGroupId == other.MissionGroupId)
            {
                // null and null
            }
            else
            {
                diff += MissionGroupId.CompareTo(other.MissionGroupId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (ResetType == null && ResetType == other.ResetType)
            {
                // null and null
            }
            else
            {
                diff += ResetType.CompareTo(other.ResetType);
            }
            if (ResetDayOfMonth == null && ResetDayOfMonth == other.ResetDayOfMonth)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetDayOfMonth - other.ResetDayOfMonth);
            }
            if (ResetDayOfWeek == null && ResetDayOfWeek == other.ResetDayOfWeek)
            {
                // null and null
            }
            else
            {
                diff += ResetDayOfWeek.CompareTo(other.ResetDayOfWeek);
            }
            if (ResetHour == null && ResetHour == other.ResetHour)
            {
                // null and null
            }
            else
            {
                diff += (int)(ResetHour - other.ResetHour);
            }
            if (CompleteNotificationNamespaceId == null && CompleteNotificationNamespaceId == other.CompleteNotificationNamespaceId)
            {
                // null and null
            }
            else
            {
                diff += CompleteNotificationNamespaceId.CompareTo(other.CompleteNotificationNamespaceId);
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