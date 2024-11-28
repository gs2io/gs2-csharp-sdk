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
	public class BalanceParameterModelMaster : IComparable
	{
        public string BalanceParameterModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Description { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public long? TotalValue { set; get; } = null!;
        public string InitialValueStrategy { set; get; } = null!;
        public Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] Parameters { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public BalanceParameterModelMaster WithBalanceParameterModelId(string balanceParameterModelId) {
            this.BalanceParameterModelId = balanceParameterModelId;
            return this;
        }
        public BalanceParameterModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public BalanceParameterModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public BalanceParameterModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public BalanceParameterModelMaster WithTotalValue(long? totalValue) {
            this.TotalValue = totalValue;
            return this;
        }
        public BalanceParameterModelMaster WithInitialValueStrategy(string initialValueStrategy) {
            this.InitialValueStrategy = initialValueStrategy;
            return this;
        }
        public BalanceParameterModelMaster WithParameters(Gs2.Gs2Enchant.Model.BalanceParameterValueModel[] parameters) {
            this.Parameters = parameters;
            return this;
        }
        public BalanceParameterModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public BalanceParameterModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public BalanceParameterModelMaster WithRevision(long? revision) {
            this.Revision = revision;
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
        public static BalanceParameterModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BalanceParameterModelMaster()
                .WithBalanceParameterModelId(!data.Keys.Contains("balanceParameterModelId") || data["balanceParameterModelId"] == null ? null : data["balanceParameterModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTotalValue(!data.Keys.Contains("totalValue") || data["totalValue"] == null ? null : (long?)(data["totalValue"].ToString().Contains(".") ? (long)double.Parse(data["totalValue"].ToString()) : long.Parse(data["totalValue"].ToString())))
                .WithInitialValueStrategy(!data.Keys.Contains("initialValueStrategy") || data["initialValueStrategy"] == null ? null : data["initialValueStrategy"].ToString())
                .WithParameters(!data.Keys.Contains("parameters") || data["parameters"] == null || !data["parameters"].IsArray ? null : data["parameters"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Enchant.Model.BalanceParameterValueModel.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
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
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["totalValue"] = TotalValue,
                ["initialValueStrategy"] = InitialValueStrategy,
                ["parameters"] = parametersJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
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
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BalanceParameterModelMaster;
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
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
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (BalanceParameterModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.balanceParameterModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (TotalValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.totalValue.error.invalid"),
                    });
                }
                if (TotalValue > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.totalValue.error.invalid"),
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
                            new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.initialValueStrategy.error.invalid"),
                        });
                }
            }
            {
                if (Parameters.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.parameters.error.tooFew"),
                    });
                }
                if (Parameters.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.parameters.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("balanceParameterModelMaster", "enchant.balanceParameterModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BalanceParameterModelMaster {
                BalanceParameterModelId = BalanceParameterModelId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                TotalValue = TotalValue,
                InitialValueStrategy = InitialValueStrategy,
                Parameters = Parameters.Clone() as Gs2.Gs2Enchant.Model.BalanceParameterValueModel[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}