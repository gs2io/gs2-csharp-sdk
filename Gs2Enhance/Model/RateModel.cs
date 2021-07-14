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
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Model
{

	[Preserve]
	public class RateModel : IComparable
	{
        public string RateModelId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string TargetInventoryModelId { set; get; }
        public string AcquireExperienceSuffix { set; get; }
        public string MaterialInventoryModelId { set; get; }
        public string[] AcquireExperienceHierarchy { set; get; }
        public string ExperienceModelId { set; get; }
        public Gs2.Gs2Enhance.Model.BonusRate[] BonusRates { set; get; }

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

    	[Preserve]
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
                .WithAcquireExperienceHierarchy(!data.Keys.Contains("acquireExperienceHierarchy") || data["acquireExperienceHierarchy"] == null ? new string[]{} : data["acquireExperienceHierarchy"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithBonusRates(!data.Keys.Contains("bonusRates") || data["bonusRates"] == null ? new Gs2.Gs2Enhance.Model.BonusRate[]{} : data["bonusRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enhance.Model.BonusRate.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["rateModelId"] = RateModelId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["targetInventoryModelId"] = TargetInventoryModelId,
                ["acquireExperienceSuffix"] = AcquireExperienceSuffix,
                ["materialInventoryModelId"] = MaterialInventoryModelId,
                ["acquireExperienceHierarchy"] = new JsonData(AcquireExperienceHierarchy == null ? new JsonData[]{} :
                        AcquireExperienceHierarchy.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["experienceModelId"] = ExperienceModelId,
                ["bonusRates"] = new JsonData(BonusRates == null ? new JsonData[]{} :
                        BonusRates.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
    }
}