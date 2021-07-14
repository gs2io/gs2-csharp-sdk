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
	public class ItemModel : IComparable
	{
        public string ItemModelId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public long? StackingLimit { set; get; }
        public bool? AllowMultipleStacks { set; get; }
        public int? SortValue { set; get; }

        public ItemModel WithItemModelId(string itemModelId) {
            this.ItemModelId = itemModelId;
            return this;
        }

        public ItemModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public ItemModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public ItemModel WithStackingLimit(long? stackingLimit) {
            this.StackingLimit = stackingLimit;
            return this;
        }

        public ItemModel WithAllowMultipleStacks(bool? allowMultipleStacks) {
            this.AllowMultipleStacks = allowMultipleStacks;
            return this;
        }

        public ItemModel WithSortValue(int? sortValue) {
            this.SortValue = sortValue;
            return this;
        }

    	[Preserve]
        public static ItemModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ItemModel()
                .WithItemModelId(!data.Keys.Contains("itemModelId") || data["itemModelId"] == null ? null : data["itemModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithStackingLimit(!data.Keys.Contains("stackingLimit") || data["stackingLimit"] == null ? null : (long?)long.Parse(data["stackingLimit"].ToString()))
                .WithAllowMultipleStacks(!data.Keys.Contains("allowMultipleStacks") || data["allowMultipleStacks"] == null ? null : (bool?)bool.Parse(data["allowMultipleStacks"].ToString()))
                .WithSortValue(!data.Keys.Contains("sortValue") || data["sortValue"] == null ? null : (int?)int.Parse(data["sortValue"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["itemModelId"] = ItemModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["stackingLimit"] = StackingLimit,
                ["allowMultipleStacks"] = AllowMultipleStacks,
                ["sortValue"] = SortValue,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ItemModelId != null) {
                writer.WritePropertyName("itemModelId");
                writer.Write(ItemModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (StackingLimit != null) {
                writer.WritePropertyName("stackingLimit");
                writer.Write(long.Parse(StackingLimit.ToString()));
            }
            if (AllowMultipleStacks != null) {
                writer.WritePropertyName("allowMultipleStacks");
                writer.Write(bool.Parse(AllowMultipleStacks.ToString()));
            }
            if (SortValue != null) {
                writer.WritePropertyName("sortValue");
                writer.Write(int.Parse(SortValue.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ItemModel;
            var diff = 0;
            if (ItemModelId == null && ItemModelId == other.ItemModelId)
            {
                // null and null
            }
            else
            {
                diff += ItemModelId.CompareTo(other.ItemModelId);
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
            if (StackingLimit == null && StackingLimit == other.StackingLimit)
            {
                // null and null
            }
            else
            {
                diff += (int)(StackingLimit - other.StackingLimit);
            }
            if (AllowMultipleStacks == null && AllowMultipleStacks == other.AllowMultipleStacks)
            {
                // null and null
            }
            else
            {
                diff += AllowMultipleStacks == other.AllowMultipleStacks ? 0 : 1;
            }
            if (SortValue == null && SortValue == other.SortValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(SortValue - other.SortValue);
            }
            return diff;
        }
    }
}