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
using Gs2.Gs2Chat.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Chat.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateRoomFromBackendRequest : Gs2Request<CreateRoomFromBackendRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Password { set; get; } = null!;
         public string[] WhiteListUserIds { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public CreateRoomFromBackendRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateRoomFromBackendRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateRoomFromBackendRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CreateRoomFromBackendRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateRoomFromBackendRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public CreateRoomFromBackendRequest WithWhiteListUserIds(string[] whiteListUserIds) {
            this.WhiteListUserIds = whiteListUserIds;
            return this;
        }
        public CreateRoomFromBackendRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public CreateRoomFromBackendRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateRoomFromBackendRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateRoomFromBackendRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithWhiteListUserIds(!data.Keys.Contains("whiteListUserIds") || data["whiteListUserIds"] == null || !data["whiteListUserIds"].IsArray ? null : data["whiteListUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData whiteListUserIdsJsonData = null;
            if (WhiteListUserIds != null && WhiteListUserIds.Length > 0)
            {
                whiteListUserIdsJsonData = new JsonData();
                foreach (var whiteListUserId in WhiteListUserIds)
                {
                    whiteListUserIdsJsonData.Add(whiteListUserId);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["userId"] = UserId,
                ["metadata"] = Metadata,
                ["password"] = Password,
                ["whiteListUserIds"] = whiteListUserIdsJsonData,
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
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (WhiteListUserIds != null) {
                writer.WritePropertyName("whiteListUserIds");
                writer.WriteArrayStart();
                foreach (var whiteListUserId in WhiteListUserIds)
                {
                    writer.Write(whiteListUserId.ToString());
                }
                writer.WriteArrayEnd();
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
            key += Name + ":";
            key += UserId + ":";
            key += Metadata + ":";
            key += Password + ":";
            key += WhiteListUserIds + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}