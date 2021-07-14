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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Enhance.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Enhance.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateRateModelMasterRequest : Gs2Request<UpdateRateModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string RateName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string TargetInventoryModelId { set; get; }
        public string AcquireExperienceSuffix { set; get; }
        public string MaterialInventoryModelId { set; get; }
        public string[] AcquireExperienceHierarchy { set; get; }
        public string ExperienceModelId { set; get; }
        public Gs2.Gs2Enhance.Model.BonusRate[] BonusRates { set; get; }

        public UpdateRateModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public UpdateRateModelMasterRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }

        public UpdateRateModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }

        public UpdateRateModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public UpdateRateModelMasterRequest WithTargetInventoryModelId(string targetInventoryModelId) {
            this.TargetInventoryModelId = targetInventoryModelId;
            return this;
        }

        public UpdateRateModelMasterRequest WithAcquireExperienceSuffix(string acquireExperienceSuffix) {
            this.AcquireExperienceSuffix = acquireExperienceSuffix;
            return this;
        }

        public UpdateRateModelMasterRequest WithMaterialInventoryModelId(string materialInventoryModelId) {
            this.MaterialInventoryModelId = materialInventoryModelId;
            return this;
        }

        public UpdateRateModelMasterRequest WithAcquireExperienceHierarchy(string[] acquireExperienceHierarchy) {
            this.AcquireExperienceHierarchy = acquireExperienceHierarchy;
            return this;
        }

        public UpdateRateModelMasterRequest WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }

        public UpdateRateModelMasterRequest WithBonusRates(Gs2.Gs2Enhance.Model.BonusRate[] bonusRates) {
            this.BonusRates = bonusRates;
            return this;
        }

    	[Preserve]
        public static UpdateRateModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRateModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
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
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
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
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
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
            writer.WriteArrayStart();
            foreach (var acquireExperienceHierarch in AcquireExperienceHierarchy)
            {
                writer.Write(acquireExperienceHierarch.ToString());
            }
            writer.WriteArrayEnd();
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var bonusRate in BonusRates)
            {
                if (bonusRate != null) {
                    bonusRate.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }
    }
}