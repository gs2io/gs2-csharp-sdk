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

namespace Gs2.Gs2Inventory.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateItemModelMasterRequest : Gs2Request<UpdateItemModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string InventoryName { set; get; } = null!;
         public string ItemName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public long? StackingLimit { set; get; } = null!;
         public bool? AllowMultipleStacks { set; get; } = null!;
         public int? SortValue { set; get; } = null!;
        public UpdateItemModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateItemModelMasterRequest WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }
        public UpdateItemModelMasterRequest WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }
        public UpdateItemModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateItemModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateItemModelMasterRequest WithStackingLimit(long? stackingLimit) {
            this.StackingLimit = stackingLimit;
            return this;
        }
        public UpdateItemModelMasterRequest WithAllowMultipleStacks(bool? allowMultipleStacks) {
            this.AllowMultipleStacks = allowMultipleStacks;
            return this;
        }
        public UpdateItemModelMasterRequest WithSortValue(int? sortValue) {
            this.SortValue = sortValue;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateItemModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateItemModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithStackingLimit(!data.Keys.Contains("stackingLimit") || data["stackingLimit"] == null ? null : (long?)(data["stackingLimit"].ToString().Contains(".") ? (long)double.Parse(data["stackingLimit"].ToString()) : long.Parse(data["stackingLimit"].ToString())))
                .WithAllowMultipleStacks(!data.Keys.Contains("allowMultipleStacks") || data["allowMultipleStacks"] == null ? null : (bool?)bool.Parse(data["allowMultipleStacks"].ToString()))
                .WithSortValue(!data.Keys.Contains("sortValue") || data["sortValue"] == null ? null : (int?)(data["sortValue"].ToString().Contains(".") ? (int)double.Parse(data["sortValue"].ToString()) : int.Parse(data["sortValue"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["inventoryName"] = InventoryName,
                ["itemName"] = ItemName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["stackingLimit"] = StackingLimit,
                ["allowMultipleStacks"] = AllowMultipleStacks,
                ["sortValue"] = SortValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (StackingLimit != null) {
                writer.WritePropertyName("stackingLimit");
                writer.Write((StackingLimit.ToString().Contains(".") ? (long)double.Parse(StackingLimit.ToString()) : long.Parse(StackingLimit.ToString())));
            }
            if (AllowMultipleStacks != null) {
                writer.WritePropertyName("allowMultipleStacks");
                writer.Write(bool.Parse(AllowMultipleStacks.ToString()));
            }
            if (SortValue != null) {
                writer.WritePropertyName("sortValue");
                writer.Write((SortValue.ToString().Contains(".") ? (int)double.Parse(SortValue.ToString()) : int.Parse(SortValue.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += InventoryName + ":";
            key += ItemName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += StackingLimit + ":";
            key += AllowMultipleStacks + ":";
            key += SortValue + ":";
            return key;
        }
    }
}