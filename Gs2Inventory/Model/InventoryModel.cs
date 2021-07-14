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
using UnityEngine.Scripting;

namespace Gs2.Gs2Inventory.Model
{

	[Preserve]
	public class InventoryModel : IComparable
	{
        public string InventoryModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public int? InitialCapacity { set; get; }
        public int? MaxCapacity { set; get; }
        public bool? ProtectReferencedItem { set; get; }
        public Gs2.Gs2Inventory.Model.ItemModel[] ItemModels { set; get; }

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

    	[Preserve]
        public static InventoryModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new InventoryModel()
                .WithInventoryModelId(!data.Keys.Contains("inventoryModelId") || data["inventoryModelId"] == null ? null : data["inventoryModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialCapacity(!data.Keys.Contains("initialCapacity") || data["initialCapacity"] == null ? null : (int?)int.Parse(data["initialCapacity"].ToString()))
                .WithMaxCapacity(!data.Keys.Contains("maxCapacity") || data["maxCapacity"] == null ? null : (int?)int.Parse(data["maxCapacity"].ToString()))
                .WithProtectReferencedItem(!data.Keys.Contains("protectReferencedItem") || data["protectReferencedItem"] == null ? null : (bool?)bool.Parse(data["protectReferencedItem"].ToString()))
                .WithItemModels(!data.Keys.Contains("itemModels") || data["itemModels"] == null ? new Gs2.Gs2Inventory.Model.ItemModel[]{} : data["itemModels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.ItemModel.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["inventoryModelId"] = InventoryModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["initialCapacity"] = InitialCapacity,
                ["maxCapacity"] = MaxCapacity,
                ["protectReferencedItem"] = ProtectReferencedItem,
                ["itemModels"] = new JsonData(ItemModels == null ? new JsonData[]{} :
                        ItemModels.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
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
                writer.Write(int.Parse(InitialCapacity.ToString()));
            }
            if (MaxCapacity != null) {
                writer.WritePropertyName("maxCapacity");
                writer.Write(int.Parse(MaxCapacity.ToString()));
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
    }
}