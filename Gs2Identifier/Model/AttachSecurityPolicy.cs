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
using UnityEngine.Scripting;

namespace Gs2.Gs2Identifier.Model
{

	[Preserve]
	public class AttachSecurityPolicy : IComparable
	{
        public string UserId { set; get; }
        public string[] SecurityPolicyIds { set; get; }
        public long? AttachedAt { set; get; }

        public AttachSecurityPolicy WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public AttachSecurityPolicy WithSecurityPolicyIds(string[] securityPolicyIds) {
            this.SecurityPolicyIds = securityPolicyIds;
            return this;
        }

        public AttachSecurityPolicy WithAttachedAt(long? attachedAt) {
            this.AttachedAt = attachedAt;
            return this;
        }

    	[Preserve]
        public static AttachSecurityPolicy FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AttachSecurityPolicy()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSecurityPolicyIds(!data.Keys.Contains("securityPolicyIds") || data["securityPolicyIds"] == null ? new string[]{} : data["securityPolicyIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithAttachedAt(!data.Keys.Contains("attachedAt") || data["attachedAt"] == null ? null : (long?)long.Parse(data["attachedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["userId"] = UserId,
                ["securityPolicyIds"] = new JsonData(SecurityPolicyIds == null ? new JsonData[]{} :
                        SecurityPolicyIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["attachedAt"] = AttachedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (SecurityPolicyIds != null) {
                writer.WritePropertyName("securityPolicyIds");
                writer.WriteArrayStart();
                foreach (var securityPolicyId in SecurityPolicyIds)
                {
                    if (securityPolicyId != null) {
                        writer.Write(securityPolicyId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AttachedAt != null) {
                writer.WritePropertyName("attachedAt");
                writer.Write(long.Parse(AttachedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AttachSecurityPolicy;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (SecurityPolicyIds == null && SecurityPolicyIds == other.SecurityPolicyIds)
            {
                // null and null
            }
            else
            {
                diff += SecurityPolicyIds.Length - other.SecurityPolicyIds.Length;
                for (var i = 0; i < SecurityPolicyIds.Length; i++)
                {
                    diff += SecurityPolicyIds[i].CompareTo(other.SecurityPolicyIds[i]);
                }
            }
            if (AttachedAt == null && AttachedAt == other.AttachedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(AttachedAt - other.AttachedAt);
            }
            return diff;
        }
    }
}