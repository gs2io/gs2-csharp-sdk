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
using Gs2.Gs2Gateway.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Gateway.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SendNotificationRequest : Gs2Request<SendNotificationRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string Subject { set; get; } = null!;
         public string Payload { set; get; } = null!;
         public bool? EnableTransferMobileNotification { set; get; } = null!;
         public string Sound { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SendNotificationRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SendNotificationRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SendNotificationRequest WithSubject(string subject) {
            this.Subject = subject;
            return this;
        }
        public SendNotificationRequest WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }
        public SendNotificationRequest WithEnableTransferMobileNotification(bool? enableTransferMobileNotification) {
            this.EnableTransferMobileNotification = enableTransferMobileNotification;
            return this;
        }
        public SendNotificationRequest WithSound(string sound) {
            this.Sound = sound;
            return this;
        }
        public SendNotificationRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public SendNotificationRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendNotificationRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendNotificationRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSubject(!data.Keys.Contains("subject") || data["subject"] == null ? null : data["subject"].ToString())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString())
                .WithEnableTransferMobileNotification(!data.Keys.Contains("enableTransferMobileNotification") || data["enableTransferMobileNotification"] == null ? null : (bool?)bool.Parse(data["enableTransferMobileNotification"].ToString()))
                .WithSound(!data.Keys.Contains("sound") || data["sound"] == null ? null : data["sound"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["subject"] = Subject,
                ["payload"] = Payload,
                ["enableTransferMobileNotification"] = EnableTransferMobileNotification,
                ["sound"] = Sound,
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
            if (Subject != null) {
                writer.WritePropertyName("subject");
                writer.Write(Subject.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            if (EnableTransferMobileNotification != null) {
                writer.WritePropertyName("enableTransferMobileNotification");
                writer.Write(bool.Parse(EnableTransferMobileNotification.ToString()));
            }
            if (Sound != null) {
                writer.WritePropertyName("sound");
                writer.Write(Sound.ToString());
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
            key += Subject + ":";
            key += Payload + ":";
            key += EnableTransferMobileNotification + ":";
            key += Sound + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}