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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class InventoryModel : IComparable
	{
        public string InventoryModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? InitialCapacity { set; get; } = null!;
        public int? MaxCapacity { set; get; } = null!;
        public bool? ProtectReferencedItem { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ItemModel[] ItemModels { set; get; } = null!;
        public InventoryModel WithInventoryModelId(string inventoryModelId) {
            this.InventoryModelId = inventoryModelId;
            return this;
        }
        public InventoryModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public InventoryModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public InventoryModel WithInitialCapacity(int? initialCapacity) {
            this.InitialCapacity = initialCapacity;
            return this;
        }
        public InventoryModel WithMaxCapacity(int? maxCapacity) {
            this.MaxCapacity = maxCapacity;
            return this;
        }
        public InventoryModel WithProtectReferencedItem(bool? protectReferencedItem) {
            this.ProtectReferencedItem = protectReferencedItem;
            return this;
        }
        public InventoryModel WithItemModels(Gs2.Gs2Inventory.Model.ItemModel[] itemModels) {
            this.ItemModels = itemModels;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+)",
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

        private static System.Text.RegularExpressions.Regex _inventoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):model:(?<inventoryName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetInventoryNameFromGrn(
            string grn
        )
        {
            var match = _inventoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["inventoryName"].Success)
            {
                return null;
            }
            return match.Groups["inventoryName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static InventoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new InventoryModel()
                .WithInventoryModelId(!data.Keys.Contains("inventoryModelId") || data["inventoryModelId"] == null ? null : data["inventoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : (int?)(data["initialCapacity"].ToString().Contains(".") ? (int)double.Parse(data["initialCapacity"].ToString()) : int.Parse(data["initialCapacity"].ToString())))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)(data["maxCapacity"].ToString().Contains(".") ? (int)double.Parse(data["maxCapacity"].ToString()) : int.Parse(data["maxCapacity"].ToString())))
                .WithProtectReferencedItem(!data.Keys.Contains("protectReferencedItem") || data["protectReferencedItem"] == null ? null : (bool?)bool.Parse(data["protectReferencedItem"].ToString()))
                .WithItemModels(!data.Keys.Contains("itemModels") || data["itemModels"] == null || !data["itemModels"].IsArray ? null : data["itemModels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.ItemModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData itemModelsJsonData = null;
            if (ItemModels != null && ItemModels.Length > 0)
            {
                itemModelsJsonData = new JsonData();
                foreach (var itemModel in ItemModels)
                {
                    itemModelsJsonData.Add(itemModel.ToJson());
                }
            }
            return new JsonData {
                ["inventoryModelId"] = InventoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["initialCapacity"] = InitialCapacity,
                ["maxCapacity"] = MaxCapacity,
                ["protectReferencedItem"] = ProtectReferencedItem,
                ["itemModels"] = itemModelsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InventoryModelId != null) {
                writer.WritePropertyName("inventoryModelId");
                writer.Write(InventoryModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InitialCapacity != null) {
                writer.WritePropertyName("initialCapacity");
                writer.Write((InitialCapacity.ToString().Contains(".") ? (int)double.Parse(InitialCapacity.ToString()) : int.Parse(InitialCapacity.ToString())));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write((MaxCapacity.ToString().Contains(".") ? (int)double.Parse(MaxCapacity.ToString()) : int.Parse(MaxCapacity.ToString())));
            }
            if (ProtectReferencedItem != null) {
                writer.WritePropertyName("protectReferencedItem");
                writer.Write(bool.Parse(ProtectReferencedItem.ToString()));
            }
            if (ItemModels != null) {
                writer.WritePropertyName("itemModels");
                writer.WriteArrayStart();
                foreach (var itemModel in ItemModels)
                {
                    if (itemModel != null) {
                        itemModel.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as InventoryModel;
            var diff = 0;
            if (InventoryModelId == null && InventoryModelId == other.InventoryModelId)
            {
                // null and null
            }
            else
            {
                diff += InventoryModelId.CompareTo(other.InventoryModelId);
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
            if (InitialCapacity == null && InitialCapacity == other.InitialCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(InitialCapacity - other.InitialCapacity);
            }
            if (MaxCapacity == null && MaxCapacity == other.MaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaxCapacity - other.MaxCapacity);
            }
            if (ProtectReferencedItem == null && ProtectReferencedItem == other.ProtectReferencedItem)
            {
                // null and null
            }
            else
            {
                diff += ProtectReferencedItem == other.ProtectReferencedItem ? 0 : 1;
            }
            if (ItemModels == null && ItemModels == other.ItemModels)
            {
                // null and null
            }
            else
            {
                diff += ItemModels.Length - other.ItemModels.Length;
                for (var i = 0; i < ItemModels.Length; i++)
                {
                    diff += ItemModels[i].CompareTo(other.ItemModels[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (InventoryModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.inventoryModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (InitialCapacity < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.initialCapacity.error.invalid"),
                    });
                }
                if (InitialCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.initialCapacity.error.invalid"),
                    });
                }
            }
            {
                if (MaxCapacity < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.maxCapacity.error.invalid"),
                    });
                }
                if (MaxCapacity > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.maxCapacity.error.invalid"),
                    });
                }
            }
            {
            }
            {
                if (ItemModels.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.itemModels.error.tooFew"),
                    });
                }
                if (ItemModels.Length > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("inventoryModel", "inventory.inventoryModel.itemModels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new InventoryModel {
                InventoryModelId = InventoryModelId,
                Name = Name,
                Metadata = Metadata,
                InitialCapacity = InitialCapacity,
                MaxCapacity = MaxCapacity,
                ProtectReferencedItem = ProtectReferencedItem,
                ItemModels = ItemModels.Clone() as Gs2.Gs2Inventory.Model.ItemModel[],
            };
        }
    }
}