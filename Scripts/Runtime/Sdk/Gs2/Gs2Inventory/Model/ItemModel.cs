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

        /** アイテムモデルマスター */
        public string itemModelId { set; get; }

        /**
         * アイテムモデルマスターを設定
         *
         * @param itemModelId アイテムモデルマスター
         * @return this
         */
        public ItemModel WithItemModelId(string itemModelId) {
            this.itemModelId = itemModelId;
            return this;
        }

        /** アイテムモデルの種類名 */
        public string name { set; get; }

        /**
         * アイテムモデルの種類名を設定
         *
         * @param name アイテムモデルの種類名
         * @return this
         */
        public ItemModel WithName(string name) {
            this.name = name;
            return this;
        }

        /** アイテムモデルの種類のメタデータ */
        public string metadata { set; get; }

        /**
         * アイテムモデルの種類のメタデータを設定
         *
         * @param metadata アイテムモデルの種類のメタデータ
         * @return this
         */
        public ItemModel WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }

        /** スタック可能な最大数量 */
        public long? stackingLimit { set; get; }

        /**
         * スタック可能な最大数量を設定
         *
         * @param stackingLimit スタック可能な最大数量
         * @return this
         */
        public ItemModel WithStackingLimit(long? stackingLimit) {
            this.stackingLimit = stackingLimit;
            return this;
        }

        /** スタック可能な最大数量を超えた時複数枠にアイテムを保管することを許すか */
        public bool? allowMultipleStacks { set; get; }

        /**
         * スタック可能な最大数量を超えた時複数枠にアイテムを保管することを許すかを設定
         *
         * @param allowMultipleStacks スタック可能な最大数量を超えた時複数枠にアイテムを保管することを許すか
         * @return this
         */
        public ItemModel WithAllowMultipleStacks(bool? allowMultipleStacks) {
            this.allowMultipleStacks = allowMultipleStacks;
            return this;
        }

        /** 表示順番 */
        public int? sortValue { set; get; }

        /**
         * 表示順番を設定
         *
         * @param sortValue 表示順番
         * @return this
         */
        public ItemModel WithSortValue(int? sortValue) {
            this.sortValue = sortValue;
            return this;
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if(this.itemModelId != null)
            {
                writer.WritePropertyName("itemModelId");
                writer.Write(this.itemModelId);
            }
            if(this.name != null)
            {
                writer.WritePropertyName("name");
                writer.Write(this.name);
            }
            if(this.metadata != null)
            {
                writer.WritePropertyName("metadata");
                writer.Write(this.metadata);
            }
            if(this.stackingLimit.HasValue)
            {
                writer.WritePropertyName("stackingLimit");
                writer.Write(this.stackingLimit.Value);
            }
            if(this.allowMultipleStacks.HasValue)
            {
                writer.WritePropertyName("allowMultipleStacks");
                writer.Write(this.allowMultipleStacks.Value);
            }
            if(this.sortValue.HasValue)
            {
                writer.WritePropertyName("sortValue");
                writer.Write(this.sortValue.Value);
            }
            writer.WriteObjectEnd();
        }

    public static string GetItemNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):inventory:(?<namespaceName>.*):model:(?<inventoryName>.*):item:(?<itemName>.*)");
        if (!match.Groups["itemName"].Success)
        {
            return null;
        }
        return match.Groups["itemName"].Value;
    }

    public static string GetInventoryNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):inventory:(?<namespaceName>.*):model:(?<inventoryName>.*):item:(?<itemName>.*)");
        if (!match.Groups["inventoryName"].Success)
        {
            return null;
        }
        return match.Groups["inventoryName"].Value;
    }

    public static string GetNamespaceNameFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):inventory:(?<namespaceName>.*):model:(?<inventoryName>.*):item:(?<itemName>.*)");
        if (!match.Groups["namespaceName"].Success)
        {
            return null;
        }
        return match.Groups["namespaceName"].Value;
    }

    public static string GetOwnerIdFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):inventory:(?<namespaceName>.*):model:(?<inventoryName>.*):item:(?<itemName>.*)");
        if (!match.Groups["ownerId"].Success)
        {
            return null;
        }
        return match.Groups["ownerId"].Value;
    }

    public static string GetRegionFromGrn(
        string grn
    )
    {
        var match = Regex.Match(grn, "grn:gs2:(?<region>.*):(?<ownerId>.*):inventory:(?<namespaceName>.*):model:(?<inventoryName>.*):item:(?<itemName>.*)");
        if (!match.Groups["region"].Success)
        {
            return null;
        }
        return match.Groups["region"].Value;
    }

    	[Preserve]
        public static ItemModel FromDict(JsonData data)
        {
            return new ItemModel()
                .WithItemModelId(data.Keys.Contains("itemModelId") && data["itemModelId"] != null ? data["itemModelId"].ToString() : null)
                .WithName(data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString() : null)
                .WithMetadata(data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString() : null)
                .WithStackingLimit(data.Keys.Contains("stackingLimit") && data["stackingLimit"] != null ? (long?)long.Parse(data["stackingLimit"].ToString()) : null)
                .WithAllowMultipleStacks(data.Keys.Contains("allowMultipleStacks") && data["allowMultipleStacks"] != null ? (bool?)bool.Parse(data["allowMultipleStacks"].ToString()) : null)
                .WithSortValue(data.Keys.Contains("sortValue") && data["sortValue"] != null ? (int?)int.Parse(data["sortValue"].ToString()) : null);
        }

        public int CompareTo(object obj)
        {
            var other = obj as ItemModel;
            var diff = 0;
            if (itemModelId == null && itemModelId == other.itemModelId)
            {
                // null and null
            }
            else
            {
                diff += itemModelId.CompareTo(other.itemModelId);
            }
            if (name == null && name == other.name)
            {
                // null and null
            }
            else
            {
                diff += name.CompareTo(other.name);
            }
            if (metadata == null && metadata == other.metadata)
            {
                // null and null
            }
            else
            {
                diff += metadata.CompareTo(other.metadata);
            }
            if (stackingLimit == null && stackingLimit == other.stackingLimit)
            {
                // null and null
            }
            else
            {
                diff += (int)(stackingLimit - other.stackingLimit);
            }
            if (allowMultipleStacks == null && allowMultipleStacks == other.allowMultipleStacks)
            {
                // null and null
            }
            else
            {
                diff += allowMultipleStacks == other.allowMultipleStacks ? 0 : 1;
            }
            if (sortValue == null && sortValue == other.sortValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(sortValue - other.sortValue);
            }
            return diff;
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["itemModelId"] = itemModelId;
            data["name"] = name;
            data["metadata"] = metadata;
            data["stackingLimit"] = stackingLimit;
            data["allowMultipleStacks"] = allowMultipleStacks;
            data["sortValue"] = sortValue;
            return data;
        }
	}
}