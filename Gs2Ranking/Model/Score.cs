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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Score : IComparable
	{
        public string ScoreId { set; get; } = null!;
        public string CategoryName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string UniqueId { set; get; } = null!;
        public string ScorerUserId { set; get; } = null!;
        public long? Value { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public Score WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _categoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCategoryNameFromGrn(
            string grn
        )
        {
            var match = _categoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["categoryName"].Success)
            {
                return null;
            }
            return match.Groups["categoryName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _scorerUserIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetScorerUserIdFromGrn(
            string grn
        )
        {
            var match = _scorerUserIdRegex.Match(grn);
            if (!match.Success || !match.Groups["scorerUserId"].Success)
            {
                return null;
            }
            return match.Groups["scorerUserId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _uniqueIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking:(?<namespaceName>.+):user:(?<userId>.+):category:(?<categoryName>.+):score:(?<scorerUserId>.+):(?<uniqueId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUniqueIdFromGrn(
            string grn
        )
        {
            var match = _uniqueIdRegex.Match(grn);
            if (!match.Success || !match.Groups["uniqueId"].Success)
            {
                return null;
            }
            return match.Groups["uniqueId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
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
                .WithValue(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)(data["score"].ToString().Contains(".") ? (long)double.Parse(data["score"].ToString()) : long.Parse(data["score"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["revision"] = Revision,
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
                writer.Write((Value.ToString().Contains(".") ? (long)double.Parse(Value.ToString()) : long.Parse(Value.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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

        public void Validate() {
            {
                if (ScoreId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.scoreId.error.tooLong"),
                    });
                }
            }
            {
                if (CategoryName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.categoryName.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.userId.error.tooLong"),
                    });
                }
            }
            {
                if (UniqueId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.uniqueId.error.tooLong"),
                    });
                }
            }
            {
                if (ScorerUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.scorerUserId.error.tooLong"),
                    });
                }
            }
            {
                if (Value < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.score.error.invalid"),
                    });
                }
                if (Value > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.score.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("score", "ranking.score.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Score {
                ScoreId = ScoreId,
                CategoryName = CategoryName,
                UserId = UserId,
                UniqueId = UniqueId,
                ScorerUserId = ScorerUserId,
                Value = Value,
                Metadata = Metadata,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}