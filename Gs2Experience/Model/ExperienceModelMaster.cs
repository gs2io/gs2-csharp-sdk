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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ExperienceModelMaster : IComparable
	{
        public string ExperienceModelId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long? DefaultExperience { set; get; }
        public long? DefaultRankCap { set; get; }
        public long? MaxRankCap { set; get; }
        public string RankThresholdName { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public ExperienceModelMaster WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }

        public ExperienceModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }

        public ExperienceModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public ExperienceModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public ExperienceModelMaster WithDefaultExperience(long? defaultExperience) {
            this.DefaultExperience = defaultExperience;
            return this;
        }

        public ExperienceModelMaster WithDefaultRankCap(long? defaultRankCap) {
            this.DefaultRankCap = defaultRankCap;
            return this;
        }

        public ExperienceModelMaster WithMaxRankCap(long? maxRankCap) {
            this.MaxRankCap = maxRankCap;
            return this;
        }

        public ExperienceModelMaster WithRankThresholdName(string rankThresholdName) {
            this.RankThresholdName = rankThresholdName;
            return this;
        }

        public ExperienceModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public ExperienceModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExperienceModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExperienceModelMaster()
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultExperience(!data.Keys.Contains("defaultExperience") || data["defaultExperience"] == null ? null : (long?)long.Parse(data["defaultExperience"].ToString()))
                .WithDefaultRankCap(!data.Keys.Contains("defaultRankCap") || data["defaultRankCap"] == null ? null : (long?)long.Parse(data["defaultRankCap"].ToString()))
                .WithMaxRankCap(!data.Keys.Contains("maxRankCap") || data["maxRankCap"] == null ? null : (long?)long.Parse(data["maxRankCap"].ToString()))
                .WithRankThresholdName(!data.Keys.Contains("rankThresholdName") || data["rankThresholdName"] == null ? null : data["rankThresholdName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["experienceModelId"] = ExperienceModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["defaultExperience"] = DefaultExperience,
                ["defaultRankCap"] = DefaultRankCap,
                ["maxRankCap"] = MaxRankCap,
                ["rankThresholdName"] = RankThresholdName,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (DefaultExperience != null) {
                writer.WritePropertyName("defaultExperience");
                writer.Write(long.Parse(DefaultExperience.ToString()));
            }
            if (DefaultRankCap != null) {
                writer.WritePropertyName("defaultRankCap");
                writer.Write(long.Parse(DefaultRankCap.ToString()));
            }
            if (MaxRankCap != null) {
                writer.WritePropertyName("maxRankCap");
                writer.Write(long.Parse(MaxRankCap.ToString()));
            }
            if (RankThresholdName != null) {
                writer.WritePropertyName("rankThresholdName");
                writer.Write(RankThresholdName.ToString());
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
            var other = obj as ExperienceModelMaster;
            var diff = 0;
            if (ExperienceModelId == null && ExperienceModelId == other.ExperienceModelId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceModelId.CompareTo(other.ExperienceModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (DefaultExperience == null && DefaultExperience == other.DefaultExperience)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultExperience - other.DefaultExperience);
            }
            if (DefaultRankCap == null && DefaultRankCap == other.DefaultRankCap)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultRankCap - other.DefaultRankCap);
            }
            if (MaxRankCap == null && MaxRankCap == other.MaxRankCap)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxRankCap - other.MaxRankCap);
            }
            if (RankThresholdName == null && RankThresholdName == other.RankThresholdName)
            {
                // null and null
            }
            else
            {
                diff += RankThresholdName.CompareTo(other.RankThresholdName);
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