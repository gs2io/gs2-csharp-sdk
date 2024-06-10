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
	public class GetSimpleItemResult : IResult
	{
        public Gs2.Gs2Inventory.Model.SimpleItem Item { set; get; } = null!;
        public Gs2.Gs2Inventory.Model.SimpleItemModel ItemModel { set; get; } = null!;

        public GetSimpleItemResult WithItem(Gs2.Gs2Inventory.Model.SimpleItem item) {
            this.Item = item;
            return this;
        }

        public GetSimpleItemResult WithItemModel(Gs2.Gs2Inventory.Model.SimpleItemModel itemModel) {
            this.ItemModel = itemModel;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetSimpleItemResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetSimpleItemResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Inventory.Model.SimpleItem.FromJson(data["item"]))
                .WithItemModel(!data.Keys.Contains("itemModel") || data["itemModel"] == null ? null : Gs2.Gs2Inventory.Model.SimpleItemModel.FromJson(data["itemModel"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["itemModel"] = ItemModel?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (ItemModel != null) {
                ItemModel.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}