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

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class JoinedGuild : IComparable
	{
        public string JoinedGuildId { set; get; } = null!;
        public string GuildModelName { set; get; } = null!;
        public string GuildName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public JoinedGuild WithJoinedGuildId(string joinedGuildId) {
            this.JoinedGuildId = joinedGuildId;
            return this;
        }
        public JoinedGuild WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public JoinedGuild WithGuildName(string guildName) {
            this.GuildName = guildName;
            return this;
        }
        public JoinedGuild WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public JoinedGuild WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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

        private static System.Text.RegularExpressions.Regex _guildModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):user:(?<userId>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
        public static JoinedGuild FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JoinedGuild()
                .WithJoinedGuildId(!data.Keys.Contains("joinedGuildId") || data["joinedGuildId"] == null ? null : data["joinedGuildId"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithGuildName(!data.Keys.Contains("guildName") || data["guildName"] == null ? null : data["guildName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["joinedGuildId"] = JoinedGuildId,
                ["guildModelName"] = GuildModelName,
                ["guildName"] = GuildName,
                ["userId"] = UserId,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (JoinedGuildId != null) {
                writer.WritePropertyName("joinedGuildId");
                writer.Write(JoinedGuildId.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
            }
            if (GuildName != null) {
                writer.WritePropertyName("guildName");
                writer.Write(GuildName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as JoinedGuild;
            var diff = 0;
            if (JoinedGuildId == null && JoinedGuildId == other.JoinedGuildId)
            {
                // null and null
            }
            else
            {
                diff += JoinedGuildId.CompareTo(other.JoinedGuildId);
            }
            if (GuildModelName == null && GuildModelName == other.GuildModelName)
            {
                // null and null
            }
            else
            {
                diff += GuildModelName.CompareTo(other.GuildModelName);
            }
            if (GuildName == null && GuildName == other.GuildName)
            {
                // null and null
            }
            else
            {
                diff += GuildName.CompareTo(other.GuildName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (JoinedGuildId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.joinedGuildId.error.tooLong"),
                    });
                }
            }
            {
                if (GuildModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.guildModelName.error.tooLong"),
                    });
                }
            }
            {
                if (GuildName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.guildName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.userId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedGuild", "guild.joinedGuild.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new JoinedGuild {
                JoinedGuildId = JoinedGuildId,
                GuildModelName = GuildModelName,
                GuildName = GuildName,
                UserId = UserId,
                CreatedAt = CreatedAt,
            };
        }
    }
}