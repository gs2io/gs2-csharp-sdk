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
        public string SubscribeId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string RoomName { set; get; } = null!;
        public Gs2.Gs2Chat.Model.NotificationType[] NotificationTypes { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Subscribe WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<roomName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<roomName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<roomName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<roomName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _roomNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):chat:(?<namespaceName>.+):user:(?<userId>.+):subscribe:(?<roomName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRoomNameFromGrn(
            string grn
        )
        {
            var match = _roomNameRegex.Match(grn);
            if (!match.Success || !match.Groups["roomName"].Success)
            {
                return null;
            }
            return match.Groups["roomName"].Value;
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
                .WithNotificationTypes(!data.Keys.Contains("notificationTypes") || data["notificationTypes"] == null || !data["notificationTypes"].IsArray ? null : data["notificationTypes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Chat.Model.NotificationType.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
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
                ["subscribeId"] = SubscribeId,
                ["userId"] = UserId,
                ["roomName"] = RoomName,
                ["notificationTypes"] = notificationTypesJsonData,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
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
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SubscribeId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.subscribeId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.userId.error.tooLong"),
                    });
                }
            }
            {
                if (RoomName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.roomName.error.tooLong"),
                    });
                }
            }
            {
                if (NotificationTypes.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.notificationTypes.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribe", "chat.subscribe.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Subscribe {
                SubscribeId = SubscribeId,
                UserId = UserId,
                RoomName = RoomName,
                NotificationTypes = NotificationTypes.Clone() as Gs2.Gs2Chat.Model.NotificationType[],
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}