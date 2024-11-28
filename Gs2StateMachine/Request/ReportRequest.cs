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
using Gs2.Gs2StateMachine.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2StateMachine.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ReportRequest : Gs2Request<ReportRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string StatusName { set; get; } = null!;
         public Gs2.Gs2StateMachine.Model.Event[] Events { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public ReportRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ReportRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public ReportRequest WithStatusName(string statusName) {
            this.StatusName = statusName;
            return this;
        }
        public ReportRequest WithEvents(Gs2.Gs2StateMachine.Model.Event[] events) {
            this.Events = events;
            return this;
        }

        public ReportRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReportRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReportRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithStatusName(!data.Keys.Contains("statusName") || data["statusName"] == null ? null : data["statusName"].ToString())
                .WithEvents(!data.Keys.Contains("events") || data["events"] == null || !data["events"].IsArray ? null : data["events"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2StateMachine.Model.Event.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData eventsJsonData = null;
            if (Events != null && Events.Length > 0)
            {
                eventsJsonData = new JsonData();
                foreach (var event_ in Events)
                {
                    eventsJsonData.Add(event_.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["statusName"] = StatusName,
                ["events"] = eventsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (StatusName != null) {
                writer.WritePropertyName("statusName");
                writer.Write(StatusName.ToString());
            }
            if (Events != null) {
                writer.WritePropertyName("events");
                writer.WriteArrayStart();
                foreach (var event_ in Events)
                {
                    if (event_ != null) {
                        event_.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += StatusName + ":";
            key += Events + ":";
            return key;
        }
    }
}