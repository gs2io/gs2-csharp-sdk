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
using UnityEngine.Scripting;

namespace Gs2.Gs2Chat.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateNotificationTypeByUserIdRequest : Gs2Request<UpdateNotificationTypeByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string RoomName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2Chat.Model.NotificationType[] NotificationTypes { set; get; }

        public UpdateNotificationTypeByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateNotificationTypeByUserIdRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }

        public UpdateNotificationTypeByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public UpdateNotificationTypeByUserIdRequest WithNotificationTypes(Gs2.Gs2Chat.Model.NotificationType[] notificationTypes) {
            this.NotificationTypes = notificationTypes;
            return this;
        }

    	[Preserve]
        public static UpdateNotificationTypeByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateNotificationTypeByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithNotificationTypes(!data.Keys.Contains("notificationTypes") || data["notificationTypes"] == null ? new Gs2.Gs2Chat.Model.NotificationType[]{} : data["notificationTypes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Chat.Model.NotificationType.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["roomName"] = RoomName,
                ["userId"] = UserId,
                ["notificationTypes"] = new JsonData(NotificationTypes == null ? new JsonData[]{} :
                        NotificationTypes.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
            writer.WriteArrayStart();
            foreach (var notificationType in NotificationTypes)
            {
                if (notificationType != null) {
                    notificationType.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}