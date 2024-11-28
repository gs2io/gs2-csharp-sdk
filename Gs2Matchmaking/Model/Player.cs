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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Player : IComparable
	{
        public string UserId { set; get; } = null!;
        public Gs2.Gs2Matchmaking.Model.Attribute_[] Attributes { set; get; } = null!;
        public string RoleName { set; get; } = null!;
        public string[] DenyUserIds { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public Player WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Player WithAttributes(Gs2.Gs2Matchmaking.Model.Attribute_[] attributes) {
            this.Attributes = attributes;
            return this;
        }
        public Player WithRoleName(string roleName) {
            this.RoleName = roleName;
            return this;
        }
        public Player WithDenyUserIds(string[] denyUserIds) {
            this.DenyUserIds = denyUserIds;
            return this;
        }
        public Player WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Player FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Player()
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAttributes(!data.Keys.Contains("attributes") || data["attributes"] == null || !data["attributes"].IsArray ? null : data["attributes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.Attribute_.FromJson(v);
                }).ToArray())
                .WithRoleName(!data.Keys.Contains("roleName") || data["roleName"] == null ? null : data["roleName"].ToString())
                .WithDenyUserIds(!data.Keys.Contains("denyUserIds") || data["denyUserIds"] == null || !data["denyUserIds"].IsArray ? null : data["denyUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData attributesJsonData = null;
            if (Attributes != null && Attributes.Length > 0)
            {
                attributesJsonData = new JsonData();
                foreach (var attribute in Attributes)
                {
                    attributesJsonData.Add(attribute.ToJson());
                }
            }
            JsonData denyUserIdsJsonData = null;
            if (DenyUserIds != null && DenyUserIds.Length > 0)
            {
                denyUserIdsJsonData = new JsonData();
                foreach (var denyUserId in DenyUserIds)
                {
                    denyUserIdsJsonData.Add(denyUserId);
                }
            }
            return new JsonData {
                ["userId"] = UserId,
                ["attributes"] = attributesJsonData,
                ["roleName"] = RoleName,
                ["denyUserIds"] = denyUserIdsJsonData,
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
            if (Attributes != null) {
                writer.WritePropertyName("attributes");
                writer.WriteArrayStart();
                foreach (var attribute in Attributes)
                {
                    if (attribute != null) {
                        attribute.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (RoleName != null) {
                writer.WritePropertyName("roleName");
                writer.Write(RoleName.ToString());
            }
            if (DenyUserIds != null) {
                writer.WritePropertyName("denyUserIds");
                writer.WriteArrayStart();
                foreach (var denyUserId in DenyUserIds)
                {
                    if (denyUserId != null) {
                        writer.Write(denyUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Player;
            var diff = 0;
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Attributes == null && Attributes == other.Attributes)
            {
                // null and null
            }
            else
            {
                diff += Attributes.Length - other.Attributes.Length;
                for (var i = 0; i < Attributes.Length; i++)
                {
                    diff += Attributes[i].CompareTo(other.Attributes[i]);
                }
            }
            if (RoleName == null && RoleName == other.RoleName)
            {
                // null and null
            }
            else
            {
                diff += RoleName.CompareTo(other.RoleName);
            }
            if (DenyUserIds == null && DenyUserIds == other.DenyUserIds)
            {
                // null and null
            }
            else
            {
                diff += DenyUserIds.Length - other.DenyUserIds.Length;
                for (var i = 0; i < DenyUserIds.Length; i++)
                {
                    diff += DenyUserIds[i].CompareTo(other.DenyUserIds[i]);
                }
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
                        new RequestError("player", "matchmaking.player.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Attributes.Length > 5) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("player", "matchmaking.player.attributes.error.tooMany"),
                    });
                }
            }
            {
                if (RoleName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("player", "matchmaking.player.roleName.error.tooLong"),
                    });
                }
            }
            {
                if (DenyUserIds.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("player", "matchmaking.player.denyUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("player", "matchmaking.player.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("player", "matchmaking.player.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Player {
                UserId = UserId,
                Attributes = Attributes.Clone() as Gs2.Gs2Matchmaking.Model.Attribute_[],
                RoleName = RoleName,
                DenyUserIds = DenyUserIds.Clone() as string[],
                CreatedAt = CreatedAt,
            };
        }
    }
}