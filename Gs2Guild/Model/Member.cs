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

namespace Gs2.Gs2Guild.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Member : IComparable
	{
        public string UserId { set; get; } = null!;
        public string RoleName { set; get; } = null!;
        public long? JoinedAt { set; get; } = null!;
        public Member WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Member WithRoleName(string roleName) {
            this.RoleName = roleName;
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
            return new Member()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRoleName(!data.Keys.Contains("roleName") || data["roleName"] == null ? null : data["roleName"].ToString())
                .WithJoinedAt(!data.Keys.Contains("joinedAt") || data["joinedAt"] == null ? null : (long?)(data["joinedAt"].ToString().Contains(".") ? (long)double.Parse(data["joinedAt"].ToString()) : long.Parse(data["joinedAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["roleName"] = RoleName,
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
            if (JoinedAt != null) {
                writer.WritePropertyName("joinedAt");
                writer.Write((JoinedAt.ToString().Contains(".") ? (long)double.Parse(JoinedAt.ToString()) : long.Parse(JoinedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Member;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RoleName == null && RoleName == other.RoleName)
            {
                // null and null
            }
            else
            {
                diff += RoleName.CompareTo(other.RoleName);
            }
            if (JoinedAt == null && JoinedAt == other.JoinedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(JoinedAt - other.JoinedAt);
            }
            return diff;
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
                JoinedAt = JoinedAt,
            };
        }
    }
}