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
	public class Inbox : IComparable
	{
        public string InboxId { set; get; }
        public string UserId { set; get; }
        public string[] FromUserIds { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Inbox WithInboxId(string inboxId) {
            this.InboxId = inboxId;
            return this;
        }

        public Inbox WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Inbox WithFromUserIds(string[] fromUserIds) {
            this.FromUserIds = fromUserIds;
            return this;
        }

        public Inbox WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Inbox WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static Inbox FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Inbox()
                .WithInboxId(!data.Keys.Contains("inboxId") || data["inboxId"] == null ? null : data["inboxId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithFromUserIds(!data.Keys.Contains("fromUserIds") || data["fromUserIds"] == null ? new string[]{} : data["fromUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["inboxId"] = InboxId,
                ["userId"] = UserId,
                ["fromUserIds"] = new JsonData(FromUserIds == null ? new JsonData[]{} :
                        FromUserIds.Select(v => {
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
            if (InboxId != null) {
                writer.WritePropertyName("inboxId");
                writer.Write(InboxId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (FromUserIds != null) {
                writer.WritePropertyName("fromUserIds");
                writer.WriteArrayStart();
                foreach (var fromUserId in FromUserIds)
                {
                    if (fromUserId != null) {
                        writer.Write(fromUserId.ToString());
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
            var other = obj as Inbox;
            var diff = 0;
            if (InboxId == null && InboxId == other.InboxId)
            {
                // null and null
            }
            else
            {
                diff += InboxId.CompareTo(other.InboxId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (FromUserIds == null && FromUserIds == other.FromUserIds)
            {
                // null and null
            }
            else
            {
                diff += FromUserIds.Length - other.FromUserIds.Length;
                for (var i = 0; i < FromUserIds.Length; i++)
                {
                    diff += FromUserIds[i].CompareTo(other.FromUserIds[i]);
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