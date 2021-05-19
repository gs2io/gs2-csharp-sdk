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
using UnityEngine.Scripting;

namespace Gs2.Gs2Chat.Model
{
	[Preserve]
	public class Room : IComparable
	{

        /** ルーム */
        public string roomId { set; get; }

        /**
         * ルームを設定
         *
         * @param roomId ルーム
         * @return this
         */
        public Room WithRoomId(string roomId) {
            this.roomId = roomId;
            return this;
        }

        /** ルーム名 */
        public string name { set; get; }

        /**
         * ルーム名を設定
         *
         * @param name ルーム名
         * @return this
         */
        public Room WithName(string name) {
            this.name = name;
            return this;
        }

        /** ルームを作成したユーザID */
        public string userId { set; get; }

        /**
         * ルームを作成したユーザIDを設定
         *
         * @param userId ルームを作成したユーザID
         * @return this
         */
        public Room WithUserId(string userId) {
            this.userId = userId;
            return this;
        }

        /** メタデータ */
        public string metadata { set; get; }

        /**
         * メタデータを設定
         *
         * @param metadata メタデータ
         * @return this
         */
        public Room WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** メッセージを投稿するために必要となるパスワード */
        public string password { set; get; }

        /**
         * メッセージを投稿するために必要となるパスワードを設定
         *
         * @param password メッセージを投稿するために必要となるパスワード
         * @return this
         */
        public Room WithPassword(string password) {
            this.password = password;
            return this;
        }

        /** ルームに参加可能なユーザIDリスト */
        public List<string> whiteListUserIds { set; get; }

        /**
         * ルームに参加可能なユーザIDリストを設定
         *
         * @param whiteListUserIds ルームに参加可能なユーザIDリスト
         * @return this
         */
        public Room WithWhiteListUserIds(List<string> whiteListUserIds) {
            this.whiteListUserIds = whiteListUserIds;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public Room WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public Room WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.roomId != null)
            {
                writer.WritePropertyName("roomId");
                writer.Write(this.roomId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.userId != null)
            {
                writer.WritePropertyName("userId");
                writer.Write(this.userId);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.password != null)
            {
                writer.WritePropertyName("password");
                writer.Write(this.password);
            }
            if(this.whiteListUserIds != null)
            {
                writer.WritePropertyName("whiteListUserIds");
                writer.WriteArrayStart();
                foreach(var item in this.whiteListUserIds)
                {
                    writer.Write(item);
                }
                writer.WriteArrayEnd();
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetRoomNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):chat:(?<namespaceName>.*):room:(?<roomName>.*)");
        if (!match.Groups["roomName"].Success)
        {
            return null;
        }
        return match.Groups["roomName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):chat:(?<namespaceName>.*):room:(?<roomName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):chat:(?<namespaceName>.*):room:(?<roomName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):chat:(?<namespaceName>.*):room:(?<roomName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Room FromDict(JsonData data)
        {
            return new Room()
                .WithRoomId(data.Keys.Contains("roomId") && data["roomId"] != null ? data["roomId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithUserId(data.Keys.Contains("userId") && data["userId"] != null ? data["userId"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithPassword(data.Keys.Contains("password") && data["password"] != null ? data["password"].ToString() : null)
                .WithWhiteListUserIds(data.Keys.Contains("whiteListUserIds") && data["whiteListUserIds"] != null ? data["whiteListUserIds"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Room;
            var diff = 0;
            if (roomId == null && roomId == other.roomId)
            {
                // null and null
            }
            else
            {
                diff += roomId.CompareTo(other.roomId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (userId == null && userId == other.userId)
            {
                // null and null
            }
            else
            {
                diff += userId.CompareTo(other.userId);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (password == null && password == other.password)
            {
                // null and null
            }
            else
            {
                diff += password.CompareTo(other.password);
            }
            if (whiteListUserIds == null && whiteListUserIds == other.whiteListUserIds)
            {
                // null and null
            }
            else
            {
                diff += whiteListUserIds.Count - other.whiteListUserIds.Count;
                for (var i = 0; i < whiteListUserIds.Count; i++)
                {
                    diff += whiteListUserIds[i].CompareTo(other.whiteListUserIds[i]);
                }
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["roomId"] = roomId;
            data["name"] = name;
            data["userId"] = userId;
            data["metadata"] = metadata;
            data["password"] = password;
            data["whiteListUserIds"] = new JsonData(whiteListUserIds);
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}