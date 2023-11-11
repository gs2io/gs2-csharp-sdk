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

namespace Gs2.Gs2Experience.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ExperienceModel : IComparable
	{
        public string ExperienceModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public long? DefaultExperience { set; get; }
        public long? DefaultRankCap { set; get; }
        public long? MaxRankCap { set; get; }
        public Gs2.Gs2Experience.Model.Threshold RankThreshold { set; get; }
        public Gs2.Gs2Experience.Model.AcquireActionRate[] AcquireActionRates { set; get; }

        public ExperienceModel WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }

        public ExperienceModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public ExperienceModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public ExperienceModel WithDefaultExperience(long? defaultExperience) {
            this.DefaultExperience = defaultExperience;
            return this;
        }

        public ExperienceModel WithDefaultRankCap(long? defaultRankCap) {
            this.DefaultRankCap = defaultRankCap;
            return this;
        }

        public ExperienceModel WithMaxRankCap(long? maxRankCap) {
            this.MaxRankCap = maxRankCap;
            return this;
        }

        public ExperienceModel WithRankThreshold(Gs2.Gs2Experience.Model.Threshold rankThreshold) {
            this.RankThreshold = rankThreshold;
            return this;
        }

        public ExperienceModel WithAcquireActionRates(Gs2.Gs2Experience.Model.AcquireActionRate[] acquireActionRates) {
            this.AcquireActionRates = acquireActionRates;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):model:(?<experienceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):model:(?<experienceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):model:(?<experienceName>.+)",
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

        private static System.Text.RegularExpressions.Regex _experienceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):experience:(?<namespaceName>.+):model:(?<experienceName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetExperienceNameFromGrn(
            string grn
        )
        {
            var match = _experienceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["experienceName"].Success)
            {
                return null;
            }
            return match.Groups["experienceName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ExperienceModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ExperienceModel()
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultExperience(!data.Keys.Contains("defaultExperience") || data["defaultExperience"] == null ? null : (long?)long.Parse(data["defaultExperience"].ToString()))
                .WithDefaultRankCap(!data.Keys.Contains("defaultRankCap") || data["defaultRankCap"] == null ? null : (long?)long.Parse(data["defaultRankCap"].ToString()))
                .WithMaxRankCap(!data.Keys.Contains("maxRankCap") || data["maxRankCap"] == null ? null : (long?)long.Parse(data["maxRankCap"].ToString()))
                .WithRankThreshold(!data.Keys.Contains("rankThreshold") || data["rankThreshold"] == null ? null : Gs2.Gs2Experience.Model.Threshold.FromJson(data["rankThreshold"]))
                .WithAcquireActionRates(!data.Keys.Contains("acquireActionRates") || data["acquireActionRates"] == null || !data["acquireActionRates"].IsArray ? new Gs2.Gs2Experience.Model.AcquireActionRate[]{} : data["acquireActionRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Experience.Model.AcquireActionRate.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData acquireActionRatesJsonData = null;
            if (AcquireActionRates != null)
            {
                acquireActionRatesJsonData = new JsonData();
                foreach (var acquireActionRate in AcquireActionRates)
                {
                    acquireActionRatesJsonData.Add(acquireActionRate.ToJson());
                }
            }
            return new JsonData {
                ["experienceModelId"] = ExperienceModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["defaultExperience"] = DefaultExperience,
                ["defaultRankCap"] = DefaultRankCap,
                ["maxRankCap"] = MaxRankCap,
                ["rankThreshold"] = RankThreshold?.ToJson(),
                ["acquireActionRates"] = acquireActionRatesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (DefaultExperience != null) {
                writer.WritePropertyName("defaultExperience");
                writer.Write(long.Parse(DefaultExperience.ToString()));
            }
            if (DefaultRankCap != null) {
                writer.WritePropertyName("defaultRankCap");
                writer.Write(long.Parse(DefaultRankCap.ToString()));
            }
            if (MaxRankCap != null) {
                writer.WritePropertyName("maxRankCap");
                writer.Write(long.Parse(MaxRankCap.ToString()));
            }
            if (RankThreshold != null) {
                writer.WritePropertyName("rankThreshold");
                RankThreshold.WriteJson(writer);
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

        public int CompareTo(object obj)
        {
            var other = obj as ExperienceModel;
            var diff = 0;
            if (ExperienceModelId == null && ExperienceModelId == other.ExperienceModelId)
            {
                // null and null
            }
            else
            {
                diff += ExperienceModelId.CompareTo(other.ExperienceModelId);
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
            if (DefaultExperience == null && DefaultExperience == other.DefaultExperience)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultExperience - other.DefaultExperience);
            }
            if (DefaultRankCap == null && DefaultRankCap == other.DefaultRankCap)
            {
                // null and null
            }
            else
            {
                diff += (int)(DefaultRankCap - other.DefaultRankCap);
            }
            if (MaxRankCap == null && MaxRankCap == other.MaxRankCap)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxRankCap - other.MaxRankCap);
            }
            if (RankThreshold == null && RankThreshold == other.RankThreshold)
            {
                // null and null
            }
            else
            {
                diff += RankThreshold.CompareTo(other.RankThreshold);
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
            return diff;
        }
    }
}