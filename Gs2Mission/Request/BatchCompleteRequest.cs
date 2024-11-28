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
	public class BatchCompleteRequest : Gs2Request<BatchCompleteRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string MissionGroupName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string[] MissionTaskNames { set; get; } = null!;
         public Gs2.Gs2Mission.Model.Config[] Config { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public BatchCompleteRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public BatchCompleteRequest WithMissionGroupName(string missionGroupName) {
            this.MissionGroupName = missionGroupName;
            return this;
        }
        public BatchCompleteRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public BatchCompleteRequest WithMissionTaskNames(string[] missionTaskNames) {
            this.MissionTaskNames = missionTaskNames;
            return this;
        }
        public BatchCompleteRequest WithConfig(Gs2.Gs2Mission.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public BatchCompleteRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchCompleteRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchCompleteRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithMissionGroupName(!data.Keys.Contains("missionGroupName") || data["missionGroupName"] == null ? null : data["missionGroupName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithMissionTaskNames(!data.Keys.Contains("missionTaskNames") || data["missionTaskNames"] == null || !data["missionTaskNames"].IsArray ? null : data["missionTaskNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.Config.FromJson(v);
                }).ToArray());
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
                ["accessToken"] = AccessToken,
                ["missionTaskNames"] = missionTaskNamesJsonData,
                ["config"] = configJsonData,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += MissionGroupName + ":";
            key += AccessToken + ":";
            key += MissionTaskNames + ":";
            key += Config + ":";
            return key;
        }
    }
}