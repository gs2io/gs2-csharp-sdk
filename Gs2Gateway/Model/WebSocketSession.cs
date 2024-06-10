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
        public string WebSocketSessionId { set; get; } = null!;
        public string ConnectionId { set; get; } = null!;
        public string NamespaceName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public WebSocketSession WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):session:(?<connectionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):session:(?<connectionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):session:(?<connectionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):session:(?<connectionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _connectionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):gateway:(?<namespaceName>.+):user:(?<userId>.+):session:(?<connectionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetConnectionIdFromGrn(
            string grn
        )
        {
            var match = _connectionIdRegex.Match(grn);
            if (!match.Success || !match.Groups["connectionId"].Success)
            {
                return null;
            }
            return match.Groups["connectionId"].Value;
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
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["revision"] = Revision,
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
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (WebSocketSessionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.webSocketSessionId.error.tooLong"),
                    });
                }
            }
            {
                if (ConnectionId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.connectionId.error.tooLong"),
                    });
                }
            }
            {
                if (NamespaceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.namespaceName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.userId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("webSocketSession", "gateway.webSocketSession.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new WebSocketSession {
                WebSocketSessionId = WebSocketSessionId,
                ConnectionId = ConnectionId,
                NamespaceName = NamespaceName,
                UserId = UserId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}