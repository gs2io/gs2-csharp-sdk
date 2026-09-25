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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class ClusterRankingData : IComparable
	{
        public string ClusterRankingDataId { set; get; }
        public string RankingName { set; get; }
        public string ClusterName { set; get; }
        public long? Season { set; get; }
        public string UserId { set; get; }
        public int? Index { set; get; }
        public int? Rank { set; get; }
        public long? Score { set; get; }
        public string Metadata { set; get; }
        public long? InvertUpdatedAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public ClusterRankingData WithClusterRankingDataId(string clusterRankingDataId) {
            this.ClusterRankingDataId = clusterRankingDataId;
            return this;
        }
        public ClusterRankingData WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public ClusterRankingData WithClusterName(string clusterName) {
            this.ClusterName = clusterName;
            return this;
        }
        public ClusterRankingData WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public ClusterRankingData WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ClusterRankingData WithIndex(int? index) {
            this.Index = index;
            return this;
        }
        public ClusterRankingData WithRank(int? rank) {
            this.Rank = rank;
            return this;
        }
        public ClusterRankingData WithScore(long? score) {
            this.Score = score;
            return this;
        }
        public ClusterRankingData WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public ClusterRankingData WithInvertUpdatedAt(long? invertUpdatedAt) {
            this.InvertUpdatedAt = invertUpdatedAt;
            return this;
        }
        public ClusterRankingData WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ClusterRankingData WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public ClusterRankingData WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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

        private static System.Text.RegularExpressions.Regex _rankingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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

        private static System.Text.RegularExpressions.Regex _scorerUserIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ClusterRankingData FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new ClusterRankingData()
                .WithClusterRankingDataId(!data.Keys.Contains("clusterRankingDataId") || data["clusterRankingDataId"] == null ? null : data["clusterRankingDataId"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithClusterName(!data.Keys.Contains("clusterName") || data["clusterName"] == null ? null : data["clusterName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["season"].ToString()))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["index"].ToString()))
                .WithRank(!data.Keys.Contains("rank") || data["rank"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["rank"].ToString()))
                .WithScore(!data.Keys.Contains("score") || data["score"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["score"].ToString()))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInvertUpdatedAt(!data.Keys.Contains("invertUpdatedAt") || data["invertUpdatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["invertUpdatedAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clusterRankingDataId"] = ClusterRankingDataId,
                ["rankingName"] = RankingName,
                ["clusterName"] = ClusterName,
                ["season"] = Season,
                ["userId"] = UserId,
                ["index"] = Index,
                ["rank"] = Rank,
                ["score"] = Score,
                ["metadata"] = Metadata,
                ["invertUpdatedAt"] = InvertUpdatedAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClusterRankingDataId != null) {
                writer.WritePropertyName("clusterRankingDataId");
                writer.Write(ClusterRankingDataId.ToString());
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
            if (Index != null) {
                writer.WritePropertyName("index");
                writer.Write((Index.ToString().Contains(".") ? (int)double.Parse(Index.ToString()) : int.Parse(Index.ToString())));
            }
            if (Rank != null) {
                writer.WritePropertyName("rank");
                writer.Write((Rank.ToString().Contains(".") ? (int)double.Parse(Rank.ToString()) : int.Parse(Rank.ToString())));
            }
            if (Score != null) {
                writer.WritePropertyName("score");
                writer.Write((Score.ToString().Contains(".") ? (long)double.Parse(Score.ToString()) : long.Parse(Score.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InvertUpdatedAt != null) {
                writer.WritePropertyName("invertUpdatedAt");
                writer.Write((InvertUpdatedAt.ToString().Contains(".") ? (long)double.Parse(InvertUpdatedAt.ToString()) : long.Parse(InvertUpdatedAt.ToString())));
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
            var other = obj as ClusterRankingData;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type ClusterRankingData.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(ClusterRankingDataId, other.ClusterRankingDataId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RankingName, other.RankingName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ClusterName, other.ClusterName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Season, other.Season);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Index, other.Index);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Rank, other.Rank);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Score, other.Score);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(InvertUpdatedAt, other.InvertUpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (ClusterRankingDataId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.clusterRankingDataId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (ClusterName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.clusterName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.season.error.invalid"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Index < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.index.error.invalid"),
                    });
                }
                if (Index > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.index.error.invalid"),
                    });
                }
            }
            {
                if (Rank < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.rank.error.invalid"),
                    });
                }
                if (Rank > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.rank.error.invalid"),
                    });
                }
            }
            {
                if (Score < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.score.error.invalid"),
                    });
                }
                if (Score > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.score.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (InvertUpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.invertUpdatedAt.error.invalid"),
                    });
                }
                if (InvertUpdatedAt > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.invertUpdatedAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingData", "ranking2.clusterRankingData.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingData {
                ClusterRankingDataId = ClusterRankingDataId,
                RankingName = RankingName,
                ClusterName = ClusterName,
                Season = Season,
                UserId = UserId,
                Index = Index,
                Rank = Rank,
                Score = Score,
                Metadata = Metadata,
                InvertUpdatedAt = InvertUpdatedAt,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}