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
	public partial class SeasonModelMaster : IComparable
	{
        public string SeasonModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public string Description { set; get; }
        public int? MaximumParticipants { set; get; }
        public string ExperienceModelId { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public SeasonModelMaster WithSeasonModelId(string seasonModelId) {
            this.SeasonModelId = seasonModelId;
            return this;
        }
        public SeasonModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public SeasonModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SeasonModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public SeasonModelMaster WithMaximumParticipants(int? maximumParticipants) {
            this.MaximumParticipants = maximumParticipants;
            return this;
        }
        public SeasonModelMaster WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public SeasonModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public SeasonModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SeasonModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SeasonModelMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static SeasonModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new SeasonModelMaster()
                .WithSeasonModelId(!data.Keys.Contains("seasonModelId") || data["seasonModelId"] == null ? null : data["seasonModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMaximumParticipants(!data.Keys.Contains("maximumParticipants") || data["maximumParticipants"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["maximumParticipants"].ToString()))
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["seasonModelId"] = SeasonModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["maximumParticipants"] = MaximumParticipants,
                ["experienceModelId"] = ExperienceModelId,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
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
            var other = obj as SeasonModelMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type SeasonModelMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(SeasonModelId, other.SeasonModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaximumParticipants, other.MaximumParticipants);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ExperienceModelId, other.ExperienceModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ChallengePeriodEventId, other.ChallengePeriodEventId);
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
                if (SeasonModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.seasonModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (MaximumParticipants < 2) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.maximumParticipants.error.invalid"),
                    });
                }
                if (MaximumParticipants > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.maximumParticipants.error.invalid"),
                    });
                }
            }
            {
                if (ExperienceModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.experienceModelId.error.tooLong"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("seasonModelMaster", "matchmaking.seasonModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SeasonModelMaster {
                SeasonModelId = SeasonModelId,
                Name = Name,
                Metadata = Metadata,
                Description = Description,
                MaximumParticipants = MaximumParticipants,
                ExperienceModelId = ExperienceModelId,
                ChallengePeriodEventId = ChallengePeriodEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}