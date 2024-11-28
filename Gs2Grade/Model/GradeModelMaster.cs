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

namespace Gs2.Gs2Grade.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GradeModelMaster : IComparable
	{
        public string GradeModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Grade.Model.DefaultGradeModel[] DefaultGrades { set; get; } = null!;
        public string ExperienceModelId { set; get; } = null!;
        public Gs2.Gs2Grade.Model.GradeEntryModel[] GradeEntries { set; get; } = null!;
        public Gs2.Gs2Grade.Model.AcquireActionRate[] AcquireActionRates { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public GradeModelMaster WithGradeModelId(string gradeModelId) {
            this.GradeModelId = gradeModelId;
            return this;
        }
        public GradeModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public GradeModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public GradeModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public GradeModelMaster WithDefaultGrades(Gs2.Gs2Grade.Model.DefaultGradeModel[] defaultGrades) {
            this.DefaultGrades = defaultGrades;
            return this;
        }
        public GradeModelMaster WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public GradeModelMaster WithGradeEntries(Gs2.Gs2Grade.Model.GradeEntryModel[] gradeEntries) {
            this.GradeEntries = gradeEntries;
            return this;
        }
        public GradeModelMaster WithAcquireActionRates(Gs2.Gs2Grade.Model.AcquireActionRate[] acquireActionRates) {
            this.AcquireActionRates = acquireActionRates;
            return this;
        }
        public GradeModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public GradeModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public GradeModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):model:(?<gradeName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):model:(?<gradeName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):model:(?<gradeName>.+)",
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

        private static System.Text.RegularExpressions.Regex _gradeNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):grade:(?<namespaceName>.+):model:(?<gradeName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGradeNameFromGrn(
            string grn
        )
        {
            var match = _gradeNameRegex.Match(grn);
            if (!match.Success || !match.Groups["gradeName"].Success)
            {
                return null;
            }
            return match.Groups["gradeName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GradeModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GradeModelMaster()
                .WithGradeModelId(!data.Keys.Contains("gradeModelId") || data["gradeModelId"] == null ? null : data["gradeModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultGrades(!data.Keys.Contains("defaultGrades") || data["defaultGrades"] == null || !data["defaultGrades"].IsArray ? null : data["defaultGrades"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.DefaultGradeModel.FromJson(v);
                }).ToArray())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithGradeEntries(!data.Keys.Contains("gradeEntries") || data["gradeEntries"] == null || !data["gradeEntries"].IsArray ? null : data["gradeEntries"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.GradeEntryModel.FromJson(v);
                }).ToArray())
                .WithAcquireActionRates(!data.Keys.Contains("acquireActionRates") || data["acquireActionRates"] == null || !data["acquireActionRates"].IsArray ? null : data["acquireActionRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.AcquireActionRate.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData defaultGradesJsonData = null;
            if (DefaultGrades != null && DefaultGrades.Length > 0)
            {
                defaultGradesJsonData = new JsonData();
                foreach (var defaultGrade in DefaultGrades)
                {
                    defaultGradesJsonData.Add(defaultGrade.ToJson());
                }
            }
            JsonData gradeEntriesJsonData = null;
            if (GradeEntries != null && GradeEntries.Length > 0)
            {
                gradeEntriesJsonData = new JsonData();
                foreach (var gradeEntry in GradeEntries)
                {
                    gradeEntriesJsonData.Add(gradeEntry.ToJson());
                }
            }
            JsonData acquireActionRatesJsonData = null;
            if (AcquireActionRates != null && AcquireActionRates.Length > 0)
            {
                acquireActionRatesJsonData = new JsonData();
                foreach (var acquireActionRate in AcquireActionRates)
                {
                    acquireActionRatesJsonData.Add(acquireActionRate.ToJson());
                }
            }
            return new JsonData {
                ["gradeModelId"] = GradeModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["defaultGrades"] = defaultGradesJsonData,
                ["experienceModelId"] = ExperienceModelId,
                ["gradeEntries"] = gradeEntriesJsonData,
                ["acquireActionRates"] = acquireActionRatesJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (GradeModelId != null) {
                writer.WritePropertyName("gradeModelId");
                writer.Write(GradeModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (DefaultGrades != null) {
                writer.WritePropertyName("defaultGrades");
                writer.WriteArrayStart();
                foreach (var defaultGrade in DefaultGrades)
                {
                    if (defaultGrade != null) {
                        defaultGrade.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (GradeEntries != null) {
                writer.WritePropertyName("gradeEntries");
                writer.WriteArrayStart();
                foreach (var gradeEntry in GradeEntries)
                {
                    if (gradeEntry != null) {
                        gradeEntry.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActionRates != null) {
                writer.WritePropertyName("acquireActionRates");
                writer.WriteArrayStart();
                foreach (var acquireActionRate in AcquireActionRates)
                {
                    if (acquireActionRate != null) {
                        acquireActionRate.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as GradeModelMaster;
            var diff = 0;
            if (GradeModelId == null && GradeModelId == other.GradeModelId)
            {
                // null and null
            }
            else
            {
                diff += GradeModelId.CompareTo(other.GradeModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (DefaultGrades == null && DefaultGrades == other.DefaultGrades)
            {
                // null and null
            }
            else
            {
                diff += DefaultGrades.Length - other.DefaultGrades.Length;
                for (var i = 0; i < DefaultGrades.Length; i++)
                {
                    diff += DefaultGrades[i].CompareTo(other.DefaultGrades[i]);
                }
            }
            if (ExperienceModelId == null && ExperienceModelId == other.ExperienceModelId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceModelId.CompareTo(other.ExperienceModelId);
            }
            if (GradeEntries == null && GradeEntries == other.GradeEntries)
            {
                // null and null
            }
            else
            {
                diff += GradeEntries.Length - other.GradeEntries.Length;
                for (var i = 0; i < GradeEntries.Length; i++)
                {
                    diff += GradeEntries[i].CompareTo(other.GradeEntries[i]);
                }
            }
            if (AcquireActionRates == null && AcquireActionRates == other.AcquireActionRates)
            {
                // null and null
            }
            else
            {
                diff += AcquireActionRates.Length - other.AcquireActionRates.Length;
                for (var i = 0; i < AcquireActionRates.Length; i++)
                {
                    diff += AcquireActionRates[i].CompareTo(other.AcquireActionRates[i]);
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
                if (GradeModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.gradeModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (DefaultGrades.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.defaultGrades.error.tooMany"),
                    });
                }
            }
            {
                if (ExperienceModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.experienceModelId.error.tooLong"),
                    });
                }
            }
            {
                if (GradeEntries.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.gradeEntries.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireActionRates.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.acquireActionRates.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("gradeModelMaster", "grade.gradeModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new GradeModelMaster {
                GradeModelId = GradeModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                DefaultGrades = DefaultGrades.Clone() as Gs2.Gs2Grade.Model.DefaultGradeModel[],
                ExperienceModelId = ExperienceModelId,
                GradeEntries = GradeEntries.Clone() as Gs2.Gs2Grade.Model.GradeEntryModel[],
                AcquireActionRates = AcquireActionRates.Clone() as Gs2.Gs2Grade.Model.AcquireActionRate[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}