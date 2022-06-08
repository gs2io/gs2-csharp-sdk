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
	public class PostByUserIdRequest : Gs2Request<PostByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string RoomName { set; get; }
        public string UserId { set; get; }
        public int? Category { set; get; }
        public string Metadata { set; get; }
        public string Password { set; get; }
        public string DuplicationAvoider { set; get; }
        public PostByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PostByUserIdRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public PostByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PostByUserIdRequest WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public PostByUserIdRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public PostByUserIdRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }

        public PostByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PostByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PostByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)int.Parse(data["category"].ToString()))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["roomName"] = RoomName,
                ["userId"] = UserId,
                ["category"] = Category,
                ["metadata"] = Metadata,
                ["password"] = Password,
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
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write(int.Parse(Category.ToString()));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}