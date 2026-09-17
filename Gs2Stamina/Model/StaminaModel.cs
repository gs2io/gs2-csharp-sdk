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
 * deny overwrite
 */

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Stamina.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
/* diff --- start
	public partial class StaminaModel : IComparable
 diff --- end */
	public class StaminaModel : IComparable /* diff +++ */
	{
        public string StaminaModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? RecoverIntervalMinutes { set; get; }
        public int? RecoverValue { set; get; }
        public int? InitialCapacity { set; get; }
        public bool? IsOverflow { set; get; }
        public int? MaxCapacity { set; get; }
        public Gs2.Gs2Stamina.Model.MaxStaminaTable MaxStaminaTable { set; get; }
        public Gs2.Gs2Stamina.Model.RecoverIntervalTable RecoverIntervalTable { set; get; }
        public Gs2.Gs2Stamina.Model.RecoverValueTable RecoverValueTable { set; get; }
        public StaminaModel WithStaminaModelId(string staminaModelId) {
            this.StaminaModelId = staminaModelId;
            return this;
        }
        public StaminaModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public StaminaModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public StaminaModel WithRecoverIntervalMinutes(int? recoverIntervalMinutes) {
            this.RecoverIntervalMinutes = recoverIntervalMinutes;
            return this;
        }
        public StaminaModel WithRecoverValue(int? recoverValue) {
            this.RecoverValue = recoverValue;
            return this;
        }
        public StaminaModel WithInitialCapacity(int? initialCapacity) {
            this.InitialCapacity = initialCapacity;
            return this;
        }
        public StaminaModel WithIsOverflow(bool? isOverflow) {
            this.IsOverflow = isOverflow;
            return this;
        }
        public StaminaModel WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }
        public StaminaModel WithMaxStaminaTable(Gs2.Gs2Stamina.Model.MaxStaminaTable maxStaminaTable) {
            this.MaxStaminaTable = maxStaminaTable;
            return this;
        }
        public StaminaModel WithRecoverIntervalTable(Gs2.Gs2Stamina.Model.RecoverIntervalTable recoverIntervalTable) {
            this.RecoverIntervalTable = recoverIntervalTable;
            return this;
        }
        public StaminaModel WithRecoverValueTable(Gs2.Gs2Stamina.Model.RecoverValueTable recoverValueTable) {
            this.RecoverValueTable = recoverValueTable;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):model:(?<staminaName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):model:(?<staminaName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):model:(?<staminaName>.+)",
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

        private static System.Text.RegularExpressions.Regex _staminaNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stamina:(?<namespaceName>.+):model:(?<staminaName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStaminaNameFromGrn(
            string grn
        )
        {
            var match = _staminaNameRegex.Match(grn);
            if (!match.Success || !match.Groups["staminaName"].Success)
            {
                return null;
            }
            return match.Groups["staminaName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StaminaModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StaminaModel()
                .WithStaminaModelId(!data.Keys.Contains("staminaModelId") || data["staminaModelId"] == null ? null : data["staminaModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["recoverIntervalMinutes"].ToString()))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["recoverValue"].ToString()))
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["initialCapacity"].ToString()))
                .WithIsOverflow(!data.Keys.Contains("isOverflow") || data["isOverflow"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableBool(data["isOverflow"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["maxCapacity"].ToString()))
                .WithMaxStaminaTable(!data.Keys.Contains("maxStaminaTable") || data["maxStaminaTable"] == null ? null : Gs2.Gs2Stamina.Model.MaxStaminaTable.FromJson(data["maxStaminaTable"]))
                .WithRecoverIntervalTable(!data.Keys.Contains("recoverIntervalTable") || data["recoverIntervalTable"] == null ? null : Gs2.Gs2Stamina.Model.RecoverIntervalTable.FromJson(data["recoverIntervalTable"]))
                .WithRecoverValueTable(!data.Keys.Contains("recoverValueTable") || data["recoverValueTable"] == null ? null : Gs2.Gs2Stamina.Model.RecoverValueTable.FromJson(data["recoverValueTable"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["staminaModelId"] = StaminaModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["recoverIntervalMinutes"] = RecoverIntervalMinutes,
                ["recoverValue"] = RecoverValue,
                ["initialCapacity"] = InitialCapacity,
                ["isOverflow"] = IsOverflow,
                ["maxCapacity"] = MaxCapacity,
                ["maxStaminaTable"] = MaxStaminaTable?.ToJson(),
                ["recoverIntervalTable"] = RecoverIntervalTable?.ToJson(),
                ["recoverValueTable"] = RecoverValueTable?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StaminaModelId != null) {
                writer.WritePropertyName("staminaModelId");
                writer.Write(StaminaModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RecoverIntervalMinutes != null) {
                writer.WritePropertyName("recoverIntervalMinutes");
                writer.Write((RecoverIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(RecoverIntervalMinutes.ToString()) : int.Parse(RecoverIntervalMinutes.ToString())));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write((RecoverValue.ToString().Contains(".") ? (int)double.Parse(RecoverValue.ToString()) : int.Parse(RecoverValue.ToString())));
            }
            if (InitialCapacity != null) {
                writer.WritePropertyName("initialCapacity");
                writer.Write((InitialCapacity.ToString().Contains(".") ? (int)double.Parse(InitialCapacity.ToString()) : int.Parse(InitialCapacity.ToString())));
            }
            if (IsOverflow != null) {
                writer.WritePropertyName("isOverflow");
                writer.Write(bool.Parse(IsOverflow.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write((MaxCapacity.ToString().Contains(".") ? (int)double.Parse(MaxCapacity.ToString()) : int.Parse(MaxCapacity.ToString())));
            }
            if (MaxStaminaTable != null) {
                writer.WritePropertyName("maxStaminaTable");
                MaxStaminaTable.WriteJson(writer);
            }
            if (RecoverIntervalTable != null) {
                writer.WritePropertyName("recoverIntervalTable");
                RecoverIntervalTable.WriteJson(writer);
            }
            if (RecoverValueTable != null) {
                writer.WritePropertyName("recoverValueTable");
                RecoverValueTable.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StaminaModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type StaminaModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(StaminaModelId, other.StaminaModelId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RecoverIntervalMinutes, other.RecoverIntervalMinutes);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RecoverValue, other.RecoverValue);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(InitialCapacity, other.InitialCapacity);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(IsOverflow, other.IsOverflow);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaxCapacity, other.MaxCapacity);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(MaxStaminaTable, other.MaxStaminaTable);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RecoverIntervalTable, other.RecoverIntervalTable);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RecoverValueTable, other.RecoverValueTable);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (StaminaModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.staminaModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (RecoverIntervalMinutes < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.recoverIntervalMinutes.error.invalid"),
                    });
                }
                if (RecoverIntervalMinutes > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.recoverIntervalMinutes.error.invalid"),
                    });
                }
            }
            {
                if (RecoverValue < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.recoverValue.error.invalid"),
                    });
                }
                if (RecoverValue > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.recoverValue.error.invalid"),
                    });
                }
            }
            {
                if (InitialCapacity < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.initialCapacity.error.invalid"),
                    });
                }
                if (InitialCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.initialCapacity.error.invalid"),
                    });
                }
            }
            {
            }
/* diff --- start
            if (IsOverflow == true) {
 diff --- end */
            if (IsOverflow ?? false) { /* diff +++ */
                if (MaxCapacity < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.maxCapacity.error.invalid"),
                    });
                }
                if (MaxCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("staminaModel", "stamina.staminaModel.maxCapacity.error.invalid"),
                    });
                }
            }
            {
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new StaminaModel {
                StaminaModelId = StaminaModelId,
                Name = Name,
                Metadata = Metadata,
                RecoverIntervalMinutes = RecoverIntervalMinutes,
                RecoverValue = RecoverValue,
                InitialCapacity = InitialCapacity,
                IsOverflow = IsOverflow,
                MaxCapacity = MaxCapacity,
                MaxStaminaTable = MaxStaminaTable?.Clone() as Gs2.Gs2Stamina.Model.MaxStaminaTable,
                RecoverIntervalTable = RecoverIntervalTable?.Clone() as Gs2.Gs2Stamina.Model.RecoverIntervalTable,
                RecoverValueTable = RecoverValueTable?.Clone() as Gs2.Gs2Stamina.Model.RecoverValueTable,
            };
        }
    }
}