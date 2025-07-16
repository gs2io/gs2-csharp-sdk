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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SendMemberRequest : IComparable
	{
        public string UserId { set; get; }
        public string TargetGuildName { set; get; }
        public string Metadata { set; get; }
        public long? CreatedAt { set; get; }
        public SendMemberRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SendMemberRequest WithTargetGuildName(string targetGuildName) {
            this.TargetGuildName = targetGuildName;
            return this;
        }
        public SendMemberRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SendMemberRequest WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SendMemberRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SendMemberRequest()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetGuildName(!data.Keys.Contains("targetGuildName") || data["targetGuildName"] == null ? null : data["targetGuildName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["targetGuildName"] = TargetGuildName,
                ["metadata"] = Metadata,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TargetGuildName != null) {
                writer.WritePropertyName("targetGuildName");
                writer.Write(TargetGuildName.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SendMemberRequest;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TargetGuildName == null && TargetGuildName == other.TargetGuildName)
            {
                // null and null
            }
            else
            {
                diff += TargetGuildName.CompareTo(other.TargetGuildName);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendMemberRequest", "guild.sendMemberRequest.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetGuildName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendMemberRequest", "guild.sendMemberRequest.targetGuildName.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendMemberRequest", "guild.sendMemberRequest.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendMemberRequest", "guild.sendMemberRequest.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("sendMemberRequest", "guild.sendMemberRequest.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SendMemberRequest {
                UserId = UserId,
                TargetGuildName = TargetGuildName,
                Metadata = Metadata,
                CreatedAt = CreatedAt,
            };
        }
    }
}