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

namespace Gs2.Gs2Enhance.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class UnleashRateModel : IComparable
	{
        public string UnleashRateModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string TargetInventoryModelId { set; get; } = null!;
        public string GradeModelId { set; get; } = null!;
        public Gs2.Gs2Enhance.Model.UnleashRateEntryModel[] GradeEntries { set; get; } = null!;
        public UnleashRateModel WithUnleashRateModelId(string unleashRateModelId) {
            this.UnleashRateModelId = unleashRateModelId;
            return this;
        }
        public UnleashRateModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public UnleashRateModel WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UnleashRateModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UnleashRateModel WithTargetInventoryModelId(string targetInventoryModelId) {
            this.TargetInventoryModelId = targetInventoryModelId;
            return this;
        }
        public UnleashRateModel WithGradeModelId(string gradeModelId) {
            this.GradeModelId = gradeModelId;
            return this;
        }
        public UnleashRateModel WithGradeEntries(Gs2.Gs2Enhance.Model.UnleashRateEntryModel[] gradeEntries) {
            this.GradeEntries = gradeEntries;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):unleashRateModel:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):unleashRateModel:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):unleashRateModel:(?<rateName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rateNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):unleashRateModel:(?<rateName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRateNameFromGrn(
            string grn
        )
        {
            var match = _rateNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rateName"].Success)
            {
                return null;
            }
            return match.Groups["rateName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UnleashRateModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UnleashRateModel()
                .WithUnleashRateModelId(!data.Keys.Contains("unleashRateModelId") || data["unleashRateModelId"] == null ? null : data["unleashRateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTargetInventoryModelId(!data.Keys.Contains("targetInventoryModelId") || data["targetInventoryModelId"] == null ? null : data["targetInventoryModelId"].ToString())
                .WithGradeModelId(!data.Keys.Contains("gradeModelId") || data["gradeModelId"] == null ? null : data["gradeModelId"].ToString())
                .WithGradeEntries(!data.Keys.Contains("gradeEntries") || data["gradeEntries"] == null || !data["gradeEntries"].IsArray ? null : data["gradeEntries"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.UnleashRateEntryModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData gradeEntriesJsonData = null;
            if (GradeEntries != null && GradeEntries.Length > 0)
            {
                gradeEntriesJsonData = new JsonData();
                foreach (var gradeEntry in GradeEntries)
                {
                    gradeEntriesJsonData.Add(gradeEntry.ToJson());
                }
            }
            return new JsonData {
                ["unleashRateModelId"] = UnleashRateModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["targetInventoryModelId"] = TargetInventoryModelId,
                ["gradeModelId"] = GradeModelId,
                ["gradeEntries"] = gradeEntriesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UnleashRateModelId != null) {
                writer.WritePropertyName("unleashRateModelId");
                writer.Write(UnleashRateModelId.ToString());
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
            if (TargetInventoryModelId != null) {
                writer.WritePropertyName("targetInventoryModelId");
                writer.Write(TargetInventoryModelId.ToString());
            }
            if (GradeModelId != null) {
                writer.WritePropertyName("gradeModelId");
                writer.Write(GradeModelId.ToString());
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UnleashRateModel;
            var diff = 0;
            if (UnleashRateModelId == null && UnleashRateModelId == other.UnleashRateModelId)
            {
                // null and null
            }
            else
            {
                diff += UnleashRateModelId.CompareTo(other.UnleashRateModelId);
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
            if (TargetInventoryModelId == null && TargetInventoryModelId == other.TargetInventoryModelId)
            {
                // null and null
            }
            else
            {
                diff += TargetInventoryModelId.CompareTo(other.TargetInventoryModelId);
            }
            if (GradeModelId == null && GradeModelId == other.GradeModelId)
            {
                // null and null
            }
            else
            {
                diff += GradeModelId.CompareTo(other.GradeModelId);
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
            return diff;
        }

        public void Validate() {
            {
                if (UnleashRateModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.unleashRateModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (TargetInventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.targetInventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (GradeModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.gradeModelId.error.tooLong"),
                    });
                }
            }
            {
                if (GradeEntries.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.gradeEntries.error.tooFew"),
                    });
                }
                if (GradeEntries.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("unleashRateModel", "enhance.unleashRateModel.gradeEntries.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new UnleashRateModel {
                UnleashRateModelId = UnleashRateModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                TargetInventoryModelId = TargetInventoryModelId,
                GradeModelId = GradeModelId,
                GradeEntries = GradeEntries.Clone() as Gs2.Gs2Enhance.Model.UnleashRateEntryModel[],
            };
        }
    }
}