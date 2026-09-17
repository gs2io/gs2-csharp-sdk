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
	public partial class Member : IComparable
	{
        public string UserId { set; get; }
        public string RoleName { set; get; }
        public string Metadata { set; get; }
        public long? JoinedAt { set; get; }
        public Member WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Member WithRoleName(string roleName) {
            this.RoleName = roleName;
            return this;
        }
        public Member WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public Member WithJoinedAt(long? joinedAt) {
            this.JoinedAt = joinedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Member FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Member()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRoleName(!data.Keys.Contains("roleName") || data["roleName"] == null ? null : data["roleName"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithJoinedAt(!data.Keys.Contains("joinedAt") || data["joinedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["joinedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["roleName"] = RoleName,
                ["metadata"] = Metadata,
                ["joinedAt"] = JoinedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RoleName != null) {
                writer.WritePropertyName("roleName");
                writer.Write(RoleName.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (JoinedAt != null) {
                writer.WritePropertyName("joinedAt");
                writer.Write((JoinedAt.ToString().Contains(".") ? (long)double.Parse(JoinedAt.ToString()) : long.Parse(JoinedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Member;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Member.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RoleName, other.RoleName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(JoinedAt, other.JoinedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("member", "guild.member.userId.error.tooLong"),
                    });
                }
            }
            {
                if (RoleName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("member", "guild.member.roleName.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("member", "guild.member.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (JoinedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("member", "guild.member.joinedAt.error.invalid"),
                    });
                }
                if (JoinedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("member", "guild.member.joinedAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Member {
                UserId = UserId,
                RoleName = RoleName,
                Metadata = Metadata,
                JoinedAt = JoinedAt,
            };
        }
    }
}