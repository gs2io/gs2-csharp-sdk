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
	public class MissionTaskModelMaster : IComparable
	{
        public string MissionTaskId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Description { set; get; }
        public string CounterName { set; get; }
        public long? TargetValue { set; get; }
        public Gs2.Gs2Mission.Model.AcquireAction[] CompleteAcquireActions { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public string PremiseMissionTaskName { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public MissionTaskModelMaster WithMissionTaskId(string missionTaskId) {
            this.MissionTaskId = missionTaskId;
            return this;
        }

        public MissionTaskModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public MissionTaskModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public MissionTaskModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public MissionTaskModelMaster WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }

        public MissionTaskModelMaster WithTargetValue(long? targetValue) {
            this.TargetValue = targetValue;
            return this;
        }

        public MissionTaskModelMaster WithCompleteAcquireActions(Gs2.Gs2Mission.Model.AcquireAction[] completeAcquireActions) {
            this.CompleteAcquireActions = completeAcquireActions;
            return this;
        }

        public MissionTaskModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

        public MissionTaskModelMaster WithPremiseMissionTaskName(string premiseMissionTaskName) {
            this.PremiseMissionTaskName = premiseMissionTaskName;
            return this;
        }

        public MissionTaskModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public MissionTaskModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MissionTaskModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MissionTaskModelMaster()
                .WithMissionTaskId(!data.Keys.Contains("missionTaskId") || data["missionTaskId"] == null ? null : data["missionTaskId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithCounterName(!data.Keys.Contains("counterName") || data["counterName"] == null ? null : data["counterName"].ToString())
                .WithTargetValue(!data.Keys.Contains("targetValue") || data["targetValue"] == null ? null : (long?)long.Parse(data["targetValue"].ToString()))
                .WithCompleteAcquireActions(!data.Keys.Contains("completeAcquireActions") || data["completeAcquireActions"] == null ? new Gs2.Gs2Mission.Model.AcquireAction[]{} : data["completeAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithPremiseMissionTaskName(!data.Keys.Contains("premiseMissionTaskName") || data["premiseMissionTaskName"] == null ? null : data["premiseMissionTaskName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["missionTaskId"] = MissionTaskId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["counterName"] = CounterName,
                ["targetValue"] = TargetValue,
                ["completeAcquireActions"] = new JsonData(CompleteAcquireActions == null ? new JsonData[]{} :
                        CompleteAcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["premiseMissionTaskName"] = PremiseMissionTaskName,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MissionTaskId != null) {
                writer.WritePropertyName("missionTaskId");
                writer.Write(MissionTaskId.ToString());
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
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (TargetValue != null) {
                writer.WritePropertyName("targetValue");
                writer.Write(long.Parse(TargetValue.ToString()));
            }
            if (CompleteAcquireActions != null) {
                writer.WritePropertyName("completeAcquireActions");
                writer.WriteArrayStart();
                foreach (var completeAcquireAction in CompleteAcquireActions)
                {
                    if (completeAcquireAction != null) {
                        completeAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            if (PremiseMissionTaskName != null) {
                writer.WritePropertyName("premiseMissionTaskName");
                writer.Write(PremiseMissionTaskName.ToString());
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
            var other = obj as MissionTaskModelMaster;
            var diff = 0;
            if (MissionTaskId == null && MissionTaskId == other.MissionTaskId)
            {
                // null and null
            }
            else
            {
                diff += MissionTaskId.CompareTo(other.MissionTaskId);
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
            if (CounterName == null && CounterName == other.CounterName)
            {
                // null and null
            }
            else
            {
                diff += CounterName.CompareTo(other.CounterName);
            }
            if (TargetValue == null && TargetValue == other.TargetValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(TargetValue - other.TargetValue);
            }
            if (CompleteAcquireActions == null && CompleteAcquireActions == other.CompleteAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += CompleteAcquireActions.Length - other.CompleteAcquireActions.Length;
                for (var i = 0; i < CompleteAcquireActions.Length; i++)
                {
                    diff += CompleteAcquireActions[i].CompareTo(other.CompleteAcquireActions[i]);
                }
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
            }
            if (PremiseMissionTaskName == null && PremiseMissionTaskName == other.PremiseMissionTaskName)
            {
                // null and null
            }
            else
            {
                diff += PremiseMissionTaskName.CompareTo(other.PremiseMissionTaskName);
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