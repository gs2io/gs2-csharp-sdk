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

namespace Gs2.Gs2Realtime.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Room : IComparable
	{
        public string RoomId { set; get; }
        public string Name { set; get; }
        public string IpAddress { set; get; }
        public int? Port { set; get; }
        public string EncryptionKey { set; get; }
        public string[] NotificationUserIds { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public Room WithRoomId(string roomId) {
            this.RoomId = roomId;
            return this;
        }
        public Room WithName(string name) {
            this.Name = name;
            return this;
        }
        public Room WithIpAddress(string ipAddress) {
            this.IpAddress = ipAddress;
            return this;
        }
        public Room WithPort(int? port) {
            this.Port = port;
            return this;
        }
        public Room WithEncryptionKey(string encryptionKey) {
            this.EncryptionKey = encryptionKey;
            return this;
        }
        public Room WithNotificationUserIds(string[] notificationUserIds) {
            this.NotificationUserIds = notificationUserIds;
            return this;
        }
        public Room WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Room WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):realtime:(?<namespaceName>.+):room:(?<roomName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):realtime:(?<namespaceName>.+):room:(?<roomName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):realtime:(?<namespaceName>.+):room:(?<roomName>.+)",
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

        private static System.Text.RegularExpressions.Regex _roomNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):realtime:(?<namespaceName>.+):room:(?<roomName>.+)",
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
        public static Room FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Room()
                .WithRoomId(!data.Keys.Contains("roomId") || data["roomId"] == null ? null : data["roomId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithIpAddress(!data.Keys.Contains("ipAddress") || data["ipAddress"] == null ? null : data["ipAddress"].ToString())
                .WithPort(!data.Keys.Contains("port") || data["port"] == null ? null : (int?)int.Parse(data["port"].ToString()))
                .WithEncryptionKey(!data.Keys.Contains("encryptionKey") || data["encryptionKey"] == null ? null : data["encryptionKey"].ToString())
                .WithNotificationUserIds(!data.Keys.Contains("notificationUserIds") || data["notificationUserIds"] == null ? new string[]{} : data["notificationUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["roomId"] = RoomId,
                ["name"] = Name,
                ["ipAddress"] = IpAddress,
                ["port"] = Port,
                ["encryptionKey"] = EncryptionKey,
                ["notificationUserIds"] = NotificationUserIds == null ? null : new JsonData(
                        NotificationUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RoomId != null) {
                writer.WritePropertyName("roomId");
                writer.Write(RoomId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (IpAddress != null) {
                writer.WritePropertyName("ipAddress");
                writer.Write(IpAddress.ToString());
            }
            if (Port != null) {
                writer.WritePropertyName("port");
                writer.Write(int.Parse(Port.ToString()));
            }
            if (EncryptionKey != null) {
                writer.WritePropertyName("encryptionKey");
                writer.Write(EncryptionKey.ToString());
            }
            if (NotificationUserIds != null) {
                writer.WritePropertyName("notificationUserIds");
                writer.WriteArrayStart();
                foreach (var notificationUserId in NotificationUserIds)
                {
                    if (notificationUserId != null) {
                        writer.Write(notificationUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Room;
            var diff = 0;
            if (RoomId == null && RoomId == other.RoomId)
            {
                // null and null
            }
            else
            {
                diff += RoomId.CompareTo(other.RoomId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (IpAddress == null && IpAddress == other.IpAddress)
            {
                // null and null
            }
            else
            {
                diff += IpAddress.CompareTo(other.IpAddress);
            }
            if (Port == null && Port == other.Port)
            {
                // null and null
            }
            else
            {
                diff += (int)(Port - other.Port);
            }
            if (EncryptionKey == null && EncryptionKey == other.EncryptionKey)
            {
                // null and null
            }
            else
            {
                diff += EncryptionKey.CompareTo(other.EncryptionKey);
            }
            if (NotificationUserIds == null && NotificationUserIds == other.NotificationUserIds)
            {
                // null and null
            }
            else
            {
                diff += NotificationUserIds.Length - other.NotificationUserIds.Length;
                for (var i = 0; i < NotificationUserIds.Length; i++)
                {
                    diff += NotificationUserIds[i].CompareTo(other.NotificationUserIds[i]);
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
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}