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

namespace Gs2.Gs2Ranking.Model
{

	[Preserve]
	public class Score : IComparable
	{
        public string ScoreId { set; get; }
        public string CategoryName { set; get; }
        public string UserId { set; get; }
        public string UniqueId { set; get; }
        public string ScorerUserId { set; get; }
        public long? Value { set; get; }
        public string Metadata { set; get; }
        public long? CreatedAt { set; get; }

        public Score WithScoreId(string scoreId) {
            this.ScoreId = scoreId;
            return this;
        }

        public Score WithCategoryName(string categoryName) {
            this.CategoryName = categoryName;
            return this;
        }

        public Score WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Score WithUniqueId(string uniqueId) {
            this.UniqueId = uniqueId;
            return this;
        }

        public Score WithScorerUserId(string scorerUserId) {
            this.ScorerUserId = scorerUserId;
            return this;
        }

        public Score WithValue(long? value) {
            this.Value = value;
            return this;
        }

        public Score WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public Score WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

    	[Preserve]
        public static Score FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Score()
                .WithScoreId(!data.Keys.Contains("scoreId") || data["scoreId"] == null ? null : data["scoreId"].ToString())
                .WithCategoryName(!data.Keys.Contains("categoryName") || data["categoryName"] == null ? null : data["categoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithUniqueId(!data.Keys.Contains("uniqueId") || data["uniqueId"] == null ? null : data["uniqueId"].ToString())
                .WithScorerUserId(!data.Keys.Contains("scorerUserId") || data["scorerUserId"] == null ? null : data["scorerUserId"].ToString())
                .WithValue(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)long.Parse(data["score"].ToString()))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["scoreId"] = ScoreId,
                ["categoryName"] = CategoryName,
                ["userId"] = UserId,
                ["uniqueId"] = UniqueId,
                ["scorerUserId"] = ScorerUserId,
                ["score"] = Value,
                ["metadata"] = Metadata,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ScoreId != null) {
                writer.WritePropertyName("scoreId");
                writer.Write(ScoreId.ToString());
            }
            if (CategoryName != null) {
                writer.WritePropertyName("categoryName");
                writer.Write(CategoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (UniqueId != null) {
                writer.WritePropertyName("uniqueId");
                writer.Write(UniqueId.ToString());
            }
            if (ScorerUserId != null) {
                writer.WritePropertyName("scorerUserId");
                writer.Write(ScorerUserId.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(long.Parse(Value.ToString()));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Score;
            var diff = 0;
            if (ScoreId == null && ScoreId == other.ScoreId)
            {
                // null and null
            }
            else
            {
                diff += ScoreId.CompareTo(other.ScoreId);
            }
            if (CategoryName == null && CategoryName == other.CategoryName)
            {
                // null and null
            }
            else
            {
                diff += CategoryName.CompareTo(other.CategoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (UniqueId == null && UniqueId == other.UniqueId)
            {
                // null and null
            }
            else
            {
                diff += UniqueId.CompareTo(other.UniqueId);
            }
            if (ScorerUserId == null && ScorerUserId == other.ScorerUserId)
            {
                // null and null
            }
            else
            {
                diff += ScorerUserId.CompareTo(other.ScorerUserId);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += (int)(Value - other.Value);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
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
    }
}