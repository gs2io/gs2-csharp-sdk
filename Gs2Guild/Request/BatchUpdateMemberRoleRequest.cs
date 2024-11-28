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
	public class BatchUpdateMemberRoleRequest : Gs2Request<BatchUpdateMemberRoleRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public Gs2.Gs2Guild.Model.Member[] Members { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public BatchUpdateMemberRoleRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public BatchUpdateMemberRoleRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public BatchUpdateMemberRoleRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public BatchUpdateMemberRoleRequest WithMembers(Gs2.Gs2Guild.Model.Member[] members) {
            this.Members = members;
            return this;
        }

        public BatchUpdateMemberRoleRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BatchUpdateMemberRoleRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BatchUpdateMemberRoleRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithMembers(!data.Keys.Contains("members") || data["members"] == null || !data["members"].IsArray ? null : data["members"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Guild.Model.Member.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
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
                ["namespaceName"] = NamespaceName,
                ["guildModelName"] = GuildModelName,
                ["accessToken"] = AccessToken,
                ["members"] = membersJsonData,
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildModelName + ":";
            key += AccessToken + ":";
            key += Members + ":";
            return key;
        }
    }
}