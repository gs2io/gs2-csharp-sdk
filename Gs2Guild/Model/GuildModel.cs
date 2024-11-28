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
	public class GuildModel : IComparable
	{
        public string GuildModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? DefaultMaximumMemberCount { set; get; } = null!;
        public int? MaximumMemberCount { set; get; } = null!;
        public int? InactivityPeriodDays { set; get; } = null!;
        public Gs2.Gs2Guild.Model.RoleModel[] Roles { set; get; } = null!;
        public string GuildMasterRole { set; get; } = null!;
        public string GuildMemberDefaultRole { set; get; } = null!;
        public int? RejoinCoolTimeMinutes { set; get; } = null!;
        public int? MaxConcurrentJoinGuilds { set; get; } = null!;
        public int? MaxConcurrentGuildMasterCount { set; get; } = null!;
        public GuildModel WithGuildModelId(string guildModelId) {
            this.GuildModelId = guildModelId;
            return this;
        }
        public GuildModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public GuildModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public GuildModel WithDefaultMaximumMemberCount(int? defaultMaximumMemberCount) {
            this.DefaultMaximumMemberCount = defaultMaximumMemberCount;
            return this;
        }
        public GuildModel WithMaximumMemberCount(int? maximumMemberCount) {
            this.MaximumMemberCount = maximumMemberCount;
            return this;
        }
        public GuildModel WithInactivityPeriodDays(int? inactivityPeriodDays) {
            this.InactivityPeriodDays = inactivityPeriodDays;
            return this;
        }
        public GuildModel WithRoles(Gs2.Gs2Guild.Model.RoleModel[] roles) {
            this.Roles = roles;
            return this;
        }
        public GuildModel WithGuildMasterRole(string guildMasterRole) {
            this.GuildMasterRole = guildMasterRole;
            return this;
        }
        public GuildModel WithGuildMemberDefaultRole(string guildMemberDefaultRole) {
            this.GuildMemberDefaultRole = guildMemberDefaultRole;
            return this;
        }
        public GuildModel WithRejoinCoolTimeMinutes(int? rejoinCoolTimeMinutes) {
            this.RejoinCoolTimeMinutes = rejoinCoolTimeMinutes;
            return this;
        }
        public GuildModel WithMaxConcurrentJoinGuilds(int? maxConcurrentJoinGuilds) {
            this.MaxConcurrentJoinGuilds = maxConcurrentJoinGuilds;
            return this;
        }
        public GuildModel WithMaxConcurrentGuildMasterCount(int? maxConcurrentGuildMasterCount) {
            this.MaxConcurrentGuildMasterCount = maxConcurrentGuildMasterCount;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):model:(?<guildModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):model:(?<guildModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):model:(?<guildModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):guild:(?<namespaceName>.+):model:(?<guildModelName>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GuildModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GuildModel()
                .WithGuildModelId(!data.Keys.Contains("guildModelId") || data["guildModelId"] == null ? null : data["guildModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultMaximumMemberCount(!data.Keys.Contains("defaultMaximumMemberCount") || data["defaultMaximumMemberCount"] == null ? null : (int?)(data["defaultMaximumMemberCount"].ToString().Contains(".") ? (int)double.Parse(data["defaultMaximumMemberCount"].ToString()) : int.Parse(data["defaultMaximumMemberCount"].ToString())))
                .WithMaximumMemberCount(!data.Keys.Contains("maximumMemberCount") || data["maximumMemberCount"] == null ? null : (int?)(data["maximumMemberCount"].ToString().Contains(".") ? (int)double.Parse(data["maximumMemberCount"].ToString()) : int.Parse(data["maximumMemberCount"].ToString())))
                .WithInactivityPeriodDays(!data.Keys.Contains("inactivityPeriodDays") || data["inactivityPeriodDays"] == null ? null : (int?)(data["inactivityPeriodDays"].ToString().Contains(".") ? (int)double.Parse(data["inactivityPeriodDays"].ToString()) : int.Parse(data["inactivityPeriodDays"].ToString())))
                .WithRoles(!data.Keys.Contains("roles") || data["roles"] == null || !data["roles"].IsArray ? null : data["roles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.RoleModel.FromJson(v);
                }).ToArray())
                .WithGuildMasterRole(!data.Keys.Contains("guildMasterRole") || data["guildMasterRole"] == null ? null : data["guildMasterRole"].ToString())
                .WithGuildMemberDefaultRole(!data.Keys.Contains("guildMemberDefaultRole") || data["guildMemberDefaultRole"] == null ? null : data["guildMemberDefaultRole"].ToString())
                .WithRejoinCoolTimeMinutes(!data.Keys.Contains("rejoinCoolTimeMinutes") || data["rejoinCoolTimeMinutes"] == null ? null : (int?)(data["rejoinCoolTimeMinutes"].ToString().Contains(".") ? (int)double.Parse(data["rejoinCoolTimeMinutes"].ToString()) : int.Parse(data["rejoinCoolTimeMinutes"].ToString())))
                .WithMaxConcurrentJoinGuilds(!data.Keys.Contains("maxConcurrentJoinGuilds") || data["maxConcurrentJoinGuilds"] == null ? null : (int?)(data["maxConcurrentJoinGuilds"].ToString().Contains(".") ? (int)double.Parse(data["maxConcurrentJoinGuilds"].ToString()) : int.Parse(data["maxConcurrentJoinGuilds"].ToString())))
                .WithMaxConcurrentGuildMasterCount(!data.Keys.Contains("maxConcurrentGuildMasterCount") || data["maxConcurrentGuildMasterCount"] == null ? null : (int?)(data["maxConcurrentGuildMasterCount"].ToString().Contains(".") ? (int)double.Parse(data["maxConcurrentGuildMasterCount"].ToString()) : int.Parse(data["maxConcurrentGuildMasterCount"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData rolesJsonData = null;
            if (Roles != null && Roles.Length > 0)
            {
                rolesJsonData = new JsonData();
                foreach (var role in Roles)
                {
                    rolesJsonData.Add(role.ToJson());
                }
            }
            return new JsonData {
                ["guildModelId"] = GuildModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["defaultMaximumMemberCount"] = DefaultMaximumMemberCount,
                ["maximumMemberCount"] = MaximumMemberCount,
                ["inactivityPeriodDays"] = InactivityPeriodDays,
                ["roles"] = rolesJsonData,
                ["guildMasterRole"] = GuildMasterRole,
                ["guildMemberDefaultRole"] = GuildMemberDefaultRole,
                ["rejoinCoolTimeMinutes"] = RejoinCoolTimeMinutes,
                ["maxConcurrentJoinGuilds"] = MaxConcurrentJoinGuilds,
                ["maxConcurrentGuildMasterCount"] = MaxConcurrentGuildMasterCount,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GuildModelId != null) {
                writer.WritePropertyName("guildModelId");
                writer.Write(GuildModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (DefaultMaximumMemberCount != null) {
                writer.WritePropertyName("defaultMaximumMemberCount");
                writer.Write((DefaultMaximumMemberCount.ToString().Contains(".") ? (int)double.Parse(DefaultMaximumMemberCount.ToString()) : int.Parse(DefaultMaximumMemberCount.ToString())));
            }
            if (MaximumMemberCount != null) {
                writer.WritePropertyName("maximumMemberCount");
                writer.Write((MaximumMemberCount.ToString().Contains(".") ? (int)double.Parse(MaximumMemberCount.ToString()) : int.Parse(MaximumMemberCount.ToString())));
            }
            if (InactivityPeriodDays != null) {
                writer.WritePropertyName("inactivityPeriodDays");
                writer.Write((InactivityPeriodDays.ToString().Contains(".") ? (int)double.Parse(InactivityPeriodDays.ToString()) : int.Parse(InactivityPeriodDays.ToString())));
            }
            if (Roles != null) {
                writer.WritePropertyName("roles");
                writer.WriteArrayStart();
                foreach (var role in Roles)
                {
                    if (role != null) {
                        role.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (GuildMasterRole != null) {
                writer.WritePropertyName("guildMasterRole");
                writer.Write(GuildMasterRole.ToString());
            }
            if (GuildMemberDefaultRole != null) {
                writer.WritePropertyName("guildMemberDefaultRole");
                writer.Write(GuildMemberDefaultRole.ToString());
            }
            if (RejoinCoolTimeMinutes != null) {
                writer.WritePropertyName("rejoinCoolTimeMinutes");
                writer.Write((RejoinCoolTimeMinutes.ToString().Contains(".") ? (int)double.Parse(RejoinCoolTimeMinutes.ToString()) : int.Parse(RejoinCoolTimeMinutes.ToString())));
            }
            if (MaxConcurrentJoinGuilds != null) {
                writer.WritePropertyName("maxConcurrentJoinGuilds");
                writer.Write((MaxConcurrentJoinGuilds.ToString().Contains(".") ? (int)double.Parse(MaxConcurrentJoinGuilds.ToString()) : int.Parse(MaxConcurrentJoinGuilds.ToString())));
            }
            if (MaxConcurrentGuildMasterCount != null) {
                writer.WritePropertyName("maxConcurrentGuildMasterCount");
                writer.Write((MaxConcurrentGuildMasterCount.ToString().Contains(".") ? (int)double.Parse(MaxConcurrentGuildMasterCount.ToString()) : int.Parse(MaxConcurrentGuildMasterCount.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GuildModel;
            var diff = 0;
            if (GuildModelId == null && GuildModelId == other.GuildModelId)
            {
                // null and null
            }
            else
            {
                diff += GuildModelId.CompareTo(other.GuildModelId);
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
            if (DefaultMaximumMemberCount == null && DefaultMaximumMemberCount == other.DefaultMaximumMemberCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultMaximumMemberCount - other.DefaultMaximumMemberCount);
            }
            if (MaximumMemberCount == null && MaximumMemberCount == other.MaximumMemberCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumMemberCount - other.MaximumMemberCount);
            }
            if (InactivityPeriodDays == null && InactivityPeriodDays == other.InactivityPeriodDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(InactivityPeriodDays - other.InactivityPeriodDays);
            }
            if (Roles == null && Roles == other.Roles)
            {
                // null and null
            }
            else
            {
                diff += Roles.Length - other.Roles.Length;
                for (var i = 0; i < Roles.Length; i++)
                {
                    diff += Roles[i].CompareTo(other.Roles[i]);
                }
            }
            if (GuildMasterRole == null && GuildMasterRole == other.GuildMasterRole)
            {
                // null and null
            }
            else
            {
                diff += GuildMasterRole.CompareTo(other.GuildMasterRole);
            }
            if (GuildMemberDefaultRole == null && GuildMemberDefaultRole == other.GuildMemberDefaultRole)
            {
                // null and null
            }
            else
            {
                diff += GuildMemberDefaultRole.CompareTo(other.GuildMemberDefaultRole);
            }
            if (RejoinCoolTimeMinutes == null && RejoinCoolTimeMinutes == other.RejoinCoolTimeMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(RejoinCoolTimeMinutes - other.RejoinCoolTimeMinutes);
            }
            if (MaxConcurrentJoinGuilds == null && MaxConcurrentJoinGuilds == other.MaxConcurrentJoinGuilds)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxConcurrentJoinGuilds - other.MaxConcurrentJoinGuilds);
            }
            if (MaxConcurrentGuildMasterCount == null && MaxConcurrentGuildMasterCount == other.MaxConcurrentGuildMasterCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxConcurrentGuildMasterCount - other.MaxConcurrentGuildMasterCount);
            }
            return diff;
        }

        public void Validate() {
            {
                if (GuildModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.guildModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (DefaultMaximumMemberCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.defaultMaximumMemberCount.error.invalid"),
                    });
                }
                if (DefaultMaximumMemberCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.defaultMaximumMemberCount.error.invalid"),
                    });
                }
            }
            {
                if (MaximumMemberCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maximumMemberCount.error.invalid"),
                    });
                }
                if (MaximumMemberCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maximumMemberCount.error.invalid"),
                    });
                }
            }
            {
                if (InactivityPeriodDays < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.inactivityPeriodDays.error.invalid"),
                    });
                }
                if (InactivityPeriodDays > 365) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.inactivityPeriodDays.error.invalid"),
                    });
                }
            }
            {
                if (Roles.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.roles.error.tooFew"),
                    });
                }
                if (Roles.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.roles.error.tooMany"),
                    });
                }
            }
            {
                if (GuildMasterRole.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.guildMasterRole.error.tooLong"),
                    });
                }
            }
            {
                if (GuildMemberDefaultRole.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.guildMemberDefaultRole.error.tooLong"),
                    });
                }
            }
            {
                if (RejoinCoolTimeMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.rejoinCoolTimeMinutes.error.invalid"),
                    });
                }
                if (RejoinCoolTimeMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.rejoinCoolTimeMinutes.error.invalid"),
                    });
                }
            }
            {
                if (MaxConcurrentJoinGuilds < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maxConcurrentJoinGuilds.error.invalid"),
                    });
                }
                if (MaxConcurrentJoinGuilds > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maxConcurrentJoinGuilds.error.invalid"),
                    });
                }
            }
            {
                if (MaxConcurrentGuildMasterCount < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maxConcurrentGuildMasterCount.error.invalid"),
                    });
                }
                if (MaxConcurrentGuildMasterCount > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("guildModel", "guild.guildModel.maxConcurrentGuildMasterCount.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new GuildModel {
                GuildModelId = GuildModelId,
                Name = Name,
                Metadata = Metadata,
                DefaultMaximumMemberCount = DefaultMaximumMemberCount,
                MaximumMemberCount = MaximumMemberCount,
                InactivityPeriodDays = InactivityPeriodDays,
                Roles = Roles.Clone() as Gs2.Gs2Guild.Model.RoleModel[],
                GuildMasterRole = GuildMasterRole,
                GuildMemberDefaultRole = GuildMemberDefaultRole,
                RejoinCoolTimeMinutes = RejoinCoolTimeMinutes,
                MaxConcurrentJoinGuilds = MaxConcurrentJoinGuilds,
                MaxConcurrentGuildMasterCount = MaxConcurrentGuildMasterCount,
            };
        }
    }
}