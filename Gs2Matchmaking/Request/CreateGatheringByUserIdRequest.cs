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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateGatheringByUserIdRequest : Gs2Request<CreateGatheringByUserIdRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string UserId { set; get; } = null!;
         public Gs2.Gs2Matchmaking.Model.Player Player { set; get; } = null!;
         public Gs2.Gs2Matchmaking.Model.AttributeRange[] AttributeRanges { set; get; } = null!;
         public Gs2.Gs2Matchmaking.Model.CapacityOfRole[] CapacityOfRoles { set; get; } = null!;
         public string[] AllowUserIds { set; get; } = null!;
         public long? ExpiresAt { set; get; } = null!;
         public Gs2.Gs2Matchmaking.Model.TimeSpan_ ExpiresAtTimeSpan { set; get; } = null!;
         public string TimeOffsetToken { set; get; } = null!;
        public string DuplicationAvoider { set; get; } = null!;
        public CreateGatheringByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateGatheringByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CreateGatheringByUserIdRequest WithPlayer(Gs2.Gs2Matchmaking.Model.Player player) {
            this.Player = player;
            return this;
        }
        public CreateGatheringByUserIdRequest WithAttributeRanges(Gs2.Gs2Matchmaking.Model.AttributeRange[] attributeRanges) {
            this.AttributeRanges = attributeRanges;
            return this;
        }
        public CreateGatheringByUserIdRequest WithCapacityOfRoles(Gs2.Gs2Matchmaking.Model.CapacityOfRole[] capacityOfRoles) {
            this.CapacityOfRoles = capacityOfRoles;
            return this;
        }
        public CreateGatheringByUserIdRequest WithAllowUserIds(string[] allowUserIds) {
            this.AllowUserIds = allowUserIds;
            return this;
        }
        public CreateGatheringByUserIdRequest WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }
        public CreateGatheringByUserIdRequest WithExpiresAtTimeSpan(Gs2.Gs2Matchmaking.Model.TimeSpan_ expiresAtTimeSpan) {
            this.ExpiresAtTimeSpan = expiresAtTimeSpan;
            return this;
        }
        public CreateGatheringByUserIdRequest WithTimeOffsetToken(string timeOffsetToken) {
            this.TimeOffsetToken = timeOffsetToken;
            return this;
        }

        public CreateGatheringByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateGatheringByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateGatheringByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPlayer(!data.Keys.Contains("player") || data["player"] == null ? null : Gs2.Gs2Matchmaking.Model.Player.FromJson(data["player"]))
                .WithAttributeRanges(!data.Keys.Contains("attributeRanges") || data["attributeRanges"] == null || !data["attributeRanges"].IsArray ? null : data["attributeRanges"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.AttributeRange.FromJson(v);
                }).ToArray())
                .WithCapacityOfRoles(!data.Keys.Contains("capacityOfRoles") || data["capacityOfRoles"] == null || !data["capacityOfRoles"].IsArray ? null : data["capacityOfRoles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.CapacityOfRole.FromJson(v);
                }).ToArray())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null || !data["allowUserIds"].IsArray ? null : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithExpiresAtTimeSpan(!data.Keys.Contains("expiresAtTimeSpan") || data["expiresAtTimeSpan"] == null ? null : Gs2.Gs2Matchmaking.Model.TimeSpan_.FromJson(data["expiresAtTimeSpan"]))
                .WithTimeOffsetToken(!data.Keys.Contains("timeOffsetToken") || data["timeOffsetToken"] == null ? null : data["timeOffsetToken"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData attributeRangesJsonData = null;
            if (AttributeRanges != null && AttributeRanges.Length > 0)
            {
                attributeRangesJsonData = new JsonData();
                foreach (var attributeRange in AttributeRanges)
                {
                    attributeRangesJsonData.Add(attributeRange.ToJson());
                }
            }
            JsonData capacityOfRolesJsonData = null;
            if (CapacityOfRoles != null && CapacityOfRoles.Length > 0)
            {
                capacityOfRolesJsonData = new JsonData();
                foreach (var capacityOfRole in CapacityOfRoles)
                {
                    capacityOfRolesJsonData.Add(capacityOfRole.ToJson());
                }
            }
            JsonData allowUserIdsJsonData = null;
            if (AllowUserIds != null && AllowUserIds.Length > 0)
            {
                allowUserIdsJsonData = new JsonData();
                foreach (var allowUserId in AllowUserIds)
                {
                    allowUserIdsJsonData.Add(allowUserId);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["player"] = Player?.ToJson(),
                ["attributeRanges"] = attributeRangesJsonData,
                ["capacityOfRoles"] = capacityOfRolesJsonData,
                ["allowUserIds"] = allowUserIdsJsonData,
                ["expiresAt"] = ExpiresAt,
                ["expiresAtTimeSpan"] = ExpiresAtTimeSpan?.ToJson(),
                ["timeOffsetToken"] = TimeOffsetToken,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Player != null) {
                Player.WriteJson(writer);
            }
            if (AttributeRanges != null) {
                writer.WritePropertyName("attributeRanges");
                writer.WriteArrayStart();
                foreach (var attributeRange in AttributeRanges)
                {
                    if (attributeRange != null) {
                        attributeRange.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CapacityOfRoles != null) {
                writer.WritePropertyName("capacityOfRoles");
                writer.WriteArrayStart();
                foreach (var capacityOfRole in CapacityOfRoles)
                {
                    if (capacityOfRole != null) {
                        capacityOfRole.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AllowUserIds != null) {
                writer.WritePropertyName("allowUserIds");
                writer.WriteArrayStart();
                foreach (var allowUserId in AllowUserIds)
                {
                    writer.Write(allowUserId.ToString());
                }
                writer.WriteArrayEnd();
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
            }
            if (ExpiresAtTimeSpan != null) {
                ExpiresAtTimeSpan.WriteJson(writer);
            }
            if (TimeOffsetToken != null) {
                writer.WritePropertyName("timeOffsetToken");
                writer.Write(TimeOffsetToken.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += Player + ":";
            key += AttributeRanges + ":";
            key += CapacityOfRoles + ":";
            key += AllowUserIds + ":";
            key += ExpiresAt + ":";
            key += ExpiresAtTimeSpan + ":";
            key += TimeOffsetToken + ":";
            return key;
        }
    }
}