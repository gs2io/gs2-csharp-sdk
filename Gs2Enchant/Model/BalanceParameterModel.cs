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
	public class BalanceParameterModel : IComparable
	{
        public string BalanceParameterModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? TotalValue { set; get; } = null!;
        public string InitialValueStrategy { set; get; } = null!;
        public Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] Parameters { set; get; } = null!;
        public BalanceParameterModel WithBalanceParameterModelId(string balanceParameterModelId) {
            this.BalanceParameterModelId = balanceParameterModelId;
            return this;
        }
        public BalanceParameterModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public BalanceParameterModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BalanceParameterModel WithTotalValue(long? totalValue) {
            this.TotalValue = totalValue;
            return this;
        }
        public BalanceParameterModel WithInitialValueStrategy(string initialValueStrategy) {
            this.InitialValueStrategy = initialValueStrategy;
            return this;
        }
        public BalanceParameterModel WithParameters(Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] parameters) {
            this.Parameters = parameters;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:balance:(?<parameterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:balance:(?<parameterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:balance:(?<parameterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):enchant:(?<namespaceName>.+):model:balance:(?<parameterName>.+)",
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
        public static BalanceParameterModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BalanceParameterModel()
                .WithBalanceParameterModelId(!data.Keys.Contains("balanceParameterModelId") || data["balanceParameterModelId"] == null ? null : data["balanceParameterModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTotalValue(!data.Keys.Contains("totalValue") || data["totalValue"] == null ? null : (long?)(data["totalValue"].ToString().Contains(".") ? (long)double.Parse(data["totalValue"].ToString()) : long.Parse(data["totalValue"].ToString())))
                .WithInitialValueStrategy(!data.Keys.Contains("initialValueStrategy") || data["initialValueStrategy"] == null ? null : data["initialValueStrategy"].ToString())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null || !data["parameters"].IsArray ? null : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.BalanceParameterValueModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
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
                ["balanceParameterModelId"] = BalanceParameterModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["totalValue"] = TotalValue,
                ["initialValueStrategy"] = InitialValueStrategy,
                ["parameters"] = parametersJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BalanceParameterModelId != null) {
                writer.WritePropertyName("balanceParameterModelId");
                writer.Write(BalanceParameterModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
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

        public int CompareTo(object obj)
        {
            var other = obj as BalanceParameterModel;
            var diff = 0;
            if (BalanceParameterModelId == null && BalanceParameterModelId == other.BalanceParameterModelId)
            {
                // null and null
            }
            else
            {
                diff += BalanceParameterModelId.CompareTo(other.BalanceParameterModelId);
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
            if (TotalValue == null && TotalValue == other.TotalValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(TotalValue - other.TotalValue);
            }
            if (InitialValueStrategy == null && InitialValueStrategy == other.InitialValueStrategy)
            {
                // null and null
            }
            else
            {
                diff += InitialValueStrategy.CompareTo(other.InitialValueStrategy);
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

        public void Validate() {
            {
                if (BalanceParameterModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.balanceParameterModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (TotalValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.totalValue.error.invalid"),
                    });
                }
                if (TotalValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.totalValue.error.invalid"),
                    });
                }
            }
            {
                switch (InitialValueStrategy) {
                    case "average":
                    case "lottery":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("balanceParameterModel", "enchant.balanceParameterModel.initialValueStrategy.error.invalid"),
                        });
                }
            }
            {
                if (Parameters.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.parameters.error.tooFew"),
                    });
                }
                if (Parameters.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModel", "enchant.balanceParameterModel.parameters.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new BalanceParameterModel {
                BalanceParameterModelId = BalanceParameterModelId,
                Name = Name,
                Metadata = Metadata,
                TotalValue = TotalValue,
                InitialValueStrategy = InitialValueStrategy,
                Parameters = Parameters.Clone() as Gs2.Gs2Enchant.Model.BalanceParameterValueModel[],
            };
        }
    }
}