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
	public class AcquireItemSetWithGradeByStampSheetResult : IResult
	{
        public Gs2.Gs2Inventory.Model.ItemSet Item { set; get; }
        public Gs2.Gs2Grade.Model.Status Status { set; get; }
        public Gs2.Gs2Inventory.Model.ItemModel ItemModel { set; get; }
        public Gs2.Gs2Inventory.Model.Inventory Inventory { set; get; }
        public long? OverflowCount { set; get; }
        public ResultMetadata Metadata { set; get; }

        public AcquireItemSetWithGradeByStampSheetResult WithItem(Gs2.Gs2Inventory.Model.ItemSet item) {
            this.Item = item;
            return this;
        }

        public AcquireItemSetWithGradeByStampSheetResult WithStatus(Gs2.Gs2Grade.Model.Status status) {
            this.Status = status;
            return this;
        }

        public AcquireItemSetWithGradeByStampSheetResult WithItemModel(Gs2.Gs2Inventory.Model.ItemModel itemModel) {
            this.ItemModel = itemModel;
            return this;
        }

        public AcquireItemSetWithGradeByStampSheetResult WithInventory(Gs2.Gs2Inventory.Model.Inventory inventory) {
            this.Inventory = inventory;
            return this;
        }

        public AcquireItemSetWithGradeByStampSheetResult WithOverflowCount(long? overflowCount) {
            this.OverflowCount = overflowCount;
            return this;
        }

        public AcquireItemSetWithGradeByStampSheetResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcquireItemSetWithGradeByStampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcquireItemSetWithGradeByStampSheetResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Inventory.Model.ItemSet.FromJson(data["item"]))
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : Gs2.Gs2Grade.Model.Status.FromJson(data["status"]))
                .WithItemModel(!data.Keys.Contains("itemModel") || data["itemModel"] == null ? null : Gs2.Gs2Inventory.Model.ItemModel.FromJson(data["itemModel"]))
                .WithInventory(!data.Keys.Contains("inventory") || data["inventory"] == null ? null : Gs2.Gs2Inventory.Model.Inventory.FromJson(data["inventory"]))
                .WithOverflowCount(!data.Keys.Contains("overflowCount") || data["overflowCount"] == null ? null : (long?)(data["overflowCount"].ToString().Contains(".") ? (long)double.Parse(data["overflowCount"].ToString()) : long.Parse(data["overflowCount"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["status"] = Status?.ToJson(),
                ["itemModel"] = ItemModel?.ToJson(),
                ["inventory"] = Inventory?.ToJson(),
                ["overflowCount"] = OverflowCount,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Status != null) {
                Status.WriteJson(writer);
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
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}