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
	public class VerifyCompleteByUserIdRequest : Gs2Request<VerifyCompleteByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string MissionGroupName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string VerifyType { set; get; } = null!;
         public string MissionTaskName { set; get; } = null!;
         public bool? MultiplyValueSpecifyingQuantity { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public VerifyCompleteByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithVerifyType(string verifyType) {
            this.VerifyType = verifyType;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithMissionTaskName(string missionTaskName) {
            this.MissionTaskName = missionTaskName;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithMultiplyValueSpecifyingQuantity(bool? multiplyValueSpecifyingQuantity) {
            this.MultiplyValueSpecifyingQuantity = multiplyValueSpecifyingQuantity;
            return this;
        }
        public VerifyCompleteByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public VerifyCompleteByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static VerifyCompleteByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new VerifyCompleteByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithVerifyType(!data.Keys.Contains("verifyType") || data["verifyType"] == null ? null : data["verifyType"].ToString())
                .WithMissionTaskName(!data.Keys.Contains("missionTaskName") || data["missionTaskName"] == null ? null : data["missionTaskName"].ToString())
                .WithMultiplyValueSpecifyingQuantity(!data.Keys.Contains("multiplyValueSpecifyingQuantity") || data["multiplyValueSpecifyingQuantity"] == null ? null : (bool?)bool.Parse(data["multiplyValueSpecifyingQuantity"].ToString()))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["missionGroupName"] = MissionGroupName,
                ["userId"] = UserId,
                ["verifyType"] = VerifyType,
                ["missionTaskName"] = MissionTaskName,
                ["multiplyValueSpecifyingQuantity"] = MultiplyValueSpecifyingQuantity,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (VerifyType != null) {
                writer.WritePropertyName("verifyType");
                writer.Write(VerifyType.ToString());
            }
            if (MissionTaskName != null) {
                writer.WritePropertyName("missionTaskName");
                writer.Write(MissionTaskName.ToString());
            }
            if (MultiplyValueSpecifyingQuantity != null) {
                writer.WritePropertyName("multiplyValueSpecifyingQuantity");
                writer.Write(bool.Parse(MultiplyValueSpecifyingQuantity.ToString()));
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += MissionGroupName + ":";
            key += UserId + ":";
            key += VerifyType + ":";
            key += MissionTaskName + ":";
            key += MultiplyValueSpecifyingQuantity + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}