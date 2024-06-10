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
	public class UpdateMissionGroupModelMasterRequest : Gs2Request<UpdateMissionGroupModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string MissionGroupName { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string ResetType { set; get; } = null!;
         public int? ResetDayOfMonth { set; get; } = null!;
         public string ResetDayOfWeek { set; get; } = null!;
         public int? ResetHour { set; get; } = null!;
         public string CompleteNotificationNamespaceId { set; get; } = null!;
        public UpdateMissionGroupModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithResetType(string resetType) {
            this.ResetType = resetType;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithResetDayOfMonth(int? resetDayOfMonth) {
            this.ResetDayOfMonth = resetDayOfMonth;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithResetDayOfWeek(string resetDayOfWeek) {
            this.ResetDayOfWeek = resetDayOfWeek;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithResetHour(int? resetHour) {
            this.ResetHour = resetHour;
            return this;
        }
        public UpdateMissionGroupModelMasterRequest WithCompleteNotificationNamespaceId(string completeNotificationNamespaceId) {
            this.CompleteNotificationNamespaceId = completeNotificationNamespaceId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateMissionGroupModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateMissionGroupModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithResetType(!data.Keys.Contains("resetType") || data["resetType"] == null ? null : data["resetType"].ToString())
                .WithResetDayOfMonth(!data.Keys.Contains("resetDayOfMonth") || data["resetDayOfMonth"] == null ? null : (int?)(data["resetDayOfMonth"].ToString().Contains(".") ? (int)double.Parse(data["resetDayOfMonth"].ToString()) : int.Parse(data["resetDayOfMonth"].ToString())))
                .WithResetDayOfWeek(!data.Keys.Contains("resetDayOfWeek") || data["resetDayOfWeek"] == null ? null : data["resetDayOfWeek"].ToString())
                .WithResetHour(!data.Keys.Contains("resetHour") || data["resetHour"] == null ? null : (int?)(data["resetHour"].ToString().Contains(".") ? (int)double.Parse(data["resetHour"].ToString()) : int.Parse(data["resetHour"].ToString())))
                .WithCompleteNotificationNamespaceId(!data.Keys.Contains("completeNotificationNamespaceId") || data["completeNotificationNamespaceId"] == null ? null : data["completeNotificationNamespaceId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["missionGroupName"] = MissionGroupName,
                ["metadata"] = Metadata,
                ["description"] = Description,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (MissionGroupName != null) {
                writer.WritePropertyName("missionGroupName");
                writer.Write(MissionGroupName.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += MissionGroupName + ":";
            key += Metadata + ":";
            key += Description + ":";
            key += ResetType + ":";
            key += ResetDayOfMonth + ":";
            key += ResetDayOfWeek + ":";
            key += ResetHour + ":";
            key += CompleteNotificationNamespaceId + ":";
            return key;
        }
    }
}