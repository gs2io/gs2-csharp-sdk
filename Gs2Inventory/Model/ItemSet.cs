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
	public class ItemSet : IComparable
	{
        public string ItemSetId { set; get; }
        public string Name { set; get; }
        public string InventoryName { set; get; }
        public string UserId { set; get; }
        public string ItemName { set; get; }
        public long? Count { set; get; }
        public string[] ReferenceOf { set; get; }
        public int? SortValue { set; get; }
        public long? ExpiresAt { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public ItemSet WithItemSetId(string itemSetId) {
            this.ItemSetId = itemSetId;
            return this;
        }

        public ItemSet WithName(string name) {
            this.Name = name;
            return this;
        }

        public ItemSet WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }

        public ItemSet WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public ItemSet WithItemName(string itemName) {
            this.ItemName = itemName;
            return this;
        }

        public ItemSet WithCount(long? count) {
            this.Count = count;
            return this;
        }

        public ItemSet WithReferenceOf(string[] referenceOf) {
            this.ReferenceOf = referenceOf;
            return this;
        }

        public ItemSet WithSortValue(int? sortValue) {
            this.SortValue = sortValue;
            return this;
        }

        public ItemSet WithExpiresAt(long? expiresAt) {
            this.ExpiresAt = expiresAt;
            return this;
        }

        public ItemSet WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public ItemSet WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

    	[Preserve]
        public static ItemSet FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ItemSet()
                .WithItemSetId(!data.Keys.Contains("itemSetId") || data["itemSetId"] == null ? null : data["itemSetId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithItemName(!data.Keys.Contains("itemName") || data["itemName"] == null ? null : data["itemName"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (long?)long.Parse(data["count"].ToString()))
                .WithReferenceOf(!data.Keys.Contains("referenceOf") || data["referenceOf"] == null ? new string[]{} : data["referenceOf"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSortValue(!data.Keys.Contains("sortValue") || data["sortValue"] == null ? null : (int?)int.Parse(data["sortValue"].ToString()))
                .WithExpiresAt(!data.Keys.Contains("expiresAt") || data["expiresAt"] == null ? null : (long?)long.Parse(data["expiresAt"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["itemSetId"] = ItemSetId,
                ["name"] = Name,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["itemName"] = ItemName,
                ["count"] = Count,
                ["referenceOf"] = new JsonData(ReferenceOf == null ? new JsonData[]{} :
                        ReferenceOf.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["sortValue"] = SortValue,
                ["expiresAt"] = ExpiresAt,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ItemSetId != null) {
                writer.WritePropertyName("itemSetId");
                writer.Write(ItemSetId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ItemName != null) {
                writer.WritePropertyName("itemName");
                writer.Write(ItemName.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(long.Parse(Count.ToString()));
            }
            if (ReferenceOf != null) {
                writer.WritePropertyName("referenceOf");
                writer.WriteArrayStart();
                foreach (var referenceO in ReferenceOf)
                {
                    if (referenceO != null) {
                        writer.Write(referenceO.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SortValue != null) {
                writer.WritePropertyName("sortValue");
                writer.Write(int.Parse(SortValue.ToString()));
            }
            if (ExpiresAt != null) {
                writer.WritePropertyName("expiresAt");
                writer.Write(long.Parse(ExpiresAt.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ItemSet;
            var diff = 0;
            if (ItemSetId == null && ItemSetId == other.ItemSetId)
            {
                // null and null
            }
            else
            {
                diff += ItemSetId.CompareTo(other.ItemSetId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (InventoryName == null && InventoryName == other.InventoryName)
            {
                // null and null
            }
            else
            {
                diff += InventoryName.CompareTo(other.InventoryName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ItemName == null && ItemName == other.ItemName)
            {
                // null and null
            }
            else
            {
                diff += ItemName.CompareTo(other.ItemName);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (ReferenceOf == null && ReferenceOf == other.ReferenceOf)
            {
                // null and null
            }
            else
            {
                diff += ReferenceOf.Length - other.ReferenceOf.Length;
                for (var i = 0; i < ReferenceOf.Length; i++)
                {
                    diff += ReferenceOf[i].CompareTo(other.ReferenceOf[i]);
                }
            }
            if (SortValue == null && SortValue == other.SortValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(SortValue - other.SortValue);
            }
            if (ExpiresAt == null && ExpiresAt == other.ExpiresAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExpiresAt - other.ExpiresAt);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}