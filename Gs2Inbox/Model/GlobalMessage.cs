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
	public class GlobalMessage : IComparable
	{
        public string GlobalMessageId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] ReadAcquireActions { set; get; } = null!;
        public Gs2.Gs2Inbox.Model.TimeSpan_ ExpiresTimeSpan { set; get; } = null!;
        [Obsolete("This method is deprecated")]
        public long? ExpiresAt { set; get; } = null!;
        public string MessageReceptionPeriodEventId { set; get; } = null!;
        public GlobalMessage WithGlobalMessageId(string globalMessageId) {
            this.GlobalMessageId = globalMessageId;
            return this;
        }
        public GlobalMessage WithName(string name) {
            this.Name = name;
            return this;
        }
        public GlobalMessage WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public GlobalMessage WithReadAcquireActions(Gs2.Core.Model.AcquireAction[] readAcquireActions) {
            this.ReadAcquireActions = readAcquireActions;
            return this;
        }
        public GlobalMessage WithExpiresTimeSpan(Gs2.Gs2Inbox.Model.TimeSpan_ expiresTimeSpan) {
            this.ExpiresTimeSpan = expiresTimeSpan;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public GlobalMessage WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public GlobalMessage WithMessageReceptionPeriodEventId(string messageReceptionPeriodEventId) {
            this.MessageReceptionPeriodEventId = messageReceptionPeriodEventId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):globalMessage:(?<globalMessageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):globalMessage:(?<globalMessageName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):globalMessage:(?<globalMessageName>.+)",
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

        private static System.Text.RegularExpressions.Regex _globalMessageNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):globalMessage:(?<globalMessageName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGlobalMessageNameFromGrn(
            string grn
        )
        {
            var match = _globalMessageNameRegex.Match(grn);
            if (!match.Success || !match.Groups["globalMessageName"].Success)
            {
                return null;
            }
            return match.Groups["globalMessageName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GlobalMessage FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GlobalMessage()
                .WithGlobalMessageId(!data.Keys.Contains("globalMessageId") || data["globalMessageId"] == null ? null : data["globalMessageId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithReadAcquireActions(!data.Keys.Contains("readAcquireActions") || data["readAcquireActions"] == null || !data["readAcquireActions"].IsArray ? null : data["readAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithExpiresTimeSpan(!data.Keys.Contains("expiresTimeSpan") || data["expiresTimeSpan"] == null ? null : Gs2.Gs2Inbox.Model.TimeSpan_.FromJson(data["expiresTimeSpan"]))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithMessageReceptionPeriodEventId(!data.Keys.Contains("messageReceptionPeriodEventId") || data["messageReceptionPeriodEventId"] == null ? null : data["messageReceptionPeriodEventId"].ToString());
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
                ["globalMessageId"] = GlobalMessageId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["readAcquireActions"] = readAcquireActionsJsonData,
                ["expiresTimeSpan"] = ExpiresTimeSpan?.ToJson(),
                ["messageReceptionPeriodEventId"] = MessageReceptionPeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GlobalMessageId != null) {
                writer.WritePropertyName("globalMessageId");
                writer.Write(GlobalMessageId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
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
            if (ExpiresTimeSpan != null) {
                writer.WritePropertyName("expiresTimeSpan");
                ExpiresTimeSpan.WriteJson(writer);
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (MessageReceptionPeriodEventId != null) {
                writer.WritePropertyName("messageReceptionPeriodEventId");
                writer.Write(MessageReceptionPeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GlobalMessage;
            var diff = 0;
            if (GlobalMessageId == null && GlobalMessageId == other.GlobalMessageId)
            {
                // null and null
            }
            else
            {
                diff += GlobalMessageId.CompareTo(other.GlobalMessageId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
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
            if (ExpiresTimeSpan == null && ExpiresTimeSpan == other.ExpiresTimeSpan)
            {
                // null and null
            }
            else
            {
                diff += ExpiresTimeSpan.CompareTo(other.ExpiresTimeSpan);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            if (MessageReceptionPeriodEventId == null && MessageReceptionPeriodEventId == other.MessageReceptionPeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += MessageReceptionPeriodEventId.CompareTo(other.MessageReceptionPeriodEventId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (GlobalMessageId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalMessage", "inbox.globalMessage.globalMessageId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalMessage", "inbox.globalMessage.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 4096) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalMessage", "inbox.globalMessage.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (ReadAcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalMessage", "inbox.globalMessage.readAcquireActions.error.tooMany"),
                    });
                }
            }
            {
            }
            {
                if (MessageReceptionPeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalMessage", "inbox.globalMessage.messageReceptionPeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GlobalMessage {
                GlobalMessageId = GlobalMessageId,
                Name = Name,
                Metadata = Metadata,
                ReadAcquireActions = ReadAcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
                ExpiresTimeSpan = ExpiresTimeSpan.Clone() as Gs2.Gs2Inbox.Model.TimeSpan_,
                ExpiresAt = ExpiresAt,
                MessageReceptionPeriodEventId = MessageReceptionPeriodEventId,
            };
        }
    }
}