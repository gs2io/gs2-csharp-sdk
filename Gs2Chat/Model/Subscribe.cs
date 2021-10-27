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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Subscribe : IComparable
	{
        public string SubscribeId { set; get; }
        public string UserId { set; get; }
        public string RoomName { set; get; }
        public Gs2.Gs2Chat.Model.NotificationType[] NotificationTypes { set; get; }
        public long? CreatedAt { set; get; }

        public Subscribe WithSubscribeId(string subscribeId) {
            this.SubscribeId = subscribeId;
            return this;
        }

        public Subscribe WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Subscribe WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }

        public Subscribe WithNotificationTypes(Gs2.Gs2Chat.Model.NotificationType[] notificationTypes) {
            this.NotificationTypes = notificationTypes;
            return this;
        }

        public Subscribe WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Subscribe FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Subscribe()
                .WithSubscribeId(!data.Keys.Contains("subscribeId") || data["subscribeId"] == null ? null : data["subscribeId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithNotificationTypes(!data.Keys.Contains("notificationTypes") || data["notificationTypes"] == null ? new Gs2.Gs2Chat.Model.NotificationType[]{} : data["notificationTypes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Chat.Model.NotificationType.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeId"] = SubscribeId,
                ["userId"] = UserId,
                ["roomName"] = RoomName,
                ["notificationTypes"] = new JsonData(NotificationTypes == null ? new JsonData[]{} :
                        NotificationTypes.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SubscribeId != null) {
                writer.WritePropertyName("subscribeId");
                writer.Write(SubscribeId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RoomName != null) {
                writer.WritePropertyName("roomName");
                writer.Write(RoomName.ToString());
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Subscribe;
            var diff = 0;
            if (SubscribeId == null && SubscribeId == other.SubscribeId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeId.CompareTo(other.SubscribeId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RoomName == null && RoomName == other.RoomName)
            {
                // null and null
            }
            else
            {
                diff += RoomName.CompareTo(other.RoomName);
            }
            if (NotificationTypes == null && NotificationTypes == other.NotificationTypes)
            {
                // null and null
            }
            else
            {
                diff += NotificationTypes.Length - other.NotificationTypes.Length;
                for (var i = 0; i < NotificationTypes.Length; i++)
                {
                    diff += NotificationTypes[i].CompareTo(other.NotificationTypes[i]);
                }
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }
    }
}