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
	public class RateModel : IComparable
	{
        public string RateModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string TargetInventoryModelId { set; get; } = null!;
        public string AcquireExperienceSuffix { set; get; } = null!;
        public string MaterialInventoryModelId { set; get; } = null!;
        public string[] AcquireExperienceHierarchy { set; get; } = null!;
        public string ExperienceModelId { set; get; } = null!;
        public Gs2.Gs2Enhance.Model.BonusRate[] BonusRates { set; get; } = null!;
        public RateModel WithRateModelId(string rateModelId) {
            this.RateModelId = rateModelId;
            return this;
        }
        public RateModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public RateModel WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public RateModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RateModel WithTargetInventoryModelId(string targetInventoryModelId) {
            this.TargetInventoryModelId = targetInventoryModelId;
            return this;
        }
        public RateModel WithAcquireExperienceSuffix(string acquireExperienceSuffix) {
            this.AcquireExperienceSuffix = acquireExperienceSuffix;
            return this;
        }
        public RateModel WithMaterialInventoryModelId(string materialInventoryModelId) {
            this.MaterialInventoryModelId = materialInventoryModelId;
            return this;
        }
        public RateModel WithAcquireExperienceHierarchy(string[] acquireExperienceHierarchy) {
            this.AcquireExperienceHierarchy = acquireExperienceHierarchy;
            return this;
        }
        public RateModel WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public RateModel WithBonusRates(Gs2.Gs2Enhance.Model.BonusRate[] bonusRates) {
            this.BonusRates = bonusRates;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):rateModel:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):rateModel:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):rateModel:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enhance:(?<namespaceName>.+):rateModel:(?<rateName>.+)",
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
        public static RateModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RateModel()
                .WithRateModelId(!data.Keys.Contains("rateModelId") || data["rateModelId"] == null ? null : data["rateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTargetInventoryModelId(!data.Keys.Contains("targetInventoryModelId") || data["targetInventoryModelId"] == null ? null : data["targetInventoryModelId"].ToString())
                .WithAcquireExperienceSuffix(!data.Keys.Contains("acquireExperienceSuffix") || data["acquireExperienceSuffix"] == null ? null : data["acquireExperienceSuffix"].ToString())
                .WithMaterialInventoryModelId(!data.Keys.Contains("materialInventoryModelId") || data["materialInventoryModelId"] == null ? null : data["materialInventoryModelId"].ToString())
                .WithAcquireExperienceHierarchy(!data.Keys.Contains("acquireExperienceHierarchy") || data["acquireExperienceHierarchy"] == null || !data["acquireExperienceHierarchy"].IsArray ? null : data["acquireExperienceHierarchy"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithBonusRates(!data.Keys.Contains("bonusRates") || data["bonusRates"] == null || !data["bonusRates"].IsArray ? null : data["bonusRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.BonusRate.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData acquireExperienceHierarchyJsonData = null;
            if (AcquireExperienceHierarchy != null && AcquireExperienceHierarchy.Length > 0)
            {
                acquireExperienceHierarchyJsonData = new JsonData();
                foreach (var acquireExperienceHierarch in AcquireExperienceHierarchy)
                {
                    acquireExperienceHierarchyJsonData.Add(acquireExperienceHierarch);
                }
            }
            JsonData bonusRatesJsonData = null;
            if (BonusRates != null && BonusRates.Length > 0)
            {
                bonusRatesJsonData = new JsonData();
                foreach (var bonusRate in BonusRates)
                {
                    bonusRatesJsonData.Add(bonusRate.ToJson());
                }
            }
            return new JsonData {
                ["rateModelId"] = RateModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["targetInventoryModelId"] = TargetInventoryModelId,
                ["acquireExperienceSuffix"] = AcquireExperienceSuffix,
                ["materialInventoryModelId"] = MaterialInventoryModelId,
                ["acquireExperienceHierarchy"] = acquireExperienceHierarchyJsonData,
                ["experienceModelId"] = ExperienceModelId,
                ["bonusRates"] = bonusRatesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RateModelId != null) {
                writer.WritePropertyName("rateModelId");
                writer.Write(RateModelId.ToString());
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
            if (AcquireExperienceSuffix != null) {
                writer.WritePropertyName("acquireExperienceSuffix");
                writer.Write(AcquireExperienceSuffix.ToString());
            }
            if (MaterialInventoryModelId != null) {
                writer.WritePropertyName("materialInventoryModelId");
                writer.Write(MaterialInventoryModelId.ToString());
            }
            if (AcquireExperienceHierarchy != null) {
                writer.WritePropertyName("acquireExperienceHierarchy");
                writer.WriteArrayStart();
                foreach (var acquireExperienceHierarch in AcquireExperienceHierarchy)
                {
                    if (acquireExperienceHierarch != null) {
                        writer.Write(acquireExperienceHierarch.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (BonusRates != null) {
                writer.WritePropertyName("bonusRates");
                writer.WriteArrayStart();
                foreach (var bonusRate in BonusRates)
                {
                    if (bonusRate != null) {
                        bonusRate.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RateModel;
            var diff = 0;
            if (RateModelId == null && RateModelId == other.RateModelId)
            {
                // null and null
            }
            else
            {
                diff += RateModelId.CompareTo(other.RateModelId);
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
            if (AcquireExperienceSuffix == null && AcquireExperienceSuffix == other.AcquireExperienceSuffix)
            {
                // null and null
            }
            else
            {
                diff += AcquireExperienceSuffix.CompareTo(other.AcquireExperienceSuffix);
            }
            if (MaterialInventoryModelId == null && MaterialInventoryModelId == other.MaterialInventoryModelId)
            {
                // null and null
            }
            else
            {
                diff += MaterialInventoryModelId.CompareTo(other.MaterialInventoryModelId);
            }
            if (AcquireExperienceHierarchy == null && AcquireExperienceHierarchy == other.AcquireExperienceHierarchy)
            {
                // null and null
            }
            else
            {
                diff += AcquireExperienceHierarchy.Length - other.AcquireExperienceHierarchy.Length;
                for (var i = 0; i < AcquireExperienceHierarchy.Length; i++)
                {
                    diff += AcquireExperienceHierarchy[i].CompareTo(other.AcquireExperienceHierarchy[i]);
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
            if (BonusRates == null && BonusRates == other.BonusRates)
            {
                // null and null
            }
            else
            {
                diff += BonusRates.Length - other.BonusRates.Length;
                for (var i = 0; i < BonusRates.Length; i++)
                {
                    diff += BonusRates[i].CompareTo(other.BonusRates[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (RateModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.rateModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (TargetInventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.targetInventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (AcquireExperienceSuffix.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.acquireExperienceSuffix.error.tooLong"),
                    });
                }
            }
            {
                if (MaterialInventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.materialInventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (AcquireExperienceHierarchy.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.acquireExperienceHierarchy.error.tooMany"),
                    });
                }
            }
            {
                if (ExperienceModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.experienceModelId.error.tooLong"),
                    });
                }
            }
            {
                if (BonusRates.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rateModel", "enhance.rateModel.bonusRates.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new RateModel {
                RateModelId = RateModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                TargetInventoryModelId = TargetInventoryModelId,
                AcquireExperienceSuffix = AcquireExperienceSuffix,
                MaterialInventoryModelId = MaterialInventoryModelId,
                AcquireExperienceHierarchy = AcquireExperienceHierarchy.Clone() as string[],
                ExperienceModelId = ExperienceModelId,
                BonusRates = BonusRates.Clone() as Gs2.Gs2Enhance.Model.BonusRate[],
            };
        }
    }
}