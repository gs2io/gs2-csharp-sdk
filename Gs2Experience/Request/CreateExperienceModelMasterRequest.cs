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
using Gs2.Gs2Experience.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Experience.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateExperienceModelMasterRequest : Gs2Request<CreateExperienceModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public long? DefaultExperience { set; get; } = null!;
         public long? DefaultRankCap { set; get; } = null!;
         public long? MaxRankCap { set; get; } = null!;
         public string RankThresholdName { set; get; } = null!;
         public Gs2.Gs2Experience.Model.AcquireActionRate[] AcquireActionRates { set; get; } = null!;
        public CreateExperienceModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateExperienceModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateExperienceModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateExperienceModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateExperienceModelMasterRequest WithDefaultExperience(long? defaultExperience) {
            this.DefaultExperience = defaultExperience;
            return this;
        }
        public CreateExperienceModelMasterRequest WithDefaultRankCap(long? defaultRankCap) {
            this.DefaultRankCap = defaultRankCap;
            return this;
        }
        public CreateExperienceModelMasterRequest WithMaxRankCap(long? maxRankCap) {
            this.MaxRankCap = maxRankCap;
            return this;
        }
        public CreateExperienceModelMasterRequest WithRankThresholdName(string rankThresholdName) {
            this.RankThresholdName = rankThresholdName;
            return this;
        }
        public CreateExperienceModelMasterRequest WithAcquireActionRates(Gs2.Gs2Experience.Model.AcquireActionRate[] acquireActionRates) {
            this.AcquireActionRates = acquireActionRates;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateExperienceModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateExperienceModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultExperience(!data.Keys.Contains("defaultExperience") || data["defaultExperience"] == null ? null : (long?)(data["defaultExperience"].ToString().Contains(".") ? (long)double.Parse(data["defaultExperience"].ToString()) : long.Parse(data["defaultExperience"].ToString())))
                .WithDefaultRankCap(!data.Keys.Contains("defaultRankCap") || data["defaultRankCap"] == null ? null : (long?)(data["defaultRankCap"].ToString().Contains(".") ? (long)double.Parse(data["defaultRankCap"].ToString()) : long.Parse(data["defaultRankCap"].ToString())))
                .WithMaxRankCap(!data.Keys.Contains("maxRankCap") || data["maxRankCap"] == null ? null : (long?)(data["maxRankCap"].ToString().Contains(".") ? (long)double.Parse(data["maxRankCap"].ToString()) : long.Parse(data["maxRankCap"].ToString())))
                .WithRankThresholdName(!data.Keys.Contains("rankThresholdName") || data["rankThresholdName"] == null ? null : data["rankThresholdName"].ToString())
                .WithAcquireActionRates(!data.Keys.Contains("acquireActionRates") || data["acquireActionRates"] == null || !data["acquireActionRates"].IsArray ? null : data["acquireActionRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Experience.Model.AcquireActionRate.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
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
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["defaultExperience"] = DefaultExperience,
                ["defaultRankCap"] = DefaultRankCap,
                ["maxRankCap"] = MaxRankCap,
                ["rankThresholdName"] = RankThresholdName,
                ["acquireActionRates"] = acquireActionRatesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
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
            if (DefaultExperience != null) {
                writer.WritePropertyName("defaultExperience");
                writer.Write((DefaultExperience.ToString().Contains(".") ? (long)double.Parse(DefaultExperience.ToString()) : long.Parse(DefaultExperience.ToString())));
            }
            if (DefaultRankCap != null) {
                writer.WritePropertyName("defaultRankCap");
                writer.Write((DefaultRankCap.ToString().Contains(".") ? (long)double.Parse(DefaultRankCap.ToString()) : long.Parse(DefaultRankCap.ToString())));
            }
            if (MaxRankCap != null) {
                writer.WritePropertyName("maxRankCap");
                writer.Write((MaxRankCap.ToString().Contains(".") ? (long)double.Parse(MaxRankCap.ToString()) : long.Parse(MaxRankCap.ToString())));
            }
            if (RankThresholdName != null) {
                writer.WritePropertyName("rankThresholdName");
                writer.Write(RankThresholdName.ToString());
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += DefaultExperience + ":";
            key += DefaultRankCap + ":";
            key += MaxRankCap + ":";
            key += RankThresholdName + ":";
            key += AcquireActionRates + ":";
            return key;
        }
    }
}