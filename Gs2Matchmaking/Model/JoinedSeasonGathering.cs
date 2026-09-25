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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class JoinedSeasonGathering : IComparable
	{
        public string JoinedSeasonGatheringId { set; get; }
        public string UserId { set; get; }
        public string SeasonName { set; get; }
        public long? Season { set; get; }
        public long? Tier { set; get; }
        public string SeasonGatheringName { set; get; }
        public long? CreatedAt { set; get; }
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new JoinedSeasonGathering()
                .WithJoinedSeasonGatheringId(!data.Keys.Contains("joinedSeasonGatheringId") || data["joinedSeasonGatheringId"] == null ? null : data["joinedSeasonGatheringId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["season"].ToString()))
                .WithTier(!data.Keys.Contains("tier") || data["tier"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["tier"].ToString()))
                .WithSeasonGatheringName(!data.Keys.Contains("seasonGatheringName") || data["seasonGatheringName"] == null ? null : data["seasonGatheringName"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type JoinedSeasonGathering.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(JoinedSeasonGatheringId, other.JoinedSeasonGatheringId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(SeasonName, other.SeasonName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Season, other.Season);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Tier, other.Tier);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(SeasonGatheringName, other.SeasonGatheringName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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