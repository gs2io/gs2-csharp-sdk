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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Guild.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateGuildModelMasterRequest : Gs2Request<UpdateGuildModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string Description { set; get; } = null!;
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
        public UpdateGuildModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateGuildModelMasterRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public UpdateGuildModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateGuildModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateGuildModelMasterRequest WithDefaultMaximumMemberCount(int? defaultMaximumMemberCount) {
            this.DefaultMaximumMemberCount = defaultMaximumMemberCount;
            return this;
        }
        public UpdateGuildModelMasterRequest WithMaximumMemberCount(int? maximumMemberCount) {
            this.MaximumMemberCount = maximumMemberCount;
            return this;
        }
        public UpdateGuildModelMasterRequest WithInactivityPeriodDays(int? inactivityPeriodDays) {
            this.InactivityPeriodDays = inactivityPeriodDays;
            return this;
        }
        public UpdateGuildModelMasterRequest WithRoles(Gs2.Gs2Guild.Model.RoleModel[] roles) {
            this.Roles = roles;
            return this;
        }
        public UpdateGuildModelMasterRequest WithGuildMasterRole(string guildMasterRole) {
            this.GuildMasterRole = guildMasterRole;
            return this;
        }
        public UpdateGuildModelMasterRequest WithGuildMemberDefaultRole(string guildMemberDefaultRole) {
            this.GuildMemberDefaultRole = guildMemberDefaultRole;
            return this;
        }
        public UpdateGuildModelMasterRequest WithRejoinCoolTimeMinutes(int? rejoinCoolTimeMinutes) {
            this.RejoinCoolTimeMinutes = rejoinCoolTimeMinutes;
            return this;
        }
        public UpdateGuildModelMasterRequest WithMaxConcurrentJoinGuilds(int? maxConcurrentJoinGuilds) {
            this.MaxConcurrentJoinGuilds = maxConcurrentJoinGuilds;
            return this;
        }
        public UpdateGuildModelMasterRequest WithMaxConcurrentGuildMasterCount(int? maxConcurrentGuildMasterCount) {
            this.MaxConcurrentGuildMasterCount = maxConcurrentGuildMasterCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateGuildModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateGuildModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
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

        public override JsonData ToJson()
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
                ["namespaceName"] = NamespaceName,
                ["guildModelName"] = GuildModelName,
                ["description"] = Description,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildModelName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += DefaultMaximumMemberCount + ":";
            key += MaximumMemberCount + ":";
            key += InactivityPeriodDays + ":";
            key += Roles + ":";
            key += GuildMasterRole + ":";
            key += GuildMemberDefaultRole + ":";
            key += RejoinCoolTimeMinutes + ":";
            key += MaxConcurrentJoinGuilds + ":";
            key += MaxConcurrentGuildMasterCount + ":";
            return key;
        }
    }
}