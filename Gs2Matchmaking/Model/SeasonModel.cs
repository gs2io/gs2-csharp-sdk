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
	public class SeasonModel : IComparable
	{
        public string SeasonModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? MaximumParticipants { set; get; } = null!;
        public string ExperienceModelId { set; get; } = null!;
        public string ChallengePeriodEventId { set; get; } = null!;
        public SeasonModel WithSeasonModelId(string seasonModelId) {
            this.SeasonModelId = seasonModelId;
            return this;
        }
        public SeasonModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public SeasonModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SeasonModel WithMaximumParticipants(int? maximumParticipants) {
            this.MaximumParticipants = maximumParticipants;
            return this;
        }
        public SeasonModel WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public SeasonModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<seasonName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<seasonName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<seasonName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<seasonName>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SeasonModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SeasonModel()
                .WithSeasonModelId(!data.Keys.Contains("seasonModelId") || data["seasonModelId"] == null ? null : data["seasonModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumParticipants(!data.Keys.Contains("maximumParticipants") || data["maximumParticipants"] == null ? null : (int?)(data["maximumParticipants"].ToString().Contains(".") ? (int)double.Parse(data["maximumParticipants"].ToString()) : int.Parse(data["maximumParticipants"].ToString())))
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["seasonModelId"] = SeasonModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["maximumParticipants"] = MaximumParticipants,
                ["experienceModelId"] = ExperienceModelId,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SeasonModelId != null) {
                writer.WritePropertyName("seasonModelId");
                writer.Write(SeasonModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MaximumParticipants != null) {
                writer.WritePropertyName("maximumParticipants");
                writer.Write((MaximumParticipants.ToString().Contains(".") ? (int)double.Parse(MaximumParticipants.ToString()) : int.Parse(MaximumParticipants.ToString())));
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SeasonModel;
            var diff = 0;
            if (SeasonModelId == null && SeasonModelId == other.SeasonModelId)
            {
                // null and null
            }
            else
            {
                diff += SeasonModelId.CompareTo(other.SeasonModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (MaximumParticipants == null && MaximumParticipants == other.MaximumParticipants)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumParticipants - other.MaximumParticipants);
            }
            if (ExperienceModelId == null && ExperienceModelId == other.ExperienceModelId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceModelId.CompareTo(other.ExperienceModelId);
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
            }
            return diff;
        }

        public void Validate() {
            {
                if (SeasonModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.seasonModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (MaximumParticipants < 2) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.maximumParticipants.error.invalid"),
                    });
                }
                if (MaximumParticipants > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.maximumParticipants.error.invalid"),
                    });
                }
            }
            {
                if (ExperienceModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.experienceModelId.error.tooLong"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModel", "matchmaking.seasonModel.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SeasonModel {
                SeasonModelId = SeasonModelId,
                Name = Name,
                Metadata = Metadata,
                MaximumParticipants = MaximumParticipants,
                ExperienceModelId = ExperienceModelId,
                ChallengePeriodEventId = ChallengePeriodEventId,
            };
        }
    }
}