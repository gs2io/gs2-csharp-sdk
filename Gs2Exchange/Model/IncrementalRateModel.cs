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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class IncrementalRateModel : IComparable
	{
        public string IncrementalRateModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction ConsumeAction { set; get; } = null!;
        public string CalculateType { set; get; } = null!;
        public long? BaseValue { set; get; } = null!;
        public long? CoefficientValue { set; get; } = null!;
        public string CalculateScriptId { set; get; } = null!;
        public string ExchangeCountId { set; get; } = null!;
        public int? MaximumExchangeCount { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; } = null!;
        public IncrementalRateModel WithIncrementalRateModelId(string incrementalRateModelId) {
            this.IncrementalRateModelId = incrementalRateModelId;
            return this;
        }
        public IncrementalRateModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public IncrementalRateModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public IncrementalRateModel WithConsumeAction(Gs2.Core.Model.ConsumeAction consumeAction) {
            this.ConsumeAction = consumeAction;
            return this;
        }
        public IncrementalRateModel WithCalculateType(string calculateType) {
            this.CalculateType = calculateType;
            return this;
        }
        public IncrementalRateModel WithBaseValue(long? baseValue) {
            this.BaseValue = baseValue;
            return this;
        }
        public IncrementalRateModel WithCoefficientValue(long? coefficientValue) {
            this.CoefficientValue = coefficientValue;
            return this;
        }
        public IncrementalRateModel WithCalculateScriptId(string calculateScriptId) {
            this.CalculateScriptId = calculateScriptId;
            return this;
        }
        public IncrementalRateModel WithExchangeCountId(string exchangeCountId) {
            this.ExchangeCountId = exchangeCountId;
            return this;
        }
        public IncrementalRateModel WithMaximumExchangeCount(int? maximumExchangeCount) {
            this.MaximumExchangeCount = maximumExchangeCount;
            return this;
        }
        public IncrementalRateModel WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):incremental:model:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):incremental:model:(?<rateName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):incremental:model:(?<rateName>.+)",
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

        private static System.Text.RegularExpressions.Regex _rateNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):incremental:model:(?<rateName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRateNameFromGrn(
            string grn
        )
        {
            var match = _rateNameRegex.Match(grn);
            if (!match.Success || !match.Groups["rateName"].Success)
            {
                return null;
            }
            return match.Groups["rateName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IncrementalRateModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IncrementalRateModel()
                .WithIncrementalRateModelId(!data.Keys.Contains("incrementalRateModelId") || data["incrementalRateModelId"] == null ? null : data["incrementalRateModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithConsumeAction(!data.Keys.Contains("consumeAction") || data["consumeAction"] == null ? null : Gs2.Core.Model.ConsumeAction.FromJson(data["consumeAction"]))
                .WithCalculateType(!data.Keys.Contains("calculateType") || data["calculateType"] == null ? null : data["calculateType"].ToString())
                .WithBaseValue(!data.Keys.Contains("baseValue") || data["baseValue"] == null ? null : (long?)(data["baseValue"].ToString().Contains(".") ? (long)double.Parse(data["baseValue"].ToString()) : long.Parse(data["baseValue"].ToString())))
                .WithCoefficientValue(!data.Keys.Contains("coefficientValue") || data["coefficientValue"] == null ? null : (long?)(data["coefficientValue"].ToString().Contains(".") ? (long)double.Parse(data["coefficientValue"].ToString()) : long.Parse(data["coefficientValue"].ToString())))
                .WithCalculateScriptId(!data.Keys.Contains("calculateScriptId") || data["calculateScriptId"] == null ? null : data["calculateScriptId"].ToString())
                .WithExchangeCountId(!data.Keys.Contains("exchangeCountId") || data["exchangeCountId"] == null ? null : data["exchangeCountId"].ToString())
                .WithMaximumExchangeCount(!data.Keys.Contains("maximumExchangeCount") || data["maximumExchangeCount"] == null ? null : (int?)(data["maximumExchangeCount"].ToString().Contains(".") ? (int)double.Parse(data["maximumExchangeCount"].ToString()) : int.Parse(data["maximumExchangeCount"].ToString())))
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["incrementalRateModelId"] = IncrementalRateModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["consumeAction"] = ConsumeAction?.ToJson(),
                ["calculateType"] = CalculateType,
                ["baseValue"] = BaseValue,
                ["coefficientValue"] = CoefficientValue,
                ["calculateScriptId"] = CalculateScriptId,
                ["exchangeCountId"] = ExchangeCountId,
                ["maximumExchangeCount"] = MaximumExchangeCount,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (IncrementalRateModelId != null) {
                writer.WritePropertyName("incrementalRateModelId");
                writer.Write(IncrementalRateModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (ConsumeAction != null) {
                writer.WritePropertyName("consumeAction");
                ConsumeAction.WriteJson(writer);
            }
            if (CalculateType != null) {
                writer.WritePropertyName("calculateType");
                writer.Write(CalculateType.ToString());
            }
            if (BaseValue != null) {
                writer.WritePropertyName("baseValue");
                writer.Write((BaseValue.ToString().Contains(".") ? (long)double.Parse(BaseValue.ToString()) : long.Parse(BaseValue.ToString())));
            }
            if (CoefficientValue != null) {
                writer.WritePropertyName("coefficientValue");
                writer.Write((CoefficientValue.ToString().Contains(".") ? (long)double.Parse(CoefficientValue.ToString()) : long.Parse(CoefficientValue.ToString())));
            }
            if (CalculateScriptId != null) {
                writer.WritePropertyName("calculateScriptId");
                writer.Write(CalculateScriptId.ToString());
            }
            if (ExchangeCountId != null) {
                writer.WritePropertyName("exchangeCountId");
                writer.Write(ExchangeCountId.ToString());
            }
            if (MaximumExchangeCount != null) {
                writer.WritePropertyName("maximumExchangeCount");
                writer.Write((MaximumExchangeCount.ToString().Contains(".") ? (int)double.Parse(MaximumExchangeCount.ToString()) : int.Parse(MaximumExchangeCount.ToString())));
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as IncrementalRateModel;
            var diff = 0;
            if (IncrementalRateModelId == null && IncrementalRateModelId == other.IncrementalRateModelId)
            {
                // null and null
            }
            else
            {
                diff += IncrementalRateModelId.CompareTo(other.IncrementalRateModelId);
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
            if (ConsumeAction == null && ConsumeAction == other.ConsumeAction)
            {
                // null and null
            }
            else
            {
                diff += ConsumeAction.CompareTo(other.ConsumeAction);
            }
            if (CalculateType == null && CalculateType == other.CalculateType)
            {
                // null and null
            }
            else
            {
                diff += CalculateType.CompareTo(other.CalculateType);
            }
            if (BaseValue == null && BaseValue == other.BaseValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(BaseValue - other.BaseValue);
            }
            if (CoefficientValue == null && CoefficientValue == other.CoefficientValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(CoefficientValue - other.CoefficientValue);
            }
            if (CalculateScriptId == null && CalculateScriptId == other.CalculateScriptId)
            {
                // null and null
            }
            else
            {
                diff += CalculateScriptId.CompareTo(other.CalculateScriptId);
            }
            if (ExchangeCountId == null && ExchangeCountId == other.ExchangeCountId)
            {
                // null and null
            }
            else
            {
                diff += ExchangeCountId.CompareTo(other.ExchangeCountId);
            }
            if (MaximumExchangeCount == null && MaximumExchangeCount == other.MaximumExchangeCount)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumExchangeCount - other.MaximumExchangeCount);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (IncrementalRateModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.incrementalRateModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
            {
                switch (CalculateType) {
                    case "linear":
                    case "power":
                    case "gs2_script":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("incrementalRateModel", "exchange.incrementalRateModel.calculateType.error.invalid"),
                        });
                }
            }
            if (CalculateType == "linear") {
                if (BaseValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.baseValue.error.invalid"),
                    });
                }
                if (BaseValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.baseValue.error.invalid"),
                    });
                }
            }
            if ((CalculateType =="linear" || CalculateType == "power")) {
                if (CoefficientValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.coefficientValue.error.invalid"),
                    });
                }
                if (CoefficientValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.coefficientValue.error.invalid"),
                    });
                }
            }
            if (CalculateType == "gs2_script") {
                if (CalculateScriptId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.calculateScriptId.error.tooLong"),
                    });
                }
            }
            {
                if (ExchangeCountId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.exchangeCountId.error.tooLong"),
                    });
                }
            }
            {
                if (MaximumExchangeCount < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.maximumExchangeCount.error.invalid"),
                    });
                }
                if (MaximumExchangeCount > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.maximumExchangeCount.error.invalid"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("incrementalRateModel", "exchange.incrementalRateModel.acquireActions.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new IncrementalRateModel {
                IncrementalRateModelId = IncrementalRateModelId,
                Name = Name,
                Metadata = Metadata,
                ConsumeAction = ConsumeAction.Clone() as Gs2.Core.Model.ConsumeAction,
                CalculateType = CalculateType,
                BaseValue = BaseValue,
                CoefficientValue = CoefficientValue,
                CalculateScriptId = CalculateScriptId,
                ExchangeCountId = ExchangeCountId,
                MaximumExchangeCount = MaximumExchangeCount,
                AcquireActions = AcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
            };
        }
    }
}