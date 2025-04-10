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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class MissionGroupModel : IComparable
	{
        public string MissionGroupId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Mission.Model.MissionTaskModel[] Tasks { set; get; }
        public string ResetType { set; get; }
        public int? ResetDayOfMonth { set; get; }
        public string ResetDayOfWeek { set; get; }
        public int? ResetHour { set; get; }
        public string CompleteNotificationNamespaceId { set; get; }
        public long? AnchorTimestamp { set; get; }
        public int? Days { set; get; }
        public MissionGroupModel WithMissionGroupId(string missionGroupId) {
            this.MissionGroupId = missionGroupId;
            return this;
        }
        public MissionGroupModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public MissionGroupModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public MissionGroupModel WithTasks(Gs2.Gs2Mission.Model.MissionTaskModel[] tasks) {
            this.Tasks = tasks;
            return this;
        }
        public MissionGroupModel WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public MissionGroupModel WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }
        public MissionGroupModel WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }
        public MissionGroupModel WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public MissionGroupModel WithCompleteNotificationNamespaceId(string completeNotificationNamespaceId) {
            this.CompleteNotificationNamespaceId = completeNotificationNamespaceId;
            return this;
        }
        public MissionGroupModel WithAnchorTimestamp(long? anchorTimestamp) {
            this.AnchorTimestamp = anchorTimestamp;
            return this;
        }
        public MissionGroupModel WithDays(int? days) {
            this.Days = days;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+)",
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

        private static System.Text.RegularExpressions.Regex _missionGroupNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):group:(?<missionGroupName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMissionGroupNameFromGrn(
            string grn
        )
        {
            var match = _missionGroupNameRegex.Match(grn);
            if (!match.Success || !match.Groups["missionGroupName"].Success)
            {
                return null;
            }
            return match.Groups["missionGroupName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MissionGroupModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MissionGroupModel()
                .WithMissionGroupId(!data.Keys.Contains("missionGroupId") || data["missionGroupId"] == null ? null : data["missionGroupId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTasks(!data.Keys.Contains("tasks") || data["tasks"] == null || !data["tasks"].IsArray ? null : data["tasks"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.MissionTaskModel.FromJson(v);
                }).ToArray())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)(data["resetDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["resetDayOfMonth"].ToString()) : int.Parse(data["resetDayOfMonth"].ToString())))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)(data["resetHour"].ToString().Contains(".") ? (int)double.Parse(data["resetHour"].ToString()) : int.Parse(data["resetHour"].ToString())))
                .WithCompleteNotificationNamespaceId(!data.Keys.Contains("completeNotificationNamespaceId") || data["completeNotificationNamespaceId"] == null ? null : data["completeNotificationNamespaceId"].ToString())
                .WithAnchorTimestamp(!data.Keys.Contains("anchorTimestamp") || data["anchorTimestamp"] == null ? null : (long?)(data["anchorTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["anchorTimestamp"].ToString()) : long.Parse(data["anchorTimestamp"].ToString())))
                .WithDays(!data.Keys.Contains("days") || data["days"] == null ? null : (int?)(data["days"].ToString().Contains(".") ? (int)double.Parse(data["days"].ToString()) : int.Parse(data["days"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData tasksJsonData = null;
            if (Tasks != null && Tasks.Length > 0)
            {
                tasksJsonData = new JsonData();
                foreach (var task in Tasks)
                {
                    tasksJsonData.Add(task.ToJson());
                }
            }
            return new JsonData {
                ["missionGroupId"] = MissionGroupId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["tasks"] = tasksJsonData,
                ["resetType"] = ResetType,
                ["resetDayOfMonth"] = ResetDayOfMonth,
                ["resetDayOfWeek"] = ResetDayOfWeek,
                ["resetHour"] = ResetHour,
                ["completeNotificationNamespaceId"] = CompleteNotificationNamespaceId,
                ["anchorTimestamp"] = AnchorTimestamp,
                ["days"] = Days,
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
            if (Tasks != null) {
                writer.WritePropertyName("tasks");
                writer.WriteArrayStart();
                foreach (var task in Tasks)
                {
                    if (task != null) {
                        task.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ResetType != null) {
                writer.WritePropertyName("resetType");
                writer.Write(ResetType.ToString());
            }
            if (ResetDayOfMonth != null) {
                writer.WritePropertyName("resetDayOfMonth");
                writer.Write((ResetDayOfMonth.ToString().Contains(".") ? (int)double.Parse(ResetDayOfMonth.ToString()) : int.Parse(ResetDayOfMonth.ToString())));
            }
            if (ResetDayOfWeek != null) {
                writer.WritePropertyName("resetDayOfWeek");
                writer.Write(ResetDayOfWeek.ToString());
            }
            if (ResetHour != null) {
                writer.WritePropertyName("resetHour");
                writer.Write((ResetHour.ToString().Contains(".") ? (int)double.Parse(ResetHour.ToString()) : int.Parse(ResetHour.ToString())));
            }
            if (CompleteNotificationNamespaceId != null) {
                writer.WritePropertyName("completeNotificationNamespaceId");
                writer.Write(CompleteNotificationNamespaceId.ToString());
            }
            if (AnchorTimestamp != null) {
                writer.WritePropertyName("anchorTimestamp");
                writer.Write((AnchorTimestamp.ToString().Contains(".") ? (long)double.Parse(AnchorTimestamp.ToString()) : long.Parse(AnchorTimestamp.ToString())));
            }
            if (Days != null) {
                writer.WritePropertyName("days");
                writer.Write((Days.ToString().Contains(".") ? (int)double.Parse(Days.ToString()) : int.Parse(Days.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as MissionGroupModel;
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
            if (Tasks == null && Tasks == other.Tasks)
            {
                // null and null
            }
            else
            {
                diff += Tasks.Length - other.Tasks.Length;
                for (var i = 0; i < Tasks.Length; i++)
                {
                    diff += Tasks[i].CompareTo(other.Tasks[i]);
                }
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
            if (AnchorTimestamp == null && AnchorTimestamp == other.AnchorTimestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(AnchorTimestamp - other.AnchorTimestamp);
            }
            if (Days == null && Days == other.Days)
            {
                // null and null
            }
            else
            {
                diff += (int)(Days - other.Days);
            }
            return diff;
        }

        public void Validate() {
            {
                if (MissionGroupId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.missionGroupId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Tasks.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.tasks.error.tooMany"),
                    });
                }
            }
            {
                switch (ResetType) {
                    case "notReset":
                    case "daily":
                    case "weekly":
                    case "monthly":
                    case "days":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("missionGroupModel", "mission.missionGroupModel.resetType.error.invalid"),
                        });
                }
            }
            if (ResetType == "monthly") {
                if (ResetDayOfMonth < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.resetDayOfMonth.error.invalid"),
                    });
                }
                if (ResetDayOfMonth > 31) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.resetDayOfMonth.error.invalid"),
                    });
                }
            }
            if (ResetType == "weekly") {
                switch (ResetDayOfWeek) {
                    case "sunday":
                    case "monday":
                    case "tuesday":
                    case "wednesday":
                    case "thursday":
                    case "friday":
                    case "saturday":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("missionGroupModel", "mission.missionGroupModel.resetDayOfWeek.error.invalid"),
                        });
                }
            }
            if ((ResetType =="monthly" || ResetType == "weekly" || ResetType == "daily")) {
                if (ResetHour < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.resetHour.error.invalid"),
                    });
                }
                if (ResetHour > 23) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.resetHour.error.invalid"),
                    });
                }
            }
            {
                if (CompleteNotificationNamespaceId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.completeNotificationNamespaceId.error.tooLong"),
                    });
                }
            }
            if (ResetType == "days") {
                if (AnchorTimestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.anchorTimestamp.error.invalid"),
                    });
                }
                if (AnchorTimestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.anchorTimestamp.error.invalid"),
                    });
                }
            }
            if (ResetType == "days") {
                if (Days < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.days.error.invalid"),
                    });
                }
                if (Days > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("missionGroupModel", "mission.missionGroupModel.days.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new MissionGroupModel {
                MissionGroupId = MissionGroupId,
                Name = Name,
                Metadata = Metadata,
                Tasks = Tasks?.Clone() as Gs2.Gs2Mission.Model.MissionTaskModel[],
                ResetType = ResetType,
                ResetDayOfMonth = ResetDayOfMonth,
                ResetDayOfWeek = ResetDayOfWeek,
                ResetHour = ResetHour,
                CompleteNotificationNamespaceId = CompleteNotificationNamespaceId,
                AnchorTimestamp = AnchorTimestamp,
                Days = Days,
            };
        }
    }
}