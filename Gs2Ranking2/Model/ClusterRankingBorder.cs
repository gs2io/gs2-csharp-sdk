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
	public class ClusterRankingBorder : IComparable
	{
        public string ClusterRankingBoarderId { set; get; }
        public string RankingName { set; get; }
        public string ClusterName { set; get; }
        public long? Season { set; get; }
        public long? Score { set; get; }
        public long? CreatedAt { set; get; }
        public ClusterRankingBorder WithClusterRankingBoarderId(string clusterRankingBoarderId) {
            this.ClusterRankingBoarderId = clusterRankingBoarderId;
            return this;
        }
        public ClusterRankingBorder WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public ClusterRankingBorder WithClusterName(string clusterName) {
            this.ClusterName = clusterName;
            return this;
        }
        public ClusterRankingBorder WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public ClusterRankingBorder WithScore(long? score) {
            this.Score = score;
            return this;
        }
        public ClusterRankingBorder WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):cluster:(?<rankingName>.+):ranking:cluster:(?<clusterName>.+):(?<season>.+):border",
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
        public static ClusterRankingBorder FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ClusterRankingBorder()
                .WithClusterRankingBoarderId(!data.Keys.Contains("clusterRankingBoarderId") || data["clusterRankingBoarderId"] == null ? null : data["clusterRankingBoarderId"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithClusterName(!data.Keys.Contains("clusterName") || data["clusterName"] == null ? null : data["clusterName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithScore(!data.Keys.Contains("score") || data["score"] == null ? null : (long?)(data["score"].ToString().Contains(".") ? (long)double.Parse(data["score"].ToString()) : long.Parse(data["score"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clusterRankingBoarderId"] = ClusterRankingBoarderId,
                ["rankingName"] = RankingName,
                ["clusterName"] = ClusterName,
                ["season"] = Season,
                ["score"] = Score,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClusterRankingBoarderId != null) {
                writer.WritePropertyName("clusterRankingBoarderId");
                writer.Write(ClusterRankingBoarderId.ToString());
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
            if (Score != null) {
                writer.WritePropertyName("score");
                writer.Write((Score.ToString().Contains(".") ? (long)double.Parse(Score.ToString()) : long.Parse(Score.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ClusterRankingBorder;
            var diff = 0;
            if (ClusterRankingBoarderId == null && ClusterRankingBoarderId == other.ClusterRankingBoarderId)
            {
                // null and null
            }
            else
            {
                diff += ClusterRankingBoarderId.CompareTo(other.ClusterRankingBoarderId);
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
            if (Score == null && Score == other.Score)
            {
                // null and null
            }
            else
            {
                diff += (int)(Score - other.Score);
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

        public void Validate() {
            {
                if (ClusterRankingBoarderId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.clusterRankingBoarderId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (ClusterName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.clusterName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.season.error.invalid"),
                    });
                }
            }
            {
                if (Score < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.score.error.invalid"),
                    });
                }
                if (Score > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.score.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingBorder", "ranking2.clusterRankingBorder.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingBorder {
                ClusterRankingBoarderId = ClusterRankingBoarderId,
                RankingName = RankingName,
                ClusterName = ClusterName,
                Season = Season,
                Score = Score,
                CreatedAt = CreatedAt,
            };
        }
    }
}