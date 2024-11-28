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
	public class Guild : IComparable
	{
        public string GuildId { set; get; } = null!;
        public string GuildModelName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string DisplayName { set; get; } = null!;
        public int? Attribute1 { set; get; } = null!;
        public int? Attribute2 { set; get; } = null!;
        public int? Attribute3 { set; get; } = null!;
        public int? Attribute4 { set; get; } = null!;
        public int? Attribute5 { set; get; } = null!;
        public string JoinPolicy { set; get; } = null!;
        public Gs2.Gs2Guild.Model.RoleModel[] CustomRoles { set; get; } = null!;
        public string GuildMemberDefaultRole { set; get; } = null!;
        public int? CurrentMaximumMemberCount { set; get; } = null!;
        public Gs2.Gs2Guild.Model.Member[] Members { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Guild WithGuildId(string guildId) {
            this.GuildId = guildId;
            return this;
        }
        public Guild WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public Guild WithName(string name) {
            this.Name = name;
            return this;
        }
        public Guild WithDisplayName(string displayName) {
            this.DisplayName = displayName;
            return this;
        }
        public Guild WithAttribute1(int? attribute1) {
            this.Attribute1 = attribute1;
            return this;
        }
        public Guild WithAttribute2(int? attribute2) {
            this.Attribute2 = attribute2;
            return this;
        }
        public Guild WithAttribute3(int? attribute3) {
            this.Attribute3 = attribute3;
            return this;
        }
        public Guild WithAttribute4(int? attribute4) {
            this.Attribute4 = attribute4;
            return this;
        }
        public Guild WithAttribute5(int? attribute5) {
            this.Attribute5 = attribute5;
            return this;
        }
        public Guild WithJoinPolicy(string joinPolicy) {
            this.JoinPolicy = joinPolicy;
            return this;
        }
        public Guild WithCustomRoles(Gs2.Gs2Guild.Model.RoleModel[] customRoles) {
            this.CustomRoles = customRoles;
            return this;
        }
        public Guild WithGuildMemberDefaultRole(string guildMemberDefaultRole) {
            this.GuildMemberDefaultRole = guildMemberDefaultRole;
            return this;
        }
        public Guild WithCurrentMaximumMemberCount(int? currentMaximumMemberCount) {
            this.CurrentMaximumMemberCount = currentMaximumMemberCount;
            return this;
        }
        public Guild WithMembers(Gs2.Gs2Guild.Model.Member[] members) {
            this.Members = members;
            return this;
        }
        public Guild WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Guild WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Guild WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):guild:(?<guildModelName>.+):(?<guildName>.+)",
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
        public static Guild FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Guild()
                .WithGuildId(!data.Keys.Contains("guildId") || data["guildId"] == null ? null : data["guildId"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDisplayName(!data.Keys.Contains("displayName") || data["displayName"] == null ? null : data["displayName"].ToString())
                .WithAttribute1(!data.Keys.Contains("attribute1") || data["attribute1"] == null ? null : (int?)(data["attribute1"].ToString().Contains(".") ? (int)double.Parse(data["attribute1"].ToString()) : int.Parse(data["attribute1"].ToString())))
                .WithAttribute2(!data.Keys.Contains("attribute2") || data["attribute2"] == null ? null : (int?)(data["attribute2"].ToString().Contains(".") ? (int)double.Parse(data["attribute2"].ToString()) : int.Parse(data["attribute2"].ToString())))
                .WithAttribute3(!data.Keys.Contains("attribute3") || data["attribute3"] == null ? null : (int?)(data["attribute3"].ToString().Contains(".") ? (int)double.Parse(data["attribute3"].ToString()) : int.Parse(data["attribute3"].ToString())))
                .WithAttribute4(!data.Keys.Contains("attribute4") || data["attribute4"] == null ? null : (int?)(data["attribute4"].ToString().Contains(".") ? (int)double.Parse(data["attribute4"].ToString()) : int.Parse(data["attribute4"].ToString())))
                .WithAttribute5(!data.Keys.Contains("attribute5") || data["attribute5"] == null ? null : (int?)(data["attribute5"].ToString().Contains(".") ? (int)double.Parse(data["attribute5"].ToString()) : int.Parse(data["attribute5"].ToString())))
                .WithJoinPolicy(!data.Keys.Contains("joinPolicy") || data["joinPolicy"] == null ? null : data["joinPolicy"].ToString())
                .WithCustomRoles(!data.Keys.Contains("customRoles") || data["customRoles"] == null || !data["customRoles"].IsArray ? null : data["customRoles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.RoleModel.FromJson(v);
                }).ToArray())
                .WithGuildMemberDefaultRole(!data.Keys.Contains("guildMemberDefaultRole") || data["guildMemberDefaultRole"] == null ? null : data["guildMemberDefaultRole"].ToString())
                .WithCurrentMaximumMemberCount(!data.Keys.Contains("currentMaximumMemberCount") || data["currentMaximumMemberCount"] == null ? null : (int?)(data["currentMaximumMemberCount"].ToString().Contains(".") ? (int)double.Parse(data["currentMaximumMemberCount"].ToString()) : int.Parse(data["currentMaximumMemberCount"].ToString())))
                .WithMembers(!data.Keys.Contains("members") || data["members"] == null || !data["members"].IsArray ? null : data["members"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.Member.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData customRolesJsonData = null;
            if (CustomRoles != null && CustomRoles.Length > 0)
            {
                customRolesJsonData = new JsonData();
                foreach (var customRole in CustomRoles)
                {
                    customRolesJsonData.Add(customRole.ToJson());
                }
            }
            JsonData membersJsonData = null;
            if (Members != null && Members.Length > 0)
            {
                membersJsonData = new JsonData();
                foreach (var member in Members)
                {
                    membersJsonData.Add(member.ToJson());
                }
            }
            return new JsonData {
                ["guildId"] = GuildId,
                ["guildModelName"] = GuildModelName,
                ["name"] = Name,
                ["displayName"] = DisplayName,
                ["attribute1"] = Attribute1,
                ["attribute2"] = Attribute2,
                ["attribute3"] = Attribute3,
                ["attribute4"] = Attribute4,
                ["attribute5"] = Attribute5,
                ["joinPolicy"] = JoinPolicy,
                ["customRoles"] = customRolesJsonData,
                ["guildMemberDefaultRole"] = GuildMemberDefaultRole,
                ["currentMaximumMemberCount"] = CurrentMaximumMemberCount,
                ["members"] = membersJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GuildId != null) {
                writer.WritePropertyName("guildId");
                writer.Write(GuildId.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (DisplayName != null) {
                writer.WritePropertyName("displayName");
                writer.Write(DisplayName.ToString());
            }
            if (Attribute1 != null) {
                writer.WritePropertyName("attribute1");
                writer.Write((Attribute1.ToString().Contains(".") ? (int)double.Parse(Attribute1.ToString()) : int.Parse(Attribute1.ToString())));
            }
            if (Attribute2 != null) {
                writer.WritePropertyName("attribute2");
                writer.Write((Attribute2.ToString().Contains(".") ? (int)double.Parse(Attribute2.ToString()) : int.Parse(Attribute2.ToString())));
            }
            if (Attribute3 != null) {
                writer.WritePropertyName("attribute3");
                writer.Write((Attribute3.ToString().Contains(".") ? (int)double.Parse(Attribute3.ToString()) : int.Parse(Attribute3.ToString())));
            }
            if (Attribute4 != null) {
                writer.WritePropertyName("attribute4");
                writer.Write((Attribute4.ToString().Contains(".") ? (int)double.Parse(Attribute4.ToString()) : int.Parse(Attribute4.ToString())));
            }
            if (Attribute5 != null) {
                writer.WritePropertyName("attribute5");
                writer.Write((Attribute5.ToString().Contains(".") ? (int)double.Parse(Attribute5.ToString()) : int.Parse(Attribute5.ToString())));
            }
            if (JoinPolicy != null) {
                writer.WritePropertyName("joinPolicy");
                writer.Write(JoinPolicy.ToString());
            }
            if (CustomRoles != null) {
                writer.WritePropertyName("customRoles");
                writer.WriteArrayStart();
                foreach (var customRole in CustomRoles)
                {
                    if (customRole != null) {
                        customRole.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (GuildMemberDefaultRole != null) {
                writer.WritePropertyName("guildMemberDefaultRole");
                writer.Write(GuildMemberDefaultRole.ToString());
            }
            if (CurrentMaximumMemberCount != null) {
                writer.WritePropertyName("currentMaximumMemberCount");
                writer.Write((CurrentMaximumMemberCount.ToString().Contains(".") ? (int)double.Parse(CurrentMaximumMemberCount.ToString()) : int.Parse(CurrentMaximumMemberCount.ToString())));
            }
            if (Members != null) {
                writer.WritePropertyName("members");
                writer.WriteArrayStart();
                foreach (var member in Members)
                {
                    if (member != null) {
                        member.WriteJson(writer);
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
            var other = obj as Guild;
            var diff = 0;
            if (GuildId == null && GuildId == other.GuildId)
            {
                // null and null
            }
            else
            {
                diff += GuildId.CompareTo(other.GuildId);
            }
            if (GuildModelName == null && GuildModelName == other.GuildModelName)
            {
                // null and null
            }
            else
            {
                diff += GuildModelName.CompareTo(other.GuildModelName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (DisplayName == null && DisplayName == other.DisplayName)
            {
                // null and null
            }
            else
            {
                diff += DisplayName.CompareTo(other.DisplayName);
            }
            if (Attribute1 == null && Attribute1 == other.Attribute1)
            {
                // null and null
            }
            else
            {
                diff += (int)(Attribute1 - other.Attribute1);
            }
            if (Attribute2 == null && Attribute2 == other.Attribute2)
            {
                // null and null
            }
            else
            {
                diff += (int)(Attribute2 - other.Attribute2);
            }
            if (Attribute3 == null && Attribute3 == other.Attribute3)
            {
                // null and null
            }
            else
            {
                diff += (int)(Attribute3 - other.Attribute3);
            }
            if (Attribute4 == null && Attribute4 == other.Attribute4)
            {
                // null and null
            }
            else
            {
                diff += (int)(Attribute4 - other.Attribute4);
            }
            if (Attribute5 == null && Attribute5 == other.Attribute5)
            {
                // null and null
            }
            else
            {
                diff += (int)(Attribute5 - other.Attribute5);
            }
            if (JoinPolicy == null && JoinPolicy == other.JoinPolicy)
            {
                // null and null
            }
            else
            {
                diff += JoinPolicy.CompareTo(other.JoinPolicy);
            }
            if (CustomRoles == null && CustomRoles == other.CustomRoles)
            {
                // null and null
            }
            else
            {
                diff += CustomRoles.Length - other.CustomRoles.Length;
                for (var i = 0; i < CustomRoles.Length; i++)
                {
                    diff += CustomRoles[i].CompareTo(other.CustomRoles[i]);
                }
            }
            if (GuildMemberDefaultRole == null && GuildMemberDefaultRole == other.GuildMemberDefaultRole)
            {
                // null and null
            }
            else
            {
                diff += GuildMemberDefaultRole.CompareTo(other.GuildMemberDefaultRole);
            }
            if (CurrentMaximumMemberCount == null && CurrentMaximumMemberCount == other.CurrentMaximumMemberCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentMaximumMemberCount - other.CurrentMaximumMemberCount);
            }
            if (Members == null && Members == other.Members)
            {
                // null and null
            }
            else
            {
                diff += Members.Length - other.Members.Length;
                for (var i = 0; i < Members.Length; i++)
                {
                    diff += Members[i].CompareTo(other.Members[i]);
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
                if (GuildId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.guildId.error.tooLong"),
                    });
                }
            }
            {
                if (GuildModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.guildModelName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.name.error.tooLong"),
                    });
                }
            }
            {
                if (DisplayName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.displayName.error.tooLong"),
                    });
                }
            }
            {
                if (Attribute1 < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute1.error.invalid"),
                    });
                }
                if (Attribute1 > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute1.error.invalid"),
                    });
                }
            }
            {
                if (Attribute2 < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute2.error.invalid"),
                    });
                }
                if (Attribute2 > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute2.error.invalid"),
                    });
                }
            }
            {
                if (Attribute3 < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute3.error.invalid"),
                    });
                }
                if (Attribute3 > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute3.error.invalid"),
                    });
                }
            }
            {
                if (Attribute4 < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute4.error.invalid"),
                    });
                }
                if (Attribute4 > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute4.error.invalid"),
                    });
                }
            }
            {
                if (Attribute5 < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute5.error.invalid"),
                    });
                }
                if (Attribute5 > 2147483645) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.attribute5.error.invalid"),
                    });
                }
            }
            {
                switch (JoinPolicy) {
                    case "anybody":
                    case "approval":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("guild", "guild.guild.joinPolicy.error.invalid"),
                        });
                }
            }
            {
                if (CustomRoles.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.customRoles.error.tooMany"),
                    });
                }
            }
            {
                if (GuildMemberDefaultRole.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.guildMemberDefaultRole.error.tooLong"),
                    });
                }
            }
            {
                if (CurrentMaximumMemberCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.currentMaximumMemberCount.error.invalid"),
                    });
                }
                if (CurrentMaximumMemberCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.currentMaximumMemberCount.error.invalid"),
                    });
                }
            }
            {
                if (Members.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.members.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guild", "guild.guild.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Guild {
                GuildId = GuildId,
                GuildModelName = GuildModelName,
                Name = Name,
                DisplayName = DisplayName,
                Attribute1 = Attribute1,
                Attribute2 = Attribute2,
                Attribute3 = Attribute3,
                Attribute4 = Attribute4,
                Attribute5 = Attribute5,
                JoinPolicy = JoinPolicy,
                CustomRoles = CustomRoles.Clone() as Gs2.Gs2Guild.Model.RoleModel[],
                GuildMemberDefaultRole = GuildMemberDefaultRole,
                CurrentMaximumMemberCount = CurrentMaximumMemberCount,
                Members = Members.Clone() as Gs2.Gs2Guild.Model.Member[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}