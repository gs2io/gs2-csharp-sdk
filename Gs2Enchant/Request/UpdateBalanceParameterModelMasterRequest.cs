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
        public string NamespaceName { set; get; }
        public string ParameterName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public long? TotalValue { set; get; }
        public string InitialValueStrategy { set; get; }
        public Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] Parameters { set; get; }
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
                .WithTotalValue(!data.Keys.Contains("totalValue") || data["totalValue"] == null ? null : (long?)long.Parse(data["totalValue"].ToString()))
                .WithInitialValueStrategy(!data.Keys.Contains("initialValueStrategy") || data["initialValueStrategy"] == null ? null : data["initialValueStrategy"].ToString())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null ? new Gs2.Gs2Enchant.Model.BalanceParameterValueModel[]{} : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.BalanceParameterValueModel.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["parameterName"] = ParameterName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["totalValue"] = TotalValue,
                ["initialValueStrategy"] = InitialValueStrategy,
                ["parameters"] = Parameters == null ? null : new JsonData(
                        Parameters.Select(v => {
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
                writer.Write(long.Parse(TotalValue.ToString()));
            }
            if (InitialValueStrategy != null) {
                writer.WritePropertyName("initialValueStrategy");
                writer.Write(InitialValueStrategy.ToString());
            }
            writer.WriteArrayStart();
            foreach (var parameter in Parameters)
            {
                if (parameter != null) {
                    parameter.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
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

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateBalanceParameterModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateBalanceParameterModelMasterRequest)x;
            return this;
        }
    }
}