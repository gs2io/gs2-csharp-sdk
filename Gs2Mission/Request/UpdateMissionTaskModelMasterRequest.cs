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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Mission.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Mission.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateMissionTaskModelMasterRequest : Gs2Request<UpdateMissionTaskModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string MissionGroupName { set; get; } = null!;
         public string MissionTaskName { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string VerifyCompleteType { set; get; } = null!;
         public Gs2.Gs2Mission.Model.TargetCounterModel TargetCounter { set; get; } = null!;
         public Gs2.Core.Model.VerifyAction[] VerifyCompleteConsumeActions { set; get; } = null!;
         public Gs2.Core.Model.AcquireAction[] CompleteAcquireActions { set; get; } = null!;
         public string ChallengePeriodEventId { set; get; } = null!;
         public string PremiseMissionTaskName { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string CounterName { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public string TargetResetType { set; get; } = null!;
        [Obsolete("This method is deprecated")]
         public long? TargetValue { set; get; } = null!;
        public UpdateMissionTaskModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithMissionTaskName(string missionTaskName) {
            this.MissionTaskName = missionTaskName;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithVerifyCompleteType(string verifyCompleteType) {
            this.VerifyCompleteType = verifyCompleteType;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithTargetCounter(Gs2.Gs2Mission.Model.TargetCounterModel targetCounter) {
            this.TargetCounter = targetCounter;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithVerifyCompleteConsumeActions(Gs2.Core.Model.VerifyAction[] verifyCompleteConsumeActions) {
            this.VerifyCompleteConsumeActions = verifyCompleteConsumeActions;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithCompleteAcquireActions(Gs2.Core.Model.AcquireAction[] completeAcquireActions) {
            this.CompleteAcquireActions = completeAcquireActions;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public UpdateMissionTaskModelMasterRequest WithPremiseMissionTaskName(string premiseMissionTaskName) {
            this.PremiseMissionTaskName = premiseMissionTaskName;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateMissionTaskModelMasterRequest WithCounterName(string counterName) {
            this.CounterName = counterName;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateMissionTaskModelMasterRequest WithTargetResetType(string targetResetType) {
            this.TargetResetType = targetResetType;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public UpdateMissionTaskModelMasterRequest WithTargetValue(long? targetValue) {
            this.TargetValue = targetValue;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateMissionTaskModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateMissionTaskModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithMissionTaskName(!data.Keys.Contains("missionTaskName") || data["missionTaskName"] == null ? null : data["missionTaskName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithVerifyCompleteType(!data.Keys.Contains("verifyCompleteType") || data["verifyCompleteType"] == null ? null : data["verifyCompleteType"].ToString())
                .WithTargetCounter(!data.Keys.Contains("targetCounter") || data["targetCounter"] == null ? null : Gs2.Gs2Mission.Model.TargetCounterModel.FromJson(data["targetCounter"]))
                .WithVerifyCompleteConsumeActions(!data.Keys.Contains("verifyCompleteConsumeActions") || data["verifyCompleteConsumeActions"] == null || !data["verifyCompleteConsumeActions"].IsArray ? null : data["verifyCompleteConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithCompleteAcquireActions(!data.Keys.Contains("completeAcquireActions") || data["completeAcquireActions"] == null || !data["completeAcquireActions"].IsArray ? null : data["completeAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithPremiseMissionTaskName(!data.Keys.Contains("premiseMissionTaskName") || data["premiseMissionTaskName"] == null ? null : data["premiseMissionTaskName"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData verifyCompleteConsumeActionsJsonData = null;
            if (VerifyCompleteConsumeActions != null && VerifyCompleteConsumeActions.Length > 0)
            {
                verifyCompleteConsumeActionsJsonData = new JsonData();
                foreach (var verifyCompleteConsumeAction in VerifyCompleteConsumeActions)
                {
                    verifyCompleteConsumeActionsJsonData.Add(verifyCompleteConsumeAction.ToJson());
                }
            }
            JsonData completeAcquireActionsJsonData = null;
            if (CompleteAcquireActions != null && CompleteAcquireActions.Length > 0)
            {
                completeAcquireActionsJsonData = new JsonData();
                foreach (var completeAcquireAction in CompleteAcquireActions)
                {
                    completeAcquireActionsJsonData.Add(completeAcquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["missionGroupName"] = MissionGroupName,
                ["missionTaskName"] = MissionTaskName,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["verifyCompleteType"] = VerifyCompleteType,
                ["targetCounter"] = TargetCounter?.ToJson(),
                ["verifyCompleteConsumeActions"] = verifyCompleteConsumeActionsJsonData,
                ["completeAcquireActions"] = completeAcquireActionsJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["premiseMissionTaskName"] = PremiseMissionTaskName,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (MissionGroupName != null) {
                writer.WritePropertyName("missionGroupName");
                writer.Write(MissionGroupName.ToString());
            }
            if (MissionTaskName != null) {
                writer.WritePropertyName("missionTaskName");
                writer.Write(MissionTaskName.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (VerifyCompleteType != null) {
                writer.WritePropertyName("verifyCompleteType");
                writer.Write(VerifyCompleteType.ToString());
            }
            if (TargetCounter != null) {
                TargetCounter.WriteJson(writer);
            }
            if (VerifyCompleteConsumeActions != null) {
                writer.WritePropertyName("verifyCompleteConsumeActions");
                writer.WriteArrayStart();
                foreach (var verifyCompleteConsumeAction in VerifyCompleteConsumeActions)
                {
                    if (verifyCompleteConsumeAction != null) {
                        verifyCompleteConsumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            if (CounterName != null) {
                writer.WritePropertyName("counterName");
                writer.Write(CounterName.ToString());
            }
            if (TargetResetType != null) {
                writer.WritePropertyName("targetResetType");
                writer.Write(TargetResetType.ToString());
            }
            if (TargetValue != null) {
                writer.WritePropertyName("targetValue");
                writer.Write((TargetValue.ToString().Contains(".") ? (long)double.Parse(TargetValue.ToString()) : long.Parse(TargetValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += MissionGroupName + ":";
            key += MissionTaskName + ":";
            key += Metadata + ":";
            key += Description + ":";
            key += VerifyCompleteType + ":";
            key += TargetCounter + ":";
            key += VerifyCompleteConsumeActions + ":";
            key += CompleteAcquireActions + ":";
            key += ChallengePeriodEventId + ":";
            key += PremiseMissionTaskName + ":";
            return key;
        }
    }
}