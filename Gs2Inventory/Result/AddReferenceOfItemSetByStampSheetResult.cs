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
	public class AddReferenceOfItemSetByStampSheetResult : IResult
	{
        public string Item { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ItemSet ItemSet { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.ItemModel ItemModel { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.Inventory Inventory { set; get; } = null!;

        public AddReferenceOfItemSetByStampSheetResult WithItem(string item) {
            this.Item = item;
            return this;
        }

        public AddReferenceOfItemSetByStampSheetResult WithItemSet(Gs2.Gs2Inventory.Model.ItemSet itemSet) {
            this.ItemSet = itemSet;
            return this;
        }

        public AddReferenceOfItemSetByStampSheetResult WithItemModel(Gs2.Gs2Inventory.Model.ItemModel itemModel) {
            this.ItemModel = itemModel;
            return this;
        }

        public AddReferenceOfItemSetByStampSheetResult WithInventory(Gs2.Gs2Inventory.Model.Inventory inventory) {
            this.Inventory = inventory;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AddReferenceOfItemSetByStampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AddReferenceOfItemSetByStampSheetResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : data["item"].ToString())
                .WithItemSet(!data.Keys.Contains("itemSet") || data["itemSet"] == null ? null : Gs2.Gs2Inventory.Model.ItemSet.FromJson(data["itemSet"]))
                .WithItemModel(!data.Keys.Contains("itemModel") || data["itemModel"] == null ? null : Gs2.Gs2Inventory.Model.ItemModel.FromJson(data["itemModel"]))
                .WithInventory(!data.Keys.Contains("inventory") || data["inventory"] == null ? null : Gs2.Gs2Inventory.Model.Inventory.FromJson(data["inventory"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item,
                ["itemSet"] = ItemSet?.ToJson(),
                ["itemModel"] = ItemModel?.ToJson(),
                ["inventory"] = Inventory?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                writer.WritePropertyName("item");
                writer.Write(Item.ToString());
            }
            if (ItemSet != null) {
                ItemSet.WriteJson(writer);
            }
            if (ItemModel != null) {
                ItemModel.WriteJson(writer);
            }
            if (Inventory != null) {
                Inventory.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}