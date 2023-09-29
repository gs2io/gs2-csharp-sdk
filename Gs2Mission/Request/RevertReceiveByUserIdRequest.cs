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
	public class RevertReceiveByUserIdRequest : Gs2Request<RevertReceiveByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string MissionGroupName { set; get; }
        public string MissionTaskName { set; get; }
        public string UserId { set; get; }
        public string DuplicationAvoider { set; get; }
        public RevertReceiveByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RevertReceiveByUserIdRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public RevertReceiveByUserIdRequest WithMissionTaskName(string missionTaskName) {
            this.MissionTaskName = missionTaskName;
            return this;
        }
        public RevertReceiveByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public RevertReceiveByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RevertReceiveByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RevertReceiveByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithMissionTaskName(!data.Keys.Contains("missionTaskName") || data["missionTaskName"] == null ? null : data["missionTaskName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["missionGroupName"] = MissionGroupName,
                ["missionTaskName"] = MissionTaskName,
                ["userId"] = UserId,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += MissionGroupName + ":";
            key += MissionTaskName + ":";
            key += UserId + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new RevertReceiveByUserIdRequest {
                NamespaceName = NamespaceName,
                MissionGroupName = MissionGroupName,
                MissionTaskName = MissionTaskName,
                UserId = UserId,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (RevertReceiveByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values RevertReceiveByUserIdRequest::namespaceName");
            }
            if (MissionGroupName != y.MissionGroupName) {
                throw new ArithmeticException("mismatch parameter values RevertReceiveByUserIdRequest::missionGroupName");
            }
            if (MissionTaskName != y.MissionTaskName) {
                throw new ArithmeticException("mismatch parameter values RevertReceiveByUserIdRequest::missionTaskName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values RevertReceiveByUserIdRequest::userId");
            }
            return new RevertReceiveByUserIdRequest {
                NamespaceName = NamespaceName,
                MissionGroupName = MissionGroupName,
                MissionTaskName = MissionTaskName,
                UserId = UserId,
            };
        }
    }
}