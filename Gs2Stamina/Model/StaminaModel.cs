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

namespace Gs2.Gs2Stamina.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StaminaModel : IComparable
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
                .WithRecoverIntervalMinutes(!data.Keys.Contains("recoverIntervalMinutes") || data["recoverIntervalMinutes"] == null ? null : (int?)int.Parse(data["recoverIntervalMinutes"].ToString()))
                .WithRecoverValue(!data.Keys.Contains("recoverValue") || data["recoverValue"] == null ? null : (int?)int.Parse(data["recoverValue"].ToString()))
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : (int?)int.Parse(data["initialCapacity"].ToString()))
                .WithIsOverflow(!data.Keys.Contains("isOverflow") || data["isOverflow"] == null ? null : (bool?)bool.Parse(data["isOverflow"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)int.Parse(data["maxCapacity"].ToString()))
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
                writer.Write(int.Parse(RecoverIntervalMinutes.ToString()));
            }
            if (RecoverValue != null) {
                writer.WritePropertyName("recoverValue");
                writer.Write(int.Parse(RecoverValue.ToString()));
            }
            if (InitialCapacity != null) {
                writer.WritePropertyName("initialCapacity");
                writer.Write(int.Parse(InitialCapacity.ToString()));
            }
            if (IsOverflow != null) {
                writer.WritePropertyName("isOverflow");
                writer.Write(bool.Parse(IsOverflow.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write(int.Parse(MaxCapacity.ToString()));
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
            var diff = 0;
            if (StaminaModelId == null && StaminaModelId == other.StaminaModelId)
            {
                // null and null
            }
            else
            {
                diff += StaminaModelId.CompareTo(other.StaminaModelId);
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
            if (RecoverIntervalMinutes == null && RecoverIntervalMinutes == other.RecoverIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(RecoverIntervalMinutes - other.RecoverIntervalMinutes);
            }
            if (RecoverValue == null && RecoverValue == other.RecoverValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(RecoverValue - other.RecoverValue);
            }
            if (InitialCapacity == null && InitialCapacity == other.InitialCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(InitialCapacity - other.InitialCapacity);
            }
            if (IsOverflow == null && IsOverflow == other.IsOverflow)
            {
                // null and null
            }
            else
            {
                diff += IsOverflow == other.IsOverflow ? 0 : 1;
            }
            if (MaxCapacity == null && MaxCapacity == other.MaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxCapacity - other.MaxCapacity);
            }
            if (MaxStaminaTable == null && MaxStaminaTable == other.MaxStaminaTable)
            {
                // null and null
            }
            else
            {
                diff += MaxStaminaTable.CompareTo(other.MaxStaminaTable);
            }
            if (RecoverIntervalTable == null && RecoverIntervalTable == other.RecoverIntervalTable)
            {
                // null and null
            }
            else
            {
                diff += RecoverIntervalTable.CompareTo(other.RecoverIntervalTable);
            }
            if (RecoverValueTable == null && RecoverValueTable == other.RecoverValueTable)
            {
                // null and null
            }
            else
            {
                diff += RecoverValueTable.CompareTo(other.RecoverValueTable);
            }
            return diff;
        }
    }
}