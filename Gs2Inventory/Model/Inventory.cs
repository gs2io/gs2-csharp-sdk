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
	public class Inventory : IComparable
	{
        public string InventoryId { set; get; }
        public string InventoryName { set; get; }
        public string UserId { set; get; }
        public int? CurrentInventoryCapacityUsage { set; get; }
        public int? CurrentInventoryMaxCapacity { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Inventory WithInventoryId(string inventoryId) {
            this.InventoryId = inventoryId;
            return this;
        }

        public Inventory WithInventoryName(string inventoryName) {
            this.InventoryName = inventoryName;
            return this;
        }

        public Inventory WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Inventory WithCurrentInventoryCapacityUsage(int? currentInventoryCapacityUsage) {
            this.CurrentInventoryCapacityUsage = currentInventoryCapacityUsage;
            return this;
        }

        public Inventory WithCurrentInventoryMaxCapacity(int? currentInventoryMaxCapacity) {
            this.CurrentInventoryMaxCapacity = currentInventoryMaxCapacity;
            return this;
        }

        public Inventory WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Inventory WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Inventory FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Inventory()
                .WithInventoryId(!data.Keys.Contains("inventoryId") || data["inventoryId"] == null ? null : data["inventoryId"].ToString())
                .WithInventoryName(!data.Keys.Contains("inventoryName") || data["inventoryName"] == null ? null : data["inventoryName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCurrentInventoryCapacityUsage(!data.Keys.Contains("currentInventoryCapacityUsage") || data["currentInventoryCapacityUsage"] == null ? null : (int?)int.Parse(data["currentInventoryCapacityUsage"].ToString()))
                .WithCurrentInventoryMaxCapacity(!data.Keys.Contains("currentInventoryMaxCapacity") || data["currentInventoryMaxCapacity"] == null ? null : (int?)int.Parse(data["currentInventoryMaxCapacity"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["inventoryId"] = InventoryId,
                ["inventoryName"] = InventoryName,
                ["userId"] = UserId,
                ["currentInventoryCapacityUsage"] = CurrentInventoryCapacityUsage,
                ["currentInventoryMaxCapacity"] = CurrentInventoryMaxCapacity,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (InventoryId != null) {
                writer.WritePropertyName("inventoryId");
                writer.Write(InventoryId.ToString());
            }
            if (InventoryName != null) {
                writer.WritePropertyName("inventoryName");
                writer.Write(InventoryName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (CurrentInventoryCapacityUsage != null) {
                writer.WritePropertyName("currentInventoryCapacityUsage");
                writer.Write(int.Parse(CurrentInventoryCapacityUsage.ToString()));
            }
            if (CurrentInventoryMaxCapacity != null) {
                writer.WritePropertyName("currentInventoryMaxCapacity");
                writer.Write(int.Parse(CurrentInventoryMaxCapacity.ToString()));
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
            var other = obj as Inventory;
            var diff = 0;
            if (InventoryId == null && InventoryId == other.InventoryId)
            {
                // null and null
            }
            else
            {
                diff += InventoryId.CompareTo(other.InventoryId);
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
            if (CurrentInventoryCapacityUsage == null && CurrentInventoryCapacityUsage == other.CurrentInventoryCapacityUsage)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentInventoryCapacityUsage - other.CurrentInventoryCapacityUsage);
            }
            if (CurrentInventoryMaxCapacity == null && CurrentInventoryMaxCapacity == other.CurrentInventoryMaxCapacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(CurrentInventoryMaxCapacity - other.CurrentInventoryMaxCapacity);
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