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
	public class EmitByUserIdRequest : Gs2Request<EmitByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string StatusName { set; get; } = null!;
         public string EventName { set; get; } = null!;
         public string Args { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public EmitByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public EmitByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public EmitByUserIdRequest WithStatusName(string statusName) {
            this.StatusName = statusName;
            return this;
        }
        public EmitByUserIdRequest WithEventName(string eventName) {
            this.EventName = eventName;
            return this;
        }
        public EmitByUserIdRequest WithArgs(string args) {
            this.Args = args;
            return this;
        }
        public EmitByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public EmitByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static EmitByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new EmitByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithStatusName(!data.Keys.Contains("statusName") || data["statusName"] == null ? null : data["statusName"].ToString())
                .WithEventName(!data.Keys.Contains("eventName") || data["eventName"] == null ? null : data["eventName"].ToString())
                .WithArgs(!data.Keys.Contains("args") || data["args"] == null ? null : data["args"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["statusName"] = StatusName,
                ["eventName"] = EventName,
                ["args"] = Args,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
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
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += StatusName + ":";
            key += EventName + ":";
            key += Args + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}