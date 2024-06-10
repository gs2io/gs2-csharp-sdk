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
	public class EmitRequest : Gs2Request<EmitRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string StatusName { set; get; } = null!;
         public string EventName { set; get; } = null!;
         public string Args { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public EmitRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public EmitRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public EmitRequest WithStatusName(string statusName) {
            this.StatusName = statusName;
            return this;
        }
        public EmitRequest WithEventName(string eventName) {
            this.EventName = eventName;
            return this;
        }
        public EmitRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }

        public EmitRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EmitRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EmitRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithStatusName(!data.Keys.Contains("statusName") || data["statusName"] == null ? null : data["statusName"].ToString())
                .WithEventName(!data.Keys.Contains("eventName") || data["eventName"] == null ? null : data["eventName"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["statusName"] = StatusName,
                ["eventName"] = EventName,
                ["args"] = Args,
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
            if (EventName != null) {
                writer.WritePropertyName("eventName");
                writer.Write(EventName.ToString());
            }
            if (Args != null) {
                writer.WritePropertyName("args");
                writer.Write(Args.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += StatusName + ":";
            key += EventName + ":";
            key += Args + ":";
            return key;
        }
    }
}