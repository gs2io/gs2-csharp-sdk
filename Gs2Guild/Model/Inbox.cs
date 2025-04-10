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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Inbox : IComparable
	{
        public string InboxId { set; get; }
        public string GuildName { set; get; }
        [Obsolete("This method is deprecated")]
        public string[] FromUserIds { set; get; }
        public Gs2.Gs2Guild.Model.ReceiveMemberRequest[] ReceiveMemberRequests { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public Inbox WithInboxId(string inboxId) {
            this.InboxId = inboxId;
            return this;
        }
        public Inbox WithGuildName(string guildName) {
            this.GuildName = guildName;
            return this;
        }
        [Obsolete("This method is deprecated")]
        public Inbox WithFromUserIds(string[] fromUserIds) {
            this.FromUserIds = fromUserIds;
            return this;
        }
        public Inbox WithReceiveMemberRequests(Gs2.Gs2Guild.Model.ReceiveMemberRequest[] receiveMemberRequests) {
            this.ReceiveMemberRequests = receiveMemberRequests;
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
        public Inbox WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+):inbox",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+):inbox",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+):inbox",
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

        private static System.Text.RegularExpressions.Regex _guildModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+):inbox",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGuildModelNameFromGrn(
            string grn
        )
        {
            var match = _guildModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["guildModelName"].Success)
            {
                return null;
            }
            return match.Groups["guildModelName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _guildNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+):inbox",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGuildNameFromGrn(
            string grn
        )
        {
            var match = _guildNameRegex.Match(grn);
            if (!match.Success || !match.Groups["guildName"].Success)
            {
                return null;
            }
            return match.Groups["guildName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Inbox FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Inbox()
                .WithInboxId(!data.Keys.Contains("inboxId") || data["inboxId"] == null ? null : data["inboxId"].ToString())
                .WithGuildName(!data.Keys.Contains("guildName") || data["guildName"] == null ? null : data["guildName"].ToString())
                .WithFromUserIds(!data.Keys.Contains("fromUserIds") || data["fromUserIds"] == null || !data["fromUserIds"].IsArray ? null : data["fromUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithReceiveMemberRequests(!data.Keys.Contains("receiveMemberRequests") || data["receiveMemberRequests"] == null || !data["receiveMemberRequests"].IsArray ? null : data["receiveMemberRequests"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.ReceiveMemberRequest.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData fromUserIdsJsonData = null;
            if (FromUserIds != null && FromUserIds.Length > 0)
            {
                fromUserIdsJsonData = new JsonData();
                foreach (var fromUserId in FromUserIds)
                {
                    fromUserIdsJsonData.Add(fromUserId);
                }
            }
            JsonData receiveMemberRequestsJsonData = null;
            if (ReceiveMemberRequests != null && ReceiveMemberRequests.Length > 0)
            {
                receiveMemberRequestsJsonData = new JsonData();
                foreach (var receiveMemberRequest in ReceiveMemberRequests)
                {
                    receiveMemberRequestsJsonData.Add(receiveMemberRequest.ToJson());
                }
            }
            return new JsonData {
                ["inboxId"] = InboxId,
                ["guildName"] = GuildName,
                ["receiveMemberRequests"] = receiveMemberRequestsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InboxId != null) {
                writer.WritePropertyName("inboxId");
                writer.Write(InboxId.ToString());
            }
            if (GuildName != null) {
                writer.WritePropertyName("guildName");
                writer.Write(GuildName.ToString());
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
            if (ReceiveMemberRequests != null) {
                writer.WritePropertyName("receiveMemberRequests");
                writer.WriteArrayStart();
                foreach (var receiveMemberRequest in ReceiveMemberRequests)
                {
                    if (receiveMemberRequest != null) {
                        receiveMemberRequest.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (GuildName == null && GuildName == other.GuildName)
            {
                // null and null
            }
            else
            {
                diff += GuildName.CompareTo(other.GuildName);
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
            if (ReceiveMemberRequests == null && ReceiveMemberRequests == other.ReceiveMemberRequests)
            {
                // null and null
            }
            else
            {
                diff += ReceiveMemberRequests.Length - other.ReceiveMemberRequests.Length;
                for (var i = 0; i < ReceiveMemberRequests.Length; i++)
                {
                    diff += ReceiveMemberRequests[i].CompareTo(other.ReceiveMemberRequests[i]);
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
                if (InboxId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.inboxId.error.tooLong"),
                    });
                }
            }
            {
                if (GuildName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.guildName.error.tooLong"),
                    });
                }
            }
            {
                if (ReceiveMemberRequests.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.receiveMemberRequests.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inbox", "guild.inbox.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Inbox {
                InboxId = InboxId,
                GuildName = GuildName,
                FromUserIds = FromUserIds?.Clone() as string[],
                ReceiveMemberRequests = ReceiveMemberRequests?.Clone() as Gs2.Gs2Guild.Model.ReceiveMemberRequest[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}