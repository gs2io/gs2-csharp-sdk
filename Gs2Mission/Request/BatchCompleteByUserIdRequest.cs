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
	public class BatchCompleteByUserIdRequest : Gs2Request<BatchCompleteByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string MissionGroupName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string[] MissionTaskNames { set; get; } = null!;
         public Gs2.Gs2Mission.Model.Config[] Config { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public BatchCompleteByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public BatchCompleteByUserIdRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public BatchCompleteByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public BatchCompleteByUserIdRequest WithMissionTaskNames(string[] missionTaskNames) {
            this.MissionTaskNames = missionTaskNames;
            return this;
        }
        public BatchCompleteByUserIdRequest WithConfig(Gs2.Gs2Mission.Model.Config[] config) {
            this.Config = config;
            return this;
        }
        public BatchCompleteByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public BatchCompleteByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchCompleteByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchCompleteByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMissionTaskNames(!data.Keys.Contains("missionTaskNames") || data["missionTaskNames"] == null || !data["missionTaskNames"].IsArray ? null : data["missionTaskNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.Config.FromJson(v);
                }).ToArray())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData missionTaskNamesJsonData = null;
            if (MissionTaskNames != null && MissionTaskNames.Length > 0)
            {
                missionTaskNamesJsonData = new JsonData();
                foreach (var missionTaskName in MissionTaskNames)
                {
                    missionTaskNamesJsonData.Add(missionTaskName);
                }
            }
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["missionGroupName"] = MissionGroupName,
                ["userId"] = UserId,
                ["missionTaskNames"] = missionTaskNamesJsonData,
                ["config"] = configJsonData,
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
            if (MissionTaskNames != null) {
                writer.WritePropertyName("missionTaskNames");
                writer.WriteArrayStart();
                foreach (var missionTaskName in MissionTaskNames)
                {
                    writer.Write(missionTaskName.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            key += MissionTaskNames + ":";
            key += Config + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}