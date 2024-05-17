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
	public class CreateGuildModelMasterRequest : Gs2Request<CreateGuildModelMasterRequest>
	{
         public string NamespaceName { set; get; }
         public string Name { set; get; }
         public string Description { set; get; }
         public string Metadata { set; get; }
         public int? DefaultMaximumMemberCount { set; get; }
         public int? MaximumMemberCount { set; get; }
         public Gs2.Gs2Guild.Model.RoleModel[] Roles { set; get; }
         public string GuildMasterRole { set; get; }
         public string GuildMemberDefaultRole { set; get; }
         public int? RejoinCoolTimeMinutes { set; get; }
        public CreateGuildModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateGuildModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateGuildModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateGuildModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateGuildModelMasterRequest WithDefaultMaximumMemberCount(int? defaultMaximumMemberCount) {
            this.DefaultMaximumMemberCount = defaultMaximumMemberCount;
            return this;
        }
        public CreateGuildModelMasterRequest WithMaximumMemberCount(int? maximumMemberCount) {
            this.MaximumMemberCount = maximumMemberCount;
            return this;
        }
        public CreateGuildModelMasterRequest WithRoles(Gs2.Gs2Guild.Model.RoleModel[] roles) {
            this.Roles = roles;
            return this;
        }
        public CreateGuildModelMasterRequest WithGuildMasterRole(string guildMasterRole) {
            this.GuildMasterRole = guildMasterRole;
            return this;
        }
        public CreateGuildModelMasterRequest WithGuildMemberDefaultRole(string guildMemberDefaultRole) {
            this.GuildMemberDefaultRole = guildMemberDefaultRole;
            return this;
        }
        public CreateGuildModelMasterRequest WithRejoinCoolTimeMinutes(int? rejoinCoolTimeMinutes) {
            this.RejoinCoolTimeMinutes = rejoinCoolTimeMinutes;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateGuildModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateGuildModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultMaximumMemberCount(!data.Keys.Contains("defaultMaximumMemberCount") || data["defaultMaximumMemberCount"] == null ? null : (int?)(data["defaultMaximumMemberCount"].ToString().Contains(".") ? (int)double.Parse(data["defaultMaximumMemberCount"].ToString()) : int.Parse(data["defaultMaximumMemberCount"].ToString())))
                .WithMaximumMemberCount(!data.Keys.Contains("maximumMemberCount") || data["maximumMemberCount"] == null ? null : (int?)(data["maximumMemberCount"].ToString().Contains(".") ? (int)double.Parse(data["maximumMemberCount"].ToString()) : int.Parse(data["maximumMemberCount"].ToString())))
                .WithRoles(!data.Keys.Contains("roles") || data["roles"] == null || !data["roles"].IsArray ? new Gs2.Gs2Guild.Model.RoleModel[]{} : data["roles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.RoleModel.FromJson(v);
                }).ToArray())
                .WithGuildMasterRole(!data.Keys.Contains("guildMasterRole") || data["guildMasterRole"] == null ? null : data["guildMasterRole"].ToString())
                .WithGuildMemberDefaultRole(!data.Keys.Contains("guildMemberDefaultRole") || data["guildMemberDefaultRole"] == null ? null : data["guildMemberDefaultRole"].ToString())
                .WithRejoinCoolTimeMinutes(!data.Keys.Contains("rejoinCoolTimeMinutes") || data["rejoinCoolTimeMinutes"] == null ? null : (int?)(data["rejoinCoolTimeMinutes"].ToString().Contains(".") ? (int)double.Parse(data["rejoinCoolTimeMinutes"].ToString()) : int.Parse(data["rejoinCoolTimeMinutes"].ToString())));
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
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["defaultMaximumMemberCount"] = DefaultMaximumMemberCount,
                ["maximumMemberCount"] = MaximumMemberCount,
                ["roles"] = rolesJsonData,
                ["guildMasterRole"] = GuildMasterRole,
                ["guildMemberDefaultRole"] = GuildMemberDefaultRole,
                ["rejoinCoolTimeMinutes"] = RejoinCoolTimeMinutes,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += DefaultMaximumMemberCount + ":";
            key += MaximumMemberCount + ":";
            key += Roles + ":";
            key += GuildMasterRole + ":";
            key += GuildMemberDefaultRole + ":";
            key += RejoinCoolTimeMinutes + ":";
            return key;
        }
    }
}