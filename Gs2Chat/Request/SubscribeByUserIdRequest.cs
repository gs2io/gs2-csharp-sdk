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
using Gs2.Gs2Chat.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class SubscribeByUserIdRequest : Gs2Request<SubscribeByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RoomName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Chat.Model.NotificationType[] NotificationTypes { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SubscribeByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SubscribeByUserIdRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public SubscribeByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscribeByUserIdRequest WithNotificationTypes(Gs2.Gs2Chat.Model.NotificationType[] notificationTypes) {
            this.NotificationTypes = notificationTypes;
            return this;
        }
        public SubscribeByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public SubscribeByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SubscribeByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithNotificationTypes(!data.Keys.Contains("notificationTypes") || data["notificationTypes"] == null || !data["notificationTypes"].IsArray ? null : data["notificationTypes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Chat.Model.NotificationType.FromJson(v);
                }).ToArray())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData notificationTypesJsonData = null;
            if (NotificationTypes != null && NotificationTypes.Length > 0)
            {
                notificationTypesJsonData = new JsonData();
                foreach (var notificationType in NotificationTypes)
                {
                    notificationTypesJsonData.Add(notificationType.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["roomName"] = RoomName,
                ["userId"] = UserId,
                ["notificationTypes"] = notificationTypesJsonData,
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
            if (RoomName != null) {
                writer.WritePropertyName("roomName");
                writer.Write(RoomName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (NotificationTypes != null) {
                writer.WritePropertyName("notificationTypes");
                writer.WriteArrayStart();
                foreach (var notificationType in NotificationTypes)
                {
                    if (notificationType != null) {
                        notificationType.WriteJson(writer);
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
            key += RoomName + ":";
            key += UserId + ":";
            key += NotificationTypes + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}