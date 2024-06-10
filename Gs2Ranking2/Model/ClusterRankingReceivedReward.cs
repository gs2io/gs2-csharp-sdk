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
	public class ClusterRankingReceivedReward : IComparable
	{
        public string ClusterRankingReceivedRewardId { set; get; } = null!;
        public string RankingName { set; get; } = null!;
        public string ClusterName { set; get; } = null!;
        public long? Season { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public long? ReceivedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ClusterRankingReceivedReward WithClusterRankingReceivedRewardId(string clusterRankingReceivedRewardId) {
            this.ClusterRankingReceivedRewardId = clusterRankingReceivedRewardId;
            return this;
        }
        public ClusterRankingReceivedReward WithRankingName(string rankingName) {
            this.RankingName = rankingName;
            return this;
        }
        public ClusterRankingReceivedReward WithClusterName(string clusterName) {
            this.ClusterName = clusterName;
            return this;
        }
        public ClusterRankingReceivedReward WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public ClusterRankingReceivedReward WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ClusterRankingReceivedReward WithReceivedAt(long? receivedAt) {
            this.ReceivedAt = receivedAt;
            return this;
        }
        public ClusterRankingReceivedReward WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):ranking2:(?<namespaceName>.+):user:(?<userId>.+):cluster:(?<rankingName>.+):(?<clusterName>.+):(?<season>.+):reward:received",
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
        public static ClusterRankingReceivedReward FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ClusterRankingReceivedReward()
                .WithClusterRankingReceivedRewardId(!data.Keys.Contains("clusterRankingReceivedRewardId") || data["clusterRankingReceivedRewardId"] == null ? null : data["clusterRankingReceivedRewardId"].ToString())
                .WithRankingName(!data.Keys.Contains("rankingName") || data["rankingName"] == null ? null : data["rankingName"].ToString())
                .WithClusterName(!data.Keys.Contains("clusterName") || data["clusterName"] == null ? null : data["clusterName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithReceivedAt(!data.Keys.Contains("receivedAt") || data["receivedAt"] == null ? null : (long?)(data["receivedAt"].ToString().Contains(".") ? (long)double.Parse(data["receivedAt"].ToString()) : long.Parse(data["receivedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clusterRankingReceivedRewardId"] = ClusterRankingReceivedRewardId,
                ["rankingName"] = RankingName,
                ["clusterName"] = ClusterName,
                ["season"] = Season,
                ["userId"] = UserId,
                ["receivedAt"] = ReceivedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClusterRankingReceivedRewardId != null) {
                writer.WritePropertyName("clusterRankingReceivedRewardId");
                writer.Write(ClusterRankingReceivedRewardId.ToString());
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
            if (ReceivedAt != null) {
                writer.WritePropertyName("receivedAt");
                writer.Write((ReceivedAt.ToString().Contains(".") ? (long)double.Parse(ReceivedAt.ToString()) : long.Parse(ReceivedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ClusterRankingReceivedReward;
            var diff = 0;
            if (ClusterRankingReceivedRewardId == null && ClusterRankingReceivedRewardId == other.ClusterRankingReceivedRewardId)
            {
                // null and null
            }
            else
            {
                diff += ClusterRankingReceivedRewardId.CompareTo(other.ClusterRankingReceivedRewardId);
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
            if (ReceivedAt == null && ReceivedAt == other.ReceivedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ReceivedAt - other.ReceivedAt);
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
                if (ClusterRankingReceivedRewardId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.clusterRankingReceivedRewardId.error.tooLong"),
                    });
                }
            }
            {
                if (RankingName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.rankingName.error.tooLong"),
                    });
                }
            }
            {
                if (ClusterName.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.clusterName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.season.error.invalid"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.userId.error.tooLong"),
                    });
                }
            }
            {
                if (ReceivedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.receivedAt.error.invalid"),
                    });
                }
                if (ReceivedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.receivedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("clusterRankingReceivedReward", "ranking2.clusterRankingReceivedReward.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ClusterRankingReceivedReward {
                ClusterRankingReceivedRewardId = ClusterRankingReceivedRewardId,
                RankingName = RankingName,
                ClusterName = ClusterName,
                Season = Season,
                UserId = UserId,
                ReceivedAt = ReceivedAt,
                Revision = Revision,
            };
        }
    }
}