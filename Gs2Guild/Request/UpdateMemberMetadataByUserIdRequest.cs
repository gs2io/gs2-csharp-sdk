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
	public class UpdateMemberMetadataByUserIdRequest : Gs2Request<UpdateMemberMetadataByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GuildModelName { set; get; } = null!;
         public string GuildName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateMemberMetadataByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateMemberMetadataByUserIdRequest WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public UpdateMemberMetadataByUserIdRequest WithGuildName(string guildName) {
            this.GuildName = guildName;
            return this;
        }
        public UpdateMemberMetadataByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public UpdateMemberMetadataByUserIdRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateMemberMetadataByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public UpdateMemberMetadataByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateMemberMetadataByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateMemberMetadataByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithGuildName(!data.Keys.Contains("guildName") || data["guildName"] == null ? null : data["guildName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["guildModelName"] = GuildModelName,
                ["guildName"] = GuildName,
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["timeOffsetToken"] = TimeOffsetToken,
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
            if (GuildName != null) {
                writer.WritePropertyName("guildName");
                writer.Write(GuildName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GuildModelName + ":";
            key += GuildName + ":";
            key += UserId + ":";
            key += Metadata + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}