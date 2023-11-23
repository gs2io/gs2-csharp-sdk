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
        public long? Revision { set; get; }
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
        public Gathering WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):gathering:(?<gatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):gathering:(?<gatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):gathering:(?<gatheringName>.+)",
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

        private static System.Text.RegularExpressions.Regex _gatheringNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):gathering:(?<gatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGatheringNameFromGrn(
            string grn
        )
        {
            var match = _gatheringNameRegex.Match(grn);
            if (!match.Success || !match.Groups["gatheringName"].Success)
            {
                return null;
            }
            return match.Groups["gatheringName"].Value;
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
                .WithAttributeRanges(!data.Keys.Contains("attributeRanges") || data["attributeRanges"] == null || !data["attributeRanges"].IsArray ? new Gs2.Gs2Matchmaking.Model.AttributeRange[]{} : data["attributeRanges"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.AttributeRange.FromJson(v);
                }).ToArray())
                .WithCapacityOfRoles(!data.Keys.Contains("capacityOfRoles") || data["capacityOfRoles"] == null || !data["capacityOfRoles"].IsArray ? new Gs2.Gs2Matchmaking.Model.CapacityOfRole[]{} : data["capacityOfRoles"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.CapacityOfRole.FromJson(v);
                }).ToArray())
                .WithAllowUserIds(!data.Keys.Contains("allowUserIds") || data["allowUserIds"] == null || !data["allowUserIds"].IsArray ? new string[]{} : data["allowUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)(data["expiresAt"].ToString().Contains(".") ? (long)double.Parse(data["expiresAt"].ToString()) : long.Parse(data["expiresAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
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
                ["gatheringId"] = GatheringId,
                ["name"] = Name,
                ["attributeRanges"] = attributeRangesJsonData,
                ["capacityOfRoles"] = capacityOfRolesJsonData,
                ["allowUserIds"] = allowUserIdsJsonData,
                ["metadata"] = Metadata,
                ["expiresAt"] = ExpiresAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
                writer.Write((ExpiresAt.ToString().Contains(".") ? (long)double.Parse(ExpiresAt.ToString()) : long.Parse(ExpiresAt.ToString())));
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
    }
}