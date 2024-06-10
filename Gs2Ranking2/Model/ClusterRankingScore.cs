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

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ClusterRankingScore : IComparable
	{
        public string ClusterRankingScoreId { set; get; } = null!;
        public string RankingName { set; get; } = null!;
        public string ClusterName { set; get; } = null!;
        public long? Season { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public long? Score { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ClusterRankingScore WithClusterRankingScoreId(string clusterRankingScoreId) {
            this.ClusterRankingScoreId = clusterRankingScoreId;
            return this;
        }
        public ClusterRankingScore WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public ClusterRankingScore WithClusterName(string clusterName) {
            this.ClusterName = clusterName;
            return this;
        }
        public ClusterRankingScore WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public ClusterRankingScore WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ClusterRankingScore WithScore(long? score) {
            this.Score = score;
            return this;
        }
        public ClusterRankingScore WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public ClusterRankingScore WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ClusterRankingScore WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public ClusterRankingScore WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
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

        private static System.Text.RegularExpressions.Regex _rankingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRankingNameFromGrn(
            string grn
        )
        {
            var match = _rankingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rankingName"].Success)
            {
                return null;
            }
            return match.Groups["rankingName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _clusterNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetClusterNameFromGrn(
            string grn
        )
        {
            var match = _clusterNameRegex.Match(grn);
            if (!match.Success || !match.Groups["clusterName"].Success)
            {
                return null;
            }
            return match.Groups["clusterName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _seasonRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):score",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSeasonFromGrn(
            string grn
        )
        {
            var match = _seasonRegex.Match(grn);
            if (!match.Success || !match.Groups["season"].Success)
            {
                return null;
            }
            return match.Groups["season"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ClusterRankingScore FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ClusterRankingScore()
                .WithClusterRankingScoreId(!data.Keys.Contains("clusterRankingScoreId") || data["clusterRankingScoreId"] == null ? null : data["clusterRankingScoreId"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithClusterName(!data.Keys.Contains("clusterName") || data["clusterName"] == null ? null : data["clusterName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithScore(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)(data["score"].ToString().Contains(".") ? (long)double.Parse(data["score"].ToString()) : long.Parse(data["score"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clusterRankingScoreId"] = ClusterRankingScoreId,
                ["rankingName"] = RankingName,
                ["clusterName"] = ClusterName,
                ["season"] = Season,
                ["userId"] = UserId,
                ["score"] = Score,
                ["metadata"] = Metadata,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClusterRankingScoreId != null) {
                writer.WritePropertyName("clusterRankingScoreId");
                writer.Write(ClusterRankingScoreId.ToString());
            }
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
            }
            if (ClusterName != null) {
                writer.WritePropertyName("clusterName");
                writer.Write(ClusterName.ToString());
            }
            if (Season != null) {
                writer.WritePropertyName("season");
                writer.Write((Season.ToString().Contains(".") ? (long)double.Parse(Season.ToString()) : long.Parse(Season.ToString())));
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Score != null) {
                writer.WritePropertyName("score");
                writer.Write((Score.ToString().Contains(".") ? (long)double.Parse(Score.ToString()) : long.Parse(Score.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
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
            var other = obj as ClusterRankingScore;
            var diff = 0;
            if (ClusterRankingScoreId == null && ClusterRankingScoreId == other.ClusterRankingScoreId)
            {
                // null and null
            }
            else
            {
                diff += ClusterRankingScoreId.CompareTo(other.ClusterRankingScoreId);
            }
            if (RankingName == null && RankingName == other.RankingName)
            {
                // null and null
            }
            else
            {
                diff += RankingName.CompareTo(other.RankingName);
            }
            if (ClusterName == null && ClusterName == other.ClusterName)
            {
                // null and null
            }
            else
            {
                diff += ClusterName.CompareTo(other.ClusterName);
            }
            if (Season == null && Season == other.Season)
            {
                // null and null
            }
            else
            {
                diff += (int)(Season - other.Season);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Score == null && Score == other.Score)
            {
                // null and null
            }
            else
            {
                diff += (int)(Score - other.Score);
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

        public void Validate() {
            {
                if (ClusterRankingScoreId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.clusterRankingScoreId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (ClusterName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.clusterName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.season.error.invalid"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Score < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.score.error.invalid"),
                    });
                }
                if (Score > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.score.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingScore", "ranking2.clusterRankingScore.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingScore {
                ClusterRankingScoreId = ClusterRankingScoreId,
                RankingName = RankingName,
                ClusterName = ClusterName,
                Season = Season,
                UserId = UserId,
                Score = Score,
                Metadata = Metadata,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}