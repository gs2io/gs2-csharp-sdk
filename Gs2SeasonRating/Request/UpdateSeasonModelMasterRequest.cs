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
using Gs2.Gs2SeasonRating.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2SeasonRating.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateSeasonModelMasterRequest : Gs2Request<UpdateSeasonModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string SeasonName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2SeasonRating.Model.TierModel[] Tiers { set; get; } = null!;
         public string ExperienceModelId { set; get; } = null!;
         public string ChallengePeriodEventId { set; get; } = null!;
        public UpdateSeasonModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithSeasonName(string seasonName) {
            this.SeasonName = seasonName;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithTiers(Gs2.Gs2SeasonRating.Model.TierModel[] tiers) {
            this.Tiers = tiers;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public UpdateSeasonModelMasterRequest WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateSeasonModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateSeasonModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithSeasonName(!data.Keys.Contains("seasonName") || data["seasonName"] == null ? null : data["seasonName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTiers(!data.Keys.Contains("tiers") || data["tiers"] == null || !data["tiers"].IsArray ? null : data["tiers"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2SeasonRating.Model.TierModel.FromJson(v);
                }).ToArray())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData tiersJsonData = null;
            if (Tiers != null && Tiers.Length > 0)
            {
                tiersJsonData = new JsonData();
                foreach (var tier in Tiers)
                {
                    tiersJsonData.Add(tier.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["seasonName"] = SeasonName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["tiers"] = tiersJsonData,
                ["experienceModelId"] = ExperienceModelId,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (SeasonName != null) {
                writer.WritePropertyName("seasonName");
                writer.Write(SeasonName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Tiers != null) {
                writer.WritePropertyName("tiers");
                writer.WriteArrayStart();
                foreach (var tier in Tiers)
                {
                    if (tier != null) {
                        tier.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += SeasonName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Tiers + ":";
            key += ExperienceModelId + ":";
            key += ChallengePeriodEventId + ":";
            return key;
        }
    }
}