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
	public class UpdateMemberRoleRequest : Gs2Request<UpdateMemberRoleRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string TargetUserId { set; get; } = null!;
         public string RoleName { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateMemberRoleRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateMemberRoleRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public UpdateMemberRoleRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public UpdateMemberRoleRequest WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }
        public UpdateMemberRoleRequest WithRoleName(string roleName) {
            this.RoleName = roleName;
            return this;
        }

        public UpdateMemberRoleRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateMemberRoleRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateMemberRoleRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString())
                .WithRoleName(!data.Keys.Contains("roleName") || data["roleName"] == null ? null : data["roleName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["guildModelName"] = GuildModelName,
                ["accessToken"] = AccessToken,
                ["targetUserId"] = TargetUserId,
                ["roleName"] = RoleName,
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
            if (TargetUserId != null) {
                writer.WritePropertyName("targetUserId");
                writer.Write(TargetUserId.ToString());
            }
            if (RoleName != null) {
                writer.WritePropertyName("roleName");
                writer.Write(RoleName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildModelName + ":";
            key += AccessToken + ":";
            key += TargetUserId + ":";
            key += RoleName + ":";
            return key;
        }
    }
}