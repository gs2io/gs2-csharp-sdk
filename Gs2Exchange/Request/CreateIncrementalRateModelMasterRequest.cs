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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateIncrementalRateModelMasterRequest : Gs2Request<CreateIncrementalRateModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.ConsumeAction ConsumeAction { set; get; }
        public string CalculateType { set; get; }
        public long? BaseValue { set; get; }
        public long? CoefficientValue { set; get; }
        public string CalculateScriptId { set; get; }
        public string ExchangeCountId { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public CreateIncrementalRateModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithConsumeAction(Gs2.Core.Model.ConsumeAction consumeAction) {
            this.ConsumeAction = consumeAction;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithCalculateType(string calculateType) {
            this.CalculateType = calculateType;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithBaseValue(long? baseValue) {
            this.BaseValue = baseValue;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithCoefficientValue(long? coefficientValue) {
            this.CoefficientValue = coefficientValue;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithCalculateScriptId(string calculateScriptId) {
            this.CalculateScriptId = calculateScriptId;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithExchangeCountId(string exchangeCountId) {
            this.ExchangeCountId = exchangeCountId;
            return this;
        }
        public CreateIncrementalRateModelMasterRequest WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateIncrementalRateModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateIncrementalRateModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithConsumeAction(!data.Keys.Contains("consumeAction") || data["consumeAction"] == null ? null : Gs2.Core.Model.ConsumeAction.FromJson(data["consumeAction"]))
                .WithCalculateType(!data.Keys.Contains("calculateType") || data["calculateType"] == null ? null : data["calculateType"].ToString())
                .WithBaseValue(!data.Keys.Contains("baseValue") || data["baseValue"] == null ? null : (long?)long.Parse(data["baseValue"].ToString()))
                .WithCoefficientValue(!data.Keys.Contains("coefficientValue") || data["coefficientValue"] == null ? null : (long?)long.Parse(data["coefficientValue"].ToString()))
                .WithCalculateScriptId(!data.Keys.Contains("calculateScriptId") || data["calculateScriptId"] == null ? null : data["calculateScriptId"].ToString())
                .WithExchangeCountId(!data.Keys.Contains("exchangeCountId") || data["exchangeCountId"] == null ? null : data["exchangeCountId"].ToString())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["consumeAction"] = ConsumeAction?.ToJson(),
                ["calculateType"] = CalculateType,
                ["baseValue"] = BaseValue,
                ["coefficientValue"] = CoefficientValue,
                ["calculateScriptId"] = CalculateScriptId,
                ["exchangeCountId"] = ExchangeCountId,
                ["acquireActions"] = AcquireActions == null ? null : new JsonData(
                        AcquireActions.Select(v => {
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
            if (ConsumeAction != null) {
                ConsumeAction.WriteJson(writer);
            }
            if (CalculateType != null) {
                writer.WritePropertyName("calculateType");
                writer.Write(CalculateType.ToString());
            }
            if (BaseValue != null) {
                writer.WritePropertyName("baseValue");
                writer.Write(long.Parse(BaseValue.ToString()));
            }
            if (CoefficientValue != null) {
                writer.WritePropertyName("coefficientValue");
                writer.Write(long.Parse(CoefficientValue.ToString()));
            }
            if (CalculateScriptId != null) {
                writer.WritePropertyName("calculateScriptId");
                writer.Write(CalculateScriptId.ToString());
            }
            if (ExchangeCountId != null) {
                writer.WritePropertyName("exchangeCountId");
                writer.Write(ExchangeCountId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var acquireAction in AcquireActions)
            {
                if (acquireAction != null) {
                    acquireAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += ConsumeAction + ":";
            key += CalculateType + ":";
            key += BaseValue + ":";
            key += CoefficientValue + ":";
            key += CalculateScriptId + ":";
            key += ExchangeCountId + ":";
            key += AcquireActions + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateIncrementalRateModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateIncrementalRateModelMasterRequest)x;
            return this;
        }
    }
}