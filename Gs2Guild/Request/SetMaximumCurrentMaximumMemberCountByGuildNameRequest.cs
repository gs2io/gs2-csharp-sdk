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
	public class SetMaximumCurrentMaximumMemberCountByGuildNameRequest : Gs2Request<SetMaximumCurrentMaximumMemberCountByGuildNameRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public int? Value { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public SetMaximumCurrentMaximumMemberCountByGuildNameRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public SetMaximumCurrentMaximumMemberCountByGuildNameRequest WithGuildName(string guildName) {
            this.GuildName = guildName;
            return this;
        }
        public SetMaximumCurrentMaximumMemberCountByGuildNameRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public SetMaximumCurrentMaximumMemberCountByGuildNameRequest WithValue(int? value) {
            this.Value = value;
            return this;
        }

        public SetMaximumCurrentMaximumMemberCountByGuildNameRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SetMaximumCurrentMaximumMemberCountByGuildNameRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SetMaximumCurrentMaximumMemberCountByGuildNameRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildName(!data.Keys.Contains("guildName") || data["guildName"] == null ? null : data["guildName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : (int?)(data["value"].ToString().Contains(".") ? (int)double.Parse(data["value"].ToString()) : int.Parse(data["value"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["guildName"] = GuildName,
                ["guildModelName"] = GuildModelName,
                ["value"] = Value,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GuildName != null) {
                writer.WritePropertyName("guildName");
                writer.Write(GuildName.ToString());
            }
            if (GuildModelName != null) {
                writer.WritePropertyName("guildModelName");
                writer.Write(GuildModelName.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write((Value.ToString().Contains(".") ? (int)double.Parse(Value.ToString()) : int.Parse(Value.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildName + ":";
            key += GuildModelName + ":";
            key += Value + ":";
            return key;
        }
    }
}