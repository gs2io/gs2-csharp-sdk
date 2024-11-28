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
using Gs2.Gs2Inventory.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inventory.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AcquireItemSetByStampSheetResult : IResult
	{
        public Gs2.Gs2Inventory.Model.ItemSet[] Items { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ItemModel ItemModel { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.Inventory Inventory { set; get; } = null!;
        public long? OverflowCount { set; get; } = null!;

        public AcquireItemSetByStampSheetResult WithItems(Gs2.Gs2Inventory.Model.ItemSet[] items) {
            this.Items = items;
            return this;
        }

        public AcquireItemSetByStampSheetResult WithItemModel(Gs2.Gs2Inventory.Model.ItemModel itemModel) {
            this.ItemModel = itemModel;
            return this;
        }

        public AcquireItemSetByStampSheetResult WithInventory(Gs2.Gs2Inventory.Model.Inventory inventory) {
            this.Inventory = inventory;
            return this;
        }

        public AcquireItemSetByStampSheetResult WithOverflowCount(long? overflowCount) {
            this.OverflowCount = overflowCount;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireItemSetByStampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireItemSetByStampSheetResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null || !data["items"].IsArray ? null : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Inventory.Model.ItemSet.FromJson(v);
                }).ToArray())
                .WithItemModel(!data.Keys.Contains("itemModel") || data["itemModel"] == null ? null : Gs2.Gs2Inventory.Model.ItemModel.FromJson(data["itemModel"]))
                .WithInventory(!data.Keys.Contains("inventory") || data["inventory"] == null ? null : Gs2.Gs2Inventory.Model.Inventory.FromJson(data["inventory"]))
                .WithOverflowCount(!data.Keys.Contains("overflowCount") || data["overflowCount"] == null ? null : (long?)(data["overflowCount"].ToString().Contains(".") ? (long)double.Parse(data["overflowCount"].ToString()) : long.Parse(data["overflowCount"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData itemsJsonData = null;
            if (Items != null && Items.Length > 0)
            {
                itemsJsonData = new JsonData();
                foreach (var item in Items)
                {
                    itemsJsonData.Add(item.ToJson());
                }
            }
            return new JsonData {
                ["items"] = itemsJsonData,
                ["itemModel"] = ItemModel?.ToJson(),
                ["inventory"] = Inventory?.ToJson(),
                ["overflowCount"] = OverflowCount,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Items != null) {
                writer.WritePropertyName("items");
                writer.WriteArrayStart();
                foreach (var item in Items)
                {
                    if (item != null) {
                        item.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ItemModel != null) {
                ItemModel.WriteJson(writer);
            }
            if (Inventory != null) {
                Inventory.WriteJson(writer);
            }
            if (OverflowCount != null) {
                writer.WritePropertyName("overflowCount");
                writer.Write((OverflowCount.ToString().Contains(".") ? (long)double.Parse(OverflowCount.ToString()) : long.Parse(OverflowCount.ToString())));
            }
            writer.WriteObjectEnd();
        }
    }
}