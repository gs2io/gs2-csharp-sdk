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
using Gs2.Gs2Enchant.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Enchant.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateRarityParameterModelMasterRequest : Gs2Request<UpdateRarityParameterModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ParameterName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? MaximumParameterCount { set; get; } = null!;
         public Gs2.Gs2Enchant.Model.RarityParameterCountModel[] ParameterCounts { set; get; } = null!;
         public Gs2.Gs2Enchant.Model.RarityParameterValueModel[] Parameters { set; get; } = null!;
        public UpdateRarityParameterModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithMaximumParameterCount(int? maximumParameterCount) {
            this.MaximumParameterCount = maximumParameterCount;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithParameterCounts(Gs2.Gs2Enchant.Model.RarityParameterCountModel[] parameterCounts) {
            this.ParameterCounts = parameterCounts;
            return this;
        }
        public UpdateRarityParameterModelMasterRequest WithParameters(Gs2.Gs2Enchant.Model.RarityParameterValueModel[] parameters) {
            this.Parameters = parameters;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRarityParameterModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRarityParameterModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithMaximumParameterCount(!data.Keys.Contains("maximumParameterCount") || data["maximumParameterCount"] == null ? null : (int?)(data["maximumParameterCount"].ToString().Contains(".") ? (int)double.Parse(data["maximumParameterCount"].ToString()) : int.Parse(data["maximumParameterCount"].ToString())))
                .WithParameterCounts(!data.Keys.Contains("parameterCounts") || data["parameterCounts"] == null || !data["parameterCounts"].IsArray ? null : data["parameterCounts"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.RarityParameterCountModel.FromJson(v);
                }).ToArray())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null || !data["parameters"].IsArray ? null : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.RarityParameterValueModel.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData parameterCountsJsonData = null;
            if (ParameterCounts != null && ParameterCounts.Length > 0)
            {
                parameterCountsJsonData = new JsonData();
                foreach (var parameterCount in ParameterCounts)
                {
                    parameterCountsJsonData.Add(parameterCount.ToJson());
                }
            }
            JsonData parametersJsonData = null;
            if (Parameters != null && Parameters.Length > 0)
            {
                parametersJsonData = new JsonData();
                foreach (var parameter in Parameters)
                {
                    parametersJsonData.Add(parameter.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["parameterName"] = ParameterName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["maximumParameterCount"] = MaximumParameterCount,
                ["parameterCounts"] = parameterCountsJsonData,
                ["parameters"] = parametersJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (ParameterName != null) {
                writer.WritePropertyName("parameterName");
                writer.Write(ParameterName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (MaximumParameterCount != null) {
                writer.WritePropertyName("maximumParameterCount");
                writer.Write((MaximumParameterCount.ToString().Contains(".") ? (int)double.Parse(MaximumParameterCount.ToString()) : int.Parse(MaximumParameterCount.ToString())));
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

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += ParameterName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += MaximumParameterCount + ":";
            key += ParameterCounts + ":";
            key += Parameters + ":";
            return key;
        }
    }
}