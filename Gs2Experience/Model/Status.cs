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

namespace Gs2.Gs2Experience.Model
{

	[Preserve]
	public class Status : IComparable
	{
        public string StatusId { set; get; }
        public string ExperienceName { set; get; }
        public string UserId { set; get; }
        public string PropertyId { set; get; }
        public long? ExperienceValue { set; get; }
        public long? RankValue { set; get; }
        public long? RankCapValue { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Status WithStatusId(string statusId) {
            this.StatusId = statusId;
            return this;
        }

        public Status WithExperienceName(string experienceName) {
            this.ExperienceName = experienceName;
            return this;
        }

        public Status WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Status WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }

        public Status WithExperienceValue(long? experienceValue) {
            this.ExperienceValue = experienceValue;
            return this;
        }

        public Status WithRankValue(long? rankValue) {
            this.RankValue = rankValue;
            return this;
        }

        public Status WithRankCapValue(long? rankCapValue) {
            this.RankCapValue = rankCapValue;
            return this;
        }

        public Status WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Status WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static Status FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Status()
                .WithStatusId(!data.Keys.Contains("statusId") || data["statusId"] == null ? null : data["statusId"].ToString())
                .WithExperienceName(!data.Keys.Contains("experienceName") || data["experienceName"] == null ? null : data["experienceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithExperienceValue(!data.Keys.Contains("experienceValue") || data["experienceValue"] == null ? null : (long?)long.Parse(data["experienceValue"].ToString()))
                .WithRankValue(!data.Keys.Contains("rankValue") || data["rankValue"] == null ? null : (long?)long.Parse(data["rankValue"].ToString()))
                .WithRankCapValue(!data.Keys.Contains("rankCapValue") || data["rankCapValue"] == null ? null : (long?)long.Parse(data["rankCapValue"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["statusId"] = StatusId,
                ["experienceName"] = ExperienceName,
                ["userId"] = UserId,
                ["propertyId"] = PropertyId,
                ["experienceValue"] = ExperienceValue,
                ["rankValue"] = RankValue,
                ["rankCapValue"] = RankCapValue,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StatusId != null) {
                writer.WritePropertyName("statusId");
                writer.Write(StatusId.ToString());
            }
            if (ExperienceName != null) {
                writer.WritePropertyName("experienceName");
                writer.Write(ExperienceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (ExperienceValue != null) {
                writer.WritePropertyName("experienceValue");
                writer.Write(long.Parse(ExperienceValue.ToString()));
            }
            if (RankValue != null) {
                writer.WritePropertyName("rankValue");
                writer.Write(long.Parse(RankValue.ToString()));
            }
            if (RankCapValue != null) {
                writer.WritePropertyName("rankCapValue");
                writer.Write(long.Parse(RankCapValue.ToString()));
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
            var other = obj as Status;
            var diff = 0;
            if (StatusId == null && StatusId == other.StatusId)
            {
                // null and null
            }
            else
            {
                diff += StatusId.CompareTo(other.StatusId);
            }
            if (ExperienceName == null && ExperienceName == other.ExperienceName)
            {
                // null and null
            }
            else
            {
                diff += ExperienceName.CompareTo(other.ExperienceName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (PropertyId == null && PropertyId == other.PropertyId)
            {
                // null and null
            }
            else
            {
                diff += PropertyId.CompareTo(other.PropertyId);
            }
            if (ExperienceValue == null && ExperienceValue == other.ExperienceValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExperienceValue - other.ExperienceValue);
            }
            if (RankValue == null && RankValue == other.RankValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RankValue - other.RankValue);
            }
            if (RankCapValue == null && RankCapValue == other.RankCapValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RankCapValue - other.RankCapValue);
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