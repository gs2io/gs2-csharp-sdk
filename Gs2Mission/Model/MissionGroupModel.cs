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
	public class MissionGroupModel : IComparable
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
                .WithTasks(!data.Keys.Contains("tasks") || data["tasks"] == null ? new Gs2.Gs2Mission.Model.MissionTaskModel[]{} : data["tasks"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.MissionTaskModel.FromJson(v);
                }).ToArray())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)int.Parse(data["resetDayOfMonth"].ToString()))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)int.Parse(data["resetHour"].ToString()))
                .WithCompleteNotificationNamespaceId(!data.Keys.Contains("completeNotificationNamespaceId") || data["completeNotificationNamespaceId"] == null ? null : data["completeNotificationNamespaceId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["missionGroupId"] = MissionGroupId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["tasks"] = new JsonData(Tasks == null ? new JsonData[]{} :
                        Tasks.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["resetType"] = ResetType,
                ["resetDayOfMonth"] = ResetDayOfMonth,
                ["resetDayOfWeek"] = ResetDayOfWeek,
                ["resetHour"] = ResetHour,
                ["completeNotificationNamespaceId"] = CompleteNotificationNamespaceId,
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
            return diff;
        }
    }
}