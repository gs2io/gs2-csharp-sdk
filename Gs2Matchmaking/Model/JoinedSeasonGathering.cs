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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class JoinedSeasonGathering : IComparable
	{
        public string JoinedSeasonGatheringId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string SeasonName { set; get; } = null!;
        public long? Season { set; get; } = null!;
        public long? Tier { set; get; } = null!;
        public string SeasonGatheringName { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public JoinedSeasonGathering WithJoinedSeasonGatheringId(string joinedSeasonGatheringId) {
            this.JoinedSeasonGatheringId = joinedSeasonGatheringId;
            return this;
        }
        public JoinedSeasonGathering WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public JoinedSeasonGathering WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public JoinedSeasonGathering WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public JoinedSeasonGathering WithTier(long? tier) {
            this.Tier = tier;
            return this;
        }
        public JoinedSeasonGathering WithSeasonGatheringName(string seasonGatheringName) {
            this.SeasonGatheringName = seasonGatheringName;
            return this;
        }
        public JoinedSeasonGathering WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
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

        private static System.Text.RegularExpressions.Regex _seasonNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSeasonNameFromGrn(
            string grn
        )
        {
            var match = _seasonNameRegex.Match(grn);
            if (!match.Success || !match.Groups["seasonName"].Success)
            {
                return null;
            }
            return match.Groups["seasonName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _seasonRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):user:(?<userId>.+):joinedGathering",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JoinedSeasonGathering FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JoinedSeasonGathering()
                .WithJoinedSeasonGatheringId(!data.Keys.Contains("joinedSeasonGatheringId") || data["joinedSeasonGatheringId"] == null ? null : data["joinedSeasonGatheringId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithTier(!data.Keys.Contains("tier") || data["tier"] == null ? null : (long?)(data["tier"].ToString().Contains(".") ? (long)double.Parse(data["tier"].ToString()) : long.Parse(data["tier"].ToString())))
                .WithSeasonGatheringName(!data.Keys.Contains("seasonGatheringName") || data["seasonGatheringName"] == null ? null : data["seasonGatheringName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["joinedSeasonGatheringId"] = JoinedSeasonGatheringId,
                ["userId"] = UserId,
                ["seasonName"] = SeasonName,
                ["season"] = Season,
                ["tier"] = Tier,
                ["seasonGatheringName"] = SeasonGatheringName,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (JoinedSeasonGatheringId != null) {
                writer.WritePropertyName("joinedSeasonGatheringId");
                writer.Write(JoinedSeasonGatheringId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (Season != null) {
                writer.WritePropertyName("season");
                writer.Write((Season.ToString().Contains(".") ? (long)double.Parse(Season.ToString()) : long.Parse(Season.ToString())));
            }
            if (Tier != null) {
                writer.WritePropertyName("tier");
                writer.Write((Tier.ToString().Contains(".") ? (long)double.Parse(Tier.ToString()) : long.Parse(Tier.ToString())));
            }
            if (SeasonGatheringName != null) {
                writer.WritePropertyName("seasonGatheringName");
                writer.Write(SeasonGatheringName.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as JoinedSeasonGathering;
            var diff = 0;
            if (JoinedSeasonGatheringId == null && JoinedSeasonGatheringId == other.JoinedSeasonGatheringId)
            {
                // null and null
            }
            else
            {
                diff += JoinedSeasonGatheringId.CompareTo(other.JoinedSeasonGatheringId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (SeasonName == null && SeasonName == other.SeasonName)
            {
                // null and null
            }
            else
            {
                diff += SeasonName.CompareTo(other.SeasonName);
            }
            if (Season == null && Season == other.Season)
            {
                // null and null
            }
            else
            {
                diff += (int)(Season - other.Season);
            }
            if (Tier == null && Tier == other.Tier)
            {
                // null and null
            }
            else
            {
                diff += (int)(Tier - other.Tier);
            }
            if (SeasonGatheringName == null && SeasonGatheringName == other.SeasonGatheringName)
            {
                // null and null
            }
            else
            {
                diff += SeasonGatheringName.CompareTo(other.SeasonGatheringName);
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
                if (JoinedSeasonGatheringId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.joinedSeasonGatheringId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.userId.error.tooLong"),
                    });
                }
            }
            {
                if (SeasonName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.seasonName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.season.error.invalid"),
                    });
                }
            }
            {
                if (Tier < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.tier.error.invalid"),
                    });
                }
                if (Tier > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.tier.error.invalid"),
                    });
                }
            }
            {
                if (SeasonGatheringName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.seasonGatheringName.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("joinedSeasonGathering", "matchmaking.joinedSeasonGathering.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new JoinedSeasonGathering {
                JoinedSeasonGatheringId = JoinedSeasonGatheringId,
                UserId = UserId,
                SeasonName = SeasonName,
                Season = Season,
                Tier = Tier,
                SeasonGatheringName = SeasonGatheringName,
                CreatedAt = CreatedAt,
            };
        }
    }
}