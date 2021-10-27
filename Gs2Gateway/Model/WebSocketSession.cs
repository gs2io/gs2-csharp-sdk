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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Gateway.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class WebSocketSession : IComparable
	{
        public string WebSocketSessionId { set; get; }
        public string ConnectionId { set; get; }
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public WebSocketSession WithWebSocketSessionId(string webSocketSessionId) {
            this.WebSocketSessionId = webSocketSessionId;
            return this;
        }

        public WebSocketSession WithConnectionId(string connectionId) {
            this.ConnectionId = connectionId;
            return this;
        }

        public WebSocketSession WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public WebSocketSession WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public WebSocketSession WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public WebSocketSession WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static WebSocketSession FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new WebSocketSession()
                .WithWebSocketSessionId(!data.Keys.Contains("webSocketSessionId") || data["webSocketSessionId"] == null ? null : data["webSocketSessionId"].ToString())
                .WithConnectionId(!data.Keys.Contains("connectionId") || data["connectionId"] == null ? null : data["connectionId"].ToString())
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["webSocketSessionId"] = WebSocketSessionId,
                ["connectionId"] = ConnectionId,
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (WebSocketSessionId != null) {
                writer.WritePropertyName("webSocketSessionId");
                writer.Write(WebSocketSessionId.ToString());
            }
            if (ConnectionId != null) {
                writer.WritePropertyName("connectionId");
                writer.Write(ConnectionId.ToString());
            }
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as WebSocketSession;
            var diff = 0;
            if (WebSocketSessionId == null && WebSocketSessionId == other.WebSocketSessionId)
            {
                // null and null
            }
            else
            {
                diff += WebSocketSessionId.CompareTo(other.WebSocketSessionId);
            }
            if (ConnectionId == null && ConnectionId == other.ConnectionId)
            {
                // null and null
            }
            else
            {
                diff += ConnectionId.CompareTo(other.ConnectionId);
            }
            if (NamespaceName == null && NamespaceName == other.NamespaceName)
            {
                // null and null
            }
            else
            {
                diff += NamespaceName.CompareTo(other.NamespaceName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}