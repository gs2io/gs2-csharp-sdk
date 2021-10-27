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

namespace Gs2.Gs2Inbox.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Message : IComparable
	{
        public string MessageId { set; get; }
        public string Name { set; get; }
        public string UserId { set; get; }
        public string Metadata { set; get; }
        public bool? IsRead { set; get; }
        public Gs2.Gs2Inbox.Model.AcquireAction[] ReadAcquireActions { set; get; }
        public long? ReceivedAt { set; get; }
        public long? ReadAt { set; get; }
        public long? ExpiresAt { set; get; }

        public Message WithMessageId(string messageId) {
            this.MessageId = messageId;
            return this;
        }

        public Message WithName(string name) {
            this.Name = name;
            return this;
        }

        public Message WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Message WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public Message WithIsRead(bool? isRead) {
            this.IsRead = isRead;
            return this;
        }

        public Message WithReadAcquireActions(Gs2.Gs2Inbox.Model.AcquireAction[] readAcquireActions) {
            this.ReadAcquireActions = readAcquireActions;
            return this;
        }

        public Message WithReceivedAt(long? receivedAt) {
            this.ReceivedAt = receivedAt;
            return this;
        }

        public Message WithReadAt(long? readAt) {
            this.ReadAt = readAt;
            return this;
        }

        public Message WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Message FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Message()
                .WithMessageId(!data.Keys.Contains("messageId") || data["messageId"] == null ? null : data["messageId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithIsRead(!data.Keys.Contains("isRead") || data["isRead"] == null ? null : (bool?)bool.Parse(data["isRead"].ToString()))
                .WithReadAcquireActions(!data.Keys.Contains("readAcquireActions") || data["readAcquireActions"] == null ? new Gs2.Gs2Inbox.Model.AcquireAction[]{} : data["readAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inbox.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithReceivedAt(!data.Keys.Contains("receivedAt") || data["receivedAt"] == null ? null : (long?)long.Parse(data["receivedAt"].ToString()))
                .WithReadAt(!data.Keys.Contains("readAt") || data["readAt"] == null ? null : (long?)long.Parse(data["readAt"].ToString()))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)long.Parse(data["expiresAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["messageId"] = MessageId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["isRead"] = IsRead,
                ["readAcquireActions"] = new JsonData(ReadAcquireActions == null ? new JsonData[]{} :
                        ReadAcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["receivedAt"] = ReceivedAt,
                ["readAt"] = ReadAt,
                ["expiresAt"] = ExpiresAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (MessageId != null) {
                writer.WritePropertyName("messageId");
                writer.Write(MessageId.ToString());
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
            if (IsRead != null) {
                writer.WritePropertyName("isRead");
                writer.Write(bool.Parse(IsRead.ToString()));
            }
            if (ReadAcquireActions != null) {
                writer.WritePropertyName("readAcquireActions");
                writer.WriteArrayStart();
                foreach (var readAcquireAction in ReadAcquireActions)
                {
                    if (readAcquireAction != null) {
                        readAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ReceivedAt != null) {
                writer.WritePropertyName("receivedAt");
                writer.Write(long.Parse(ReceivedAt.ToString()));
            }
            if (ReadAt != null) {
                writer.WritePropertyName("readAt");
                writer.Write(long.Parse(ReadAt.ToString()));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write(long.Parse(ExpiresAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Message;
            var diff = 0;
            if (MessageId == null && MessageId == other.MessageId)
            {
                // null and null
            }
            else
            {
                diff += MessageId.CompareTo(other.MessageId);
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
            if (IsRead == null && IsRead == other.IsRead)
            {
                // null and null
            }
            else
            {
                diff += IsRead == other.IsRead ? 0 : 1;
            }
            if (ReadAcquireActions == null && ReadAcquireActions == other.ReadAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += ReadAcquireActions.Length - other.ReadAcquireActions.Length;
                for (var i = 0; i < ReadAcquireActions.Length; i++)
                {
                    diff += ReadAcquireActions[i].CompareTo(other.ReadAcquireActions[i]);
                }
            }
            if (ReceivedAt == null && ReceivedAt == other.ReceivedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ReceivedAt - other.ReceivedAt);
            }
            if (ReadAt == null && ReadAt == other.ReadAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ReadAt - other.ReadAt);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            return diff;
        }
    }
}