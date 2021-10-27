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
	public class Gathering : IComparable
	{
        public string GatheringId { set; get; }
        public string Name { set; get; }
        public Gs2.Gs2Matchmaking.Model.AttributeRange[] AttributeRanges { set; get; }
        public Gs2.Gs2Matchmaking.Model.CapacityOfRole[] CapacityOfRoles { set; get; }
        public string[] AllowUserIds { set; get; }
        public string Metadata { set; get; }
        public long? ExpiresAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Gathering WithGatheringId(string gatheringId) {
            this.GatheringId = gatheringId;
            return this;
        }

        public Gathering WithName(string name) {
            this.Name = name;
            return this;
        }

        public Gathering WithAttributeRanges(Gs2.Gs2Matchmaking.Model.AttributeRange[] attributeRanges) {
            this.AttributeRanges = attributeRanges;
            return this;
        }

        public Gathering WithCapacityOfRoles(Gs2.Gs2Matchmaking.Model.CapacityOfRole[] capacityOfRoles) {
            this.CapacityOfRoles = capacityOfRoles;
            return this;
        }

        public Gathering WithAllowUserIds(string[] allowUserIds) {
            this.AllowUserIds = allowUserIds;
            return this;
        }

        public Gathering WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public Gathering WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }

        public Gathering WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Gathering WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Gathering FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Gathering()
                .WithGatheringId(!data.Keys.Contains("gatheringId") || data["gatheringId"] == null ? null : data["gatheringId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithAttributeRanges(!data.Keys.Contains("attributeRanges") || data["attributeRanges"] == null ? new Gs2.Gs2Matchmaking.Model.AttributeRange[]{} : data["attributeRanges"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.AttributeRange.FromJson(v);
                }).ToArray())
                .WithCapacityOfRoles(!data.Keys.Contains("capacityOfRoles") || data["capacityOfRoles"] == null ? new Gs2.Gs2Matchmaking.Model.CapacityOfRole[]{} : data["capacityOfRoles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.CapacityOfRole.FromJson(v);
                }).ToArray())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null ? new string[]{} : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)long.Parse(data["expiresAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["gatheringId"] = GatheringId,
                ["name"] = Name,
                ["attributeRanges"] = new JsonData(AttributeRanges == null ? new JsonData[]{} :
                        AttributeRanges.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["capacityOfRoles"] = new JsonData(CapacityOfRoles == null ? new JsonData[]{} :
                        CapacityOfRoles.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["allowUserIds"] = new JsonData(AllowUserIds == null ? new JsonData[]{} :
                        AllowUserIds.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["metadata"] = Metadata,
                ["expiresAt"] = ExpiresAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GatheringId != null) {
                writer.WritePropertyName("gatheringId");
                writer.Write(GatheringId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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
                    if (allowUserId != null) {
                        writer.Write(allowUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write(long.Parse(ExpiresAt.ToString()));
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
            var other = obj as Gathering;
            var diff = 0;
            if (GatheringId == null && GatheringId == other.GatheringId)
            {
                // null and null
            }
            else
            {
                diff += GatheringId.CompareTo(other.GatheringId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (AttributeRanges == null && AttributeRanges == other.AttributeRanges)
            {
                // null and null
            }
            else
            {
                diff += AttributeRanges.Length - other.AttributeRanges.Length;
                for (var i = 0; i < AttributeRanges.Length; i++)
                {
                    diff += AttributeRanges[i].CompareTo(other.AttributeRanges[i]);
                }
            }
            if (CapacityOfRoles == null && CapacityOfRoles == other.CapacityOfRoles)
            {
                // null and null
            }
            else
            {
                diff += CapacityOfRoles.Length - other.CapacityOfRoles.Length;
                for (var i = 0; i < CapacityOfRoles.Length; i++)
                {
                    diff += CapacityOfRoles[i].CompareTo(other.CapacityOfRoles[i]);
                }
            }
            if (AllowUserIds == null && AllowUserIds == other.AllowUserIds)
            {
                // null and null
            }
            else
            {
                diff += AllowUserIds.Length - other.AllowUserIds.Length;
                for (var i = 0; i < AllowUserIds.Length; i++)
                {
                    diff += AllowUserIds[i].CompareTo(other.AllowUserIds[i]);
                }
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
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