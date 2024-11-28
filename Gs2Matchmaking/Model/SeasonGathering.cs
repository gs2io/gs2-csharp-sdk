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
	public class SeasonGathering : IComparable
	{
        public string SeasonGatheringId { set; get; } = null!;
        public string SeasonName { set; get; } = null!;
        public long? Season { set; get; } = null!;
        public long? Tier { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string[] Participants { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public SeasonGathering WithSeasonGatheringId(string seasonGatheringId) {
            this.SeasonGatheringId = seasonGatheringId;
            return this;
        }
        public SeasonGathering WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public SeasonGathering WithSeason(long? season) {
            this.Season = season;
            return this;
        }
        public SeasonGathering WithTier(long? tier) {
            this.Tier = tier;
            return this;
        }
        public SeasonGathering WithName(string name) {
            this.Name = name;
            return this;
        }
        public SeasonGathering WithParticipants(string[] participants) {
            this.Participants = participants;
            return this;
        }
        public SeasonGathering WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SeasonGathering WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
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

        private static System.Text.RegularExpressions.Regex _tierRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTierFromGrn(
            string grn
        )
        {
            var match = _tierRegex.Match(grn);
            if (!match.Success || !match.Groups["tier"].Success)
            {
                return null;
            }
            return match.Groups["tier"].Value;
        }

        private static System.Text.RegularExpressions.Regex _seasonGatheringNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):season:(?<seasonName>.+):(?<season>.+):(?<tier>.+):gathering:(?<seasonGatheringName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSeasonGatheringNameFromGrn(
            string grn
        )
        {
            var match = _seasonGatheringNameRegex.Match(grn);
            if (!match.Success || !match.Groups["seasonGatheringName"].Success)
            {
                return null;
            }
            return match.Groups["seasonGatheringName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SeasonGathering FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SeasonGathering()
                .WithSeasonGatheringId(!data.Keys.Contains("seasonGatheringId") || data["seasonGatheringId"] == null ? null : data["seasonGatheringId"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithSeason(!data.Keys.Contains("season") || data["season"] == null ? null : (long?)(data["season"].ToString().Contains(".") ? (long)double.Parse(data["season"].ToString()) : long.Parse(data["season"].ToString())))
                .WithTier(!data.Keys.Contains("tier") || data["tier"] == null ? null : (long?)(data["tier"].ToString().Contains(".") ? (long)double.Parse(data["tier"].ToString()) : long.Parse(data["tier"].ToString())))
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithParticipants(!data.Keys.Contains("participants") || data["participants"] == null || !data["participants"].IsArray ? null : data["participants"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData participantsJsonData = null;
            if (Participants != null && Participants.Length > 0)
            {
                participantsJsonData = new JsonData();
                foreach (var participant in Participants)
                {
                    participantsJsonData.Add(participant);
                }
            }
            return new JsonData {
                ["seasonGatheringId"] = SeasonGatheringId,
                ["seasonName"] = SeasonName,
                ["season"] = Season,
                ["tier"] = Tier,
                ["name"] = Name,
                ["participants"] = participantsJsonData,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SeasonGatheringId != null) {
                writer.WritePropertyName("seasonGatheringId");
                writer.Write(SeasonGatheringId.ToString());
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
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Participants != null) {
                writer.WritePropertyName("participants");
                writer.WriteArrayStart();
                foreach (var participant in Participants)
                {
                    if (participant != null) {
                        writer.Write(participant.ToString());
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as SeasonGathering;
            var diff = 0;
            if (SeasonGatheringId == null && SeasonGatheringId == other.SeasonGatheringId)
            {
                // null and null
            }
            else
            {
                diff += SeasonGatheringId.CompareTo(other.SeasonGatheringId);
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
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Participants == null && Participants == other.Participants)
            {
                // null and null
            }
            else
            {
                diff += Participants.Length - other.Participants.Length;
                for (var i = 0; i < Participants.Length; i++)
                {
                    diff += Participants[i].CompareTo(other.Participants[i]);
                }
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
                if (SeasonGatheringId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.seasonGatheringId.error.tooLong"),
                    });
                }
            }
            {
                if (SeasonName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.seasonName.error.tooLong"),
                    });
                }
            }
            {
                if (Season < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.season.error.invalid"),
                    });
                }
                if (Season > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.season.error.invalid"),
                    });
                }
            }
            {
                if (Tier < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.tier.error.invalid"),
                    });
                }
                if (Tier > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.tier.error.invalid"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.name.error.tooLong"),
                    });
                }
            }
            {
                if (Participants.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.participants.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonGathering", "matchmaking.seasonGathering.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SeasonGathering {
                SeasonGatheringId = SeasonGatheringId,
                SeasonName = SeasonName,
                Season = Season,
                Tier = Tier,
                Name = Name,
                Participants = Participants.Clone() as string[],
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}