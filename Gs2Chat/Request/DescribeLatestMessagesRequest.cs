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
	public class DescribeLatestMessagesRequest : Gs2Request<DescribeLatestMessagesRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RoomName { set; get; } = null!;
         public string Password { set; get; } = null!;
         public int? Category { set; get; } = null!;
         public string AccessToken { set; get; } = null!;
         public string PageToken { set; get; } = null!;
         public int? Limit { set; get; } = null!;
        public DescribeLatestMessagesRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DescribeLatestMessagesRequest WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public DescribeLatestMessagesRequest WithPassword(string password) {
            this.Password = password;
            return this;
        }
        public DescribeLatestMessagesRequest WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public DescribeLatestMessagesRequest WithAccessToken(string accessToken) {
            this.AccessToken = accessToken;
            return this;
        }
        public DescribeLatestMessagesRequest WithPageToken(string pageToken) {
            this.PageToken = pageToken;
            return this;
        }
        public DescribeLatestMessagesRequest WithLimit(int? limit) {
            this.Limit = limit;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DescribeLatestMessagesRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DescribeLatestMessagesRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithPassword(!data.Keys.Contains("password") || data["password"] == null ? null : data["password"].ToString())
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)(data["category"].ToString().Contains(".") ? (int)double.Parse(data["category"].ToString()) : int.Parse(data["category"].ToString())))
                .WithAccessToken(!data.Keys.Contains("accessToken") || data["accessToken"] == null ? null : data["accessToken"].ToString())
                .WithPageToken(!data.Keys.Contains("pageToken") || data["pageToken"] == null ? null : data["pageToken"].ToString())
                .WithLimit(!data.Keys.Contains("limit") || data["limit"] == null ? null : (int?)(data["limit"].ToString().Contains(".") ? (int)double.Parse(data["limit"].ToString()) : int.Parse(data["limit"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["roomName"] = RoomName,
                ["password"] = Password,
                ["category"] = Category,
                ["accessToken"] = AccessToken,
                ["pageToken"] = PageToken,
                ["limit"] = Limit,
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
            if (Password != null) {
                writer.WritePropertyName("password");
                writer.Write(Password.ToString());
            }
            if (Category != null) {
                writer.WritePropertyName("category");
                writer.Write((Category.ToString().Contains(".") ? (int)double.Parse(Category.ToString()) : int.Parse(Category.ToString())));
            }
            if (AccessToken != null) {
                writer.WritePropertyName("accessToken");
                writer.Write(AccessToken.ToString());
            }
            if (PageToken != null) {
                writer.WritePropertyName("pageToken");
                writer.Write(PageToken.ToString());
            }
            if (Limit != null) {
                writer.WritePropertyName("limit");
                writer.Write((Limit.ToString().Contains(".") ? (int)double.Parse(Limit.ToString()) : int.Parse(Limit.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RoomName + ":";
            key += Password + ":";
            key += Category + ":";
            key += AccessToken + ":";
            key += PageToken + ":";
            key += Limit + ":";
            return key;
        }
    }
}