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

namespace Gs2.Gs2Enchant.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RarityParameterModel : IComparable
	{
        public string RarityParameterModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? MaximumParameterCount { set; get; }
        public Gs2.Gs2Enchant.Model.RarityParameterCountModel[] ParameterCounts { set; get; }
        public Gs2.Gs2Enchant.Model.RarityParameterValueModel[] Parameters { set; get; }

        public RarityParameterModel WithRarityParameterModelId(string rarityParameterModelId) {
            this.RarityParameterModelId = rarityParameterModelId;
            return this;
        }

        public RarityParameterModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public RarityParameterModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public RarityParameterModel WithMaximumParameterCount(int? maximumParameterCount) {
            this.MaximumParameterCount = maximumParameterCount;
            return this;
        }

        public RarityParameterModel WithParameterCounts(Gs2.Gs2Enchant.Model.RarityParameterCountModel[] parameterCounts) {
            this.ParameterCounts = parameterCounts;
            return this;
        }

        public RarityParameterModel WithParameters(Gs2.Gs2Enchant.Model.RarityParameterValueModel[] parameters) {
            this.Parameters = parameters;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:rarity:(?<parameterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:rarity:(?<parameterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:rarity:(?<parameterName>.+)",
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

        private static System.Text.RegularExpressions.Regex _parameterNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:rarity:(?<parameterName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetParameterNameFromGrn(
            string grn
        )
        {
            var match = _parameterNameRegex.Match(grn);
            if (!match.Success || !match.Groups["parameterName"].Success)
            {
                return null;
            }
            return match.Groups["parameterName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RarityParameterModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RarityParameterModel()
                .WithRarityParameterModelId(!data.Keys.Contains("rarityParameterModelId") || data["rarityParameterModelId"] == null ? null : data["rarityParameterModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumParameterCount(!data.Keys.Contains("maximumParameterCount") || data["maximumParameterCount"] == null ? null : (int?)int.Parse(data["maximumParameterCount"].ToString()))
                .WithParameterCounts(!data.Keys.Contains("parameterCounts") || data["parameterCounts"] == null ? new Gs2.Gs2Enchant.Model.RarityParameterCountModel[]{} : data["parameterCounts"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.RarityParameterCountModel.FromJson(v);
                }).ToArray())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null ? new Gs2.Gs2Enchant.Model.RarityParameterValueModel[]{} : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.RarityParameterValueModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData parameterCountsJsonData = null;
            if (ParameterCounts != null)
            {
                parameterCountsJsonData = new JsonData();
                foreach (var parameterCount in ParameterCounts)
                {
                    parameterCountsJsonData.Add(parameterCount.ToJson());
                }
            }
            JsonData parametersJsonData = null;
            if (Parameters != null)
            {
                parametersJsonData = new JsonData();
                foreach (var parameter in Parameters)
                {
                    parametersJsonData.Add(parameter.ToJson());
                }
            }
            return new JsonData {
                ["rarityParameterModelId"] = RarityParameterModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["maximumParameterCount"] = MaximumParameterCount,
                ["parameterCounts"] = parameterCountsJsonData,
                ["parameters"] = parametersJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RarityParameterModelId != null) {
                writer.WritePropertyName("rarityParameterModelId");
                writer.Write(RarityParameterModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MaximumParameterCount != null) {
                writer.WritePropertyName("maximumParameterCount");
                writer.Write(int.Parse(MaximumParameterCount.ToString()));
            }
            if (ParameterCounts != null) {
                writer.WritePropertyName("parameterCounts");
                writer.WriteArrayStart();
                foreach (var parameterCount in ParameterCounts)
                {
                    if (parameterCount != null) {
                        parameterCount.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Parameters != null) {
                writer.WritePropertyName("parameters");
                writer.WriteArrayStart();
                foreach (var parameter in Parameters)
                {
                    if (parameter != null) {
                        parameter.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RarityParameterModel;
            var diff = 0;
            if (RarityParameterModelId == null && RarityParameterModelId == other.RarityParameterModelId)
            {
                // null and null
            }
            else
            {
                diff += RarityParameterModelId.CompareTo(other.RarityParameterModelId);
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
            if (MaximumParameterCount == null && MaximumParameterCount == other.MaximumParameterCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumParameterCount - other.MaximumParameterCount);
            }
            if (ParameterCounts == null && ParameterCounts == other.ParameterCounts)
            {
                // null and null
            }
            else
            {
                diff += ParameterCounts.Length - other.ParameterCounts.Length;
                for (var i = 0; i < ParameterCounts.Length; i++)
                {
                    diff += ParameterCounts[i].CompareTo(other.ParameterCounts[i]);
                }
            }
            if (Parameters == null && Parameters == other.Parameters)
            {
                // null and null
            }
            else
            {
                diff += Parameters.Length - other.Parameters.Length;
                for (var i = 0; i < Parameters.Length; i++)
                {
                    diff += Parameters[i].CompareTo(other.Parameters[i]);
                }
            }
            return diff;
        }
    }
}