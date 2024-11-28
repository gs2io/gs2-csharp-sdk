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
        public string MessageId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public bool? IsRead { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] ReadAcquireActions { set; get; } = null!;
        public long? ReceivedAt { set; get; } = null!;
        public long? ReadAt { set; get; } = null!;
        public long? ExpiresAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Message WithReadAcquireActions(Gs2.Core.Model.AcquireAction[] readAcquireActions) {
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
        public Message WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+):message:(?<messageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+):message:(?<messageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+):message:(?<messageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+):message:(?<messageName>.+)",
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

        private static System.Text.RegularExpressions.Regex _messageNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+):message:(?<messageName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetMessageNameFromGrn(
            string grn
        )
        {
            var match = _messageNameRegex.Match(grn);
            if (!match.Success || !match.Groups["messageName"].Success)
            {
                return null;
            }
            return match.Groups["messageName"].Value;
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
                .WithReadAcquireActions(!data.Keys.Contains("readAcquireActions") || data["readAcquireActions"] == null || !data["readAcquireActions"].IsArray ? null : data["readAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithReceivedAt(!data.Keys.Contains("receivedAt") || data["receivedAt"] == null ? null : (long?)(data["receivedAt"].ToString().Contains(".") ? (long)double.Parse(data["receivedAt"].ToString()) : long.Parse(data["receivedAt"].ToString())))
                .WithReadAt(!data.Keys.Contains("readAt") || data["readAt"] == null ? null : (long?)(data["readAt"].ToString().Contains(".") ? (long)double.Parse(data["readAt"].ToString()) : long.Parse(data["readAt"].ToString())))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData readAcquireActionsJsonData = null;
            if (ReadAcquireActions != null && ReadAcquireActions.Length > 0)
            {
                readAcquireActionsJsonData = new JsonData();
                foreach (var readAcquireAction in ReadAcquireActions)
                {
                    readAcquireActionsJsonData.Add(readAcquireAction.ToJson());
                }
            }
            return new JsonData {
                ["messageId"] = MessageId,
                ["name"] = Name,
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["isRead"] = IsRead,
                ["readAcquireActions"] = readAcquireActionsJsonData,
                ["receivedAt"] = ReceivedAt,
                ["readAt"] = ReadAt,
                ["expiresAt"] = ExpiresAt,
                ["revision"] = Revision,
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
                writer.Write((ReceivedAt.ToString().Contains(".") ? (long)double.Parse(ReceivedAt.ToString()) : long.Parse(ReceivedAt.ToString())));
            }
            if (ReadAt != null) {
                writer.WritePropertyName("readAt");
                writer.Write((ReadAt.ToString().Contains(".") ? (long)double.Parse(ReadAt.ToString()) : long.Parse(ReadAt.ToString())));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
                if (MessageId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.messageId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.name.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 4096) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                if (ReadAcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.readAcquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (ReceivedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.receivedAt.error.invalid"),
                    });
                }
                if (ReceivedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.receivedAt.error.invalid"),
                    });
                }
            }
            {
                if (ReadAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.readAt.error.invalid"),
                    });
                }
                if (ReadAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.readAt.error.invalid"),
                    });
                }
            }
            {
                if (ExpiresAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.expiresAt.error.invalid"),
                    });
                }
                if (ExpiresAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.expiresAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("message", "inbox.message.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Message {
                MessageId = MessageId,
                Name = Name,
                UserId = UserId,
                Metadata = Metadata,
                IsRead = IsRead,
                ReadAcquireActions = ReadAcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                ReceivedAt = ReceivedAt,
                ReadAt = ReadAt,
                ExpiresAt = ExpiresAt,
                Revision = Revision,
            };
        }
    }
}