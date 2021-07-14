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

namespace Gs2.Gs2Friend.Model
{

	[Preserve]
	public class Friend : IComparable
	{
        public string FriendId { set; get; }
        public string UserId { set; get; }
        public string[] TargetUserIds { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Friend WithFriendId(string friendId) {
            this.FriendId = friendId;
            return this;
        }

        public Friend WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Friend WithTargetUserIds(string[] targetUserIds) {
            this.TargetUserIds = targetUserIds;
            return this;
        }

        public Friend WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Friend WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static Friend FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Friend()
                .WithFriendId(!data.Keys.Contains("friendId") || data["friendId"] == null ? null : data["friendId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserIds(!data.Keys.Contains("targetUserIds") || data["targetUserIds"] == null ? new string[]{} : data["targetUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["friendId"] = FriendId,
                ["userId"] = UserId,
                ["targetUserIds"] = new JsonData(TargetUserIds == null ? new JsonData[]{} :
                        TargetUserIds.Select(v => {
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
            if (FriendId != null) {
                writer.WritePropertyName("friendId");
                writer.Write(FriendId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TargetUserIds != null) {
                writer.WritePropertyName("targetUserIds");
                writer.WriteArrayStart();
                foreach (var targetUserId in TargetUserIds)
                {
                    if (targetUserId != null) {
                        writer.Write(targetUserId.ToString());
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
            var other = obj as Friend;
            var diff = 0;
            if (FriendId == null && FriendId == other.FriendId)
            {
                // null and null
            }
            else
            {
                diff += FriendId.CompareTo(other.FriendId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TargetUserIds == null && TargetUserIds == other.TargetUserIds)
            {
                // null and null
            }
            else
            {
                diff += TargetUserIds.Length - other.TargetUserIds.Length;
                for (var i = 0; i < TargetUserIds.Length; i++)
                {
                    diff += TargetUserIds[i].CompareTo(other.TargetUserIds[i]);
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