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
	public class ReceiveMemberRequest : IComparable
	{
        public string UserId { set; get; } = null!;
        public string TargetGuildName { set; get; } = null!;
        public ReceiveMemberRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ReceiveMemberRequest WithTargetGuildName(string targetGuildName) {
            this.TargetGuildName = targetGuildName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReceiveMemberRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReceiveMemberRequest()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetGuildName(!data.Keys.Contains("targetGuildName") || data["targetGuildName"] == null ? null : data["targetGuildName"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["targetGuildName"] = TargetGuildName,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ReceiveMemberRequest;
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
            return diff;
        }

        public void Validate() {
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveMemberRequest", "guild.receiveMemberRequest.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TargetGuildName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("receiveMemberRequest", "guild.receiveMemberRequest.targetGuildName.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ReceiveMemberRequest {
                UserId = UserId,
                TargetGuildName = TargetGuildName,
            };
        }
    }
}