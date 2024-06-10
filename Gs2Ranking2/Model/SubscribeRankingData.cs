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
	public class SubscribeRankingData : IComparable
	{
        public string SubscribeRankingDataId { set; get; } = null!;
        public string RankingName { set; get; } = null!;
        public long? Season { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Index { set; get; } = null!;
        public int? Rank { set; get; } = null!;
        public string ScorerUserId { set; get; } = null!;
        public long? Score { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public SubscribeRankingData WithSubscribeRankingDataId(string subscribeRankingDataId) {
            this.SubscribeRankingDataId = subscribeRankingDataId;
            return this;
        }
        public SubscribeRankingData WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public SubscribeRankingData WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public SubscribeRankingData WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public SubscribeRankingData WithIndex(int? index) {
            this.Index = index;
            return this;
        }
        public SubscribeRankingData WithRank(int? rank) {
            this.Rank = rank;
            return this;
        }
        public SubscribeRankingData WithScorerUserId(string scorerUserId) {
            this.ScorerUserId = scorerUserId;
            return this;
        }
        public SubscribeRankingData WithScore(long? score) {
            this.Score = score;
            return this;
        }
        public SubscribeRankingData WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SubscribeRankingData WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SubscribeRankingData WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SubscribeRankingData WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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

        private static System.Text.RegularExpressions.Regex _seasonRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):ranking:subscribe:(?<rankingName>.+):(?<season>.+):user:(?<scorerUserId>.+):score",
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
        public static SubscribeRankingData FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SubscribeRankingData()
                .WithSubscribeRankingDataId(!data.Keys.Contains("subscribeRankingDataId") || data["subscribeRankingDataId"] == null ? null : data["subscribeRankingDataId"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithIndex(!data.Keys.Contains("index") || data["index"] == null ? null : (int?)(data["index"].ToString().Contains(".") ? (int)double.Parse(data["index"].ToString()) : int.Parse(data["index"].ToString())))
                .WithRank(!data.Keys.Contains("rank") || data["rank"] == null ? null : (int?)(data["rank"].ToString().Contains(".") ? (int)double.Parse(data["rank"].ToString()) : int.Parse(data["rank"].ToString())))
                .WithScorerUserId(!data.Keys.Contains("scorerUserId") || data["scorerUserId"] == null ? null : data["scorerUserId"].ToString())
                .WithScore(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)(data["score"].ToString().Contains(".") ? (long)double.Parse(data["score"].ToString()) : long.Parse(data["score"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["subscribeRankingDataId"] = SubscribeRankingDataId,
                ["rankingName"] = RankingName,
                ["season"] = Season,
                ["userId"] = UserId,
                ["index"] = Index,
                ["rank"] = Rank,
                ["scorerUserId"] = ScorerUserId,
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
            if (SubscribeRankingDataId != null) {
                writer.WritePropertyName("subscribeRankingDataId");
                writer.Write(SubscribeRankingDataId.ToString());
            }
            if (RankingName != null) {
                writer.WritePropertyName("rankingName");
                writer.Write(RankingName.ToString());
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
            if (ScorerUserId != null) {
                writer.WritePropertyName("scorerUserId");
                writer.Write(ScorerUserId.ToString());
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
            var other = obj as SubscribeRankingData;
            var diff = 0;
            if (SubscribeRankingDataId == null && SubscribeRankingDataId == other.SubscribeRankingDataId)
            {
                // null and null
            }
            else
            {
                diff += SubscribeRankingDataId.CompareTo(other.SubscribeRankingDataId);
            }
            if (RankingName == null && RankingName == other.RankingName)
            {
                // null and null
            }
            else
            {
                diff += RankingName.CompareTo(other.RankingName);
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
            if (Index == null && Index == other.Index)
            {
                // null and null
            }
            else
            {
                diff += (int)(Index - other.Index);
            }
            if (Rank == null && Rank == other.Rank)
            {
                // null and null
            }
            else
            {
                diff += (int)(Rank - other.Rank);
            }
            if (ScorerUserId == null && ScorerUserId == other.ScorerUserId)
            {
                // null and null
            }
            else
            {
                diff += ScorerUserId.CompareTo(other.ScorerUserId);
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
                if (SubscribeRankingDataId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.subscribeRankingDataId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.season.error.invalid"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.userId.error.tooLong"),
                    });
                }
            }
            {
                if (Index < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.index.error.invalid"),
                    });
                }
                if (Index > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.index.error.invalid"),
                    });
                }
            }
            {
                if (Rank < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.rank.error.invalid"),
                    });
                }
                if (Rank > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.rank.error.invalid"),
                    });
                }
            }
            {
                if (ScorerUserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.scorerUserId.error.tooLong"),
                    });
                }
            }
            {
                if (Score < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.score.error.invalid"),
                    });
                }
                if (Score > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.score.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 512) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("subscribeRankingData", "ranking2.subscribeRankingData.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SubscribeRankingData {
                SubscribeRankingDataId = SubscribeRankingDataId,
                RankingName = RankingName,
                Season = Season,
                UserId = UserId,
                Index = Index,
                Rank = Rank,
                ScorerUserId = ScorerUserId,
                Score = Score,
                Metadata = Metadata,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}