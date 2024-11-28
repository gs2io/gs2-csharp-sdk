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
	public class UpdateBalanceParameterModelMasterRequest : Gs2Request<UpdateBalanceParameterModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string ParameterName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public long? TotalValue { set; get; } = null!;
         public string InitialValueStrategy { set; get; } = null!;
         public Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] Parameters { set; get; } = null!;
        public UpdateBalanceParameterModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithParameterName(string parameterName) {
            this.ParameterName = parameterName;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithTotalValue(long? totalValue) {
            this.TotalValue = totalValue;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithInitialValueStrategy(string initialValueStrategy) {
            this.InitialValueStrategy = initialValueStrategy;
            return this;
        }
        public UpdateBalanceParameterModelMasterRequest WithParameters(Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] parameters) {
            this.Parameters = parameters;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateBalanceParameterModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateBalanceParameterModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithParameterName(!data.Keys.Contains("parameterName") || data["parameterName"] == null ? null : data["parameterName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTotalValue(!data.Keys.Contains("totalValue") || data["totalValue"] == null ? null : (long?)(data["totalValue"].ToString().Contains(".") ? (long)double.Parse(data["totalValue"].ToString()) : long.Parse(data["totalValue"].ToString())))
                .WithInitialValueStrategy(!data.Keys.Contains("initialValueStrategy") || data["initialValueStrategy"] == null ? null : data["initialValueStrategy"].ToString())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null || !data["parameters"].IsArray ? null : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.BalanceParameterValueModel.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
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
                ["totalValue"] = TotalValue,
                ["initialValueStrategy"] = InitialValueStrategy,
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
            if (TotalValue != null) {
                writer.WritePropertyName("totalValue");
                writer.Write((TotalValue.ToString().Contains(".") ? (long)double.Parse(TotalValue.ToString()) : long.Parse(TotalValue.ToString())));
            }
            if (InitialValueStrategy != null) {
                writer.WritePropertyName("initialValueStrategy");
                writer.Write(InitialValueStrategy.ToString());
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
            key += TotalValue + ":";
            key += InitialValueStrategy + ":";
            key += Parameters + ":";
            return key;
        }
    }
}