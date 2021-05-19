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

namespace Gs2.Gs2Matchmaking.Model
{
	[Preserve]
	public class Gathering : IComparable
	{

        /** ギャザリング */
        public string gatheringId { set; get; }

        /**
         * ギャザリングを設定
         *
         * @param gatheringId ギャザリング
         * @return this
         */
        public Gathering WithGatheringId(string gatheringId) {
            this.gatheringId = gatheringId;
            return this;
        }

        /** ギャザリング名 */
        public string name { set; get; }

        /**
         * ギャザリング名を設定
         *
         * @param name ギャザリング名
         * @return this
         */
        public Gathering WithName(string name) {
            this.name = name;
            return this;
        }

        /** 募集条件 */
        public List<AttributeRange> attributeRanges { set; get; }

        /**
         * 募集条件を設定
         *
         * @param attributeRanges 募集条件
         * @return this
         */
        public Gathering WithAttributeRanges(List<AttributeRange> attributeRanges) {
            this.attributeRanges = attributeRanges;
            return this;
        }

        /** 参加者 */
        public List<CapacityOfRole> capacityOfRoles { set; get; }

        /**
         * 参加者を設定
         *
         * @param capacityOfRoles 参加者
         * @return this
         */
        public Gathering WithCapacityOfRoles(List<CapacityOfRole> capacityOfRoles) {
            this.capacityOfRoles = capacityOfRoles;
            return this;
        }

        /** 参加を許可するユーザIDリスト */
        public List<string> allowUserIds { set; get; }

        /**
         * 参加を許可するユーザIDリストを設定
         *
         * @param allowUserIds 参加を許可するユーザIDリスト
         * @return this
         */
        public Gathering WithAllowUserIds(List<string> allowUserIds) {
            this.allowUserIds = allowUserIds;
            return this;
        }

        /** メタデータ */
        public string metadata { set; get; }

        /**
         * メタデータを設定
         *
         * @param metadata メタデータ
         * @return this
         */
        public Gathering WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** ギャザリングの有効期限 */
        public long? expiresAt { set; get; }

        /**
         * ギャザリングの有効期限を設定
         *
         * @param expiresAt ギャザリングの有効期限
         * @return this
         */
        public Gathering WithExpiresAt(long? expiresAt) {
            this.expiresAt = expiresAt;
            return this;
        }

        /** 作成日時 */
        public long? createdAt { set; get; }

        /**
         * 作成日時を設定
         *
         * @param createdAt 作成日時
         * @return this
         */
        public Gathering WithCreatedAt(long? createdAt) {
            this.createdAt = createdAt;
            return this;
        }

        /** 最終更新日時 */
        public long? updatedAt { set; get; }

        /**
         * 最終更新日時を設定
         *
         * @param updatedAt 最終更新日時
         * @return this
         */
        public Gathering WithUpdatedAt(long? updatedAt) {
            this.updatedAt = updatedAt;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.gatheringId != null)
            {
                writer.WritePropertyName("gatheringId");
                writer.Write(this.gatheringId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.attributeRanges != null)
            {
                writer.WritePropertyName("attributeRanges");
                writer.WriteArrayStart();
                foreach(var item in this.attributeRanges)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.capacityOfRoles != null)
            {
                writer.WritePropertyName("capacityOfRoles");
                writer.WriteArrayStart();
                foreach(var item in this.capacityOfRoles)
                {
                    item.WriteJson(writer);
                }
                writer.WriteArrayEnd();
            }
            if(this.allowUserIds != null)
            {
                writer.WritePropertyName("allowUserIds");
                writer.WriteArrayStart();
                foreach(var item in this.allowUserIds)
                {
                    writer.Write(item);
                }
                writer.WriteArrayEnd();
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.expiresAt.HasValue)
            {
                writer.WritePropertyName("expiresAt");
                writer.Write(this.expiresAt.Value);
            }
            if(this.createdAt.HasValue)
            {
                writer.WritePropertyName("createdAt");
                writer.Write(this.createdAt.Value);
            }
            if(this.updatedAt.HasValue)
            {
                writer.WritePropertyName("updatedAt");
                writer.Write(this.updatedAt.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetGatheringNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):matchmaking:(?<namespaceName>.*):gathering:(?<gatheringName>.*)");
        if (!match.Groups["gatheringName"].Success)
        {
            return null;
        }
        return match.Groups["gatheringName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):matchmaking:(?<namespaceName>.*):gathering:(?<gatheringName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):matchmaking:(?<namespaceName>.*):gathering:(?<gatheringName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):matchmaking:(?<namespaceName>.*):gathering:(?<gatheringName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static Gathering FromDict(JsonData data)
        {
            return new Gathering()
                .WithGatheringId(data.Keys.Contains("gatheringId") && data["gatheringId"] != null ? data["gatheringId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithAttributeRanges(data.Keys.Contains("attributeRanges") && data["attributeRanges"] != null ? data["attributeRanges"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Matchmaking.Model.AttributeRange.FromDict(value);
                    }
                ).ToList() : null)
                .WithCapacityOfRoles(data.Keys.Contains("capacityOfRoles") && data["capacityOfRoles"] != null ? data["capacityOfRoles"].Cast<JsonData>().Select(value =>
                    {
                        return Gs2.Gs2Matchmaking.Model.CapacityOfRole.FromDict(value);
                    }
                ).ToList() : null)
                .WithAllowUserIds(data.Keys.Contains("allowUserIds") && data["allowUserIds"] != null ? data["allowUserIds"].Cast<JsonData>().Select(value =>
                    {
                        return value.ToString();
                    }
                ).ToList() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithExpiresAt(data.Keys.Contains("expiresAt") && data["expiresAt"] != null ? (long?)long.Parse(data["expiresAt"].ToString()) : null)
                .WithCreatedAt(data.Keys.Contains("createdAt") && data["createdAt"] != null ? (long?)long.Parse(data["createdAt"].ToString()) : null)
                .WithUpdatedAt(data.Keys.Contains("updatedAt") && data["updatedAt"] != null ? (long?)long.Parse(data["updatedAt"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Gathering;
            var diff = 0;
            if (gatheringId == null && gatheringId == other.gatheringId)
            {
                // null and null
            }
            else
            {
                diff += gatheringId.CompareTo(other.gatheringId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (attributeRanges == null && attributeRanges == other.attributeRanges)
            {
                // null and null
            }
            else
            {
                diff += attributeRanges.Count - other.attributeRanges.Count;
                for (var i = 0; i < attributeRanges.Count; i++)
                {
                    diff += attributeRanges[i].CompareTo(other.attributeRanges[i]);
                }
            }
            if (capacityOfRoles == null && capacityOfRoles == other.capacityOfRoles)
            {
                // null and null
            }
            else
            {
                diff += capacityOfRoles.Count - other.capacityOfRoles.Count;
                for (var i = 0; i < capacityOfRoles.Count; i++)
                {
                    diff += capacityOfRoles[i].CompareTo(other.capacityOfRoles[i]);
                }
            }
            if (allowUserIds == null && allowUserIds == other.allowUserIds)
            {
                // null and null
            }
            else
            {
                diff += allowUserIds.Count - other.allowUserIds.Count;
                for (var i = 0; i < allowUserIds.Count; i++)
                {
                    diff += allowUserIds[i].CompareTo(other.allowUserIds[i]);
                }
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (expiresAt == null && expiresAt == other.expiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(expiresAt - other.expiresAt);
            }
            if (createdAt == null && createdAt == other.createdAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(createdAt - other.createdAt);
            }
            if (updatedAt == null && updatedAt == other.updatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(updatedAt - other.updatedAt);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["gatheringId"] = gatheringId;
            data["name"] = name;
            data["attributeRanges"] = new JsonData(attributeRanges.Select(item => item.ToDict()));
            data["capacityOfRoles"] = new JsonData(capacityOfRoles.Select(item => item.ToDict()));
            data["allowUserIds"] = new JsonData(allowUserIds);
            data["metadata"] = metadata;
            data["expiresAt"] = expiresAt;
            data["createdAt"] = createdAt;
            data["updatedAt"] = updatedAt;
            return data;
        }
	}
}