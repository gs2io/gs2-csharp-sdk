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
	public class CreateGuildRequest : Gs2Request<CreateGuildRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string DisplayName { set; get; } = null!;
         public int? Attribute1 { set; get; } = null!;
         public int? Attribute2 { set; get; } = null!;
         public int? Attribute3 { set; get; } = null!;
         public int? Attribute4 { set; get; } = null!;
         public int? Attribute5 { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string MemberMetadata { set; get; } = null!;
         public string JoinPolicy { set; get; } = null!;
         public Gs2.Gs2Guild.Model.RoleModel[] CustomRoles { set; get; } = null!;
         public string GuildMemberDefaultRole { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public CreateGuildRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateGuildRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public CreateGuildRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public CreateGuildRequest WithDisplayName(string displayName) {
            this.DisplayName = displayName;
            return this;
        }
        public CreateGuildRequest WithAttribute1(int? attribute1) {
            this.Attribute1 = attribute1;
            return this;
        }
        public CreateGuildRequest WithAttribute2(int? attribute2) {
            this.Attribute2 = attribute2;
            return this;
        }
        public CreateGuildRequest WithAttribute3(int? attribute3) {
            this.Attribute3 = attribute3;
            return this;
        }
        public CreateGuildRequest WithAttribute4(int? attribute4) {
            this.Attribute4 = attribute4;
            return this;
        }
        public CreateGuildRequest WithAttribute5(int? attribute5) {
            this.Attribute5 = attribute5;
            return this;
        }
        public CreateGuildRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateGuildRequest WithMemberMetadata(string memberMetadata) {
            this.MemberMetadata = memberMetadata;
            return this;
        }
        public CreateGuildRequest WithJoinPolicy(string joinPolicy) {
            this.JoinPolicy = joinPolicy;
            return this;
        }
        public CreateGuildRequest WithCustomRoles(Gs2.Gs2Guild.Model.RoleModel[] customRoles) {
            this.CustomRoles = customRoles;
            return this;
        }
        public CreateGuildRequest WithGuildMemberDefaultRole(string guildMemberDefaultRole) {
            this.GuildMemberDefaultRole = guildMemberDefaultRole;
            return this;
        }

        public CreateGuildRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateGuildRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateGuildRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithDisplayName(!data.Keys.Contains("displayName") || data["displayName"] == null ? null : data["displayName"].ToString())
                .WithAttribute1(!data.Keys.Contains("attribute1") || data["attribute1"] == null ? null : (int?)(data["attribute1"].ToString().Contains(".") ? (int)double.Parse(data["attribute1"].ToString()) : int.Parse(data["attribute1"].ToString())))
                .WithAttribute2(!data.Keys.Contains("attribute2") || data["attribute2"] == null ? null : (int?)(data["attribute2"].ToString().Contains(".") ? (int)double.Parse(data["attribute2"].ToString()) : int.Parse(data["attribute2"].ToString())))
                .WithAttribute3(!data.Keys.Contains("attribute3") || data["attribute3"] == null ? null : (int?)(data["attribute3"].ToString().Contains(".") ? (int)double.Parse(data["attribute3"].ToString()) : int.Parse(data["attribute3"].ToString())))
                .WithAttribute4(!data.Keys.Contains("attribute4") || data["attribute4"] == null ? null : (int?)(data["attribute4"].ToString().Contains(".") ? (int)double.Parse(data["attribute4"].ToString()) : int.Parse(data["attribute4"].ToString())))
                .WithAttribute5(!data.Keys.Contains("attribute5") || data["attribute5"] == null ? null : (int?)(data["attribute5"].ToString().Contains(".") ? (int)double.Parse(data["attribute5"].ToString()) : int.Parse(data["attribute5"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMemberMetadata(!data.Keys.Contains("memberMetadata") || data["memberMetadata"] == null ? null : data["memberMetadata"].ToString())
                .WithJoinPolicy(!data.Keys.Contains("joinPolicy") || data["joinPolicy"] == null ? null : data["joinPolicy"].ToString())
                .WithCustomRoles(!data.Keys.Contains("customRoles") || data["customRoles"] == null || !data["customRoles"].IsArray ? null : data["customRoles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.RoleModel.FromJson(v);
                }).ToArray())
                .WithGuildMemberDefaultRole(!data.Keys.Contains("guildMemberDefaultRole") || data["guildMemberDefaultRole"] == null ? null : data["guildMemberDefaultRole"].ToString());
        }

        public override JsonData ToJson()
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
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["accessToken"] = AccessToken,
                ["guildModelName"] = GuildModelName,
                ["displayName"] = DisplayName,
                ["attribute1"] = Attribute1,
                ["attribute2"] = Attribute2,
                ["attribute3"] = Attribute3,
                ["attribute4"] = Attribute4,
                ["attribute5"] = Attribute5,
                ["metadata"] = Metadata,
                ["memberMetadata"] = MemberMetadata,
                ["joinPolicy"] = JoinPolicy,
                ["customRoles"] = customRolesJsonData,
                ["guildMemberDefaultRole"] = GuildMemberDefaultRole,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
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
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MemberMetadata != null) {
                writer.WritePropertyName("memberMetadata");
                writer.Write(MemberMetadata.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += AccessToken + ":";
            key += GuildModelName + ":";
            key += DisplayName + ":";
            key += Attribute1 + ":";
            key += Attribute2 + ":";
            key += Attribute3 + ":";
            key += Attribute4 + ":";
            key += Attribute5 + ":";
            key += Metadata + ":";
            key += MemberMetadata + ":";
            key += JoinPolicy + ":";
            key += CustomRoles + ":";
            key += GuildMemberDefaultRole + ":";
            return key;
        }
    }
}