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
	public class UpdateRoomRequest : Gs2Request<UpdateRoomRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RoomName { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Password { set; get; } = null!;
         public string[] WhiteListUserIds { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public UpdateRoomRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRoomRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public UpdateRoomRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRoomRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public UpdateRoomRequest WithWhiteListUserIds(string[] whiteListUserIds) {
            this.WhiteListUserIds = whiteListUserIds;
            return this;
        }
        public UpdateRoomRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }

        public UpdateRoomRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRoomRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRoomRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithWhiteListUserIds(!data.Keys.Contains("whiteListUserIds") || data["whiteListUserIds"] == null || !data["whiteListUserIds"].IsArray ? null : data["whiteListUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString());
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
                ["roomName"] = RoomName,
                ["metadata"] = Metadata,
                ["password"] = Password,
                ["whiteListUserIds"] = whiteListUserIdsJsonData,
                ["accessToken"] = AccessToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RoomName != null) {
                writer.WritePropertyName("roomName");
                writer.Write(RoomName.ToString());
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
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RoomName + ":";
            key += Metadata + ":";
            key += Password + ":";
            key += WhiteListUserIds + ":";
            key += AccessToken + ":";
            return key;
        }
    }
}