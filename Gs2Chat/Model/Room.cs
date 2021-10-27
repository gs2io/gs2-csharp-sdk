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
	public class Room : IComparable
	{
        public string RoomId { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public string Metadata { set; get; }
        public string Password { set; get; }
        public string[] WhiteListUserIds { set; get; }
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

        public Room WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Room WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public Room WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public Room WithWhiteListUserIds(string[] whiteListUserIds) {
            this.WhiteListUserIds = whiteListUserIds;
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
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithWhiteListUserIds(!data.Keys.Contains("whiteListUserIds") || data["whiteListUserIds"] == null ? new string[]{} : data["whiteListUserIds"].Cast<JsonData>().Select(v => {
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
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["password"] = Password,
                ["whiteListUserIds"] = new JsonData(WhiteListUserIds == null ? new JsonData[]{} :
                        WhiteListUserIds.Select(v => {
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (WhiteListUserIds != null) {
                writer.WritePropertyName("whiteListUserIds");
                writer.WriteArrayStart();
                foreach (var whiteListUserId in WhiteListUserIds)
                {
                    if (whiteListUserId != null) {
                        writer.Write(whiteListUserId.ToString());
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
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Password == null && Password == other.Password)
            {
                // null and null
            }
            else
            {
                diff += Password.CompareTo(other.Password);
            }
            if (WhiteListUserIds == null && WhiteListUserIds == other.WhiteListUserIds)
            {
                // null and null
            }
            else
            {
                diff += WhiteListUserIds.Length - other.WhiteListUserIds.Length;
                for (var i = 0; i < WhiteListUserIds.Length; i++)
                {
                    diff += WhiteListUserIds[i].CompareTo(other.WhiteListUserIds[i]);
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